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

        private void GuiDuLieuBan_Click(object sender, EventArgs e)
        {
            DateTime ngayBanHang = dateTimeNgayBan.Value;
            GuiDuLieuBanTheoNgay(ngayBanHang);
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
                                        cmdSql.CommandText =
                                            @"SELECT ID,MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,NGAYTAO,NGAYPHATSINH,MANGUOITAO,NGUOITAO,MAQUAYBAN,LOAIGIAODICH,HINHTHUCTHANHTOAN,TIENKHACHDUA,TIENVOUCHER,TIENTHEVIP,TIENTRALAI,TIENTHE,TIENCOD,TIENMAT,TTIENCOVAT,THOIGIAN,MAKHACHHANG,UNITCODE FROM dbo.NVGDQUAY_ASYNCCLIENT WHERE UNITCODE = '" +
                                            Session.Session.CurrentUnitCode + "'";cmdSql.CommandType = CommandType.Text;
                                        SqlDataReader dataReaderClient = cmdSql.ExecuteReader();
                                        if (dataReaderClient.HasRows)
                                        {
                                            while (dataReaderClient.Read())
                                            {
                                                OracleCommand cmdOracleSelect = new OracleCommand();
                                                cmdOracleSelect.Connection = connectionOracle;
                                                cmdOracleSelect.CommandText =
                                                    @"SELECT ID,MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,UNITCODE FROM NVGDQUAY_ASYNCCLIENT WHERE ID = :ID AND MAGIAODICHQUAYPK = :MAGIAODICHQUAYPK AND UNITCODE = '" +
                                                    Session.Session.CurrentUnitCode + "'";
                                                cmdOracleSelect.CommandType = CommandType.Text;
                                                cmdOracleSelect.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value =
                                                    dataReaderClient["ID"].ToString();
                                                cmdOracleSelect.Parameters.Add("MAGIAODICHQUAYPK", OracleDbType.NVarchar2, 50)
                                                    .Value = dataReaderClient["MAGIAODICHQUAYPK"].ToString();
                                                OracleDataReader dataReaderOracle = cmdOracleSelect.ExecuteReader();
                                                if (!dataReaderOracle.HasRows)
                                                {
                                                    int LOAIGIAODICH = 0;
                                                    DateTime NGAYTAO;
                                                    DateTime NGAYPHATSINH;
                                                    DateTime I_CREATE_DATE = DateTime.Now;
                                                    DateTime I_UPDATE_DATE = DateTime.Now;
                                                    decimal TIENKHACHDUA = 0;
                                                    decimal TIENVOUCHER = 0;
                                                    decimal TIENTHEVIP = 0;
                                                    decimal TIENTRALAI = 0;
                                                    decimal TIENTHE = 0;
                                                    decimal TIENCOD = 0;
                                                    decimal TIENMAT = 0;
                                                    decimal TTIENCOVAT = 0;
                                                    string ID = dataReaderClient["ID"] != null
                                                        ? dataReaderClient["ID"].ToString()
                                                        : Guid.NewGuid().ToString();
                                                    string MAGIAODICH = dataReaderClient["MAGIAODICH"] != null
                                                        ? dataReaderClient["MAGIAODICH"].ToString()
                                                        : "";
                                                    string MAGIAODICHQUAYPK = dataReaderClient["MAGIAODICHQUAYPK"] != null
                                                        ? dataReaderClient["MAGIAODICHQUAYPK"].ToString()
                                                        : "";
                                                    string MADONVI = dataReaderClient["MADONVI"] != null
                                                        ? dataReaderClient["MADONVI"].ToString()
                                                        : "";
                                                    int.TryParse(
                                                        dataReaderClient["LOAIGIAODICH"] != null
                                                            ? dataReaderClient["LOAIGIAODICH"].ToString()
                                                            : "", out LOAIGIAODICH);
                                                    DateTime.TryParse(
                                                        dataReaderClient["NGAYTAO"] != null
                                                            ? dataReaderClient["NGAYTAO"].ToString()
                                                            : "", out NGAYTAO);
                                                    DateTime.TryParse(
                                                        dataReaderClient["NGAYPHATSINH"] != null
                                                            ? dataReaderClient["NGAYPHATSINH"].ToString()
                                                            : "", out NGAYPHATSINH);
                                                    string MANGUOITAO = dataReaderClient["MANGUOITAO"] != null
                                                        ? dataReaderClient["MANGUOITAO"].ToString()
                                                        : "";
                                                    string NGUOITAO = dataReaderClient["NGUOITAO"] != null
                                                        ? dataReaderClient["NGUOITAO"].ToString()
                                                        : "";
                                                    string MAQUAYBAN = dataReaderClient["MAQUAYBAN"] != null
                                                        ? dataReaderClient["MAQUAYBAN"].ToString()
                                                        : "";
                                                    string HINHTHUCTHANHTOAN = dataReaderClient["HINHTHUCTHANHTOAN"] != null
                                                        ? dataReaderClient["HINHTHUCTHANHTOAN"].ToString()
                                                        : "";
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENKHACHDUA"] != null
                                                            ? dataReaderClient["TIENKHACHDUA"].ToString()
                                                            : "", out TIENKHACHDUA);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENVOUCHER"] != null
                                                            ? dataReaderClient["TIENVOUCHER"].ToString()
                                                            : "", out TIENVOUCHER);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENTHEVIP"] != null
                                                            ? dataReaderClient["TIENTHEVIP"].ToString()
                                                            : "", out TIENTHEVIP);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENTRALAI"] != null
                                                            ? dataReaderClient["TIENTRALAI"].ToString()
                                                            : "", out TIENTRALAI);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENTHE"] != null
                                                            ? dataReaderClient["TIENTHE"].ToString()
                                                            : "", out TIENTHE);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENCOD"] != null
                                                            ? dataReaderClient["TIENCOD"].ToString()
                                                            : "", out TIENCOD);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENMAT"] != null
                                                            ? dataReaderClient["TIENMAT"].ToString()
                                                            : "", out TIENMAT);
                                                    decimal.TryParse(
                                                        dataReaderClient["TTIENCOVAT"] != null
                                                            ? dataReaderClient["TTIENCOVAT"].ToString()
                                                            : "", out TTIENCOVAT);
                                                    string THOIGIAN = dataReaderClient["THOIGIAN"] != null
                                                        ? dataReaderClient["THOIGIAN"].ToString()
                                                        : "";
                                                    string MAKHACHHANG = dataReaderClient["MAKHACHHANG"] != null
                                                        ? dataReaderClient["MAKHACHHANG"].ToString()
                                                        : "";
                                                    string UNITCODE = dataReaderClient["UNITCODE"] != null
                                                        ? dataReaderClient["UNITCODE"].ToString()
                                                        : "";
                                                    OracleCommand cmdOracleInsert = new OracleCommand();
                                                    cmdOracleInsert.Connection = connectionOracle;
                                                    cmdOracleInsert.CommandText = string.Format(
                                                        @"INSERT INTO NVGDQUAY_ASYNCCLIENT(ID,MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,LOAIGIAODICH,NGAYTAO,MANGUOITAO,
                                            NGUOITAO,MAQUAYBAN,NGAYPHATSINH,HINHTHUCTHANHTOAN,TIENKHACHDUA,TIENVOUCHER,TIENTHEVIP,TIENTRALAI,TIENTHE,TIENCOD,TIENMAT,TTIENCOVAT,THOIGIAN,MAKHACHHANG,UNITCODE) 
                                            VALUES (:ID,:MAGIAODICH,:MAGIAODICHQUAYPK,:MADONVI,:LOAIGIAODICH,:NGAYTAO,:MANGUOITAO,:NGUOITAO,:MAQUAYBAN,:NGAYPHATSINH,:HINHTHUCTHANHTOAN,:TIENKHACHDUA,:TIENVOUCHER,
                                            :TIENTHEVIP,:TIENTRALAI,:TIENTHE,:TIENCOD,:TIENMAT,:TTIENCOVAT,:THOIGIAN,:MAKHACHHANG,:UNITCODE)");
                                                    cmdOracleInsert.CommandType = CommandType.Text;
                                                    cmdOracleInsert.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = ID;
                                                    cmdOracleInsert.Parameters.Add("MAGIAODICH", OracleDbType.NVarchar2, 50)
                                                        .Value = MAGIAODICH;
                                                    cmdOracleInsert.Parameters
                                                            .Add("MAGIAODICHQUAYPK", OracleDbType.NVarchar2, 50).Value =
                                                        MAGIAODICHQUAYPK;
                                                    cmdOracleInsert.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50)
                                                        .Value = MADONVI;
                                                    cmdOracleInsert.Parameters.Add("LOAIGIAODICH", OracleDbType.Int32).Value =
                                                        LOAIGIAODICH;
                                                    cmdOracleInsert.Parameters.Add("NGAYTAO", OracleDbType.Date).Value =
                                                        NGAYTAO;
                                                    cmdOracleInsert.Parameters.Add("MANGUOITAO", OracleDbType.NVarchar2, 50)
                                                        .Value = MANGUOITAO;
                                                    cmdOracleInsert.Parameters.Add("NGUOITAO", OracleDbType.NVarchar2, 50)
                                                        .Value = NGUOITAO;
                                                    cmdOracleInsert.Parameters.Add("MAQUAYBAN", OracleDbType.NVarchar2, 50)
                                                        .Value = MAQUAYBAN;
                                                    cmdOracleInsert.Parameters.Add("NGAYPHATSINH", OracleDbType.Date).Value =
                                                        NGAYPHATSINH;
                                                    cmdOracleInsert.Parameters
                                                            .Add("HINHTHUCTHANHTOAN", OracleDbType.NVarchar2, 50).Value =
                                                        HINHTHUCTHANHTOAN;
                                                    cmdOracleInsert.Parameters.Add("TIENKHACHDUA", OracleDbType.Decimal).Value =
                                                        TIENKHACHDUA;
                                                    cmdOracleInsert.Parameters.Add("TIENVOUCHER", OracleDbType.Decimal).Value =
                                                        TIENVOUCHER;
                                                    cmdOracleInsert.Parameters.Add("TIENTHEVIP", OracleDbType.Decimal).Value =
                                                        TIENTHEVIP;
                                                    cmdOracleInsert.Parameters.Add("TIENTRALAI", OracleDbType.Decimal).Value =
                                                        TIENTRALAI;
                                                    cmdOracleInsert.Parameters.Add("TIENTHE", OracleDbType.Decimal).Value =
                                                        TIENTHE;
                                                    cmdOracleInsert.Parameters.Add("TIENCOD", OracleDbType.Decimal).Value =
                                                        TIENCOD;
                                                    cmdOracleInsert.Parameters.Add("TIENMAT", OracleDbType.Decimal).Value =
                                                        TIENMAT;
                                                    cmdOracleInsert.Parameters.Add("TTIENCOVAT", OracleDbType.Decimal).Value =
                                                        TTIENCOVAT;
                                                    cmdOracleInsert.Parameters.Add("THOIGIAN", OracleDbType.NVarchar2, 50)
                                                        .Value = THOIGIAN;
                                                    cmdOracleInsert.Parameters.Add("MAKHACHHANG", OracleDbType.NVarchar2, 50)
                                                        .Value = MAKHACHHANG;
                                                    cmdOracleInsert.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50)
                                                        .Value = UNITCODE;
                                                    int rowCount = cmdOracleInsert.ExecuteNonQuery();
                                                    if (rowCount > 0) totalParent += rowCount;
                                                }
                                            }
                                        }

                                        //bảng con
                                        SqlCommand cmdSqlDetails = new SqlCommand();
                                        cmdSqlDetails.Connection = connectionClient;
                                        cmdSqlDetails.CommandText = string.Format(
                                            @"SELECT ID,MAGDQUAYPK,MAKHOHANG,MADONVI,MAVATTU,MANGUOITAO,NGUOITAO,MABOPK,NGAYTAO,NGAYPHATSINH,SOLUONG,TTIENCOVAT,GIABANLECOVAT,TYLECHIETKHAU,TIENCHIETKHAU,TYLEKHUYENMAI,TIENKHUYENMAI,TYLEVOUCHER,TIENVOUCHER,TYLELAILE,GIAVON,MAVAT,VATBAN,MACHUONGTRINHKM,UNITCODE FROM dbo.NVHANGGDQUAY_ASYNCCLIENT WHERE UNITCODE = '" +
                                            Session.Session.CurrentUnitCode + "'");
                                        cmdSqlDetails.CommandType = CommandType.Text;
                                        SqlDataReader dataReaderClientDetails = cmdSqlDetails.ExecuteReader();
                                        if (dataReaderClientDetails.HasRows)
                                        {
                                            while (dataReaderClientDetails.Read())
                                            {
                                                OracleCommand cmdOracleSelect = new OracleCommand();
                                                cmdOracleSelect.Connection = connectionOracle;
                                                cmdOracleSelect.CommandText = string.Format(
                                                    @"SELECT ID,MAGDQUAYPK,MADONVI FROM NVHANGGDQUAY_ASYNCCLIENT WHERE ID=:ID AND MAGDQUAYPK=:MAGDQUAYPK AND MADONVI=:MADONVI");
                                                cmdOracleSelect.CommandType = CommandType.Text;
                                                cmdOracleSelect.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value =
                                                    dataReaderClientDetails["ID"].ToString();
                                                cmdOracleSelect.Parameters.Add("MAGDQUAYPK", OracleDbType.NVarchar2, 50).Value =
                                                    dataReaderClientDetails["MAGDQUAYPK"].ToString();
                                                cmdOracleSelect.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value =
                                                    Session.Session.CurrentUnitCode;
                                                OracleDataReader dataReaderOracleDetails = cmdOracleSelect.ExecuteReader();
                                                if (!dataReaderOracleDetails.HasRows)
                                                {
                                                    DateTime NGAYTAO;
                                                    DateTime NGAYPHATSINH;
                                                    decimal SOLUONG = 0;
                                                    decimal GIABANLECOVAT = 0;
                                                    decimal TYLECHIETKHAU = 0;
                                                    decimal TIENCHIETKHAU = 0;
                                                    decimal TYLEKHUYENMAI = 0;
                                                    decimal TIENKHUYENMAI = 0;
                                                    decimal TYLEVOUCHER = 0;
                                                    decimal TIENVOUCHER = 0;
                                                    decimal TYLELAILE = 0;
                                                    decimal GIAVON = 0;
                                                    decimal VATBAN = 0;
                                                    decimal TTIENCOVAT = 0;
                                                    string ID = dataReaderClientDetails["ID"] != null
                                                        ? dataReaderClientDetails["ID"].ToString()
                                                        : Guid.NewGuid().ToString();
                                                    string MAGDQUAYPK = dataReaderClientDetails["MAGDQUAYPK"] != null
                                                        ? dataReaderClientDetails["MAGDQUAYPK"].ToString()
                                                        : "";
                                                    string MAKHOHANG = dataReaderClientDetails["MAKHOHANG"] != null
                                                        ? dataReaderClientDetails["MAKHOHANG"].ToString()
                                                        : "";
                                                    string MADONVI = dataReaderClientDetails["MADONVI"] != null
                                                        ? dataReaderClientDetails["MADONVI"].ToString()
                                                        : "";
                                                    string MAVATTU = dataReaderClientDetails["MAVATTU"] != null
                                                        ? dataReaderClientDetails["MAVATTU"].ToString()
                                                        : "";
                                                    string MANGUOITAO = dataReaderClientDetails["MANGUOITAO"] != null
                                                        ? dataReaderClientDetails["MANGUOITAO"].ToString()
                                                        : "";
                                                    string NGUOITAO = dataReaderClientDetails["NGUOITAO"] != null
                                                        ? dataReaderClientDetails["NGUOITAO"].ToString()
                                                        : "";
                                                    string MABOPK = dataReaderClientDetails["MABOPK"] != null
                                                        ? dataReaderClientDetails["MABOPK"].ToString()
                                                        : "";
                                                    DateTime.TryParse(dataReaderClientDetails["NGAYTAO"] != null
                                                            ? dataReaderClientDetails["NGAYTAO"].ToString()
                                                            : "", out NGAYTAO);
                                                    DateTime.TryParse(
                                                        dataReaderClientDetails["NGAYPHATSINH"] != null
                                                            ? dataReaderClientDetails["NGAYPHATSINH"].ToString()
                                                            : "", out NGAYPHATSINH);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["SOLUONG"] != null
                                                            ? dataReaderClientDetails["SOLUONG"].ToString()
                                                            : "", out SOLUONG);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TTIENCOVAT"] != null
                                                            ? dataReaderClientDetails["TTIENCOVAT"].ToString()
                                                            : "", out TTIENCOVAT);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["GIABANLECOVAT"] != null
                                                            ? dataReaderClientDetails["GIABANLECOVAT"].ToString()
                                                            : "", out GIABANLECOVAT);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TYLECHIETKHAU"] != null
                                                            ? dataReaderClientDetails["TYLECHIETKHAU"].ToString()
                                                            : "", out TYLECHIETKHAU);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TIENCHIETKHAU"] != null
                                                            ? dataReaderClientDetails["TIENCHIETKHAU"].ToString()
                                                            : "", out TIENCHIETKHAU);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TYLEKHUYENMAI"] != null
                                                            ? dataReaderClientDetails["TYLEKHUYENMAI"].ToString()
                                                            : "", out TYLEKHUYENMAI);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TIENKHUYENMAI"] != null
                                                            ? dataReaderClientDetails["TIENKHUYENMAI"].ToString()
                                                            : "", out TIENKHUYENMAI);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TYLEVOUCHER"] != null
                                                            ? dataReaderClientDetails["TYLEVOUCHER"].ToString()
                                                            : "", out TYLEVOUCHER);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TIENVOUCHER"] != null
                                                            ? dataReaderClientDetails["TIENVOUCHER"].ToString()
                                                            : "", out TIENVOUCHER);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TYLELAILE"] != null
                                                            ? dataReaderClientDetails["TYLELAILE"].ToString()
                                                            : "", out TYLELAILE);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["GIAVON"] != null
                                                            ? dataReaderClientDetails["GIAVON"].ToString()
                                                            : "", out GIAVON);
                                                    string MAVAT = dataReaderClientDetails["MAVAT"] != null
                                                        ? dataReaderClientDetails["MAVAT"].ToString()
                                                        : "";
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["VATBAN"] != null
                                                            ? dataReaderClientDetails["VATBAN"].ToString()
                                                            : "", out VATBAN);
                                                    string MACHUONGTRINHKM = dataReaderClientDetails["MACHUONGTRINHKM"] != null
                                                        ? dataReaderClientDetails["MACHUONGTRINHKM"].ToString()
                                                        : "";
                                                    string UNITCODE = dataReaderClientDetails["UNITCODE"] != null
                                                        ? dataReaderClientDetails["UNITCODE"].ToString()
                                                        : "";
                                                    OracleCommand cmdOracleInsert = new OracleCommand();
                                                    cmdOracleInsert.Connection = connectionOracle;
                                                    cmdOracleInsert.CommandText = string.Format(@"INSERT INTO NVHANGGDQUAY_ASYNCCLIENT(ID,MAGDQUAYPK,MAKHOHANG,MADONVI,MAVATTU,NGUOITAO,
                                            MABOPK,NGAYTAO,NGAYPHATSINH,SOLUONG,TTIENCOVAT,GIABANLECOVAT,TYLECHIETKHAU,TIENCHIETKHAU,TYLEKHUYENMAI,TIENKHUYENMAI,TYLEVOUCHER,TIENVOUCHER,TYLELAILE,GIAVON,MAVAT,VATBAN,MACHUONGTRINHKM) 
                                            VALUES (:ID,:MAGDQUAYPK,:MAKHOHANG,:MADONVI,:MAVATTU,:NGUOITAO,:MABOPK,:NGAYTAO,:NGAYPHATSINH,:SOLUONG,:TTIENCOVAT,:GIABANLECOVAT,:TYLECHIETKHAU,
                                            :TIENCHIETKHAU,:TYLEKHUYENMAI,:TIENKHUYENMAI,:TYLEVOUCHER,:TIENVOUCHER,:TYLELAILE,:GIAVON,:MAVAT,:VATBAN,:MACHUONGTRINHKM)");
                                                    cmdOracleInsert.CommandType = CommandType.Text;
                                                    cmdOracleInsert.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = ID;
                                                    cmdOracleInsert.Parameters.Add("MAGDQUAYPK", OracleDbType.NVarchar2, 50)
                                                        .Value = MAGDQUAYPK;
                                                    cmdOracleInsert.Parameters.Add("MAKHOHANG", OracleDbType.NVarchar2, 50)
                                                        .Value = MAKHOHANG;
                                                    cmdOracleInsert.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50)
                                                        .Value = MADONVI;
                                                    cmdOracleInsert.Parameters.Add("MAVATTU", OracleDbType.NVarchar2, 50)
                                                        .Value = MAVATTU;
                                                    cmdOracleInsert.Parameters.Add("NGUOITAO", OracleDbType.NVarchar2, 50)
                                                        .Value = NGUOITAO;
                                                    cmdOracleInsert.Parameters.Add("MABOPK", OracleDbType.NVarchar2, 50).Value =
                                                        MABOPK;
                                                    cmdOracleInsert.Parameters.Add("NGAYTAO", OracleDbType.Date).Value =
                                                        NGAYTAO;
                                                    cmdOracleInsert.Parameters.Add("NGAYPHATSINH", OracleDbType.Date).Value =
                                                        NGAYPHATSINH;
                                                    cmdOracleInsert.Parameters.Add("SOLUONG", OracleDbType.Decimal).Value =
                                                        SOLUONG;
                                                    cmdOracleInsert.Parameters.Add("TTIENCOVAT", OracleDbType.Decimal).Value =
                                                        TTIENCOVAT;
                                                    cmdOracleInsert.Parameters.Add("GIABANLECOVAT", OracleDbType.Decimal)
                                                        .Value = GIABANLECOVAT;
                                                    cmdOracleInsert.Parameters.Add("TYLECHIETKHAU", OracleDbType.Decimal)
                                                        .Value = TYLECHIETKHAU;
                                                    cmdOracleInsert.Parameters.Add("TIENCHIETKHAU", OracleDbType.Decimal)
                                                        .Value = TIENCHIETKHAU;
                                                    cmdOracleInsert.Parameters.Add("TYLEKHUYENMAI", OracleDbType.Decimal)
                                                        .Value = TYLEKHUYENMAI;
                                                    cmdOracleInsert.Parameters.Add("TIENKHUYENMAI", OracleDbType.Decimal)
                                                        .Value = TIENKHUYENMAI;
                                                    cmdOracleInsert.Parameters.Add("TYLEVOUCHER", OracleDbType.Decimal).Value =
                                                        TYLEVOUCHER;
                                                    cmdOracleInsert.Parameters.Add("TIENVOUCHER", OracleDbType.Decimal).Value =
                                                        TIENVOUCHER;
                                                    cmdOracleInsert.Parameters.Add("TYLELAILE", OracleDbType.Decimal).Value =
                                                        TYLELAILE;
                                                    cmdOracleInsert.Parameters.Add("GIAVON", OracleDbType.Decimal).Value =
                                                        GIAVON;
                                                    cmdOracleInsert.Parameters.Add("MAVAT", OracleDbType.NVarchar2, 50).Value =
                                                        MAVAT;
                                                    cmdOracleInsert.Parameters.Add("VATBAN", OracleDbType.Decimal).Value =
                                                        VATBAN;
                                                    cmdOracleInsert.Parameters
                                                            .Add("MACHUONGTRINHKM", OracleDbType.NVarchar2, 50).Value =
                                                        MACHUONGTRINHKM;
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



        public static void GuiDuLieuBanTheoNgay(DateTime NgayBanHang)
        {
            int totalParent = 0;
            int totalChildren = 0;
            DateTime NgayBan = new DateTime(NgayBanHang.Year, NgayBanHang.Month, NgayBanHang.Day, 0, 0, 0);
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
                                        cmdSql.CommandText = @"SELECT ID,MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,NGAYTAO,NGAYPHATSINH,MANGUOITAO,NGUOITAO,MAQUAYBAN,LOAIGIAODICH,HINHTHUCTHANHTOAN,TIENKHACHDUA,TIENVOUCHER,TIENTHEVIP,TIENTRALAI,TIENTHE,TIENCOD,TIENMAT,TTIENCOVAT,THOIGIAN,MAKHACHHANG,UNITCODE FROM dbo.NVGDQUAY_ASYNCCLIENT WHERE UNITCODE = @UNITCODE AND NGAYPHATSINH = @NGAYPHATSINH ";
                                        cmdSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = Session.Session.CurrentUnitCode;
                                        cmdSql.Parameters.Add("NGAYPHATSINH", SqlDbType.Date).Value = NgayBan;
                                        cmdSql.CommandType = CommandType.Text;
                                        SqlDataReader dataReaderClient = cmdSql.ExecuteReader();
                                        if (dataReaderClient.HasRows)
                                        {
                                            while (dataReaderClient.Read())
                                            {
                                                OracleCommand cmdOracleSelect = new OracleCommand();
                                                cmdOracleSelect.Connection = connectionOracle;
                                                cmdOracleSelect.CommandText = @"SELECT ID,MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,UNITCODE FROM NVGDQUAY_ASYNCCLIENT WHERE ID = :ID AND MAGIAODICHQUAYPK = :MAGIAODICHQUAYPK AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'";
                                                cmdOracleSelect.CommandType = CommandType.Text;
                                                cmdOracleSelect.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value =
                                                    dataReaderClient["ID"].ToString();
                                                cmdOracleSelect.Parameters.Add("MAGIAODICHQUAYPK", OracleDbType.NVarchar2, 50)
                                                    .Value = dataReaderClient["MAGIAODICHQUAYPK"].ToString();
                                                OracleDataReader dataReaderOracle = cmdOracleSelect.ExecuteReader();
                                                if (!dataReaderOracle.HasRows)
                                                {
                                                    int LOAIGIAODICH = 0;
                                                    DateTime NGAYTAO;
                                                    DateTime NGAYPHATSINH;
                                                    DateTime I_CREATE_DATE = DateTime.Now;
                                                    DateTime I_UPDATE_DATE = DateTime.Now;
                                                    decimal TIENKHACHDUA = 0;
                                                    decimal TIENVOUCHER = 0;
                                                    decimal TIENTHEVIP = 0;
                                                    decimal TIENTRALAI = 0;
                                                    decimal TIENTHE = 0;
                                                    decimal TIENCOD = 0;
                                                    decimal TIENMAT = 0;
                                                    decimal TTIENCOVAT = 0;
                                                    string ID = dataReaderClient["ID"] != null
                                                        ? dataReaderClient["ID"].ToString()
                                                        : Guid.NewGuid().ToString();
                                                    string MAGIAODICH = dataReaderClient["MAGIAODICH"] != null
                                                        ? dataReaderClient["MAGIAODICH"].ToString()
                                                        : "";
                                                    string MAGIAODICHQUAYPK = dataReaderClient["MAGIAODICHQUAYPK"] != null
                                                        ? dataReaderClient["MAGIAODICHQUAYPK"].ToString()
                                                        : "";
                                                    string MADONVI = dataReaderClient["MADONVI"] != null
                                                        ? dataReaderClient["MADONVI"].ToString()
                                                        : "";
                                                    int.TryParse(
                                                        dataReaderClient["LOAIGIAODICH"] != null
                                                            ? dataReaderClient["LOAIGIAODICH"].ToString()
                                                            : "", out LOAIGIAODICH);
                                                    DateTime.TryParse(
                                                        dataReaderClient["NGAYTAO"] != null
                                                            ? dataReaderClient["NGAYTAO"].ToString()
                                                            : "", out NGAYTAO);
                                                    DateTime.TryParse(
                                                        dataReaderClient["NGAYPHATSINH"] != null
                                                            ? dataReaderClient["NGAYPHATSINH"].ToString()
                                                            : "", out NGAYPHATSINH);
                                                    string MANGUOITAO = dataReaderClient["MANGUOITAO"] != null
                                                        ? dataReaderClient["MANGUOITAO"].ToString()
                                                        : "";
                                                    string NGUOITAO = dataReaderClient["NGUOITAO"] != null
                                                        ? dataReaderClient["NGUOITAO"].ToString()
                                                        : "";
                                                    string MAQUAYBAN = dataReaderClient["MAQUAYBAN"] != null
                                                        ? dataReaderClient["MAQUAYBAN"].ToString()
                                                        : "";
                                                    string HINHTHUCTHANHTOAN = dataReaderClient["HINHTHUCTHANHTOAN"] != null
                                                        ? dataReaderClient["HINHTHUCTHANHTOAN"].ToString()
                                                        : "";
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENKHACHDUA"] != null
                                                            ? dataReaderClient["TIENKHACHDUA"].ToString()
                                                            : "", out TIENKHACHDUA);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENVOUCHER"] != null
                                                            ? dataReaderClient["TIENVOUCHER"].ToString()
                                                            : "", out TIENVOUCHER);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENTHEVIP"] != null
                                                            ? dataReaderClient["TIENTHEVIP"].ToString()
                                                            : "", out TIENTHEVIP);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENTRALAI"] != null
                                                            ? dataReaderClient["TIENTRALAI"].ToString()
                                                            : "", out TIENTRALAI);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENTHE"] != null
                                                            ? dataReaderClient["TIENTHE"].ToString()
                                                            : "", out TIENTHE);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENCOD"] != null
                                                            ? dataReaderClient["TIENCOD"].ToString()
                                                            : "", out TIENCOD);
                                                    decimal.TryParse(
                                                        dataReaderClient["TIENMAT"] != null
                                                            ? dataReaderClient["TIENMAT"].ToString()
                                                            : "", out TIENMAT);
                                                    decimal.TryParse(
                                                        dataReaderClient["TTIENCOVAT"] != null
                                                            ? dataReaderClient["TTIENCOVAT"].ToString()
                                                            : "", out TTIENCOVAT);
                                                    string THOIGIAN = dataReaderClient["THOIGIAN"] != null
                                                        ? dataReaderClient["THOIGIAN"].ToString()
                                                        : "";
                                                    string MAKHACHHANG = dataReaderClient["MAKHACHHANG"] != null
                                                        ? dataReaderClient["MAKHACHHANG"].ToString()
                                                        : "";
                                                    string UNITCODE = dataReaderClient["UNITCODE"] != null
                                                        ? dataReaderClient["UNITCODE"].ToString()
                                                        : "";
                                                    OracleCommand cmdOracleInsert = new OracleCommand();
                                                    cmdOracleInsert.Connection = connectionOracle;
                                                    cmdOracleInsert.CommandText = string.Format(
                                                        @"INSERT INTO NVGDQUAY_ASYNCCLIENT(ID,MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,LOAIGIAODICH,NGAYTAO,MANGUOITAO,
                                            NGUOITAO,MAQUAYBAN,NGAYPHATSINH,HINHTHUCTHANHTOAN,TIENKHACHDUA,TIENVOUCHER,TIENTHEVIP,TIENTRALAI,TIENTHE,TIENCOD,TIENMAT,TTIENCOVAT,THOIGIAN,MAKHACHHANG,UNITCODE) 
                                            VALUES (:ID,:MAGIAODICH,:MAGIAODICHQUAYPK,:MADONVI,:LOAIGIAODICH,:NGAYTAO,:MANGUOITAO,:NGUOITAO,:MAQUAYBAN,:NGAYPHATSINH,:HINHTHUCTHANHTOAN,:TIENKHACHDUA,:TIENVOUCHER,
                                            :TIENTHEVIP,:TIENTRALAI,:TIENTHE,:TIENCOD,:TIENMAT,:TTIENCOVAT,:THOIGIAN,:MAKHACHHANG,:UNITCODE)");
                                                    cmdOracleInsert.CommandType = CommandType.Text;
                                                    cmdOracleInsert.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = ID;
                                                    cmdOracleInsert.Parameters.Add("MAGIAODICH", OracleDbType.NVarchar2, 50)
                                                        .Value = MAGIAODICH;
                                                    cmdOracleInsert.Parameters
                                                            .Add("MAGIAODICHQUAYPK", OracleDbType.NVarchar2, 50).Value =
                                                        MAGIAODICHQUAYPK;
                                                    cmdOracleInsert.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50)
                                                        .Value = MADONVI;
                                                    cmdOracleInsert.Parameters.Add("LOAIGIAODICH", OracleDbType.Int32).Value =
                                                        LOAIGIAODICH;
                                                    cmdOracleInsert.Parameters.Add("NGAYTAO", OracleDbType.Date).Value =
                                                        NGAYTAO;
                                                    cmdOracleInsert.Parameters.Add("MANGUOITAO", OracleDbType.NVarchar2, 50)
                                                        .Value = MANGUOITAO;
                                                    cmdOracleInsert.Parameters.Add("NGUOITAO", OracleDbType.NVarchar2, 50)
                                                        .Value = NGUOITAO;
                                                    cmdOracleInsert.Parameters.Add("MAQUAYBAN", OracleDbType.NVarchar2, 50)
                                                        .Value = MAQUAYBAN;
                                                    cmdOracleInsert.Parameters.Add("NGAYPHATSINH", OracleDbType.Date).Value =
                                                        NGAYPHATSINH;
                                                    cmdOracleInsert.Parameters
                                                            .Add("HINHTHUCTHANHTOAN", OracleDbType.NVarchar2, 50).Value =
                                                        HINHTHUCTHANHTOAN;
                                                    cmdOracleInsert.Parameters.Add("TIENKHACHDUA", OracleDbType.Decimal).Value =
                                                        TIENKHACHDUA;
                                                    cmdOracleInsert.Parameters.Add("TIENVOUCHER", OracleDbType.Decimal).Value =
                                                        TIENVOUCHER;
                                                    cmdOracleInsert.Parameters.Add("TIENTHEVIP", OracleDbType.Decimal).Value =
                                                        TIENTHEVIP;
                                                    cmdOracleInsert.Parameters.Add("TIENTRALAI", OracleDbType.Decimal).Value =
                                                        TIENTRALAI;
                                                    cmdOracleInsert.Parameters.Add("TIENTHE", OracleDbType.Decimal).Value =
                                                        TIENTHE;
                                                    cmdOracleInsert.Parameters.Add("TIENCOD", OracleDbType.Decimal).Value =
                                                        TIENCOD;
                                                    cmdOracleInsert.Parameters.Add("TIENMAT", OracleDbType.Decimal).Value =
                                                        TIENMAT;
                                                    cmdOracleInsert.Parameters.Add("TTIENCOVAT", OracleDbType.Decimal).Value =
                                                        TTIENCOVAT;
                                                    cmdOracleInsert.Parameters.Add("THOIGIAN", OracleDbType.NVarchar2, 50)
                                                        .Value = THOIGIAN;
                                                    cmdOracleInsert.Parameters.Add("MAKHACHHANG", OracleDbType.NVarchar2, 50)
                                                        .Value = MAKHACHHANG;
                                                    cmdOracleInsert.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50)
                                                        .Value = UNITCODE;
                                                    int rowCount = cmdOracleInsert.ExecuteNonQuery();
                                                    if (rowCount > 0) totalParent += rowCount;
                                                }
                                            }
                                        }

                                        //bảng con
                                        SqlCommand cmdSqlDetails = new SqlCommand();
                                        cmdSqlDetails.Connection = connectionClient;
                                        cmdSqlDetails.CommandText = string.Format(@"SELECT ID,MAGDQUAYPK,MAKHOHANG,MADONVI,MAVATTU,MANGUOITAO,NGUOITAO,MABOPK,NGAYTAO,NGAYPHATSINH,SOLUONG,TTIENCOVAT,GIABANLECOVAT,TYLECHIETKHAU,TIENCHIETKHAU,TYLEKHUYENMAI,TIENKHUYENMAI,TYLEVOUCHER,TIENVOUCHER,TYLELAILE,GIAVON,MAVAT,VATBAN,MACHUONGTRINHKM,UNITCODE FROM dbo.NVHANGGDQUAY_ASYNCCLIENT WHERE UNITCODE=@UNITCODE AND NGAYPHATSINH=@NGAYPHATSINH");
                                        cmdSqlDetails.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = Session.Session.CurrentUnitCode;
                                        cmdSqlDetails.Parameters.Add("NGAYPHATSINH", SqlDbType.Date).Value = NgayBan;
                                        cmdSqlDetails.CommandType = CommandType.Text;
                                        SqlDataReader dataReaderClientDetails = cmdSqlDetails.ExecuteReader();
                                        if (dataReaderClientDetails.HasRows)
                                        {
                                            while (dataReaderClientDetails.Read())
                                            {
                                                OracleCommand cmdOracleSelect = new OracleCommand();
                                                cmdOracleSelect.Connection = connectionOracle;
                                                cmdOracleSelect.CommandText = string.Format(@"SELECT ID,MAGDQUAYPK,MADONVI FROM NVHANGGDQUAY_ASYNCCLIENT WHERE ID=:ID AND MAGDQUAYPK=:MAGDQUAYPK AND MADONVI=:MADONVI");
                                                cmdOracleSelect.CommandType = CommandType.Text;
                                                cmdOracleSelect.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value =
                                                    dataReaderClientDetails["ID"].ToString();
                                                cmdOracleSelect.Parameters.Add("MAGDQUAYPK", OracleDbType.NVarchar2, 50).Value =
                                                    dataReaderClientDetails["MAGDQUAYPK"].ToString();
                                                cmdOracleSelect.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value =
                                                    Session.Session.CurrentUnitCode;
                                                OracleDataReader dataReaderOracleDetails = cmdOracleSelect.ExecuteReader();
                                                if (!dataReaderOracleDetails.HasRows)
                                                {
                                                    DateTime NGAYTAO;
                                                    DateTime NGAYPHATSINH;
                                                    decimal SOLUONG = 0;
                                                    decimal GIABANLECOVAT = 0;
                                                    decimal TYLECHIETKHAU = 0;
                                                    decimal TIENCHIETKHAU = 0;
                                                    decimal TYLEKHUYENMAI = 0;
                                                    decimal TIENKHUYENMAI = 0;
                                                    decimal TYLEVOUCHER = 0;
                                                    decimal TIENVOUCHER = 0;
                                                    decimal TYLELAILE = 0;
                                                    decimal GIAVON = 0;
                                                    decimal VATBAN = 0;
                                                    decimal TTIENCOVAT = 0;
                                                    string ID = dataReaderClientDetails["ID"] != null
                                                        ? dataReaderClientDetails["ID"].ToString()
                                                        : Guid.NewGuid().ToString();
                                                    string MAGDQUAYPK = dataReaderClientDetails["MAGDQUAYPK"] != null
                                                        ? dataReaderClientDetails["MAGDQUAYPK"].ToString()
                                                        : "";
                                                    string MAKHOHANG = dataReaderClientDetails["MAKHOHANG"] != null
                                                        ? dataReaderClientDetails["MAKHOHANG"].ToString(): "";
                                                    string MADONVI = dataReaderClientDetails["MADONVI"] != null
                                                        ? dataReaderClientDetails["MADONVI"].ToString()
                                                        : "";
                                                    string MAVATTU = dataReaderClientDetails["MAVATTU"] != null
                                                        ? dataReaderClientDetails["MAVATTU"].ToString()
                                                        : "";
                                                    string MANGUOITAO = dataReaderClientDetails["MANGUOITAO"] != null
                                                        ? dataReaderClientDetails["MANGUOITAO"].ToString()
                                                        : "";
                                                    string NGUOITAO = dataReaderClientDetails["NGUOITAO"] != null
                                                        ? dataReaderClientDetails["NGUOITAO"].ToString()
                                                        : "";
                                                    string MABOPK = dataReaderClientDetails["MABOPK"] != null
                                                        ? dataReaderClientDetails["MABOPK"].ToString()
                                                        : "";
                                                    DateTime.TryParse(dataReaderClientDetails["NGAYTAO"] != null
                                                            ? dataReaderClientDetails["NGAYTAO"].ToString()
                                                            : "", out NGAYTAO);
                                                    DateTime.TryParse(
                                                        dataReaderClientDetails["NGAYPHATSINH"] != null
                                                            ? dataReaderClientDetails["NGAYPHATSINH"].ToString()
                                                            : "", out NGAYPHATSINH);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["SOLUONG"] != null
                                                            ? dataReaderClientDetails["SOLUONG"].ToString()
                                                            : "", out SOLUONG);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TTIENCOVAT"] != null
                                                            ? dataReaderClientDetails["TTIENCOVAT"].ToString()
                                                            : "", out TTIENCOVAT);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["GIABANLECOVAT"] != null
                                                            ? dataReaderClientDetails["GIABANLECOVAT"].ToString()
                                                            : "", out GIABANLECOVAT);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TYLECHIETKHAU"] != null
                                                            ? dataReaderClientDetails["TYLECHIETKHAU"].ToString()
                                                            : "", out TYLECHIETKHAU);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TIENCHIETKHAU"] != null
                                                            ? dataReaderClientDetails["TIENCHIETKHAU"].ToString()
                                                            : "", out TIENCHIETKHAU);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TYLEKHUYENMAI"] != null
                                                            ? dataReaderClientDetails["TYLEKHUYENMAI"].ToString()
                                                            : "", out TYLEKHUYENMAI);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TIENKHUYENMAI"] != null
                                                            ? dataReaderClientDetails["TIENKHUYENMAI"].ToString()
                                                            : "", out TIENKHUYENMAI);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TYLEVOUCHER"] != null
                                                            ? dataReaderClientDetails["TYLEVOUCHER"].ToString()
                                                            : "", out TYLEVOUCHER);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TIENVOUCHER"] != null
                                                            ? dataReaderClientDetails["TIENVOUCHER"].ToString()
                                                            : "", out TIENVOUCHER);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["TYLELAILE"] != null
                                                            ? dataReaderClientDetails["TYLELAILE"].ToString()
                                                            : "", out TYLELAILE);
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["GIAVON"] != null
                                                            ? dataReaderClientDetails["GIAVON"].ToString()
                                                            : "", out GIAVON);
                                                    string MAVAT = dataReaderClientDetails["MAVAT"] != null
                                                        ? dataReaderClientDetails["MAVAT"].ToString()
                                                        : "";
                                                    decimal.TryParse(
                                                        dataReaderClientDetails["VATBAN"] != null
                                                            ? dataReaderClientDetails["VATBAN"].ToString()
                                                            : "", out VATBAN);
                                                    string MACHUONGTRINHKM = dataReaderClientDetails["MACHUONGTRINHKM"] != null
                                                        ? dataReaderClientDetails["MACHUONGTRINHKM"].ToString()
                                                        : "";
                                                    string UNITCODE = dataReaderClientDetails["UNITCODE"] != null
                                                        ? dataReaderClientDetails["UNITCODE"].ToString()
                                                        : "";
                                                    OracleCommand cmdOracleInsert = new OracleCommand();
                                                    cmdOracleInsert.Connection = connectionOracle;
                                                    cmdOracleInsert.CommandText = string.Format(@"INSERT INTO NVHANGGDQUAY_ASYNCCLIENT(ID,MAGDQUAYPK,MAKHOHANG,MADONVI,MAVATTU,NGUOITAO,
                                            MABOPK,NGAYTAO,NGAYPHATSINH,SOLUONG,TTIENCOVAT,GIABANLECOVAT,TYLECHIETKHAU,TIENCHIETKHAU,TYLEKHUYENMAI,TIENKHUYENMAI,TYLEVOUCHER,TIENVOUCHER,TYLELAILE,GIAVON,MAVAT,VATBAN,MACHUONGTRINHKM) 
                                            VALUES (:ID,:MAGDQUAYPK,:MAKHOHANG,:MADONVI,:MAVATTU,:NGUOITAO,:MABOPK,:NGAYTAO,:NGAYPHATSINH,:SOLUONG,:TTIENCOVAT,:GIABANLECOVAT,:TYLECHIETKHAU,
                                            :TIENCHIETKHAU,:TYLEKHUYENMAI,:TIENKHUYENMAI,:TYLEVOUCHER,:TIENVOUCHER,:TYLELAILE,:GIAVON,:MAVAT,:VATBAN,:MACHUONGTRINHKM)");
                                                    cmdOracleInsert.CommandType = CommandType.Text;
                                                    cmdOracleInsert.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = ID;
                                                    cmdOracleInsert.Parameters.Add("MAGDQUAYPK", OracleDbType.NVarchar2, 50)
                                                        .Value = MAGDQUAYPK;
                                                    cmdOracleInsert.Parameters.Add("MAKHOHANG", OracleDbType.NVarchar2, 50)
                                                        .Value = MAKHOHANG;
                                                    cmdOracleInsert.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50)
                                                        .Value = MADONVI;
                                                    cmdOracleInsert.Parameters.Add("MAVATTU", OracleDbType.NVarchar2, 50)
                                                        .Value = MAVATTU;
                                                    cmdOracleInsert.Parameters.Add("NGUOITAO", OracleDbType.NVarchar2, 50)
                                                        .Value = NGUOITAO;
                                                    cmdOracleInsert.Parameters.Add("MABOPK", OracleDbType.NVarchar2, 50).Value =
                                                        MABOPK;
                                                    cmdOracleInsert.Parameters.Add("NGAYTAO", OracleDbType.Date).Value =
                                                        NGAYTAO;
                                                    cmdOracleInsert.Parameters.Add("NGAYPHATSINH", OracleDbType.Date).Value =
                                                        NGAYPHATSINH;
                                                    cmdOracleInsert.Parameters.Add("SOLUONG", OracleDbType.Decimal).Value =
                                                        SOLUONG;
                                                    cmdOracleInsert.Parameters.Add("TTIENCOVAT", OracleDbType.Decimal).Value =
                                                        TTIENCOVAT;
                                                    cmdOracleInsert.Parameters.Add("GIABANLECOVAT", OracleDbType.Decimal)
                                                        .Value = GIABANLECOVAT;
                                                    cmdOracleInsert.Parameters.Add("TYLECHIETKHAU", OracleDbType.Decimal)
                                                        .Value = TYLECHIETKHAU;
                                                    cmdOracleInsert.Parameters.Add("TIENCHIETKHAU", OracleDbType.Decimal)
                                                        .Value = TIENCHIETKHAU;
                                                    cmdOracleInsert.Parameters.Add("TYLEKHUYENMAI", OracleDbType.Decimal)
                                                        .Value = TYLEKHUYENMAI;
                                                    cmdOracleInsert.Parameters.Add("TIENKHUYENMAI", OracleDbType.Decimal)
                                                        .Value = TIENKHUYENMAI;
                                                    cmdOracleInsert.Parameters.Add("TYLEVOUCHER", OracleDbType.Decimal).Value =
                                                        TYLEVOUCHER;
                                                    cmdOracleInsert.Parameters.Add("TIENVOUCHER", OracleDbType.Decimal).Value =
                                                        TIENVOUCHER;
                                                    cmdOracleInsert.Parameters.Add("TYLELAILE", OracleDbType.Decimal).Value =
                                                        TYLELAILE;
                                                    cmdOracleInsert.Parameters.Add("GIAVON", OracleDbType.Decimal).Value =
                                                        GIAVON;
                                                    cmdOracleInsert.Parameters.Add("MAVAT", OracleDbType.NVarchar2, 50).Value =
                                                        MAVAT;
                                                    cmdOracleInsert.Parameters.Add("VATBAN", OracleDbType.Decimal).Value =
                                                        VATBAN;
                                                    cmdOracleInsert.Parameters
                                                            .Add("MACHUONGTRINHKM", OracleDbType.NVarchar2, 50).Value =
                                                        MACHUONGTRINHKM;
                                                    int rowCount = cmdOracleInsert.ExecuteNonQuery();
                                                    if (rowCount > 0) totalChildren += rowCount;
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    NotificationLauncher.ShowNotificationError("Thông báo", "Xảy ra lỗi", 1, "0x1", "0x8", "normal");
                                }
                                finally
                                {
                                    connectionClient.Close();
                                }
                            }
                        }
                        catch
                        {
                            NotificationLauncher.ShowNotificationError("Thông báo", "Xảy ra lỗi", 1, "0x1", "0x8", "normal");
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
