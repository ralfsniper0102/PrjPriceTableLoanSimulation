using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PrjPriceTableLoanSimulation.Domain.Entities;
using PrjPriceTableLoanSimulation.Infrastructure.Context;
using PrjPriceTableLoanSimulation.Infrastructure.Repositories;

namespace PrjPriceTableLoanSimulation.Test
{
    public class GenericRepositoryTestPaymentSchedule : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly GenericRepository<PaymentSchedule, int> _paymentScheduleRepository;

        public GenericRepositoryTestPaymentSchedule()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
            _dbContext = new ApplicationDbContext(options);

            var mockLogger = new Mock<ILogger<GenericRepository<PaymentSchedule, int>>>();
            _paymentScheduleRepository = new GenericRepository<PaymentSchedule, int>(_dbContext, mockLogger.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldAddEntityAndSaveChanges()
        {
            // Arrange
            var entity = new PaymentSchedule
            {
                ProposalId = 1,
                Proposal = new Proposal(),
                Month = 1,
                Principal = 100,
                Interest = 50,
                Balance = 50
            };

            // Act
            var result = await _paymentScheduleRepository.InsertAsync(entity);

            // Assert
            var insertedEntity = await _dbContext.Set<PaymentSchedule>().FindAsync(result.Id);
            Assert.NotNull(insertedEntity);
            Assert.Equal(entity.Principal, insertedEntity.Principal);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEntity()
        {
            // Arrange
            var entity = new PaymentSchedule
            {
                ProposalId = 1,
                Proposal = new Proposal(),
                Month = 1,
                Principal = 100,
                Interest = 50,
                Balance = 50
            };
            _dbContext.Set<PaymentSchedule>().Add(entity);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _paymentScheduleRepository.Queryable(x => x.Id == entity.Id).FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity.Principal, result.Principal);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntityAndSaveChanges()
        {
            // Arrange
            var entity = new PaymentSchedule
            {
                ProposalId = 1,
                Proposal = new Proposal(),
                Month = 1,
                Principal = 100,
                Interest = 50,
                Balance = 50
            };
            _dbContext.Set<PaymentSchedule>().Add(entity);
            await _dbContext.SaveChangesAsync();

            // Act
            entity.Principal = 200;
            var result = await _paymentScheduleRepository.UpdateAsync(entity);

            // Assert
            var updatedEntity = await _dbContext.Set<PaymentSchedule>().FindAsync(result.Id);
            Assert.NotNull(updatedEntity);
            Assert.Equal(entity.Principal, updatedEntity.Principal);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEntityAndSaveChanges()
        {
            // Arrange
            var entity = new PaymentSchedule
            {
                ProposalId = 1,
                Proposal = new Proposal(),
                Month = 1,
                Principal = 100,
                Interest = 50,
                Balance = 50
            };
            _dbContext.Set<PaymentSchedule>().Add(entity);
            await _dbContext.SaveChangesAsync();

            // Act
            await _paymentScheduleRepository.DeleteAsync(entity);

            // Assert
            var deletedEntity = await _dbContext.Set<PaymentSchedule>().FindAsync(entity.Id);
            Assert.Null(deletedEntity);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
