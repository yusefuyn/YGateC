using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Interfaces.Application.Advanced;
using YGate.Json.Operations;
using YGate.Server.Attributes;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class EntitieController : Controller
    {
        Operations operations;
        public EntitieController(Operations operations)
        {
            this.operations = operations;
        }

        #region public

        /// <summary>
        /// Varlık ekleme işi yapar kendine ait EntitieRequestViewModel Objesine sahiptir.
        /// </summary>
        /// <param name="RequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddEntitite([FromBody] RequestParameter parameter)
        {
            EntitieRequestViewModel RequestModel = parameter.ConvertParameters<EntitieRequestViewModel>();
            RequestResult result = new("Add Entitie");
            result.Result = EnumRequestResult.Success;
            #region ModeliDuzenleme
            RequestModel.MainModel.DBGuid = YGate.String.Operations.GuidGen.Generate("Entitie");
            RequestModel.MainModel.Values = RequestModel.MainModel.Values
                .Select(values =>
                {
                    values.EntitieDbGuid = RequestModel.MainModel.DBGuid;
                    values.DBGuid = YGate.String.Operations.GuidGen.Generate("MainEntitieValue");
                    return values;
                })
                .ToList();

            RequestModel.SubModel = RequestModel.SubModel.Select(subModel =>
            {
                subModel.ParentEntitieDBGuid = RequestModel.MainModel.DBGuid; // Ebeveyn objenin dbguid'i geliyor.
                subModel.DBGuid = YGate.String.Operations.GuidGen.Generate("SubEntitie"); // Objenin dbguid'i yeniden atanıyor
                subModel.Values = subModel.Values.Select(subModelValues =>
                { // Values'leri düzenleniyor
                    subModelValues.DBGuid = YGate.String.Operations.GuidGen.Generate("SubEntitieValue"); // valueslere yeni dbguid atanıyor
                    subModelValues.EntitieDbGuid = subModel.DBGuid; // valueslerin ebeveny dbguid'i atanıyor
                    return subModelValues;
                }).ToList();
                return subModel;
            }).ToList();
            #endregion
            string json = YGate.Json.Operations.JsonSerialize.Serialize(RequestModel);
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
                Hash = YGate.String.Operations.Hash.ComputeSHA256("System")
            };
            operations.Context.EntitieOwner.Add(entitieOwnerTransfer);
            #endregion
            operations.Context.SaveChanges();
            result.Object = RequestModel;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        [GetAuthorizeToken]
        public string Transfer([FromBody] RequestParameter parameter)
        {
            RequestResult result = new("Transfer Object");
            dynamic dn = parameter.ConvertParameters<dynamic>();

            string EntitieGuid = dn["ObjectGuid"];
            string TransferVictimGuid = dn["VictimGuid"];
            string OwnerPassword = dn["Password"];
            string OwnerID = parameter.Token.ToString();

            if (!operations.ObjectOwnedByTheUser(EntitieGuid, OwnerID))
            {
                result.Result = EnumRequestResult.Error;
                result.ShortDescription = "The object is not yours.";
                result.LongDescription = "The object is not yours.";
                return YGate.Json.Operations.JsonSerialize.Serialize(result);
            }

            if (!operations.IsThereSuchAUser(TransferVictimGuid))
            {
                result.Result = EnumRequestResult.Error;
                result.ShortDescription = "There is no such user.";
                result.LongDescription = "There is no such user.";
                return YGate.Json.Operations.JsonSerialize.Serialize(result);
            }

            if (!operations.UserPasswordIsCorrect(OwnerID,OwnerPassword))
            {
                result.Result = EnumRequestResult.Error;
                result.ShortDescription = "Your password is incorrect.";
                result.LongDescription = "Your password is incorrect.";
                return YGate.Json.Operations.JsonSerialize.Serialize(result);
            }

            EntitieOwnerTransfer entitieOwnerTransfer = new() {
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

            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }



        /// <summary>
        /// Sadece Guid'i verilen Varlığı alır ve Bir ViewModel olarak döndürür.
        /// Alt üst ilişkisi yapmaz.
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetEntitieViewModel([FromBody] RequestParameter parameter)
        { // TODO : Convert.ToInt32 yarınlarımızda küçük kalması nedeniyle sorun çıkartabilir.
            // ama bu gün bunu iplemiyorum çünkü zaten sadece ben kullanıyorum rehabet olursa dön bir bak.
            string EntitieId = parameter.Parameters.ToString().Split("-").Last();
            var entitie = operations.Context.Entities.FirstOrDefault(xd => xd.Id == Convert.ToInt32(EntitieId));
            string Guid = entitie.DBGuid;


            RequestResult result = new("Get Entitie");
            result.Result = EnumRequestResult.Success;


            var returnedobj = operations.GetEntitieViewModelByEntitieDBGuid(Guid); // Serileştirilir
            returnedobj.Transfers.AddRange(operations.GetEntitieTransferList(Guid));
            result.Object = returnedobj;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);// Cavabunga.
        }



        /// <summary>
        /// Tüm varlıkları çağırır. Kendi içerisinde çağırdığı GetEntitieViewModels Alt Üst ilişkisi yapar.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetAllEntitieViewModel([FromBody] RequestParameter parameter = null)
        {
            RequestResult result = new("Get Entities");
            result.Result = EnumRequestResult.Success;

            var list = operations._GetEntitieViewModels();


            result.Object = list;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }


        /// <summary>
        /// OwnerGuid parametresine ait varlıkları alır ve döndürür.
        /// </summary>
        /// <param name="ownerGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetAllMyEntitieViewModel([FromBody] RequestParameter parameter)
        {

            string ownerGuid = parameter.Parameters.ToString();
            // TODO : Authorize attribute'sini server tarafında yaptıktan sonra gönderilen ownerGuid ve attribute ile alınan cookie değerindeki userıd'in bir bağlantısı
            // var mı yok mu ona bakacağız ona göre geri döndüreceğiz.
            // Paylaşımlı varlık yapısı için.

            RequestResult result = new("Get Entities");
            result.Result = EnumRequestResult.Success;

            List<EntitieViewModel> list = YGate.Mapping.Operations.ConvertToList<EntitieViewModel>(
                operations.Context.Entities.Where(xd => xd.CreatorGuid == ownerGuid).ToList()
                );


            operations._EntitieViewModelGetInfo(ref list);

            result.Object = list;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }


        /// <summary>
        /// CategoryGuid'e ait varlıkları alır.
        /// Category Entitie'lere ait alt elemanları bulur ve ekler.
        /// </summary>
        /// <param name="categoryGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetAllEntitieButCategoryGuid([FromBody] RequestParameter parameter)
        {
            string categoryGuid = parameter.Parameters.ToString();
            RequestResult result = new($"Get Entities but Category Guid {categoryGuid}");
            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            result.Object = operations.GetEntitiesFromCategoryGuid(categoryGuid);
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }



        /// <summary>
        /// Seo'ya uygun çalışma için yapılmıştır temel GetAllEntitieButCategoryGuid çalıştırmaktadır.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [GetAuthorizeToken]
        public string GetAllEntitieButCategoryId([FromBody] RequestParameter parameter)
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
                return YGate.Json.Operations.JsonSerialize.Serialize(new RequestResult("") { To = EnumTo.Server, Object = null, LongDescription = "Verilen kategori ID'sine ait kategori bulunamadı.", Result = EnumRequestResult.Stop, ShortDescription = "Kategori bulunamadı" });

            List<Role> categoryRoles = operations.GetCategoryRoleFromCategoryGuid(cat.DBGuid);

            if (categoryRoles.Any(xd => xd.Name == "All"))
            {
                var obj = operations.GetEntitiesFromCategoryGuid(cat.DBGuid);
                result.Object = obj;
            }
            else
            {
                if (string.IsNullOrEmpty(parameter.Token))
                    return YGate.Json.Operations.JsonSerialize.Serialize(new RequestResult("") { To = EnumTo.Server, Object = null, LongDescription = "Sayfayı görüntüleme için giriş yapmanız gerekmektedir.", Result = EnumRequestResult.Stop, ShortDescription = "Sayfayı görüntüleme için giriş yapmanız gerekmektedir." });

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
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }




        /// <summary>
        /// Sadece varlık sınıfı döndürür.
        /// </summary>
        /// <param name="EntitieGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetEntitie([FromBody] RequestParameter parameter)
        {
            string EntitieGuid = parameter.Parameters.ToString();
            RequestResult result = new($"Get Entities but Guid {EntitieGuid}");
            result.Result = EnumRequestResult.Success;
            var obj = operations.Context.Entities.FirstOrDefault(xd => xd.DBGuid == EntitieGuid);
            result.Object = obj;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }


        /// <summary>
        /// Varlıkların alt ilişiğindeki diğer varlıkları ve onların Özelliklerinide siler bunu yaparken transaction açıktır
        /// hata alırsa işlemi geri alır.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete([FromBody] RequestParameter parameter)
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
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        #endregion

    }
}
