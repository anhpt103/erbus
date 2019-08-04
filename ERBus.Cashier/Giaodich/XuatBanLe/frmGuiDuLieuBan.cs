using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ERBus.Cashier.Common;
using DevExpress.XtraSplashScreen;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public partial class frmGuiDuLieuBan : Form
    {
        public frmGuiDuLieuBan()
        {
            InitializeComponent();
        }

        private void DongBoDuLieuBan_Click(object sender, EventArgs e)
        {
            DongBoTatCaDuLieuBan();
        }

        public static void DongBoTatCaDuLieuBan()
        {
            int totalParent = 0;
            int totalChildren = 0;
            try
            {
                SplashScreenManager.ShowForm(typeof(WaitForm1));
                using (OracleConnection connectionOracle = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    connectionOracle.Open();
                    if (connectionOracle.State == ConnectionState.Open)
                    {
                        try
                        {
                            using (SqlConnection connectionClient = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionClient.Open();
                                try
                                {
                                    if (connectionClient.State == ConnectionState.Open)
                                    {
                                        //bảng cha
                                        SqlCommand cmdSql = new SqlCommand();
                                        cmdSql.Connection = connectionClient;
                                        cmdSql.CommandText = @"SELECT [ID],[MA_GIAODICH],[LOAI_GIAODICH],[NGAY_GIAODICH],[MAKHACHHANG],[THOIGIAN_TAO],[TIENKHACH_TRA],[TIEN_TRALAI_KHACH],[MAKHO_XUAT],[MA_VOUCHER],[DIENGIAI],[I_CREATE_DATE],[I_CREATE_BY],[I_UPDATE_DATE],[I_UPDATE_BY],[I_STATE],[UNITCODE]  FROM [ERBUS_CASHIER].[dbo].[GIAODICH]";
                                        cmdSql.CommandType = CommandType.Text;
                                        SqlDataReader dataReaderClient = cmdSql.ExecuteReader();
                                        if (dataReaderClient.HasRows)
                                        {
                                            while (dataReaderClient.Read())
                                            {
                                                OracleCommand cmdOracleSelect = new OracleCommand();
                                                cmdOracleSelect.Connection = connectionOracle;
                                                cmdOracleSelect.CommandText = @"SELECT ID,MA_GIAODICH,UNITCODE FROM GIAODICH WHERE ID = :ID AND MA_GIAODICH = :MA_GIAODICH AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'";
                                                cmdOracleSelect.CommandType = CommandType.Text;
                                                cmdOracleSelect.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = dataReaderClient["ID"].ToString();
                                                cmdOracleSelect.Parameters.Add("MA_GIAODICH", OracleDbType.NVarchar2, 70).Value = dataReaderClient["MA_GIAODICH"].ToString();
                                                OracleDataReader dataReaderOracle = cmdOracleSelect.ExecuteReader();
                                                if (!dataReaderOracle.HasRows)
                                                {
                                                    DateTime NGAY_GIAODICH;
                                                    DateTime I_CREATE_DATE = DateTime.Now;
                                                    DateTime I_UPDATE_DATE = DateTime.Now;
                                                    decimal TIENKHACH_TRA = 0;
                                                    decimal TIEN_TRALAI_KHACH = 0;
                                                    string ID = dataReaderClient["ID"] != null
                                                        ? dataReaderClient["ID"].ToString()
                                                        : Guid.NewGuid().ToString();
                                                    string MA_GIAODICH = dataReaderClient["MA_GIAODICH"] != null
                                                        ? dataReaderClient["MA_GIAODICH"].ToString()
                                                        : "";
                                                    string LOAI_GIAODICH = dataReaderClient["LOAI_GIAODICH"] != null
                                                        ? dataReaderClient["LOAI_GIAODICH"].ToString()
                                                        : "";
                                                    DateTime.TryParse(
                                                        dataReaderClient["NGAY_GIAODICH"] != null
                                                            ? dataReaderClient["NGAY_GIAODICH"].ToString()
                                                            : "", out NGAY_GIAODICH);
                                                    string MAKHACHHANG = dataReaderClient["MAKHACHHANG"] != null
                                                        ? dataReaderClient["MAKHACHHANG"].ToString()
                                                        : "";
                                                    string THOIGIAN_TAO = dataReaderClient["THOIGIAN_TAO"] != null
                                                        ? dataReaderClient["THOIGIAN_TAO"].ToString()
                                                        : "";
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENKHACH_TRA"] != null
                                                            ? dataReaderClient["TIENKHACH_TRA"].ToString()
                                                            : "", out TIENKHACH_TRA);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIEN_TRALAI_KHACH"] != null
                                                            ? dataReaderClient["TIEN_TRALAI_KHACH"].ToString()
                                                            : "", out TIEN_TRALAI_KHACH);
                                                    string MAKHO_XUAT = dataReaderClient["MAKHO_XUAT"] != null
                                                       ? dataReaderClient["MAKHO_XUAT"].ToString()
                                                       : "";
                                                    string MA_VOUCHER = dataReaderClient["MA_VOUCHER"] != null
                                                       ? dataReaderClient["MA_VOUCHER"].ToString()
                                                       : "";
                                                    string DIENGIAI = dataReaderClient["DIENGIAI"] != null
                                                       ? dataReaderClient["DIENGIAI"].ToString()
                                                       : "";
                                                    DateTime.TryParse(
                                                    dataReaderClient["I_CREATE_DATE"] != null
                                                        ? dataReaderClient["I_CREATE_DATE"].ToString()
                                                        : "", out I_CREATE_DATE);
                                                    string I_CREATE_BY = dataReaderClient["I_CREATE_BY"] != null
                                                        ? dataReaderClient["I_CREATE_BY"].ToString()
                                                        : "";
                                                    DateTime.TryParse(
                                                    dataReaderClient["I_UPDATE_DATE"] != null
                                                        ? dataReaderClient["I_UPDATE_DATE"].ToString()
                                                        : "", out I_UPDATE_DATE);
                                                    string I_UPDATE_BY = dataReaderClient["I_UPDATE_BY"] != null
                                                        ? dataReaderClient["I_UPDATE_BY"].ToString()
                                                        : "";
                                                    string I_STATE = dataReaderClient["I_STATE"] != null
                                                        ? dataReaderClient["I_STATE"].ToString()
                                                        : "";
                                                    string UNITCODE = dataReaderClient["UNITCODE"] != null
                                                        ? dataReaderClient["UNITCODE"].ToString()
                                                        : "";
                                                    OracleCommand cmdOracleInsert = new OracleCommand();
                                                    cmdOracleInsert.Connection = connectionOracle;
                                                    cmdOracleInsert.CommandText = string.Format(@"INSERT INTO GIAODICH(ID,MA_GIAODICH,LOAI_GIAODICH,NGAY_GIAODICH,MAKHACHHANG,THOIGIAN_TAO,TIENKHACH_TRA,TIEN_TRALAI_KHACH,MAKHO_XUAT,MA_VOUCHER,DIENGIAI,I_CREATE_DATE,I_CREATE_BY,I_UPDATE_DATE,I_UPDATE_BY,I_STATE,UNITCODE) VALUES (:ID,:MA_GIAODICH,:LOAI_GIAODICH,:NGAY_GIAODICH,:MAKHACHHANG,:THOIGIAN_TAO,:TIENKHACH_TRA,:TIEN_TRALAI_KHACH,:MAKHO_XUAT,:MA_VOUCHER,:DIENGIAI,:I_CREATE_DATE,:I_CREATE_BY,:I_UPDATE_DATE,:I_UPDATE_BY,:I_STATE,:UNITCODE)");
                                                    cmdOracleInsert.CommandType = CommandType.Text;
                                                    cmdOracleInsert.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = ID;
                                                    cmdOracleInsert.Parameters.Add("MA_GIAODICH", OracleDbType.NVarchar2, 70).Value = MA_GIAODICH;
                                                    cmdOracleInsert.Parameters
                                                            .Add("LOAI_GIAODICH", OracleDbType.NVarchar2, 15).Value =
                                                        LOAI_GIAODICH;
                                                    cmdOracleInsert.Parameters.Add("NGAY_GIAODICH", OracleDbType.Date).Value =
                                                        NGAY_GIAODICH;
                                                    cmdOracleInsert.Parameters.Add("MAKHACHHANG", OracleDbType.NVarchar2, 50)
                                                        .Value = MAKHACHHANG;
                                                    cmdOracleInsert.Parameters.Add("THOIGIAN_TAO", OracleDbType.NVarchar2, 12)
                                                        .Value = THOIGIAN_TAO;
                                                    cmdOracleInsert.Parameters.Add("TIENKHACH_TRA", OracleDbType.Decimal).Value =
                                                        TIENKHACH_TRA;
                                                    cmdOracleInsert.Parameters.Add("TIEN_TRALAI_KHACH", OracleDbType.Decimal).Value =
                                                        TIEN_TRALAI_KHACH;
                                                    cmdOracleInsert.Parameters.Add("MAKHO_XUAT", OracleDbType.NVarchar2, 50)
                                                        .Value = MAKHO_XUAT;
                                                    cmdOracleInsert.Parameters.Add("MA_VOUCHER", OracleDbType.NVarchar2, 50)
                                                        .Value = MA_VOUCHER;
                                                    cmdOracleInsert.Parameters.Add("DIENGIAI", OracleDbType.NVarchar2, 50)
                                                       .Value = DIENGIAI;
                                                    cmdOracleInsert.Parameters.Add("I_CREATE_DATE", OracleDbType.Date).Value = I_CREATE_DATE;
                                                    cmdOracleInsert.Parameters.Add("I_CREATE_BY", OracleDbType.NVarchar2, 25).Value = I_CREATE_BY;
                                                    cmdOracleInsert.Parameters.Add("I_UPDATE_DATE", OracleDbType.Date).Value = I_UPDATE_DATE;
                                                    cmdOracleInsert.Parameters.Add("I_UPDATE_BY", OracleDbType.NVarchar2, 25).Value = I_UPDATE_BY;
                                                    cmdOracleInsert.Parameters.Add("I_STATE", OracleDbType.NVarchar2, 1).Value = I_STATE;
                                                    cmdOracleInsert.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 10).Value = UNITCODE;
                                                    int rowCount = cmdOracleInsert.ExecuteNonQuery();
                                                    if (rowCount > 0) totalParent += rowCount;
                                                }
                                            }
                                        }

                                        //bảng con
                                        SqlCommand cmdSqlDetails = new SqlCommand();
                                        cmdSqlDetails.Connection = connectionClient;
                                        cmdSqlDetails.CommandText = string.Format(@"SELECT [ID],[MA_GIAODICH],[MAHANG],[MATHUE_RA],[SOLUONG],[GIABANLE_VAT],[MA_KHUYENMAI],[TYLE_KHUYENMAI],[TIEN_KHUYENMAI],[TYLE_CHIETKHAU],[TIEN_CHIETKHAU],[TIENTHE_VIP],[TIEN_VOUCHER],[THANHTIEN],[SAPXEP]  FROM [ERBUS_CASHIER].[dbo].[GIAODICH_CHITIET]");
                                        cmdSqlDetails.CommandType = CommandType.Text;
                                        SqlDataReader dataReaderClientDetails = cmdSqlDetails.ExecuteReader();
                                        if (dataReaderClientDetails.HasRows)
                                        {
                                            while (dataReaderClientDetails.Read())
                                            {
                                                OracleCommand cmdOracleSelect = new OracleCommand();
                                                cmdOracleSelect.Connection = connectionOracle;
                                                cmdOracleSelect.CommandText = string.Format(@"SELECT ID,MA_GIAODICH FROM GIAODICH_CHITIET WHERE ID=:ID AND MA_GIAODICH=:MA_GIAODICH");
                                                cmdOracleSelect.CommandType = CommandType.Text;
                                                cmdOracleSelect.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = dataReaderClientDetails["ID"].ToString();
                                                cmdOracleSelect.Parameters.Add("MA_GIAODICH", OracleDbType.NVarchar2, 70).Value = dataReaderClientDetails["MA_GIAODICH"].ToString();
                                                OracleDataReader dataReaderOracleDetails = cmdOracleSelect.ExecuteReader();
                                                if (!dataReaderOracleDetails.HasRows)
                                                {
                                                    decimal SOLUONG = 0;
                                                    decimal GIABANLE_VAT = 0;
                                                    decimal TYLE_KHUYENMAI = 0;
                                                    decimal TIEN_KHUYENMAI = 0;
                                                    decimal TYLE_CHIETKHAU = 0;
                                                    decimal TIEN_CHIETKHAU = 0;
                                                    decimal TIENTHE_VIP = 0;
                                                    decimal TIEN_VOUCHER = 0;
                                                    decimal THANHTIEN = 0;
                                                    int SAPXEP = 0;
                                                    string ID = dataReaderClientDetails["ID"] != null
                                                        ? dataReaderClientDetails["ID"].ToString()
                                                        : Guid.NewGuid().ToString();
                                                    string MA_GIAODICH = dataReaderClientDetails["MA_GIAODICH"] != null
                                                        ? dataReaderClientDetails["MA_GIAODICH"].ToString()
                                                        : "";
                                                    string MAHANG = dataReaderClientDetails["MAHANG"] != null
                                                        ? dataReaderClientDetails["MAHANG"].ToString()
                                                        : "";
                                                    string MATHUE_RA = dataReaderClientDetails["MATHUE_RA"] != null
                                                        ? dataReaderClientDetails["MATHUE_RA"].ToString()
                                                        : "";
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["SOLUONG"] != null
                                                            ? dataReaderClientDetails["SOLUONG"].ToString()
                                                            : "", out SOLUONG);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["GIABANLE_VAT"] != null
                                                            ? dataReaderClientDetails["GIABANLE_VAT"].ToString()
                                                            : "", out GIABANLE_VAT);
                                                    string MA_KHUYENMAI = dataReaderClientDetails["MA_KHUYENMAI"] != null
                                                        ? dataReaderClientDetails["MA_KHUYENMAI"].ToString()
                                                        : "";
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TYLE_KHUYENMAI"] != null
                                                            ? dataReaderClientDetails["TYLE_KHUYENMAI"].ToString()
                                                            : "", out TYLE_KHUYENMAI);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TIEN_KHUYENMAI"] != null
                                                            ? dataReaderClientDetails["TIEN_KHUYENMAI"].ToString()
                                                            : "", out TIEN_KHUYENMAI);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TYLE_CHIETKHAU"] != null
                                                            ? dataReaderClientDetails["TYLE_CHIETKHAU"].ToString()
                                                            : "", out TYLE_CHIETKHAU);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TIEN_CHIETKHAU"] != null
                                                            ? dataReaderClientDetails["TIEN_CHIETKHAU"].ToString()
                                                            : "", out TIEN_CHIETKHAU);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TIENTHE_VIP"] != null
                                                            ? dataReaderClientDetails["TIENTHE_VIP"].ToString()
                                                            : "", out TIENTHE_VIP);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TIEN_VOUCHER"] != null
                                                            ? dataReaderClientDetails["TIEN_VOUCHER"].ToString()
                                                            : "", out TIEN_VOUCHER);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["THANHTIEN"] != null
                                                            ? dataReaderClientDetails["THANHTIEN"].ToString()
                                                            : "", out THANHTIEN);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TIEN_VOUCHER"] != null
                                                            ? dataReaderClientDetails["TIEN_VOUCHER"].ToString()
                                                            : "", out TIEN_VOUCHER);
                                                    int.TryParse(
                                                        dataReaderClientDetails["SAPXEP"] != null
                                                            ? dataReaderClientDetails["SAPXEP"].ToString()
                                                            : "", out SAPXEP);
                                                    OracleCommand cmdOracleInsert = new OracleCommand();
                                                    cmdOracleInsert.Connection = connectionOracle;
                                                    cmdOracleInsert.CommandText = string.Format(@"INSERT INTO GIAODICH_CHITIET(ID,MA_GIAODICH,MAHANG,MATHUE_RA,SOLUONG,GIABANLE_VAT,MA_KHUYENMAI,TYLE_KHUYENMAI,TIEN_KHUYENMAI,TYLE_CHIETKHAU,TIEN_CHIETKHAU,TIENTHE_VIP,TIEN_VOUCHER,THANHTIEN,SAPXEP) 
                                                    VALUES (:ID,:MA_GIAODICH,:MAHANG,:MATHUE_RA,:SOLUONG,:GIABANLE_VAT,:MA_KHUYENMAI,:TYLE_KHUYENMAI,:TIEN_KHUYENMAI,:TYLE_CHIETKHAU,:TIEN_CHIETKHAU,:TIENTHE_VIP,:TIEN_VOUCHER,:THANHTIEN,:SAPXEP)");
                                                    cmdOracleInsert.CommandType = CommandType.Text;
                                                    cmdOracleInsert.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = ID;
                                                    cmdOracleInsert.Parameters.Add("MA_GIAODICH", OracleDbType.NVarchar2, 70)
                                                        .Value = MA_GIAODICH;
                                                    cmdOracleInsert.Parameters.Add("MAHANG", OracleDbType.NVarchar2, 50)
                                                        .Value = MAHANG;
                                                    cmdOracleInsert.Parameters.Add("MATHUE_RA", OracleDbType.NVarchar2, 50)
                                                        .Value = MATHUE_RA;
                                                    cmdOracleInsert.Parameters.Add("SOLUONG", OracleDbType.Decimal).Value =
                                                        SOLUONG;
                                                    cmdOracleInsert.Parameters.Add("GIABANLE_VAT", OracleDbType.Decimal).Value =
                                                        GIABANLE_VAT;
                                                    cmdOracleInsert.Parameters.Add("MA_KHUYENMAI", OracleDbType.NVarchar2, 30)
                                                        .Value = MA_KHUYENMAI;
                                                    cmdOracleInsert.Parameters.Add("TYLE_KHUYENMAI", OracleDbType.Decimal)
                                                        .Value = TYLE_KHUYENMAI;
                                                    cmdOracleInsert.Parameters.Add("TIEN_KHUYENMAI", OracleDbType.Decimal)
                                                        .Value = TIEN_KHUYENMAI;
                                                    cmdOracleInsert.Parameters.Add("TYLE_CHIETKHAU", OracleDbType.Decimal)
                                                        .Value = TYLE_CHIETKHAU;
                                                    cmdOracleInsert.Parameters.Add("TIEN_CHIETKHAU", OracleDbType.Decimal)
                                                        .Value = TIEN_CHIETKHAU;
                                                    cmdOracleInsert.Parameters.Add("TIENTHE_VIP", OracleDbType.Decimal).Value =
                                                        TIENTHE_VIP;
                                                    cmdOracleInsert.Parameters.Add("TIEN_VOUCHER", OracleDbType.Decimal).Value =
                                                        TIEN_VOUCHER;
                                                    cmdOracleInsert.Parameters.Add("THANHTIEN", OracleDbType.Decimal).Value =
                                                        THANHTIEN;
                                                    cmdOracleInsert.Parameters.Add("SAPXEP", OracleDbType.Int32).Value =
                                                        SAPXEP;
                                                    int rowCount = cmdOracleInsert.ExecuteNonQuery();
                                                    if (rowCount > 0) totalChildren += rowCount;
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {

                                }
                                finally
                                {
                                    connectionClient.Close();
                                }
                            }
                        }
                        catch
                        {

                        }
                        finally
                        {
                            connectionOracle.Close();
                            SplashScreenManager.CloseForm();
                        }
                    }
                    MessageBox.Show("Đồng bộ thành công " + totalParent + " bản ghi ");
                    MessageBox.Show("Đồng bộ thành công " + totalChildren + " bản ghi giao dịch con");
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
                MessageBox.Show("Xảy ra lỗi khi đồng bộ dữ liệu");
                SplashScreenManager.CloseForm();
            }
        }
    }
}
