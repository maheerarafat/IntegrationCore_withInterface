using IntegrationCore.IntegrationLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationCore.BGServices
{
    public class DataRefreshService : HostedService
    {
        //private readonly RandomStringProvider _randomStringProvider;
        private IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        public DataRefreshService(IConfiguration config, IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {
            // _randomStringProvider = randomStringProvider;
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;

            if (_cache.Get("IsSettingsLoaded") == null || (bool)_cache.Get("IsSettingsLoaded") == false)
            {
                Cache cache = new Cache(_cache);
                IntegrationSettings.LoadSettings(_hostingEnvironment);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //await _randomStringProvider.UpdateString(cancellationToken);
                Cache.GetSession(true);
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "cancellationToken : " + cancellationToken, TrackInterface: true);
                await Task.Delay(TimeSpan.FromMinutes(2), cancellationToken);
            }
        }
    }
}
