FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["PrjPriceTableLoanSimulation.ProcessingAPI/PrjPriceTableLoanSimulation.ProcessingAPI.csproj", "PrjPriceTableLoanSimulation.ProcessingAPI/"]
COPY ["PrjPriceTableLoanSimulation.Application/PrjPriceTableLoanSimulation.Application.csproj", "PrjPriceTableLoanSimulation.Application/"]
COPY ["PrjPriceTableLoanSimulation.Domain/PrjPriceTableLoanSimulation.Domain.csproj", "PrjPriceTableLoanSimulation.Domain/"]
COPY ["PrjPriceTableLoanSimulation.Exception/PrjPriceTableLoanSimulation.Exception.csproj", "PrjPriceTableLoanSimulation.Exception/"]
COPY ["PrjPriceTableLoanSimulation.UseCase/PrjPriceTableLoanSimulation.UseCase.csproj", "PrjPriceTableLoanSimulation.UseCase/"]
COPY ["PrjPriceTableLoanSimulation.Localization/PrjPriceTableLoanSimulation.Localization.csproj", "PrjPriceTableLoanSimulation.Localization/"]
COPY ["PrjPriceTableLoanSimulation.Composition/PrjPriceTableLoanSimulation.Composition.csproj", "PrjPriceTableLoanSimulation.Composition/"]
COPY ["PrjPriceTableLoanSimulation.Infrastructure/PrjPriceTableLoanSimulation.Infrastructure.csproj", "PrjPriceTableLoanSimulation.Infrastructure/"]
COPY ["PrjPriceTableLoanSimulation.Persistence/PrjPriceTableLoanSimulation.Persistence.csproj", "PrjPriceTableLoanSimulation.Persistence/"]
COPY ["PrjPriceTableLoanSimulation.Messaging/PrjPriceTableLoanSimulation.Messaging.csproj", "PrjPriceTableLoanSimulation.Messaging/"]
RUN dotnet restore "./PrjPriceTableLoanSimulation.ProcessingAPI/PrjPriceTableLoanSimulation.ProcessingAPI.csproj"

COPY . .
WORKDIR "/src/PrjPriceTableLoanSimulation.ProcessingAPI"
RUN dotnet publish "./PrjPriceTableLoanSimulation.ProcessingAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

EXPOSE 5000
WORKDIR /app

COPY --from=build /app/publish . 

ENTRYPOINT ["dotnet", "PrjPriceTableLoanSimulation.ProcessingAPI.dll"]
