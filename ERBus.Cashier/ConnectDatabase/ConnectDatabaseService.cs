using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace ERBus.Cashier.ConnectDatabase
{
    public class ConnectDatabaseService
    {
        public static bool CheckTableExistInDatabase(string tableName)
        {
            bool exists = false;
            SqlConnection connection = new SqlConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ERBusCashier"].ToString();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string SqlExistDbQuery = string.Format(@"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + tableName + "'");
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = connection;
                    sqlCmd.CommandText = SqlExistDbQuery;
                    sqlCmd.CommandType = CommandType.Text;
                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        exists = true;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return exists;
        }
    }
}
