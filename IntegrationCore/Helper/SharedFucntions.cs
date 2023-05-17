using System.Text.RegularExpressions;
namespace IntegrationCore.Helper
{
    public class SharedFucntions
    {
        public string Sanitize(string value , string field)
        {
            string result = value;
            if (!string.IsNullOrEmpty(value)) 
            {
                switch (field)
                {
                    case "AttName":
                        Regex re = new System.Text.RegularExpressions.Regex("[;\\\\/:*@#!'$%^&\\[\\]()?\"<>+=~|&{};']");
                        result =  re.Replace(value, "");
                        result = result.Replace(" ", "_").Replace(System.Environment.NewLine,string.Empty).Replace("\u0640" ,"");
                        
                        break;
                        
                }

            }
        return value;
        }
    }
}
