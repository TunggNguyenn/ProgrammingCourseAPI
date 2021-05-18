using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgrammingCourse.Utilities
{
    public class Email
    {
        public static void SendEmailOTP(string toEmail, int OTP)
        {
            //create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("programmingcourse17_3@outlook.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Send OTP";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"<h1>Your OTP Code: {OTP}</h1>" };

            // send email
            var smtp = new SmtpClient();
            smtp.Connect("smtp.outlook.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("programmingcourse17_3@outlook.com", "hcmus@phanmem");
            smtp.Send(email);
            smtp.Disconnect(true);
        }


        public static void SendEmailSuccessfulVerification(string toEmail)
        {
            //create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("programmingcourse17_3@outlook.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Send OTP";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "<h1>Your OTP Code Verification is successful!</h1>" };

            // send email
            var smtp = new SmtpClient();
            smtp.Connect("smtp.outlook.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("programmingcourse17_3@outlook.com", "hcmus@phanmem");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
