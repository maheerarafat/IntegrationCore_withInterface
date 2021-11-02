using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Xml;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;

namespace IntegrationCore.IntegrationLibrary
{
    public class IntegrationSettings
    {
        //need to be in application path
        public static string ConfigPath;// = @"C:\Users\Ghaith Abbas\source\repos\IntegrationCore\IntegrationCore\IntegrationSetting.config";
        //public static Dictionary<String, String> key_value = new Dictionary<String, String>();
        public IntegrationSettings(string configPath)
        {
            ConfigPath = configPath;
        }
        public static bool LoadSettings(IHostingEnvironment _hostingEnvironment= null )
        {
            try
            {
                if(_hostingEnvironment != null && string.IsNullOrWhiteSpace(ConfigPath))
                {
                    ConfigPath = _hostingEnvironment.ContentRootPath + @"\IntegrationSetting.config";
                }
                

                XmlDocument xdoc = new XmlDocument();
                if (File.Exists(ConfigPath))
                {
                    xdoc.Load(ConfigPath);
                    Cache.SetCache("ConfigPath", ConfigPath);
                    Cache.SetCache("DBType", xdoc.SelectSingleNode("Configuration_Properties/DBType").InnerText);
                    Cache.SetCache("LoginUser", xdoc.SelectSingleNode("Configuration_Properties/LoginUser").InnerText);
                    Cache.SetCache("LoginPassword", xdoc.SelectSingleNode("Configuration_Properties/LoginPassword").InnerText);
                    Cache.SetCache("ConnectionString", xdoc.SelectSingleNode("Configuration_Properties/ConnectionString").InnerText);
                    Cache.SetCache("DBPassword", xdoc.SelectSingleNode("Configuration_Properties/DBPassword").InnerText);
                    Cache.SetCache("UserName", xdoc.SelectSingleNode("Configuration_Properties/UserName").InnerText);
                    Cache.SetCache("Password", xdoc.SelectSingleNode("Configuration_Properties/Password").InnerText);
                    Cache.SetCache("APIURL", xdoc.SelectSingleNode("Configuration_Properties/APIURL").InnerText);
                    Cache.SetCache("DataFolder", xdoc.SelectSingleNode("Configuration_Properties/DataFolder").InnerText);
                    Cache.SetCache("EnableLogging", xdoc.SelectSingleNode("Configuration_Properties/EnableLogging").InnerText);
                    Cache.SetCache("EnableLoggingInfo", xdoc.SelectSingleNode("Configuration_Properties/EnableLoggingInfo").InnerText);
                    //Cache.SetCache("Logs", xdoc.SelectSingleNode("Configuration_Properties/log4net/appender/file").InnerText);
                    Cache.SetCache("IntegrationDataFolder", xdoc.SelectSingleNode("Configuration_Properties/IntegrationDataFolder").InnerText);
                    Cache.SetCache("DMSInterfaceURL", xdoc.SelectSingleNode("Configuration_Properties/DMSInterfaceURL").InnerText);
                    Cache.SetCache("SiteEndPoint", xdoc.SelectSingleNode("Configuration_Properties/SiteEndPoint").InnerText);
                    Cache.SetCache("DefaultReference", xdoc.SelectSingleNode("Configuration_Properties/DefaultReference").InnerText);
                    Cache.SetCache("EntityPrefix", xdoc.SelectSingleNode("Configuration_Properties/EntityPrefix").InnerText);

                    Cache.SetCache("RightFaxFolderPath", xdoc.SelectSingleNode("Configuration_Properties/RightFaxFolderPath").InnerText);
                    Cache.SetCache("TarasolINTPassword", xdoc.SelectSingleNode("Configuration_Properties/TarasolINTPassword").InnerText);
                    Cache.SetCache("TarasolINTUser", xdoc.SelectSingleNode("Configuration_Properties/TarasolINTUser").InnerText);
                    Cache.SetCache("ESBURL", xdoc.SelectSingleNode("Configuration_Properties/ESBURL").InnerText);
                    
                    Cache.SetCache("ESBEmail", xdoc.SelectSingleNode("Configuration_Properties/ESBEmail").InnerText);
                    Cache.SetCache("ESBEmailUser", xdoc.SelectSingleNode("Configuration_Properties/ESBEmailUser").InnerText);
                    Cache.SetCache("ESBEmailPassword", xdoc.SelectSingleNode("Configuration_Properties/ESBEmailPassword").InnerText);

                    Cache.SetCache("ESBSMSUser", xdoc.SelectSingleNode("Configuration_Properties/ESBSMSUser").InnerText);
                    Cache.SetCache("ESBSMSPass", xdoc.SelectSingleNode("Configuration_Properties/ESBSMSPass").InnerText);
                    Cache.SetCache("MOISMSUser", xdoc.SelectSingleNode("Configuration_Properties/MOISMSUser").InnerText);
                    Cache.SetCache("MOISMSPass", xdoc.SelectSingleNode("Configuration_Properties/MOISMSPass").InnerText);
                    Cache.SetCache("ESBECSUser", xdoc.SelectSingleNode("Configuration_Properties/ESBECSUser").InnerText);
                    Cache.SetCache("ESBECSPass", xdoc.SelectSingleNode("Configuration_Properties/ESBECSPass").InnerText);
                    
                    Cache.SetCache("FaxServer", xdoc.SelectSingleNode("Configuration_Properties/FaxServer").InnerText);
                    Cache.SetCache("FaxPass", xdoc.SelectSingleNode("Configuration_Properties/FaxPass").InnerText);
                    Cache.SetCache("FaxUser", xdoc.SelectSingleNode("Configuration_Properties/FaxUser").InnerText);
                    Cache.SetCache("FromFaxField", xdoc.SelectSingleNode("Configuration_Properties/FromFaxField").InnerText);
                    
                    Cache.SetCache("TokenPeriod", xdoc.SelectSingleNode("Configuration_Properties/TokenPeriod").InnerText);
                    Cache.SetCache("ESBHRUser", xdoc.SelectSingleNode("Configuration_Properties/ESBHRUser").InnerText);
                    Cache.SetCache("ESBHRPass", xdoc.SelectSingleNode("Configuration_Properties/ESBHRPass").InnerText);
                    Cache.SetCache("MOISMSChannel", xdoc.SelectSingleNode("Configuration_Properties/MOISMSChannel").InnerText);

                    Cache.SetCache("DMSUser", xdoc.SelectSingleNode("Configuration_Properties/DMSUser").InnerText);
                    Cache.SetCache("DMSPass", xdoc.SelectSingleNode("Configuration_Properties/DMSPass").InnerText);
                    

                    Cache.SetCache("IsSettingsLoaded", true);
                    return true;
                }
                else
                {
                    Cache.SetCache("IsSettingsLoaded", false);
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
        public static object GetSetting(string SettingsCacheKey, IHostingEnvironment _hostingEnvironment = null)
        {
            try
            {
                if (LoadSettings(_hostingEnvironment))
                {
                    return Cache.GetFromCache(SettingsCacheKey);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }
            
            
        }
        public static object SetSetting(Models.SettingModel settingModel)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                if (File.Exists(ConfigPath))
                {
                    xdoc.Load(ConfigPath);
                    if(settingModel.DBType != null) xdoc.SelectSingleNode("Configuration_Properties/DBType").InnerText = settingModel.DBType;
                    xdoc.SelectSingleNode("Configuration_Properties/UserName").InnerText = settingModel.UserName;
                    if(settingModel.Password != null)xdoc.SelectSingleNode("Configuration_Properties/Password").InnerText = settingModel.Password;
                    xdoc.SelectSingleNode("Configuration_Properties/APIURL").InnerText = settingModel.APIURL;
                    xdoc.SelectSingleNode("Configuration_Properties/ConnectionString").InnerText = settingModel.ConnectionString;
                    if (settingModel.DBPassword != null) xdoc.SelectSingleNode("Configuration_Properties/DBPassword").InnerText = settingModel.DBPassword;
                    //xdoc.SelectSingleNode("Configuration_Properties/EnableLogging").InnerText = settingModel.EnableLogging.ToString();
                    xdoc.SelectSingleNode("Configuration_Properties/EnableLoggingInfo").InnerText = settingModel.EnableLoggingInfo.ToString();
                    xdoc.SelectSingleNode("Configuration_Properties/IntegrationDataFolder").InnerText = settingModel.IntegrationDataFolder;
                    xdoc.SelectSingleNode("Configuration_Properties/DMSInterfaceURL").InnerText = settingModel.DMSInterfaceURL;
                    xdoc.SelectSingleNode("Configuration_Properties/SiteEndPoint").InnerText = settingModel.SiteEndPoint;
                    xdoc.SelectSingleNode("Configuration_Properties/DefaultReference").InnerText = settingModel.DefaultReference;
                    //
                    xdoc.SelectSingleNode("Configuration_Properties/EntityPrefix").InnerText = settingModel.EntityPrefix;
                    if (settingModel.ESBURL != null)xdoc.SelectSingleNode("Configuration_Properties/ESBURL").InnerText = settingModel.ESBURL;

                    xdoc.SelectSingleNode("Configuration_Properties/ESBEmail").InnerText = settingModel.ESBEmail;
                    xdoc.SelectSingleNode("Configuration_Properties/ESBEmailUser").InnerText = settingModel.ESBEmailUser;
                    if (settingModel.ESBEmailPassword != null) xdoc.SelectSingleNode("Configuration_Properties/ESBEmailPassword").InnerText = settingModel.ESBEmailPassword;

                    xdoc.SelectSingleNode("Configuration_Properties/ESBSMSUser").InnerText = settingModel.ESBSMSUser;
                    if (settingModel.ESBSMSPass != null) xdoc.SelectSingleNode("Configuration_Properties/ESBSMSPass").InnerText = settingModel.ESBSMSPass;
                    xdoc.SelectSingleNode("Configuration_Properties/MOISMSUser").InnerText = settingModel.MOISMSUser;
                    if (settingModel.MOISMSPass != null) xdoc.SelectSingleNode("Configuration_Properties/MOISMSPass").InnerText = settingModel.MOISMSPass;
                    xdoc.SelectSingleNode("Configuration_Properties/ESBECSUser").InnerText = settingModel.ESBECSUser;
                    if (settingModel.ESBECSPass != null) xdoc.SelectSingleNode("Configuration_Properties/ESBECSPass").InnerText = settingModel.ESBECSPass;
                    xdoc.SelectSingleNode("Configuration_Properties/TokenPeriod").InnerText = settingModel.TokenPeriod;
                    xdoc.SelectSingleNode("Configuration_Properties/ESBHRUser").InnerText = settingModel.ESBHRUser;
                    if (settingModel.ESBHRPass != null) xdoc.SelectSingleNode("Configuration_Properties/ESBHRPass").InnerText = settingModel.ESBHRPass;
                    xdoc.SelectSingleNode("Configuration_Properties/MOISMSChannel").InnerText = settingModel.MOISMSChannel;
                    
                    xdoc.SelectSingleNode("Configuration_Properties/DMSUser").InnerText = settingModel.DMSUser;
                    if (settingModel.DMSPass != null) xdoc.SelectSingleNode("Configuration_Properties/DMSPass").InnerText = settingModel.DMSPass;

                    xdoc.SelectSingleNode("Configuration_Properties/FaxServer").InnerText = settingModel.FaxServer;
                    xdoc.SelectSingleNode("Configuration_Properties/FaxUser").InnerText = settingModel.FaxUser;
                    xdoc.SelectSingleNode("Configuration_Properties/FaxPass").InnerText = settingModel.FaxPass;
                    xdoc.SelectSingleNode("Configuration_Properties/FromFaxField").InnerText = settingModel.FromFaxField;
                    //xdoc.SelectSingleNode("Configuration_Properties/RightFaxFolderPath").InnerText = settingModel.RightFaxFolderPath;
                    xdoc.SelectSingleNode("Configuration_Properties/TarasolINTUser").InnerText = settingModel.TarasolINTUser;
                    if (settingModel.TarasolINTPassword != null) xdoc.SelectSingleNode("Configuration_Properties/TarasolINTPassword").InnerText = settingModel.TarasolINTPassword;
                    //if(settingModel.DataFolder == null)xdoc.SelectSingleNode("Configuration_Properties/DataFolder").InnerText= settingModel.DataFolder;
                    //xdoc.SelectSingleNode("Configuration_Properties/ConfigPath").InnerText= settingModel.ConfigPath;
                    xdoc.Save(ConfigPath);
                    return true;
                }
                else
                {
                    Cache.SetCache("IsSettingsLoaded", false);
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }


        }

        public static bool testConnection(string ConnectionString,string password)
        {
            string DbType = GetSetting("DBType").ToString();
            string DBPassword = string.IsNullOrEmpty(password)?GetSetting("DBPassword").ToString():password;

            string connectionString = ConnectionString + DBPassword;
            if (DbType == "1")
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        return true;
                    }
                }
                   
                    
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        return true;
                    }
                }
                
                
            }
            return false;

        }
    }
}
