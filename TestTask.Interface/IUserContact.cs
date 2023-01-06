using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Model;

namespace TestTask.Interface
{
    public interface IUserContact : ICrud<UserContactM>
    {
        public Task<UserContactM> GetFirstByUserId(Guid id);
        public Task<List<UserContactM>> GetAllByUserId(Guid id);
    }
}
