using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentUnionApp.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: Feedback
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     Sends feedback email to the admin
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="message"></param>
        [HttpPost]
        [Authorize]
        public void SendFeedback(string name, string email, string message)
        {
            MailSender mailSender = new MailSender();
            mailSender.FeedbackEmail(name, email, StringHelper.ReplaceNewLineWithBr(message));
        }
    }
}