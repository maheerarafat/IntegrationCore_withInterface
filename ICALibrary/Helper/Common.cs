using ICALibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static ICALibrary.Helper.Enum;

namespace ICALibrary.Helper
{
    public static class Common
    {
        private static Random _random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomNumberStr(int length)
        {
            try
            {
                const string chars = "0123456789";
                return new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            }
            catch (Exception e)
            {

                throw e;
            }
           
        }


        /// <summary>
        /// Replace the tags that generted from serialze 
        /// </summary>
        /// <param name="envelope"></param>
        /// <returns></returns>
        public static string ReplaceXMLStr(String envelope)
        {
            envelope = envelope.Replace("not1_x003A_", "not1:")
                                   .Replace("com_x003A_", "com:")
                                   .Replace(@"xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
                                   .Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "")
                                   .Replace("<not1:Body >", "<not1:Body>")
                                   .Replace("<Body >", "<tar1:Body>")
                                   .Replace("</Body>", "</tar1:Body>")
                                   .Replace("tar1_x003A_", "tar1:")
                                   .Replace("<tar1:Attachments />", "");

            envelope = Regex.Replace(envelope, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            return envelope;
        }

        public static string RemoveSpecialChar(String envelope)
        {
            envelope = envelope.Replace(":", "")
                .Replace("/", "")
                .Replace("\n", " ")
                .Replace("\r", " ")
                .Replace(",", "")
                .TrimStart()
                .TrimEnd();
            envelope = RemoveSpaces(envelope);

            return envelope;
        }

        public static string RemoveRegexBodyHTML(String envelope)
        {
            string result = "empty";

            if (!String.IsNullOrWhiteSpace(envelope) )
            {
                result = Regex.Replace(envelope, "<.*?>", String.Empty)
               .Replace("\n", String.Empty)
               .Replace("\r", " ")
               .Replace("&nbsp", String.Empty)
               .Replace(";", String.Empty);
            }
            return Base64Encode(RemoveSpaces(result.Trim()));
        }


        public static string RemoveSpaces(String envelope)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            envelope = regex.Replace(envelope, " ");

            return envelope;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="envelope"></param>
        /// <returns></returns>
        public static string UnicodeXMLFormatted(String envelope)
        {
            String whaitSpace = " ";
            envelope = envelope.Replace(Environment.NewLine, "")
                .Replace("\t", "")
                .Replace(whaitSpace, "");

            return envelope;
        }


        public static string GetPrefixName(String name)
        {
            if (name.ToLower().Contains("ap"))
            {
                return Constant.AP;
            }
            else if (name.ToLower().Contains("up"))
            {
                return Constant.UP;
            } 
            else if (name.ToLower().Contains("ps"))
            {
                return Constant.PS;
            }
            else
                return String.Empty;
        }

        public static string Base64Encode(String plainText)
        {
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string Base64Decode(String base64EncodeData)
        {
            try
            {
                var base64EncodeBytes = Convert.FromBase64String(base64EncodeData);
                return Encoding.UTF8.GetString(base64EncodeBytes);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }


    public class IntResultLib
    {
        public static IntResult GetResult(Exception ex)
        {
            IntResult eSBResult = new IntResult
            {
                Code = ex.HResult,
                EMessage = ex.Message,
                LMessage = ex.Message
            };
            return eSBResult;
        }

        /// <summary>
        /// Get the result message 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static IntResult GetResult(int code)
        {
            IntResult eSBResult = new IntResult();
            if (code == 2)
            {
                eSBResult.Code = 2;
                eSBResult.EMessage = "Input data not valid";
                eSBResult.LMessage = "البيانات المرسلة غير صحيحة";
            }
            else if (code == 200)
            {
                eSBResult.Code = 1;
                eSBResult.EMessage = "Success";
                eSBResult.LMessage = "تمت العملية بنجاح";
            }
            else if (code == 4)
            {
                eSBResult.Code = 2;
                eSBResult.EMessage = "Input data not valid";
                eSBResult.LMessage = "رمز التأكيد غير صحيح";
            }
            else if (code == 0)
            {
                eSBResult.Code = 0;
                eSBResult.EMessage = "Failed";
                eSBResult.LMessage = "فشلت عملية الإرسال";
            }
            else
            {
                eSBResult.Code = 1;
                eSBResult.EMessage = "Success";
                eSBResult.LMessage = "تمت العملية بنجاح";
            }
            return eSBResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static IntResult GetResult(int code, string msg)
        {
            IntResult eSBResult = new IntResult();
            if (code == 2)
            {
                eSBResult.Code = 2;
                eSBResult.EMessage = "Input data not valid";
                eSBResult.LMessage = "البيانات المرسلة غير صحيحة";
                eSBResult.Details = msg;
            }
            else
            {
                eSBResult.Code = 1;
                eSBResult.EMessage = "Success";
                eSBResult.LMessage = "تمت العملية بنجاح";
                eSBResult.Details = msg;
            }

            return eSBResult;
        }


        public static IntResult GetResult(int code, string msg, responseStatusNotification statusNotification)
        {
            IntResult eSBResult = new IntResult();
            if (code == 2)
            {
                eSBResult.Code = 2;
                eSBResult.EMessage = "Input data not valid";
                eSBResult.LMessage = "البيانات المرسلة غير صحيحة";
                eSBResult.Details = msg;
                eSBResult.statusNotification = statusNotification;
            }
            else
            {
                eSBResult.Code = 1;
                eSBResult.EMessage = "Success";
                eSBResult.LMessage = "تمت العملية بنجاح";
                eSBResult.Details = msg;
            }

            return eSBResult;
        }
    }

}
