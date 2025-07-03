using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EFCore.Postgre.Configuration
{
    public class DonutConfig : IEntityTypeConfiguration<Donut>
    {
        public void Configure(EntityTypeBuilder<Donut> builder)
        {
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Donut_Price", "\"Price\" >= 0");
            });
        }
    }
}
