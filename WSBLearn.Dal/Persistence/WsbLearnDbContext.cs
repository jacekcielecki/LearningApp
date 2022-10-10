using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Dal.Persistence
{
    public class WsbLearnDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


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

            modelBuilder.Entity<User>(eb =>
            {
                eb.Property(x => x.Username)
                    .IsRequired();

                eb.Property(x => x.Password)
                    .IsRequired();

                eb.Property(x => x.EmailAddress)
                    .IsRequired();
            });

            modelBuilder.Entity<Role>(eb =>
            {
                eb.Property(x => x.Name)
                    .IsRequired();

                eb.HasData(
                    new Role { Id = 1, Name = "Admin" },
                    new Role { Id = 2, Name = "User" });
            });
        }
    }
}
