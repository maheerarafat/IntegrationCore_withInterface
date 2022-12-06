using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using IntegrationCore.Models;

namespace IntegrationCore.IntegrationLibrary
{
    public class Jobs
    {
        public static object GetLogJobESBSuccuss(int pageNumber, int pageSize, string searchStr, ref int recordsTotal)
        {
            List<ICALibrary.Models.IntJobEsb> result = null;
            List<ICALibrary.Models.CountTbl> count_result = null;
            string serachWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                serachWhere = $" and (flddescription like '%{searchStr}%' or fldname like '%{searchStr}%' ) ";
            }

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

                        var query = String.Format("SELECT fldid, fldname, flddescription, flddateStart, flddateEnd, fldduration " +
                            " FROM (SELECT a.*, rownum r__ FROM (SELECT FLDid, FLDname, Flddescription, Flddatestart, Flddateend, Fldduration " +
                            $" FROM INTJobEsb WHERE Fldtype = {(int)ICALibrary.Helper.Enum.responseCode.Success} {serachWhere} ORDER BY Flddatestart DESC ) a " +
                            " WHERE rownum < (({0} * {1}) + 1 ) ) WHERE r__ >= ((({0}-1) * {1}) + 1)",
                            pageNumber + 1, pageSize);



                        object instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntJobEsb), false);
                        result = SqlMapper.Query<ICALibrary.Models.IntJobEsb>(conn, query, commandType: CommandType.Text).ToList();



                        query = $"select count(*) as FLDCOUNT from INTJobEsb WHERE Fldtype = {(int)ICALibrary.Helper.Enum.responseCode.Success} {serachWhere}";
                        count_result = SqlMapper.Query<ICALibrary.Models.CountTbl>(conn, query, commandType: CommandType.Text).ToList();

                        if (count_result.Count > 0)
                        {
                            recordsTotal = count_result.ElementAt(0).FLDCOUNT;
                        }
                    }
                }


            }
            catch (OracleException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public static object GetLogJobESBFaild(int pageNumber, int pageSize, string searchStr, ref int recordsTotal)
        {
            List<ICALibrary.Models.IntJobEsb> result = null;
            List<ICALibrary.Models.CountTbl> count_result = null;
            string serachWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                serachWhere = $" and ( flddescription like '%{searchStr}%' or fldname like '%{searchStr}%' ) ";
            }

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
                        var query = String.Format("SELECT FLDid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest" +
                            " FROM (SELECT a.*, rownum r__ FROM (SELECT FLDid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest " +
                            $" FROM INTJobEsb WHERE Fldtype = {(int)ICALibrary.Helper.Enum.responseCode.Failed} {serachWhere} ORDER BY Flddatestart DESC ) a" +
                            " WHERE rownum < (({0} * {1}) + 1 ) ) WHERE r__ >= ((({0}-1) * {1}) + 1)",
                            pageNumber + 1, pageSize);

                        object instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntJobEsb), false);
                        result = SqlMapper.Query<ICALibrary.Models.IntJobEsb>(conn, query, commandType: CommandType.Text).ToList();



                        query = $"select count(*) as FLDCOUNT from INTJobEsb WHERE Fldtype = {(int)ICALibrary.Helper.Enum.responseCode.Failed} {serachWhere}";
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
                throw ex;
            }

            return result;
        }

        public static object GetLogJobESBWait(int pageNumber, int pageSize, ref int recordsTotal)
        {
            List<ICALibrary.Models.IntJobEsb> result = null;
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
                        var query = String.Format("SELECT FLDid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest" +
                            " FROM (SELECT a.*, rownum r__ FROM (SELECT FLDid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest " +
                            $" FROM INTJobEsb WHERE Fldtype = {(int)ICALibrary.Helper.Enum.responseCode.Failed} " +
                            " and fldnumbertimesendschudle >= {2} and fldsendschudle = {3} ORDER BY Flddatestart DESC ) a WHERE rownum < (({0} * {1}) + 1 ) ) WHERE r__ >= ((({0}-1) * {1}) + 1)",
                            pageNumber + 1, pageSize, ICALibrary.Helper.Constant.MaxNumberSchedule, (int)ICALibrary.Helper.Enum.scheduleType.NotStarted);

                        object instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntJobEsb), false);
                        result = SqlMapper.Query<ICALibrary.Models.IntJobEsb>(conn, query, commandType: CommandType.Text).ToList();

                        foreach (var item in result)
                        {
                            if (!String.IsNullOrWhiteSpace(item?.fldbodyrequest))
                            {
                                item.fldbodyrequest = System.Net.WebUtility.HtmlEncode(item?.fldbodyrequest);  //System.Web.HttpUtility.HtmlEncode(item.fldbodyrequest);
                                item.fldbodyrequest = item?.fldbodyrequest.Replace(" ", "%20");
                            }
                        }

                        query = String.Format($"select count(*) as FLDCOUNT from INTJobEsb WHERE Fldtype = {(int)ICALibrary.Helper.Enum.responseCode.Failed} " +
                            " and fldnumbertimesendschudle >= {0} and fldsendschudle = {1}",
                            ICALibrary.Helper.Constant.MaxNumberSchedule, (int)ICALibrary.Helper.Enum.scheduleType.NotStarted);
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
                throw ex;
            }

            return result;
        }

        public static object GetLogJobESBInProgress(int pageNumber, int pageSize, ref int recordsTotal)
        {
            List<ICALibrary.Models.IntJobEsb> result = null;
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
                        var query = String.Format("SELECT FLDid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest, fldnumbertimesendschudle" +
                            " FROM (SELECT a.*, rownum r__ FROM (SELECT FLDid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest, fldnumbertimesendschudle " +
                            $" FROM INTJobEsb WHERE Fldtype = {(int)ICALibrary.Helper.Enum.responseCode.Failed} " +
                            " and fldnumbertimesendschudle < {2} and fldsendschudle = {3} ORDER BY Flddatestart DESC ) a WHERE rownum < (({0} * {1}) + 1 ) ) WHERE r__ >= ((({0}-1) * {1}) + 1)",
                            pageNumber + 1, pageSize, ICALibrary.Helper.Constant.MaxNumberSchedule, (int)ICALibrary.Helper.Enum.scheduleType.NotStarted);

                        object instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntJobEsb), false);
                        result = SqlMapper.Query<ICALibrary.Models.IntJobEsb>(conn, query, commandType: CommandType.Text).ToList();



                        query = String.Format($"select count(*) as FLDCOUNT from INTJobEsb WHERE Fldtype = {(int)ICALibrary.Helper.Enum.responseCode.Failed} " +
                            " and fldnumbertimesendschudle < {0} and fldsendschudle = {1}",
                            ICALibrary.Helper.Constant.MaxNumberSchedule, (int)ICALibrary.Helper.Enum.scheduleType.NotStarted);
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
                throw ex;
            }

            return result;
        }

        public static object AddLogJobESB(string name, string description, int type, DateTime startDate, DateTime endDate, Double duration, String bodyRequest, int statusSchedule)
        {
            object result = null;
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
                        string startDateStr = startDate.ToString("dd/MM/yyyy HH:mm:ss");
                        string endDateStr = endDate.ToString("dd/MM/yyyy HH:mm:ss");

                        var query = String.Format("INSERT INTO INTJobEsb (FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, FLDBODYREQUEST, FLDSENDSCHUDLE, FLDNUMBERTIMESENDSCHUDLE) " +
                            "VALUES ('{0}', '{1}',{2}, TO_DATE('{3}', 'DD/MM/YYYY HH24:MI:SS') , TO_DATE('{4}', 'DD/MM/YYYY HH24:MI:SS'), {5}, '{6}', {7}, {8})",
                            name, description, type, startDateStr, endDateStr, duration, bodyRequest, statusSchedule, 1);
                        int res = conn.Execute(query);


                    }
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }


        public static async Task ProccessSchedule(ConfigAttribute configAttribute)
        {
            List<ICALibrary.Models.IntJobEsb> result = null;
            string transactionRefNo = string.Empty;

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
                        var query = String.Format("select FLDid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest, FLDNUMBERTIMESENDSCHUDLE from INTJobEsb where fldsendschudle = {0} and fldnumbertimesendschudle < {1}",
                             (int)ICALibrary.Helper.Enum.scheduleType.NotStarted, ICALibrary.Helper.Constant.MaxNumberSchedule);

                        object instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntJobEsb), false);
                        result = SqlMapper.Query<ICALibrary.Models.IntJobEsb>(conn, query, commandType: CommandType.Text).ToList();


                    }

                    foreach (var item in result)
                    {
                        try
                        {
                            if (item.fldname.StartsWith(ICALibrary.Helper.Constant.MethodSendSMSICA) && item.fldnumbertimesendschudle < ICALibrary.Helper.Constant.MaxNumberSchedule)
                            {
                                List<SMSObject> sMSObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SMSObject>>(item.fldbodyrequest);
                                IntResult jsonResult = SMS.SendSMSICA(sMSObject, configAttribute, out transactionRefNo);
                                if (jsonResult.Code == (int)ICALibrary.Helper.Enum.responseCode.Success)
                                {
                                    if (conn.State == ConnectionState.Open)
                                    {
                                        var query = String.Format("update INTJobEsb set fldsendschudle = {0} where fldid = {1} ", (int)ICALibrary.Helper.Enum.scheduleType.Done, item.fldid);
                                        int res = conn.Execute(query);
                                    }
                                }
                                else
                                {
                                    if (conn.State == ConnectionState.Open)
                                    {

                                        var query = String.Format("update INTJobEsb set fldnumbertimesendschudle = {0} where fldid = {1} ", item.fldnumbertimesendschudle + 1, item.fldid);
                                        int res = conn.Execute(query);
                                    }
                                }

                            }
                        }
                        catch (Exception)
                        {
                            if (conn.State == ConnectionState.Open)
                            {

                                var query = String.Format("update INTJobEsb set fldnumbertimesendschudle = {0} where fldid = {1} ", item.fldnumbertimesendschudle + 1, item.fldid);
                                int res = conn.Execute(query);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //if (conn.State == ConnectionState.Open)
                //    conn.Close();
                throw ex;
            }

        }


        public static object GetReportJobESB(ReportFilter report)
        {

            ReportResult reportResult = new ReportResult();
            List<ICALibrary.Models.IntJobEsb> result = null;
            string todayDate = DateTime.Now.ToString("yyyyMMdd");

            try
            {
                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();
                    if (conn.State == ConnectionState.Open)
                    {

                        var query = String.Format("select fldtype, count(*) cnt from " +
                            " ( select distinct e.fldtype, e.flddatestart from INTJobEsb e " +
                            " where e.fldtype in ({0},{1}) and e.flddatestart >= to_date('{2}','yyyy-MM-dd') " +
                            " ) " +
                            " group by fldtype ",
                            (int)ICALibrary.Helper.Enum.responseCode.Success, (int)ICALibrary.Helper.Enum.responseCode.Failed, report.Date);


                        object instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntJobEsb), false);
                        result = SqlMapper.Query<ICALibrary.Models.IntJobEsb>(conn, query, commandType: CommandType.Text).ToList();

                        reportResult.Resources = result;

                        query = "select count(*) cnt from (select distinct e.fldname, e.flddatestart " +
                            " from INTJobEsb e " +
                            $" where e.fldtype = {(int)ICALibrary.Helper.Enum.responseCode.Failed} and TO_CHAR(flddatestart,'YYYYMMDD') = '{todayDate}')";

                        instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntJobEsb), false);
                        ICALibrary.Models.IntJobEsb item = SqlMapper.Query<ICALibrary.Models.IntJobEsb>(conn, query, commandType: CommandType.Text).FirstOrDefault();

                        if (item != null)
                        {
                            reportResult.Incident = item.cnt;
                        }

                        query = " select fldname, count(*) cnt  from ( " +
                                "  select regexp_substr(fldname,'(.*?)(:|$)',1,1,null,1) fldname from( " +
                                "    select distinct e.fldname, e.flddatestart from INTJobEsb e " +
                                $"    where e.fldtype = {(int)ICALibrary.Helper.Enum.responseCode.Failed} and TO_CHAR(flddatestart,'YYYYMMDD') = '{todayDate}' " +
                                "  ) " +
                                " ) " +
                                " group by fldname";

                        instance = Activator.CreateInstance(typeof(ICALibrary.Models.IntJobEsb), false);
                        result = SqlMapper.Query<ICALibrary.Models.IntJobEsb>(conn, query, commandType: CommandType.Text).ToList();

                        reportResult.ServicesCount = result;

                        string InDraftDirectory = (string)IntegrationSettings.GetSetting("IntegrationDataFolder") + "/In/Draft";
                        System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(InDraftDirectory);
                        reportResult.CountFolderInDraft = Helper.Common.GetReportDirectoriesCount(directory, todayDate);

                        reportResult.Date = DateTime.Now.ToString("yyyy/MM/dd");

                    }
                }


            }
            catch (OracleException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return reportResult;
        }


        public static void UpdateProccessScheduleStatus(string fldid, int status)
        {
            List<ICALibrary.Models.IntJobEsb> result = null;

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
                        var query = String.Format("update INTJobEsb set fldsendschudle = {0} where fldid = {1} ", status, fldid);
                        // 
                        int res = conn.Execute(query);
                    }
                }

            }
            catch (Exception ex)
            {
                //if (conn.State == ConnectionState.Open)
                //    conn.Close();
                throw ex;
            }

        }
    }
}
