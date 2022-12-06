using IntegrationCore.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.IntegrationLibrary
{
    public class Cache
    {
        private static IMemoryCache _cache;
        
        public Cache(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public static object GetFromCache(string CacheKey)
        {
           return _cache.Get(CacheKey);
        }


        public static bool CheckAvailableCache()
        {
            return (_cache != null);
        }

        public static void SetCache(string CacheKey, object Value)
        {
            if (_cache.Get(CacheKey) == null)
            {
                _cache.CreateEntry(CacheKey);
            }
            _cache.Set(CacheKey, Value);
        }

        public static LoginResponse GetSession( bool isNew = false )
        {
            LoginResponse jsonResponse = new LoginResponse();

            string UserName = IntegrationSettings.GetSetting("TarasolINTUser").ToString(); // need to read from config file
            string Password = Encryption.DecryptString(IntegrationSettings.GetSetting("TarasolINTPassword").ToString());
            LoginResponse TarasolSession_object = (LoginResponse)_cache.Get("TarasolSession");
            //string TarasolSession = "";
            if (TarasolSession_object != null && isNew == false)
            {                
                return TarasolSession_object;
            }
            else
            {
                _cache.CreateEntry("TarasolSession");
                string json = @"{
                ""username"":""" + UserName + @""",
	            ""password"":""" + Password + @""",
	            ""Language"":""0"",
	            ""DomainLogin"":"""",
	            ""IPuser"":""111.111.2.160,TARASOL,4""
                }";
                object jsonResult = CallExternalAPI.CallPostAPI(_cache.Get("APIURL").ToString() + "/Corr/login", json);
                TarasolSession_object = JsonConvert.DeserializeObject<LoginResponse>(jsonResult.ToString());
                _cache.Set("TarasolSession", TarasolSession_object);
            }

            return TarasolSession_object;
        }
    }
}
