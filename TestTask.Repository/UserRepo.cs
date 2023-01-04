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
    public class UserRepo : IUser
    {
        private readonly TestTaskDbContext _db;

        public UserRepo(TestTaskDbContext db)
        {
            _db = db;
        }
        //Add User
        public async Task<int> Create(UserM t)
        {
            try
            {
                t.CreatedOn = DateTime.Now;
                _db.Users.Add(t);
                return await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        //Delete User
        public Task<int> Delete(object id)
        {
            throw new NotImplementedException();
        }
        //Get all Users
        public async Task<IEnumerable<UserM>> GetAll()
        {
            return await _db.Users.Where(i => i.IsDeleted == false).ToListAsync();
        }
        //Get user by id
        public Task<UserM> GetById(int id)
        {
            throw new NotImplementedException();
        }
        //Update user
        public Task<int> Update(UserM t)
        {
            throw new NotImplementedException();
        }
    }
}
