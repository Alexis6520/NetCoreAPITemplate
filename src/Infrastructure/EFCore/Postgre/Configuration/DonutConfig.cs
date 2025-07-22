using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EFCore.Postgre.Configuration
{
    internal class DonutConfig : IEntityTypeConfiguration<Donut>
    {
        public void Configure(EntityTypeBuilder<Donut> builder)
        {
            builder.ToTable(x =>
            {
                x.HasCheckConstraint("CK_Donuts_Price", "\"Price\">=0");
            });
        }
    }
}
