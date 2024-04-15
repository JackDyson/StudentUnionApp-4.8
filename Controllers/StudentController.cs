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

        /// <summary>
        ///     Returns a list of students
        /// </summary>
        /// <returns></returns>
        [Authorize]
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

        /// <summary>
        ///     Adds a student to the database
        /// </summary>
        /// <param name="clubName"></param>
        /// <param name="position"></param>
        /// <param name="studentName"></param>
        /// <param name="preferredName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="emailAddress"></param>
        /// <param name="agreementSigned"></param>
        /// <param name="trainingComplete"></param>
        /// <param name="membershipPurchased"></param>
        /// <param name="foodCertified"></param>
        [Authorize]
        public void AddStudent(string clubName, string position, string studentName, string preferredName, string phoneNumber, string emailAddress, bool agreementSigned, bool trainingComplete, bool membershipPurchased, bool foodCertified)
        {
            _context.AddStudent(clubName, position, studentName, preferredName, phoneNumber, emailAddress, agreementSigned, trainingComplete, membershipPurchased, foodCertified);
        }

        /// <summary>
        ///     Updates an existing student in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="clubName"></param>
        /// <param name="position"></param>
        /// <param name="studentName"></param>
        /// <param name="preferredName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="emailAddress"></param>
        /// <param name="agreementSigned"></param>
        /// <param name="trainingComplete"></param>
        /// <param name="membershipPurchased"></param>
        /// <param name="foodCertified"></param>
        [Authorize]
        public void UpdateStudent(int id, string clubName, string position, string studentName, string preferredName, string phoneNumber, string emailAddress, bool agreementSigned, bool trainingComplete, bool membershipPurchased, bool foodCertified)
        {
            _context.UpdateStudent(id, clubName, position, studentName, preferredName, phoneNumber, emailAddress, agreementSigned, trainingComplete, membershipPurchased, foodCertified);
        }

        /// <summary>
        ///     Deletes an existing student from the database
        /// </summary>
        /// <param name="id"></param>
        [Authorize]
        public void DeleteStudent(int id)
        {
            _context.DeleteStudent(id);
        }

        /// <summary>
        ///     Gets a list of societies
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///     Gets a list of positions
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///     Gets a list of email templates
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///     Sends an email to a list of students using a template
        /// </summary>
        /// <param name="templateID"></param>
        /// <param name="students"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Using an uploaded file, processes the file and adds students to the database
        /// </summary>
        /// <param name="file">Excel file</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult UploadStudents(string file)
        {
            return Json(new Upload().ProcessBase64File(file), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Downloads the student upload template
        /// </summary>
        /// <returns></returns>
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