using LearningApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningApp.Infrastructure.Configurations
{
    public class CategoryGroupConfiguration : IEntityTypeConfiguration<CategoryGroup>
    {
        public void Configure(EntityTypeBuilder<CategoryGroup> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(400).IsRequired();

            builder.HasMany(c => c.Categories)
                .WithMany(e => e.CategoryGroups);
        }
    }
}
