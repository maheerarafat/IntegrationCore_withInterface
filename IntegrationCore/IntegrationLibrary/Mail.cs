using IntegrationCore.Models;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IntegrationCore.IntegrationLibrary
{
    public class Mail
    {
        
        public static object SendEmail(Models.Mail mail)
        {
            try
            {
                List<SoapEMialAttachment> emailAttachments = new List<SoapEMialAttachment>();
                foreach (EmailAttachment item in mail.Attachments)
                {
                    var provider = new FileExtensionContentTypeProvider();
                    string _contentType;
                    if (!provider.TryGetContentType(item.FileName + "." + item.FileType.Replace(".", ""), out _contentType))
                    {
                        _contentType = "application/octet-stream";
                    }
                    emailAttachments.Add(new SoapEMialAttachment()
                    {
                        Body = item.FileContent,
                        ContentType = _contentType,
                        FileName = item.FileName + "." + item.FileType.Replace(".","")
                    });
                }
                List<SoapEmail> _Emailcc = new List<SoapEmail>();
                List<SoapEmail> _EmailTo = new List<SoapEmail>();
                foreach (var email in mail.Receivers)
                {
                    if (email.ReceiverType == ReceiverType.to)
                        _EmailTo.Add(new SoapEmail() { email= email.ReceiverEmail });
                    else
                        _Emailcc.Add(new SoapEmail() { email = email.ReceiverEmail });
                };
                SoapEmailObject emailObject = new SoapEmailObject()
                {
                    Attachments = emailAttachments,
                    EmailCC = _Emailcc,
                    EmailTo = _EmailTo,
                    EmailSubject = mail.Subject,
                    EmailContent = mail.Body
                };
                string XMLstring = XMLProcessing.Serialize(emailObject);
                return SoapRequest.CallEmailWebService(XMLstring);
            }
            catch (Exception)
            {

                Exception exception = new Exception("Unable to send Email");
                return IntResultLib.GetResult(exception);
            }
        }
    }
}
