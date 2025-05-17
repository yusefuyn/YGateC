using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Entities;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Mapping;
using YGate.Server.Facades;

namespace YGate.BusinessLayer.EFCore.Concretes.Repositories
{
    public class EntitieRepository : IEntitieRepository
    {
        Operations operations;
        IBaseFacades baseFacades;
        public EntitieRepository(Operations operations, IBaseFacades baseFacades)
        {
            this.operations = operations;
            this.baseFacades = baseFacades;
        }


        public async Task<IRequestResult> AddEntitite(IRequestParameter parameter)
        {
            EntitieRequestViewModel RequestModel = baseFacades.JsonSerializer.Deserialize<EntitieRequestViewModel>(parameter.Parameters.ToString());
            RequestResult result = new("Add Entitie");
            result.Result = EnumRequestResult.Success;
            #region ModeliDuzenleme
            RequestModel.MainModel.DBGuid = String.Operations.GuidGen.Generate("Entitie");
            RequestModel.MainModel.Values = RequestModel.MainModel.Values
                .Select(values =>
                {
                    values.EntitieDbGuid = RequestModel.MainModel.DBGuid;
                    values.DBGuid = String.Operations.GuidGen.Generate("MainEntitieValue");
                    return values;
                })
                .ToList();

            RequestModel.SubModel = RequestModel.SubModel.Select(subModel =>
            {
                subModel.ParentEntitieDBGuid = RequestModel.MainModel.DBGuid; // Ebeveyn objenin dbguid'i geliyor.
                subModel.DBGuid = String.Operations.GuidGen.Generate("SubEntitie"); // Objenin dbguid'i yeniden atanıyor
                subModel.Values = subModel.Values.Select(subModelValues =>
                { // Values'leri düzenleniyor
                    subModelValues.DBGuid = String.Operations.GuidGen.Generate("SubEntitieValue"); // valueslere yeni dbguid atanıyor
                    subModelValues.EntitieDbGuid = subModel.DBGuid; // valueslerin ebeveny dbguid'i atanıyor
                    return subModelValues;
                }).ToList();
                return subModel;
            }).ToList();
            #endregion
            string json = baseFacades.JsonSerializer.Serialize(RequestModel);
            #region DBKayıt
            operations.Context.Entities.Add(RequestModel.MainModel);
            operations.Context.EntitiePropertyValues.AddRange(RequestModel.MainModel.Values);
            operations.Context.Entities.AddRange(RequestModel.SubModel);
            RequestModel.SubModel.ForEach(xd =>
            {
                operations.Context.EntitiePropertyValues.AddRange(xd.Values);
            });
            #endregion
            #region TransferObjesiHazirlama
            EntitieOwnerTransfer entitieOwnerTransfer = new()
            {
                DateTimeUTC = DateTime.UtcNow,
                EntitieGuid = RequestModel.MainModel.DBGuid,
                OldOwnerGuid = "System",
                NewOwnerGuid = RequestModel.MainModel.CreatorGuid,
                Hash = String.Operations.Hash.ComputeSHA256("System")
            };
            operations.Context.EntitieOwner.Add(entitieOwnerTransfer);
            #endregion
            operations.Context.SaveChanges();
            result.Object = RequestModel;
            return result;
        }

        public async Task<IRequestResult> Delete(IRequestParameter parameter)
        {
            string guid = parameter.Parameters.ToString();
            RequestResult result = new($"Delete Entitie {guid}");
            result.Result = EnumRequestResult.Success;
            using (var transaction = operations.Context.Database.BeginTransaction())
            {
                try
                {
                    operations._RemoveEntitie(guid);
                    operations.Context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return result;
        }

        public async Task<IRequestResult> GetAllEntitieButCategoryGuid(IRequestParameter parameter)
        {
            string categoryGuid = parameter.Parameters.ToString();
            RequestResult result = new($"Get Entities but Category Guid {categoryGuid}");
            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            result.Object = operations.GetEntitiesFromCategoryGuid(categoryGuid);
            return result;
        }

        public async Task<IRequestResult> GetAllEntitieButCategoryId(IRequestParameter parameter)
        {

            // Burada farklı yaklaşımlar söz konusu olabilir.
            // Üzerinde düşün parametre olarak bir Token sınıfı geçebilirim.
            // JWT Tokeni token sınıfına verebilir içerisinde rolleri alabilirim.
            // Neticede token kullanıcının rollerini barındırıyor.
            // Bu fazla işlemi azaltır. Kullanıcı rollerini çekmek vs gibi.
            // RequestParameter sınıfının içindede bu özel token sınıfını kullanabilirim.
            // Unutmaki RequestParameter sınıfındaki Token'i Ardışık işlemlerde güvenlik adımı olarak kullanmak için koymuştum.
            // Bilmiyorum ne yapsam biraz düşünmem lazım ama ... puf ;\
            // ------------------------------------------------------------------------------------------------------------------------
            // Şu anda Entitie güncelleme ve Decentralize üzerinde düşünmem lazım.
            // Entitie'leri bir json sınıfı olarak tutmayı unutma her entitie sınıfında data adında yerde tutacağız
            // Entitie sınıfının versiyonları olacak 1 2 diye güncelleme yapıldığında versiyon artacak ve data'da güncel hali tutulacak
            // BlockChain için anca bu şekilde güncelleme yapılabilir.
            // Eski bloklara dokunamayız ama aynı bloğun biraz farklısını ekleyemeyiz değilya !



            string categoryId = parameter.Parameters.ToString();
            RequestResult result = new($"Get Entities but Category Id {categoryId}")
            {
                Result = EnumRequestResult.Success,
                To = EnumTo.Server
            };

            Category? cat = operations.Context.Categories.FirstOrDefault(xd => xd.Id == Convert.ToUInt32(categoryId));



            // Buraya dikkat ya eski bir url ile giriş yapmaya çalışmış yada
            // bu kullanıcı birşeyin peşinde.
            // İleride loglanması gerekebilir.
            if (cat == null)
                return new RequestResult("") { To = EnumTo.Server, Object = null, LongDescription = "Verilen kategori ID'sine ait kategori bulunamadı.", Result = EnumRequestResult.Stop, ShortDescription = "Kategori bulunamadı" };

            List<Role> categoryRoles = operations.GetCategoryRoleFromCategoryGuid(cat.DBGuid);

            if (categoryRoles.Any(xd => xd.Name == "All"))
            {
                var obj = operations.GetEntitiesFromCategoryGuid(cat.DBGuid);
                result.Object = obj;
            }
            else
            {
                if (string.IsNullOrEmpty(parameter.Token))
                    return new RequestResult("") { To = EnumTo.Server, Object = null, LongDescription = "Sayfayı görüntüleme için giriş yapmanız gerekmektedir.", Result = EnumRequestResult.Stop, ShortDescription = "Sayfayı görüntüleme için giriş yapmanız gerekmektedir." };

                List<Role> userRoles = operations.GetUserRolesFromUserGuid(parameter.Token);

                bool hasAccess = userRoles.Intersect(categoryRoles).Any();

                if (hasAccess)
                {
                    var obj = operations.GetEntitiesFromCategoryGuid(cat.DBGuid);
                    result.Object = obj;
                }
                else
                {
                    result.Result = EnumRequestResult.Stop;
                    result.ShortDescription = "Yetkiniz yok";
                    result.LongDescription = "Bu kategoriye erişim izniniz bulunmamaktadır.";
                    result.Object = null;
                }
            }
            return result;
        }

        public async Task<IRequestResult> GetAllEntitieViewModel(IRequestParameter parameter = null)
        {
            RequestResult result = new("Get Entities");
            result.Result = EnumRequestResult.Success;

            var list = operations._GetEntitieViewModels();


            result.Object = list;
            return result;
        }

        public async Task<IRequestResult> GetAllMyEntitieViewModel(IRequestParameter parameter)
        {
            string ownerGuid = parameter.Token.ToString();
            RequestResult result = new("Get Entities");
            result.Result = EnumRequestResult.Success;


            if (string.IsNullOrEmpty(ownerGuid))
            {
                result.Result = EnumRequestResult.Error;
                result.ShortDescription = "There is no valid token or you are not logged in.";
                result.LongDescription = "There is no valid token or you are not logged in.";
                return result;
            }

            // TODO : Authorize attribute'sini server tarafında yaptıktan sonra gönderilen ownerGuid ve attribute ile alınan cookie değerindeki userıd'in bir bağlantısı
            // var mı yok mu ona bakacağız ona göre geri döndüreceğiz.
            // Paylaşımlı varlık yapısı için.

            List<string> EntitieGuidList = operations.Context.EntitieOwner
                .Where(xd => xd.NewOwnerGuid == ownerGuid)
                .Select(xd => xd.EntitieGuid)
                .ToList();
            List<Entitie> EntitieList = operations.Context.Entities
                .Where(xd => EntitieGuidList.Contains(xd.DBGuid))
                .ToList();
            List<EntitieViewModel> list = Mapping.Operations.ConvertToList<EntitieViewModel>(EntitieList);


            operations._EntitieViewModelGetInfo(ref list);

            result.Object = list;
            return result;
        }

        public async Task<IRequestResult> GetEntitie(IRequestParameter parameter)
        {
            string EntitieGuid = parameter.Parameters.ToString();
            RequestResult result = new($"Get Entities but Guid {EntitieGuid}");
            result.Result = EnumRequestResult.Success;
            var obj = operations.Context.Entities.FirstOrDefault(xd => xd.DBGuid == EntitieGuid);
            result.Object = obj;
            return result;
        }

        public async Task<IRequestResult> GetEntitieViewModel(IRequestParameter parameter)
        {
            // TODO : Convert.ToInt32 yarınlarımızda küçük kalması nedeniyle sorun çıkartabilir.
            // ama bu gün bunu iplemiyorum çünkü zaten sadece ben kullanıyorum rehabet olursa dön bir bak.
            string EntitieId = parameter.Parameters.ToString().Split("-").Last();
            var entitie = operations.Context.Entities.FirstOrDefault(xd => xd.Id == Convert.ToInt32(EntitieId));
            string Guid = entitie.DBGuid;


            RequestResult result = new("Get Entitie");
            result.Result = EnumRequestResult.Success;


            var returnedobj = operations.GetEntitieViewModelByEntitieDBGuid(Guid); // Serileştirilir
            returnedobj.Transfers.AddRange(operations.GetEntitieTransferList(Guid));
            result.Object = returnedobj;
            return result;// Cavabunga.
        }

        public async Task<IRequestResult> Transfer(IRequestParameter parameter)
        {
            RequestResult result = new("Transfer Object");
            dynamic dn = baseFacades.JsonSerializer.Deserialize<dynamic>(parameter.Parameters.ToString());

            string EntitieGuid = dn["ObjectGuid"];
            string TransferVictimGuid = dn["VictimGuid"];
            string OwnerPassword = dn["Password"];
            string OwnerID = parameter.Token.ToString();

            if (!operations.ObjectOwnedByTheUser(EntitieGuid, OwnerID))
            {
                result.Result = EnumRequestResult.Error;
                result.ShortDescription = "The object is not yours.";
                result.LongDescription = "The object is not yours.";
                return result;
            }

            if (!operations.IsThereSuchAUser(TransferVictimGuid))
            {
                result.Result = EnumRequestResult.Error;
                result.ShortDescription = "There is no such user.";
                result.LongDescription = "There is no such user.";
                return result;
            }

            if (!operations.UserPasswordIsCorrect(OwnerID, OwnerPassword))
            {
                result.Result = EnumRequestResult.Error;
                result.ShortDescription = "Your password is incorrect.";
                result.LongDescription = "Your password is incorrect.";
                return result;
            }

            EntitieOwnerTransfer entitieOwnerTransfer = new()
            {
                EntitieGuid = EntitieGuid,
                NewOwnerGuid = TransferVictimGuid,
                OldOwnerGuid = OwnerID,
                DateTimeUTC = DateTime.UtcNow,
                Hash = "0",
            };

            operations.Context.EntitieOwner.Add(entitieOwnerTransfer);
            operations.Context.SaveChanges();

            result.Result = EnumRequestResult.Success;
            result.ShortDescription = "Operation Success";
            result.LongDescription = "Operation Success";

            return result;
        }
    }
}
