using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Interface
{
    public interface ICrud<T>
    {
        public Task<int> Create(T t);
        public Task<int> Update(T t);
        public Task<int> Delete(object id);
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(int id);

    }
}
