using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.Metrics;
using TestTask.Interface;
using TestTask.Model;

namespace TestTask.WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser iUser;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(IUser _iUser, IWebHostEnvironment hostEnvironment)
        {
            iUser = _iUser;
            _hostEnvironment = hostEnvironment;
        }

        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            UserM user = new UserM();
            user.userContacts.Add(new UserContactM() { UserContactId = 1 });

            return View(user);
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("UserId,FullName,Gender,Photo,iPhotoFile")] UserM user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await iUser.Create(user);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View(user);
            }

            return View(user);
        }
        //Create New User
        [HttpPost]
        //public async Task<JsonResult> CreateUser(string FullName, string Password, IFormFile iPhotoFile)
        public async Task<JsonResult> CreateUser(UserM usr)
        {
            try
            {
                //Save image to wwwroot/user photo
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (usr.iPhotoFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(usr.iPhotoFile.FileName);
                    string extension = Path.GetExtension(usr.iPhotoFile.FileName);
                    usr.Photo = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/UserPicture/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await usr.iPhotoFile.CopyToAsync(fileStream);
                    }
                }

                //Save new user
                await iUser.Create(usr);

            }
            catch (Exception ex)
            {

            }

            var result = new { outpt = 1 };


            return Json(result);
        }
        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
