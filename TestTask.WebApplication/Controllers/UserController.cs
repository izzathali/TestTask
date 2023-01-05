using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.Metrics;
using System.Threading.Channels;
using TestTask.Interface;
using TestTask.Model;

namespace TestTask.WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser iUser;
        private readonly IUserContact iUserContact;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(IUser _iUser, IWebHostEnvironment hostEnvironment, IUserContact iUserContact)
        {
            iUser = _iUser;
            _hostEnvironment = hostEnvironment;
            this.iUserContact = iUserContact;
        }

        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetAllUser()
        {
            var user = await iUser.GetAll();

            return Json(user);
        }
        [HttpGet]
        public async Task<JsonResult> LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data  
                //var customerData = (from tempcustomer in _context.CustomerTB
                //                    select tempcustomer);
                var customerData = await iUser.GetAll();

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.FullName == searchValue);
                }

                //total number of rows count   
                recordsTotal = customerData.Count();
                //Paging   
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            //UserM user = new UserM();
            //user.userContacts.Add(new UserContactM() { UserContactId = 1 });
            //return View(user);

            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("UserId,FullName,Gender,Photo,iPhotoFile")] UserM user)
        {

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(user);

        }

        //Create New User
        [HttpPost]
        public async Task<JsonResult> CreateUser(UserM usr)
        {
            Guid? changes = null;
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
                changes = await iUser.Create(usr);

            }
            catch (Exception ex)
            {
            }

            var result = new { outpt = changes };

            return Json(result);
        }
        //Create User Contact
        [HttpPost]
        public async Task<JsonResult> CreateUserContact(UserContactM usrCon)
        {
            Guid? changes = null;

            try
            {
                //Save new user contact
                changes = await iUserContact.Create(usrCon);
            }
            catch (Exception ex)
            {

            }
            var result = new { res = changes };

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
