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

        /// <summary>
        ///     Returns a list of all staff
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///     Creates a new staff member in the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        [Authorize]
        public void AddStaff(string name, string email, string role)
        {
            _context.AddStaff(name, email, "password", role);
        }

        /// <summary>
        ///     Updates an existing staff member in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="role"></param>
        /// <param name="passwordReset"></param>
        [Authorize]
        public void UpdateStaff(int id, string name, string role, bool passwordReset)
        {
            _context.UpdateStaff(id, name, role, passwordReset);
        }

        

        /// <summary>
        ///     Deletes an existing staff member from the database
        /// </summary>
        /// <param name="id"></param>
        [Authorize]
        public void DeleteStaff(int id)
        {
            _context.DeleteStaff(id);
        }

        #endregion
    }
}