using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Application.Advanced;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class Account : IAccount, IShowedIndexable
    {
        public Account()
        {
            DBGuid = YGate.String.Operations.GuidGen.Generate("Account");
            IsActive = true;
            Status = AccountStatus.NotVerified;
            Roles = new();
        }

        [Key]
        public int Id { get; set; }
        [DisplayName("Hesap Anahtarı")]
        public string DBGuid { get; set; }
        [DisplayName("Görüntüleme Sırası")]
        public int ShowIndex { get; set; }
        [DisplayName("Aktif")]
        public bool IsActive { get; set; }
        [DisplayName("Sahibi")]
        public string OwnerGuid { get; set; }
        [DisplayName("Şifrenin Sha256 algoritma İmzası")]
        public string Password { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Kullanıcı Adı")]
        public string Username { get; set; }


        [NotMapped]
        public List<Role> Roles { get; set; } = new();

        [DefaultValue(AccountStatus.NotVerified)]
        public AccountStatus Status { get; set; }
    }

    public enum AccountStatus { 
        NotVerified,
        Verified,
        Banned
    }
}
