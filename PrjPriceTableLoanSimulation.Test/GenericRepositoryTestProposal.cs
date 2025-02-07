using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PrjPriceTableLoanSimulation.Domain.Entities;
using PrjPriceTableLoanSimulation.Infrastructure.Context;
using PrjPriceTableLoanSimulation.Infrastructure.Repositories;

namespace PrjPriceTableLoanSimulation.Test
{
    public class GenericRepositoryTestProposal : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly GenericRepository<Proposal, int> _proposalRepository;

        public GenericRepositoryTestProposal()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
            _dbContext = new ApplicationDbContext(options);

            var mockLogger = new Mock<ILogger<GenericRepository<Proposal, int>>>();
            _proposalRepository = new GenericRepository<Proposal, int>(_dbContext, mockLogger.Object);
        }

        [Fact]
        public async Task InsertAsync_ShouldAddEntityAndSaveChanges()
        {
            // Arrange
            var entity = new Proposal 
            {
                LoanAmount = 1000,
                AnnualInterestRate = 0.01m,
                NumberOfMonths = 12,
                MonthlyPayment = 100,
                TotalInterest = 1200,
                TotalPayment = 1200,
                PaymentSchedules = new List<PaymentSchedule>()
            };

            // Act
            var result = await _proposalRepository.InsertAsync(entity);

            // Assert
            var insertedEntity = await _dbContext.Set<Proposal>().FindAsync(result.Id);
            Assert.NotNull(insertedEntity);
            Assert.Equal(entity.LoanAmount, insertedEntity.LoanAmount);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnEntity()
        {
            // Arrange
            var entity = new Proposal
            {
                LoanAmount = 1000,
                AnnualInterestRate = 0.01m,
                NumberOfMonths = 12,
                MonthlyPayment = 100,
                TotalInterest = 1200,
                TotalPayment = 1200,
                PaymentSchedules = new List<PaymentSchedule>()
            };
            _dbContext.Set<Proposal>().Add(entity);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _proposalRepository.Queryable(x => x.Id == entity.Id).FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity.LoanAmount, result.LoanAmount);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntityAndSaveChanges()
        {
            // Arrange
            var entity = new Proposal
            {
                LoanAmount = 1000,
                AnnualInterestRate = 0.01m,
                NumberOfMonths = 12,
                MonthlyPayment = 100,
                TotalInterest = 1200,
                TotalPayment = 1200,
                PaymentSchedules = new List<PaymentSchedule>()
            };
            _dbContext.Set<Proposal>().Add(entity);
            await _dbContext.SaveChangesAsync();

            // Act
            entity.LoanAmount = 2000;
            var result = await _proposalRepository.UpdateAsync(entity);

            // Assert
            var updatedEntity = await _dbContext.Set<Proposal>().FindAsync(result.Id);
            Assert.NotNull(updatedEntity);
            Assert.Equal(entity.LoanAmount, updatedEntity.LoanAmount);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEntityAndSaveChanges()
        {
            // Arrange
            var entity = new Proposal
            {
                LoanAmount = 1000,
                AnnualInterestRate = 0.01m,
                NumberOfMonths = 12,
                MonthlyPayment = 100,
                TotalInterest = 1200,
                TotalPayment = 1200,
                PaymentSchedules = new List<PaymentSchedule>()
            };
            _dbContext.Set<Proposal>().Add(entity);
            await _dbContext.SaveChangesAsync();

            // Act
            await _proposalRepository.DeleteAsync(entity);

            // Assert
            var deletedEntity = await _dbContext.Set<Proposal>().FindAsync(entity.Id);
            Assert.Null(deletedEntity);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
