using Microsoft.EntityFrameworkCore;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Dal.Persistence
{
    public class WsbLearnDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

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

                eb.Property(x => x.QuestionsPerLevel)
                    .HasMaxLength(400)
                    .IsRequired();

                eb.Property(x => x.Levels)
                    .HasMaxLength(400)
                    .IsRequired();
            });
        }
    }
}
