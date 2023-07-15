using LearningApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.Infrastructure.Persistence
{
    public class LearningAppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<CategoryProgress> CategoryProgresses { get; set; }
        public DbSet<LevelProgress> LevelProgresses { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<CategoryGroup> CategoryGroups { get; set; }

        public LearningAppDbContext(DbContextOptions<LearningAppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
