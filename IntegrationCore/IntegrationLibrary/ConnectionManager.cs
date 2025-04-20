using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
//using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Threading.Tasks;

namespace IntegrationCore.IntegrationLibrary
{
    public class ConnectionManager: IDisposable
    {
        private string _DbType { get; set; }
        private string _DBPassword { get; set; }

        private string _connectionString { get; set; }

        public OracleConnection connOra { get; set; }
        public SqlConnection connSql { get; set; }

        public ConnectionManager()
        {
            _DbType = IntegrationSettings.GetSetting("DBType").ToString();
            _DBPassword = Encryption.DecryptString(IntegrationSettings.GetSetting("DBPassword").ToString());
            _connectionString = IntegrationSettings.GetSetting("ConnectionString").ToString() + _DBPassword;

            if (_DbType == "1")
            {
                connOra = new OracleConnection(_connectionString);
                connOra.Open();
            }
            else
            {
                connSql = new SqlConnection(_connectionString);
                connSql.Open();
            }
        }

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

        public IDbConnection GetConnectionDisposal()
        {

            if (_DbType == "1")
            {
                return connOra;
            }
            else
            {
                return connSql;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                if (_DbType == "1")
                {
                    connOra.Close();
                }
                else
                {
                    connSql.Close();
                }
            }
        }

    }
}
