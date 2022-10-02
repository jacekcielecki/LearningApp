using Microsoft.EntityFrameworkCore;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Dal.Persistence
{
    public class WsbLearnDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }


        public WsbLearnDbContext(DbContextOptions<WsbLearnDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(eb =>
            {
                eb.Property(x => x.Name)
                    .HasMaxLength(20)
                    .IsRequired();

                eb.Property(x => x.Description)
                    .HasMaxLength(400);

                eb.Property(x => x.IconUrl)
                    .HasMaxLength(400);

                eb.Property(x => x.QuestionsPerLesson)
                    .IsRequired();

                eb.Property(x => x.LessonsPerLevel)
                    .IsRequired();
            });

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Questions)
                .WithOne(e => e.Category)
                .IsRequired();

            modelBuilder.Entity<Question>(eb =>
            {
                eb.Property(x => x.Level)
                    .IsRequired();

                eb.Property(x => x.QuestionContent)
                    .HasMaxLength(400)
                    .IsRequired();

                eb.Property(x => x.ImageUrl)
                    .HasMaxLength(400)
                    .IsRequired();

                eb.Property(x => x.CorrectAnswer)
                    .IsRequired();
            });
        }
    }
}
