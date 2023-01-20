using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Interface
{
    public interface IImage
    {
        public Task<string?> SaveUserPhoto(string wwwRootPath, string folderPath, IFormFile iPhotoFile);
    }
}
