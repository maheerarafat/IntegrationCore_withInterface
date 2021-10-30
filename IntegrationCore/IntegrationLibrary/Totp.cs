using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationCore.Models;
using OtpNet;

namespace IntegrationCore.IntegrationLibrary
{
    public class Totp
    {

        public static string GenerateTOTP(out string totpCode)
        {
            try
            {

                var key = KeyGeneration.GenerateRandomKey(20);

                var base32String = Base32Encoding.ToString(key);
                var base32Bytes = Base32Encoding.ToBytes(base32String);
                var totp = new OtpNet.Totp(base32Bytes, step: 300, totpSize: 6);

                totpCode = totp.ComputeTotp(DateTime.UtcNow);


                return base32String;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public static IntResult CompareTOTP(string inTotpCode, string key)
        {
            try
            {

                var base32String = key;
                var base32Bytes = Base32Encoding.ToBytes(base32String);
                var totp = new OtpNet.Totp(base32Bytes, step: 300, totpSize: 6);

                string totpCode = totp.ComputeTotp(DateTime.UtcNow);

                if (inTotpCode == totpCode)
                {
                    return IntResultLib.GetResult(1);
                }
                return IntResultLib.GetResult(4); ;
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
