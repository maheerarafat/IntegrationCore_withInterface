using IntegrationCore.IntegrationLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Helper
{
    public class Common
    {
        public static string CheckEmptyString(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            return string.Empty;
        }

        public static string RemoveZeroLeadingPhonenumber(string input)
        {
            if (input.StartsWith("0"))
            {
                return input.Remove(0, 1);
            }

            return input;
        }

        public static string RemovePrefixEntity(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                string replace = (string)IntegrationSettings.GetSetting("EntityPrefix");
                if (!string.IsNullOrWhiteSpace(replace))
                {
                    return input.Replace((string)IntegrationSettings.GetSetting("EntityPrefix"), String.Empty);
                }
            }
            return input;
        }

        public static string AddPrefixEntity(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                return $"{(string)IntegrationSettings.GetSetting("EntityPrefix")}{input}";
            }
            return string.Empty;
        }

        public static int GetReportDirectoriesCount(DirectoryInfo directoryInfo, string startDate)
        {
            return directoryInfo.GetDirectories().Where(e => e.CreationTime.ToString("yyyyMMdd") == startDate && e.Name.Length > 30)
                .ToList().Count();
        }
    }
}
