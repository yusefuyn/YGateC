using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;

namespace YGate.Entities.ViewModels
{
    public class AdministratorUsersList
    {
        public Account Accounts { get; set; }
        public List<AccountProperties> AccountProperties { get; set; }
        public List<AccountPasswords> AccountPasswords { get; set; }
        public List<Role> AssignableRoles { get; set; }// Atanabilir roller.
    }
}
