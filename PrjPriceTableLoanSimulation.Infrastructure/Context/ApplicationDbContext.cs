using Microsoft.EntityFrameworkCore;
using PrjPriceTableLoanSimulation.Domain.Entities;
using PrjPriceTableLoanSimulation.Persistence.Mappings;

namespace PrjPriceTableLoanSimulation.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<PaymentSchedule> PaymentSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProposalMap());
            modelBuilder.ApplyConfiguration(new PaymentScheduleMap());
        }
    }
}
