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
using static TestTask.WebApplication.Controllers.Enum;

namespace TestTask.WebApplication.Controllers
{
    public class UserController : BaseController
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
        public ActionResult Index(int p = 1, int s = 10)
        {
            var users = iUser.Users(p, s);
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
                //Default photo path
                usr.Photo = "bk1234357618.jpg";

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
                        Alert("User has successfully saved", NotificationType.success);
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        Alert("Something went wrong!", NotificationType.error);
                        return View(usr);
                    }
                }
                else
                {
                    Alert("Input property wrong!", NotificationType.error);
                    return View(usr);
                }

            }
            catch (Exception ex)
            {
                Alert("Something went wrong!", NotificationType.error);
                return View(usr);

            }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserM user)
        {
            int changes = 0;

            try
            {
                if (ModelState.IsValid)
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

                    if (changes > 0 )
                    {
                        Alert("User has successfully Updated", NotificationType.success);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Alert("Something went wrong!", NotificationType.error);
                        return View(user);
                    }
                }
                else
                {
                    Alert("Something went wrong!", NotificationType.error);
                    return View(user);
                }

            }
            catch (Exception ex)
            {
                Alert("Something went wrong!", NotificationType.error);
                return View(user);
            }

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
