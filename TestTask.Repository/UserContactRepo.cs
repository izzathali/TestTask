using Microsoft.EntityFrameworkCore;
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
        public async Task<Guid?> Create(UserContactM t)
        {
            try
            {
                t.CreatedOn = DateTime.Now;
                _db.UserContacts.Add(t);
                await _db.SaveChangesAsync();

                return t.UserConId;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<int> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserContactM>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserContactM>> GetAllByUserId(Guid id)
        {
            return await _db.UserContacts.Where(i => i.IsDeleted == false && i.UserId == id).ToListAsync();
        }

        public Task<UserContactM> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserContactM> GetFirstByUserId(Guid id)
        {
            return await _db.UserContacts.Where(i => i.IsDeleted == false && i.UserId == id).FirstOrDefaultAsync();
        }

        public Task<int> Update(UserContactM t)
        {
            throw new NotImplementedException();
        }
    }
}
