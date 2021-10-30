using IntegrationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.IntegrationLibrary
{
    public class IntResultLib
    {
        public static IntResult GetResult(Exception ex)
        {
            IntResult eSBResult = new IntResult
            {
                Code = ex.HResult,
                EMessage = ex.Message,
                LMessage = ex.Message 
            };
            return eSBResult;
        }
        public static IntResult GetResult(int code)
        {
            IntResult eSBResult = new IntResult();
            if (code == 2)
            {
                eSBResult.Code = 2;
                eSBResult.EMessage = "Input data not valid";
                eSBResult.LMessage = "البيانات المرسلة غير صحيحة";
            }
            else if (code == 200)
            {
                eSBResult.Code = 200;
                eSBResult.EMessage = "Success";
                eSBResult.LMessage = "تمت العملية بنجاح";
            }
            else if (code == 4)
            {
                eSBResult.Code = 2;
                eSBResult.EMessage = "Input data not valid";
                eSBResult.LMessage = "رمز التأكيد غير صحيح";
            }
            else if (code == 0)
            {
                eSBResult.Code = 0;
                eSBResult.EMessage = "Failed";
                eSBResult.LMessage = "فشلت عملية الإرسال";
            }
            else
            {
                eSBResult.Code = 1;
                eSBResult.EMessage = "Success";
                eSBResult.LMessage = "تمت العملية بنجاح";
            }
           
            return eSBResult;
        }
    }
}
