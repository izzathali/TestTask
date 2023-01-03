using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Model;

namespace TestTask.Data
{
    public class TestTaskDbContext : DbContext
    {
        public TestTaskDbContext(DbContextOptions<TestTaskDbContext> options) : base(options)
        {

        }

        public DbSet<UserM> Users { get; set; }
        public DbSet<UserContactM> UserContacts { get; set; }
    }
}
