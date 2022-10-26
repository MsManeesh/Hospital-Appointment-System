using Hospital_Appointment.DAL;
using Hospital_Appointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospital_Appointment.Controllers
{
    
    public class UserController : Controller
    {
        // GET: User
        UserDbHandler userDb = new UserDbHandler();
        
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        
        // GET: User/Create
        public ActionResult Create()
        {
            return View();

        }

        // POST: User/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {

            try
            {
                
                if (ModelState.IsValid)
                {
                    User userExist = userDb.GetSingleUserDetails(user.Email);
                    if (userExist == null)
                    {
                        if (userDb.AddUser(user))
                        {
                            ModelState.Clear();
                            return RedirectToAction("Index");
                        }
                        ModelState.AddModelError("", "Server Error");
                    }
                    ModelState.AddModelError("", "Email Already Exist");
                }
                return View(user);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " " + ex.StackTrace);
                return View(user);
            }

        }

        // GET: User/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {

            User user = userDb.GetUser(id);
            if (user != null)
            {
                
                return View(user);
            }
                
            return RedirectToAction("Index");
        }

        // POST: User/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    if (userDb.UpdateUser(user))
                    {
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Server Error");
                }
                return View(user);


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " " + ex.StackTrace);
                return View(user);
            }
        }

        // GET: User/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                return View(userDb.GetUser(id));
            }
            catch (Exception ex)
            {

                TempData["error"] = "<script>alert(" + ex.Message + " " + ex.StackTrace + ")</script>";
                return RedirectToAction("index");
            }

        }

        // POST: User/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, User user)
        {
            try
            {
                if (userDb.DeleteUser(id))
                {
                    TempData["msg"] = "<script>alert('User Deleted Successfully')</script>";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Unadle to Delete");
                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " " + ex.StackTrace);
                return View();
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult UsersData()
        {
            try
            {
                var start = Convert.ToInt32(Request.Form["start"]);
                var length = Convert.ToInt32(Request.Form["length"]);
                var searchValue = Convert.ToString(Request.Form["search[value]"]);
                var sortColumnDirection = Request.Form["order[0][dir]"];
                var sortColumn = Request.Form["order[0][column]"];
                var count = userDb.GetUserCount();
                if (length < 0)
                    length = count;
                List<User> users = userDb.GetUsers(start, length, searchValue, sortColumn, sortColumnDirection);
                return Json(new { data = users, recordsTotal = count, recordsFiltered = count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return Json(null);
            }
        }
    }
}