using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.Metrics;
using TestTask.Interface;
using TestTask.Model;

namespace TestTask.WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser iUser;
        public UserController(IUser _iUser)
        {
            iUser = _iUser;
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
            return View();
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
        public JsonResult CreateUser(string FullName, string Password)
        {
            var result = new { FullName = FullName, Pass = Password };


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
