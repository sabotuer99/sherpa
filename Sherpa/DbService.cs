using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace Sherpa
{
    class DbService
    {
        private string sqlTable;
        private string excelDataQuery;
        private string sqlConnectionString;

        public DbService()
        {
            init(); 
        }

        public bool appendExcelToDatabase(string excelfilepath)
        {
            try
            {
                string excelConnectionString = getOleDbConnectionString(excelfilepath);

                //series of commands to bulk copy data from the excel file into our sql table
                OleDbConnection oledbconn = new OleDbConnection(excelConnectionString);
                OleDbCommand oledbcmd = new OleDbCommand(excelDataQuery, oledbconn);
                oledbconn.Open();
                OleDbDataReader dr = oledbcmd.ExecuteReader();

                //use bulk copy to write the data to the database
                SqlBulkCopy bulkcopy = new SqlBulkCopy(sqlConnectionString);
                bulkcopy.DestinationTableName = sqlTable;
                bulkcopy.WriteToServer(dr);

                oledbconn.Close();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        protected string getOleDbConnectionString(string filepath)
        {
            
            string fileExtension = Path.GetExtension(filepath).ToUpper();
            string connectionString = "";

            try
            {
                connectionString = ConfigurationManager.ConnectionStrings[fileExtension]
                    .ConnectionString
                    .Replace("[[FILEPATH]]",filepath);              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            return connectionString;        
        }

        protected void init()
        {
            excelDataQuery = ConfigurationManager.AppSettings["excelDataQuery"];
            sqlTable = ConfigurationManager.AppSettings["sqlTable"];
            sqlConnectionString = ConfigurationManager.ConnectionStrings["SqlDatabase"].ConnectionString;
        }
    }
}

//private void clearSqlTable(string sqlTable, string sqlConnectionString)
//{
//    string clearSql = "delete from " + sqlTable;
//    SqlConnection sqlConn = new SqlConnection(sqlConnectionString);
//    SqlCommand sqlCmd = new SqlCommand(clearSql, sqlConn);
//    sqlConn.Open();
//    sqlCmd.ExecuteNonQuery();
//    sqlConn.Close();
//}
