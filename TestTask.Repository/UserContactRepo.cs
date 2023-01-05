using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Data;
using TestTask.Interface;
using TestTask.Model;

namespace TestTask.Repository
{
    public class UserContactRepo : IUserContact
    {
        private readonly TestTaskDbContext _db;
        public UserContactRepo(TestTaskDbContext db)
        {
            _db = db;
        }
        //Add User Contact
        public async Task<int> Create(UserContactM t)
        {
            try
            {
                t.CreatedOn = DateTime.Now;
                _db.UserContacts.Add(t);
                return await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Task<int> Delete(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserContactM>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserContactM> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(UserContactM t)
        {
            throw new NotImplementedException();
        }
    }
}
