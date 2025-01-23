using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;

namespace YGate.Entities.ViewModels
{
    public class UserPublicViewModel
    {
        public string ReferanceName { get; set; } = "";
        public string Username { get; set; } = "";
        public List<Role> PublicRoles { get; set; }
        public List<AccountProperties> PublicProperties { get; set; }
        public List<EntitieViewModel> PublicEntities { get; set; }
        public string ProfileMessage { get; set; } = "Profil mesajı gelecek.";
        public string ProfilePicture { get; set; } = "";
        public string Status { get; set; }

    }
}
