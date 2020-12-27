using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLManager.Database
{
    [Table("jwt_securities")]
    public class JwtSecurity
    {
        public Guid DeviceId { get; set; }
        public int UserId { get; set; }
        public Guid RefreshToken { get; set; }
        public DateTime CreateTimestamp { get; set; }
        public DateTime LastUpdatedTimestamp { get; set; }

        public static void BuildModel(ModelBuilder modelBuilder)
        {
            var jwtEntity = modelBuilder.Entity<JwtSecurity>();
            
            //Primary Key
            jwtEntity.HasKey(x => new { x.DeviceId, x.UserId });

            //Column Definitions
            jwtEntity.Property(x => x.DeviceId)
            .IsRequired()
            .HasColumnName(nameof(JwtSecurity.DeviceId).ToSnakeCase());

            jwtEntity.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName(nameof(JwtSecurity.UserId).ToSnakeCase());

            jwtEntity.Property(x => x.RefreshToken)
            .IsRequired()
            .HasColumnName(nameof(JwtSecurity.RefreshToken).ToSnakeCase());

            jwtEntity.Property(x => x.CreateTimestamp)
            .IsRequired()
            .HasDefaultValueSql("now() at time zone 'utc'")
            .HasColumnName(nameof(JwtSecurity.CreateTimestamp).ToSnakeCase());

            jwtEntity.Property(x => x.LastUpdatedTimestamp)
            .IsRequired()
            .HasDefaultValueSql("now() at time zone 'utc'")
            .HasColumnName(nameof(JwtSecurity.LastUpdatedTimestamp).ToSnakeCase());            
        }
    }
}