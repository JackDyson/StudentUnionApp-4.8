using System;
using System.Collections.Generic;
using System.IO;
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
        public void AddStudent(string clubName, string position, string studentName, string preferredName, string phoneNumber, string emailAddress, bool agreementSigned, bool trainingComplete, bool membershipPurchased, bool foodCertified)
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

        // get email templates
        [Authorize]
        public ActionResult GetEmailTemplates()
        {
            try
            {
                return Json(_context.GetEmailTemplates(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // send email to multiple students
        [Authorize]
        public ActionResult SendEmailToStudents(int templateID, List<Students> students)
        {
            bool success = true;
            // get email template
            Email_Templates emailTemplate = _context.GetEmailTemplate(templateID);                
            MailSender emailSender = new MailSender();
            // send email to each student
            foreach (Students student in students)
            {
                if (!emailSender.SendEmailFromTemplate(student, emailTemplate.Subject, emailTemplate.Email_Content))
                {
                    success = false;
                }
            }
            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }

        // upload students from excel
        [Authorize]
        public ActionResult UploadStudents(string file)
        {
            return Json(new Upload().ProcessBase64File(file), JsonRequestBehavior.AllowGet);
        }

        // download template excel file
        [Authorize]
        public ActionResult DownloadTemplate()
        {
            string filePath = HttpContext.Server.MapPath("~/UploadTemplates/HSU-Upload-Template.xlsx");

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound();
            }

            // Return the file as a FileStreamResult
            return File(new FileStream(filePath, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "HSU-Upload-Template.xlsx");
        }

        #endregion
    }
}