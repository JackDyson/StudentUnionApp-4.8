using StudentUnionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentUnionApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Perform login operation
                // For example, check the user's credentials against a database
                bool isValidUser = CheckUserCredentials(model.Email, model.Password);

                if (isValidUser)
                {
                    // Redirect to another page or perform other actions upon successful login
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
            // Placeholder for actual authentication logic
            // This method should return true if credentials are valid, false otherwise
            return false; // Always return false in this placeholder method
        }
    }
}