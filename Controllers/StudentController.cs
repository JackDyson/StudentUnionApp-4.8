using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentUnionApp.Controllers
{
    public class StudentController : Controller
    {
        DatabaseContext _context = new DatabaseContext();

        #region Views

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Database Methods

        [Authorize]
        // returns all students
        public ActionResult GetStudents()
        {
            try
            {
                return Json(_context.GetStudents(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // adds a new student
        [Authorize]
        public void AddStudentstring(string clubName, string position, string studentName, string preferredName, string phoneNumber, string emailAddress, bool agreementSigned, bool trainingComplete, bool membershipPurchased, bool foodCertified)
        {
            _context.AddStudent(clubName, position, studentName, preferredName, phoneNumber, emailAddress, agreementSigned, trainingComplete, membershipPurchased, foodCertified);
        }

        // updates a student
        [Authorize]
        public void UpdateStudent(int id, string clubName, string position, string studentName, string preferredName, string phoneNumber, string emailAddress, bool agreementSigned, bool trainingComplete, bool membershipPurchased, bool foodCertified)
        {
            _context.UpdateStudent(id, clubName, position, studentName, preferredName, phoneNumber, emailAddress, agreementSigned, trainingComplete, membershipPurchased, foodCertified);
        }

        // deletes a student
        [Authorize]
        public void DeleteStudent(int id)
        {
            _context.DeleteStudent(id);
        }

        // get society list
        [Authorize]
        public ActionResult GetSocietyList()
        {
            try
            {
                return Json(_context.GetSocieties(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // get position list
        [Authorize]
        public ActionResult GetPositionList()
        {
            try
            {
                return Json(_context.GetPositions(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}