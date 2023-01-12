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
        public ActionResult Index(int p = 1)
        {
            var users = iUser.Users(p);
            return View(users);
        }
        // GET: UserController/Create
        public ActionResult Create()
        {
            try
            {
                UserM user = new UserM();
                user.userContacts.Add(new UserContactM()
                {
                    UserConId = Guid.NewGuid()
                });
                return View(user);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserM usr)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Guid? user_id = null;

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

                    if (user_id != null)
                    {
                        TempData["userid"] = user_id;
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        TempData["msg"] = "error";
                        return View(usr);
                    }
                }


            }
            catch (Exception ex)
            {

            }


            return View(usr);
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

        //Update User
        [HttpPut]
        public async Task<JsonResult> UpdateUser(UserM user)
        {
            int changes = 0;

            try
            {
                if (user.iPhotoFile != null)
                {
                    //Save image to wwwroot/user photo
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    if (user.iPhotoFile != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(user.iPhotoFile.FileName);
                        string extension = Path.GetExtension(user.iPhotoFile.FileName);
                        user.Photo = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/UserPicture/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await user.iPhotoFile.CopyToAsync(fileStream);
                        }
                    }
                }
                changes = await iUser.Update(user);
            }
            catch (Exception ex)
            {

            }
            var result = new { changes = changes };

            return Json(result);
        }
        //Update User
        [HttpPut]
        public async Task<JsonResult> UpdateUserContact(UserContactM userCon)
        {
            int changes = 0;

            try
            {

                changes = await iUserContact.Update(userCon);
            }
            catch (Exception ex)
            {

            }
            var result = new { changes = changes };

            return Json(result);
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
                var chnges_user_cont = await iUserContact.DeleteByUserId(UserId);
            }
            catch (Exception ex)
            {

            }

            var result = new { changes = changes };
            return Json(result);
        }

    }
}
