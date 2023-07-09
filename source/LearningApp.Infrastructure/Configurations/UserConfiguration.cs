using LearningApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningApp.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Username).HasColumnType("varchar(2000)").IsRequired();
            builder.Property(x => x.Password).HasColumnType("varchar(2000)").IsRequired();
            builder.Property(x => x.EmailAddress).HasColumnType("varchar(2000)").IsRequired();
            builder.Property(x => x.ProfilePictureUrl).HasColumnType("varchar(2000)").IsRequired();
            builder.Property(x => x.VerificationToken).HasColumnType("varchar(2000)");

            builder.HasOne(u => u.UserProgress)
                .WithOne(up => up.User)
                .HasForeignKey<UserProgress>(up => up.UserId);

            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            builder.HasMany(u => u.Questions)
                .WithOne(q => q.Creator)
                .HasForeignKey(q => q.CreatorId);

            builder.HasMany(u => u.Categories)
                .WithOne(c => c.Creator)
                .HasForeignKey(c => c.CreatorId);
        }
    }
}
