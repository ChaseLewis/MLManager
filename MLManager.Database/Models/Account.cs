using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLManager.Database
{
    [Table("accounts")]
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public DateTime CreateTimestamp { get; set; }

        public static void BuildModel(ModelBuilder modelBuilder)
        {
            var accountEntity = modelBuilder.Entity<Account>();
            
            accountEntity.HasKey(x => x.AccountId);

            accountEntity.Property(x => x.AccountId)
            .IsRequired()
            .UseIdentityAlwaysColumn()
            .HasColumnName(nameof(Account.AccountId).ToSnakeCase());

            accountEntity.Property(x => x.Name)
            .IsRequired()
            .HasColumnName(nameof(Account.Name).ToSnakeCase());

            accountEntity.Property(x => x.CreateTimestamp)
            .IsRequired()
            .HasDefaultValueSql("now() at time zone 'utc'")
            .HasColumnName(nameof(Account.CreateTimestamp).ToSnakeCase());
        }
    }
}