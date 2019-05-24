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
                    Session.Session.SESSION_ONLINE = result;
                }
            }
           catch
            {
                result = false;
                Session.Session.SESSION_ONLINE = result;
            }
            finally
            {
                if (Session.Session.SESSION_ONLINE)  connection.Close();
                if (Session.Session.SESSION_ONLINE)  connection.Dispose();
                Session.Session.SESSION_ONLINE = result;
            }
            return result;
        }
    }
}
