using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;
namespace ERBus.Cashier.Common
{
    public class Config
    {
        public static bool CheckConnectToServer()
        {
            bool result = false;
            OracleConnection connection = new OracleConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ERBusConnection"].ToString();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return result;
        }
    }
}
