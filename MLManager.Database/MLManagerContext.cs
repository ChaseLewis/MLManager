using Npgsql;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MLManager.Database
{
    public class MLManagerContext : DbContext
    {
        private static DbContextOptions<MLManagerContext> BuildOptions(string connectionString)
        {
            DbContextOptionsBuilder<MLManagerContext> optionsBuilder = new DbContextOptionsBuilder<MLManagerContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return optionsBuilder.Options;
        }

        public MLManagerContext(string connectionString)
            : base(BuildOptions(connectionString))
            {

                
            }

        public MLManagerContext(DbContextOptions<MLManagerContext> options)
            : base(options) 
        {  

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<DatasetSchema> DatasetSchemas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var types = modelBuilder.Model.GetEntityTypes().ToList();
            //Need to translate the names to snake case to make this Citus compatible - important for creating auto-sharding capabilities

            //Fluent API definitions are just used for properties that can't be defined by data annotations
            var userEntity = modelBuilder.Entity<User>();   

            //User FluentAPI
            userEntity.HasIndex(u => u.Email).IsUnique();
            userEntity.HasIndex(u => u.Username).IsUnique();
            userEntity.Property(u => u.UserId).UseIdentityAlwaysColumn();
            userEntity.Property(u => u.RegistrationTimestamp).HasDefaultValueSql("now() at time zone 'utc'");

            //Dataset FluentAPI
            var datasetEntity = modelBuilder.Entity<Dataset>();
            datasetEntity.HasIndex(u => new { u.UserId, u.DatasetName }).IsUnique();
            datasetEntity.Property(u => u.DatasetId).UseIdentityAlwaysColumn();
            datasetEntity.Property(u => u.CreationTimestamp).HasDefaultValueSql("now() at time zone 'utc'");
            datasetEntity.HasOne<User>().WithMany().HasForeignKey(u => u.UserId);

            //DatasetSchema FluentAPI
            var datasetSchemaEntity = modelBuilder.Entity<DatasetSchema>();
            datasetSchemaEntity.HasKey(u => new { u.DatasetId, u.VersionId });
            datasetSchemaEntity.Property(u => u.CreationTimestamp).HasDefaultValueSql("now() at time zone 'utc'");
            datasetSchemaEntity.HasOne<User>().WithMany().HasForeignKey(u => u.UserId);
            datasetSchemaEntity.HasOne<Dataset>().WithMany().HasForeignKey(u => u.DatasetId);
        }
    }
}