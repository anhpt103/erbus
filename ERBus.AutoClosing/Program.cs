using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
namespace ERBus.AutoClosing
{
    class Program
    {
        public static int GetPeriod(DateTime day)
        {
            int result = 0;
            string time = day.Date.Day + "-" + day.Date.Month + "-" + day.Date.Year;
            try
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = connection;
                            cmd.InitialLONGFetchSize = 1000;
                            cmd.CommandText = "SELECT KYKETOAN FROM KYKETOAN WHERE TO_DATE(TUNGAY,'DD-MM-YY') = TO_DATE('" + time + "','DD-MM-YY')";
                            cmd.CommandType = CommandType.Text;
                            OracleDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    int.TryParse(dataReader["KYKETOAN"].ToString(), out result);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error to get period;");
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error to get period '" + ex + "';");
            }
            return result;
        }

        public static bool UpdateStatePeriodAndCloseInventory(string preTableName, string nextTableName, string unitCode, int year, int period)
        {
            bool result = false;
            try
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    connection.Open();
                    OracleCommand cmd = new OracleCommand();
                    OracleTransaction transaction;
                    transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    try
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            cmd.Connection = connection;
                            cmd.InitialLONGFetchSize = 1000;
                            cmd.CommandText = string.Format(@"UPDATE KYKETOAN SET TRANGTHAI = 10 WHERE KYKETOAN = {0} AND NAM = {1}", period, DateTime.Now.Year);
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            cmd.InitialLONGFetchSize = 1000;
                            cmd.CommandText = "ERBUS.XUATNHAPTON.XNT_KHOASO";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("P_TABLENAME_KYTRUOC", OracleDbType.Varchar2, 15, preTableName, ParameterDirection.Input);
                            cmd.Parameters.Add("P_TABLENAME", OracleDbType.Varchar2, 15, nextTableName, ParameterDirection.Input);
                            cmd.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2, 10, unitCode, ParameterDirection.Input);
                            cmd.Parameters.Add("P_NAM", OracleDbType.Int32, year, ParameterDirection.Input);
                            cmd.Parameters.Add("P_KY", OracleDbType.Int32, period, ParameterDirection.Input);
                            
                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                            Console.WriteLine(" ==> Khóa sổ xong kỳ : " + period + ";");
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        result = false;
                        Console.WriteLine("Error update period '" + ex.ToString() + "';");
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error update period '" + ex + "';");
            }
            return result;
        }
      
        static void Main(string[] args)
        {
            var today = DateTime.Now.AddDays(+1);
            var yesterday = today.AddDays(-1);
            var kytruoc = GetPeriod(yesterday);
            if (kytruoc == 0)
            {
                Console.WriteLine("Error to get period");
                return;
            }
            else
            {
                Console.WriteLine("Tiến trình khóa sổ khởi chạy");
                Console.WriteLine("------------Khóa sổ kỳ " + kytruoc + "-------------");
                string preTableName = "XNT_" + DateTime.Now.Year + "_KY_" + (kytruoc - 1);
                string nextTableName = "XNT_" + DateTime.Now.Year + "_KY_" + (kytruoc);
                string unitCode = "01";
                int year = DateTime.Now.Year;
                if (!UpdateStatePeriodAndCloseInventory(preTableName, nextTableName, unitCode, year, kytruoc))
                {
                    Console.WriteLine("Cannot update status period: " + kytruoc + ";");
                    Console.WriteLine("Rollback;");
                }
                else
                {
                    Console.WriteLine("Cập nhật trạng thái và duyệt kỳ : " + kytruoc + " thành công;");
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("Bắt đầu khóa kỳ ngày mới: " + (kytruoc + 1) + ";");
                    preTableName = "XNT_" + DateTime.Now.Year + "_KY_" + (kytruoc);
                    nextTableName = "XNT_" + DateTime.Now.Year + "_KY_" + (kytruoc + 1);
                    if (!UpdateStatePeriodAndCloseInventory(preTableName, nextTableName, unitCode, year, (kytruoc + 1)))
                    {
                        Console.WriteLine("Cannot update status period: " + (kytruoc + 1) + "");
                        Console.WriteLine("Rollback");
                    }
                    else
                    {
                        Console.WriteLine("Cập nhật trạng thái và duyệt kỳ : " + (kytruoc + 1) + " thành công;");
                    }
                }
            }
            Console.WriteLine("Enter random key to close moniter");
            Environment.Exit(0);
        }
    }
}
