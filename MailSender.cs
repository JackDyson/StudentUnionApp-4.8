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
            #if DEBUG
            _Smtp = new SmtpClient()
            {
                Host = "mailcluster.protocall.local",
                Port = 25,
                EnableSsl = false
            };
            #else
            _Smtp = new SmtpClient()
            {
                Host = "smtp.ionos.co.uk",
                Port = 25,
                EnableSsl = false,
                Credentials = new System.Net.NetworkCredential("info@sutest.co.uk", "uaW4ATuZRUcB")
            };
            #endif
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

        #region Misc


        /// <summary>
        ///     Builds a standard footer for the emails sent via the Cron service
        /// </summary>
        /// <param name="cronId">CronID of engine (where applicable)</param>
        /// <returns></returns>
        private string EmailFooter(string senderName = "RB Academy", long cronId = 0)
        {
            var footer = "" + Environment.NewLine;
            try
            {
                footer += $"Regards," + Environment.NewLine + senderName;
            }
            catch (Exception ex)
            {
                _Context.AddErrorLog("MailSender", "EmailFooter", ex.Message, senderName);
            }
            return footer;
        }

        #endregion

        #endregion
    }
}