using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.IntegrationLibrary
{
    public class ConnectionManager
    {
        public static IDbConnection GetConnection()
        {
            string DbType = IntegrationSettings.GetSetting("DBType").ToString();
            string DBPassword =Encryption.DecryptString(IntegrationSettings.GetSetting("DBPassword").ToString()) ;
            
            string connectionString = IntegrationSettings.GetSetting("ConnectionString").ToString() + DBPassword;
            if(DbType == "1")
            {
                OracleConnection conn = new OracleConnection(connectionString);
                return conn;
            }
            else
            {
                SqlConnection conn = new SqlConnection(connectionString);
                return conn;
            }
            
            
        }
    
    }
}
