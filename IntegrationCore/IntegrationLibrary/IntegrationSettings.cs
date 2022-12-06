using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Configuration;
using ICALibrary.Models;
using System.Xml.Serialization;
using System.Text;

namespace IntegrationCore.IntegrationLibrary
{
    public class IntegrationSettings
    {
        //need to be in application path
        public static string ConfigPath = string.Empty;

        public IntegrationSettings(string configPath)
        {
            ConfigPath = configPath;
        }

        //public static Dictionary<String, String> key_value = new Dictionary<String, String>();
        public static bool LoadSettings(IHostingEnvironment _hostingEnvironment = null)
        {
            try
            {

                if (Cache.CheckAvailableCache())
                {
                    if (_hostingEnvironment != null && string.IsNullOrWhiteSpace(ConfigPath))
                    {
                        ConfigPath = _hostingEnvironment.ContentRootPath + @"\IntegrationSetting.config";
                    }

                    var cacheObject = Cache.GetFromCache("IsSettingsLoaded");
                    bool IsSettingsLoaded = (cacheObject is null) ? false : (bool)cacheObject;


                    XmlDocument xdoc = new XmlDocument();
                    if (File.Exists(ConfigPath) && !IsSettingsLoaded)
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
                        Cache.SetCache("DevelopSecKey", xdoc.SelectSingleNode("Configuration_Properties/DevelopSecKey").InnerText);
                        Cache.SetCache("APIURL", xdoc.SelectSingleNode("Configuration_Properties/APIURL").InnerText);
                        Cache.SetCache("DataFolder", xdoc.SelectSingleNode("Configuration_Properties/DataFolder").InnerText);
                        Cache.SetCache("EnableLogging", xdoc.SelectSingleNode("Configuration_Properties/EnableLogging").InnerText);
                        //Cache.SetCache("Logs", xdoc.SelectSingleNode("Configuration_Properties/log4net/appender/file").InnerText);
                        Cache.SetCache("IntegrationDataFolder", xdoc.SelectSingleNode("Configuration_Properties/IntegrationDataFolder").InnerText);
                        Cache.SetCache("DMSInterfaceURL", xdoc.SelectSingleNode("Configuration_Properties/DMSInterfaceURL").InnerText);
                        Cache.SetCache("SiteEndPoint", xdoc.SelectSingleNode("Configuration_Properties/SiteEndPoint").InnerText);
                        Cache.SetCache("RightFaxFolderPath", xdoc.SelectSingleNode("Configuration_Properties/RightFaxFolderPath").InnerText);
                        Cache.SetCache("TarasolINTPassword", xdoc.SelectSingleNode("Configuration_Properties/TarasolINTPassword").InnerText);
                        Cache.SetCache("TarasolINTUser", xdoc.SelectSingleNode("Configuration_Properties/TarasolINTUser").InnerText);
                        Cache.SetCache("ESBURL", xdoc.SelectSingleNode("Configuration_Properties/ESBURL").InnerText);
                        // Email
                        Cache.SetCache("ESBEmailL", xdoc.SelectSingleNode("Configuration_Properties/ESBEmailL").InnerText);
                        Cache.SetCache("ESBSMSUser", xdoc.SelectSingleNode("Configuration_Properties/ESBSMSUser").InnerText);
                        Cache.SetCache("ESBSMSPass", xdoc.SelectSingleNode("Configuration_Properties/ESBSMSPass").InnerText);
                        Cache.SetCache("ESBECSUser", xdoc.SelectSingleNode("Configuration_Properties/ESBECSUser").InnerText);
                        Cache.SetCache("ESBECSPass", xdoc.SelectSingleNode("Configuration_Properties/ESBECSPass").InnerText);
                        Cache.SetCache("TimeStartSendEmail", xdoc.SelectSingleNode("Configuration_Properties/TimeStartSendEmail").InnerText);
                        Cache.SetCache("ClassificationId", xdoc.SelectSingleNode("Configuration_Properties/ClassificationId").InnerText);
                        Cache.SetCache("EmailDistributors", xdoc.SelectSingleNode("Configuration_Properties/EmailDistributors").InnerText);
                        Cache.SetCache("EntityPrefix", xdoc.SelectSingleNode("Configuration_Properties/EntityPrefix").InnerText);
                        // Load code response of ESB
                        XmlNode xmlNode = xdoc.SelectSingleNode("Configuration_Properties/ResponseCodesESB");
                        if (xmlNode != null)
                        {
                            string xmlCodeResponseESB = xmlNode.OuterXml;
                            byte[] byteCodeResponseESB = Encoding.UTF8.GetBytes(xmlCodeResponseESB);
                            MemoryStream stream = new MemoryStream(byteCodeResponseESB);
                            ResponseCodesESB responseCodesESB = new ResponseCodesESB();
                            XmlSerializer deSerializer = new XmlSerializer(typeof(ResponseCodesESB),
                                new XmlRootAttribute("ResponseCodesESB"));
                            responseCodesESB = (ResponseCodesESB)deSerializer.Deserialize(stream);
                            Cache.SetCache("ResponseCodesESB", responseCodesESB);
                        }
                       // End load

                       Cache.SetCache("IsSettingsLoaded", true);
                        return true;
                    }
                    else if (IsSettingsLoaded)
                    {
                        return true;
                    }
                    else
                    {
                        if (Cache.CheckAvailableCache())
                            Cache.SetCache("IsSettingsLoaded", false);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                //
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

                throw ex;
            }


        }
        public static object SetSetting(Models.Setting settingModel)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                if (File.Exists(ConfigPath))
                {
                    xdoc.Load(ConfigPath);
                    xdoc.SelectSingleNode("Configuration_Properties/DBType").InnerText = settingModel.DBType;
                    xdoc.SelectSingleNode("Configuration_Properties/UserName").InnerText = settingModel.UserName;
                    xdoc.SelectSingleNode("Configuration_Properties/Password").InnerText = settingModel.Password;
                    xdoc.SelectSingleNode("Configuration_Properties/APIURL").InnerText = settingModel.APIURL;
                    //xdoc.SelectSingleNode("Configuration_Properties/ConfigPath").InnerText= settingModel.ConfigPath;
                    xdoc.SelectSingleNode("Configuration_Properties/ConnectionString").InnerText = settingModel.ConnectionString;
                    xdoc.SelectSingleNode("Configuration_Properties/DataFolder").InnerText = settingModel.DataFolder;
                    xdoc.SelectSingleNode("Configuration_Properties/DBPassword").InnerText = settingModel.DBPassword;
                    xdoc.SelectSingleNode("Configuration_Properties/EnableLogging").InnerText = settingModel.EnableLogging.ToString();
                    xdoc.SelectSingleNode("Configuration_Properties/IntegrationDataFolder").InnerText = settingModel.IntegrationDataFolder;
                    //xdoc.SelectSingleNode("Configuration_Properties/DMSInterfaceURL").InnerText = settingModel.DMSInterfaceURL;
                    //xdoc.SelectSingleNode("Configuration_Properties/SiteEndPoint").InnerText = settingModel.SiteEndPoint;
                    //xdoc.SelectSingleNode("Configuration_Properties/RightFaxFolderPath").InnerText = settingModel.RightFaxFolderPath;
                    //xdoc.SelectSingleNode("Configuration_Properties/TarasolINTUser").InnerText = settingModel.TarasolINTUser;
                    //xdoc.SelectSingleNode("Configuration_Properties/TarasolINTPassword").InnerText = settingModel.TarasolINTPassword;
                    xdoc.SelectSingleNode("Configuration_Properties/ESBURL").InnerText = settingModel.ESBURL;
                    //ESBEmailL
                    //xdoc.SelectSingleNode("Configuration_Properties/ESBEmailL").InnerText = settingModel.ESBEmailL;
                    //xdoc.SelectSingleNode("Configuration_Properties/ESBSMSUser").InnerText = settingModel.ESBSMSUser;
                    //xdoc.SelectSingleNode("Configuration_Properties/ESBSMSPass").InnerText = settingModel.ESBSMSPass;
                    //xdoc.SelectSingleNode("Configuration_Properties/ESBECSUser").InnerText = settingModel.ESBECSUser;
                    //xdoc.SelectSingleNode("Configuration_Properties/ESBECSPass").InnerText = settingModel.ESBECSPass;
                    xdoc.SelectSingleNode("Configuration_Properties/TimeStartSendEmail").InnerText = settingModel.TimeStartSendEmail;
                    xdoc.SelectSingleNode("Configuration_Properties/ClassificationId").InnerText = settingModel.ClassificationId;
                    xdoc.SelectSingleNode("Configuration_Properties/EntityPrefix").InnerText = settingModel.EntityPrefix;
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

                throw ex;
            }
        }
    }
}
