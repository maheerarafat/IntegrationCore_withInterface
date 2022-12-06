using ICALibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICALibrary.Helper
{
    public static class Constant
    {
        /// <summary>
        /// General
        /// </summary>
        //public static string Username = "ESB.TARASOL";
        //public static string Password = "123pass!@#";

        /// <summary>
        /// SMS constant
        /// </summary>
        public static string notificationTypeSMS = "SMS";
        public static string subjectSMS = "ICA";
        public static string fromAddresSMS = "";
        public static string serviceNameSMS = "SEND_NOTIFICATION";
        public static string sourceChannelSMS = "TARASOL";
        public static int serviceVersionSMS = 1;
        public static string hashSMS = "";

        /// <summary>
        /// Email constant
        /// </summary>
        public static string notificationTypeEmail = "EMAIL";

        /// <summary>
        /// Document constant
        /// </summary>
        public static string serviceNameAddDocument = "TARASOL_ADD_DOCUMENT";
        public static string serviceNameDeliveryReport = "TARASOL_DELIVERY_REPORT";
        public static string ResponseSuccessTarasolNotification = "\"Code\":200";

        public static string urlDeliveryReport = "/deliveryReport";
        public static string urlAddNewDocument = "/addNewDocument";

        /// <summary>
        /// Language constant
        /// </summary>
        public static string ARABIC_STR = "arabic";
        public static string ENGLISH_STR = "english";
        public static string ARABIC_NUM = "1";
        public static string ENGLISH_NUM = "0";

        /// <summary>
        /// code name of past HR name in ECS
        /// </summary> 
        public static string AP = "ap"; // national and local
        public static string UP = "up"; // union
        public static string PS = "ps"; // virtual


        /// <summary>
        /// Functions name
        /// </summary>
        public static string MethodSendAckICA = "SendAckICA";
        public static string MethodExportFromFolderICA = "ExportFromFolderICA";
        public static string MethodWriteAck = "WriteAck";
        public static string MethodSendSMSICA = "SMS.SendSMS:";

        /// <summary>
        /// Schedule constant
        /// </summary>
        public static int MaxNumberSchedule = 3;

        /// <summary>
        /// Files download
        /// </summary>
        public static string FileContentType = "APPLICATION/octet-stream";
        public static string FileLogExtenstion = ".txt";
        public static int MaxFilePathLength = 248;

        // Session expired
        public static string SessionExpired = "session expired";

        //  Response Code ESB
        public static List<ResponseCodeESB> responseCodes = new List<ResponseCodeESB> { 
            new ResponseCodeESB { Code=1009, Description = "Duplicate Transaction Reference Number" },
            new ResponseCodeESB { Code=9997, Description = "Request Timeout" },
            new ResponseCodeESB { Code=9998, Description = "Service Not Available" },
        };
    }


    public static class ConstantMessage
    {
        public static string OTPMessage = "رمز التأكيد {0}";
    }
}
