using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PrjPriceTableLoanSimulation.Exception.Exceptions;
using PrjPriceTableLoanSimulation.Messaging.DTOs;
using PrjPriceTableLoanSimulation.UseCase.Enums;
using PrjPriceTableLoanSimulation.UseCase.UseCases.DeleteSimulate;
using PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulateById;
using PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulates;
using PrjPriceTableLoanSimulation.UseCase.UseCases.Simulate;
using PrjPriceTableLoanSimulation.UseCase.UseCases.UpdateSimulate;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PrjPriceTableLoanSimulation.Messaging
{
    public class RabbitMqServerService : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private static Dictionary<string, List<ChunkMessageRequest>> _chunksBuffer = new();
        private readonly string _requestQueue;
        private readonly Serilog.ILogger _logger;

        public RabbitMqServerService(IModel channel, IMediator mediator, IServiceProvider serviceProvider, IConfiguration configuration, Serilog.ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
            _channel = channel;
            _mediator = mediator;
            _serviceProvider = serviceProvider;
            _requestQueue = configuration["RabbitMqSettings:RequestQueueName"];
            _channel.QueueDeclare(queue: _requestQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Iniciando o consumo das mensagens...");
            var consumer = InitializerConsumer(_channel, _requestQueue, _logger);
            consumer.Received += async (model, ea) => await HandleMessageAsync(ea, stoppingToken);

            await Task.Delay(-1, stoppingToken);
        }

        private static EventingBasicConsumer InitializerConsumer(IModel channel, string queueName, Serilog.ILogger logger)
        {
            logger.Information("Declarando a fila...");
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            logger.Information("Definindo configurações de consumo...");
            channel.BasicQos(0, 1, false);

            logger.Information("Implementação das configurações de consumo...");
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            return consumer;
        }

        private async Task HandleMessageAsync(BasicDeliverEventArgs ea, CancellationToken stoppingToken)
        {
            _logger.Information("Constroi o objeto chunkMessage...");
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            ChunkMessageRequest? chunkMessage = DeserializeRequestMessage(message);
            if (chunkMessage == null) return;

            _logger.Information("Adicionando a mensagem ao buffer...");
            var correlationId = ea.BasicProperties.CorrelationId;
            if (!_chunksBuffer.ContainsKey(correlationId))
                _chunksBuffer[correlationId] = new List<ChunkMessageRequest>();

            _chunksBuffer[correlationId].Add(chunkMessage);

            _logger.Information("Processando as partes da mensagem...");
            await ProcessChunks(ea, chunkMessage);
        }

        private ChunkMessageRequest? DeserializeRequestMessage(string message)
        {
            try
            {
                return JsonConvert.DeserializeObject<ChunkMessageRequest>(message);
            }
            catch (JsonException ex)
            {
                _logger.Error(ex, $"Erro ao desserializar a mensagem: {ex.Message}");
                return null;
            }
        }

        private async Task ProcessChunks(BasicDeliverEventArgs ea, ChunkMessageRequest chunkMessage)
        {
            var correlationId = ea.BasicProperties.CorrelationId;

            _logger.Information("Verificando se a mensagem chegou ao fim...");
            if (_chunksBuffer[correlationId].Count == chunkMessage.TotalChunks)
            {
                _logger.Information("Preparando a mensagem completa...");
                var fullMessage = string.Concat(_chunksBuffer[correlationId]
                    .OrderBy(chunk => chunk.CurrentChunk)
                    .Select(chunk => Encoding.UTF8.GetString(chunk.Payload)));

                try
                {
                    _logger.Information("Processando a mensagem completa...");
                    await ProcessFullMessage(fullMessage, ea, chunkMessage.RequestType);
                }
                catch (System.Exception ex)
                {
                    _logger.Error(ex, $"Erro ao processar mensagem: {ex.Message}");

                    ChunkMessageResponse response = new()
                    {
                        Payload = Encoding.UTF8.GetBytes(ex.Message),
                        CurrentChunk = 1,
                        TotalChunks = 1,
                        StatusCode = 500
                    };

                    await SendReplyAsync(ea, response, 500);
                }

                _logger.Information("Removendo a mensagem do buffer...");
                _chunksBuffer.Remove(correlationId);
            }

            _logger.Information("Aprova a mensagem...");
            _channel.BasicAck(ea.DeliveryTag, false);
        }

        private async Task ProcessFullMessage(string fullMessage, BasicDeliverEventArgs ea, RequestTypeEnum requestType)
        {
            object? request = null;

            _logger.Information("Verificando o tipo de requisição e Deserializando a mensagem...");
            switch (requestType)
            {
                case RequestTypeEnum.SimulationRequest:
                    request = JsonConvert.DeserializeObject<SimulateRequest>(fullMessage);
                    break;

                case RequestTypeEnum.GetSimulatesRequest:
                    request = JsonConvert.DeserializeObject<GetSimulatesRequest>(fullMessage);
                    break;

                case RequestTypeEnum.GetSimulateByIdRequest:
                    request = JsonConvert.DeserializeObject<GetSimulateByIdRequest>(fullMessage);
                    break;

                case RequestTypeEnum.UpdateSimulateRequest:
                    request = JsonConvert.DeserializeObject<UpdateSimulateRequest>(fullMessage);
                    break;

                case RequestTypeEnum.DeleteSimulateRequest:
                    request = JsonConvert.DeserializeObject<DeleteSimulateRequest>(fullMessage);
                    break;

                default:
                    Console.WriteLine($"Tipo de requisição desconhecida");
                    await SendReplyAsync(ea, $"Tipo de requisição desconhecida", 500);
                    return;
            }

            _logger.Information("Inicialização do escopo...");
            using (var scope = _serviceProvider.CreateScope())
            {
                var response = default(object);

                _logger.Information("Obtendo o mediador...");
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                try
                {
                    _logger.Information("Processando a requisição...");
                    response = await mediator.Send(request);

                    _logger.Information("Enviando a resposta...");
                    await SendReplyAsync(ea, response, 200);
                }
                catch (ConflictException ex)
                {
                    _logger.Error(ex, $"ConflictException: {ex.Message})");

                    _logger.Information("Enviando a resposta...");
                    await SendReplyAsync(ea, ex.Message, 409);

                }
                catch (PreconditionFailedException ex)
                {
                    _logger.Error(ex, $"PreconditionFailedException: {JsonConvert.SerializeObject(ex.BadRequestObjectResult.Value)}");
                       
                    _logger.Information("Enviando a resposta...");
                    await SendReplyAsync(ea, ex.BadRequestObjectResult.Value, 412);
                }
                catch (System.Exception ex)
                {
                    _logger.Error(ex, $"Erro ao processar requisição: {ex.Message})");

                    _logger.Information("Enviando a resposta...");
                    await SendReplyAsync(ea, ex.Message, 500);
                }
            }
        }

        private async Task SendReplyAsync(BasicDeliverEventArgs ea, object response, int statusCode)
        {
            _logger.Information("Criando propriedades da resposta...");
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = ea.BasicProperties.CorrelationId;

            _logger.Information("Serializando a resposta...");
            var replyMessage = JsonConvert.SerializeObject(response);
            var responseBytes = Encoding.UTF8.GetBytes(replyMessage);

            _logger.Information("Dividindo a resposta em chunks...");
            const int chunkSize = 128;
            int totalChunks = (int)Math.Ceiling((double)responseBytes.Length / chunkSize);

            for (int i = 0; i < totalChunks; i++)
            {
                _logger.Information($"Criando chunk {i + 1} de {totalChunks}...");
                int currentChunkSize = Math.Min(chunkSize, responseBytes.Length - (i * chunkSize));
                byte[] chunkData = new byte[currentChunkSize];
                Array.Copy(responseBytes, i * chunkSize, chunkData, 0, currentChunkSize);

                _logger.Information($"Criando o objeto ChunkMessageResponse para o chunk {i + 1} de {totalChunks}...");
                var chunkMessage = new ChunkMessageResponse
                {
                    TotalChunks = totalChunks,
                    CurrentChunk = i + 1,
                    Payload = chunkData,
                    StatusCode = statusCode
                };

                _logger.Information($"Serializando o chunk {i + 1} de {totalChunks}...");
                var chunkMessageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(chunkMessage));

                _logger.Information($"Enviando o chunk {i + 1} de {totalChunks}...");
                _channel.BasicPublish(
                    exchange: "",
                    routingKey: ea.BasicProperties.ReplyTo,
                    basicProperties: replyProps,
                    body: chunkMessageBytes);

                _logger.Information($"Aguardando 10 ms de atraso...");
                await Task.Delay(10);
            }
        }

    }
}