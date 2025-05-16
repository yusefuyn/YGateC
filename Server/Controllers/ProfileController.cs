using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProfileController : Controller
    {
        IProfileRepository profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            this.profileRepository = profileRepository;
        }

        [HttpPost]
        public IRequestResult GetMyProperties([FromBody] RequestParameter parameter)
        {
            return profileRepository.GetMyProperties(parameter);
        }

        [HttpPost]
        public IRequestResult AddPropertiesToProfile([FromBody] RequestParameter parameter)
        {
            return profileRepository.AddPropertiesToProfile(parameter);

        }

        [HttpPost]
        public IRequestResult GetProfileByUserGuid([FromBody] RequestParameter parameter)
        {
            return profileRepository.GetProfileByUserGuid(parameter);
        }
    }
}
