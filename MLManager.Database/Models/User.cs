using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLManager.Database
{
    [Table("users")]
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationTimestamp { get; set; }
        public DateTime? VerifiedEmailTimestamp { get; set; }
        public static void BuildModel(ModelBuilder modelBuilder)
        {
            var userEntity = modelBuilder.Entity<User>();   

            //Primary Key
            userEntity.HasKey(u => u.UserId);

            //Nonclustered Indices
            userEntity.HasIndex(u => u.Email).IsUnique();
            userEntity.HasIndex(u => u.Username).IsUnique();

            //Column Definitions
            userEntity.Property(u => u.UserId)
            .IsRequired()
            .UseIdentityAlwaysColumn()
            .HasColumnName(nameof(User.UserId).ToSnakeCase());

            userEntity.Property(u => u.FirstName)
            .IsRequired()
            .HasColumnName(nameof(User.FirstName).ToSnakeCase());

            userEntity.Property(u => u.LastName)
            .IsRequired()
            .HasColumnName(nameof(User.LastName).ToSnakeCase());

            userEntity.Property(u => u.Username)
            .IsRequired()
            .HasColumnName(nameof(User.Username).ToSnakeCase());
            
            userEntity.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(60)
            .HasColumnName(nameof(User.PasswordHash).ToSnakeCase());

            userEntity.Property(u => u.Email)
            .IsRequired()
            .HasColumnName(nameof(User.Email).ToSnakeCase());

            userEntity.Property(u => u.PhoneNumber)
            .HasMaxLength(12)
            .HasColumnName(nameof(User.PhoneNumber).ToSnakeCase());

            userEntity.Property(u => u.RegistrationTimestamp)
            .IsRequired()
            .HasColumnName(nameof(User.RegistrationTimestamp).ToSnakeCase())
            .HasDefaultValueSql("now() at time zone 'utc'");

            userEntity.Property(u => u.VerifiedEmailTimestamp)
            .HasColumnName(nameof(User.VerifiedEmailTimestamp).ToSnakeCase());
        }
    }
}