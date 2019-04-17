using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ERBus.Cashier.Common;
using Oracle.ManagedDataAccess.Client;
namespace ERBus.Cashier.ConnectDatabase
{
    public partial class FrmConnectDatabase : Form
    {
        public FrmConnectDatabase()
        {
            InitializeComponent();
        }

        public void UpdateConfigFile_ERBusConnection(string con)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement xElement in XmlDoc.DocumentElement)
            {
                if (!string.IsNullOrEmpty(xElement.Name) && xElement.Name == "connectionStrings")
                {
                    if (!string.IsNullOrEmpty(xElement.FirstChild.Attributes[0].Value) && xElement.FirstChild.Attributes[0].Value == "ERBusConnection")
                    {
                        xElement.FirstChild.Attributes[1].Value = con;
                    }
                }
            }
            XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        public void UpdateConfigFile_ERBusCashier(string con)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement xElement in XmlDoc.DocumentElement)
            {
                if (!string.IsNullOrEmpty(xElement.Name) && xElement.Name == "connectionStrings")
                {
                    if (!string.IsNullOrEmpty(xElement.LastChild.Attributes[0].Value) && xElement.LastChild.Attributes[0].Value == "ERBusCashier")
                    {
                        xElement.LastChild.Attributes[1].Value = con;
                    }
                }
            }
            XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        private void btnCreateConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtHostName.Text) || string.IsNullOrEmpty(txtPassword.Text) ||
                string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtServiceName.Text))
            {
                NotificationLauncher.ShowNotificationWarning("Lỗi nhập", "Yêu cầu nhập đầy đủ thông tin !", 1,
                           "0x1", "0x8", "normal");
            }
            else
            {
                //Constructing connection string from the inputs
                StringBuilder connetStringOracle = new StringBuilder("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=");
                connetStringOracle.Append(txtHostName.Text.Trim());
                connetStringOracle.Append(")(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=");
                connetStringOracle.Append(txtServiceName.Text.Trim());
                connetStringOracle.Append(")));USER ID=");
                connetStringOracle.Append(txtUsername.Text.Trim());
                connetStringOracle.Append(";PASSWORD=");
                connetStringOracle.Append(txtPassword.Text.Trim());
                connetStringOracle.Append(";");
                string strCon = connetStringOracle.ToString();
                UpdateConfigFile_ERBusConnection(strCon);
                OracleConnection connection = new OracleConnection();
                try
                {
                    //test connect
                    ConfigurationManager.RefreshSection("connectionStrings");
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ERBusConnection"].ToString();
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        NotificationLauncher.ShowNotification("Thông báo", "Kết nối thành công với cơ sở dữ liệu Oracle", 1,
                            "0x1", "0x8", "normal");
                        this.Close();
                        this.Dispose();
                    }
                    else
                    {
                        NotificationLauncher.ShowNotificationError("Thông báo", "Lỗi không kết nối được với cơ sở dữ liệu Oracle", 1,
                            "0x1", "0x8", "normal");
                    }
                }
                catch
                {
                    NotificationLauncher.ShowNotificationError("Thông báo", "Không có khả năn kết nối tới HostName ! Kiểm tra thông tin kết nối", 1,
                            "0x1", "0x8", "normal");
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        private void btnCheckConnect_Click(object sender, EventArgs e)
        {
            OracleConnection connection = new OracleConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ERBusConnection"].ToString();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    NotificationLauncher.ShowNotification("Thông báo", "Kết nối thành công với cơ sở dữ liệu Oracle", 1,
                        "0x1", "0x8", "normal");
                    this.Close();
                    this.Dispose();
                }
                else
                {
                    NotificationLauncher.ShowNotificationError("Thông báo", "Lỗi không kết nối được với cơ sở dữ liệu Oracle", 1,
                        "0x1", "0x8", "normal");
                }
            }
            catch
            {
                NotificationLauncher.ShowNotificationError("Thông báo", "Lỗi không kết nối được với cơ sở dữ liệu", 1,
                         "0x1", "0x8", "normal");
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
      
        
        private void btnCreateConnectSql_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDatabaseNameSql.Text) || string.IsNullOrEmpty(txtPasswordSql.Text) ||
                string.IsNullOrEmpty(txtUsernameSql.Text))
            {
                NotificationLauncher.ShowNotificationWarning("Lỗi nhập", "Yêu cầu nhập đầy đủ thông tin !", 1,
                    "0x1", "0x8", "normal");
            }
            else
            {
                //Constructing connection string from the inputs
                StringBuilder connetStringSql = new StringBuilder("Data Source=");
                connetStringSql.Append(txtHostnameSql.Text.Trim());
                connetStringSql.Append(";Initial Catalog=");
                connetStringSql.Append(txtDatabaseNameSql.Text.Trim());
                connetStringSql.Append(";Integrated Security=True;User ID=");
                connetStringSql.Append(txtUsernameSql.Text.Trim());
                connetStringSql.Append(";Password=");
                connetStringSql.Append(txtPasswordSql.Text.Trim());
                connetStringSql.Append(";MultipleActiveResultSets=true");
                string strCon = connetStringSql.ToString();
                UpdateConfigFile_ERBusCashier(strCon);
                SqlConnection connection = new SqlConnection();
                try
                {
                    //test connect
                    ConfigurationManager.RefreshSection("connectionStrings");
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ERBusCashier"].ToString();
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        //khởi tạo cấu trúc db
                        string SqlCreateDbQuery = string.Format("SELECT DATABASE_ID FROM SYS.DATABASES WHERE NAME = '{0}'", "ERBUS_CASHIER");;
                        using (SqlCommand sqlCmd = new SqlCommand(SqlCreateDbQuery, connection))
                        {
                            object resultObj = sqlCmd.ExecuteScalar();
                            if (resultObj == null)
                            {
                                //câu lệnh create DB
                                SqlCommand cmdCreateDb = new SqlCommand();
                                cmdCreateDb.Connection = connection;
                                cmdCreateDb.CommandText = string.Format(@"CREATE DATABASE ERBUS_CASHIER");
                                int i = cmdCreateDb.ExecuteNonQuery();
                                if(i > 0)
                                {
                                   
                                }
                            }
                            else
                            {
                                if (!ConnectDatabaseService.CheckTableExistInDatabase("AU_DONVI"))
                                {

                                }
                            }
                        }
                        NotificationLauncher.ShowNotification("Thông báo", "Kết nối thành công với cơ sở dữ liệu SQL", 1,
                            "0x1", "0x8", "normal");


                        this.Close();
                        this.Dispose();
                    }
                    else
                    {
                        NotificationLauncher.ShowNotificationError("Thông báo", "Lỗi không kết nối được với cơ sở dữ liệu SQL", 1,
                            "0x1", "0x8", "normal");
                    }
                }
                catch
                {
                    NotificationLauncher.ShowNotificationError("Thông báo", "Không có khả năn kết nối tới HostName ! Kiểm tra thông tin kết nối", 1,
                            "0x1", "0x8", "normal");
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        private void btnCheckConnectSql_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ERBusCashier"].ToString();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    NotificationLauncher.ShowNotification("Thông báo", "Kết nối thành công với cơ sở dữ liệu SQL", 1,
                        "0x1", "0x8", "normal");
                    this.Close();
                    this.Dispose();
                }
                else
                {
                    NotificationLauncher.ShowNotificationError("Thông báo", "Lỗi không kết nối được với cơ sở dữ liệu SQL", 1,
                        "0x1", "0x8", "normal");
                }
            }
            catch
            {
                NotificationLauncher.ShowNotificationError("Thông báo", "Lỗi không kết nối được với cơ sở dữ liệu SQL", 1,
                         "0x1", "0x8", "normal");
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
