using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using ERBus.Cashier.Utils;

namespace ERBus.Cashier.Hethong
{
    public partial class FrmDoiMatKhau : Form
    {
        public FrmDoiMatKhau()
        {
            InitializeComponent();
            lblNotification.Text = "";
            txtUserName.Focus();
        }
        private void btnSavePassWord_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                lblNotification.Text = "Chưa nhập tài khoản";
                lblNotification.ForeColor = System.Drawing.Color.Red;
            }
            else if (string.IsNullOrEmpty(txtPassWordOld.Text))
            {
                lblNotification.Text = "Phải xác thực mật khẩu cũ";
                lblNotification.ForeColor = System.Drawing.Color.Red;
            }
            else if (string.IsNullOrEmpty(txtPassWordNew.Text))
            {
                lblNotification.Text = "Chưa nhập mật khẩu mới";
                lblNotification.ForeColor = System.Drawing.Color.Red;
            }
            else if (string.IsNullOrEmpty(txtPassWordNewConfirm.Text))
            {
                lblNotification.Text = "Phải xác thực mật khẩu mới";
                lblNotification.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (!txtPassWordNew.Text.Trim().Equals(txtPassWordNewConfirm.Text.Trim()))
                {
                    lblNotification.Text = "Xác thực mật khẩu mới không chính xác";
                    lblNotification.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    bool RESULT = LAY_THONGTIN_NGUOIDUNG(txtUserName.Text.Trim(), MD5Encrypt.MD5Hash(txtPassWordOld.Text.Trim()));
                    if (RESULT)
                    {
                       int RESULT_DOIMATKHAU =  DOIMATKHAU(txtUserName.Text.Trim(), MD5Encrypt.MD5Hash(txtPassWordNew.Text.Trim()));
                        if(RESULT_DOIMATKHAU > 0)
                        {
                            lblNotification.Text = "Đổi mật khẩu thành công";
                            lblNotification.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblNotification.Text = "Đổi mật khẩu không thành công";
                            lblNotification.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lblNotification.Text = "Tài khoản hoặc mật khẩu cũ không đúng";
                        lblNotification.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        private bool LAY_THONGTIN_NGUOIDUNG(string USERNAME, string PASSWORD)
        {
            bool RESULT = false;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand commamdNguoiDung = new OracleCommand();
                        commamdNguoiDung.Connection = connection;
                        commamdNguoiDung.CommandText = string.Format(@"SELECT COUNT(*) FROM NGUOIDUNG WHERE USERNAME = :USERNAME AND PASSWORD = :PASSWORD");
                        commamdNguoiDung.Parameters.Add("USERNAME", OracleDbType.NVarchar2, 50).Value = USERNAME;
                        commamdNguoiDung.Parameters.Add("PASSWORD", OracleDbType.NVarchar2, 50).Value = PASSWORD;
                        OracleDataReader dataReaderNguoiDung = commamdNguoiDung.ExecuteReader();
                        if (dataReaderNguoiDung.HasRows)
                        {
                            RESULT = true;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("KHÔNG TRUY VẤN ĐƯỢC THÔNG TIN NGƯỜI DÙNG");
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return RESULT;
        }

        private int DOIMATKHAU(string USERNAME, string NEW_PASSWORD)
        {
            int RESULT = 0;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand commamdNguoiDung = new OracleCommand();
                        commamdNguoiDung.Connection = connection;
                        commamdNguoiDung.CommandText = string.Format(@"UPDATE NGUOIDUNG SET PASSWORD = '"+ NEW_PASSWORD + "' WHERE USERNAME = '"+ USERNAME + "'");
                        RESULT = commamdNguoiDung.ExecuteNonQuery();
                    }
                    catch
                    {
                        MessageBox.Show("XẢY RA LỖI KHI CẬP NHẬT THÔNG TIN NGƯỜI DÙNG");
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return RESULT;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
