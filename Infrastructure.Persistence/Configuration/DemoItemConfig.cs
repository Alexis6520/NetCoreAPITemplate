using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    internal class DemoItemConfig
    {
        public DemoItemConfig(EntityTypeBuilder<DemoItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("decimal(10,3)");
        }
    }
}
