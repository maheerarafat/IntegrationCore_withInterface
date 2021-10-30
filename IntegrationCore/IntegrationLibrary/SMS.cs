using IntegrationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.IntegrationLibrary
{
    public class SMS
    {
        public static object SendSMS(List<SMSObject> sMSObjects)
        {
            try
            {
                foreach (var sMSObject in sMSObjects)
                {
                    ECS_SMSRequest eCS_SMSRequest = new ECS_SMSRequest()
                    {
                        MobileNumber = sMSObject.MobileNumber,
                        Message = sMSObject.Message,
                        Language = sMSObject.Language
                    };
                    string XMLstring = XMLProcessing.Serialize(eCS_SMSRequest);
                    SoapRequest.CallSMSWebService(XMLstring);
                }
                return IntResultLib.GetResult(200);
            }
            catch (Exception ex)
            {

                Exception exception = new Exception("Unable to send SMS - ex:" + ex.Message);
                throw exception;
            }
        }
        //public static object SendOTP(string mobile, string userNumber)
        //{
        //    try
        //    {
        //        // Generate TOTP and security-key
        //        string otp = String.Empty;
        //        string securityKey = Totp.GenerateTOTP(out otp);


        //        // Save the security-key in table the lookup
        //        SyncHR.UpdateSecurityKeyUser(securityKey, userNumber);

        //        // Send OTP to mobile number
        //        List<SMSObject> sMSObjects = new List<SMSObject>();
        //        SMSObject sms = new SMSObject();
        //        sms.MobileNumber = mobile;
        //        sms.Message = string.Format(Helper.ConstantMessage.OTPMessage, otp);
        //        sms.Language = Helper.Constant.ARABIC_NUM;
        //        sMSObjects.Add(sms);

        //        var result = SendSMS(sMSObjects);
        //        //IntResult result = IntResultLib.GetResult(1); // TODO remove 
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public static IntResult checkOTP(string code, string userNumber)
        //{
        //    try
        //    {

        //        // get the security-key in table the lookup
        //        string securityKey = SyncHR.GetUserSecuirtyLookup(userNumber);

        //        // check TOTP and security-key
        //        string otp = code;
        //        IntResult result = Totp.CompareTOTP(otp, securityKey);

        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
    }
}
