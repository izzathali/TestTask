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
                    user.LastModifiedOn = DateTime.Now;
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
            try
            {
                return await _db.Users.Where(i => i.IsDeleted == false).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<UserM>();
            }
        }
        //Get user by id
        public async Task<UserM> GetById(Guid id)
        {
            try
            {
                return await _db.Users.Where(i => i.IsDeleted == false && i.UserId == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return new UserM();
            }
        }

        //Update user
        public async Task<int> Update(UserM t)
        {
            try
            {
                if (t != null) {

                    t.LastModifiedOn = DateTime.Now;
                    _db.Users.Update(t);
                }

                return await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public PagedResult<UserM> Users(int p,int rows)
        {
            return this._db.Users.Where(i => i.IsDeleted == false).GetPaged(p, rows);
        }
    }
}
