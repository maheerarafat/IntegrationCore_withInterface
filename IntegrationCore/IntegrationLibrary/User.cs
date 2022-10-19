using Dapper;
using IntegrationCore.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IntegrationCore.IntegrationLibrary
{
    public class User
    {
        public static object GetUserInfo(SearchObject searchObject)
        {
            object result = null;
            ////bool EnableLoggingInfo = (bool)IntegrationSettings.GetSetting("EnableLoggingInfo");
            try
            {
                var SampleData = Convert.ToBoolean( IntegrationSettings.GetSetting("SampleData"));
                Models.User sampleUser = new Models.User();
                string GUID = Guid.NewGuid().ToString();
                List<string[]> Parameters = new List<string[]>();
                if (searchObject.SearchKey == "0000")
                {
                    
                    sampleUser = new Models.User

                    {
                        FldNickName = "0000",//Emp_no Militery Number
                        FldName = "HR_00000",
                        FldFirstName = "foo",// Emp_name_E full name in one field
                        FldLocalFirstName = "foo",// Emp_name_A full name in one field
                        FldTitle = "ال" + "foo",
                        FldLocalTitle = "ال" + "foo",
                        FldUID = GUID,
                        FldEmail = "foo",
                        FldCellPhoneNumber = "0123123123",
                        HREMPID = "147852",
                        HRDEPTCODE = "1010",
                        HRPOSCODE = "114477",// need to check in service
                        HRPOSNAME = "foo",// need to check in service
                        FldUserCode = "foo",
                        ResponseCode = "200",
                        FldTypeID = "1"
                        
                    };
                }
                else if (searchObject.SearchKey == "0001")
                {
                    sampleUser = new Models.User
                    {
                        FldNickName = "0001",//Emp_no Militery Number
                        FldName = "HR_00001",
                        FldFirstName = "foo2",// Emp_name_E full name in one field
                        FldLocalFirstName = "foo2",// Emp_name_A full name in one field
                        FldTitle = "ال" + "foo2",
                        FldLocalTitle = "ال" + "foo2",
                        FldUID = GUID,
                        FldEmail = "foo2@ss.com",
                        FldCellPhoneNumber = "01141234",
                        HREMPID = "00001",
                        HRDEPTCODE = "1010",
                        HRPOSCODE = "1123",// need to check in service
                        HRPOSNAME = "foo2",// need to check in service
                        FldUserCode = "foo2",
                        ResponseCode = "200",
                        FldTypeID = "2"
                    };
                    
                }
                string[] searchPar = { (searchObject.FieldName == "MNum") ?"MilitaryNumber":"EmiratesID" , searchObject.SearchKey.Trim() };
                string[] searchTypeIDPar = { searchObject.FieldName = "TypeId", searchObject.SearchKey = searchObject.TypeId };
                Parameters.Add(searchPar);
                HREmployeeDetail ed=new HREmployeeDetail() ;
                HREmployeeDetails hREmployeeDetails = new HREmployeeDetails();
                string objStr;
                if (! SampleData)
                {
                    try
                    {
                        hREmployeeDetails = XMLProcessing.Deserializer<HREmployeeDetails>(SoapRequest.CallHRWebService("GetEmployeeDetailsRequest", Parameters: Parameters).ToString());
                        objStr = Newtonsoft.Json.JsonConvert.SerializeObject(hREmployeeDetails);
                    }
                    catch
                    {
                        Models.User user1 = new Models.User()
                        {
                            ResponseCode = "0",
                            LMessage = "لايوجد نتائج",
                            EMessage = "No results"
                        };
                        return user1;
                    }
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "hREmployeeDetails: " + objStr,TrackInterface: Logger.EnableLoggingInfo);
                ed = hREmployeeDetails.HREmployeeDetailsList[0];
                if (ed.EmployeeID == "")
                {
                    Models.User user1 = new Models.User()
                    {
                        ResponseCode = "0",
                        LMessage = "لايوجد نتائج",
                        EMessage = "No results"
                    };
                    return user1;
                }
                }
           
                
                Models.User user = new Models.User
                {
                    FldNickName = ed.MilitaryNumber,//Emp_no Militery Number
                    //FldName =  ed.EmployeeID,
                    FldFirstName = ed.EnglishEmployeeName,// Emp_name_E full name in one field
                    FldLocalFirstName = ed.ArabicEmployeeName,// Emp_name_A full name in one field
                    FldTitle = "ال" + ed.RankName,
                    FldLocalTitle = "ال" + ed.RankName,
                    FldUID = GUID,
                    FldEmail = ed.ExternalEmail,
                    FldCellPhoneNumber = ed.PhonceNumber,
                    HREMPID = ed.EmployeeID,
                    HRDEPTCODE = ed.LastDepartmentID,
                    HRPOSCODE = ed.LastDepartmentID
                            + "_" + ((string.IsNullOrEmpty(ed.OccupationGroupID)) ? 
                              (ed.JobTileGroupID + "_" + ed.JobTitleID + "_T") 
                            : (ed.OccupationGroupID + "_" + ed.OccupationJobID + "_A")),// need to check in service
                    HRPOSNAME = ((string.IsNullOrEmpty(ed.OccupationGroupID)) ? ((ed.JobTileGroupID != "999")?ed.JobTileGroupName:ed.JobTitleName) : (ed.OccupationGroupName)),// need to check in service
                    FldUserCode = (string.IsNullOrEmpty(ed.EmiratesID))?"-":ed.EmiratesID ,
                    FldName  = "hr_" + ed.EmployeeID,
                    ResponseCode = "200",
                    FldTypeID = ed.TypeId

                };
                
                if (SampleData)
                {
                    ed.EmployeeID = sampleUser.HREMPID;
                    user = sampleUser;

                }

             //   Check if user is already in Tarasol
                string DecPass = Encryption.DecryptString(IntegrationSettings.GetSetting("DBPassword").ToString());
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Connect to Database: " + IntegrationSettings.GetSetting("ConnectionString").ToString() /*+ "@#$11"+ DecPass*/, TrackInterface: Logger.EnableLoggingInfo);
                ///OracleConnection conn = new OracleConnection(IntegrationSettings.GetSetting("ConnectionString").ToString() + DecPass);
                using (OracleConnection conn = new OracleConnection(IntegrationSettings.GetSetting("ConnectionString").ToString() + DecPass))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }


                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select u.flduid,u.fldname cnt from INTHRUSERSMAP m inner join tsusers u on u.flduid = m.flduseruid where fldhrempid = " + ed.EmployeeID;
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, cmd.CommandText, TrackInterface: Logger.EnableLoggingInfo);
                    var reader = cmd.ExecuteReader();
                    string UUID = "0";
                    string fldname = "0";
                    while (reader.Read())
                    {
                        UUID = reader.GetValue(0).ToString();
                        fldname = reader.GetValue(1).ToString();
                    }
                    if (UUID == "0")
                    {
                        cmd.CommandText = "delete from INTHRUSERSMAP where fldhrempid = " + ed.EmployeeID + "";
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, cmd.CommandText, TrackInterface: Logger.EnableLoggingInfo);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "insert into INTHRUSERSMAP(flduseruid,fldhrempid) values('" + GUID + "'," + ed.EmployeeID + ")";
                        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, cmd.CommandText, TrackInterface: Logger.EnableLoggingInfo);
                        cmd.ExecuteNonQuery();

                        //Check if Position title already exists and take its code
                        cmd.CommandText = "select FLDCode from TSPOSITIONTITLE where FLDLName = '" + user.HRPOSNAME + "'";
                        reader = cmd.ExecuteReader();
                        string Ecode = "";
                        while (reader.Read())
                        {
                            Ecode = reader.GetValue(0).ToString();
                        }
                        if (Ecode != "")
                        {
                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "Existing position title: " + Ecode, TrackInterface: true);
                            user.HRPOSCODE = user.HRDEPTCODE + "_" + Ecode;
                        }
                        user.FldName = "hr_" + ed.EmployeeID;
                    }
                    else
                    {
                        user.FldUID = UUID;
                        user.FldName = fldname;
                        // return error message to tarasol
                        //user.ResponseCode = "201";
                        //user.LMessage = "المستخدم موجود مسبقاً في تراسل";
                        //user.EMessage = "User already exists in Tarasol";
                    }
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "UUID: " + UUID, TrackInterface: true);

                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "user.FldName " + user.FldName, TrackInterface: true);
                    result = user;
                }

            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }

            return result;
        }

        //public static object UpdateDelegationList()
        //{
        //    object result = null;
        //    try
        //    {
        //        var conn = ConnectionManager.GetConnection();
        //        if (conn.State == ConnectionState.Closed)
        //        {
        //            conn.Open();
        //        }

        //        if (conn.State == ConnectionState.Open)
        //        {
        //            //Get Last Sync Date
        //            IDbCommand cmd = conn.CreateCommand();
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "INTEGRATION.GetLastUpdateDate";

        //            var tableName = cmd.CreateParameter();
        //            tableName.ParameterName = "TableName";
        //            tableName.DbType = DbType.String;
        //            tableName.Direction = ParameterDirection.Input;
        //            tableName.Value = "intuservacation";
        //            cmd.Parameters.Add(tableName);

        //            var returnedValue = cmd.CreateParameter();
        //            returnedValue.ParameterName = "Return_Value";
        //            returnedValue.DbType = DbType.DateTime;
        //            returnedValue.Direction = ParameterDirection.ReturnValue;
        //            cmd.Parameters.Add(returnedValue);
        //            IDataReader dataReader = cmd.ExecuteReader();

        //            //Expose Service
        //            HRVacations hRVacations = XMLProcessing.Deserializer<HRVacations>(SoapRequest.CallHRWebService("GetListOfVacationsRequest"/*,Parameters*/).ToString());
        //            //Divide Result to equal parts
        //            var SplittedLists = XMLProcessing.SplitList(hRVacations.VacationsList, 100);

        //            //Execute Update
        //            IDbCommand UpdateCommand = conn.CreateCommand();
        //            UpdateCommand.CommandType = CommandType.StoredProcedure;
        //            UpdateCommand.CommandText = "INTEGRATION.UpdateDelegationList";
        //            var VacationList = new OracleParameter("XList", OracleDbType.XmlType, 0, "")
        //            {
        //                Direction = ParameterDirection.Input
        //            };
        //            UpdateCommand.Parameters.Add(VacationList);

        //            //Execute each bulk alone
        //            foreach (var item in SplittedLists)
        //            {
        //                HRVacations s = new HRVacations
        //                {
        //                    VacationsList = item
        //                };
        //                string Xml = XMLProcessing.Serialize(s);
        //                VacationList.Value = Xml;
        //                UpdateCommand.ExecuteReader();
        //            }
        //            conn.Close();
        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
        //        throw ex;
        //    }

        //    return result;
        //}

        //public static object UpdateUserInfo()
        //{
        //    object result = null;
        //    try
        //    {
        //        var conn = ConnectionManager.GetConnection();
        //        if (conn.State == ConnectionState.Closed)
        //        {
        //            conn.Open();
        //        }

        //        if (conn.State == ConnectionState.Open)
        //        {
        //            //Get Last Sync Date
        //            IDbCommand cmd = conn.CreateCommand();
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "INTEGRATION.GetLastUpdateDate";

        //            var tableName = cmd.CreateParameter();
        //            tableName.ParameterName = "TableName";
        //            tableName.DbType = DbType.String;
        //            tableName.Direction = ParameterDirection.Input;
        //            tableName.Value = "intemployeesupdates";
        //            cmd.Parameters.Add(tableName);

        //            var returnedValue = cmd.CreateParameter();
        //            returnedValue.ParameterName = "Return_Value";
        //            returnedValue.DbType = DbType.DateTime;
        //            returnedValue.Direction = ParameterDirection.ReturnValue;
        //            cmd.Parameters.Add(returnedValue);
        //            IDataReader dataReader = cmd.ExecuteReader();

        //            //Expose Service
        //            HRUpdatedEmployees hREmployeeDetails = XMLProcessing.Deserializer<HRUpdatedEmployees>(
        //                                                   SoapRequest.CallHRWebService("GetListOfUpdatedEmployeesRequest"/*,Parameters*/).ToString());
        //            //Dealing with user info fields except department and position
        //            //Divide Result to equal parts
        //            var SplittedLists = XMLProcessing.SplitList(hREmployeeDetails.HRUpdatedEmployeessList
        //                .Where(x => x.TransactionType == "U" && x.ColumnName != "LAST_DEP_ID" && x.ColumnName != "JOBTLGRP_ID")
        //                .OrderBy(x => x.TransactionDate)
        //                .ToList(), 100);

        //            //Execute Update
        //            IDbCommand UpdateCommand = conn.CreateCommand();
        //            UpdateCommand.CommandType = CommandType.StoredProcedure;
        //            UpdateCommand.CommandText = "INTEGRATION.UpdateUsers";
        //            var EmployeeList = new OracleParameter("XList", OracleDbType.XmlType, 0, "")
        //            {
        //                Direction = ParameterDirection.Input
        //            };
        //            UpdateCommand.Parameters.Add(EmployeeList);


        //            foreach (var item in SplittedLists)
        //            {
        //                HRUpdatedEmployees s = new HRUpdatedEmployees
        //                {   
        //                    HRUpdatedEmployeessList = item
        //                };
        //                string Xml = XMLProcessing.Serialize(s);
        //                EmployeeList.Value = Xml;
        //                UpdateCommand.ExecuteReader();
        //            }

        //            // Dealing with user department and position changes 
        //            List<HREmployeeTrasInfo> TempUpdateList = new List<HREmployeeTrasInfo>();
        //            foreach (var item in hREmployeeDetails.HRUpdatedEmployeessList
        //                .Where(x => x.TransactionType == "U" && (x.ColumnName == "LAST_DEP_ID" || x.ColumnName == "JOBTLGRP_ID"))
        //                .OrderBy(x => x.TransactionDate)
        //                .ToList())
        //            {
        //                if (TempUpdateList.Find(x => x.HRUpdatedEmployee.EmployeeID == item.EmployeeID) == null)
        //                {
        //                    List<string[]> par = new List<string[]>();
        //                    string[] parstring = { "MilitaryNumber", item.EmployeeID }; // should get military number from hr service
        //                    par.Add(parstring);
        //                    HREmployeeDetail hREmployeeDetail = XMLProcessing.Deserializer<HREmployeeDetail>(SoapRequest.CallHRWebService("GetEmployeeDetails", par).ToString());
        //                    HREmployeeTrasInfo hREmployeeTrasInfo = new HREmployeeTrasInfo()
        //                    {
        //                        HREmployeeDetail = hREmployeeDetail,
        //                        HRUpdatedEmployee = item
        //                    };
        //                    TempUpdateList.Add(hREmployeeTrasInfo);
        //                }
        //            }
        //            conn.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
        //        throw ex;
        //    }

        //    return result;
        //}

        //public static object ChangePassword(UserModel user)
        //{
        //    try
        //    {
        //        AM_EncryptCore.clsEncrypt clsEncrypt = new AM_EncryptCore.clsEncrypt();
        //        string errorMsg = string.Empty;
        //        string encodePassword = clsEncrypt.EncryptString(user.Password, "NVS", AM_EncryptCore.clsEncrypt.OperationKind.oEncrypt, ref errorMsg);
        //        //string query = "update tsusers u set u.fldpassword = '{0}' where lower(u.fldnickname) = lower('{1}')";
        //        SyncHR.UpdatePassword(user.Username,encodePassword);

        //        return IntResultLib.GetResult(1);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile("ChangePassword." + System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "stage Exception  :" + ex.Message);
        //        throw;
        //    }

        //    return IntResultLib.GetResult(1);
        //}

        //public static object GetUserInfo2(SearchObject searchObject)
        //{
        //    object result = null;
        //    try
        //    {
        //        //Dummy Data for test when search key is 0000
        //        //if(searchObject.SearchKey == "0000")
        //        //{
        //        //    Models.User Duser = new Models.User();
        //        //    Duser.FldName = "0000";
        //        //    Duser.FldFirstName = "foo1";
        //        //    Duser.FldLocalFirstName = "تحريبي";
        //        //    Duser.FldTitle = "الموظف";
        //        //    return Duser;
        //        //}
        //        ////////////////End Dumy data/////////////////

        //        //Expose HR service from ESB to get User Data
        //        string GUID = Guid.NewGuid().ToString();
        //        List<string[]> Parameters = new List<string[]>();
        //        string[] searchPar = { (searchObject.FieldName == "MNum") ? "MilitaryNumber" : "EmiratesID", searchObject.SearchKey };
        //        Parameters.Add(searchPar);
        //        //HREmployeeDetails hREmployeeDetails = XMLProcessing.Deserializer<HREmployeeDetails>( SoapRequest.CallHRWebService("GetEmployeeDetailsRequest",Parameters:Parameters).ToString());
        //        //HREmployeeDetail ed = hREmployeeDetails.HREmployeeDetailsList[0];
        //        //Models.User user = new Models.User
        //        //{
        //        //    FldNickName = ed.MilitaryNumber,//Emp_no Militery Number
        //        //    FldName = ed.EmployeeID,
        //        //    FldFirstName = ed.EnglishEmployeeName,// Emp_name_E full name in one field
        //        //    FldLocalFirstName = ed.ArabicEmployeeName,// Emp_name_A full name in one field
        //        //    FldTitle = ed.RankName,
        //        //    FldLocalTitle = ed.RankName,
        //        //    FldUID = GUID,
        //        //    FldEmail = ed.ExternalEmail,
        //        //    FldCellPhoneNumber = ed.PhonceNumber,
        //        //    HREMPID = ed.EmployeeID,
        //        //    HRDEPTCODE = ed.LastDepartmentID,
        //        //    HRPOSCODE = ed.LastDepartmentID
        //        //            + "_" + ((string.IsNullOrEmpty(ed.OccupationGroupID)) ? (ed.JobTileGroupID + "_" + ed.JobTitleID) : (ed.OccupationGroupID + "_" + ed.OccupationJobID)),// need to check in service
        //        //    HRPOSNAME = ((string.IsNullOrEmpty(ed.OccupationGroupID)) ? (ed.JobTileGroupName) : (ed.OccupationGroupName)),// need to check in service
        //        //    ResponseCode = "200"
        //        //};
        //        // Dummy Data
        //        Models.User user = new Models.User
        //        {
        //            FldNickName = "1",
        //            FldName = "2214",
        //            FldFirstName = "Ghaith Abbas",
        //            FldLocalFirstName = "غيث عباس",
        //            FldTitle = "civilian",
        //            FldLocalTitle = "مدني",
        //            FldUID = "72e516c7-c25b-41f7-bad6-0e0c5c4b04ea",
        //            FldEmail = "g@nvs.com",
        //            FldCellPhoneNumber = "025878987",
        //            HREMPID = "2214",
        //            HRDEPTCODE = "860006",
        //            HRPOSCODE = "86006_1_15",
        //            HRPOSNAME = "استشاري تقني",
        //            ResponseCode = "200",
        //            LMessage = "",
        //            EMessage = ""

        //        };
                
                
        //        //
        //        //Check if user is already in Tarasol
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Connect to Database: " + IntegrationSettings.GetSetting("ConnectionString").ToString());
        //        string DecPass = Encryption.DecryptString(IntegrationSettings.GetSetting("DBPassword").ToString());
        //        OracleConnection conn = new OracleConnection(IntegrationSettings.GetSetting("ConnectionString").ToString() + DecPass);
        //        if (conn.State == ConnectionState.Closed)
        //        {
        //            conn.Open();
        //        }
        //        OracleCommand cmd = conn.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = "select count(*) cnt from INTHRUSERSMAP m inner join tsusers u on u.flduid = m.flduseruid where fldhrempid = 2214" ;
        //        var reader = cmd.ExecuteReader();
        //        int count = 0;

        //        while (reader.Read())
        //        {
        //            count = Convert.ToInt32(reader.GetValue(0));
        //        }
        //        if (count == 0)
        //        {
        //            cmd = conn.CreateCommand();
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = "insert into INTHRUSERSMAP(flduseruid,fldhrempid) values('" + GUID + "',2214)";
        //            cmd.ExecuteNonQuery();
        //        }
        //        else
        //        {
        //            // return error message to tarasol
        //            user.ResponseCode = "0";
        //            user.LMessage = "المستخدم موجود مسبقاً في تراسل";
        //            user.EMessage = "User already exists in Tarasol";
        //        }
        //        result = user;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
        //        throw ex;
        //    }

        //    return result;
        //}
    }
}
