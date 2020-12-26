using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLManager.Database
{

    [Table("permissions")]
    public class Permission
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public PermissionType PermissionType { get; set; }
        public PermissionLevel PermissionLevel { get; set; }
        public DateTime CreateTimestamp { get; set; }

        public static void BuildModel(ModelBuilder modelBuilder)
        {
            var permissionEntity = modelBuilder.Entity<Permission>();

            //Primary Account
            permissionEntity.HasKey(x => new { x.UserId, x.AccountId, x.PermissionType });

            //Column Definitions
            permissionEntity.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName(nameof(Permission.UserId).ToSnakeCase());

            permissionEntity.Property(x => x.AccountId)
            .IsRequired()
            .HasColumnName(nameof(Permission.AccountId).ToSnakeCase());

            permissionEntity.Property(x => x.PermissionType)
            .IsRequired()
            .HasColumnName(nameof(Permission.PermissionType).ToSnakeCase());

            permissionEntity.Property(x => x.PermissionLevel)
            .IsRequired()
            .HasColumnName(nameof(Permission.PermissionLevel).ToSnakeCase());

            permissionEntity.Property(x => x.CreateTimestamp)
            .IsRequired()
            .HasDefaultValueSql("now() at time zone 'utc'")
            .HasColumnName(nameof(Permission.CreateTimestamp).ToSnakeCase());

            //Foreign Key
            permissionEntity.HasOne<PermissionTypeEntity>().WithMany().HasForeignKey(x => x.PermissionType);
            permissionEntity.HasOne<Account>().WithMany().HasForeignKey(x => x.AccountId);
            permissionEntity.HasOne<User>().WithMany().HasForeignKey(x => x.UserId);
        }
    }
}