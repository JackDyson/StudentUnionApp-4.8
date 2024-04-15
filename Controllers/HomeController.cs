using Microsoft.Ajax.Utilities;
using StudentUnionApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
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

        /// <summary>
        ///     Log the user in and redirect to the home page if successful
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Log the user out and redirect to the login page
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        /// <summary>
        ///     Return if the password needs updating for the given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult CheckPasswordReset(string email)
        {
            if (CheckIfPasswordNeedsUpdating(email))
            {
                return Json(new { success = true, responseText = "Password needs updating" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "Password does not need updating" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///    Update the password for the given email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult UpdatePassword(string email, string password)
        {
            _context.UpdatePassword(email, PasswordHelper.HashPassword(password));
            return Json(new { success = true, responseText = "Password updated" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Check if the user credentials are valid
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool CheckUserCredentials(string email, string password)
        {
            // This method returns true if credentials are valid, false otherwise
            var staff = _context.GetStaff().ToList();
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

        /// <summary>
        ///     Check if the password reset flag is set for the given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool CheckIfPasswordNeedsUpdating(string email)
        {
            var staff = _context.Staff.ToList();
            foreach (var s in staff)
            {
                if (s.Email == email)
                {
                    if (s.Reset_Password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

    }
}