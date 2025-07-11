using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EFCore.SqlServer.Configuration
{
    public class DonutConfig : IEntityTypeConfiguration<Donut>
    {
        public void Configure(EntityTypeBuilder<Donut> builder)
        {
            builder.ToTable(x => x.HasCheckConstraint("CK_Donuts_Price", "[Price]>=0"));
        }
    }
}
