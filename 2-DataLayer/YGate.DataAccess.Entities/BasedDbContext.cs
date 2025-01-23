using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;

namespace YGate.DataAccess.Entities
{


    /// <summary>
    /// OnModelCreating buradan devralınıyor dbcontext direk kullanılmamalıdır kullanılırsa db düzgün yapılandırılmayabilir.
    /// </summary>


    // Daha sonra gerekirse [Index] leme yap. Performans testleri lazım.

    public class BasedDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
