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

        /// <summary>
        ///     Returns a list of email templates
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///     Adds an email template
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        [Authorize]
        public void AddEmailTemplate(string title, string subject, string content)
        {
            _context.AddEmailTemplate(title, subject, content);
        }

        /// <summary>
        ///     Updates an existing email template
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        [Authorize]
        public void UpdateEmailTemplate(int id, string title, string subject, string content)
        {
            _context.UpdateEmailTemplate(id, title, subject, content);
        }

        /// <summary>
        ///     Deletes an existing email template
        /// </summary>
        /// <param name="id"></param>
        [Authorize]
        public void DeleteEmailTemplate(int id)
        {
            _context.DeleteEmailTemplate(id);
        }

        #endregion
    }
}