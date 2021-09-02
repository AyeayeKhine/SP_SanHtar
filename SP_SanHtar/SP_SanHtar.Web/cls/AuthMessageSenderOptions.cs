using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SP_SanHtar.Web.cls
{
    public class AuthMessageSenderOptions : IEmailSender
    {

        public async Task SendEmailAsync1(string email, string subject, string message)
        {
            try
            {
                //From Address    
                string FromAddress = "ikhae20492@gmial.com";
                string FromAdressTitle = "SPSH";

                //To Address    
                string ToAddress = email;
                string ToAdressTitle = "Microsoft ASP.NET Core";
                string Subject = subject;
                string BodyContent = message;

                //Smtp Server    
                string SmtpServer = "smtp.gmail.com";
                //string SmtpServer = "smtp.mailgun.org";
                //Smtp Port Number    
                //int SmtpPortNumber = 587;
                int SmtpPortNumber = 465;
                //Our servers listen on ports 25, 587, and 465 (SSL/TLS)

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress
                                        (FromAdressTitle,
                                         FromAddress
                                         ));
                mimeMessage.To.Add(new MailboxAddress
                                         (ToAdressTitle,
                                         ToAddress
                                         ));
                mimeMessage.Subject = Subject; //Subject  
                mimeMessage.Body = new TextPart("plain")
                {
                    Text = BodyContent
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(SmtpServer, SmtpPortNumber, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH");
                    client.Authenticate(
                        "ikhae20492@gmial.com",
                        "Row90919"
                        );
                    //client.Authenticate(
                    //    "postmaster@sandbox78f8b3ced89a46888e199bda3f734935.mailgun.org",
                    //    "e17f4a45a57155ce1b588ecf0135a368-060550c6-508ddd89"
                    //    );
                    await client.SendAsync(mimeMessage);
                    Console.WriteLine("The mail has been sent successfully !!");
                    Console.ReadLine();
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                //From Address    
                string FromAddress = "mailsenderSpsh@gmail.com";
                string FromAdressTitle = "SPSH";

                //To Address    
                string ToAddress = email;
                string ToAdressTitle = "Microsoft ASP.NET Core";
                string Subject = subject;
                string BodyContent = message;

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                mail.From = new System.Net.Mail.MailAddress(FromAddress);
                mail.To.Add(new System.Net.Mail.MailAddress(ToAddress));
                //mail.Bcc.Add(new System.Net.Mail.MailAddress("Three@gmail.com"));
                mail.Subject = Subject;
                mail.Body = BodyContent;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "smtp.office365.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                System.Net.NetworkCredential credential = new System.Net.NetworkCredential();

                credential.UserName = "mailsenderSpsh@gmail.com";
                credential.Password = "tnhRdbA1";
                smtp.Credentials = credential;
               
                smtp.Send(mail);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task SendEmailAsync(string email, string subject, string message, string file)
        {
            try
            {

                //From Address    
                string FromAddress = "mailsender@fusionsol.com";
                string FromAdressTitle = "SPSH";

                //To Address    
                string ToAddress = email;
                string ToAdressTitle = "Microsoft ASP.NET Core";
                string Subject = subject;
                string BodyContent = message;

                //Smtp Server    
                string SmtpServer = "smtp.office365.com";
                //string SmtpServer = "smtp.mailgun.org";
                //Smtp Port Number    
                int SmtpPortNumber = 587;
                //int SmtpPortNumber = 465;
                //Our servers listen on ports 25, 587, and 465 (SSL/TLS)


                // byte[] imageByteData = System.IO.File.ReadAllBytes(file);


                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                mail.From = new System.Net.Mail.MailAddress(FromAddress);
                mail.To.Add(new System.Net.Mail.MailAddress(ToAddress));
                //mail.Bcc.Add(new System.Net.Mail.MailAddress("Three@gmail.com"));
                mail.Subject = Subject;
                mail.Body = BodyContent + "  By " + email;
                //aaUri uriObj = new Uri(file);

                //aastring BlobName = Path.GetFileName(uriObj.LocalPath);
                if (file != "")
                {
                    //System.Net.Mail.Attachment inline = new System.Net.Mail.Attachment("@"+file);
                    //inline.ContentType.Name = BlobName;
                    //mail.Attachments.Add(inline);
                    //System.Net.HttpWebRequest rq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(new Uri(file));
                    //Stream stream = rq.GetRequestStream();
                    var bytes = new byte[0];
                    var mimeType = "";
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        // name = System.IO.Path.GetFileName(file);
                        bytes = wc.DownloadData(file);
                        mimeType = wc.ResponseHeaders["content-type"];
                    }
                    Stream S = new MemoryStream(bytes);
                    System.Net.Mail.Attachment inline = new System.Net.Mail.Attachment(S, "MG.png");
                    mail.Attachments.Add(inline);

                }

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "smtp.office365.com";
                smtp.Port = 587;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                System.Net.NetworkCredential credential = new System.Net.NetworkCredential();

                credential.UserName = "mailsender@fusionsol.com";
                credential.Password = "Wup26633";

                smtp.Credentials = credential;
                smtp.EnableSsl = true;
                smtp.Send(mail);
                //IK
                //using (var client = new SmtpClient())
                //{
                //    client.Connect(SmtpServer, SmtpPortNumber, false);
                //    client.AuthenticationMechanisms.Remove("XOAUTH");
                //    client.Authenticate(
                //        "mailsender@fusionsol.com",
                //        "Wup26633"
                //        );
                //    //client.Authenticate(
                //    //    "postmaster@sandbox78f8b3ced89a46888e199bda3f734935.mailgun.org",
                //    //    "e17f4a45a57155ce1b588ecf0135a368-060550c6-508ddd89"
                //    //    );
                //    await client.Send(mail);
                //    Console.WriteLine("The mail has been sent successfully !!");
                //    Console.ReadLine();
                //    await client.DisconnectAsync(true);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendMessageSmtp(string email, string subject, string message)
        {
            try
            {
                // Compose a message
                MimeMessage mail = new MimeMessage();
                mail.From.Add(new MailboxAddress("Excited Admin", "ayeayekhine@sandbox78f8b3ced89a46888e199bda3f734935.mailgun.org"));
                mail.To.Add(new MailboxAddress("Excited User", email));
                mail.Subject = "Hello";
                mail.Body = new TextPart("plain")
                {
                    Text = message,
                };

                // Send it!
                using (var client = new SmtpClient())
                {
                    // XXX - Should this be a little different?
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect("smtp.mailgun.org", 587, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate("postmaster@sandbox78f8b3ced89a46888e199bda3f734935.mailgun.org", "e17f4a45a57155ce1b588ecf0135a368-060550c6-508ddd89");

                    client.Send(mail);
                    client.Disconnect(true);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
