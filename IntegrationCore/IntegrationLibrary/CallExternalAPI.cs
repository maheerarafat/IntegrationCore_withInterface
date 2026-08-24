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

namespace IntegrationCore.IntegrationLibrary
{
    public class CallExternalAPI
    {


        public static object CallPostAPI(string APIURL, string PostData, string Tarasol_Token = "")
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

                    var content = new StringContent(PostData, Encoding.UTF8, "application/json");

                    // Sending POST request
                    var response = client.PostAsync(APIURL, content).GetAwaiter().GetResult();

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Unauthorized (Post Request) Rec01   : ");
                        return CallPostAPI(APIURL, PostData, Cache.GetSession(isNew: true).Token);
                    }

                    var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Tarasol Result: " + result);

                    if (result.ToLower().Contains("session expired"))
                    {
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Session Expired Tarasol Result (Post Request) Rec02  : ");
                        return CallPostAPI(APIURL, PostData, Cache.GetSession(isNew: true).Token);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                string errorJson = @"{
            ""result"":""error"",
            ""message"":""\" + ex.Message;
                errorJson += @"""}";
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occurred in  : " + ex.Message);
                return errorJson;
            }
        }

        public static object CallGetAPI(string APIURL, string Tarasol_Token = "")
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
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Unauthorized (Get Request) Rec01  : ");
                        return CallGetAPI(APIURL, Cache.GetSession(isNew: true).Token); // Recursion for unauthorized
                    }

                    var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (result.ToLower().Contains("session expired"))
                    {
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Session Expired Tarasol Result (Get Request) Rec02  : ");
                        return CallGetAPI(APIURL, Cache.GetSession(isNew: true).Token); // Recursion for session expired
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                string errorJson = @"{
            ""result"":""error"",
            ""message"":""\" + ex.Message;
                errorJson += @"""}";
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occurred in  : " + ex.Message);
                return errorJson;
            }
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
                throw;
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
