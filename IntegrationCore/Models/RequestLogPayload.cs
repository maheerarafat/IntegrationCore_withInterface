using System;

namespace IntegrationCore.Models
{
    public class RequestLogPayload
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public string RequestBody { get; set; }
        public string Token { get; set; }
    }
}
