using Dapper;
using IntegrationCore.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;

namespace IntegrationCore.IntegrationLibrary
{
    public class Departments
    {
        //public static object GetDepartments()
        //{
        //    object result = null;
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
        //            var query = "select fldcode as Code,fldlname as LName,fldename as EName from tsdepartment";

        //            //result = SqlMapper.Query(conn, query, param: dyParam, commandType: CommandType.StoredProcedure);
        //            object instance = Activator.CreateInstance(typeof(Models.Department),false);
        //            result = SqlMapper.Query<Models.Target>(conn, query,commandType: CommandType.Text).ToList();
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

        public static object GetDepartmentInfo(SearchObject searchObject)
        {
            object result = null;
            try
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start");
                List<Models.Department> departmentList = new List<Models.Department>();
                if(string.IsNullOrEmpty(searchObject.SearchKey.Trim()))
                {
                    return result;
                }
                Models.Department department = new Models.Department();
                if (searchObject.SearchKey == "0000")
                {
                    department.FLDLNAME = "قسم تجريبي";//dept_name
                    department.FLDENAME = "test department";// Dept_Name_eng
                    department.FLDPARENTID = "1231";// Dept_ID_PAR
                    department.FLDCODE = "0000"; // Dept_id
                    department.FLDACTIVE = "1";                              //status , entry_date
                    department.FLDOLDDEPTID = "150";
                    department.FLDOLDDEPTACTIVE = "1";
                    department.FLDTYPE = 3;
                    departmentList.Add(department);
                    return departmentList;
                }
                else if (searchObject.SearchKey == "0001")
                {
                    department.FLDLNAME = "قسم تجريبي";//dept_name
                    department.FLDENAME = "test department";// Dept_Name_eng
                    department.FLDPARENTID = "1231";// Dept_ID_PAR
                    department.FLDCODE = "0001"; // Dept_id
                    department.FLDACTIVE = "0";                              //status , entry_date
                    department.FLDOLDDEPTID = "150";
                    department.FLDOLDDEPTACTIVE = "1";
                    department.FLDTYPE = 3;
                    departmentList.Add(department);
                    return departmentList;
                }
                else if (searchObject.SearchKey == "0002")
                {
                    department.FLDLNAME = "قسم تجريبي";//dept_name
                    department.FLDENAME = "test department";// Dept_Name_eng
                    department.FLDPARENTID = "1231";// Dept_ID_PAR
                    department.FLDCODE = "0002"; // Dept_id
                    department.FLDACTIVE = "1";                              //status , entry_date
                    department.FLDOLDDEPTID = "150";
                    department.FLDOLDDEPTACTIVE = "0";
                    department.FLDTYPE = 3;
                    departmentList.Add(department);
                    return departmentList;
                }
                else if (searchObject.SearchKey == "0003")
                {
                    department.FLDLNAME = "قسم تجريبي";//dept_name
                    department.FLDENAME = "";// Dept_Name_eng
                    department.FLDPARENTID = "1231";// Dept_ID_PAR
                    department.FLDCODE = "0003"; // Dept_id
                    department.FLDACTIVE = "1";                              //status , entry_date
                    department.FLDOLDDEPTID = "150";
                    department.FLDOLDDEPTACTIVE = "0";
                    department.FLDTYPE = 3;
                    departmentList.Add(department);
                    return departmentList;
                }
                //Expose HR service from ESB to get Department Data
                if (searchObject.FieldName.ToLower() == "fldcode") 
                { 
                    searchObject.FieldName = "DepartmentID"; 
                }
                List<string[]> Parameters = new List<string[]>();
                if(searchObject.FieldName == "DepartmentID" || searchObject.FieldName == "OldDepartmentID" || searchObject.FieldName == "ParentID")
                {
                    string[] DepartmentID = { searchObject.FieldName, searchObject.SearchKey.Trim() };
                    Parameters.Add(DepartmentID);
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start request");
                    
                    string res = SoapRequest.CallHRWebService("GetListOfAllDepartmentsRequest", Parameters: Parameters).ToString();
                    
                    HRDepartments hRDepartments = XMLProcessing.Deserializer<HRDepartments>(res);
                    Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "end request");
                    Department dept = new Department();
                    foreach (var hRDepartment in hRDepartments.HRDepartmentsList)
                    {
                        string OldDeptStatus = "N";
                        if (!string.IsNullOrEmpty(hRDepartment.OldDepartmentID))
                        {
                            Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "get status for old dept :" + hRDepartment.OldDepartmentID);
                            Parameters = new List<string[]>();
                            string[] OldDepartmentID = { searchObject.FieldName, hRDepartment.OldDepartmentID };
                            Parameters.Add(OldDepartmentID);
                            try
                            {
                                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Start request -old");
                                res = SoapRequest.CallHRWebService("GetListOfAllDepartmentsRequest", Parameters: Parameters).ToString();
                                //xml = new XmlDocument();
                                //xml.LoadXml(res);
                                //res = xml.GetElementsByTagName("soap-env:Body")[0].InnerXml.ToString().Replace(@"xmlns=""http://www.moi.gov.ae/2018/MoiIntegration/HRMurasalatBSSchema""", "");
                                HRDepartments OldhRDepartments = XMLProcessing.Deserializer<HRDepartments>(res);
                                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "end request -old");
                                if (OldhRDepartments.HRDepartmentsList.Count != 0)
                                {
                                    OldDeptStatus = OldhRDepartments.HRDepartmentsList.First().DepartmentStatus;
                                }

                                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Info, "Old sts:" + OldDeptStatus);
                            }
                            catch (Exception)
                            {

                                OldDeptStatus = "0";
                            }

                        }
                        dept = new Department
                        {
                            FLDCODE = hRDepartment.DepartmentID,
                            FLDLNAME = hRDepartment.DepartmentArabicName,
                            FLDENAME = /*(string.IsNullOrEmpty(hRDepartment.DepartmentEnglishName)) ? "-" :*/ hRDepartment.DepartmentEnglishName,
                            FLDPARENTID = hRDepartment.DepartmentParentID,
                            FLDACTIVE = (hRDepartment.DepartmentStatus == "A") ? "1" : "0",
                            FLDOLDDEPTID = hRDepartment.OldDepartmentID,
                            FLDOLDDEPTACTIVE = (OldDeptStatus == "A") ? "1" : "0"
                        };

                        switch (hRDepartment.DepartmentClassCode)
                        {
                            case "6": case "5": case "99": dept.FLDTYPE = 4; break;
                            case "4": case "9": case "8": case "14": dept.FLDTYPE = 3; break;
                            case "2": case "3": dept.FLDTYPE = 2; break;
                            default: dept.FLDTYPE = 1; break;
                        };
                        departmentList.Add(dept);
                    }

                    //Rank , Nationality Emp_status
                    result = departmentList;
                }
                
            }
            catch (Exception ex)
            {
                Logger.AddToLogFile(System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.Enum_LogType._Error, "error occured in  : " + ex.Message);
                throw ex;
            }

            return result;
        }


        //public static object UpdateDepartments()
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

        //            var ServiceName = cmd.CreateParameter();
        //            ServiceName.ParameterName = "ServiceName";
        //            ServiceName.DbType = DbType.String;
        //            ServiceName.Direction = ParameterDirection.Input;
        //            ServiceName.Value = "departments";
        //            cmd.Parameters.Add(ServiceName);

        //            var returnedValue = cmd.CreateParameter();
        //            returnedValue.ParameterName = "Return_Value";
        //            returnedValue.DbType = DbType.DateTime;
        //            returnedValue.Direction = ParameterDirection.ReturnValue;
        //            cmd.Parameters.Add(returnedValue);
        //            IDataReader dataReader = cmd.ExecuteReader();

        //            //Expose Service
        //            List<string[]> Parameters = new List<string[]>();
        //            string[] datestring = { "last_date_update", DateTime.Now.ToString() };
        //            Parameters.Add(datestring);
        //            HRUpdatedDepartments hRUpdatedDepartments = XMLProcessing.Deserializer<HRUpdatedDepartments>
        //                (SoapRequest.CallHRWebService("GetListOfUpdatedDepartmentsRequest", Parameters).ToString());
        //            //Divide Result to equal parts
        //            //Take update transactions
        //            //Oreder list by transaction date
        //            var SplittedLists = XMLProcessing.SplitList(hRUpdatedDepartments.HRUpdatedDepartmentsList
        //                .Where(x=>x.TransactionType == "U")
        //                .OrderBy(x => x.TransactionDate)
        //                .ToList(), 100);

        //            //Execute Update
        //            IDbCommand UpdateCommand = conn.CreateCommand();
        //            UpdateCommand.CommandType = CommandType.StoredProcedure;
        //            UpdateCommand.CommandText = "INTEGRATION.UpdateDepartments";
        //            var UpdatedDepartmentsList = new OracleParameter("XList", OracleDbType.XmlType, 0, "")
        //            {
        //                Direction = ParameterDirection.Input
        //            };
        //            UpdateCommand.Parameters.Add(UpdatedDepartmentsList);

                    
        //            UpdateCommand.Parameters.Add(ServiceName);
        //            foreach (var item in SplittedLists)
        //            {
        //                HRUpdatedDepartments s = new HRUpdatedDepartments
        //                {
        //                    HRUpdatedDepartmentsList = item
        //                };
        //                string Xml = XMLProcessing.Serialize(s);
        //                UpdatedDepartmentsList.Value = Xml;
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
    }
}
