using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace GT.Core.Database
{
    public class SqlServerDatabase
    {
        public SqlConnection GetSqlConnection(string server, string database, string user, string password)
        {
            string conString = "server = " + server + ";database = " + database + ";User ID = " + user + ";password = " + password + "; Connection Timeout = 0;";
            SqlConnection con = new SqlConnection(conString);
            return con;
        }

        public SqlConnection GetSqlConnection(string server, string database, string user, string password,int timeout)
        {
            string conString = "server = " + server + ";database = " + database + ";User ID = " + user + ";password = " + password + "; Connection Timeout = "+timeout+";";
            SqlConnection con = new SqlConnection(conString);
            return con;
        }

        public DataTable SelectData(SqlConnection sqlCon, string queryComand)
        {
            return SelectData(sqlCon, queryComand, null);
        }

        public DataTable SelectData(SqlConnection sqlCon, string queryComand, SqlParameter[] sqlParameter)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = queryComand;
            sqlCommand.Connection = sqlCon;
            if (sqlParameter != null)
            {
                sqlCommand.Parameters.AddRange(sqlParameter);
            }
            SqlDataAdapter a = new SqlDataAdapter(sqlCommand);
            a.SelectCommand.CommandTimeout = 0;

            DataTable t = new DataTable();
            a.Fill(t);
            a.Dispose();
            return t;
        }

        public void InsertData (SqlConnection sqlCon,string queryComand, SqlParameter[] sqlParameter)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = queryComand;
            sqlCommand.Connection = sqlCon;
            if (sqlParameter != null)
            {
                sqlCommand.Parameters.AddRange(sqlParameter);
            }
            sqlCommand.ExecuteNonQuery();
        }

        public void UpdateData(SqlConnection sqlCon, string queryComand, SqlParameter[] sqlParameter)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = queryComand;
            sqlCommand.Connection = sqlCon;
            if (sqlParameter != null)
            {
                sqlCommand.Parameters.AddRange(sqlParameter);
            }
            sqlCommand.ExecuteNonQuery();
        }

        public DataTable GetDataTableFromStore(SqlConnection sqlCon,string storeName, SqlParameter[] sqlParameter)
        {
            DataTable dt = new DataTable();
            try
            {
                string q2 = storeName;

                SqlCommand cmd = sqlCon.CreateCommand();

                //if (input.timeout > 0)
                //    cmd.CommandTimeout = input.timeout;

                cmd.CommandText = q2;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParameter);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd ;
                da.Fill(dt);

                cmd.Dispose();
                return dt;
            }
            catch(Exception ex)
            {
                return null;
            }

        }

        public DataTable GetDataTableFromStore(SqlConnection sqlCon, string storeName)
        {
            DataTable dt = new DataTable();
            try
            {
                string q2 = storeName;

                SqlCommand cmd = sqlCon.CreateCommand();

                //if (input.timeout > 0)
                //    cmd.CommandTimeout = input.timeout;

                cmd.CommandText = q2;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Dispose();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public DataSet GetDataSetFromStore(SqlConnection sqlCon, string storeName, SqlParameter[] sqlParameter)
        {
            DataSet dt = new DataSet();
            try
            {
                string q2 = storeName;

                SqlCommand cmd = sqlCon.CreateCommand();

                //if (input.timeout > 0)
                //    cmd.CommandTimeout = input.timeout;

                cmd.CommandText = q2;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParameter);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Dispose();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public bool RunStore(SqlConnection sqlCon, string storeName, SqlParameter[] sqlParameter)
        {
            DataTable dt = new DataTable();
            try
            {
                string q2 = storeName;

                SqlCommand cmd = sqlCon.CreateCommand();

                //if (input.timeout > 0)
                //    cmd.CommandTimeout = input.timeout;

                cmd.CommandText = q2;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParameter);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
