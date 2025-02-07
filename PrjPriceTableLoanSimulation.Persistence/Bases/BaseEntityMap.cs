using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrjPriceTableLoanSimulation.Domain.Bases;

namespace PrjPriceTableLoanSimulation.Persistence.Bases
{
    public static class BaseEntityMap
    {
        public static void Configure<TEntity, TKey>(EntityTypeBuilder<TEntity> builder) where TEntity : BaseEntity<TKey>
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.CreatedAt).HasDefaultValueSql("current_timestamp");
        }
    }
}
