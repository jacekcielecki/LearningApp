using LearningApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningApp.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Username).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.EmailAddress).IsRequired();

            builder.HasOne(u => u.UserProgress)
                .WithOne(up => up.User)
                .HasForeignKey<UserProgress>(up => up.UserId);

            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            builder.HasMany(u => u.Questions)
                .WithOne(q => q.Author)
                .HasForeignKey(q => q.AuthorId);
        }
    }
}
