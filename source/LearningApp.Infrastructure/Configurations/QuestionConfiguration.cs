using LearningApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningApp.Infrastructure.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Property(x => x.QuestionContent).HasColumnType("varchar(2000)").IsRequired();
            builder.Property(x => x.ImageUrl).HasColumnType("varchar(2000)");
            builder.Property(x => x.A).HasColumnType("varchar(2000)");
            builder.Property(x => x.B).HasColumnType("varchar(2000)");
            builder.Property(x => x.C).HasColumnType("varchar(2000)");
            builder.Property(x => x.D).HasColumnType("varchar(2000)");
        }
    }
}
