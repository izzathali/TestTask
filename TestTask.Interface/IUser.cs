using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Model;

namespace TestTask.Interface
{
    public interface IUser : ICrud<UserM>
    {
        IQueryable<UserM> Users { get; }
    }
}
