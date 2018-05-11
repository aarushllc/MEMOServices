using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace MEMOServices
{
    public class EmailCode
    {

        public EmailCode()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void SendMail(string Body, string EmailId, string Subject)
        {
            try
            {
                string buffer = Body;
                MailMessage mail = new MailMessage();
                System.Net.Mail.SmtpClient SmtpMail = new System.Net.Mail.SmtpClient();
                mail.To.Add(EmailId);
                mail.Bcc.Add("khengar.kher@aarushinfoweb.com");
                mail.From = new MailAddress("noreply@cashlogic.com");
                mail.Subject = Subject;
                mail.Body = buffer;
                mail.IsBodyHtml = true;
                SmtpMail.Host = "smtpout.secureserver.net";
                SmtpMail.Credentials = new NetworkCredential("khengar.kher@aarushinfoweb.com", "Password1");
                SmtpMail.EnableSsl = false;
                SmtpMail.ServicePoint.MaxIdleTime = 10;
                SmtpMail.Port = 3535;
                SmtpMail.Send(mail);
            }
            catch (Exception ex)
            {
                string buffer1 = Body;
                MailMessage mail1 = new MailMessage();
            }
        }

    }
}