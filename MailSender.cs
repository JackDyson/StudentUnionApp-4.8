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
        ///     Sends an email informing us that the service has started
        /// </summary>
        /// <param name="version">Service version number</param>
        public void StartUpEmail(string version)
        {
            try
            {
                MailAddress to = new MailAddress("jack.dyson@Abzorb.co.uk");
                MailAddress from = new MailAddress("info@sutest.co.uk");
                MailMessage mail = new MailMessage(from, to)
                {
                    Subject = $"Started Cron Service - v{version}",
                    IsBodyHtml = true,
                    Body = $"Successfully started Cron Service {Environment.NewLine} {EmailFooter("Automation Service")}"
                };
                mail.IsBodyHtml = true;
                _Smtp.Send(mail);
            }
            catch (Exception ex)
            {
                _Context.ConnectToApi(ex.InnerException.StackTrace);
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