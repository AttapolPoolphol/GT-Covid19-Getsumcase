using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ConsoleApp3.Service;
using System.Threading;
using System.Configuration;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            service SV = new service();

            try
            {
                SV.ConnectDB();
                SV.CallAPI();
                SV.Update_Province();
                SV.Log(1);
            }
            catch(Exception ex) 
            {
                Thread.Sleep(int.Parse(ConfigurationSettings.AppSettings["Wait"]));
            }
        }
    }
}
