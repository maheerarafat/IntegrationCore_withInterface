using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Models
{
    public class Setting
    {
        //public string ConfigPath { get; set; }
        public string LoginUser { get; set; }
        public string LoginPassword { get; set; }
        public string DBType { get; set; }
        public string ConnectionString { get; set; }
        public string DBPassword { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DevelopSecKey { get; set; }
        public string APIURL { get; set; }
        public string DataFolder { get; set; }
        public bool EnableLogging { get; set; }
        public string Logs { get; set; }
        public string IntegrationDataFolder { get; set; }
        public string IsSettingsLoaded { get; set; }
        public string DMSInterfaceURL { get; set; }
        public string SiteEndPoint { get; set; }
        public string RightFaxFolderPath { get; set; }
        public string TarasolINTPassword { get; set; }
        public string TarasolINTUser { get; set; }
        public string ESBURL { get; set; }
        public string ESBEmailL { get; set; }
        public string ESBSMSUser { get; set; }
        public string ESBSMSPass { get; set; }
        public string ESBECSUser { get; set; }
        public string ESBECSPass { get; set; }
        public string TimeStartSendEmail { get; set; }

        public string ClassificationId { get; set; }

        public string EmailDistributors { get; set; }

        public string EntityPrefix { get; set; }
    }
}
