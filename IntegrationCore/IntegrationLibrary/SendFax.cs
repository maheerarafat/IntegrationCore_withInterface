using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Fax.WebApi;
using System.Net.Http;

namespace IntegrationCore.IntegrationLibrary
{
    public class SendFax
    {
        /// <summary>
        /// Sends a fax.
        /// </summary>
        public static async Task Send(string ExchangeNo,string CorrUID, List<MultipartFormDataContent> files, string faxNumber, string url, string username, string password, string nameRecipient = "MOI", DirectoryInfo downloadDirectory = null)
        {
            // Create HTTP client
            var client = await HttpClientFax.CreateClient(username, password, url);

            // Get API root, which provides links to other API resources.
            // By using these links, you avoid tying your code to the
            // current structure of the API, which is subject to change.
            // In contrast, the link relationship names will not change.
            var root = await client.Get<Root>(url);
            var attachmentsUrl = root.User.Links.First(link => link.Rel == "Attachments").Href;
            var sendJobsUrl = root.User.Links.First(link => link.Rel == "SendJobs").Href;

            // Upload the file as an attachment.
            // The return value is the URL of the uploaded file.
            List<string> attachmentUrls = await UploadAttachments(client, attachmentsUrl, files);

            // Create the send job using the fax number and the attachment.
            // The return value is the URL of the created job.
            var job = new SendJob();
            job.Recipients.Add(new Recipient { Destination = faxNumber, Name = nameRecipient });
            foreach (var attachmentUrl in attachmentUrls)
            {
                job.AttachmentUrls.Add(attachmentUrl);
            }
            var jobUrl = await client.Post(sendJobsUrl, job);

            // Wait for job to reach completion
            job = await WaitForJob(client, jobUrl);

            // If no documents were generated then there's nothing else to check.
            if (job.DocumentCount == 0)
            {
                Correspondence.SendNotificationToTarasol(ExchangeNo, CorrUID, 2, 0, "", "");
                throw new ArgumentException("Documents can't be null or empty.", "DocumentCount");
            }
                
            else
                Correspondence.SendNotificationToTarasol(ExchangeNo, CorrUID, 1, 0, "", "");

        }

        /// <summary>
        /// Uploads a file as an attachment.
        /// </summary>
        static async Task<List<string>> UploadAttachments(HttpClient client, string attachmentsUrl, List<MultipartFormDataContent> files)
        {
            List<string> attachmentUrls = new List<string>();
            // Upload the file using standard multipart form data.
            foreach (var content in files)
            {
                // POST the data; the response's location header will be the URL of the uploaded file
                var attachmentUrl = await client.Post(attachmentsUrl, content);
                attachmentUrls.Add(attachmentUrl);
            }

            return attachmentUrls;
        }

        /// <summary>
        /// Waits for a job to reach a final status.
        /// </summary>
        static async Task<SendJob> WaitForJob(HttpClient client, string jobUrl)
        {
            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Job processing - Fax Operation ", TrackInterface: true);

            SendJob job;
            do
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                job = await client.Get<SendJob>(jobUrl);

                Console.Write(".");

            } while (job.Condition != "Succeeded" && job.Condition != "Cancelled" && job.Condition != "Failed");

            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, $"Job processing - Fax Operation status {job.Status}", TrackInterface: true);

            return job;
        }


    }
}
