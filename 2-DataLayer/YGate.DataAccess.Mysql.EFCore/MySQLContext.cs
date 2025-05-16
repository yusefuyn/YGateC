using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using YGate.DataAccess.Entities;
using YGate.Entities.BasedModel;

namespace YGate.DataAccess.Mysql.EFCore
{
    public class MySQLContext : BasedDbContext, IContext
    {
        private readonly string _connectionString;
        public MySQLContext(string connectionString)
        {
            _connectionString = connectionString;
            this.Database = base.Database;

        }
        public MySQLContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySQL(_connectionString);
        }
        public void MigrateDb()
        {
            base.Database.Migrate();
        }
        public string DatabaseConnectionStringName { get; set; } = "MysqlConnectionString";
        public int SaveChanges => base.SaveChanges();

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountPasswords> AccountsPasswords { get; set; }
        public DbSet<CategoryRoles> CategoryRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<AccountProperties> AccountProperties { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTemplate> CategoryTemplates { get; set; }
        public DbSet<CategoryTemplateValue> CategoryTemplateValues { get; set; }
        public DbSet<MeasurementCategory> MeasurementCategories { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<PropertyGroup> PropertyGroups { get; set; }
        public DatabaseFacade Database { get; set; }
        public DbSet<PropertyGroupValue> PropertyGroupValues { get; set; }
        public DbSet<Entitie> Entities { get; set; }
        public DbSet<DynamicPage> DynamicPages { get; set; }
        public DbSet<EntitieOwnerTransfer> EntitieOwner { get; set; }
        public DbSet<EntitiePropertyValue> EntitiePropertyValues { get; set; }
        public DbSet<CategoryHtmlTemplate> CategoryHtmlTemplates { get; set; }
        public DbSet<PageParameter> PageParameters { get ; set ; }
    }
}
