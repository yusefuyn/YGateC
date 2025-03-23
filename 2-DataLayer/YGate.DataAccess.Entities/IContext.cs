using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using YGate.Entities.BasedModel;
namespace YGate.DataAccess.Entities
{
    public interface IContext
    {
        protected string DatabaseConnectionStringName { get; set; }
        int SaveChanges();
        public void MigrateDb();
        DbSet<Account> Accounts { get; set; }
        DbSet<AccountPasswords> AccountsPasswords { get; set; }
        DbSet<AccountProperties> AccountProperties { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<CategoryTemplate> CategoryTemplates { get; set; }
        DbSet<CategoryTemplateValue> CategoryTemplateValues { get; set; }
        DbSet<Comment> Comments { get; set; }

        DbSet<MeasurementCategory> MeasurementCategories { get; set; }
        DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        DbSet<PropertyGroup> PropertyGroups { get; set; }
        DbSet<PropertyGroupValue> PropertyGroupValues { get; set; }

        DbSet<Entitie> Entities { get; set; }
        DbSet<EntitiePropertyValue> EntitiePropertyValues { get; set; }
        DbSet<EntitieOwnerTransfer> EntitieOwner { get; set; }

        DbSet<CategoryHtmlTemplate> CategoryHtmlTemplates { get; set; }
        DatabaseFacade Database { get; set; }

        DbSet<Role> Roles { get; set; }
        DbSet<AccountRole> AccountRoles { get; set; }

        DbSet<CategoryRoles> CategoryRoles { get; set; }
        DbSet<DynamicPage> DynamicPages { get; set; }

    }
}
