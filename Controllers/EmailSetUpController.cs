using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentUnionApp.Controllers
{
    public class EmailSetUpController : Controller
    {
        DatabaseContext _context = new DatabaseContext();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        #region Database Methods

        // returns all email templates
        [Authorize]
        public ActionResult GetEmailTemplates()
        {
            try
            {
                return Json(_context.GetEmailTemplates().ToList().OrderBy(d => d.Title), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // adds a new email template
        [Authorize]
        public void AddEmailTemplate(string title, string subject, string content)
        {
            _context.AddEmailTemplate(title, subject, content);
        }

        // updates an email template
        [Authorize]
        public void UpdateEmailTemplate(int id, string title, string subject, string content)
        {
            _context.UpdateEmailTemplate(id, title, subject, content);
        }

        // deletes an email template
        [Authorize]
        public void DeleteEmailTemplate(int id)
        {
            _context.DeleteEmailTemplate(id);
        }

        #endregion
    }
}