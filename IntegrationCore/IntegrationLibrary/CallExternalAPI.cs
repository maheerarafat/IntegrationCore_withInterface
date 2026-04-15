using IntegrationCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntegrationCore.IntegrationLibrary
{
    public class CallExternalAPI
    {
        public static object CallPostAPI(string APIURL, string PostData, string Tarasol_Token = "")
        {
            DateTime startDate = DateTime.Now;
            int requestId = 0;

            try
            {
                requestId = CreateRequestLog("POST", APIURL, PostData, Tarasol_Token);
                var responseData = ExecutePost(APIURL, PostData, Tarasol_Token);
                bool isSuccess = responseData.StatusCode >= HttpStatusCode.OK && responseData.StatusCode < HttpStatusCode.MultipleChoices;
                UpdateRequestLogAfterResponse(requestId, "POST", APIURL, responseData, startDate, isSuccess, string.Empty);
                return responseData.Body;
            }
            catch (Exception ex)
            {
                UpdateRequestLogAfterResponse(requestId, "POST", APIURL, new HttpResponseData
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Body = ex.Message
                }, startDate, false, ex.Message);

                string errorJson = @"{
            ""result"":""error"",
            ""message"":""\" + ex.Message;
                errorJson += @"""}";
                return errorJson;
            }
        }

        public static object CallGetAPI(string APIURL, string Tarasol_Token = "")
        {
            DateTime startDate = DateTime.Now;
            int requestId = 0;

            try
            {
                requestId = CreateRequestLog("GET", APIURL, string.Empty, Tarasol_Token);
                var responseData = ExecuteGet(APIURL, Tarasol_Token);
                bool isSuccess = responseData.StatusCode >= HttpStatusCode.OK && responseData.StatusCode < HttpStatusCode.MultipleChoices;
                UpdateRequestLogAfterResponse(requestId, "GET", APIURL, responseData, startDate, isSuccess, string.Empty);
                return responseData.Body;
            }
            catch (Exception ex)
            {
                UpdateRequestLogAfterResponse(requestId, "GET", APIURL, new HttpResponseData
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Body = ex.Message
                }, startDate, false, ex.Message);

                string errorJson = @"{
            ""result"":""error"",
            ""message"":""\" + ex.Message;
                errorJson += @"""}";
                return errorJson;
            }
        }

        public static IntResult RetryRequest(int requestId)
        {
            try
            {
                IntJobEsb requestLog = Jobs.GetLogJobById(requestId);
                if (requestLog == null || string.IsNullOrWhiteSpace(requestLog.fldbodyrequest))
                {
                    return IntResultLib.GetResult(0);
                }

                RequestLogPayload payload = JsonConvert.DeserializeObject<RequestLogPayload>(requestLog.fldbodyrequest);
                if (payload == null || string.IsNullOrWhiteSpace(payload.Url) || string.IsNullOrWhiteSpace(payload.Method))
                {
                    return IntResultLib.GetResult(0);
                }

                DateTime startDate = DateTime.Now;
                Jobs.MarkRequestAsInProgress(requestId);
                HttpResponseData responseData = payload.Method.ToUpper() == "POST"
                    ? ExecutePost(payload.Url, payload.RequestBody, payload.Token)
                    : ExecuteGet(payload.Url, payload.Token);

                bool isSuccess = responseData.StatusCode >= HttpStatusCode.OK && responseData.StatusCode < HttpStatusCode.MultipleChoices;
                UpdateRequestLogAfterResponse(requestId, payload.Method.ToUpper(), payload.Url, responseData, startDate, isSuccess, string.Empty);
                return isSuccess ? IntResultLib.GetResult(1) : IntResultLib.GetResult(0);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Retry request failed: " + ex.Message);
                return IntResultLib.GetResult(ex);
            }
        }

        private static int CreateRequestLog(string method, string APIURL, string requestBody, string token)
        {
            RequestLogPayload payload = new RequestLogPayload()
            {
                Method = method,
                Url = APIURL,
                RequestBody = requestBody,
                Token = token
            };

            string payloadAsString = JsonConvert.SerializeObject(payload);
            return Jobs.AddPendingRequestLog("External API Request", $"{method} {APIURL} - Pending", TrimForDb(payloadAsString, 3500));
        }

        private static HttpResponseData ExecutePost(string APIURL, string PostData, string token)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(600000);
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var content = new StringContent(PostData ?? string.Empty, Encoding.UTF8, "application/json");
                var response = client.PostAsync(APIURL, content).GetAwaiter().GetResult();
                return new HttpResponseData()
                {
                    StatusCode = response.StatusCode,
                    Body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
                };
            }
        }

        private static HttpResponseData ExecuteGet(string APIURL, string token)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(600000);
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = client.GetAsync(APIURL).GetAwaiter().GetResult();
                return new HttpResponseData()
                {
                    StatusCode = response.StatusCode,
                    Body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
                };
            }
        }

        private static void UpdateRequestLogAfterResponse(int requestId, string method, string apiUrl, HttpResponseData responseData, DateTime startDate, bool isSuccess, string exceptionMessage)
        {
            if (requestId <= 0)
            {
                return;
            }

            string description = $"{method} {apiUrl} | Status:{(int)responseData.StatusCode} | {(isSuccess ? "Success" : "Failed")}";
            if (!string.IsNullOrWhiteSpace(exceptionMessage))
            {
                description += " | Error:" + exceptionMessage;
            }
            if (!string.IsNullOrWhiteSpace(responseData.Body))
            {
                description += " | Response:" + TrimForDb(responseData.Body, 500);
            }

            Jobs.UpdateRequestLogResult(
                requestId,
                description,
                isSuccess ? (int)Helper.Enum.responseCode.Success : (int)Helper.Enum.responseCode.Failed,
                DateTime.Now,
                (DateTime.Now - startDate).TotalMilliseconds,
                isSuccess ? (int)Helper.Enum.scheduleType.Done : (int)Helper.Enum.scheduleType.NotStarted);
        }

        private static string TrimForDb(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        private class HttpResponseData
        {
            public HttpStatusCode StatusCode { get; set; }
            public string Body { get; set; }
        }



        public static MemoryStream CallGetFileAPI(string APIURL, string Tarasol_Token)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(600000); // Adjusting the timeout

                    if (!string.IsNullOrEmpty(Tarasol_Token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Tarasol_Token);
                    }

                    // Sending GET request
                    var response = client.GetAsync(APIURL).GetAwaiter().GetResult();

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return CallGetFileAPI(APIURL, Cache.GetSession(isNew: true).Token); // Recursion for unauthorized
                    }

                    // Read the response stream and store it in a MemoryStream
                    using (Stream responseStream = response.Content.ReadAsStream())
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        responseStream.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        return memoryStream;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occurred in  : " + ex.Message);
                throw;
            }
        }

        public static async Task<SendFileToTarasolReturnedData> CallPostFileAPIAsync(byte[] FileBytes, string fileName, string APIURL, string Tarasol_Token)
        {
            try
            {
                //MemoryStream memoryStream = new MemoryStream();
                //file.CopyTo(memoryStream);

                //byte[] FileBytes;
                //using (var br = new BinaryReader(file.OpenReadStream()))
                //{
                //    FileBytes = br.ReadBytes((int)(file.OpenReadStream().Length));
                //}
                MultipartFormDataContent multiForm = new MultipartFormDataContent();
                ByteArrayContent byteArrayContent = new ByteArrayContent(FileBytes);
                multiForm.Add(byteArrayContent, "file", fileName);
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMinutes(10);
                var response = await httpClient.PostAsync(APIURL, multiForm);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ScanFileResponse scanFileResponse = await response.Content.ReadAsAsync<ScanFileResponse>(new[] { new JsonMediaTypeFormatter() });
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Scan File Response : " + scanFileResponse.Value, TrackInterface: true);
                    SendFileToTarasolReturnedData sendFileToTarasolReurnedData = new SendFileToTarasolReturnedData()
                    {
                        FileID = scanFileResponse.Value,
                        Name = fileName
                    };
                    return sendFileToTarasolReurnedData;
                }
                else
                {
                    ScanFileResponse scanFileResponse = await response.Content.ReadAsAsync<ScanFileResponse>(new[] { new JsonMediaTypeFormatter() });
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Scan File Response : " + scanFileResponse.Value, TrackInterface: true);
                    SendFileToTarasolReturnedData senFileToTarasolReurnedData = new SendFileToTarasolReturnedData()
                    {
                        FileID = "",
                        Name = fileName
                    };
                    return senFileToTarasolReurnedData;
                }




            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }
        }

        public static async Task<IntResult> CallPostFileAPIAsync(string fileContent, string fileName, string APIURL, string Tarasol_Token)
        {
            try
            {

                var multiForm = new MultipartFormDataContent();
                var base64array = Convert.FromBase64String(fileContent);
                ByteArrayContent byteArrayContent = new ByteArrayContent(base64array);
                multiForm.Add(byteArrayContent, "file", fileName);
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMinutes(10);
                var response = await httpClient.PostAsync(APIURL, multiForm);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Scan File Response : " + response);
                return IntResultLib.GetResult(1);

            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                return IntResultLib.GetResult(ex);
            }
        }

        public static List<Models.Attachment> GetMultiFilesFromTarasol(string APIURL, string Tarasol_Token, string DocUID, string SessionID)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(600000); // Adjusting the timeout

                    if (!string.IsNullOrEmpty(Tarasol_Token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Tarasol_Token);
                    }

                    // Define the directory where the files will be saved
                    string directory = IntegrationSettings.GetSetting("IntegrationDataFolder").ToString() + "\\IntegrationTemp\\" + SessionID + "\\" + DocUID;

                    // Send GET request to fetch the file
                    var response = client.GetAsync(APIURL).GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Failed to fetch file from Tarasol API");
                    }

                    // Create the directory if it doesn't exist
                    Directory.CreateDirectory(directory);

                    // Save the response stream to a file
                    string filePath = directory + "\\" + DocUID + ".tarasol";

                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        response.Content.CopyToAsync(fileStream).GetAwaiter().GetResult();
                    }

                    // Rename and extract the downloaded tarasol file
                    string zipFilePath = Path.ChangeExtension(filePath, ".zip");
                    File.Move(filePath, zipFilePath);
                    ZipFile.ExtractToDirectory(zipFilePath, directory);

                    // Get all files in the directory
                    string[] files = Directory.GetFiles(directory, "*.*");
                    List<Models.Attachment> attachments = new List<Models.Attachment>();

                    // Process each file in the directory
                    foreach (string item in files)
                    {
                        if (!item.EndsWith(DocUID + ".tarasol") && !item.EndsWith(DocUID + ".zip") && !item.EndsWith(DocUID + ".json"))
                        {
                            byte[] bytes = File.ReadAllBytes(item);
                            string stream = Convert.ToBase64String(bytes);

                            Models.Attachment attachment = new Models.Attachment()
                            {
                                FileContent = stream,
                                FileName = Path.GetFileName(item),
                                FileExtension = Path.GetExtension(item)
                            };

                            attachments.Add(attachment);

                            // Delete the file after processing
                            File.Delete(item);
                        }
                    }

                    // Cleanup the directory
                    Directory.Delete(directory, true);

                    return attachments;
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occurred in  : " + ex.Message);
                throw;
            }
        }

        public static void CallGetFaxFile(string APIURL, string Auth, string filePath)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(600000); // Adjusting the timeout

                    // Adding the Authorization header
                    client.DefaultRequestHeaders.Add("Authorization", "Basic " + Auth);

                    // Sending GET request
                    var response = client.GetAsync(APIURL).GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to get fax file. Status Code: {response.StatusCode}");
                    }

                    // Writing the content to the specified file path
                    using (var output = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        response.Content.CopyToAsync(output).GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Error occurred in : " + ex.Message);
                throw;
            }
        }

    }
}
