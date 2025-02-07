using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrjPriceTableLoanSimulation.Domain.Entities;
using PrjPriceTableLoanSimulation.Persistence.Bases;

namespace PrjPriceTableLoanSimulation.Persistence.Mappings
{
    public class ProposalMap : IEntityTypeConfiguration<Proposal>
    {
        public void Configure(EntityTypeBuilder<Proposal> builder)
        {
            BaseEntityMap.Configure<Proposal, int>(builder);
            builder.ToTable("Proposta", "app_proposal");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.PaymentSchedules)
                .WithOne(x => x.Proposal)
                .HasForeignKey(x => x.ProposalId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
