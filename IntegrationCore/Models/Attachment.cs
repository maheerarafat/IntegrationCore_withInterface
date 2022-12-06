

namespace IntegrationCore.Models
{
    public class Attachment
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContent { get; set; } // base64 format
        public string FileDescription { get; set; }
        public string FileNumber { get; set; }

    }
}
