using IntegrationCore.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace IntegrationCore.IntegrationLibrary
{
    public class User
    {
        public static object GetUserInfo(SearchObject searchObject)
        {
            object result = null;
            try
            {
                //Dummy Data for test when search key is 0000
                if (searchObject.SearchKey == "0000")
                {
                    Models.User Duser = new Models.User();
                    Duser.FldName = "0000";
                    Duser.FldFirstName = "foo1";
                    Duser.FldLocalFirstName = "تحريبي";
                    Duser.FldTitle = "الموظف";
                    return Duser;
                }
                ////////////////End Dumy data/////////////////

                //Expose HR service from ESB to get User Data
                List<string[]> Parameters = new List<string[]>();
                string[] MilitaryNumber = { "MilitaryNumber", searchObject.SearchKey };
                Parameters.Add(MilitaryNumber);
                HREmployeeDetails hREmployeeDetails = XMLProcessing.Deserializer<HREmployeeDetails>(SoapRequest.CallHRWebService("GetEmployeeDetailsRequest", Parameters: Parameters).ToString());
                HREmployeeDetail hREmployeeDetail = hREmployeeDetails.HREmployeeDetailsList[0];
                Models.User user = new Models.User
                {
                    FldName = hREmployeeDetail.MilitaryNumber,//Emp_no Militery Number
                    FldFirstName = hREmployeeDetail.EnglishEmployeeName,// Emp_name_E full name in one field
                    FldLocalFirstName = hREmployeeDetail.ArabicEmployeeName,// Emp_name_A full name in one field
                    FldTitle = hREmployeeDetail.RankName, // JOBTLGP_NAME
                    FldLocalTitle = hREmployeeDetail.RankName// JOBTLGP_NAME
                };
                //Nationality, Emp_status
                result = user;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public static object UpdateDelegationList()
        {
            object result = null;
            try
            {
                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();
                    if (conn.State == ConnectionState.Open)
                    {
                        //Get Last Sync Date
                        IDbCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "INTEGRATION.GetLastUpdateDate";

                        var tableName = cmd.CreateParameter();
                        tableName.ParameterName = "TableName";
                        tableName.DbType = DbType.String;
                        tableName.Direction = ParameterDirection.Input;
                        tableName.Value = "intuservacation";
                        cmd.Parameters.Add(tableName);

                        var returnedValue = cmd.CreateParameter();
                        returnedValue.ParameterName = "Return_Value";
                        returnedValue.DbType = DbType.DateTime;
                        returnedValue.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnedValue);
                        IDataReader dataReader = cmd.ExecuteReader();

                        //Expose Service
                        HRVacations hRVacations = XMLProcessing.Deserializer<HRVacations>(SoapRequest.CallHRWebService("GetListOfVacationsRequest"/*,Parameters*/).ToString());
                        //Divide Result to equal parts
                        var SplittedLists = XMLProcessing.SplitList(hRVacations.VacationsList, 100);

                        //Execute Update
                        IDbCommand UpdateCommand = conn.CreateCommand();
                        UpdateCommand.CommandType = CommandType.StoredProcedure;
                        UpdateCommand.CommandText = "INTEGRATION.UpdateDelegationList";
                        var VacationList = new OracleParameter("XList", OracleDbType.XmlType, 0, "")
                        {
                            Direction = ParameterDirection.Input
                        };
                        UpdateCommand.Parameters.Add(VacationList);

                        //Execute each bulk alone
                        foreach (var item in SplittedLists)
                        {
                            HRVacations s = new HRVacations
                            {
                                VacationsList = item
                            };
                            string Xml = XMLProcessing.Serialize(s);
                            VacationList.Value = Xml;
                            UpdateCommand.ExecuteReader();
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

        public static object UpdateUserInfo()
        {
            object result = null;
            try
            {
                using (var cnm = new ConnectionManager())
                {
                    var conn = cnm.GetConnection();
                    if (conn.State == ConnectionState.Open)
                    {
                        //Get Last Sync Date
                        IDbCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "INTEGRATION.GetLastUpdateDate";

                        var tableName = cmd.CreateParameter();
                        tableName.ParameterName = "TableName";
                        tableName.DbType = DbType.String;
                        tableName.Direction = ParameterDirection.Input;
                        tableName.Value = "intemployeesupdates";
                        cmd.Parameters.Add(tableName);

                        var returnedValue = cmd.CreateParameter();
                        returnedValue.ParameterName = "Return_Value";
                        returnedValue.DbType = DbType.DateTime;
                        returnedValue.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnedValue);
                        IDataReader dataReader = cmd.ExecuteReader();

                        //Expose Service
                        HRUpdatedEmployees hREmployeeDetails = XMLProcessing.Deserializer<HRUpdatedEmployees>(SoapRequest.CallHRWebService("GetListOfUpdatedEmployeesRequest"/*,Parameters*/).ToString());
                        //Divide Result to equal parts
                        var SplittedLists = XMLProcessing.SplitList(hREmployeeDetails.HRUpdatedEmployeessList, 100);

                        //Execute Update
                        IDbCommand UpdateCommand = conn.CreateCommand();
                        UpdateCommand.CommandType = CommandType.StoredProcedure;
                        UpdateCommand.CommandText = "INTEGRATION.UpdateEmployeeInfo";
                        var EmployeeList = new OracleParameter("XList", OracleDbType.XmlType, 0, "")
                        {
                            Direction = ParameterDirection.Input
                        };
                        UpdateCommand.Parameters.Add(EmployeeList);


                        foreach (var item in SplittedLists)
                        {
                            HRUpdatedEmployees s = new HRUpdatedEmployees
                            {
                                HRUpdatedEmployeessList = item
                            };
                            string Xml = XMLProcessing.Serialize(s);
                            EmployeeList.Value = Xml;
                            UpdateCommand.ExecuteReader();
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


        /// <summary>
        /// Changer user password in table Tarasol using AM_EncryptCore
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static object ChangePassword(UserModel user)
        {
            try
            {
                AM_EncryptCore.clsEncrypt clsEncrypt = new AM_EncryptCore.clsEncrypt();
                string errorMsg = string.Empty;
                string encodePassword = clsEncrypt.EncryptString(user.Password, "NVS", AM_EncryptCore.clsEncrypt.OperationKind.oEncrypt, ref errorMsg);
                //string query = "update tsusers u set u.fldpassword = '{0}' where lower(u.fldnickname) = lower('{1}')";
                SyncHR.UpdatePassword(user.Username, encodePassword);

                return IntResultLib.GetResult(1);
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile("ChangePassword." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
                throw;
            }
        }

    }
}
