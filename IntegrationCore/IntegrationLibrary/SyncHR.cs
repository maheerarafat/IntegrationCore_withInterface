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

        public static List<ICALibrary.Models.IntHistoryUpdateHR> GetHistoryUpdateHR(int pageNumber, int pageSize, ref int recordsTotal)
        {
            List<ICALibrary.Models.IntHistoryUpdateHR> result = null;
            List<ICALibrary.Models.CountTbl> count_result = null;

            try
            {
                //var dyParam = new OracleDynamicParameters();
                //dyParam.Add("EMP_ID", OracleDbType.Int32, ParameterDirection.Input, empId);
                //dyParam.Add("EMP_DETAIL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();

                    if (conn.State == ConnectionState.Open)
                    {
                        var query = String.Format("SELECT FLDID, FLDNAMETTABLE, FLDLASTUPDATEDATE FROM (SELECT a.*, rownum r__ FROM (select FLDID, FLDNAMETTABLE, FLDLASTUPDATEDATE from INTHistoryUpdateHR ORDER BY FLDLASTUPDATEDATE DESC ) a WHERE rownum < (({0} * {1}) + 1 ) ) WHERE r__ >= ((({0}-1) * {1}) + 1)",
                           pageNumber + 1, pageSize);

                        //result = SqlMapper.Query(conn, query, param: dyParam, commandType: CommandType.StoredProcedure);
                        object instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntHistoryUpdateHR), false);
                        result = SqlMapper.Query<ICALibrary.Models.IntHistoryUpdateHR>(conn, query, commandType: CommandType.Text).ToList();


                        query = "select count(*) as FLDCOUNT from INTHistoryUpdateHR";
                        count_result = SqlMapper.Query<ICALibrary.Models.CountTbl>(conn, query, commandType: CommandType.Text).ToList();

                        if (count_result.Count > 0)
                        {
                            recordsTotal = count_result.ElementAt(0).FLDCOUNT;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
                throw ex;
            }

            return result;
        }



        public static object GetActivityLogHR(int pageNumber, int pageSize, ref int recordsTotal)
        {
            List<ICALibrary.Models.IntActivityLogHR> result = null;
            List<ICALibrary.Models.CountTbl> count_result = null;

            try
            {
                //var dyParam = new OracleDynamicParameters();
                //dyParam.Add("EMP_ID", OracleDbType.Int32, ParameterDirection.Input, empId);
                //dyParam.Add("EMP_DETAIL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();
                    if (conn.State == ConnectionState.Open)
                    {
                        var query = String.Format("SELECT FLDID, FLDSYNCNAME, FLDDESCRIPTION, FLDDATE FROM (SELECT a.*, rownum r__ FROM (SELECT FLDID, FLDSYNCNAME, FLDDESCRIPTION, FLDDATE FROM INTACTIVTYLOGHR ORDER BY FLDDATE DESC, FLDTYPE DESC ) a WHERE rownum < (({0} * {1}) + 1 ) ) WHERE r__ >= ((({0}-1) * {1}) + 1)",
                            pageNumber + 1, pageSize);

                        //result = SqlMapper.Query(conn, query, param: dyParam, commandType: CommandType.StoredProcedure);
                        object instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntActivityLogHR), false);
                        result = SqlMapper.Query<ICALibrary.Models.IntActivityLogHR>(conn, query, commandType: CommandType.Text).ToList();



                        query = "select count(*) as FLDCOUNT from INTACTIVTYLOGHR";
                        count_result = SqlMapper.Query<ICALibrary.Models.CountTbl>(conn, query, commandType: CommandType.Text).ToList();

                        if (count_result.Count > 0)
                        {
                            recordsTotal = count_result.ElementAt(0).FLDCOUNT;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
                throw ex;
            }

            return result;
        }


        public static object GetActivityLogErrorHR(int pageNumber, int pageSize, ref int recordsTotal)
        {
            List<ICALibrary.Models.IntActivityLogErrorHR> result = null;
            List<ICALibrary.Models.CountTbl> count_result = null;

            try
            {
                //var dyParam = new OracleDynamicParameters();
                //dyParam.Add("EMP_ID", OracleDbType.Int32, ParameterDirection.Input, empId);
                //dyParam.Add("EMP_DETAIL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();

                    if (conn.State == ConnectionState.Open)
                    {
                        var query = String.Format("SELECT FLDID, FLDSYNCNAME, FLDDESCRIPTION, FLDDATE FROM (SELECT a.*, rownum r__ FROM (SELECT FLDID, FLDSYNCNAME, FLDDESCRIPTION, FLDDATE FROM INTACTIVTYLOGERRORHR ORDER BY FLDDATE DESC ) a WHERE rownum < (({0} * {1}) + 1 ) ) WHERE r__ >= ((({0}-1) * {1}) + 1)",
                            pageNumber + 1, pageSize);

                        //result = SqlMapper.Query(conn, query, param: dyParam, commandType: CommandType.StoredProcedure);
                        object instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntActivityLogErrorHR), false);
                        result = SqlMapper.Query<ICALibrary.Models.IntActivityLogErrorHR>(conn, query, commandType: CommandType.Text).ToList();
                        conn.Close();

                        query = "select count(*) as FLDCOUNT from INTACTIVTYLOGERRORHR";
                        count_result = SqlMapper.Query<ICALibrary.Models.CountTbl>(conn, query, commandType: CommandType.Text).ToList();

                        if (count_result.Count > 0)
                        {
                            recordsTotal = count_result.ElementAt(0).FLDCOUNT;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
                throw ex;
            }

            return result;
        }


        public static object RunProcedureSync(string id)
        {
            try
            {
                List<ICALibrary.Models.IntHistoryUpdateHR> result = null;
                String tableName = string.Empty;
                String ProcedureName = string.Empty;

                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();

                    if (conn.State == ConnectionState.Open)
                    {
                        // Get Table name to sync
                        var query = String.Format("select FLDID, FLDNAMETTABLE, FLDLASTUPDATEDATE, FLDPROCEDURENAME from INTHistoryUpdateHR WHERE FLDID = {0}", id);
                        result = SqlMapper.Query<ICALibrary.Models.IntHistoryUpdateHR>(conn, query, commandType: CommandType.Text).ToList();
                        if (result.Count > 0)
                        {
                            tableName = result.ElementAt(0).FLDNAMETTABLE;
                            ProcedureName = result.ElementAt(0).FLDProcedureName;

                            //Get Last Sync Date
                            IDbCommand cmd = conn.CreateCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = ProcedureName;
                            var parameter1 = cmd.CreateParameter();
                            parameter1.ParameterName = "view_name_sync";
                            parameter1.DbType = DbType.String;
                            parameter1.Direction = ParameterDirection.Input;
                            parameter1.Value = tableName;
                            cmd.Parameters.Add(parameter1);
                            IDataReader dataReader = cmd.ExecuteReader();
                        }

                    }
                }

                return IntResultLib.GetResult(1);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
                throw ex;
            }
        }

        public static object GetUserDetails(String num, int type)
        {
            List<ICALibrary.Models.IntUser> result = null;

            try
            {
                //var dyParam = new OracleDynamicParameters();
                //dyParam.Add("EMP_ID", OracleDbType.Int32, ParameterDirection.Input, empId);
                //dyParam.Add("EMP_DETAIL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();
                    if (conn.State == ConnectionState.Open)
                    {
                        var query = String.Format("select E.UAE_ID, E.MAIL, E.MOBILE_NO, E.EMP_NAME_A, E.EMP_NAME_E, E.RANK_ID, E.EMP_STATUS, E.LAST_DEPT_ID, R.RANK_NAME_A, D.DEPT_NAME_A " +
                            "from TRASUL_EMP_INFO E inner join TRASUL_RANK_VIEW R on R.RANK_ID = E.RANK_ID " +
                            "inner join TRASUL_DEPT_VIEW D on D.DEPT_ID = E.LAST_DEPT_ID " +
                            "where E.emp_no = {0} and E.type_id = {1}", num, type);

                        //result = SqlMapper.Query(conn, query, param: dyParam, commandType: CommandType.StoredProcedure);
                        object instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntUser), false);
                        result = SqlMapper.Query<ICALibrary.Models.IntUser>(conn, query, commandType: CommandType.Text).ToList();
                    }

                    if (result.Count == 0)
                    {
                        Exception exception = new Exception("Not Found");
                        throw exception;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
                throw ex;
            }

            return result;
        }


        public static object GetUserMobileLookup(String num)
        {
            List<Models.IntUser> result = new List<Models.IntUser>();

            try
            {
                //var dyParam = new OracleDynamicParameters();
                //dyParam.Add("EMP_ID", OracleDbType.Int32, ParameterDirection.Input, empId);
                //dyParam.Add("EMP_DETAIL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();
                    if (conn.State == ConnectionState.Open)
                    {

                        IDbCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "OTP.GetMobileNumber";



                        var returnedValue = cmd.CreateParameter();
                        returnedValue.ParameterName = "result";
                        returnedValue.DbType = DbType.String;
                        returnedValue.Direction = ParameterDirection.ReturnValue;
                        returnedValue.Size = 50;
                        cmd.Parameters.Add(returnedValue);

                        var par1 = cmd.CreateParameter();
                        par1.ParameterName = "username";
                        par1.SourceColumn = "username";
                        par1.DbType = DbType.String;
                        par1.Size = 50;
                        par1.Direction = ParameterDirection.Input;
                        par1.Value = num;
                        cmd.Parameters.Add(par1);
                        // Call Read before accessing data.
                        IDataReader dataReader = cmd.ExecuteReader();
                        if (returnedValue.Value != null)
                        {
                            result.Add(new Models.IntUser()
                            {
                                MOBILE_NO = returnedValue.Value.ToString()
                            });

                        }
                    }

                    if (result.Count == 0)
                    {
                        Exception exception = new Exception("Not Found");
                        throw exception;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
                throw ex;
            }

            return result;
        }


        public static object UpdateUser(ICALibrary.Models.IntUserRequest user)
        {
            List<ICALibrary.Models.UserTarasol> result = null;

            try
            {
                String prefixName = String.Empty;
                String GUID = String.Empty;
                String nickName = String.Empty;
                String nickNamePrefix = String.Empty;


                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();
                    if (conn.State == ConnectionState.Open)
                    {

                        var query = String.Format("select flduid, fldnickname from tsusers where lower(fldnickname) = lower('{0}')", user.ecs.ToLower());
                        //result = SqlMapper.Query(conn, query, param: dyParam, commandType: CommandType.StoredProcedure);
                        object instance = Activator.CreateInstance(typeof(ICALibrary.Models.UserTarasol), false);
                        result = SqlMapper.Query<ICALibrary.Models.UserTarasol>(conn, query, commandType: CommandType.Text).ToList();

                        if (result.Count > 0)
                        {
                            GUID = result.ElementAt(0).flduid;
                            nickName = result.ElementAt(0).fldnickname;
                        }
                        else
                        {
                            Exception exception = new Exception("Not Found");
                            throw exception;
                        }

                        nickNamePrefix = ICALibrary.Helper.Common.GetPrefixName(nickName);
                        int resNum = conn.Execute(String.Format("update tsusers u set lower(u.fldnickname) = lower('{0}') where u.flduid = '{1}'", nickNamePrefix + user.empno, GUID));

                        // run procuder 
                        String ProcedureName = "syncecs2hr";
                        String para1 = "FLDUSERID";
                        String para2 = "FLDCODE";

                        IDbCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = ProcedureName;
                        var parameter1 = cmd.CreateParameter();
                        parameter1.ParameterName = para1;
                        parameter1.DbType = DbType.String;
                        parameter1.Direction = ParameterDirection.Input;
                        parameter1.Value = GUID;
                        cmd.Parameters.Add(parameter1);
                        var parameter2 = cmd.CreateParameter();
                        parameter2.ParameterName = para2;
                        parameter2.DbType = DbType.String;
                        parameter2.Direction = ParameterDirection.Input;
                        parameter2.Value = user.code;
                        cmd.Parameters.Add(parameter2);
                        IDataReader dataReader = cmd.ExecuteReader();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
                throw ex;
            }
            return result;
        }


        public static void UpdateSecurityKeyUser(string securityKey, string userNumber)
        {
            try
            {
                String prefixName = String.Empty;
                String GUID = String.Empty;
                String nickName = String.Empty;
                String nickNamePrefix = String.Empty;


                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();

                    if (conn.State == ConnectionState.Open)
                    {
                        IDbCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "OTP.SetSecurityCode";



                        var returnedValue = cmd.CreateParameter();
                        returnedValue.ParameterName = "result";
                        returnedValue.DbType = DbType.Boolean;
                        returnedValue.Direction = ParameterDirection.ReturnValue;
                        //returnedValue.Size = 50;
                        cmd.Parameters.Add(returnedValue);

                        var par1 = cmd.CreateParameter();
                        par1.ParameterName = "username";
                        par1.SourceColumn = "username";
                        par1.DbType = DbType.String;
                        par1.Size = 50;
                        par1.Direction = ParameterDirection.Input;
                        par1.Value = userNumber;
                        cmd.Parameters.Add(par1);

                        var par2 = cmd.CreateParameter();
                        par2.ParameterName = "Key";
                        par2.SourceColumn = "Key";
                        par2.DbType = DbType.String;
                        par2.Size = 40;
                        par2.Direction = ParameterDirection.Input;
                        par2.Value = securityKey;
                        cmd.Parameters.Add(par2);

                        // Call Read before accessing data.
                        IDataReader dataReader = cmd.ExecuteReader();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
                throw ex;
            }
        }


        public static void UpdatePassword(string username, string password)
        {
            try
            {



                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();

                    if (conn.State == ConnectionState.Open)
                    {
                        IDbCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "otp.ChangePassword";



                        var returnedValue = cmd.CreateParameter();
                        returnedValue.ParameterName = "result";
                        returnedValue.DbType = DbType.Boolean;
                        returnedValue.Direction = ParameterDirection.ReturnValue;
                        //returnedValue.Size = 50;
                        cmd.Parameters.Add(returnedValue);

                        var par1 = cmd.CreateParameter();
                        par1.ParameterName = "username";
                        par1.SourceColumn = "username";
                        par1.DbType = DbType.String;
                        par1.Size = 50;
                        par1.Direction = ParameterDirection.Input;
                        par1.Value = username;
                        cmd.Parameters.Add(par1);

                        var par2 = cmd.CreateParameter();
                        par2.ParameterName = "password";
                        par2.SourceColumn = "password";
                        par2.DbType = DbType.String;
                        par2.Size = 255;
                        par2.Direction = ParameterDirection.Input;
                        par2.Value = password;
                        cmd.Parameters.Add(par2);

                        // Call Read before accessing data.
                        IDataReader dataReader = cmd.ExecuteReader();

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
                throw ex;
            }
        }

        public static string GetUserSecuirtyLookup(String num)
        {
            List<Models.IntUser> result = new List<Models.IntUser>();
            string secuirtyKey = string.Empty;

            try
            {
                //var dyParam = new OracleDynamicParameters();
                //dyParam.Add("EMP_ID", OracleDbType.Int32, ParameterDirection.Input, empId);
                //dyParam.Add("EMP_DETAIL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();

                    if (conn.State == ConnectionState.Open)
                    {
                        IDbCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "otp.GetSecurityCode";



                        var returnedValue = cmd.CreateParameter();
                        returnedValue.ParameterName = "result";
                        returnedValue.DbType = DbType.String;
                        returnedValue.Direction = ParameterDirection.ReturnValue;
                        returnedValue.Size = 40;
                        cmd.Parameters.Add(returnedValue);

                        var par1 = cmd.CreateParameter();
                        par1.ParameterName = "username";
                        par1.SourceColumn = "username";
                        par1.DbType = DbType.String;
                        par1.Size = 50;
                        par1.Direction = ParameterDirection.Input;
                        par1.Value = num;
                        cmd.Parameters.Add(par1);

                        IDataReader dataReader = cmd.ExecuteReader();
                        if (returnedValue.Value != null)
                        {
                            result.Add(new Models.IntUser()
                            {
                                SECURITYKEY = returnedValue.Value.ToString()
                            });

                        }
                    }

                    if (result.Count == 0 || result.Count > 1)
                    {
                        Exception exception = new Exception("Not Found");
                        throw exception;
                    }
                    else
                    {
                        secuirtyKey = result.ElementAt(0).SECURITYKEY;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("SyncHR." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
                throw ex;
            }

            return secuirtyKey;
        }


    }
}
