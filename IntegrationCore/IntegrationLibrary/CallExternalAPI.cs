using IntegrationCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(APIURL);
                if (Tarasol_Token != "")
                {
                    httpWebRequest.Headers["Authorization"] = "Bearer " + Tarasol_Token;
                }
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = PostData;

                    streamWriter.Write(json);
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    CallPostAPI(APIURL, PostData, Cache.GetSession(isNew: true).Token);
                }
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    streamReader.Dispose();
                    streamReader.Close();
                    //object jsonResponse = JsonConvert.DeserializeObject<object>(result);
                    if(result.ToLower().Contains("session expired"))
                    {
                        CallPostAPI(APIURL, PostData, Cache.GetSession(isNew: true).Token);
                    }
                    return result;
                }


                //HttpWebRequest Request = WebRequest.Create("http://tarasolsql/TarasolAPI4/Corr/login?username="+ username +"&password="+ password) as HttpWebRequest;
                //HttpWebResponse Response = Request.GetResponse() as HttpWebResponse;
                //Stream stream = Response.GetResponseStream();
                //StreamReader sr = new StreamReader(stream);
                //string jsonResult = sr.ReadToEnd();

            }
            catch (Exception ex)
            {
                string errorJson = @"{
                    ""result"":""error"",
	                ""message"":""\" + ex.Message;
                errorJson += @"""}";
                //object error = JsonConvert.DeserializeObject<object>(errorJson);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                return errorJson;
            }
        }

        public static object CallGetAPI(string APIURL, string Tarasol_Token="")
        {
            try
            {

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIURL);
                if (Tarasol_Token != "")
                {
                    httpWebRequest.Headers["Authorization"] = "Bearer " + Tarasol_Token;
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    CallGetAPI(APIURL, Cache.GetSession(isNew: true).Token);
                }
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    streamReader.Dispose();
                    streamReader.Close();
                    if (result.ToLower().Contains("session expired"))
                    {
                        CallGetAPI(APIURL, Cache.GetSession(isNew: true).Token);
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
                return errorJson;
            }
        }

        //public static async Task<object> CallGetAPIAsync(string APIURL, string Tarasol_Token = "")
        //{
        //    try
        //    {

        //        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIURL);
        //        if (Tarasol_Token != "")
        //        {
        //            httpWebRequest.Headers["Authorization"] = "Bearer " + Tarasol_Token;
        //        }

        //        WebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            string result = streamReader.ReadToEnd();
        //            Task.Delay(0).Wait();
        //            streamReader.Dispose();
        //            streamReader.Close();

        //            return result;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string errorJson = @"{
        //            ""result"":""error"",
	       //         ""message"":""\" + ex.Message;
        //        errorJson += @"""}";
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
        //        return errorJson;
        //    }
        //}

        public static MemoryStream CallGetFileAPI(string APIURL, string Tarasol_Token)
        {
            try
            {

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIURL);
                if (Tarasol_Token != "")
                {
                    httpWebRequest.Headers["Authorization"] = "Bearer " + Tarasol_Token;
                }
                using (HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        CallGetFileAPI(APIURL, Cache.GetSession(isNew: true).Token);
                    }
                    //try
                    //{
                    //    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    //    {
                    //        string result = streamReader.ReadToEnd();
                    //        streamReader.Dispose();
                    //        streamReader.Close();
                    //        if (result.Contains("Session Expired"))
                    //        {
                    //            CallGetFileAPI(APIURL, Cache.GetSession(isNew: true).Token);
                    //        }
                    //        byte[] byteArray = Encoding.ASCII.GetBytes(result);
                    //        MemoryStream stream = new MemoryStream(byteArray);
                    //        return stream;
                    //    }
                    //}
                    //catch (Exception)
                    //{                        
                    //}
                    //return null;
                    using (Stream responseStream = httpResponse.GetResponseStream())
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        byte[] buffer = new byte[1024];
                        int byteCount;
                        do
                        {
                            byteCount = responseStream.Read(buffer, 0, buffer.Length);
                            memoryStream.Write(buffer, 0, byteCount);
                        } while (byteCount > 0);
                        return memoryStream;
                    }
                }
            }
            catch (Exception ex)
            {
                
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
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
                var response = await httpClient.PostAsync(APIURL, multiForm);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ScanFileResponse scanFileResponse =  await response.Content.ReadAsAsync<ScanFileResponse>(new[] { new JsonMediaTypeFormatter() });
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Scan File Response : " + scanFileResponse.Value);
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
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Scan File Response : " + scanFileResponse.Value);
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

        public static List<Attachment> GetMultiFilesFromTarasol(string APIURL, string Tarasol_Token, string DocUID, string SessionID)
        {
            try
            {

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIURL);
                if (Tarasol_Token != "")
                {
                    httpWebRequest.Headers["Authorization"] = "Bearer " + Tarasol_Token;
                }
                string directory = IntegrationSettings.GetSetting("IntegrationDataFolder").ToString() + "\\IntegrationTemp\\" + SessionID + "\\" + DocUID;
                using (HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Directory.CreateDirectory(directory);
                    using (Stream output = File.OpenWrite(directory + "\\" + DocUID + ".tarasol"))
                    {
                        using (Stream responseStream = httpResponse.GetResponseStream())
                        {
                            responseStream.CopyTo(output);
                        }
                    }

                }
                string FilePath = directory + "\\" + DocUID + ".tarasol";
                File.Move(FilePath, Path.ChangeExtension(FilePath, ".zip"));
                FilePath = FilePath.Replace(".tarasol", "");
                ZipFile.ExtractToDirectory(FilePath + ".zip", directory);
                string[] Files = Directory.GetFiles(directory, "*.*");
                List<Attachment> list = new List<Attachment>();
                foreach (string item in Files)
                {
                    if (!item.EndsWith(DocUID + ".tarasol") && !item.EndsWith(DocUID + ".zip") && !item.EndsWith(DocUID + ".json"))
                    {
                        Byte[] bytes = File.ReadAllBytes(item);
                        string stream = Convert.ToBase64String(bytes);
                        Attachment attachment = new Attachment()
                        {
                            FileContent = stream,
                            FileName = Path.GetFileName(item),
                            FileExtension = Path.GetExtension(item)
                        };
                        list.Add(attachment);
                        File.Delete(item);
                    }
                }
                Directory.Delete(IntegrationSettings.GetSetting("IntegrationDataFolder").ToString() + "\\IntegrationTemp\\" + SessionID, true);
                return list;
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }
        }

        public static void CallGetFaxFile(string APIURL, string Auth,string filePath)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(APIURL);
                
                httpWebRequest.Headers.Add("Authorization", "Basic " + Auth);
                WebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    using (Stream output = File.OpenWrite(filePath))
                    {
                        using (Stream responseStream = httpResponse.GetResponseStream())
                        {
                            responseStream.CopyTo(output);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }
        }

    }
}
