using System;
using System.Collections.Generic;
using System.Text;

namespace ICALibrary.Helper
{
    public class Enum
    {
        /// <summary>
        /// 
        /// </summary>
        public enum lang
        {
            English,
            Arabic 
        }

        /// <summary>
        /// 
        /// </summary>
        public enum responseCode
        {
            SuccessNotify,
            Success,
            Failed
        }

        public enum responseStatusNotification
        {
            NotTryAgain=0,
            TryAgain=2
        }


        public enum scheduleType
        {
            Done,
            Progress,
            NotStarted
        }
    }
}
