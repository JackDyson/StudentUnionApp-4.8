using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace StudentUnionApp
{
    public class MailSender
    {
        #region Global Variables

        private readonly DatabaseContext _Context;
        private readonly SmtpClient _Smtp;

        #endregion

        #region Constructor

        /// <summary>
        ///     Constructor for Mail Sender class
        /// </summary>
        public MailSender()
        {
            _Context = new DatabaseContext();
            _Smtp = new SmtpClient()
            {
                Host = "smtp.ionos.co.uk",
                Port = 25,
                EnableSsl = false,
                Credentials = new System.Net.NetworkCredential("info@sutest.co.uk", "uaW4ATuZRUcB")
            };
        }


        #endregion

        #region Emails

        /// <summary>
        ///     Send a feedback email to the support team (me)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="message"></param>
        public void FeedbackEmail(string name, string email, string message)
        {
            try
            {
                MailAddress to = new MailAddress("jack.c.dyson@gmail.com");
                MailAddress from = new MailAddress("info@sutest.co.uk");
                MailMessage mail = new MailMessage(from, to)
                {
                    Subject = $"Feedback from {name}",
                    IsBodyHtml = true,
                    Body = $"Name: {name} <br> Email: {email} <br> Message: {message} <br>"
                };
                _Smtp.Send(mail);

                try
                {
                    // Send a confirmation email to the user
                    MailAddress toUser = new MailAddress(email);
                    MailMessage mailUser = new MailMessage(from, toUser)
                    {
                        Subject = $"Feedback Confirmation",
                        IsBodyHtml = true,
                        Body = $"Thank you for your feedback. We will be in touch soon. <br>"
                    };
                }
                catch (Exception ex)
                {
                    _Context.AddErrorLog("MailSender", "FeedbackEmail", $"Sending to person who submitted feedback: {ex.InnerException.Message}");
                }

            }
            catch (Exception ex)
            {
                _Context.AddErrorLog("MailSender", "FeedbackEmail", $"Sending to jack.c.dyson: {ex.InnerException.Message}");
            }
        }

        /// <summary>
        ///    Send an email to a student from a template
        /// </summary>
        /// <param name="studemt"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool SendEmailFromTemplate(Students studemt, string subject, string body)
        {
            var content = ReplaceEmailTemplate(body, studemt.Student_Name, studemt.Preferred_Name, studemt.Club_Name, studemt.Position);
            try
            {
                MailAddress from = new MailAddress("info@sutest.co.uk");
                MailAddress toAddress = new MailAddress(studemt.Email_Address);
                MailMessage mail = new MailMessage(from, toAddress)
                {
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = StringHelper.ReplaceNewLineWithBr(content)
                };
                _Smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                _Context.AddErrorLog("MailSender", "SendEmailFromTemplate", ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     Replaces multiple instances of [Name], [Prefferred_Name] [Club] and [Position] with the appropriate values
        /// </summary>
        /// <param name="template"></param>
        /// <param name="name"></param>
        /// <param name="preferredName"></param>
        /// <param name="club"></param>
        /// <param name="position"></param>
        /// <returns>The body of text with the replaced values</returns>
        public string ReplaceEmailTemplate(string template, string name, string preferredName, string club, string position)
        {
            template = template.Replace("[Name]", name);
            template = template.Replace("[Preferred_Name]", preferredName);
            template = template.Replace("[Club]", club);
            template = template.Replace("[Position]", position);
            return template;
        }

        #endregion
    }
}