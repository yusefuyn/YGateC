using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Interfaces.DataAccessLayer
{
    public interface IEFCoreOperations<T> 
        where T : class, IIndexable,new()
    {
        public List<T> GetAll();
        public List<T> Gets(Func<T, bool> predicate);
        public T Get(Func<T, bool> predicate);
        public bool Delete(T tentity);
        public bool Add(T tentity);
    }
}
