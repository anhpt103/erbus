using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ERBus.Cashier.Common;
using ERBus.Cashier.ConnectDatabase;
using ERBus.Cashier.Giaodich.XuatBanLe;
using ERBus.Cashier.Utils;
using DevExpress.XtraSplashScreen;
using Oracle.ManagedDataAccess.Client;
using ERBus.Cashier.Hethong;
namespace ERBus.Cashier
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            txtNgayPhatSinh.Text = Environment.MachineName;
        }

        private void LOGIN()
        {
            string Username = txtUserName.Text.Trim();
            string passWord = txtPassWord.Text.Trim();
            string passMd5 = MD5Encrypt.MD5Hash(passWord).Trim();
            //check true false login
            try
            {
                //BEGIN LOGIN
                if (Config.CheckConnectToServer()) // nếu có mạng lan
                {
                    using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                
                                OracleCommand cmd = new OracleCommand();
                                cmd.Connection = connection;
                                cmd.CommandText = string.Format(@"SELECT USERNAME,MANHANVIEN,TENNHANVIEN,UNITCODE FROM NGUOIDUNG WHERE USERNAME = '" + Username + "' AND PASSWORD = '" + passMd5 + "'");
                                OracleDataReader dataReader = null;
                                dataReader = cmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        Session.Session.CurrentMaNhanVien = dataReader["MANHANVIEN"].ToString();
                                        Session.Session.CurrentTenNhanVien = dataReader["TENNHANVIEN"].ToString();
                                        Session.Session.CurrentUnitCode = dataReader["UNITCODE"].ToString();
                                        Session.Session.CurrentCodeStore = Session.Session.CurrentUnitCode;
                                        SYNCHRONIZE_DATA.KHOASODULIEU();
                                        cmd.Parameters.Clear();
                                        cmd.CommandText = string.Format(@"SELECT TEN_CUAHANG,DIACHI,SODIENTHOAI FROM CUAHANG WHERE MA_CUAHANG = '" + Session.Session.CurrentUnitCode + "'");
                                        OracleDataReader dataReaderDonVi = null;
                                        dataReaderDonVi = cmd.ExecuteReader();
                                        if (dataReaderDonVi.HasRows)
                                        {
                                            while (dataReaderDonVi.Read())
                                            {
                                                Session.Session.CurrentPhone = dataReaderDonVi["SODIENTHOAI"].ToString();
                                                Session.Session.CurrentAddress = dataReaderDonVi["DIACHI"].ToString();
                                                Session.Session.CurrentNameStore = dataReaderDonVi["TEN_CUAHANG"].ToString();
                                            }
                                        }
                                        dataReaderDonVi.Close();
                                        Session.Session.CurrentUserName = dataReader["USERNAME"].ToString();
                                        Session.Session.CurrentNgayPhatSinh = FrmXuatBanLeService.GET_NGAYHACHTOAN_CSDL_ORACLE();
                                        Session.Session.CurrentTableNamePeriod = FrmXuatBanLeService.GET_TABLE_NAME_NGAYHACHTOAN_CSDL_ORACLE();
                                        Session.Session.CurrentWareHouse = "KH2";
                                        SplashScreenManager.ShowForm(typeof(WaitForm1));
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_NGUOIDUNG();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_KHACHHANG();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_KHUYENMAI();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_KHUYENMAI_CHITIET();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_THAMSOHETHONG();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_MATHANG();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_MATHANG_GIA();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_CUAHANG();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_DONVITINH();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_THUE();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_HANGKHACHHANG();
                                        
                                        SplashScreenManager.CloseForm();
                                        FrmMain frmMain = new FrmMain();
                                        frmMain.ShowDialog();
                                        break;
                                    }
                                }
                                else
                                {
                                    NotificationLauncher.ShowNotificationWarning("Thông báo", "Thông tin đăng nhập không đúng", 1, "0x1", "0x8", "normal");
                                    txtPassWord.Text = "";
                                    txtPassWord.Focus();}
                            }
                            catch (Exception ex)
                            {
                                NotificationLauncher.ShowNotificationError("Thông báo", "Không có kết nối với cơ sở dữ liệu máy chủ", 1, "0x1", "0x8", "normal");
                                WriteLogs.LogError(ex);
                            }
                            finally
                            {
                                connection.Close();
                                connection.Dispose();
                            }
                        }
                        else
                        {
                            NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                        }
                    }
                }
                else
                {
                    //Kết nối với SQL
                    using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                    {
                        connectionSa.Open();
                        if (connectionSa.State == ConnectionState.Open)
                        {
                            try
                            {
                                SqlCommand cmdSelectSa = new SqlCommand();
                                cmdSelectSa.Connection = connectionSa;
                                cmdSelectSa.CommandText = string.Format(@"SELECT MANHANVIEN,TENNHANVIEN,UNITCODE FROM [dbo].[NGUOIDUNG] WHERE USERNAME = '" + Username + "' AND PASSWORD = '" + passMd5 + "'");
                                SqlDataReader dataReader = null;
                                dataReader = cmdSelectSa.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        Session.Session.CurrentMaNhanVien = dataReader["MANHANVIEN"].ToString();
                                        Session.Session.CurrentTenNhanVien = dataReader["TENNHANVIEN"].ToString();
                                        Session.Session.CurrentUnitCode = dataReader["UNITCODE"].ToString();
                                        Session.Session.CurrentCodeStore = Session.Session.CurrentUnitCode;
                                        SqlCommand commandDonVi = new SqlCommand();
                                        commandDonVi.Connection = connectionSa;
                                        commandDonVi.CommandText = string.Format(@"SELECT TEN_CUAHANG,DIACHI,SODIENTHOAI FROM [dbo].[CUAHANG] WHERE MA_CUAHANG = '" + Session.Session.CurrentUnitCode + "'");
                                        SqlDataReader dataReaderDonVi = null;
                                        dataReaderDonVi = commandDonVi.ExecuteReader();
                                        if (dataReaderDonVi.HasRows)
                                        {
                                            while (dataReaderDonVi.Read())
                                            {
                                                Session.Session.CurrentPhone = dataReaderDonVi["SODIENTHOAI"].ToString();
                                                Session.Session.CurrentAddress = dataReaderDonVi["DIACHI"].ToString();
                                                Session.Session.CurrentNameStore = dataReaderDonVi["TEN_CUAHANG"].ToString();
                                            }
                                        }
                                        dataReaderDonVi.Close();
                                        Session.Session.CurrentUserName = dataReader["USERNAME"].ToString();
                                        //nếu mất mạng thì ngày phát sinh là ngày hiện tại
                                        Session.Session.CurrentNgayPhatSinh = DateTime.Now;
                                        //Session.Session.CurrentWareHouse = (Session.Session.CurrentUnitCode + "-K2").ToUpper().Trim();
                                        FrmMain frmMain = new FrmMain();
                                        frmMain.ShowDialog();
                                        break;
                                    }
                                }
                                else
                                {
                                    NotificationLauncher.ShowNotificationWarning("Thông báo", "Thông tin đăng nhập không đúng", 1, "0x1", "0x8", "normal");
                                    txtPassWord.Text = "";
                                    txtPassWord.Focus();
                                }
                            }
                            catch (Exception ex)
                            {
                                NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu máy bán", 1, "0x1", "0x8", "normal");
                                WriteLogs.LogError(ex);
                            }
                            finally
                            {
                                connectionSa.Close();
                                connectionSa.Dispose();
                            }
                        }
                        else
                        {
                            NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                        }
                    }
                }
                //END LOGIN
            }
            catch (Exception ex)
            {
                NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu máy bán", 1, "0x1", "0x8", "normal");
                WriteLogs.LogError(ex);
            }
        }
        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            LOGIN();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPassWord.Text != "" && txtUserName.Text != "")
                {
                    LOGIN();
                }
                else
                {
                    if (string.IsNullOrEmpty(txtUserName.Text)) txtUserName.Focus();
                    else txtPassWord.Focus();
                }
            }
        }

        private void txtPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPassWord.Text != "" && txtUserName.Text != "")
                {
                    LOGIN();
                }
                else
                {
                    if (string.IsNullOrEmpty(txtUserName.Text)) txtUserName.Focus();
                    else txtPassWord.Focus();
                }
            }
        }

        private void btnCreateConnect_Click(object sender, EventArgs e)
        {
            FrmConnectDatabase frmConnectDatabase = new FrmConnectDatabase();
            frmConnectDatabase.ShowDialog();
        }

        private void btnModifieldPassword_Click(object sender, EventArgs e)
        {
            FrmDoiMatKhau frmDoiMatKhau = new FrmDoiMatKhau();
            frmDoiMatKhau.ShowDialog();
        }
    }
}
