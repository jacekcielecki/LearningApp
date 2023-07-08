using LearningApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningApp.Infrastructure.Configurations
{
    public class CategoryProgressConfiguration : IEntityTypeConfiguration<CategoryProgress>
    {
        public void Configure(EntityTypeBuilder<CategoryProgress> builder)
        {
            builder.Property(x => x.CategoryName).IsRequired();

            builder.HasMany(cp => cp.LevelProgresses)
                .WithOne(lp => lp.CategoryProgress)
                .HasForeignKey(lp => lp.CategoryProgressId);
        }
    }
}
