
using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace BusinessLogicLayer.Helpers
{
    public class MailHelper
    {
        public void SendMail(string subject = null, string message = null)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(message);
            writer.Flush();
            stream.Position = 0;


            MailMessage eMail = new MailMessage();

            eMail.From = new MailAddress("esmaasnli1@gmail.com");
            //
            eMail.To.Add("esmaasnli1@gmail.com");
            //
            eMail.Subject = subject;
            //
            SmtpClient smtp = new SmtpClient();
            //
            smtp.Credentials = new System.Net.NetworkCredential("esmaasnli1@gmail.com", "sifre");
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            eMail.IsBodyHtml = true;

            if (!string.IsNullOrEmpty(message))
            {
                eMail.Attachments.Add(new Attachment(stream, "Exception.txt", "text/plain"));
            }

            //send the mail
            smtp.Send(eMail);
        }


    }
}


