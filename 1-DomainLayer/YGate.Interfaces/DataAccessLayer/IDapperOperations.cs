using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Interfaces.DataAccessLayer
{
    public interface IGeneralOperations<T>
        where T : class, IIndexable, new()
    {
        public Task<List<T>> GetAll();
        public Task<List<T>> Gets(List<string> ids);
        public Task<List<T>> Gets(List<int> dbindexs);
        public Task<T> Get(string id);
        public Task<T> Insert(T entity);
        public Task<T> Update(T entity);
        public Task<T> Delete(string id);
        public Task<T> Deletes(List<string> ids);
        public Task<T> GetCustomQuery(string query, object value);
    }
}

