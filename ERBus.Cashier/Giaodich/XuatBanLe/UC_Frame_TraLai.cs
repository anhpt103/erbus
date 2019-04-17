using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using ERBus.Cashier.Dto;
using ERBus.Cashier.Common;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public delegate void XuLyThanhToanBanTraLai(bool statusBanTraLai);
    public delegate void Status_TimKiem(string MaGiaoDichQuay, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc);
    public partial class UC_Frame_TraLai : UserControl
    {
        int currentRowDgvTraLai = -1;
        private int VALUE_SELECTED_CHANGE = 0;
        public UC_Frame_TraLai()
        {
            InitializeComponent();
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã giao dịch" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Số tiền" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Thu ngân tạo" });
            cboDieuKienChon.DataSource = _comboItems;
            cboDieuKienChon.DisplayMember = "TEXT";
            cboDieuKienChon.ValueMember = "VALUE";
            //default combobox
            cboDieuKienChon.SelectedIndex = VALUE_SELECTED_CHANGE;
            dateTimeDenNgay.Format = DateTimePickerFormat.Custom;
            dateTimeDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeDenNgay.Value = DateTime.Now;
            dateTimeTuNgay.Format = DateTimePickerFormat.Custom;
            dateTimeTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeTuNgay.Value = DateTime.Now.AddDays(-7);
            txtTraLai_ThuNgan.Text = Session.Session.CurrentTenNhanVien;
            txtTraLai_MaVatTu.SelectAll();
            txtTraLai_SoLuong.SelectAll();
            txtKeySearch.Enabled = false;
            dateTimeTuNgay.Enabled = false;
            dateTimeDenNgay.Enabled = false;
            btnTimKiem.Enabled = false;
            btnF1ThanhToanTraLai.Enabled = false;
            btnF5LamMoiGiaoDich.Enabled = false;
            btnF3GiamHang.Enabled = false;
            btnF4TangHang.Enabled = false;
            btnF2ThemMoiTraLai.Enabled = true;
        }

        //TÌM KIẾM GIAO DỊCH TRƯỜNG HỢP CÓ MẠNG
        public static List<GIAODICH_DTO> TIMKIEM_GIAODICHQUAY(string KeySearch, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc)
        {
            GIAODICH_DTO GDQUAY_DTO = new GIAODICH_DTO();
            List<GIAODICH_DTO> listReturn = new List<GIAODICH_DTO>();
            try
            {
                using (
                    OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString)
                    )
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = connection;
                            cmd.InitialLONGFetchSize = 1000;
                            cmd.CommandText = "TIMKIEM_GIAODICHQUAY";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("P_KEYSEARCH", OracleDbType.Varchar2).Value = KeySearch;
                            cmd.Parameters.Add("P_TUNGAY", OracleDbType.Date).Value = TuNgay;
                            cmd.Parameters.Add("P_DENNGAY", OracleDbType.Date).Value = DenNgay;
                            cmd.Parameters.Add("P_DIEUKIENLOC", OracleDbType.Int32).Value = DieuKienLoc;
                            cmd.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2).Value = Session.Session.CurrentUnitCode;
                            cmd.Parameters.Add("CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                            OracleDataReader dataReader = null;
                            dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {

                                while (dataReader.Read())
                                {
                                
                                    decimal TIENKHACH_TRA, TIENTHE, TIEN_TRALAI_KHACH, THANHTIEN = 0;
                                    GDQUAY_DTO.MA_GIAODICH = dataReader["MA_GIAODICH"].ToString();
                                    GDQUAY_DTO.LOAI_GIAODICH = dataReader["LOAI_GIAODICH"].ToString();
                                    GDQUAY_DTO.I_CREATE_DATE = DateTime.Parse(dataReader["I_CREATE_DATE"].ToString());
                                    GDQUAY_DTO.I_CREATE_BY = dataReader["I_CREATE_BY"].ToString();
                                    GDQUAY_DTO.NGAY_GIAODICH = DateTime.Parse(dataReader["NGAY_GIAODICH"].ToString());
                                    GDQUAY_DTO.MA_VOUCHER = dataReader["MA_VOUCHER"].ToString();
                                    decimal.TryParse(dataReader["TIENKHACH_TRA"].ToString(), out TIENKHACH_TRA);
                                    GDQUAY_DTO.TIENKHACH_TRA = TIENKHACH_TRA;
                                    decimal.TryParse(dataReader["TIEN_TRALAI_KHACH"].ToString(), out TIEN_TRALAI_KHACH);
                                    GDQUAY_DTO.TIEN_TRALAI_KHACH = TIEN_TRALAI_KHACH;
                                    decimal.TryParse(dataReader["TIENTHE"].ToString(), out TIENTHE);
                                    GDQUAY_DTO.TIENTHE = TIENTHE;
                                    decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIEN);
                                    GDQUAY_DTO.THANHTIEN = THANHTIEN;
                                    GDQUAY_DTO.THOIGIAN_TAO = dataReader["THOIGIAN_TAO"].ToString();
                                    GDQUAY_DTO.MAKHACHHANG = dataReader["MAKHACHHANG"].ToString();
                                    GDQUAY_DTO.UNITCODE = dataReader["UNITCODE"].ToString();
                                    listReturn.Add(GDQUAY_DTO);
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("Thông báo", "KHÔNG TÌM THẤY MÃ GIAO DỊCH", 1, "0x1", "0x8", "normal");
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
                WriteLogs.LogError(ex);
                NotificationLauncher.ShowNotificationError("Thông báo", "Xảy ra lỗi", 1, "0x1", "0x8", "normal");
            }
            return listReturn;
        }
        //TÌM KIẾM GIAO DỊCH TRƯỜNG HỢP MẤT MẠNG
        public static List<GIAODICH_DTO> TIMKIEM_GIAODICHQUAY_FROM_SQL(string KeySearch, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc)
        {
            GIAODICH_DTO GDQUAY_DTO = new GIAODICH_DTO();
            List<GIAODICH_DTO> listReturn = new List<GIAODICH_DTO>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT [MA_GIAODICH],[MAGIAODICHQUAYPK],[MADONVI],[I_CREATE_BY],[NGAYPHATSINH],[I_CREATE_BY],[NGUOITAO],[MAQUAYBAN],[LOAI_GIAODICH],[HINHTHUCTHANHTOAN],[TIENKHACH_TRA],[TIEN_VOUCHER],[TIENTHE],[TIEN_TRALAI_KHACH],[TIENTHE],[TIENCOD],[TIENMAT],[THANHTIEN],[THOIGIAN_TAO],[MAKHACHHANG],[UNITCODE] FROM [dbo].[NVGDQUAY_ASYNCCLIENT]";
                        SqlDataReader dataReader = null;
                        dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {

                            while (dataReader.Read())
                            {
                                decimal TIENKHACH_TRA, TIENTHE, TIEN_TRALAI_KHACH, THANHTIEN = 0;
                                GDQUAY_DTO.MA_GIAODICH = dataReader["MA_GIAODICH"].ToString();
                                GDQUAY_DTO.LOAI_GIAODICH = dataReader["LOAI_GIAODICH"].ToString();
                                GDQUAY_DTO.I_CREATE_DATE = dataReader["I_CREATE_DATE"] != DBNull.Value ? DateTime.Parse(dataReader["I_CREATE_DATE"].ToString()) : DateTime.Now;
                                GDQUAY_DTO.I_CREATE_BY = dataReader["I_CREATE_BY"].ToString();
                                GDQUAY_DTO.NGAY_GIAODICH = DateTime.Parse(dataReader["NGAY_GIAODICH"].ToString());
                                decimal.TryParse(dataReader["TIENKHACH_TRA"].ToString(), out TIENKHACH_TRA);
                                GDQUAY_DTO.TIENKHACH_TRA = TIENKHACH_TRA;
                                decimal.TryParse(dataReader["TIENTHE"].ToString(), out TIENTHE);
                                GDQUAY_DTO.TIENTHE = TIENTHE;
                                decimal.TryParse(dataReader["TIEN_TRALAI_KHACH"].ToString(), out TIEN_TRALAI_KHACH);
                                GDQUAY_DTO.TIEN_TRALAI_KHACH = TIEN_TRALAI_KHACH;
                                decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIEN);
                                GDQUAY_DTO.THANHTIEN = THANHTIEN;
                                GDQUAY_DTO.THOIGIAN_TAO = dataReader["THOIGIAN_TAO"].ToString();
                                GDQUAY_DTO.MAKHACHHANG = dataReader["MAKHACHHANG"].ToString();
                                GDQUAY_DTO.UNITCODE = dataReader["UNITCODE"].ToString();
                                listReturn.Add(GDQUAY_DTO);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return listReturn;
        }
        public static IEnumerable<DateTime> EachDay(DateTime fromDate, DateTime toDate)
        {
            for (var day = fromDate.Date; day.Date <= toDate.Date; day = day.AddDays(1))
                yield return day;
        }

        public static GIAODICH_DTO SEARCH_BY_CODE_PAY_FROM_SQLSERVER(string MA_GIAODICH)
        {
            //Kết nối với SQL
            GIAODICH_DTO GDQUAY_DTO = new GIAODICH_DTO();
            string MaGiaoDichQuayPk = MA_GIAODICH.Trim() + "." + Session.Session.CurrentUnitCode.Split('-')[1];
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmdParent = new SqlCommand();
                        cmdParent.Connection = connection;
                        cmdParent.CommandText = string.Format(@"SELECT [MA_GIAODICH],[MAGIAODICHQUAYPK],[MADONVI],[I_CREATE_BY],[NGAYPHATSINH],[I_CREATE_BY],[NGUOITAO],[MAQUAYBAN],[LOAI_GIAODICH],[HINHTHUCTHANHTOAN],[TIENKHACH_TRA],[TIEN_VOUCHER],[TIENTHE],[TIEN_TRALAI_KHACH],[TIENTHE],[TIENCOD],[TIENMAT],[THANHTIEN],[THOIGIAN_TAO],[MAKHACHHANG],[UNITCODE] FROM [dbo].[NVGDQUAY_ASYNCCLIENT] WHERE MAGIAODICHQUAYPK = @MAGIAODICHQUAYPK AND UNITCODE = @UNITCODE");
                        cmdParent.CommandType = CommandType.Text;
                        cmdParent.Parameters.Add("MAGIAODICHQUAYPK", SqlDbType.NVarChar, 50).Value = MaGiaoDichQuayPk.Trim();
                        cmdParent.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                        SqlDataReader dataReaderParent = null;
                        dataReaderParent = cmdParent.ExecuteReader();
                        if (dataReaderParent.HasRows)
                        {
                            decimal TIENKHACH_TRA, TIENTHE, TIEN_TRALAI_KHACH, THANHTIEN = 0;
                            while (dataReaderParent.Read())
                            {
                                GDQUAY_DTO.LOAI_GIAODICH = dataReaderParent["LOAI_GIAODICH"].ToString();
                                GDQUAY_DTO.MA_GIAODICH = dataReaderParent["MA_GIAODICH"].ToString().Trim();
                                GDQUAY_DTO.I_CREATE_DATE = DateTime.Parse(dataReaderParent["I_CREATE_DATE"].ToString());
                                GDQUAY_DTO.I_CREATE_BY = dataReaderParent["I_CREATE_BY"].ToString();
                                GDQUAY_DTO.NGAY_GIAODICH = DateTime.Parse(dataReaderParent["NGAY_GIAODICH"].ToString());
                                decimal.TryParse(dataReaderParent["TIENKHACH_TRA"].ToString(), out TIENKHACH_TRA);
                                GDQUAY_DTO.TIENKHACH_TRA = TIENKHACH_TRA;
                                decimal.TryParse(dataReaderParent["TIENTHE"].ToString(), out TIENTHE);
                                GDQUAY_DTO.TIENTHE = TIENTHE;
                                decimal.TryParse(dataReaderParent["TIEN_TRALAI_KHACH"].ToString(), out TIEN_TRALAI_KHACH);
                                GDQUAY_DTO.TIEN_TRALAI_KHACH = TIEN_TRALAI_KHACH;
                                decimal.TryParse(dataReaderParent["THANHTIEN"].ToString(), out THANHTIEN);
                                GDQUAY_DTO.THANHTIEN = THANHTIEN;
                                GDQUAY_DTO.THOIGIAN_TAO = dataReaderParent["THOIGIAN_TAO"].ToString();
                                GDQUAY_DTO.MAKHACHHANG = dataReaderParent["MAKHACHHANG"].ToString();
                                GDQUAY_DTO.UNITCODE = dataReaderParent["UNITCODE"].ToString();
                            }
                        }

                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT [MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAHANG],[I_CREATE_BY],[NGUOITAO],[MABOPK],[I_CREATE_BY],[NGAYPHATSINH] ,[SOLUONG],[THANHTIEN],[GIABANLE_VAT],[TYLE_CHIETKHAU]
                        ,[TIEN_CHIETKHAU],[TYLE_KHUYENMAI],[TIEN_KHUYENMAI],[TIEN_VOUCHER],[TIEN_VOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[VATBAN],[MA_KHUYENMAI],[UNITCODE] FROM [dbo].[GIAODICH_CHITIET] WHERE MAGDQUAYPK = @MAGDQUAYPK AND MADONVI = @MADONVI");
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("MA_GIAODICH", SqlDbType.NVarChar, 50).Value = GDQUAY_DTO.MA_GIAODICH;
                        cmd.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = GDQUAY_DTO.UNITCODE;
                        SqlDataReader dataReader = dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            decimal giaVon, giaBanLeVAT, GIATRI_THUE_RA, TIEN_CHIETKHAU, TYLE_CHIETKHAU, TYLE_KHUYENMAI, TIEN_KHUYENMAI, TIEN_VOUCHER, TYLELAILE, GIAVON, THANHTIEN = 0;
                            decimal soLuong = 0;
                            int i = 0;
                            List<BOHANG_DTO> listBoHang = new List<BOHANG_DTO>();
                            while (dataReader.Read())
                            {
                                GIAODICH_CHITIET item = new GIAODICH_CHITIET();
                                string MaBoHangPk = dataReader["MABOPK"].ToString().Trim();
                                if (!string.IsNullOrEmpty(MaBoHangPk) && !MaBoHangPk.Equals("BH"))
                                {
                                    decimal SOLUONG, GIABANLE_VAT, THANHTIENCOVAT = 0;
                                    string MAHANG = dataReader["MAHANG"].ToString().ToUpper().Trim();
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                    decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                    decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIENCOVAT);
                                    List<BOHANG_DETAILS_DTO> listBoHangMatHangExist = new List<BOHANG_DETAILS_DTO>();
                                    BOHANG_DTO boHangExist = listBoHang.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk));
                                    if (boHangExist == null)
                                    {
                                        BOHANG_DTO boHang = new BOHANG_DTO();
                                        boHang.MABOHANG = MaBoHangPk;
                                        boHang.THANHTIEN = THANHTIENCOVAT;
                                        boHang.TONGSL = SOLUONG;
                                        BOHANG_DETAILS_DTO MatHangExist = listBoHangMatHangExist.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAHANG.Equals(MAHANG));
                                        if (MatHangExist == null)
                                        {
                                            BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                            mathang.MABOHANG = MaBoHangPk;
                                            mathang.MAHANG = MAHANG;
                                            mathang.SOLUONG = SOLUONG;
                                            mathang.GIABANLE_VAT = GIABANLE_VAT;
                                            mathang.THANHTIEN = THANHTIENCOVAT;
                                            boHang.MATHANG_BOHANG.Add(mathang);
                                        }
                                        listBoHang.Add(boHang);
                                    }
                                    else
                                    {
                                        boHangExist.THANHTIEN += THANHTIENCOVAT;
                                        boHangExist.TONGSL += SOLUONG;
                                        BOHANG_DETAILS_DTO MatHangExist = boHangExist.MATHANG_BOHANG.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAHANG.Equals(MAHANG));
                                        if (MatHangExist == null)
                                        {
                                            BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                            mathang.MABOHANG = MaBoHangPk;
                                            mathang.MAHANG = MAHANG;
                                            mathang.SOLUONG = SOLUONG;
                                            mathang.GIABANLE_VAT = GIABANLE_VAT;
                                            mathang.THANHTIEN = THANHTIENCOVAT;
                                            boHangExist.MATHANG_BOHANG.Add(mathang);
                                        }
                                    }
                                }
                                else
                                {
                                    item.MAHANG = dataReader["MAHANG"].ToString();
                                    EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
                                    _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(item.MAHANG,GDQUAY_DTO.UNITCODE);
                                    item.TENHANG = _EXTEND_VATTU_DTO.TENHANG;
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out soLuong);
                                    item.SOLUONG = soLuong;
                                    decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out giaBanLeVAT);
                                    item.GIABANLE_VAT = giaBanLeVAT;
                                    decimal.TryParse(dataReader["GIAVON"].ToString(), out giaVon);
                                    item.GIAVON = giaVon;
                                    item.MABOPK = dataReader["MABOPK"].ToString();
                                    decimal.TryParse(dataReader["GIATRI_THUE_RA"].ToString(), out GIATRI_THUE_RA);
                                    item.GIATRI_THUE_RA = GIATRI_THUE_RA;
                                    decimal.TryParse(dataReader["TIEN_CHIETKHAU"].ToString(), out TIEN_CHIETKHAU);
                                    item.TIEN_CHIETKHAU = TIEN_CHIETKHAU;
                                    decimal.TryParse(dataReader["TYLE_CHIETKHAU"].ToString(), out TYLE_CHIETKHAU);
                                    item.TYLE_CHIETKHAU = TYLE_CHIETKHAU;
                                    decimal.TryParse(dataReader["TYLE_KHUYENMAI"].ToString(), out TYLE_KHUYENMAI);
                                    item.TYLE_KHUYENMAI = TYLE_KHUYENMAI;
                                    decimal.TryParse(dataReader["TIEN_KHUYENMAI"].ToString(), out TIEN_KHUYENMAI);
                                    item.TIEN_KHUYENMAI = TIEN_KHUYENMAI;
                                    decimal.TryParse(dataReader["TIEN_VOUCHER"].ToString(), out TIEN_VOUCHER);
                                    item.TIEN_VOUCHER = TIEN_VOUCHER;
                                    decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                    item.GIAVON = GIAVON;
                                    decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIEN);
                                    item.THANHTIEN = THANHTIEN;
                                    item.MATHUE_RA = dataReader["MATHUE_RA"].ToString();
                                    item.DONVITINH = _EXTEND_VATTU_DTO.DONVITINH;
                                    GDQUAY_DTO.LST_DETAILS.Add(item);
                                }
                            }
                            //Add mã bó hàng vào list
                            if (listBoHang.Count > 0)
                            {
                                foreach (BOHANG_DTO row in listBoHang)
                                {
                                    GIAODICH_CHITIET item = new GIAODICH_CHITIET();
                                    decimal TONGLE = 0;
                                    decimal SUM_SOLUONG_BO = 0;
                                    item.MAHANG = row.MABOHANG;
                                    SqlCommand commamdBoHang = new SqlCommand();
                                    commamdBoHang.Connection = connection;
                                    commamdBoHang.CommandText = string.Format(@"SELECT DM_BOHANG.MABOHANG,DM_BOHANG.TENBOHANG,SUM(DM_BOHANGCHITIET.SOLUONG) AS TONGSOLUONG,SUM(DM_BOHANGCHITIET.TONGLE) AS TONGLE FROM DM_BOHANG INNER JOIN DM_BOHANGCHITIET ON DM_BOHANG.MABOHANG = DM_BOHANGCHITIET.MABOHANG WHERE DM_BOHANG.MABOHANG = @MABOHANG AND DM_BOHANG.UNITCODE = @UNITCODE GROUP BY DM_BOHANG.MABOHANG,DM_BOHANG.TENBOHANG");
                                    commamdBoHang.Parameters.Add("MABOHANG", SqlDbType.NVarChar, 50).Value = row.MABOHANG;
                                    commamdBoHang.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                                    SqlDataReader dataReaderBoHang = commamdBoHang.ExecuteReader();
                                    if (dataReaderBoHang.HasRows)
                                    {
                                        while (dataReaderBoHang.Read())
                                        {
                                            item.TENHANG = dataReaderBoHang["TENBOHANG"].ToString();
                                            decimal.TryParse(dataReaderBoHang["TONGSOLUONG"].ToString(), out SUM_SOLUONG_BO);
                                            decimal.TryParse(dataReaderBoHang["TONGLE"].ToString(), out TONGLE);
                                        }
                                    }
                                    decimal.TryParse((row.TONGSL / SUM_SOLUONG_BO).ToString(), out soLuong);
                                    item.SOLUONG = soLuong;
                                    decimal.TryParse(row.THANHTIEN.ToString(), out giaBanLeVAT);
                                    item.GIABANLE_VAT = giaBanLeVAT;
                                    item.DONVITINH = "BÓ";
                                    item.GIAVON = 0;
                                    item.MABOPK = row.MABOHANG.ToString();
                                    item.GIATRI_THUE_RA = 0;
                                    item.MA_KHUYENMAI = "";
                                    item.TIEN_CHIETKHAU = 0;
                                    item.TYLE_CHIETKHAU = 0;
                                    item.TYLE_KHUYENMAI = Decimal.Round((row.THANHTIEN - TONGLE) / row.THANHTIEN);
                                    item.TIEN_KHUYENMAI = row.THANHTIEN - TONGLE;
                                    item.TIEN_VOUCHER = 0;
                                    item.GIAVON = 0;
                                    item.THANHTIEN = giaBanLeVAT;
                                    item.MATHUE_RA = "";
                                    GDQUAY_DTO.LST_DETAILS.Add(item);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
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
                    NotificationLauncher.ShowNotificationError("Thông báo", "Không có kết nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                }
            }
            return GDQUAY_DTO;
        }

        public static GIAODICH_DTO SEARCH_BY_CODE_PAY_FROM_ORACLE(string MA_GIAODICH)
        {
            GIAODICH_DTO GDQUAY_DTO = new GIAODICH_DTO();
            //BEGIN LOGIN
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmdParent = new OracleCommand();
                        cmdParent.Connection = connection;
                        cmdParent.CommandText = string.Format(@"SELECT * FROM GIAODICH WHERE MA_GIAODICH = :MA_GIAODICH AND UNITCODE = :UNITCODE");
                        cmdParent.CommandType = CommandType.Text;
                        cmdParent.Parameters.Add("MA_GIAODICH", OracleDbType.NVarchar2, 50).Value = MA_GIAODICH.Trim();
                        cmdParent.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                        OracleDataReader dataReaderParent = null;
                        dataReaderParent = cmdParent.ExecuteReader();
                        if (dataReaderParent.HasRows)
                        {
                            decimal TIENKHACH_TRA, TIEN_TRALAI_KHACH = 0;
                            while (dataReaderParent.Read())
                            {
                                GDQUAY_DTO.LOAI_GIAODICH = dataReaderParent["LOAI_GIAODICH"].ToString();
                                GDQUAY_DTO.MA_GIAODICH = dataReaderParent["MA_GIAODICH"].ToString().Trim();
                                GDQUAY_DTO.I_CREATE_DATE = DateTime.Parse(dataReaderParent["I_CREATE_DATE"].ToString());
                                GDQUAY_DTO.I_CREATE_BY = dataReaderParent["I_CREATE_BY"].ToString();
                                GDQUAY_DTO.NGAY_GIAODICH = DateTime.Parse(dataReaderParent["NGAY_GIAODICH"].ToString());
                                decimal.TryParse(dataReaderParent["TIENKHACH_TRA"].ToString(), out TIENKHACH_TRA);
                                GDQUAY_DTO.TIENKHACH_TRA = TIENKHACH_TRA;
                                decimal.TryParse(dataReaderParent["TIEN_TRALAI_KHACH"].ToString(), out TIEN_TRALAI_KHACH);
                                GDQUAY_DTO.TIEN_TRALAI_KHACH = TIEN_TRALAI_KHACH;
                                GDQUAY_DTO.THOIGIAN_TAO = dataReaderParent["THOIGIAN_TAO"].ToString();
                                GDQUAY_DTO.MAKHACHHANG = dataReaderParent["MAKHACHHANG"].ToString();
                                GDQUAY_DTO.UNITCODE = dataReaderParent["UNITCODE"].ToString();
                            }
                        }
                        string TABLE_NAME = "";
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = string.Format(@"SELECT
                                                        'XNT_'
                                                        || NAM
                                                        || '_KY_'
                                                        || KYKETOAN AS TABLE_NAME
                                                    FROM
                                                        KYKETOAN
                                                    WHERE TUNGAY = :TUNGAY");
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("TUNGAY", OracleDbType.Date).Value = GDQUAY_DTO.NGAY_GIAODICH;
                        OracleDataReader dataReaderKyKeToan = null;
                        dataReaderKyKeToan = cmd.ExecuteReader();
                        if (dataReaderKyKeToan.HasRows)
                        {
                            while (dataReaderKyKeToan.Read())
                            {
                                TABLE_NAME = dataReaderKyKeToan["TABLE_NAME"].ToString();
                            }
                        }
                           
                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = string.Format(@"SELECT a.ID,a.MA_GIAODICH,a.MAHANG,d.TENHANG,a.MATHUE_RA,a.SOLUONG,a.GIABANLE_VAT,a.MA_KHUYENMAI,a.TYLE_KHUYENMAI,a.TIEN_KHUYENMAI,a.TYLE_CHIETKHAU,a.TIEN_CHIETKHAU,
                        a.TIENTHE_VIP,a.TIEN_VOUCHER,a.THANHTIEN,a.SAPXEP ,b.GIAVON,c.GIATRI AS GIATRI_THUE_RA
                        FROM GIAODICH_CHITIET a INNER JOIN " + TABLE_NAME + " b ON a.MAHANG = b.MAHANG AND b.MAKHO = '"+Session.Session.CurrentWareHouse + "' INNER JOIN THUE c ON a.MATHUE_RA = c.MATHUE INNER JOIN MATHANG d ON a.MAHANG = d.MAHANG AND a.MA_GIAODICH = '" + MA_GIAODICH + "'");
                        OracleDataReader dataReader = null;
                        dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            decimal giaVon, giaBanLeVAT, GIATRI_THUE_RA, TIEN_CHIETKHAU, TYLE_CHIETKHAU, TYLE_KHUYENMAI, TIEN_KHUYENMAI, TIEN_VOUCHER, GIAVON, THANHTIEN = 0;
                            decimal soLuong = 0;
                            int i = 0;
                            List<BOHANG_DTO> listBoHang = new List<BOHANG_DTO>();
                            while (dataReader.Read())
                            {
                                GIAODICH_CHITIET item = new GIAODICH_CHITIET();
                                string MaBoHangPk = "BH";
                                if (!string.IsNullOrEmpty(MaBoHangPk) && !MaBoHangPk.Equals("BH"))
                                {
                                    decimal SOLUONG, GIABANLE_VAT, THANHTIENCOVAT = 0;
                                    string MAHANG = dataReader["MAHANG"].ToString().ToUpper().Trim();
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                    decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                    decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIENCOVAT);
                                    List<BOHANG_DETAILS_DTO> listBoHangMatHangExist = new List<BOHANG_DETAILS_DTO>();
                                    BOHANG_DTO boHangExist = listBoHang.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk));
                                    if (boHangExist == null)
                                    {
                                        BOHANG_DTO boHang = new BOHANG_DTO();
                                        boHang.MABOHANG = MaBoHangPk;
                                        boHang.THANHTIEN = THANHTIENCOVAT;
                                        boHang.TONGSL = SOLUONG;
                                        BOHANG_DETAILS_DTO MatHangExist = listBoHangMatHangExist.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAHANG.Equals(MAHANG));
                                        if (MatHangExist == null)
                                        {
                                            BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                            mathang.MABOHANG = MaBoHangPk;
                                            mathang.MAHANG = MAHANG;
                                            mathang.SOLUONG = SOLUONG;
                                            mathang.GIABANLE_VAT = GIABANLE_VAT;
                                            mathang.THANHTIEN = THANHTIENCOVAT;
                                            boHang.MATHANG_BOHANG.Add(mathang);
                                        }
                                        listBoHang.Add(boHang);
                                    }
                                    else
                                    {
                                        boHangExist.THANHTIEN += THANHTIENCOVAT;
                                        boHangExist.TONGSL += SOLUONG;
                                        BOHANG_DETAILS_DTO MatHangExist = boHangExist.MATHANG_BOHANG.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAHANG.Equals(MAHANG));
                                        if (MatHangExist == null)
                                        {
                                            BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                            mathang.MABOHANG = MaBoHangPk;
                                            mathang.MAHANG = MAHANG;
                                            mathang.SOLUONG = SOLUONG;
                                            mathang.GIABANLE_VAT = GIABANLE_VAT;
                                            mathang.THANHTIEN = THANHTIENCOVAT;
                                            boHangExist.MATHANG_BOHANG.Add(mathang);
                                        }
                                    }
                                }
                                else
                                {

                                    item.MAHANG = dataReader["MAHANG"].ToString();
                                    item.TENHANG = dataReader["TENHANG"].ToString();
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out soLuong);
                                    item.SOLUONG = soLuong;
                                    decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out giaBanLeVAT);
                                    item.GIABANLE_VAT = giaBanLeVAT;
                                    decimal.TryParse(dataReader["GIAVON"].ToString(), out giaVon);
                                    item.GIAVON = giaVon;
                                    decimal.TryParse(dataReader["GIATRI_THUE_RA"].ToString(), out GIATRI_THUE_RA);
                                    item.GIATRI_THUE_RA = GIATRI_THUE_RA;
                                    if (dataReader["MA_KHUYENMAI"] != null)
                                    {
                                        item.MA_KHUYENMAI = dataReader["MA_KHUYENMAI"].ToString();
                                    }
                                    else
                                    {
                                        item.MA_KHUYENMAI = ";";
                                    }
                                    decimal.TryParse(dataReader["TIEN_CHIETKHAU"].ToString(), out TIEN_CHIETKHAU);
                                    item.TIEN_CHIETKHAU = TIEN_CHIETKHAU;
                                    decimal.TryParse(dataReader["TYLE_CHIETKHAU"].ToString(), out TYLE_CHIETKHAU);
                                    item.TYLE_CHIETKHAU = TYLE_CHIETKHAU;
                                    decimal.TryParse(dataReader["TYLE_KHUYENMAI"].ToString(), out TYLE_KHUYENMAI);
                                    item.TYLE_KHUYENMAI = TYLE_KHUYENMAI;
                                    decimal.TryParse(dataReader["TIEN_KHUYENMAI"].ToString(), out TIEN_KHUYENMAI);
                                    item.TIEN_KHUYENMAI = TIEN_KHUYENMAI;
                                    decimal.TryParse(dataReader["TIEN_VOUCHER"].ToString(), out TIEN_VOUCHER);
                                    item.TIEN_VOUCHER = TIEN_VOUCHER;
                                    decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                    item.GIAVON = GIAVON;
                                    decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIEN);
                                    item.THANHTIEN = THANHTIEN;
                                    item.MATHUE_RA = dataReader["MATHUE_RA"].ToString();
                                    OracleCommand commamdVatTu = new OracleCommand();
                                    commamdVatTu.Connection = connection;
                                    commamdVatTu.CommandText = string.Format(@"SELECT b.TENDONVITINH AS DONVITINH FROM MATHANG a INNER JOIN DONVITINH b ON a.MADONVITINH = b.MADONVITINH AND a.MAHANG = :MAHANG AND a.UNITCODE = :UNITCODE");
                                    commamdVatTu.Parameters.Add("MAHANG", OracleDbType.NVarchar2, 50).Value = item.MAHANG;
                                    commamdVatTu.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                    OracleDataReader dataReaderVatTu = commamdVatTu.ExecuteReader();
                                    if (dataReaderVatTu.HasRows)
                                    {
                                        if (dataReaderVatTu.HasRows)
                                        {
                                            while (dataReaderVatTu.Read())
                                            {
                                                item.DONVITINH = dataReaderVatTu["DONVITINH"].ToString();
                                            }
                                        }
                                    }
                                    GDQUAY_DTO.LST_DETAILS.Add(item);
                                }
                            }
                            //Add mã bó hàng vào list
                            if (listBoHang.Count > 0)
                            {
                                foreach (BOHANG_DTO row in listBoHang)
                                {
                                    GIAODICH_CHITIET item = new GIAODICH_CHITIET();
                                    decimal TONGLE = 0;
                                    decimal SUM_SOLUONG_BO = 0;
                                    item.MAHANG = row.MABOHANG;
                                    OracleCommand commamdBoHang = new OracleCommand();
                                    commamdBoHang.Connection = connection;
                                    commamdBoHang.CommandText = string.Format(@"SELECT a.MABOHANG,a.TENBOHANG,SUM(b.SOLUONG) AS TONGSOLUONG,SUM(b.TONGTIEN) AS TONGLE FROM BOHANG a INNER JOIN BOHANG_CHITIET b ON a.MABOHANG = b.MABOHANG WHERE a.MABOHANG = :MABOHANG AND a.UNITCODE = :UNITCODE GROUP BY a.MABOHANG,a.TENBOHANG");
                                    commamdBoHang.Parameters.Add("MABOHANG", OracleDbType.NVarchar2, 50).Value = row.MABOHANG;
                                    commamdBoHang.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                    OracleDataReader dataReaderBoHang = commamdBoHang.ExecuteReader();
                                    if (dataReaderBoHang.HasRows)
                                    {
                                        while (dataReaderBoHang.Read())
                                        {
                                            item.TENHANG = dataReaderBoHang["TENBOHANG"].ToString();
                                            decimal.TryParse(dataReaderBoHang["TONGSOLUONG"].ToString(), out SUM_SOLUONG_BO);
                                            decimal.TryParse(dataReaderBoHang["TONGLE"].ToString(), out TONGLE);
                                        }
                                    }
                                    decimal.TryParse((row.TONGSL / SUM_SOLUONG_BO).ToString(), out soLuong);
                                    item.SOLUONG = soLuong;
                                    item.GIABANLE_VAT = TONGLE;
                                    item.DONVITINH = "BÓ";
                                    item.GIAVON = 0;
                                    item.MABOPK = row.MABOHANG;
                                    item.GIATRI_THUE_RA = row.TYLEVAT_BO;
                                    item.MA_KHUYENMAI = "";
                                    item.TIEN_CHIETKHAU = 0;
                                    item.TYLE_CHIETKHAU = 0;
                                    item.TIEN_KHUYENMAI = 0;
                                    item.TIEN_VOUCHER = 0;
                                    item.TIEN_VOUCHER = 0;
                                    if (row.MATHANG_BOHANG.Count > 0)
                                    {
                                        decimal SUM_TTIENCOVAT_MATHANG_BOHANG = 0;
                                        foreach (BOHANG_DETAILS_DTO _BOHANG_DETAILS_DTO in row.MATHANG_BOHANG)
                                        {
                                            SUM_TTIENCOVAT_MATHANG_BOHANG += _BOHANG_DETAILS_DTO.THANHTIEN;
                                        }
                                        item.THANHTIEN = SUM_TTIENCOVAT_MATHANG_BOHANG;
                                    }
                                    item.GIAVON = 0;
                                    item.THANHTIEN = row.THANHTIEN;
                                    item.MATHUE_RA = row.MAVAT_BO;
                                    GDQUAY_DTO.LST_DETAILS.Add(item);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
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
                    NotificationLauncher.ShowNotificationError("Thông báo", "Không có kết nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                }
            }
            return GDQUAY_DTO;
            //END LOGIN
        }
        private void BINDING_DATA_TO_DATAGRIDVIEW(string MaGiaoDichQuay)
        {
            GIAODICH_DTO data = new GIAODICH_DTO();
            data.LST_DETAILS = new List<GIAODICH_CHITIET>();
            try
            {
                if (!string.IsNullOrEmpty(MaGiaoDichQuay))
                {
                    try
                    {
                        if (Config.CheckConnectToServer())
                        {
                            data = SEARCH_BY_CODE_PAY_FROM_ORACLE(MaGiaoDichQuay);
                        }
                        else
                        {
                            data = SEARCH_BY_CODE_PAY_FROM_SQLSERVER(MaGiaoDichQuay);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("THÔNG BÁO ! XẢY RA LỖI KHI LẤY DỮ LIỆU TỪ CƠ SỞ DỮ LIỆU");
                    }
                    if (data != null)
                    {
                        dgvDetails_Tab.Rows.Clear();
                        dgvDetails_Tab.DataSource = null;
                        dgvDetails_Tab.Refresh();
                        decimal SUM_SOLUONG = 0;
                        decimal SUM_TIENKHUYENMAI = 0;
                        foreach (GIAODICH_CHITIET vattu in data.LST_DETAILS)
                        {
                            SUM_TIENKHUYENMAI += vattu.TIEN_KHUYENMAI;
                            SUM_SOLUONG += vattu.SOLUONG;
                            int idx = dgvDetails_Tab.Rows.Add();
                            DataGridViewRow rowData = dgvDetails_Tab.Rows[idx];
                            rowData.Cells["KEY"].Value = idx + 1;
                            rowData.Cells["MAHANG"].Value = vattu.MAHANG;
                            rowData.Cells["TENHANG"].Value = vattu.TENHANG;
                            rowData.Cells["DONVITINH"].Value = vattu.DONVITINH;
                            rowData.Cells["GIABANLE_VAT"].Value = FormatCurrency.FormatMoney(vattu.GIABANLE_VAT);
                            rowData.Cells["SOLUONG"].Value = FormatCurrency.FormatMoney(vattu.SOLUONG);
                            rowData.Cells["GIAVON"].Value = FormatCurrency.FormatMoney(vattu.GIAVON);
                            rowData.Cells["THANHTIEN"].Value = FormatCurrency.FormatMoney(vattu.THANHTIEN);
                            rowData.Cells["TIEN_KHUYENMAI"].Value = FormatCurrency.FormatMoney(vattu.TIEN_KHUYENMAI);
                            if (dgvDetails_Tab.RowCount > 0)
                            {
                                this.dgvDetails_Tab.Sort(this.dgvDetails_Tab.Columns["KEY"], ListSortDirection.Ascending);
                                this.dgvDetails_Tab.ClearSelection();
                                this.dgvDetails_Tab.Rows[0].Selected = true;
                            }
                        }
                        txtChiTiet_ThuNgan.Text = data.I_CREATE_BY;
                        txtChiTiet_ThoiGian.Text = data.NGAY_GIAODICH.ToString("dd/MM/yyyy") + " - " + data.THOIGIAN_TAO;
                        txtChiTiet_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG);
                        txtChiTiet_TienTraLai.Text = FormatCurrency.FormatMoney(data.TIEN_TRALAI_KHACH);
                        txtChiTiet_TongTien.Text = FormatCurrency.FormatMoney(data.THANHTIEN);
                        txtChiTiet_TienChietKhau.Text = FormatCurrency.FormatMoney(SUM_TIENKHUYENMAI);
                    }
                }
            }
            catch
            {
                MessageBox.Show("THÔNG BÁO ! XẢY RA LỖI KHI ĐẨY DỮ LIỆU DATAGRIDVIEW ");
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string MA_GIAODICH = txtKeySearch.Text.Trim();
            BINDING_DATA_TO_DATAGRIDVIEW(MA_GIAODICH);
        }
        public void RefreshGiaoDichBanTraLai(bool state)
        {
            if (state)
            {
                txtKeySearch.Focus();
                btnF2ThemMoiTraLai.Enabled = true;
                btnF1ThanhToanTraLai.Enabled = false;
                btnF3GiamHang.Enabled = false;
                btnF4TangHang.Enabled = false;
                btnF5LamMoiGiaoDich.Enabled = false;
                txtKeySearch.Enabled = false;
                dateTimeTuNgay.Enabled = false;
                dateTimeDenNgay.Enabled = false;
            }
        }
        const int WM_KEYDOWN = 0x0101;
        //const int WM_KEYUP = 0x0100;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                if ((Keys)m.WParam == Keys.F1)
                {
                    GIAODICH_DTO _GIAODICH_DTO = new GIAODICH_DTO();
                    GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_BILL = new GIAODICH_DTO();
                    if (Config.CheckConnectToServer())
                    {
                        _GIAODICH_DTO = FrmThanhToanTraLaiService.KHOITAO_DULIEU_THANHTOAN_BANTRALAI_FROM_ORACLE(txtTraLai_MaGiaoDich.Text.Trim(), txtTraLai_TongTien.Text, dgvTraLai);
                        _NVGDQUAY_ASYNCCLIENT_BILL = FrmThanhToanTraLaiService.KHOITAO_BILL_THANHTOAN_BANTRALAI_FROM_ORACLE(txtTraLai_MaGiaoDich.Text.Trim(), txtTraLai_TongTien.Text, dgvTraLai);
                    }
                    else
                    {
                        _GIAODICH_DTO = FrmThanhToanTraLaiService.KHOITAO_DULIEU_THANHTOAN_BANTRALAI_FROM_SQLSERVER(txtTraLai_MaGiaoDich.Text.Trim(), txtTraLai_TongTien.Text, dgvTraLai);
                        _NVGDQUAY_ASYNCCLIENT_BILL = FrmThanhToanTraLaiService.KHOITAO_BILL_THANHTOAN_BANTRALAI_FROM_SQLSERVER(txtTraLai_MaGiaoDich.Text.Trim(), txtTraLai_TongTien.Text, dgvTraLai);
                    }
                    FrmThanhToanTraLai frmThanhToanTraLai = new FrmThanhToanTraLai(_GIAODICH_DTO, _NVGDQUAY_ASYNCCLIENT_BILL);
                    frmThanhToanTraLai.SetHanler(RefreshGiaoDichBanTraLai); //Set sự kiện khi đóng form Thanh toán 
                    frmThanhToanTraLai.ShowDialog();
                }
                if ((Keys)m.WParam == Keys.F2)
                {

                    if (string.IsNullOrEmpty(txtTraLai_MaGiaoDich.Text.Trim()) && dgvTraLai.RowCount == 0 && dgvDetails_Tab.RowCount == 0)
                    {
                        btnF2ThemMoiTraLai.Enabled = false;
                        txtKeySearch.Enabled = true;
                        txtKeySearch.Focus();
                        dateTimeTuNgay.Enabled = true;
                        dateTimeDenNgay.Enabled = true;
                        btnTimKiem.Enabled = true;

                    }

                    if (btnF2ThemMoiTraLai.Enabled && dgvTraLai.RowCount > 0)
                    {
                        txtKeySearch.Text = "";
                        txtChiTiet_TienChietKhau.Text = "";
                        txtChiTiet_ThoiGian.Text = "";
                        txtChiTiet_ThuNgan.Text = "";
                        txtChiTiet_TienChietKhau.Text = "";
                        txtChiTiet_TienTraLai.Text = "";
                        txtChiTiet_TongTien.Text = "";
                        txtTraLai_SoLuong.Text = "";
                        txtTraLai_SoLuong.Text = "";
                        txtTraLai_MaGiaoDich.Text = "";
                        txtTraLai_ThuNgan.Text = "";
                        txtChiTiet_TongSoLuong.Text = "";
                        txtTraLai_TongSoLuong.Text = "";
                        txtTraLai_MaVatTu.Text = "";
                        txtTraLai_TongTien.Text = "";
                        if (dgvDetails_Tab.RowCount > 0)
                        {
                            dgvDetails_Tab.Rows.Clear();
                            dgvDetails_Tab.DataSource = null;
                            dgvDetails_Tab.Refresh();
                        }
                        if (dgvTraLai.RowCount > 0)
                        {
                            dgvTraLai.Rows.Clear();
                            dgvTraLai.DataSource = null;
                            dgvTraLai.Refresh();
                            SUM_SOLUONG_BAN_TRALAI(dgvTraLai);
                            SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai);
                        }
                    }
                }
                //currentRowDgvTraLai
                if ((Keys)m.WParam == Keys.F3)
                {
                    if (currentRowDgvTraLai != -1 && dgvTraLai.RowCount > 0)
                    {
                        decimal SoLuongDangChon = 0;
                        decimal.TryParse(dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value.ToString(), out SoLuongDangChon);
                        SoLuongDangChon = SoLuongDangChon - 1;
                        if (SoLuongDangChon > 0)
                        {
                            decimal SoLuongGioiHanNhap = 0;
                            string MaVatTuDangChon = dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                            if (!string.IsNullOrEmpty(MaVatTuDangChon) && dgvDetails_Tab.RowCount > 0)
                            {
                                foreach (DataGridViewRow rowCheck in dgvDetails_Tab.Rows)
                                {
                                    string code = rowCheck.Cells["MAHANG"].Value.ToString();
                                    if (!string.IsNullOrEmpty(code) &&
                                        code.Trim().ToUpper() == MaVatTuDangChon.Trim().ToUpper())
                                    {
                                        decimal.TryParse(
                                            dgvDetails_Tab.Rows[rowCheck.Index].Cells["SOLUONG"].Value.ToString(),
                                            out SoLuongGioiHanNhap);
                                    }
                                }
                            }
                            if (SoLuongDangChon > SoLuongGioiHanNhap)
                            {
                                NotificationLauncher.ShowNotificationError("Cảnh báo", "Quá số lượng cho phép", 1, "0x1",
                                    "0x8", "normal");
                            }
                            else
                            {
                                decimal DONGIA = 0;
                                decimal.TryParse(dgvTraLai.Rows[currentRowDgvTraLai].Cells["GIABANLE_VAT_TRALAI"].Value.ToString(),out DONGIA);
                                dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value =
                                    FormatCurrency.FormatMoney(SoLuongDangChon);
                                dgvTraLai.Rows[currentRowDgvTraLai].Cells["THANHTIEN_TRALAI"].Value =
                                    FormatCurrency.FormatMoney(DONGIA * SoLuongDangChon);
                                txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                                txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                            }
                        }
                        else
                        {
                            DialogResult result = MessageBox.Show("BẠN MUỐN XÓA MÃ NÀY ?", "CẢNH BÁO", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes)
                            {
                                if (dgvTraLai.Rows.Count == 1)
                                {
                                    dgvTraLai.Rows.Clear();
                                    dgvTraLai.DataSource = null;
                                    dgvTraLai.Refresh();

                                    txtTraLai_SoLuong.Text = "";
                                    txtTraLai_MaVatTu.Text = "";
                                    txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                                    txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                                }
                                else
                                {
                                    dgvTraLai.Rows.RemoveAt(currentRowDgvTraLai);
                                }
                            }
                            else if (result == DialogResult.No)
                            {
                                //code for No
                            }
                            else if (result == DialogResult.Cancel)
                            {
                                //code for Cancel
                            }
                        }
                        if (this.dgvTraLai.Rows.Count > 0)
                        {
                            this.dgvTraLai.Sort(this.dgvTraLai.Columns["KEY_TRALAI"], ListSortDirection.Ascending);
                            this.dgvTraLai.ClearSelection();
                            this.dgvTraLai.Rows[0].Selected = true;
                            txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                            txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                        }
                    }
                }
                if ((Keys)m.WParam == Keys.F4)
                {
                    if (currentRowDgvTraLai != -1 && dgvTraLai.RowCount > 0)
                    {
                        decimal SoLuongDangChon = 0;
                        decimal.TryParse(dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value.ToString(), out SoLuongDangChon);
                        SoLuongDangChon = SoLuongDangChon + 1;
                        decimal SoLuongGioiHanNhap = 0;
                        string MaVatTuDangChon = dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                        if (!string.IsNullOrEmpty(MaVatTuDangChon) && dgvDetails_Tab.RowCount > 0)
                        {
                            foreach (DataGridViewRow rowCheck in dgvDetails_Tab.Rows)
                            {
                                string code = rowCheck.Cells["MAHANG"].Value.ToString();
                                if (!string.IsNullOrEmpty(code) && code.Trim().ToUpper() == MaVatTuDangChon.Trim().ToUpper())
                                {
                                    decimal.TryParse(dgvDetails_Tab.Rows[rowCheck.Index].Cells["SOLUONG"].Value.ToString(), out SoLuongGioiHanNhap);
                                }
                            }
                        }
                        if (SoLuongDangChon > SoLuongGioiHanNhap)
                        {
                            NotificationLauncher.ShowNotificationError("CẢNH BÁO", "QUÁ SỐ LƯỢNG CHO PHÉP", 1, "0x1", "0x8", "normal");
                        }
                        else
                        {
                            decimal DONGIA = 0; decimal.TryParse(dgvTraLai.Rows[currentRowDgvTraLai].Cells["GIABANLE_VAT_TRALAI"].Value.ToString(), out DONGIA);
                            dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value = FormatCurrency.FormatMoney(SoLuongDangChon);
                            dgvTraLai.Rows[currentRowDgvTraLai].Cells["THANHTIEN_TRALAI"].Value = FormatCurrency.FormatMoney(DONGIA * SoLuongDangChon);
                            txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                            txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                        }
                    }
                }
                if ((Keys)m.WParam == Keys.F5)
                {
                    if (dgvTraLai.RowCount > 0)
                    {
                        DialogResult result = MessageBox.Show("BẠN MUỐN LÀM MỚI GIAO DỊCH ?", "CẢNH BÁO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            dgvTraLai.Rows.Clear();
                            dgvTraLai.DataSource = null;
                            dgvTraLai.Refresh();
                            SUM_SOLUONG_BAN_TRALAI(dgvTraLai);
                            SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai);

                            btnF1ThanhToanTraLai.Enabled = false;
                            btnF3GiamHang.Enabled = false;
                            btnF4TangHang.Enabled = false;
                            btnF5LamMoiGiaoDich.Enabled = false;
                        }
                    }
                }
                if ((Keys)m.WParam == Keys.F6)
                {
                    FrmTimKiemGiaoDich frmSearch = new FrmTimKiemGiaoDich(txtKeySearch.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                    frmSearch.SetHanlerGiaoDichSearch(LoadDataSearch);
                    frmSearch.ShowDialog();
                }
            }
            return base.ProcessKeyPreview(ref m);
        }
        public void LoadDataSearch(string MaGiaoDichTimKiem, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc)
        {
            cboDieuKienChon.SelectedIndex = DieuKienLoc;
            dateTimeDenNgay.Format = DateTimePickerFormat.Custom;
            dateTimeDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeDenNgay.Value = DenNgay;
            dateTimeTuNgay.Format = DateTimePickerFormat.Custom;
            dateTimeTuNgay.CustomFormat = "dd/MM/yyyy"; this.dateTimeTuNgay.Value = TuNgay;
            GIAODICH_DTO data = new GIAODICH_DTO();
            data.LST_DETAILS = new List<GIAODICH_CHITIET>();
            try
            {
                if (!string.IsNullOrEmpty(MaGiaoDichTimKiem))
                {
                    if (Config.CheckConnectToServer())
                    {
                        data = SEARCH_BY_CODE_PAY_FROM_ORACLE(MaGiaoDichTimKiem);
                    }
                    else
                    {
                        data = SEARCH_BY_CODE_PAY_FROM_SQLSERVER(MaGiaoDichTimKiem);
                    }
                    if (data != null)
                    {
                        dgvDetails_Tab.Rows.Clear();
                        dgvDetails_Tab.DataSource = null;
                        dgvDetails_Tab.Refresh();
                        decimal SUM_SOLUONG = 0;
                        decimal SUM_TIENKHUYENMAI = 0;
                        foreach (GIAODICH_CHITIET vattu in data.LST_DETAILS)
                        {
                            SUM_TIENKHUYENMAI += vattu.TIEN_KHUYENMAI;
                            SUM_SOLUONG += vattu.SOLUONG;
                            int idx = dgvDetails_Tab.Rows.Add();
                            DataGridViewRow rowData = dgvDetails_Tab.Rows[idx];
                            rowData.Cells["KEY"].Value = idx + 1;
                            rowData.Cells["MAHANG"].Value = vattu.MAHANG;
                            rowData.Cells["TENHANG"].Value = vattu.TENHANG;
                            rowData.Cells["DONVITINH"].Value = vattu.DONVITINH;
                            rowData.Cells["DONGIA"].Value = FormatCurrency.FormatMoney(vattu.GIABANLE_VAT);
                            rowData.Cells["SOLUONG"].Value = FormatCurrency.FormatMoney(vattu.SOLUONG);
                            rowData.Cells["GIAVON"].Value = FormatCurrency.FormatMoney(vattu.GIAVON);
                            rowData.Cells["THANHTIEN"].Value = FormatCurrency.FormatMoney(vattu.THANHTIEN);
                            rowData.Cells["TIEN_KHUYENMAI"].Value = FormatCurrency.FormatMoney(vattu.TIEN_KHUYENMAI);
                            if (dgvDetails_Tab.RowCount > 0)
                            {
                                this.dgvDetails_Tab.Sort(this.dgvDetails_Tab.Columns["KEY"], ListSortDirection.Ascending);
                                this.dgvDetails_Tab.ClearSelection();
                                this.dgvDetails_Tab.Rows[0].Selected = true;
                            }
                        }
                        if (dgvTraLai.RowCount <= 0)
                        {
                            btnF1ThanhToanTraLai.Enabled = false;
                            btnF3GiamHang.Enabled = false;
                            btnF4TangHang.Enabled = false;
                            btnF5LamMoiGiaoDich.Enabled = false;
                        }
                        txtChiTiet_ThuNgan.Text = data.I_CREATE_BY;
                        txtChiTiet_ThoiGian.Text = data.NGAY_GIAODICH.ToString("dd/MM/yyyy") + " - " + data.THOIGIAN_TAO;
                        txtChiTiet_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG);
                        txtChiTiet_TienTraLai.Text = FormatCurrency.FormatMoney(data.TIEN_TRALAI_KHACH);
                        txtChiTiet_TongTien.Text = FormatCurrency.FormatMoney(data.THANHTIEN);
                        txtChiTiet_TienChietKhau.Text = FormatCurrency.FormatMoney(SUM_TIENKHUYENMAI);
                        txtTraLai_MaGiaoDich.Text = data.MA_GIAODICH.Trim() + "-TRL";
                        txtKeySearch.Enabled = true;
                        dateTimeTuNgay.Enabled = true;
                        dateTimeDenNgay.Enabled = true;
                        btnTimKiem.Enabled = true; txtKeySearch.Text = "";

                    }
                }
            }
            catch (Exception ex)
            {
                txtKeySearch.Text = "";
                WriteLogs.LogError(ex);
            }
        }

        private decimal SUM_TIEN_THANHTOAN_TRALAI(DataGridView dgvCheck)
        {
            decimal SUM_THANHTIEN = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal THANHTIEN_ROW = 0;
                    decimal.TryParse(rowCheck.Cells["THANHTIEN_TRALAI"].Value.ToString(), out THANHTIEN_ROW);
                    SUM_THANHTIEN = SUM_THANHTIEN + THANHTIEN_ROW;
                }
            }
            return SUM_THANHTIEN;
        }
        private decimal SUM_SOLUONG_BAN_TRALAI(DataGridView dgvCheck)
        {
            decimal SUM_SOLUONG = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal SOLUONG_ROW = 0;
                    decimal.TryParse(rowCheck.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG_ROW);
                    SUM_SOLUONG = SUM_SOLUONG + SOLUONG_ROW;
                }
            }
            return SUM_SOLUONG;
        }

        private void dgvDetails_Tab_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            currentRowDgvTraLai = 0;
            string maVatTuChange = dgvDetails_Tab.Rows[e.RowIndex].Cells["MAHANG"].Value.ToString().Trim();
            if (CheckExistInDgvTraLai(dgvTraLai, maVatTuChange) != -1)
            {
                NotificationLauncher.ShowNotificationWarning("Thông báo", "Mã này đã chọn !", 1, "0x1", "0x8", "normal");
                txtTraLai_SoLuong.SelectAll();
                txtTraLai_SoLuong.Focus();
            }
            else
            {
                if (e.RowIndex != -1)
                {
                    currentRowDgvTraLai = e.RowIndex;
                    int idx = 0;
                    int index = e.RowIndex;
                    try
                    {
                        idx = dgvTraLai.Rows.Add();
                    }
                    catch (Exception ex)
                    {

                    }
                    DataGridViewRow rowDataTraLai = dgvTraLai.Rows[idx];
                    rowDataTraLai.Cells["KEY_TRALAI"].Value = idx + 1;
                    string MAHANG = dgvDetails_Tab.Rows[index].Cells["MAHANG"].Value.ToString().Trim();
                    rowDataTraLai.Cells["MAHANG_TRALAI"].Value = MAHANG;
                    string TENHANG = dgvDetails_Tab.Rows[index].Cells["TENHANG"].Value.ToString().Trim();
                    rowDataTraLai.Cells["TENHANG_TRALAI"].Value = TENHANG;
                    decimal SoLuong = 0; decimal.TryParse(dgvDetails_Tab.Rows[index].Cells["SOLUONG"].Value.ToString(), out SoLuong);
                    rowDataTraLai.Cells["SOLUONG_TRALAI"].Value = FormatCurrency.FormatMoney(SoLuong);
                    decimal DonGia = 0; decimal.TryParse(dgvDetails_Tab.Rows[index].Cells["GIABANLE_VAT"].Value.ToString(), out DonGia);
                    rowDataTraLai.Cells["GIABANLE_VAT_TRALAI"].Value = FormatCurrency.FormatMoney(DonGia);
                    string DonViTinh = dgvDetails_Tab.Rows[index].Cells["DONVITINH"].Value != null ? dgvDetails_Tab.Rows[index].Cells["DONVITINH"].Value.ToString() : "";
                    rowDataTraLai.Cells["DONVITINH_TRALAI"].Value = DonViTinh;
                    decimal TIEN_KHUYENMAI = 0; decimal.TryParse(dgvDetails_Tab.Rows[index].Cells["TIEN_KHUYENMAI"].Value.ToString(), out TIEN_KHUYENMAI);
                    rowDataTraLai.Cells["TIEN_KHUYENMAI_TRALAI"].Value = FormatCurrency.FormatMoney(TIEN_KHUYENMAI);
                    decimal ThanhTien = 0; decimal.TryParse(dgvDetails_Tab.Rows[index].Cells["THANHTIEN"].Value.ToString(), out ThanhTien);
                    rowDataTraLai.Cells["THANHTIEN_TRALAI"].Value = FormatCurrency.FormatMoney(ThanhTien);
                    decimal GiaVon = 0; decimal.TryParse(dgvDetails_Tab.Rows[index].Cells["GIAVON"].Value.ToString(), out GiaVon);
                    rowDataTraLai.Cells["GIAVON_TRALAI"].Value = FormatCurrency.FormatMoney(GiaVon);
                    if (dgvTraLai.RowCount > 0)
                    {
                        this.dgvTraLai.Sort(this.dgvTraLai.Columns["KEY_TRALAI"], ListSortDirection.Ascending);
                        this.dgvTraLai.ClearSelection();
                        this.dgvTraLai.Rows[0].Selected = true;
                        txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                        txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                    }
                    btnF1ThanhToanTraLai.Enabled = true;
                    btnF3GiamHang.Enabled = true;
                    btnF4TangHang.Enabled = true;
                    btnF5LamMoiGiaoDich.Enabled = true;
                    txtTraLai_MaVatTu.Text = MAHANG;
                    txtTraLai_SoLuong.Text = FormatCurrency.FormatMoney(SoLuong);
                    txtTraLai_SoLuong.SelectAll();
                    txtTraLai_SoLuong.Focus();
                }
            }
        }

        private int CheckExistInDgvTraLai(DataGridView dgvTraLai, string MAHANG)
        {
            int result = -1;
            foreach (DataGridViewRow rowCheck in dgvTraLai.Rows)
            {
                string code = string.Empty;
                code = rowCheck.Cells["MAHANG_TRALAI"].Value.ToString();
                if (!string.IsNullOrEmpty(code) && code.Trim().ToUpper() == MAHANG.Trim().ToUpper())
                {
                    result = rowCheck.Index;
                }
            }
            return result;
        }

        //Setcurrent Row
        // Sự kiện này để thay đổi giá trị điều kiện tìm kiếm
        private void cboDieuKienChon_SelectedValueChanged(object sender, EventArgs e)
        {
            VALUE_SELECTED_CHANGE = (int)cboDieuKienChon.SelectedIndex;
        }

        private void txtTraLai_SoLuong_Validating(object sender, CancelEventArgs e)
        {
            //index dòng đang chọn trên datagridview bán trả lại
            if (currentRowDgvTraLai != -1 && dgvTraLai.RowCount > 0)
            {
                decimal SoLuongNhapVao = 0;
                decimal SoLuongGioiHanNhap = 0;
                decimal.TryParse(txtTraLai_SoLuong.Text, out SoLuongNhapVao);
                string MaVatTuDangChon = dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                if (!string.IsNullOrEmpty(MaVatTuDangChon) && dgvDetails_Tab.RowCount > 0)
                {
                    foreach (DataGridViewRow rowCheck in dgvDetails_Tab.Rows)
                    {
                        string code = rowCheck.Cells["MAHANG"].Value.ToString();
                        if (!string.IsNullOrEmpty(code) && code.Trim().ToUpper() == MaVatTuDangChon.Trim().ToUpper())
                        {
                            decimal.TryParse(dgvDetails_Tab.Rows[rowCheck.Index].Cells["SOLUONG"].Value.ToString(), out SoLuongGioiHanNhap);
                        }
                    }
                }
                if (SoLuongNhapVao > SoLuongGioiHanNhap)
                {
                    //NotificationLauncher.ShowNotification("Cảnh báo", "Quá số lượng cho phép", 1, "0x1", "0x8","normal");
                }
                else
                {
                    decimal DONGIA = 0; decimal.TryParse(dgvTraLai.Rows[currentRowDgvTraLai].Cells["GIABANLE_VAT_TRALAI"].Value.ToString(), out DONGIA);
                    dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value = FormatCurrency.FormatMoney(SoLuongNhapVao);
                    dgvTraLai.Rows[currentRowDgvTraLai].Cells["THANHTIEN_TRALAI"].Value = FormatCurrency.FormatMoney(DONGIA * SoLuongNhapVao);
                    txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                    txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                }
            }
        }

        //Setcurrent Row
        // Sự kiện này để thay đổi rowIndex chọn
        private void dgvTraLai_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTraLai.CurrentRow.Index != currentRowDgvTraLai)
            {
                currentRowDgvTraLai = dgvTraLai.CurrentRow.Index;
                txtTraLai_MaVatTu.Text = dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAHANG_TRALAI"].Value != null ? dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAHANG_TRALAI"].Value.ToString().ToUpper().Trim() : "";
                txtTraLai_SoLuong.Text = dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value != null ? dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value.ToString().ToUpper().Trim() : "0";
            }
        }

        //txtKeySearch_KeyDown == txtKeySearch_Validating
        private void txtKeySearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtKeySearch.Text.Length > 3)
            {
                try
                {
                    //SỰ KIỆN NHẤN ENTER TÌM KIẾM GIAO DỊCH
                    if (Config.CheckConnectToServer()) // CÓ MẠNG INTERNET
                    {
                        List<GIAODICH_DTO> listData = TIMKIEM_GIAODICHQUAY(txtKeySearch.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                        if (listData.Count > 0 && listData.Count == 1)
                        {
                            //TÌM ĐÚNG MÃ GIAO DỊCH THÌ BINDING DỮ LIỆU VÀO GRIDVIEW
                            BINDING_DATA_TO_DATAGRIDVIEW(listData[0].MA_GIAODICH.Trim());
                            txtTraLai_MaGiaoDich.Text = listData[0].MA_GIAODICH.Trim() + "-TRL";
                        }
                        else
                        {
                            FrmTimKiemGiaoDich frmSearch = new FrmTimKiemGiaoDich(txtKeySearch.Text.Trim(),
                                dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                            frmSearch.SetHanlerGiaoDichSearch(LoadDataSearch);
                            frmSearch.ShowDialog();
                        }
                    }
                    else // MẤT MẠNG INTERNET
                    {
                        List<GIAODICH_DTO> listData = TIMKIEM_GIAODICHQUAY_FROM_SQL(txtKeySearch.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                        if (listData.Count > 0 && listData.Count == 1)
                        {
                            //TÌM ĐÚNG MÃ GIAO DỊCH THÌ BINDING DỮ LIỆU VÀO GRIDVIEW
                            BINDING_DATA_TO_DATAGRIDVIEW(listData[0].MA_GIAODICH.Trim());
                            txtTraLai_MaGiaoDich.Text = listData[0].MA_GIAODICH.Trim() + "-TRL";
                        }
                    }
                    //Trường hợp nhấn tìm kiếm nhiều lần thì clear datagridView dgvTraLai
                    if (dgvTraLai.RowCount > 0)
                    {
                        dgvTraLai.Rows.Clear();
                        dgvTraLai.DataSource = null;
                        dgvTraLai.Refresh();
                        SUM_SOLUONG_BAN_TRALAI(dgvTraLai);
                        SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai);
                    }
                }
                catch
                {
                    MessageBox.Show("THÔNG BÁO ! XẢY RA LỖI KHI TÌM KIẾM !");
                }
            }
        }
        //txtKeySearch_KeyDown == txtKeySearch_Validating
        private void txtKeySearch_Validating(object sender, CancelEventArgs e)
        {
            if (txtKeySearch.Text.Length > 3)
            {
                //SỰ KIỆN NHẤN ENTER TÌM KIẾM GIAO DỊCH
                if (Config.CheckConnectToServer()) // CÓ MẠNG INTERNET
                {
                    List<GIAODICH_DTO> listData = TIMKIEM_GIAODICHQUAY(txtKeySearch.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                    if (listData.Count > 0 && listData.Count == 1)
                    {
                        //TÌM ĐÚNG MÃ GIAO DỊCH THÌ BINDING DỮ LIỆU VÀO GRIDVIEW
                        BINDING_DATA_TO_DATAGRIDVIEW(listData[0].MA_GIAODICH.Trim());
                        txtTraLai_MaGiaoDich.Text = listData[0].MA_GIAODICH.Trim() + "-TRL";
                    }
                    else
                    {
                        FrmTimKiemGiaoDich frmSearch = new FrmTimKiemGiaoDich(txtKeySearch.Text.Trim(),
                            dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                        frmSearch.SetHanlerGiaoDichSearch(LoadDataSearch);
                        frmSearch.ShowDialog();
                    }
                }
                else // MẤT MẠNG INTERNET
                {
                    List<GIAODICH_DTO> listData = TIMKIEM_GIAODICHQUAY_FROM_SQL(txtKeySearch.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                    if (listData.Count > 0 && listData.Count == 1)
                    {
                        //TÌM ĐÚNG MÃ GIAO DỊCH THÌ BINDING DỮ LIỆU VÀO GRIDVIEW
                        BINDING_DATA_TO_DATAGRIDVIEW(listData[0].MA_GIAODICH);
                        txtTraLai_MaGiaoDich.Text = listData[0].MA_GIAODICH.Trim() + "-TRL";
                    }
                }
                //Trường hợp nhấn tìm kiếm nhiều lần thì clear datagridView dgvTraLai
                if (dgvTraLai.RowCount > 0)
                {
                    dgvTraLai.Rows.Clear();
                    dgvTraLai.DataSource = null;
                    dgvTraLai.Refresh();
                    SUM_SOLUONG_BAN_TRALAI(dgvTraLai);
                    SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai);
                }
            }
        }

        private void txtTraLai_SoLuong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //index dòng đang chọn trên datagridview bán trả lại
                if (currentRowDgvTraLai != -1 && dgvTraLai.RowCount > 0)
                {
                    decimal SoLuongNhapVao = 0;
                    decimal SoLuongGioiHanNhap = 0;
                    decimal.TryParse(txtTraLai_SoLuong.Text, out SoLuongNhapVao);
                    string MaVatTuDangChon = dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                    if (!string.IsNullOrEmpty(MaVatTuDangChon) && dgvDetails_Tab.RowCount > 0)
                    {
                        foreach (DataGridViewRow rowCheck in dgvDetails_Tab.Rows)
                        {
                            string code = rowCheck.Cells["MAHANG"].Value.ToString();
                            if (!string.IsNullOrEmpty(code) && code.Trim().ToUpper() == MaVatTuDangChon.Trim().ToUpper())
                            {
                                decimal.TryParse(dgvDetails_Tab.Rows[rowCheck.Index].Cells["SOLUONG"].Value.ToString(), out SoLuongGioiHanNhap);
                            }
                        }
                    }
                    if (SoLuongNhapVao > SoLuongGioiHanNhap)
                    {
                        NotificationLauncher.ShowNotificationWarning("Cảnh báo", "Quá số lượng cho phép", 1, "0x1", "0x8", "normal");
                    }
                    else
                    {
                        decimal DONGIA = 0; decimal.TryParse(dgvTraLai.Rows[currentRowDgvTraLai].Cells["GIABANLE_VAT_TRALAI"].Value.ToString(), out DONGIA);
                        dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value = FormatCurrency.FormatMoney(SoLuongNhapVao);
                        dgvTraLai.Rows[currentRowDgvTraLai].Cells["THANHTIEN_TRALAI"].Value = FormatCurrency.FormatMoney(DONGIA * SoLuongNhapVao);
                        txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                        txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                    }
                }
            }
        }

        private void btnF2ThemMoiTraLai_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTraLai_MaGiaoDich.Text.Trim()) && dgvTraLai.RowCount == 0 && dgvDetails_Tab.RowCount == 0)
            {
                btnF2ThemMoiTraLai.Enabled = false;
                txtKeySearch.Enabled = true;
                txtKeySearch.Focus();
                dateTimeTuNgay.Enabled = true;
                dateTimeDenNgay.Enabled = true;
                btnTimKiem.Enabled = true;

            }

            if (btnF2ThemMoiTraLai.Enabled && dgvTraLai.RowCount > 0)
            {
                txtKeySearch.Text = "";
                txtChiTiet_TienChietKhau.Text = "";
                txtChiTiet_ThoiGian.Text = "";
                txtChiTiet_ThuNgan.Text = "";
                txtChiTiet_TienChietKhau.Text = "";
                txtChiTiet_TienTraLai.Text = "";
                txtChiTiet_TongTien.Text = "";
                txtTraLai_SoLuong.Text = "";
                txtTraLai_SoLuong.Text = "";
                txtTraLai_MaGiaoDich.Text = "";
                txtTraLai_ThuNgan.Text = "";
                txtChiTiet_TongSoLuong.Text = "";
                txtTraLai_TongSoLuong.Text = "";
                txtTraLai_MaVatTu.Text = "";
                txtTraLai_TongTien.Text = "";
                if (dgvDetails_Tab.RowCount > 0)
                {
                    dgvDetails_Tab.Rows.Clear();
                    dgvDetails_Tab.DataSource = null;
                    dgvDetails_Tab.Refresh();
                }
                if (dgvTraLai.RowCount > 0)
                {
                    dgvTraLai.Rows.Clear();
                    dgvTraLai.DataSource = null;
                    dgvTraLai.Refresh();
                    SUM_SOLUONG_BAN_TRALAI(dgvTraLai);
                    SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai);
                }
            }
        }
    }
}
