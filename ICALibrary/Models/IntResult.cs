using System;
using System.Collections.Generic;
using System.Text;
using static ICALibrary.Helper.Enum;

namespace ICALibrary.Models
{
    public class IntResult
    {
        public int Code { get; set; }
        public string LMessage { get; set; }
        public string EMessage { get; set; }
        public string Details { get; set; }

        public string Entity { get; set; }

        public responseStatusNotification statusNotification { get; set; } = responseStatusNotification.NotTryAgain;
    }
}
