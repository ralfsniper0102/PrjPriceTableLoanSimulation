using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrjPriceTableLoanSimulation.Domain.Entities;

namespace PrjPriceTableLoanSimulation.Persistence.Mappings
{
    public class PaymentScheduleMap : IEntityTypeConfiguration<PaymentSchedule>
    {
        public void Configure(EntityTypeBuilder<PaymentSchedule> builder)
        {
            builder.ToTable("PaymentSchedule", "app_proposal");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Month).IsRequired();
            builder.Property(x => x.Principal).IsRequired();
            builder.Property(x => x.Interest).IsRequired();
            builder.Property(x => x.Balance).IsRequired();

            builder.HasOne(x => x.Proposal)
                .WithMany(x => x.PaymentSchedules)
                .HasForeignKey(x => x.ProposalId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
