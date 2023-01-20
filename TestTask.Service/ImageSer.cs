using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Interface;

namespace TestTask.Service
{
    public class ImageSer :IImage
    {
        public ImageSer()
        {

        }
        //Save user photo 
        public async Task<string?> SaveUserPhoto(string wwwRootPath, string folderPath, IFormFile iPhotoFile)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(iPhotoFile.FileName);
                string extension = Path.GetExtension(iPhotoFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + folderPath, fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await iPhotoFile.CopyToAsync(fileStream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
