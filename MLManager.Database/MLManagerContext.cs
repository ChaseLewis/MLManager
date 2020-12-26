using Dapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public MLManagerContext(DbContextOptions<MLManagerContext> options)
        : base(options) 
        {  
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionTypeEntity> PermissionTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<DatasetSchema> DatasetSchemas { get; set; }
        public DbSet<DataItem> DataItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            
            var types = modelBuilder.Model.GetEntityTypes().ToList();
            //Need to translate the names to snake case to make this Citus compatible - important for creating auto-sharding capabilities

            //Fluent API definitions are just used for properties that can't be defined by data annotations
            Account.BuildModel(modelBuilder);
            User.BuildModel(modelBuilder);
            Dataset.BuildModel(modelBuilder);
            DatasetSchema.BuildModel(modelBuilder);
            DataItem.BuildModel(modelBuilder);
            PermissionTypeEntity.BuildModel(modelBuilder);
            Permission.BuildModel(modelBuilder);
        }
    }
}