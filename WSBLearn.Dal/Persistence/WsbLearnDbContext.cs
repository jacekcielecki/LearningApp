using Microsoft.EntityFrameworkCore;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Dal.Persistence
{
    public class WsbLearnDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<CategoryProgress> CategoryProgresses { get; set; }
        public DbSet<LevelProgress> LevelProgresses { get; set; }
        public DbSet<Achievement> Achievements { get; set; }

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
            });

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Questions)
                .WithOne(e => e.Category)
                .IsRequired();


            modelBuilder.Entity<User>()
                .HasOne(c => c.UserProgress)
                .WithOne(e => e.User)
                .HasForeignKey<UserProgress>(a => a.UserId);

            modelBuilder.Entity<Question>(eb =>
            {
                eb.Property(x => x.QuestionContent)
                    .HasMaxLength(400)
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

            modelBuilder.Entity<UserProgress>(eb =>
            {
                eb.Property(x => x.Level)
                    .IsRequired();
            });

            modelBuilder.Entity<CategoryProgress>(eb =>
            {
                eb.Property(x => x.CategoryName)
                    .IsRequired();
            });

            modelBuilder.Entity<LevelProgress>(eb =>
            {
                eb.Property(x => x.LevelName)
                    .IsRequired();
            });

            modelBuilder.Entity<Achievement>(eb =>
            {
                eb.Property(x => x.Name)
                    .HasMaxLength(20)
                    .IsRequired();
            });
        }
    }
}
