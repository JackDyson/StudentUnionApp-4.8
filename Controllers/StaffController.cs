using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace StudentUnionApp.Controllers
{
    public class StaffController : Controller
    {
        DatabaseContext _context = new DatabaseContext();
        // GET: Staff
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        #region Database Methods

        // returns all staff
        [Authorize]
        public ActionResult GetStaff()
        {
            try
            {
                return Json(_context.GetStaff().ToList().OrderBy(d => d.Name), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // adds a new staff
        [Authorize]
        public void AddStaff(string name, string email, string password, string role)
        {
            _context.AddStaff(name, email, PasswordHelper.HashPassword(password), role);
        }

        // updates a staff
        [Authorize]
        public void UpdateStaff(int id, string name, string email, string password, string role)
        {
            _context.UpdateStaff(id, name, email, PasswordHelper.HashPassword(password), role);
        }

        // deletes a staff
        [Authorize]
        public void DeleteStaff(int id)
        {
            _context.DeleteStaff(id);
        }

        #endregion
    }
}