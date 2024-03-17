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

        [HttpPost]
        public void SendFeedback(string name, string email, string message)
        {
            MailSender mailSender = new MailSender();
            mailSender.FeedbackEmail(name, email, message);
        }
    }
}