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

        public IQueryable<UserM> Users => (from tempUser in _db.Users select tempUser);

        //Add User
        public async Task<Guid?> Create(UserM t)
        {
            try
            {
                t.CreatedOn = DateTime.Now;
                _db.Users.Add(t);
                await _db.SaveChangesAsync();

                return t.UserId;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //Delete User
        public async Task<int> Delete(Guid id)
        {
            try
            {
                var user = await _db.Users.FindAsync(id);

                if (user != null)
                {
                    user.IsDeleted = true;
                    _db.Users.Update(user);
                }

                return await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        //Get all Users
        public async Task<IEnumerable<UserM>> GetAll()
        {
            return await _db.Users.Where(i => i.IsDeleted == false).ToListAsync();
        }
        //Get user by id
        public async Task<UserM> GetById(Guid id)
        {
            return await _db.Users.Where(i => i.IsDeleted == false && i.UserId == id).FirstOrDefaultAsync();
        }

        //Update user
        public Task<int> Update(UserM t)
        {
            throw new NotImplementedException();
        }
    }
}
