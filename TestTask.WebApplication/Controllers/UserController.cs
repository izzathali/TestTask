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
        public async Task<ActionResult> Index()
        {

            return View(await iUser.GetAll());
        }
        // GET: UserController
        public ActionResult IndexGrid()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][FullName]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all data  
                var customerData = iUser.Users;

                //Sorting  
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}

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
            catch (Exception)
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

        //Create New User
        [HttpPost]
        public async Task<JsonResult> CreateUser(UserM usr)
        {
            Guid? user_id = null;
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
                user_id = await iUser.Create(usr);

            }
            catch (Exception ex)
            {
            }

            var result = new { inserted_user_id = user_id };

            return Json(result);
        }
        //Create User Contact
        [HttpPost]
        public async Task<JsonResult> CreateUserContact(UserContactM usrCon)
        {
            Guid? user_contact_id = null;

            try
            {
                //Save new user contact
                user_contact_id = await iUserContact.Create(usrCon);
            }
            catch (Exception ex)
            {

            }
            var result = new { inserted_user_con_id = user_contact_id };

            return Json(result);
        }
        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            try
            {
                //Get User by user id
                var user = await iUser.GetById(id);
                //Get user contact by user id
                var userCont = await iUserContact.GetFirstByUserId(id);

                if (user == null)
                {
                    return RedirectToAction("PageNotFound", "Error");
                }

                ViewBag.UserContact = userCont;

                return View(user);
            }
            catch (Exception ex)
            {
                return View();
            }
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
        //Delete User
        [HttpDelete]
        public async Task<JsonResult> Delete(Guid UserId)
        {
            int changes = 0;

            try
            {
                changes = await iUser.Delete(UserId);
            }
            catch (Exception ex)
            {

            }

            var result = new { changes = changes };
            return Json(result);
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
