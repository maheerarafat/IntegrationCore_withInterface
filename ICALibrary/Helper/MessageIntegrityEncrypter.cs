using System;
using System.Collections.Generic;
using System.Text;
using ae.gov.id.crypto;

namespace ICALibrary.Helper
{
    public class MessageIntegrityEncrypter
    {

        // Development seciurty key this change every two years this key 
        // generated from ICA side
        private static string _developSecKey = "4A49014E579E142217AEA2455508EFB419FB172FBADA44605192843060D4E3B1";


        /// <summary>
        /// Encrypt the username and password that send in request
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String EncryptDataICA(String data, String developSecKey)
        {
            // Encryption
            string enc_str = Encrypter.EncryptData(data, developSecKey);

            return enc_str;
        }


        /// <summary>
        /// Hash the body of Message 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String SignMessageICA(String data, String developSecKey)
        {
            // Hash SHA256
            //string hash_SHA256 = Hasher.GenerateSHA256Hash(data);
            //string hash_base64 = Encrypter.EncryptData(hash_SHA256, developSecKey);

            // Hash in base64
            string hash_base64 = MessageSigner.SignMessage(data, developSecKey);

            return hash_base64;
        }

    }
}
