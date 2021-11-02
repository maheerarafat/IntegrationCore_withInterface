
namespace IntegrationCore.Models
{
    public class SettingModel
    {
        
        //public string ConfigPath { get; set; }

        public string LoginUser { get; set; }
        public string LoginPassword { get; set; }
        public string DBType { get; set; }
        public string ConnectionString { get; set; }
        public string DBPassword { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string APIURL { get; set; }
        public string DataFolder { get; set; }
        public bool EnableLogging { get; set; }
        public bool EnableLoggingInfo { get; set; }
        public string Logs { get; set; }
        public string IntegrationDataFolder { get; set; }
        public string IsSettingsLoaded { get; set; }

        public string DMSInterfaceURL { get; set; }
        public string SiteEndPoint { get; set; }
        public string DefaultReference { get; set; }
        
        public string EntityPrefix { get; set; }
        
        public string RightFaxFolderPath { get; set; }
        public string TarasolINTPassword { get; set; }
        public string TarasolINTUser { get; set; }
        public string ESBURL { get; set; }
        public string ESBEmail { get; set; }
        public string ESBEmailUser { get; set; }
        public string ESBEmailPassword { get; set; }
        public string ESBSMSUser { get; set; }
        public string ESBSMSPass { get; set; }
        public string MOISMSUser { get; set; }
        public string MOISMSPass { get; set; }
        public string ESBECSUser { get; set; }
        public string ESBECSPass { get; set; }
        public string ESBHRUser { get; set; }
        public string ESBHRPass { get; set; }
        public string FaxServer { get; set; }
        public string FaxUser { get; set; }
        public string FaxPass { get; set; }
        public string FromFaxField { get; set; }
        public string TokenPeriod { get; set; }
        public string MOISMSChannel { get; set; }
        public string DMSUser { get; set; }
        public string DMSPass { get; set; }
        //   
    }
}
