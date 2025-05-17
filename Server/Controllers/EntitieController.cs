using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Interfaces.Application.Advanced;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Json.Operations;
using YGate.Server.Attributes;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class EntitieController : Controller
    {

        IEntitieRepository entitieRepository;

        public EntitieController(IEntitieRepository entitieRepository)
        {
            this.entitieRepository = entitieRepository;
        }


        #region public

        /// <summary>
        /// Varlık ekleme işi yapar kendine ait EntitieRequestViewModel Objesine sahiptir.
        /// </summary>
        /// <param name="RequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IRequestResult> AddEntitite([FromBody] RequestParameter parameter)
        {
            return await entitieRepository.AddEntitite(parameter);
        }

        [HttpPost]
        [GetAuthorizeToken]
        public async Task<IRequestResult> Transfer([FromBody] RequestParameter parameter)
        {
            return await entitieRepository.Transfer(parameter);
        }



        /// <summary>
        /// Sadece Guid'i verilen Varlığı alır ve Bir ViewModel olarak döndürür.
        /// Alt üst ilişkisi yapmaz.
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IRequestResult> GetEntitieViewModel([FromBody] RequestParameter parameter)
        {
            return await entitieRepository.GetEntitieViewModel(parameter);

        }



        /// <summary>
        /// Tüm varlıkları çağırır. Kendi içerisinde çağırdığı GetEntitieViewModels Alt Üst ilişkisi yapar.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IRequestResult> GetAllEntitieViewModel([FromBody] RequestParameter parameter = null)
        {
            return await entitieRepository.GetAllEntitieViewModel(parameter);

        }


        /// <summary>
        /// OwnerGuid parametresine ait varlıkları alır ve döndürür.
        /// </summary>
        /// <param name="ownerGuid"></param>
        /// <returns></returns>
        [HttpPost]
        [GetAuthorizeToken]
        public async Task<IRequestResult> GetAllMyEntitieViewModel([FromBody] RequestParameter parameter)
        {

            return await entitieRepository.GetAllMyEntitieViewModel(parameter);

        }


        /// <summary>
        /// CategoryGuid'e ait varlıkları alır.
        /// Category Entitie'lere ait alt elemanları bulur ve ekler.
        /// </summary>
        /// <param name="categoryGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IRequestResult> GetAllEntitieButCategoryGuid([FromBody] RequestParameter parameter)
        {
            return await entitieRepository.GetAllEntitieButCategoryGuid(parameter);
        }



        /// <summary>
        /// Seo'ya uygun çalışma için yapılmıştır temel GetAllEntitieButCategoryGuid çalıştırmaktadır.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [GetAuthorizeToken]
        public async Task<IRequestResult> GetAllEntitieButCategoryId([FromBody] RequestParameter parameter)
        {

            return await entitieRepository.GetAllEntitieButCategoryId(parameter);

        }




        /// <summary>
        /// Sadece varlık sınıfı döndürür.
        /// </summary>
        /// <param name="EntitieGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IRequestResult> GetEntitie([FromBody] RequestParameter parameter)
        {
            return await entitieRepository.GetEntitie(parameter);
        }


        /// <summary>
        /// Varlıkların alt ilişiğindeki diğer varlıkları ve onların Özelliklerinide siler bunu yaparken transaction açıktır
        /// hata alırsa işlemi geri alır.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IRequestResult> Delete([FromBody] RequestParameter parameter)
        {
            return await entitieRepository.Delete(parameter);
        }

        #endregion

    }
}
