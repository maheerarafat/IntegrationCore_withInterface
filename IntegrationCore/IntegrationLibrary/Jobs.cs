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
        public static object GetLogJobESBSuccuss(int pageNumber, int pageSize, ref int recordsTotal)
        {
            List<IntJobEsb> result = null;
            List<CountTbl> count_result = null;

            try
            {

                using (var conn = ConnectionManager.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    if (conn.State == ConnectionState.Open)
                    {

                        var query = String.Format("SELECT flduid, fldname, flddescription, flddateStart, flddateEnd, fldduration " +
                            " FROM (SELECT a.*, rownum r__ FROM (SELECT flduid, FLDname, Flddescription, Flddatestart, Flddateend, Fldduration " +
                            $" FROM INTJobEsb WHERE Fldtype = {(int)Helper.Enum.responseCode.Success} ORDER BY Flddatestart DESC ) a " +
                            " WHERE rownum < (({0} * {1}) + 1 ) ) WHERE r__ >= ((({0}-1) * {1}) + 1)",
                            pageNumber + 1, pageSize);

                        object instance = Activator.CreateInstance(typeof(IntJobEsb), false);
                        result = SqlMapper.Query<IntJobEsb>(conn, query, commandType: CommandType.Text).ToList();



                        query = $"select count(*) as FLDCOUNT from INTJobEsb WHERE Fldtype = {(int)Helper.Enum.responseCode.Success}";
                        count_result = SqlMapper.Query<CountTbl>(conn, query, commandType: CommandType.Text).ToList();

                        if (count_result.Count > 0)
                        {
                            recordsTotal = count_result.ElementAt(0).FLDCOUNT;
                        }
                        conn.Close();

                    }

                }



            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static object GetLogJobESBFaild(int pageNumber, int pageSize, ref int recordsTotal)
        {
            List<IntJobEsb> result = null;
            List<CountTbl> count_result = null;

            try
            {

                using (var conn = ConnectionManager.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    if (conn.State == ConnectionState.Open)
                    {

                        var query = String.Format("SELECT flduid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest" +
                             " FROM (SELECT a.*, rownum r__ FROM (SELECT flduid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest " +
                             $" FROM INTJobEsb WHERE Fldtype = {(int)Helper.Enum.responseCode.Failed} ORDER BY Flddatestart DESC ) a" +
                             " WHERE rownum < (({0} * {1}) + 1 ) ) WHERE r__ >= ((({0}-1) * {1}) + 1)",
                             pageNumber + 1, pageSize);

                        object instance = Activator.CreateInstance(typeof(IntJobEsb), false);
                        result = SqlMapper.Query<IntJobEsb>(conn, query, commandType: CommandType.Text).ToList();



                        query = $"select count(*) as FLDCOUNT from INTJobEsb WHERE Fldtype = {(int)Helper.Enum.responseCode.Failed}";
                        count_result = SqlMapper.Query<CountTbl>(conn, query, commandType: CommandType.Text).ToList();

                        if (count_result.Count > 0)
                        {
                            recordsTotal = count_result.ElementAt(0).FLDCOUNT;
                        }
                        conn.Close();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static object GetLogJobESBWait(int pageNumber, int pageSize, ref int recordsTotal)
        {
            List<IntJobEsb> result = null;
            List<CountTbl> count_result = null;

            try
            {

                using (var conn = ConnectionManager.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    if (conn.State == ConnectionState.Open)
                    {

                        var query = String.Format("SELECT flduid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest" +
                            " FROM (SELECT a.*, rownum r__ FROM (SELECT flduid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest " +
                            $" FROM INTJobEsb WHERE Fldtype = {(int)Helper.Enum.responseCode.Failed} " +
                            " and fldnumbertimesendschudle >= {2} and fldsendschudle = {3} ORDER BY Flddatestart DESC ) a WHERE rownum < (({0} * {1}) + 1 ) ) WHERE r__ >= ((({0}-1) * {1}) + 1)",
                            pageNumber + 1, pageSize, Helper.Constant.MaxNumberSchedule, (int)Helper.Enum.scheduleType.NotStarted);

                        object instance = Activator.CreateInstance(typeof(IntJobEsb), false);
                        result = SqlMapper.Query<IntJobEsb>(conn, query, commandType: CommandType.Text).ToList();

                        foreach (var item in result)
                        {
                            if (!String.IsNullOrWhiteSpace(item?.fldbodyrequest))
                            {
                                item.fldbodyrequest = System.Net.WebUtility.HtmlEncode(item?.fldbodyrequest);  //System.Web.HttpUtility.HtmlEncode(item.fldbodyrequest);
                                item.fldbodyrequest = item?.fldbodyrequest.Replace(" ", "%20");
                            }
                        }

                        query = String.Format($"select count(*) as FLDCOUNT from INTJobEsb WHERE Fldtype = {(int)Helper.Enum.responseCode.Failed} " +
                            " and fldnumbertimesendschudle >= {0} and fldsendschudle = {1}",
                            Helper.Constant.MaxNumberSchedule, (int)Helper.Enum.scheduleType.NotStarted);
                        count_result = SqlMapper.Query<CountTbl>(conn, query, commandType: CommandType.Text).ToList();

                        if (count_result.Count > 0)
                        {
                            recordsTotal = count_result.ElementAt(0).FLDCOUNT;
                        }
                        conn.Close();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static object GetLogJobESBInProgress(int pageNumber, int pageSize, ref int recordsTotal)
        {
            List<IntJobEsb> result = null;
            List<CountTbl> count_result = null;

            try
            {

                using (var conn = ConnectionManager.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    if (conn.State == ConnectionState.Open)
                    {

                        var query = String.Format("SELECT flduid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest, fldnumbertimesendschudle" +
                            " FROM (SELECT a.*, rownum r__ FROM (SELECT flduid, FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, fldbodyrequest, fldnumbertimesendschudle " +
                            $" FROM INTJobEsb WHERE Fldtype = {(int)Helper.Enum.responseCode.Failed} " +
                            " and fldnumbertimesendschudle < {2} and fldsendschudle = {3} ORDER BY Flddatestart DESC ) a WHERE rownum < (({0} * {1}) + 1 ) ) WHERE r__ >= ((({0}-1) * {1}) + 1)",
                            pageNumber + 1, pageSize, Helper.Constant.MaxNumberSchedule, (int)Helper.Enum.scheduleType.NotStarted);

                        object instance = Activator.CreateInstance(typeof(IntJobEsb), false);
                        result = SqlMapper.Query<IntJobEsb>(conn, query, commandType: CommandType.Text).ToList();



                        query = String.Format($"select count(*) as FLDCOUNT from INTJobEsb WHERE Fldtype = {(int)Helper.Enum.responseCode.Failed} " +
                            " and fldnumbertimesendschudle < {0} and fldsendschudle = {1}",
                            Helper.Constant.MaxNumberSchedule, (int)Helper.Enum.scheduleType.NotStarted);
                        count_result = SqlMapper.Query<CountTbl>(conn, query, commandType: CommandType.Text).ToList();

                        if (count_result.Count > 0)
                        {
                            recordsTotal = count_result.ElementAt(0).FLDCOUNT;
                        }
                        conn.Close();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static object AddLogJobESB(string name, string description, int type, DateTime startDate, DateTime endDate, Double duration, String bodyRequest, int statusSchedule)
        {
            object result = null;
            try
            {

                using (var conn = ConnectionManager.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    if (conn.State == ConnectionState.Open)
                    {

                        string startDateStr = startDate.ToString("dd/MM/yyyy HH:mm:ss");
                        string endDateStr = endDate.ToString("dd/MM/yyyy HH:mm:ss");

                        var query = String.Format("INSERT INTO INTJobEsb (FLDname, Flddescription, Fldtype, Flddatestart, Flddateend, Fldduration, FLDBODYREQUEST, FLDSENDSCHUDLE, FLDNUMBERTIMESENDSCHUDLE) " +
                            "VALUES ('{0}', '{1}',{2}, TO_DATE('{3}', 'DD/MM/YYYY HH24:MI:SS') , TO_DATE('{4}', 'DD/MM/YYYY HH24:MI:SS'), {5}, '{6}', {7}, {8})",
                            name, description.Replace("'", "''"), type, startDateStr, endDateStr, duration, bodyRequest.Replace("'", "''"), statusSchedule, 1);
                        //Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Job query : " + query, TrackInterface: Logger.EnableLoggingInfo, PrintInterface: Logger.EnableLoggingInfo);
                        int res = conn.Execute(query);
                        conn.Close();
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Failed : " + ex.Message);
            }

            return result;
        }


        public static void UpdateProccessScheduleStatus(string fldid, int status)
        {


            try
            {

                using (var conn = ConnectionManager.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    if (conn.State == ConnectionState.Open)
                    {

                        var query = String.Format("update INTJobEsb set fldsendschudle = {0} where flduid = {1} ", status, fldid);
                        // 
                        int res = conn.Execute(query);
                        conn.Close();
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
