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

    public class FilesRefreshService: HostedService
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;
        public FilesRefreshService(/*RandomStringProvider randomStringProvider,*/IConfiguration config, IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {
            // _randomStringProvider = randomStringProvider;
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;

            if (_cache.Get("IsSettingsLoaded") == null || (bool)_cache.Get("IsSettingsLoaded") == false)
            {
                Cache cache = new Cache(memoryCache: _cache);
                IntegrationSettings.LoadSettings(_hostingEnvironment);
            }
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //await _randomStringProvider.UpdateString(cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(8), cancellationToken);
            }
        }

        
    }
}
