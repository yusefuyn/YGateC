
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.DataAccess.Mssql.EFCore;
using YGate.DataAccess.Mysql.EFCore;
using YGate.DataAccess.Postgresql.EFCore;
using YGate.DataAccess.Sqllite.EFCore;
using YGate.Entities;
using YGate.Json.Operations;
using YGate.Entities.BasedModel;
using File = YGate.IO.Operations.File;
using YGate.Interfaces.Shared.Based;
using Microsoft.EntityFrameworkCore;
using YGate.DataAccess.Entities;
using YGate.Entities.ViewModels;
using YGate.Mapping;
using Azure;
using YGate.Entities.ResultModel;
using System.Configuration;
using YGate.Interfaces.OperationLayer;

namespace YGate.BusinessLayer.EFCore
{
    public class Operations
    {
        public IContext Context;
        IJsonSerializer jsonSerializer;

        public Operations(IJsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        public List<ConnectionString> DbSettings { get; set; } = new();
        public void AddDbSettings(List<ConnectionString> addedDbSettings)
        {
            DbSettings.AddRange(addedDbSettings);
            var res = new OperationResult<bool>("Settings Read");
            Operation.Runner<bool>.Run(ref res, () =>
            {
                SetContext(DbSettings.FirstOrDefault(xd => xd.Active == true));
                res.Obj = true;
            });
        }

        public CategoryRolesResultModel GetCategoryRole(string guid)
        {

            CategoryRolesResultModel categoryRoles = new CategoryRolesResultModel();

            List<Role> roleslist = (from categoryRole in Context.CategoryRoles
                                    join role in Context.Roles on categoryRole.RoleGuid equals role.DBGuid
                                    where categoryRole.CategoryGuid == guid
                                    select role).ToList();

            categoryRoles.AddedRoles = roleslist;
            categoryRoles.AddeableRoles = Context.Roles
                .Where(role => !roleslist.Contains(role) && role.Name != "Administrator")  // Contains ile karşılaştırma yapıyoruz
                .ToList();
            categoryRoles.Category = Context.Categories.FirstOrDefault(category => category.DBGuid == guid);
            return categoryRoles;
        }

        private void SetContext(ConnectionString connectionObject)
        {
            if (connectionObject == null)
                return;

            switch (connectionObject.Type)
            {
                case DbType.Mysql:
                    Context = new MySQLContext(connectionObject.Value);
                    break;
                case DbType.Postgresql:
                    Context = new PostgreSQLContext(connectionObject.Value);
                    break;
                case DbType.SqlLite:
                    Context = new SQLLiteContext(connectionObject.Value);
                    break;
                case DbType.MsSql:
                    Context = new MsSQLContext(connectionObject.Value);
                    break;
            }
        }

        public OperationResult<Account> GetAccount(string AccountDBGuid)
        {
            return GetAccountInternal(AccountDBGuid, null, null);
        }

        public OperationResult<Account> GetAccount(string AccountName, string AccountPassword)
        {
            return GetAccountInternal(null, AccountName, AccountPassword);
        }

        private OperationResult<Account> GetAccountInternal(string AccountDBGuid, string AccountName, string AccountPassword)
        {
            OperationResult<Account> operationResult = new($"GetAccount operations {AccountName ?? AccountDBGuid}");
            operationResult.Result = EnumOperationResult.Success;

            // Hesap sorgusunu yapıyoruz. İki farklı parametreyle de sorgulama yapıyoruz.
            Account account;
            if (!string.IsNullOrEmpty(AccountDBGuid))
            {
                account = Context.Accounts.SingleOrDefault(xd => xd.DBGuid == AccountDBGuid);
            }
            else
            {
                account = Context.Accounts.SingleOrDefault(xd => xd.Username == AccountName && xd.Password == AccountPassword && xd.IsActive == true);
            }

            // Hesap bulunamazsa hata döndürüyoruz.
            if (account == null)
            {
                operationResult.Result = EnumOperationResult.Error;
                operationResult.ShortDescription = "Hesapl bulunamadı.";
                return operationResult;
            }

            // Hesap ve roller ile ilgili işlemi gerçekleştirelim.
            account.Roles = GetRolesForAccount(account.DBGuid).ToList();  // Rolleri alıyoruz.
            operationResult.Obj = account;

            return operationResult;
        }

        private IEnumerable<Role> GetRolesForAccount(string accountGuid)
        {
            // AccountRoles'u ve ilgili rolleri tek seferde çekiyoruz.
            var accountRoles = Context.AccountRoles
                .Where(xd => xd.ToGuid == accountGuid)
                .Join(Context.Roles, ar => ar.RoleGuid, r => r.DBGuid, (ar, r) => r)
                .Distinct()
                .ToList();

            return accountRoles;
        }

        public OperationResult<bool> RegisterAccount(string username, string password, string emial)
        {
            OperationResult<bool> result = new("Register Account");
            YGate.Operation.Runner<bool>.Run(ref result, () =>
            {
                if (Context.Accounts.Where(xd => xd.Email == emial).Count() > 0)
                {
                    result.Result = EnumOperationResult.Error;
                    result.ShortDescription = "Email address has been used before";
                    result.Obj = false;
                    return;
                }
                if (Context.Accounts.Where(xd => xd.Username == username).Count() > 0)
                {
                    result.Result = EnumOperationResult.Error;
                    result.ShortDescription = "Username has been used before";
                    result.Obj = false;
                    return;
                }

                Account account = new Account()
                {
                    Username = username,
                    Password = password,
                    Email = emial,
                    DBGuid = YGate.String.Operations.GuidGen.Generate("account"),
                    IsActive = true,
                    CreatorGuid = "Admin",
                };
                AccountPasswords passwords = new()
                {
                    CreateDate = DateTime.Now,
                    DBGuid = YGate.String.Operations.GuidGen.Generate("passwd"),
                    IsActive = true,
                    CreatorGuid = account.DBGuid,
                    Password = password,
                };

                Context.Accounts.Add(account);
                Context.AccountsPasswords.Add(passwords);

                result.Obj = Context.SaveChanges() == 0 ? true : false;
            });
            return result;
        }

        public int GetAllUsersCount()
        {
            return Context.Accounts.Count();
        }

        public void DBUpdate()
        {
            Context.MigrateDb();
            Context.SaveChanges();
        }


        public async Task<List<CategoryViewModel>> GetCategoryTreeAsync()
        {
            List<CategoryViewModel> categories = Context.Categories
                .Where(c => c.ParentCategoryId == null)
                .Select(c => new CategoryViewModel()
                {
                    DBGuid = c.DBGuid,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    LongDescription = c.LongDescription,
                    Address = c.Address,
                    Icon = c.Icon,
                    CreatorGuid = c.CreatorGuid,
                    ShortDescription = c.ShortDescription,
                    Id = c.Id,
                }).ToList();
            foreach (CategoryViewModel category in categories)
                LoadChildCategories(category);
            return categories;
        }

        public void LoadFirstChildCategories(CategoryViewModel parentCategory)
        {
            var childCategories = Context.Categories
                .Where(c => c.ParentCategoryId == parentCategory.Id)
                .Select(c => new CategoryViewModel()
                {
                    DBGuid = c.DBGuid,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    LongDescription = c.LongDescription,
                    CreatorGuid = c.CreatorGuid,
                    ShortDescription = c.ShortDescription,
                    Id = c.Id,
                })
                .ToList();

            parentCategory.ChildCategories = childCategories;
        }

        public void LoadChildCategories(CategoryViewModel parentCategory)
        {
            var childCategories = Context.Categories
                .Where(c => c.ParentCategoryId == parentCategory.Id)
                .Select(c => new CategoryViewModel()
                {
                    DBGuid = c.DBGuid,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    LongDescription = c.LongDescription,
                    CreatorGuid = c.CreatorGuid,
                    ShortDescription = c.ShortDescription,
                    Id = c.Id,
                })
                .ToList();

            parentCategory.ChildCategories = childCategories;

            foreach (var childCategory in childCategories)
                LoadChildCategories(childCategory);
        }

        public CategoryViewModel GetCategoryButViewModel(Func<Category, bool> pre)
        {
            CategoryViewModel returnedObj = ConvertCategoryToViewModel(Context.Categories.FirstOrDefault(pre));

            returnedObj.Template = Context.CategoryTemplates
                .Where(xd => xd.CategoryId == returnedObj.Id && xd.IsActive)
                .Select(xd => YGate.Mapping.Operations.Convert<CategoryTemplateViewModel>(xd))
                .ToList();

            foreach (CategoryTemplateViewModel temp in returnedObj.Template)
            {
                if (temp.ValueType == PropertyValueType.ItemGroup)
                { // Tipi itemGroup'ise grubu al values'e yerleştir.
                    var objs = Context.CategoryTemplateValues.Where(tempval => tempval.CategoryTemplateGuid == temp.DBGuid).ToList();
                    var objss = Context.PropertyGroupValues.Where(pgv => pgv.PropertyGroupGuid == objs[0].ValueGroupGuid);
                    temp.Values = jsonSerializer.Serialize(objss);
                }

                if (temp.ValueType != PropertyValueType.Combos) // Tipi combos değilse hepsinin categorytemplatevalues'ine ilk elemanı ekle
                    temp.categoryTemplateValues.Add(new());

                if (temp.ValueType == PropertyValueType.Combos)
                    temp.Values = jsonSerializer.Serialize(Context.PropertyGroupValues.Where(xd => xd.PropertyGroupGuid == temp.ValueGroupGuid).ToList());

                if (temp.ValueType == PropertyValueType.Unit)
                {
                    string categoryTemplateValueGuid = Context.CategoryTemplateValues.SingleOrDefault(xd => xd.CategoryTemplateGuid == temp.DBGuid).ValueGroupGuid;
                    string MeasurementCategory = Context.MeasurementCategories.SingleOrDefault(xd => xd.DBGuid == categoryTemplateValueGuid).DBGuid;
                    List<MeasurementUnit> MeasurementUnitsList = Context.MeasurementUnits.Where(measurementUnit => measurementUnit.MeasurementCategoryGuid == MeasurementCategory).ToList();
                    temp.Values = jsonSerializer.Serialize(MeasurementUnitsList);
                }
            }

            return returnedObj;
        }

        public bool DeleteCategoryTemplate(CategoryTemplate temp)
        {
            try
            {
                var cont = Context.CategoryTemplates.FirstOrDefault(xd => xd.DBGuid == temp.DBGuid);
                Context.CategoryTemplates.Remove(cont);
                Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public CategoryViewModel ConvertCategoryToViewModel(Category category)
        {
            return YGate.Mapping.Operations.Convert<CategoryViewModel>(category);
        }

        public AdministratorUsersList GetAdministratorUserItem(string UserGuid)
        {
            AdministratorUsersList list = new();
            var opres = GetAccount(UserGuid);
            if (opres.Result == EnumOperationResult.Success)
                list.Accounts = opres.Obj;
            list.AccountPasswords = Context.AccountsPasswords.Where(xd => xd.CreatorGuid == UserGuid).ToList();
            list.AccountProperties = Context.AccountProperties.Where(xd => xd.CreatorGuid == UserGuid).ToList();
            list.AssignableRoles = Context.Roles.Where(xd => xd.IsActive == true).ToList();
            return list;
        }

        public List<EntitieViewModel> GetEntitieListByUserDBGuid(string userDBGuid)
        {
            var entities = Context.Entities.Where(xd => xd.CreatorGuid == userDBGuid).Select(dx => dx.DBGuid).ToList();
            var returnedList = new List<EntitieViewModel>();
            entities.ForEach(xd =>
            {
                var entitie = GetEntitieViewModelByEntitieDBGuid(xd);
                returnedList.Add(entitie);
            });
            return returnedList;
        }

        public EntitieViewModel GetEntitieViewModelByEntitieDBGuid(string guid)
        {
            var obj = _GetEntitieViewModels(xd => xd.DBGuid == guid).FirstOrDefault(); // Ana öğe gelir
            var list = _GetSubItemList(xd => xd.ParentEntitieDBGuid == guid); // Alt öğeler gelir
            _EntitieViewModelGetInfo(ref list); // Alt öğelerin bilgileri getirilir. Values'leri hariç
            //list.ForEach(xd => xd.Values = _GetEntitiePropertyValues(xd.DBGuid)); // Alt öğelerin Valuesleri gelir.
            list.Add(obj); // Ana Öğe tek listeye katılır.


            _SetChildEntities(ref list, obj); // Alt üst İlişkisine sokulurlar
            var returnedobj = list.FirstOrDefault(); // Alt üst ilişkisine sokulmuş listenin ilk elemanı alınır.
            return returnedobj;
        }


        #region entitieOperations
        /// <summary>
        /// Alt varlıklarıda dahil verilen guid varlığınıda siler.
        /// </summary>
        /// <param name="guid"></param>
        public void _RemoveEntitie(string guid)
        {
            var MainObj = Context.Entities.SingleOrDefault(xd => xd.DBGuid == guid);
            var SubObjs = Context.Entities.Where(xd => xd.ParentEntitieDBGuid == MainObj.DBGuid).ToList();
            SubObjs.ForEach(subObj =>
            {
                _RemoveEntitie(subObj.DBGuid);
            });
            Context.Entities.Remove(MainObj);
            var Proprs = Context.EntitiePropertyValues.Where(xd => xd.EntitieDbGuid == MainObj.DBGuid);
            Context.EntitiePropertyValues.RemoveRange(Proprs);
        }

        /// <summary>
        /// Varlıkları DB'den alır ve onları birer EntitieViewModel'e dönüştürür gerekli bilgileri doldurma Fonksiyonunu çağırır.
        /// </summary>
        /// <param name="predi"></param>
        /// <returns></returns>
        public List<EntitieViewModel> _GetEntitieViewModels(Func<Entitie, bool> predi = null)
        {
            List<EntitieViewModel> list = null;

            if (predi == null)
                list = YGate.Mapping.Operations.ConvertToList<EntitieViewModel>(Context.Entities.Where(xd => xd.IsActive == true));
            else
                list = YGate.Mapping.Operations.ConvertToList<EntitieViewModel>(Context.Entities.Where(predi));


            _EntitieViewModelGetInfo(ref list);

            return list;
        }

        /// <summary>
        /// DBGuid'i verilen varlığa ait alt elemanları getirir.
        /// </summary>
        /// <param name="EntitieDBGuid"></param>
        /// <returns></returns>
        public List<EntitieViewModel> _GetSubItemList(string EntitieDBGuid)
        {
            return _GetSubItemList(xd => xd.ParentEntitieDBGuid == EntitieDBGuid);
        }

        /// <summary>
        /// Sadece alt üyeleri ve onların altını bulmak ve onları birer EntitieViewModel'e dönüştürme işlemi yapar
        /// Info getirmez. Boşluk Doldurmaz.
        /// </summary>
        /// <param name="predi"></param>
        /// <returns></returns>
        public List<EntitieViewModel> _GetSubItemList(Func<Entitie, bool> predi)
        {
            var returnedList = new List<EntitieViewModel>();

            // Sadece bir sorgu yaparak tüm elemanları çek
            var convertedList = Context.Entities.Where(predi).ToList();

            // Çekilen veriyi listeye ekle
            returnedList.AddRange(YGate.Mapping.Operations.ConvertToList<EntitieViewModel>(convertedList));

            return returnedList;
        }

        /// <summary>
        /// Verilen Listedeki Öğeleri Alt Üst İlişkisine Sokar ve Bir Kaç Bilgiyi Çekip Doldurur.
        /// Liste boş yada yeterli eleman yok ise alt üst ilişkisi doğru tanımlanmaz.
        /// </summary>
        /// <param name="list"></param>
        public void _EntitieViewModelGetInfo(ref List<EntitieViewModel> list)
        {
            var dbguids = list.ToList().Select(ent => ent.DBGuid);
            List<string> ListDbGuids = list.Select(l => l.DBGuid).ToList();
            List<string> ListCategoryDBGuids = list.Select(l => l.CategoryDBGuid).ToList();
            List<string> ownerGuids = Context.EntitieOwner
                .Where(xd => dbguids.Contains(xd.EntitieGuid))
                .OrderByDescending(xd => xd.DateTimeUTC)
                .Select(xd => xd.NewOwnerGuid)
                .ToList();

            List<Category> categories = Context.Categories
                .Where(category => ListCategoryDBGuids.Contains(category.DBGuid))
                .ToList();

            List<CategoryHtmlTemplate> htmlTemplates = Context.CategoryHtmlTemplates
                .Where(template => ListCategoryDBGuids.Contains(template.CategoryGuid))
                .ToList();

            Dictionary<string, string> accountDictionary =
            Context.Accounts
                .Where(xd => ownerGuids.Contains(xd.DBGuid))
                .ToDictionary(xd => xd.DBGuid, xd => xd.Username);

            Dictionary<string, string> categoryDictionary = categories.ToDictionary(c => c.DBGuid, c => c.Name);
            Dictionary<string, CategoryHtmlTemplate> htmlTemplateDictionary = htmlTemplates.ToDictionary(ht => ht.CategoryGuid, ht => ht);

            list.ForEach(entitieViewModel =>
            {
                entitieViewModel.Values = _GetEntitiePropertyValues(xd => xd.EntitieDbGuid == entitieViewModel.DBGuid);

                entitieViewModel.OwnerName = accountDictionary.ContainsKey(entitieViewModel.CreatorGuid)
                    ? accountDictionary[entitieViewModel.CreatorGuid]
                    : null;

                if (categoryDictionary.TryGetValue(entitieViewModel.CategoryDBGuid, out var categoryName))
                    entitieViewModel.CategoryName = categoryName;

                if (htmlTemplateDictionary.TryGetValue(entitieViewModel.CategoryDBGuid, out var template))
                    entitieViewModel.HtmlTemplate = template;
            });

            var ParentModels = list.Where(xd => string.IsNullOrEmpty(xd.ParentEntitieDBGuid)).ToList();
            foreach (var parentModel in ParentModels)
            {
                _SetChildEntities(ref list, parentModel);
            }
        }

        /// <summary>
        /// Bu Alt yada Üst Elemanları İlk Parametrede Arar. 
        /// Eğer Liste boş ise alt elemanları ilişkileyemez.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentModel"></param>
        public void _SetChildEntities(ref List<EntitieViewModel> list, EntitieViewModel parentModel)
        {
            var childModels = list.Where(xd => xd.ParentEntitieDBGuid == parentModel.DBGuid).ToList();
            foreach (var child in childModels)
                _SetChildEntities(ref list, child);
            parentModel.ChildEntitie = childModels;
            list.RemoveAll(xd => xd.ParentEntitieDBGuid == parentModel.DBGuid);
        }

        /// <summary>
        /// DBGuid'den EntitieViewModel Döndürür.
        /// </summary>
        /// <param name="DBGuid"></param>
        /// <returns></returns>
        public EntitieViewModel _GetEntitieViewModel(string DBGuid) => _GetEntitieViewModel(xd => xd.DBGuid == DBGuid);

        /// <summary>
        /// Predicate'ten EntitieViewModel Döndürür
        /// </summary>
        /// <param name="predi"></param>
        /// <returns></returns>
        public EntitieViewModel _GetEntitieViewModel(Func<Entitie, bool> predi)
        {
            return YGate.Mapping.Operations.Convert<EntitieViewModel>(Context.Entities.FirstOrDefault(predi));
        }

        /// <summary>
        /// DbGuid'i verilen varlığın Properties'lerini döndürür.
        /// </summary>
        /// <param name="EntitieDBGuid"></param>
        /// <returns></returns>
        public List<EntitiePropertyValue> _GetEntitiePropertyValues(string EntitieDBGuid)
        => _GetEntitiePropertyValues(xd => xd.EntitieDbGuid == EntitieDBGuid);
        /// <summary>
        /// Verilen Predicate'ten varlığın Properties'lerini döndürür.
        /// </summary>
        /// <param name="predi"></param>
        /// <returns></returns>
        public List<EntitiePropertyValue> _GetEntitiePropertyValues(Func<EntitiePropertyValue, bool> predi)
        {
            // Tipide dahil her datayı birleştirir.
            List<EntitiePropertyValue> entityPropertyValues = Context.EntitiePropertyValues
                .Where(predi)
                .ToList();

            // Tiplerini getirmek için
            foreach (var propValue in entityPropertyValues)
            {
                // Normal bir öğe değilse
                var categoryTemp = Context.CategoryTemplates.FirstOrDefault(category => category.DBGuid == propValue.CategoryTemplateGuid);
                if (categoryTemp != null)
                {
                    propValue.Seo = categoryTemp.Seo;
                    propValue.Type = categoryTemp.ValueType;
                    continue;
                }
            }
            foreach (var xd in entityPropertyValues)
            {
                try
                {
                    if (xd.Type == PropertyValueType.Unit)
                    {
                        dynamic obj = jsonSerializer.Deserialize<dynamic>(xd.PropertyValue);
                        float Integer = obj.IntegerVal;
                        string UnitGuid = obj.UnitGuid;
                        var Unit = Context.MeasurementUnits.FirstOrDefault(xd => xd.DBGuid == UnitGuid);
                        string UnitValue = Unit.Name;
                        if (!string.IsNullOrEmpty(Unit.Symbol))
                            UnitValue = Unit.Symbol;
                        xd.PropertyValue = $"{Integer}{UnitValue}";
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            return entityPropertyValues;
        }

        public List<EntitieViewModel> GetEntitiesFromCategoryGuid(string CategoryGuid)
        {
            List<EntitieViewModel> list = _GetEntitieViewModels(xd => xd.CategoryDBGuid == CategoryGuid).ToList();
            List<EntitieViewModel> allItems = new List<EntitieViewModel>(list);
            // Kuyruk ile alt öğeleri al
            Queue<EntitieViewModel> queue = new Queue<EntitieViewModel>(list);
            while (queue.Any())
            {
                var categoryEntitie = queue.Dequeue();
                var subList = _GetSubItemList(xd => xd.ParentEntitieDBGuid == categoryEntitie.DBGuid);
                allItems.AddRange(subList);
                // Alt öğeleri kuyruğa ekle (rekürsif yerine)
                foreach (var subItem in subList)
                {
                    queue.Enqueue(subItem);
                }
            }
            _EntitieViewModelGetInfo(ref allItems);

            return allItems;
        }

        public bool UserAuthorizeControlFromUserGuid(string UserGuid, List<string> Roles)
        {
            bool returned = false;
            List<Role> userRole = GetUserRolesFromUserGuid(UserGuid);
            returned = Roles.Any(role => userRole.Any(xd => xd.Name == role));
            return returned;
        }

        public List<Role> GetUserRolesFromUserGuid(string UserGuid)
        {
            return Context.AccountRoles
                    .Where(xd => xd.ToGuid == UserGuid)
                    .Join(Context.Roles,
                          accountRole => accountRole.RoleGuid,
                          role => role.DBGuid,
                          (accountRole, role) => role)
                    .ToList();
        }

        public List<Role> GetCategoryRoleFromCategoryGuid(string CategoryGuid)
        {
            var returned = Context.CategoryRoles
                             .Where(xd => xd.CategoryGuid == CategoryGuid)
                             .Join(Context.Roles,
                                   categoryRole => categoryRole.RoleGuid,
                                   role => role.DBGuid,
                                   (categoryRole, role) => role)
                             .ToList();
            var administratorRole = Context.Roles.FirstOrDefault(xd => xd.Name == "Administrator");
            returned.Add(administratorRole);

            return returned;
        }

        public bool ObjectOwnedByTheUser(object objectguid, object ownerguid)
        {
            if (Context.EntitieOwner.Where(xd => xd.EntitieGuid == objectguid && xd.NewOwnerGuid == ownerguid).FirstOrDefault() == null)
                return false;
            else
                return true;
        }

        public bool IsThereSuchAUser(string UserID)
        {
            if (Context.Accounts.Where(xd => xd.DBGuid == UserID).Count() > 0)
                return true;
            else
                return false;
        }

        public bool UserPasswordIsCorrect(string ownerGuid,string ownerPassword)
        {
            string pass = YGate.String.Operations.Hash.SaltAndSHA512(ownerPassword);
            var users = Context.Accounts.Where(xd => xd.DBGuid == ownerGuid && xd.Password == pass);
            if (users.Count() > 0)
                return true;
            else
                return false;
        }

        public List<EntitieOwnerTransfer> GetEntitieTransferList(string guid)
        {
            return Context.EntitieOwner.Where(xd=> xd.EntitieGuid == guid).ToList();
        }
        #endregion

    }
}
