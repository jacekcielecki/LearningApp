using LearningApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningApp.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).HasColumnType("varchar(2000)").IsRequired();
            builder.Property(x => x.Description).HasColumnType("varchar(2000)");
            builder.Property(x => x.IconUrl).HasColumnType("varchar(2000)");

            builder.HasMany(c => c.Questions)
                .WithOne(q => q.Category)
                .IsRequired();
        }
    }
}
