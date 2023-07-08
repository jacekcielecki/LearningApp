using LearningApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningApp.Infrastructure.Configurations
{
    public class UserProgressConfiguration : IEntityTypeConfiguration<UserProgress>
    {
        public void Configure(EntityTypeBuilder<UserProgress> builder)
        {
            builder.Property(x => x.Level).IsRequired();

            builder.HasMany(up => up.Achievements)
                .WithMany(a => a.UserProgresses);

            builder.HasMany(up => up.CategoryProgress)
                .WithOne(cp => cp.UserProgress)
                .HasForeignKey(cp => cp.UserProgressId);
        }
    }
}
