using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IntegrationCore.IntegrationLibrary;
using IntegrationCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;


namespace IntegrationCore.BGServices
{
    public class JoberSMSService : HostedService
    {

        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IMemoryCache _cache;

        private  System.Timers.Timer _aTimer;

        public JoberSMSService(IConfiguration config, IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;

            _aTimer = new System.Timers.Timer();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {

                //_aTimer.Interval = 30000;
                //_aTimer.Elapsed += OnTimedEvent;
                //_aTimer.AutoReset = true;
                //_aTimer.Enabled = true;
                ConfigAttribute configAttribute = new ConfigAttribute
                {
                    consumerID = _config["ICA:notification_service:consumer_id"],
                    urlServiceNotify = _config["ICA:notification_service:url"]
                };
                Task.Factory.StartNew(() => Jobs.ProccessSchedule(configAttribute));

                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }



    }
}
