using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
namespace IntegrationCore.IntegrationLibrary
{
    public class DirectEmailService
    {
        //public static string SendEmail(string toAddress, string subject, string body)
        //{
        //    string result = "Message Sent Successfully..!!";
        //    string senderID = "admin@flow.com";
        //    const string senderPassword = "admin";
        //    try
        //    {
        //        SmtpClient smtp = new SmtpClient
        //        {
        //            Host = "localhost",
        //            Port = 25,
        //            DeliveryMethod = SmtpDeliveryMethod.Network,
        //            Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
        //            Timeout = 30000,
        //        };
        //        MailMessage message = new MailMessage(senderID, toAddress, subject, body);
        //        smtp.Send(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
        //        result = "Error sending email.!!!";
        //    }
        //    return result;
        //}
    }
}
