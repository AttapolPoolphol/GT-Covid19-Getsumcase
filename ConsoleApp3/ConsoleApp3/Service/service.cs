using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using RestSharp;
using Newtonsoft.Json;
using System.Threading;
using System.Data;
using System.Configuration;
using GT.Core.Database;

namespace ConsoleApp3.Service
{
    public class service
    {
        public List<SumCaseProperties> data;
        public SqlCommand sqlCommand;
        
        SqlServerDatabase sqlServerDatabase = new SqlServerDatabase();



        //public SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DB"]);
        


        public void ConnectDB() 
        {
             SqlConnection con = sqlServerDatabase.GetSqlConnection(ConfigurationSettings.AppSettings["db_server"].ToString()
                                                                        , ConfigurationSettings.AppSettings["db_databaseName"].ToString()
                                                                        , ConfigurationSettings.AppSettings["db_user"].ToString()
                                                                        , ConfigurationSettings.AppSettings["db_password"].ToString());
            if (con.State == System.Data.ConnectionState.Open)
            {

            }
        }

        public List<SumCaseProperties> CallAPI()
        {
            try
            {
                var client = new RestClient(ConfigurationSettings.AppSettings["API"]);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                string json = response.Content;
                Console.WriteLine("Process...");
                List<SumCaseProperties> data = JsonConvert.DeserializeObject<List<SumCaseProperties>>(json);
                this.data = data;
                return this.data;
            }
            catch
            {
                Log(2);
                Thread.Sleep(int.Parse(ConfigurationSettings.AppSettings["Wait"]));
                CallAPI();
                return this.data;

            }
        }

        public void Update_Province()
        {          
            if (Check() == false)
            {
                SqlConnection con = sqlServerDatabase.GetSqlConnection(ConfigurationSettings.AppSettings["db_server"].ToString()
                                                                       , ConfigurationSettings.AppSettings["db_databaseName"].ToString()
                                                                       , ConfigurationSettings.AppSettings["db_user"].ToString()
                                                                       , ConfigurationSettings.AppSettings["db_password"].ToString());
                con.Open();
                foreach (var i in this.data)
                {

                    string query_update = @"UPDATE [dbo].[Province] Set [sumCases] = @sumCount " +
                        //"from [dbo].[Province] p INNER JOIN [dbo].[adminTH] a on p.Province = a.PROV_NAMT" +
                        "where [province] = @province";


                    //string query_insert = "INSERT INTO [dbo].[Province]([Province],[ProvinceEn],[sumCases]) " +
                    //    "VALUES(@province,@provinceEn,@sumCount)";


                    SqlCommand sqlCommand = new SqlCommand(query_update, con);

                    sqlCommand.Parameters.AddWithValue("@province", i.province);
                    sqlCommand.Parameters.AddWithValue("@sumCount", i.total_case);
                    //sqlCommand.Parameters.AddWithValue("@provinceEn", data.Province[i].ProvinceEn);



                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();


                }
                Console.WriteLine("Update Success");
                con.Close();
            }
            else
            {
                Console.WriteLine("latest update");
                Environment.Exit(0);
            }

        }

        public bool Check()
        {
            SqlConnection con = sqlServerDatabase.GetSqlConnection(ConfigurationSettings.AppSettings["db_server"].ToString()
                                                                        , ConfigurationSettings.AppSettings["db_databaseName"].ToString()
                                                                        , ConfigurationSettings.AppSettings["db_user"].ToString()
                                                                        , ConfigurationSettings.AppSettings["db_password"].ToString());
            con.Open();
                bool status = false;

                string query_select = @"SELECT top 1 [updateTime]FROM [dbo].[Log] " +
                    "where updateTime is not null and logStatus = 1 order by updateTime desc";


                SqlCommand sqlCommand = new SqlCommand(query_select, con);

            object UpdateTime = sqlCommand.ExecuteScalar();
            DateTime lastdata = DateTime.Parse(this.data.FirstOrDefault().update_date);

            if (lastdata.Equals(UpdateTime))
                {
                    status = true;
                }
                else
                {

                }

                sqlCommand.ExecuteNonQuery();

                con.Close();

                return status;
            
        }


        public void Log(int status)
        {
            SqlConnection con = sqlServerDatabase.GetSqlConnection(ConfigurationSettings.AppSettings["db_server"].ToString()
                                                                        , ConfigurationSettings.AppSettings["db_databaseName"].ToString()
                                                                        , ConfigurationSettings.AppSettings["db_user"].ToString()
                                                                        , ConfigurationSettings.AppSettings["db_password"].ToString());
            con.Open();
            string query_insert = @"INSERT INTO [dbo].[Log]([logStatus],[timestamp],[updateTime]) " +
                    "VALUES(@status,GETDATE(),NULL)";

            SqlCommand sqlCommand = new SqlCommand(query_insert, con);
          
            try
            {
                    if (status == 1)
                    {
                        query_insert = @"INSERT INTO [dbo].[Log]([logStatus],[timestamp],[updateTime]) " +
                        "VALUES(@status,GETDATE(),@updateTime)";

                        sqlCommand = new SqlCommand(query_insert, con);

                        sqlCommand.Parameters.AddWithValue("@status", status);
                        sqlCommand.Parameters.AddWithValue("@updateTime", this.data.FirstOrDefault().update_date);

                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Dispose();

                    }
                    else
                    {

                        sqlCommand.Parameters.AddWithValue("@status", status);
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Dispose();

                    }
                    con.Close();
                }

            catch
            {
                
            }
           
        }

    }
}
