using LearningApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningApp.Infrastructure.Configurations
{
    public class LevelProgressConfiguration : IEntityTypeConfiguration<LevelProgress>
    {
        public void Configure(EntityTypeBuilder<LevelProgress> builder)
        {
            builder.Property(x => x.LevelName).HasColumnType("varchar(2000)").IsRequired();
        }
    }
}
