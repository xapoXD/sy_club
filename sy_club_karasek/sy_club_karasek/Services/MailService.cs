using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using club.soundyard.web.Models;
using System.Security.Authentication;

namespace club.soundyard.web.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        public bool SendMail(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        try
                        {
                            mailClient.Connect(_mailSettings.Server, _mailSettings.Port, false);
                            //mailClient.Authenticate("user1", "user1");
                        }
                        catch (SmtpCommandException ex)
                        {
                            Console.WriteLine("Error trying to connect: {0}", ex.Message);
                            Console.WriteLine("\tStatusCode: {0}", ex.StatusCode);
                            return false;
                        }
                        catch (SmtpProtocolException ex)
                        {
                            Console.WriteLine("Protocol error while trying to connect: {0}", ex.Message);
                            return false;
                        }

                        try
                        {
                            mailClient.Send(emailMessage);
                        }
                        catch (SmtpCommandException ex)
                        {
                            Console.WriteLine("Error sending message: {0}", ex.Message);
                            Console.WriteLine("\tStatusCode: {0}", ex.StatusCode);

                            switch (ex.ErrorCode)
                            {
                                case SmtpErrorCode.RecipientNotAccepted:
                                    Console.WriteLine("\tRecipient not accepted: {0}", ex.Mailbox);
                                    break;
                                case SmtpErrorCode.SenderNotAccepted:
                                    Console.WriteLine("\tSender not accepted: {0}", ex.Mailbox);
                                    break;
                                case SmtpErrorCode.MessageNotAccepted:
                                    Console.WriteLine("\tMessage not accepted.");
                                    break;
                            }
                        }
                        catch (SmtpProtocolException ex)
                        {
                            Console.WriteLine("Protocol error while sending message: {0}", ex.Message);
                        }

                        mailClient.Disconnect(true);
                    }

                }
                

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


    }
}
