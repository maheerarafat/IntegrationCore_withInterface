using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace IntegrationCore.IntegrationLibrary
{
    public class SyncHR
    {


        //public static object GetUserMobileLookup(String num)
        //{
        //    List<Models.IntUser> result = new List<Models.IntUser>();

        //    try
        //    {
        //        //var dyParam = new OracleDynamicParameters();
        //        //dyParam.Add("EMP_ID", OracleDbType.Int32, ParameterDirection.Input, empId);
        //        //dyParam.Add("EMP_DETAIL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

        //        var conn = ConnectionManager.GetConnection();
        //        if (conn.State == ConnectionState.Closed)
        //        {
        //            conn.Open();
        //        }

        //        if (conn.State == ConnectionState.Open)
        //        {
                    
        //            IDbCommand cmd = conn.CreateCommand();
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "otp.GetMobileNumber";

                    

        //            var returnedValue = cmd.CreateParameter();
        //            returnedValue.ParameterName = "result";
        //            returnedValue.DbType = DbType.String;
        //            returnedValue.Direction = ParameterDirection.ReturnValue;
        //            returnedValue.Size = 50;
        //            cmd.Parameters.Add(returnedValue);

        //            var par1 = cmd.CreateParameter();
        //            par1.ParameterName = "username";
        //            par1.SourceColumn = "username";
        //            par1.DbType = DbType.String;
        //            par1.Size = 50;
        //            par1.Direction = ParameterDirection.Input;
        //            par1.Value = num;
        //            cmd.Parameters.Add(par1);
        //            // Call Read before accessing data.
        //            IDataReader dataReader = cmd.ExecuteReader();
        //            if(returnedValue.Value != null)
        //            {
        //                result.Add(new Models.IntUser()
        //                {
        //                    MOBILE_NO = returnedValue.Value.ToString()
        //                });

        //            }
        //            conn.Close();
        //        }

        //        if (result.Count == 0)
        //        {
        //            Exception exception = new Exception("Not Found");
        //            throw exception;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
        //        throw;
        //    }

        //    return result;
        //}


        //public static void UpdateSecurityKeyUser(string securityKey, string userNumber)
        //{
        //    try
        //    {
        //        String prefixName = String.Empty;
        //        String GUID = String.Empty;
        //        String nickName = String.Empty;
        //        String nickNamePrefix = String.Empty;


        //        var conn = ConnectionManager.GetConnection();
        //        if (conn.State == ConnectionState.Closed)
        //        {
        //            conn.Open();
        //        }

        //        if (conn.State == ConnectionState.Open)
        //        {
        //            IDbCommand cmd = conn.CreateCommand();
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "otp.SetSecurityCode";



        //            var returnedValue = cmd.CreateParameter();
        //            returnedValue.ParameterName = "result";
        //            returnedValue.DbType = DbType.Boolean;
        //            returnedValue.Direction = ParameterDirection.ReturnValue;
        //            //returnedValue.Size = 50;
        //            cmd.Parameters.Add(returnedValue);

        //            var par1 = cmd.CreateParameter();
        //            par1.ParameterName = "username";
        //            par1.SourceColumn = "username";
        //            par1.DbType = DbType.String;
        //            par1.Size = 50;
        //            par1.Direction = ParameterDirection.Input;
        //            par1.Value = userNumber ;
        //            cmd.Parameters.Add(par1);

        //            var par2 = cmd.CreateParameter();
        //            par2.ParameterName = "Key";
        //            par2.SourceColumn = "Key";
        //            par2.DbType = DbType.String;
        //            par2.Size = 40;
        //            par2.Direction = ParameterDirection.Input;
        //            par2.Value = securityKey;
        //            cmd.Parameters.Add(par2);

        //            // Call Read before accessing data.
        //            IDataReader dataReader = cmd.ExecuteReader();
                    
        //            conn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
        //        throw;
        //    }
        //}
        //public static void UpdatePassword(string username, string password)
        //{
        //    try
        //    {
                


        //        var conn = ConnectionManager.GetConnection();
        //        if (conn.State == ConnectionState.Closed)
        //        {
        //            conn.Open();
        //        }

        //        if (conn.State == ConnectionState.Open)
        //        {
        //            IDbCommand cmd = conn.CreateCommand();
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "otp.ChangePassword";



        //            var returnedValue = cmd.CreateParameter();
        //            returnedValue.ParameterName = "result";
        //            returnedValue.DbType = DbType.Boolean;
        //            returnedValue.Direction = ParameterDirection.ReturnValue;
        //            //returnedValue.Size = 50;
        //            cmd.Parameters.Add(returnedValue);

        //            var par1 = cmd.CreateParameter();
        //            par1.ParameterName = "username";
        //            par1.SourceColumn = "username";
        //            par1.DbType = DbType.String;
        //            par1.Size = 50;
        //            par1.Direction = ParameterDirection.Input;
        //            par1.Value = username;
        //            cmd.Parameters.Add(par1);

        //            var par2 = cmd.CreateParameter();
        //            par2.ParameterName = "password";
        //            par2.SourceColumn = "password";
        //            par2.DbType = DbType.String;
        //            par2.Size = 255;
        //            par2.Direction = ParameterDirection.Input;
        //            par2.Value = password;
        //            cmd.Parameters.Add(par2);

        //            // Call Read before accessing data.
        //            IDataReader dataReader = cmd.ExecuteReader();

        //            conn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
        //        throw;
        //    }
        //}
        //public static string GetUserSecuirtyLookup(String num)
        //{
        //    List<Models.IntUser> result = new List<Models.IntUser>();
        //    string secuirtyKey = string.Empty;

        //    try
        //    {
        //        //var dyParam = new OracleDynamicParameters();
        //        //dyParam.Add("EMP_ID", OracleDbType.Int32, ParameterDirection.Input, empId);
        //        //dyParam.Add("EMP_DETAIL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

        //        var conn = ConnectionManager.GetConnection();
        //        if (conn.State == ConnectionState.Closed)
        //        {
        //            conn.Open();
        //        }

        //        if (conn.State == ConnectionState.Open)
        //        {
        //            IDbCommand cmd = conn.CreateCommand();
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "otp.GetSecurityCode";



        //            var returnedValue = cmd.CreateParameter();
        //            returnedValue.ParameterName = "result";
        //            returnedValue.DbType = DbType.String;
        //            returnedValue.Direction = ParameterDirection.ReturnValue;
        //            returnedValue.Size = 40;
        //            cmd.Parameters.Add(returnedValue);

        //            var par1 = cmd.CreateParameter();
        //            par1.ParameterName = "username";
        //            par1.SourceColumn = "username";
        //            par1.DbType = DbType.String;
        //            par1.Size = 50;
        //            par1.Direction = ParameterDirection.Input;
        //            par1.Value = num;
        //            cmd.Parameters.Add(par1);
                     
        //            IDataReader dataReader = cmd.ExecuteReader();
        //            if (returnedValue.Value != null)
        //            {
        //                result.Add(new Models.IntUser()
        //                {
        //                    SECURITYKEY = returnedValue.Value.ToString()
        //                });

        //            }
        //            conn.Close();
                    
        //        }

        //        if (result.Count == 0 || result.Count > 1)
        //        {
        //            Exception exception = new Exception("Not Found");
        //            throw exception;
        //        }else
        //        {
        //            secuirtyKey = result.ElementAt(0).SECURITYKEY;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
        //        throw;
        //    }

        //    return secuirtyKey;
        //}
    }
}
