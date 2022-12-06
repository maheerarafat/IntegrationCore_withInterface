using Dapper;
using IntegrationCore.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace IntegrationCore.IntegrationLibrary
{
    public class Departments
    {
        public static object GetDepartments()
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
                        var query = "select fldcode as Code,fldlname as LName,fldename as EName from tsdepartment";

                        //result = SqlMapper.Query(conn, query, param: dyParam, commandType: CommandType.StoredProcedure);
                        object instance = Activator.CreateInstance(typeof(Models.Department), false);
                        result = SqlMapper.Query<Models.Target>(conn, query, commandType: CommandType.Text).ToList();
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public static object GetDepartmentInfo(SearchObject searchObject)
        {
            object result = null;
            try
            {
                Models.Department department = new Models.Department();
                if (searchObject.SearchKey == "0000")
                {
                    department.FLDLNAME = "قسم تجريبي";//dept_name
                    department.FLDENAME = "test department";// Dept_Name_eng
                    department.FLDPARENTID = "1231";// Dept_ID_PAR
                    department.FLDCODE = "tDp223"; // Dept_id
                                                   //status , entry_date
                    return department;
                }
                //Expose HR service from ESB to get Department Data

                List<string[]> Parameters = new List<string[]>();
                string[] DepartmentID = { searchObject.FieldName, searchObject.SearchKey };
                Parameters.Add(DepartmentID);
                HRDepartments hRDepartments = XMLProcessing.Deserializer<HRDepartments>(SoapRequest.CallHRWebService("GetListOfAllDepartmentsRequest", Parameters: Parameters).ToString());
                List<Models.Department> departmentList = new List<Models.Department>();
                foreach (var hRDepartment in hRDepartments.HRDepartmentsList)
                {
                    Department dept = new Department
                    {
                        FLDCODE = hRDepartment.DepartmentID,
                        FLDLNAME = hRDepartment.DepartmentArabicName,
                        FLDENAME = hRDepartment.DepartmentEnglishName,
                        FLDPARENTID = hRDepartment.DepartmentParentID
                    };
                    departmentList.Add(dept);
                }

                //Rank , Nationality Emp_status
                result = departmentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        public static object UpdateDepartments()
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
                        tableName.Value = "intdepartmentsupdates";
                        cmd.Parameters.Add(tableName);

                        var returnedValue = cmd.CreateParameter();
                        returnedValue.ParameterName = "Return_Value";
                        returnedValue.DbType = DbType.DateTime;
                        returnedValue.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnedValue);
                        IDataReader dataReader = cmd.ExecuteReader();

                        //Expose Service
                        HRUpdatedDepartments hRUpdatedDepartments = XMLProcessing.Deserializer<HRUpdatedDepartments>(SoapRequest.CallHRWebService("GetListOfUpdatedDepartmentsRequest"/*,Parameters*/).ToString());
                        //Divide Result to equal parts
                        var SplittedLists = XMLProcessing.SplitList(hRUpdatedDepartments.HRUpdatedDepartmentsList, 100);

                        //Execute Update
                        IDbCommand UpdateCommand = conn.CreateCommand();
                        UpdateCommand.CommandType = CommandType.StoredProcedure;
                        UpdateCommand.CommandText = "INTEGRATION.UpdateDepartments";
                        var UpdatedDepartmentsList = new OracleParameter("XList", OracleDbType.XmlType, 0, "")
                        {
                            Direction = ParameterDirection.Input
                        };
                        UpdateCommand.Parameters.Add(UpdatedDepartmentsList);


                        foreach (var item in SplittedLists)
                        {
                            HRUpdatedDepartments s = new HRUpdatedDepartments
                            {
                                HRUpdatedDepartmentsList = item
                            };
                            string Xml = XMLProcessing.Serialize(s);
                            UpdatedDepartmentsList.Value = Xml;
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
    }
}
