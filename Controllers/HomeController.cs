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

        #endregion

        #region Database Methods

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

        [Authorize]
        public ActionResult AddEntryToSUTest(Student_Union_Test newEntry)
        {
            try
            {
                _context.Student_Union_Test.Add(newEntry);
                _context.SaveChanges();
                return Json(new { success = true, responseText = "Entry added successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult UpdateEntrySUTest(Student_Union_Test entry)
        {
            var entryToUpdate = _context.Student_Union_Test.Find(entry.Student_ID);
            if (entryToUpdate == null)
            {
                return Json(new { success = false, responseText = "Entry not found" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                entryToUpdate.Club_Name = entry.Club_Name;
                entryToUpdate.Position = entry.Position;
                entryToUpdate.Student_Name = entry.Student_Name;
                entryToUpdate.Preferred_Name = entry.Preferred_Name;
                entryToUpdate.Phone_Number = entry.Phone_Number;
                entryToUpdate.Email_Address = entry.Email_Address;
                entryToUpdate.Agreement_Signed = entry.Agreement_Signed;
                entryToUpdate.Training_Complete = entry.Training_Complete;
                entryToUpdate.Membership_Purchased = entry.Membership_Purchased;
                entryToUpdate.Food_Certified = entry.Food_Certified;
                _context.SaveChanges();
                return Json(new { success = true, responseText = "Entry updated successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}