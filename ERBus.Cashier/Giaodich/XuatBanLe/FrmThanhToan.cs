using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ERBus.Cashier.Common;
using ERBus.Cashier.Danhmuc;
using ERBus.Cashier.Dto;
using Oracle.ManagedDataAccess.Client;
using System.Linq;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public delegate void STATUS_TIMKIEM_KHACHHANG(KHACHHANG_DTO _KHACHHANG_DTO);
    public delegate void STATUS_THEMMOI_KHACHHANG(KHACHHANG_DTO _KHACHHANG_DTO);
    public partial class FrmThanhToan : Form
    {
        public STATUS_THEMMOI_KHACHHANG _STATUS_THEMMOI_KHACHHANG;
        public XuLyThanhToan handler;
        private EnumCommon.LOAI_GIAODICH LOAI_GIAODICH_GLOBAL;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        private bool LOG_THANHTOAN = false;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        //FORM THANH TOÁN BÁN LẺ
        GIAODICH_DTO _GIAODICH_DTO_GLOBAL = new GIAODICH_DTO();
        GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new GIAODICH_DTO();
        GIAODICH_DTO _GIAODICH_DTO_CLONE = new GIAODICH_DTO();
        GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_BILL_CLONE = new GIAODICH_DTO();

        decimal GLOBAL_TONGTIEN_HOADON_BANDAU = new decimal();
        public FrmThanhToan(GIAODICH_DTO GIAODICH_DTO, GIAODICH_DTO NVGDQUAY_ASYNCCLIENT_BILL, EnumCommon.LOAI_GIAODICH loaiGiaoDich, decimal TONGTIEN_HOADON_BANDAU)
        {
            InitializeComponent();
            LOAI_GIAODICH_GLOBAL = loaiGiaoDich;
            int _currentUcFrame = FrmMain._currentUcFrame;
            this.Text = "THANH TOÁN HÓA ĐƠN " + (_currentUcFrame + 1);
            txtThanhToan_MaGiaoDich.Text = UC_Frame_BanLe.THANHTOAN_MAGIAODICH;
            txtThanhToan_TienThanhToan.Text = FormatCurrency.FormatMoney(UC_Frame_BanLe.THANHTOAN_TONGTIEN_THANHTOAN);
            txtThanhToan_TienMat.Text = FormatCurrency.FormatMoney(UC_Frame_BanLe.THANHTOAN_TONGTIEN_THANHTOAN);
            txtThanhToan_TienQuyDoiDiem.Text = "0";
            txtThanhToan_TienPhaiTra.Text = FormatCurrency.FormatMoney(UC_Frame_BanLe.THANHTOAN_TONGTIEN_THANHTOAN); ;
            txtThanhToan_TienMat.Focus();
            this.ActiveControl = txtThanhToan_TienMat;
            txtThanhToan_TienMat.SelectAll();
            _GIAODICH_DTO_GLOBAL = GIAODICH_DTO;
            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = NVGDQUAY_ASYNCCLIENT_BILL;
            GLOBAL_TONGTIEN_HOADON_BANDAU = TONGTIEN_HOADON_BANDAU;
            txtThanhToan_MaKhachHang.ReadOnly = true;
            btnF10_QuyDoi.Visible = false;
            btnF10_QuyDoi.Enabled = false;
            txtThanhToan_QuyDoi_ToiDa.Visible = false;
            _GIAODICH_DTO_CLONE = new GIAODICH_DTO();
            _GIAODICH_DTO_CLONE = ObjectCopier.Clone(GIAODICH_DTO);

            _NVGDQUAY_ASYNCCLIENT_BILL_CLONE = new GIAODICH_DTO();
            _NVGDQUAY_ASYNCCLIENT_BILL_CLONE = ObjectCopier.Clone(NVGDQUAY_ASYNCCLIENT_BILL);
        }
        public void SetHanler(XuLyThanhToan xuLy)
        {
            this.handler = xuLy;
        }
        public void SETHANDLER_STATUS_THEMMOI_KHACHHANG(STATUS_THEMMOI_KHACHHANG _STATUS_THEMMOI_KHACHHANG)
        {
            this._STATUS_THEMMOI_KHACHHANG = _STATUS_THEMMOI_KHACHHANG;
        }
        private void btnThanhToan_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UPDATE_KHACHHANG_TO_ORACLE(string MAKHACHHANG, decimal QUYDOI_TIEN_THANH_DIEM, decimal TONGTIEN, string HANG_KHACHHANG)
        {
            if (!string.IsNullOrEmpty(MAKHACHHANG) && TONGTIEN > 0)
            {
                //trừ điểm hiện tại trước
                decimal SODIEM_MOI = 0;
                SODIEM_MOI = _GIAODICH_DTO_GLOBAL.SODIEM - _GIAODICH_DTO_GLOBAL.DIEMQUYDOI;
                decimal DIEM_TICH_LUY = 0;
                if (QUYDOI_TIEN_THANH_DIEM != 0) DIEM_TICH_LUY = SODIEM_MOI + decimal.Round(TONGTIEN / QUYDOI_TIEN_THANH_DIEM, 2);
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryUpdate = "";
                                OracleCommand cmd = new OracleCommand();
                                cmd.Connection = connection;
                                if (string.IsNullOrEmpty(HANG_KHACHHANG) || HANG_KHACHHANG.Equals(""))
                                {
                                    queryUpdate = string.Format(@"UPDATE KHACHHANG SET MAHANG = '" + _GIAODICH_DTO_GLOBAL.HANGKHACHHANG_MOI + "' ,SODIEM = NVL(" + DIEM_TICH_LUY + ", 0), TONGTIEN = NVL(TONGTIEN, 0) + " + TONGTIEN + " WHERE MAKHACHHANG = '" + MAKHACHHANG + "'");
                                }
                                else
                                {
                                    queryUpdate = string.Format(@"UPDATE KHACHHANG SET SODIEM = NVL(" + DIEM_TICH_LUY + ", 0), TONGTIEN = NVL(TONGTIEN, 0) + " + TONGTIEN + " WHERE MAKHACHHANG = '" + MAKHACHHANG + "'");
                                }
                                cmd.CommandText = queryUpdate;
                                cmd.CommandType = CommandType.Text;
                                int count = cmd.ExecuteNonQuery();
                                if (count > 0)
                                {
                                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.SODIEM = DIEM_TICH_LUY;
                                    decimal SOTIEN_LENHANG = 0;
                                    queryUpdate = string.Format(@"SELECT MAHANG,SOTIEN_LENHANG FROM (SELECT MAHANG,SOTIEN_LENHANG FROM HANGKHACHHANG WHERE SOTIEN_LENHANG > " + _GIAODICH_DTO_GLOBAL.SOTIEN_LENHANG + " ORDER BY SOTIEN_LENHANG ASC) WHERE ROWNUM = 1");
                                    cmd.CommandText = queryUpdate;
                                    cmd.CommandType = CommandType.Text;
                                    OracleDataReader dataReaderHangKhachHang = null;
                                    dataReaderHangKhachHang = cmd.ExecuteReader();
                                    if (dataReaderHangKhachHang.HasRows)
                                    {
                                        while (dataReaderHangKhachHang.Read())
                                        {
                                            if (dataReaderHangKhachHang["SOTIEN_LENHANG"] != null) decimal.TryParse(dataReaderHangKhachHang["SOTIEN_LENHANG"].ToString(), out SOTIEN_LENHANG);
                                            string MAHANGKH = dataReaderHangKhachHang["MAHANG"] != null ? dataReaderHangKhachHang["MAHANG"].ToString() : "";
                                            if (_GIAODICH_DTO_GLOBAL.TONGTIEN_KHACHHANG + TONGTIEN >= SOTIEN_LENHANG)
                                            {
                                                TANG_HANG_KHACHHANG(MAKHACHHANG, _GIAODICH_DTO_GLOBAL.HANGKHACHHANG_MOI, MAHANGKH, _GIAODICH_DTO_GLOBAL.MA_GIAODICH);
                                            }
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
                                UPDATE_KHACHHANG_TO_SQLSERVER(MAKHACHHANG, QUYDOI_TIEN_THANH_DIEM, TONGTIEN, HANG_KHACHHANG);
                                connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
        }
        private void UPDATE_KHACHHANG_TO_SQLSERVER(string MAKHACHHANG, decimal QUYDOI_TIEN_THANH_DIEM, decimal TONGTIEN, string HANG_KHACHHANG)
        {
            if (!string.IsNullOrEmpty(MAKHACHHANG) && TONGTIEN > 0)
            {
                //trừ điểm hiện tại trước
                decimal SODIEM_MOI = 0;
                SODIEM_MOI = _GIAODICH_DTO_GLOBAL.SODIEM - _GIAODICH_DTO_GLOBAL.DIEMQUYDOI;
                decimal DIEM_TICH_LUY = 0;
                if (QUYDOI_TIEN_THANH_DIEM != 0) DIEM_TICH_LUY = SODIEM_MOI + decimal.Round(TONGTIEN / QUYDOI_TIEN_THANH_DIEM, 2);
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryUpdate = "";
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = connection;
                                if (string.IsNullOrEmpty(HANG_KHACHHANG) || HANG_KHACHHANG.Equals(""))
                                {
                                    queryUpdate = string.Format(@"UPDATE dbo.KHACHHANG SET MAHANG = '" + _GIAODICH_DTO_GLOBAL.HANGKHACHHANG_MOI + "' ,SODIEM = ISNULL(@SODIEM, 0), TONGTIEN = ISNULL(TONGTIEN, 0) + " + TONGTIEN + " WHERE MAKHACHHANG = @MAKHACHHANG");
                                }
                                else
                                {
                                    queryUpdate = string.Format(@"UPDATE dbo.KHACHHANG SET SODIEM = ISNULL(@SODIEM, 0), TONGTIEN = ISNULL(TONGTIEN, 0) + @TONGTIEN WHERE MAKHACHHANG = @MAKHACHHANG");
                                }
                                cmd.CommandText = queryUpdate;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add("MAKHACHHANG", SqlDbType.NVarChar, 50).Value = MAKHACHHANG;
                                cmd.Parameters.Add("SODIEM", SqlDbType.Decimal).Value = DIEM_TICH_LUY;
                                cmd.Parameters.Add("TONGTIEN", SqlDbType.Decimal).Value = TONGTIEN;
                                int count = cmd.ExecuteNonQuery();
                                if (count > 0)
                                {
                                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.SODIEM = DIEM_TICH_LUY;
                                };
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
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
        }


        private void TANG_HANG_KHACHHANG(string MAKHACHHANG, string HANG_HIENTAI, string HANG_MOI, string MA_GIAODICH)
        {
            if (!string.IsNullOrEmpty(MAKHACHHANG) && !string.IsNullOrEmpty(HANG_MOI))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryUpdate = "";
                                if (!string.IsNullOrEmpty(HANG_MOI))
                                {
                                    OracleCommand cmdUpdateHangKhachHang = new OracleCommand();
                                    cmdUpdateHangKhachHang.Connection = connection;
                                    queryUpdate = string.Format(@"UPDATE KHACHHANG SET MAHANG = :HANGKHACHHANG_NEW WHERE MAKHACHHANG = :MAKHACHHANG AND UNITCODE = :UNITCODE");
                                    cmdUpdateHangKhachHang.CommandText = queryUpdate;
                                    cmdUpdateHangKhachHang.CommandType = CommandType.Text;
                                    cmdUpdateHangKhachHang.Parameters.Clear();
                                    cmdUpdateHangKhachHang.Parameters.Add("HANGKHACHHANG_NEW", OracleDbType.NVarchar2, 50).Value = HANG_MOI;
                                    cmdUpdateHangKhachHang.Parameters.Add("MAKHACHHANG", OracleDbType.NVarchar2, 50).Value = MAKHACHHANG;
                                    cmdUpdateHangKhachHang.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                    int count = cmdUpdateHangKhachHang.ExecuteNonQuery();
                                    if (count > 0)
                                    {
                                        //LƯU LỊCH SỬ TĂNG HẠNG
                                        cmdUpdateHangKhachHang.Parameters.Clear();
                                        queryUpdate = string.Format(@"INSERT INTO LICHSU_TANGHANG(ID,MAKHACHHANG,MAHANG_CU,MAHANG_MOI,NGAY_LENHANG,THOIGIAN_LENHANG,MA_GIAODICH_LENHANG) VALUES (@ID,@MAKHACHHANG,@MAHANG_CU,@MAHANG_MOI,@NGAY_LENHANG,@THOIGIAN_LENHANG,@MA_GIAODICH_LENHANG)");
                                        cmdUpdateHangKhachHang.CommandText = queryUpdate;
                                        cmdUpdateHangKhachHang.CommandType = CommandType.Text;
                                        cmdUpdateHangKhachHang.Parameters.Clear();
                                        cmdUpdateHangKhachHang.Parameters.Add("ID", OracleDbType.Varchar2, 50).Value = Guid.NewGuid().ToString();
                                        cmdUpdateHangKhachHang.Parameters.Add("MAKHACHHANG", OracleDbType.Varchar2, 50).Value = MAKHACHHANG;
                                        cmdUpdateHangKhachHang.Parameters.Add("MAHANG_CU", OracleDbType.Varchar2, 50).Value = HANG_HIENTAI;
                                        cmdUpdateHangKhachHang.Parameters.Add("MAHANG_MOI", OracleDbType.Varchar2, 50).Value = HANG_MOI;
                                        cmdUpdateHangKhachHang.Parameters.Add("NGAY_LENHANG", OracleDbType.Date).Value = DateTime.Now;
                                        cmdUpdateHangKhachHang.Parameters.Add("THOIGIAN_LENHANG", OracleDbType.Varchar2, 50).Value = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" +DateTime.Now.Second;
                                        cmdUpdateHangKhachHang.Parameters.Add("MA_GIAODICH_LENHANG", OracleDbType.Varchar2, 70).Value = MA_GIAODICH;
                                        NotificationLauncher.ShowNotification("Thông báo lên hạng", "Chúc mừng khách hàng đã được lên hạng mới", 1, "0x1", "0x8", "normal");
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
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
        }
        /// <summary>
        /// Lưu dữ liệu bán lẻ
        /// </summary>
        /// <param name="header"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        private int SAVE_DATA_TO_ORACLE(GIAODICH_DTO _GIAODICH_DTO)
        {
            int countCheckInsertSuccess = 0;
            if (_GIAODICH_DTO != null)
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryInsert = "";
                                OracleCommand cmd = new OracleCommand();
                                cmd.Connection = connection;
                                queryInsert =
                                    string.Format(@"INSERT INTO GIAODICH (ID,MA_GIAODICH,LOAI_GIAODICH,NGAY_GIAODICH,MAKHACHHANG,THOIGIAN_TAO,TIENKHACH_TRA,TIEN_TRALAI_KHACH,
                                    MAKHO_XUAT,MA_VOUCHER,I_CREATE_DATE,I_CREATE_BY,I_STATE,UNITCODE) VALUES (
                                    :ID,:MA_GIAODICH,:LOAI_GIAODICH,:NGAY_GIAODICH,:MAKHACHHANG,:THOIGIAN_TAO,
                                    :TIENKHACH_TRA,:TIEN_TRALAI_KHACH,:MAKHO_XUAT,:MA_VOUCHER,:I_CREATE_DATE,:I_CREATE_BY,:I_STATE,:UNITCODE)");
                                cmd.CommandText = queryInsert;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = _GIAODICH_DTO.ID;
                                cmd.Parameters.Add("MA_GIAODICH", OracleDbType.Varchar2, 70).Value = _GIAODICH_DTO.MA_GIAODICH;
                                cmd.Parameters.Add("LOAI_GIAODICH", OracleDbType.Varchar2, 10).Value = _GIAODICH_DTO.LOAI_GIAODICH;
                                cmd.Parameters.Add("NGAY_GIAODICH", OracleDbType.Date).Value = _GIAODICH_DTO.NGAY_GIAODICH;
                                cmd.Parameters.Add("MAKHACHHANG", OracleDbType.Varchar2, 50).Value = !string.IsNullOrEmpty(_GIAODICH_DTO.MAKHACHHANG) ? _GIAODICH_DTO.MAKHACHHANG : "KHACHLE";
                                cmd.Parameters.Add("THOIGIAN_TAO", OracleDbType.Varchar2, 12).Value = _GIAODICH_DTO.THOIGIAN_TAO;
                                cmd.Parameters.Add("TIENKHACH_TRA", OracleDbType.Decimal).Value = _GIAODICH_DTO.TIENKHACH_TRA != 0 ? _GIAODICH_DTO.TIENKHACH_TRA : _GIAODICH_DTO.THANHTIEN;
                                cmd.Parameters.Add("TIEN_TRALAI_KHACH", OracleDbType.Decimal).Value = _GIAODICH_DTO.TIEN_TRALAI_KHACH;
                                cmd.Parameters.Add("MAKHO_XUAT", OracleDbType.NVarchar2, 50).Value = !string.IsNullOrEmpty(_GIAODICH_DTO.MAKHO_XUAT) ? _GIAODICH_DTO.MAKHO_XUAT : Session.Session.CurrentWareHouse;
                                cmd.Parameters.Add("MA_VOUCHER", OracleDbType.NVarchar2, 50).Value = _GIAODICH_DTO.MA_VOUCHER;
                                cmd.Parameters.Add("I_CREATE_DATE", OracleDbType.Date).Value = _GIAODICH_DTO.I_CREATE_DATE;
                                cmd.Parameters.Add("I_CREATE_BY", OracleDbType.NVarchar2, 300).Value = _GIAODICH_DTO.I_CREATE_BY;
                                cmd.Parameters.Add("I_STATE", OracleDbType.NVarchar2, 50).Value = _GIAODICH_DTO.I_STATE;
                                cmd.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = _GIAODICH_DTO.UNITCODE;
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    countCheckInsertSuccess++;
                                    INSERT_DATA_HANG_GIAODICHQUAY(_GIAODICH_DTO, ref countCheckInsertSuccess);

                                }
                                //chạy store trừ xuất nhập tồn        
                                if (!string.IsNullOrEmpty(Session.Session.CurrentTableNamePeriod))
                                {
                                    OracleCommand cmdTruTon = new OracleCommand();
                                    cmdTruTon.Connection = connection;
                                    cmdTruTon.CommandText = @"ERBUS.XUATNHAPTON.XNT_GIAM_PHIEU";
                                    cmdTruTon.CommandType = CommandType.StoredProcedure;
                                    cmdTruTon.Parameters.Add("P_TABLENAME", OracleDbType.Varchar2, 50).Value = Session.Session.CurrentTableNamePeriod;
                                    cmdTruTon.Parameters.Add("P_NAM", OracleDbType.Varchar2, 50).Value = Session.Session.CurrentYear;
                                    cmdTruTon.Parameters.Add("P_KY", OracleDbType.Decimal).Value = decimal.Parse(Session.Session.CurrentPeriod);
                                    cmdTruTon.Parameters.Add("P_ID", OracleDbType.Varchar2, 50).Value = _GIAODICH_DTO.ID;
                                    cmdTruTon.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLogs.LogError(ex);
                            }
                            finally
                            {
                                SAVE_DATA_TO_SQL_HAVE_INTERNET(_GIAODICH_DTO);
                                connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
            return countCheckInsertSuccess;
        }
        private int SAVE_DATA_TO_SQL_HAVE_INTERNET(GIAODICH_DTO _GIAODICH_DTO)
        {
            int countCheckInsertSuccess = 0;
            if (_GIAODICH_DTO != null)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryInsert = "";
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = connection;
                                queryInsert = string.Format(@"INSERT INTO GIAODICH (ID,MA_GIAODICH,LOAI_GIAODICH,NGAY_GIAODICH,MAKHACHHANG,THOIGIAN_TAO,TIENKHACH_TRA,TIEN_TRALAI_KHACH,
                                    MAKHO_XUAT,MA_VOUCHER,I_CREATE_DATE,I_CREATE_BY,I_STATE,UNITCODE) VALUES (
                                    @ID,@MA_GIAODICH,@LOAI_GIAODICH,@NGAY_GIAODICH,@MAKHACHHANG,@THOIGIAN_TAO,
                                    @TIENKHACH_TRA,@TIEN_TRALAI_KHACH,@MAKHO_XUAT,@MA_VOUCHER,@I_CREATE_DATE,@I_CREATE_BY,@I_STATE,@UNITCODE)");
                                cmd.CommandText = queryInsert;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add("ID", SqlDbType.NVarChar, 50).Value = _GIAODICH_DTO.ID;
                                cmd.Parameters.Add("MA_GIAODICH", SqlDbType.NVarChar, 70).Value = _GIAODICH_DTO.MA_GIAODICH;
                                cmd.Parameters.Add("LOAI_GIAODICH", SqlDbType.VarChar, 10).Value = _GIAODICH_DTO.LOAI_GIAODICH;
                                cmd.Parameters.Add("NGAY_GIAODICH", SqlDbType.Date).Value = _GIAODICH_DTO.NGAY_GIAODICH;
                                cmd.Parameters.Add("MAKHACHHANG", SqlDbType.VarChar, 50).Value = !string.IsNullOrEmpty(_GIAODICH_DTO.MAKHACHHANG) ? _GIAODICH_DTO.MAKHACHHANG : "KHACHLE";
                                cmd.Parameters.Add("THOIGIAN_TAO", SqlDbType.VarChar, 12).Value = _GIAODICH_DTO.THOIGIAN_TAO;
                                cmd.Parameters.Add("TIENKHACH_TRA", SqlDbType.Decimal).Value = _GIAODICH_DTO.TIENKHACH_TRA != 0 ? _GIAODICH_DTO.TIENKHACH_TRA : _GIAODICH_DTO.THANHTIEN;
                                cmd.Parameters.Add("TIEN_TRALAI_KHACH", SqlDbType.Decimal).Value = _GIAODICH_DTO.TIEN_TRALAI_KHACH;
                                cmd.Parameters.Add("MAKHO_XUAT", SqlDbType.VarChar, 50).Value = !string.IsNullOrEmpty(_GIAODICH_DTO.MAKHO_XUAT) ? _GIAODICH_DTO.MAKHO_XUAT : Session.Session.CurrentWareHouse;
                                cmd.Parameters.Add("MA_VOUCHER", SqlDbType.VarChar, 50).Value = _GIAODICH_DTO.MA_VOUCHER;
                                cmd.Parameters.Add("I_CREATE_DATE", SqlDbType.Date).Value = _GIAODICH_DTO.I_CREATE_DATE;
                                cmd.Parameters.Add("I_CREATE_BY", SqlDbType.VarChar, 50).Value = _GIAODICH_DTO.I_CREATE_BY;
                                cmd.Parameters.Add("I_STATE", SqlDbType.VarChar, 1).Value = _GIAODICH_DTO.I_STATE;
                                cmd.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = _GIAODICH_DTO.UNITCODE;
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    countCheckInsertSuccess++;
                                    INSERT_DATA_HANG_GIAODICHQUAY_SQL(_GIAODICH_DTO, ref countCheckInsertSuccess);
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
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
            return countCheckInsertSuccess;
        }



        private int SAVE_DATA_TO_SQL(GIAODICH_DTO _GIAODICH_DTO)
        {
            int countCheckInsertSuccess = 0;
            if (_GIAODICH_DTO != null)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryInsert = "";
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = connection;
                                queryInsert = string.Format(@"INSERT INTO GIAODICH (ID,MA_GIAODICH,LOAI_GIAODICH,NGAY_GIAODICH,MAKHACHHANG,THOIGIAN_TAO,TIENKHACH_TRA,TIEN_TRALAI_KHACH,
                                    MAKHO_XUAT,MA_VOUCHER,I_CREATE_DATE,I_CREATE_BY,I_STATE,UNITCODE) VALUES (
                                    @ID,@MA_GIAODICH,@LOAI_GIAODICH,@NGAY_GIAODICH,@MAKHACHHANG,@THOIGIAN_TAO,
                                    @TIENKHACH_TRA,@TIEN_TRALAI_KHACH,@MAKHO_XUAT,@MA_VOUCHER,@I_CREATE_DATE,@I_CREATE_BY,@I_STATE,@UNITCODE)");
                                cmd.CommandText = queryInsert;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add("ID", SqlDbType.NVarChar, 50).Value = _GIAODICH_DTO.ID;
                                cmd.Parameters.Add("MA_GIAODICH", SqlDbType.NVarChar, 70).Value = _GIAODICH_DTO.MA_GIAODICH;
                                cmd.Parameters.Add("LOAI_GIAODICH", SqlDbType.VarChar, 10).Value = _GIAODICH_DTO.LOAI_GIAODICH;
                                cmd.Parameters.Add("NGAY_GIAODICH", SqlDbType.Date).Value = _GIAODICH_DTO.NGAY_GIAODICH;
                                cmd.Parameters.Add("MAKHACHHANG", SqlDbType.VarChar, 50).Value = !string.IsNullOrEmpty(_GIAODICH_DTO.MAKHACHHANG) ? _GIAODICH_DTO.MAKHACHHANG : "KHACHLE";
                                cmd.Parameters.Add("THOIGIAN_TAO", SqlDbType.VarChar, 12).Value = _GIAODICH_DTO.THOIGIAN_TAO;
                                cmd.Parameters.Add("TIENKHACH_TRA", SqlDbType.Decimal).Value = _GIAODICH_DTO.TIENKHACH_TRA != 0 ? _GIAODICH_DTO.TIENKHACH_TRA : _GIAODICH_DTO.THANHTIEN;
                                cmd.Parameters.Add("TIEN_TRALAI_KHACH", SqlDbType.Decimal).Value = _GIAODICH_DTO.TIEN_TRALAI_KHACH;
                                cmd.Parameters.Add("MAKHO_XUAT", SqlDbType.VarChar, 50).Value = !string.IsNullOrEmpty(_GIAODICH_DTO.MAKHO_XUAT) ? _GIAODICH_DTO.MAKHO_XUAT : Session.Session.CurrentWareHouse;
                                cmd.Parameters.Add("MA_VOUCHER", SqlDbType.VarChar, 50).Value = _GIAODICH_DTO.MA_VOUCHER;
                                cmd.Parameters.Add("I_CREATE_DATE", SqlDbType.Date).Value = _GIAODICH_DTO.I_CREATE_DATE;
                                cmd.Parameters.Add("I_CREATE_BY", SqlDbType.VarChar, 50).Value = _GIAODICH_DTO.I_CREATE_BY;
                                cmd.Parameters.Add("I_STATE", SqlDbType.VarChar, 1).Value = _GIAODICH_DTO.I_STATE;
                                cmd.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = _GIAODICH_DTO.UNITCODE;
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    countCheckInsertSuccess++;
                                    INSERT_DATA_HANG_GIAODICHQUAY_SQL(_GIAODICH_DTO, ref countCheckInsertSuccess);

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
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
            return countCheckInsertSuccess;
        }

        private void LUU_DULIEU(GIAODICH_DTO _GIAODICH_DTO)
        {
            try
            {
                int countSave = 0;
                if (Config.CheckConnectToServer())
                {
                    countSave = SAVE_DATA_TO_ORACLE(_GIAODICH_DTO);
                }
                else
                {
                    countSave = SAVE_DATA_TO_SQL(_GIAODICH_DTO);
                }
                if (countSave >= 2)
                {
                    NotificationLauncher.ShowNotification("Thông báo", "Hoàn thành giao dịch", 1, "0x1", "0x8", "normal");
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
            }
            ////UPDATE KHÁCH HÀNG
            if (!string.IsNullOrEmpty(_GIAODICH_DTO_GLOBAL.MAKHACHHANG))
            {
                try
                {
                    if (Config.CheckConnectToServer())
                    {
                        UPDATE_KHACHHANG_TO_ORACLE(_GIAODICH_DTO_GLOBAL.MAKHACHHANG, _GIAODICH_DTO_GLOBAL.QUYDOITIEN_THANH_DIEM, _GIAODICH_DTO.THANHTIEN, _GIAODICH_DTO_GLOBAL.HANGKHACHHANG);
                    }
                    else
                    {
                        UPDATE_KHACHHANG_TO_SQLSERVER(_GIAODICH_DTO_GLOBAL.MAKHACHHANG, _GIAODICH_DTO_GLOBAL.QUYDOITIEN_THANH_DIEM, _GIAODICH_DTO.THANHTIEN, _GIAODICH_DTO_GLOBAL.HANGKHACHHANG);
                    }
                }
                catch { }
            }
        }
        private HANGKHACHHANG_DTO TINHTOAN_DULIEU_HANGKHACHHANG(string MaHangKhachHang)
        {
            HANGKHACHHANG_DTO _HANGKHACHHANG_DTO = new HANGKHACHHANG_DTO();
            try
            {
                if (string.IsNullOrEmpty(MaHangKhachHang) || MaHangKhachHang.Equals(""))
                {
                    if (Config.CheckConnectToServer())
                    {
                        _HANGKHACHHANG_DTO = FrmThanhToanService.LAY_QUYDOI_TIEN_THANH_DIEM_HANGKHACHHANG_KHOIDAU_FROM_ORACLE();
                    }
                    else
                    {
                        _HANGKHACHHANG_DTO = FrmThanhToanService.LAY_QUYDOI_TIEN_THANH_DIEM_HANGKHACHHANG_KHOIDAU_FROM_SQLSERVER();
                    }
                }
                else
                {
                    if (Config.CheckConnectToServer())
                    {
                        _HANGKHACHHANG_DTO = FrmThanhToanService.LAY_QUYDOI_THEOHANGKH_FROM_ORACLE(MaHangKhachHang.Trim());
                    }
                    else
                    {
                        _HANGKHACHHANG_DTO = FrmThanhToanService.LAY_QUYDOI_THEOHANGKH_FROM_SQLSERVER(MaHangKhachHang.Trim());
                    }
                }
            }
            catch
            {
            }
            return _HANGKHACHHANG_DTO;
        }
        #region Tính toán trừ trực tiếp tiền trên hóa đơn theo hạng khách hàng - Tạm thời comment phòng sau này sử dụng
        //private decimal TINHTOAN_DULIEU_CHIETKHAU_HANGKHACHHANG(string MaKhachHang)
        //{
        //    decimal THANHTIEN_BANDAU = GLOBAL_TONGTIEN_HOADON_BANDAU;
        //    KHACHHANG_GIAMGIA_DTO _KHACHHANG_GIAMGIA_DTO = new KHACHHANG_GIAMGIA_DTO();
        //    _KHACHHANG_GIAMGIA_DTO = LAY_HANG_KHACHHANG_FROM_DMKHACHHANG(MaKhachHang);
        //    if (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA >= 0)
        //    {
        //        _GIAODICH_DTO_GLOBAL.TIENTHE = THANHTIEN_BANDAU * _KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100;
        //        _GIAODICH_DTO_GLOBAL.THANHTIEN = THANHTIEN_BANDAU * (1 - (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100));
        //        _GIAODICH_DTO_GLOBAL.TIENMAT = _GIAODICH_DTO_GLOBAL.THANHTIEN;
        //        _GIAODICH_DTO_GLOBAL.MAKHACHHANG = MaKhachHang;
        //        //KHỞI TẠO DỮ LIỆU BILL
        //        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENTHE = _GIAODICH_DTO_GLOBAL.TIENTHE;
        //        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.THANHTIEN = _GIAODICH_DTO_GLOBAL.THANHTIEN;
        //        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENMAT = _GIAODICH_DTO_GLOBAL.THANHTIEN;
        //        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG = MaKhachHang;
        //        //
        //        txtThanhToan_TienThanhToan.Text = FormatCurrency.FormatMoney(_GIAODICH_DTO_GLOBAL.THANHTIEN);
        //        txtTienThanhToanCoChietKhau.Visible = true;
        //        txtTienThanhToanCoChietKhau.Enabled = true;
        //        txtTienThanhToanCoChietKhau.Text = FormatCurrency.FormatMoney(THANHTIEN_BANDAU) + " CHIẾT KHẤU:" + _KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA + "%" + " CÒN:" + FormatCurrency.FormatMoney(_GIAODICH_DTO_GLOBAL.THANHTIEN);
        //        if (!string.IsNullOrEmpty(txtThanhToan_TienMat.Text) && !string.IsNullOrEmpty(txtThanhToan_TienTraLai.Text))
        //        {
        //            decimal THANHTIEN = 0;
        //            decimal.TryParse(txtThanhToan_TienThanhToan.Text, out THANHTIEN);
        //            txtThanhToan_TienMat.Text = FormatCurrency.FormatMoney(THANHTIEN);
        //            txtThanhToan_TienTraLai.Text = "0";

        //        }
        //        if (_GIAODICH_DTO_GLOBAL.LST_DETAILS.Count > 0)
        //        {
        //            foreach (GIAODICH_CHITIET _GIAODICH_CHITIET in _GIAODICH_DTO_GLOBAL.LST_DETAILS)
        //            {
        //                decimal THANHTIEN_HANG = _GIAODICH_DTO_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAHANG == _GIAODICH_CHITIET.MAHANG && x.MABOPK == _GIAODICH_CHITIET.MABOPK).THANHTIEN;
        //                decimal TIENCHIETKHAU_HANG = _GIAODICH_DTO_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAHANG == _GIAODICH_CHITIET.MAHANG && x.MABOPK == _GIAODICH_CHITIET.MABOPK).TIENCHIETKHAU;
        //                decimal TYLECHIETKHAU_HANG = _GIAODICH_DTO_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAHANG == _GIAODICH_CHITIET.MAHANG && x.MABOPK == _GIAODICH_CHITIET.MABOPK).TYLECHIETKHAU;
        //                _GIAODICH_CHITIET.LOAIKHUYENMAI = "THE";
        //                _GIAODICH_CHITIET.MACHUONGTRINHKM = "THE";
        //                decimal TIENCHIETKHAU_THE = THANHTIEN_HANG * (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100);
        //                _GIAODICH_CHITIET.TIENCHIETKHAU = TIENCHIETKHAU_HANG + TIENCHIETKHAU_THE;
        //                _GIAODICH_CHITIET.TYLECHIETKHAU = TYLECHIETKHAU_HANG + _KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA;
        //                _GIAODICH_CHITIET.THANHTIEN = THANHTIEN_HANG * (1 - (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100));
        //                _GIAODICH_CHITIET.MAKHACHHANG = MaKhachHang;
        //            }
        //        }
        //        else
        //        {
        //            string NOTIFICATION = string.Format(@"XẢY RA LỖI ! XIN THỰC HIỆN LẠI GIAO DỊCH NÀY");
        //            MessageBox.Show(NOTIFICATION);
        //        }
        //        if (_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS.Count > 0)
        //        {
        //            foreach (GIAODICH_CHITIET _GIAODICH_CHITIET in _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS)
        //            {
        //                decimal THANHTIEN_HANG = _NVGDQUAY_ASYNCCLIENT_BILL_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAHANG == _GIAODICH_CHITIET.MAHANG && x.MABOPK == _GIAODICH_CHITIET.MABOPK).THANHTIEN;
        //                decimal TIENCHIETKHAU_HANG = _NVGDQUAY_ASYNCCLIENT_BILL_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAHANG == _GIAODICH_CHITIET.MAHANG && x.MABOPK == _GIAODICH_CHITIET.MABOPK).TIENCHIETKHAU;
        //                decimal TYLECHIETKHAU_HANG = _NVGDQUAY_ASYNCCLIENT_BILL_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAHANG == _GIAODICH_CHITIET.MAHANG && x.MABOPK == _GIAODICH_CHITIET.MABOPK).TYLECHIETKHAU;
        //                _GIAODICH_CHITIET.LOAIKHUYENMAI = "THE";
        //                _GIAODICH_CHITIET.MACHUONGTRINHKM = "THE";
        //                decimal TIENCHIETKHAU_THE = THANHTIEN_HANG * (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100);
        //                _GIAODICH_CHITIET.TIENCHIETKHAU = TIENCHIETKHAU_HANG + TIENCHIETKHAU_THE;
        //                _GIAODICH_CHITIET.TYLECHIETKHAU = TYLECHIETKHAU_HANG + _KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA;
        //                _GIAODICH_CHITIET.THANHTIEN = THANHTIEN_HANG * (1 - (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100));
        //                _GIAODICH_CHITIET.MAKHACHHANG = MaKhachHang;
        //            }
        //        }
        //        //LOG_THANHTOAN
        //        LOG_THANHTOAN = true;
        //    }
        //    else
        //    {
        //        string NOTIFICATION = string.Format(@"KHÁCH HÀNG '{0}' CHƯA ĐỦ ĐIỀU KIỆN CHIẾT KHẤU", MaKhachHang);
        //        MessageBox.Show(NOTIFICATION);
        //        //LOG_THANHTOAN
        //        LOG_THANHTOAN = false;
        //    }
        //    return _KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA;
        //}
        #endregion

        private void THANHTOAN_HOADON_BANLE()
        {
            if (LOG_THANHTOAN)
            {
                LOG_THANHTOAN = false;
                txtThanhToan_TienMat.Focus();txtThanhToan_TienMat.SelectAll();
                return;
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(_GIAODICH_DTO_GLOBAL.MAKHACHHANG) && _GIAODICH_DTO_GLOBAL.TIENTHE > 0)
                    {
                        decimal TIEN_CHIADEU_MATHANG = 0;
                        if (_GIAODICH_DTO_GLOBAL.LST_DETAILS.Count > 0)
                            TIEN_CHIADEU_MATHANG = decimal.Round(_GIAODICH_DTO_GLOBAL.TIENTHE / _GIAODICH_DTO_GLOBAL.LST_DETAILS.Count, 2);
                        if (_GIAODICH_DTO_GLOBAL.LST_DETAILS.Count > 0)
                        {
                            foreach (GIAODICH_CHITIET row in _GIAODICH_DTO_GLOBAL.LST_DETAILS)
                            {
                                GIAODICH_CHITIET rowClone = _GIAODICH_DTO_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAHANG == row.MAHANG);
                                if (rowClone != null)
                                {
                                    decimal THANHTIEN_HANG = rowClone.THANHTIEN;
                                    if (row.SOLUONG > 0)
                                    {
                                        row.TIENTHE_VIP = decimal.Round(TIEN_CHIADEU_MATHANG, 2);
                                    }
                                    else
                                    {
                                        MessageBox.Show("CẢNH BÁO ! MÃ '" + rowClone.MAHANG + "', CÓ SỐ LƯỢNG = '0' ! XIN KIỂM TRA LẠI HOẶC GIỮ LẠI HÓA ĐƠN ĐỂ KIỂM TRA");
                                    }
                                    row.THANHTIEN = THANHTIEN_HANG - row.TIENTHE_VIP;
                                }
                            }
                        }
                        if (_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS.Count > 0)
                        {
                            decimal TIEN_CHIADEU_MATHANG_BILL = 0;
                            if (_GIAODICH_DTO_GLOBAL.TIENTHE > 0)
                            {
                                TIEN_CHIADEU_MATHANG_BILL = decimal.Round(_GIAODICH_DTO_GLOBAL.TIENTHE / _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS.Count, 2);
                            }
                            foreach (GIAODICH_CHITIET rowBill in _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS)
                            {
                                GIAODICH_CHITIET rowBillClone = _NVGDQUAY_ASYNCCLIENT_BILL_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAHANG == rowBill.MAHANG);
                                if (rowBillClone != null)
                                {
                                    decimal THANHTIEN_HANG = rowBillClone.THANHTIEN;
                                    if (rowBill.SOLUONG > 0)
                                    {
                                        rowBill.TIENTHE_VIP = decimal.Round(TIEN_CHIADEU_MATHANG_BILL, 2);
                                    }
                                    else
                                    {
                                        MessageBox.Show("CẢNH BÁO ! MÃ '" + rowBillClone.MAHANG + "', CÓ SỐ LƯỢNG = '0' ! XIN KIỂM TRA LẠI HOẶC GIỮ LẠI HÓA ĐƠN ĐỂ KIỂM TRA");
                                    }
                                    rowBill.THANHTIEN = THANHTIEN_HANG - rowBill.TIENTHE_VIP;
                                }
                            }
                        }
                    }
                    //LƯU DỮ LIỆU VÀO BẢNG
                    LUU_DULIEU(_GIAODICH_DTO_GLOBAL);
                }
                catch
                {
                    MessageBox.Show("CẢNH BÁO ! XẢY RA LỖI KHI LƯU HÓA ĐƠN NÀY, HÃY LƯU LẠI HÓA ĐƠN ĐỂ KIỂM TRA ! XIN CẢM ƠN ");
                }
                try
                {
                    //IN HÓA ĐƠN
                    THUCHIEN_IN_HOADON();
                }
                catch (Exception ex)
                {
                    WriteLogs.LogError(ex);
                }
            }
        }
        private void THUCHIEN_IN_HOADON()
        {
            string TONGTIEN_BANGCHU = ConvertSoThanhChu.ChuyenDoiSoThanhChu(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.THANHTIEN);
            string MA_TEN_KHACHHANG = "";
            
            if (Config.CheckConnectToServer())
            {
                MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_ORACLE(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG);
            }
            else
            {
                MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_SQLSERVER(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG);
            }
            using (frmPrintBill frm = new frmPrintBill())
            {
                try
                {
                    BILL_DTO infoBill = new BILL_DTO()
                    {
                        ADDRESS = Session.Session.CurrentAddress,
                        CONLAI = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIEN_TRALAI_KHACH,
                        PHONE = Session.Session.CurrentPhone,
                        MAKH = MA_TEN_KHACHHANG,
                        DIEM = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.SODIEM,
                        INFOTHUNGAN = "THU NGÂN: " + Session.Session.CurrentTenNhanVien,
                        MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MA_GIAODICH,
                        THANHTIENCHU = TONGTIEN_BANGCHU,
                        TIENKHACHTRA = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACH_TRA,
                        TENCUAHANG = Session.Session.CurrentNameStore
                    };
                    frm.PrintInvoice_BanLe(infoBill, _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL);
                }
                catch
                {
                }
                finally
                {
                    this.handler(true);
                    this.Dispose();
                    frm.Dispose();
                    frm.Refresh();
                }
            }
        }
        private const int WM_KEYDOWN = 256;
        private const int WM_KEYDOWN_2 = 260;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                if ((Keys)m.WParam == Keys.Escape)
                {
                    this.Close();
                    this.Dispose();
                    _GIAODICH_DTO_GLOBAL = new GIAODICH_DTO();
                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new GIAODICH_DTO();
                    GLOBAL_TONGTIEN_HOADON_BANDAU = new decimal();
                    LOG_THANHTOAN = false;
                }
                if ((Keys)m.WParam == Keys.F9)
                {
                    txtThanhToan_MaKhachHang.Focus();
                    txtThanhToan_MaKhachHang.ReadOnly = false;
                    txtThanhToan_MaKhachHang.Focus();
                    txtThanhToan_MaKhachHang.SelectAll();
                    txtThanhToan_TienQuyDoiDiem.Enabled = false;
                }
                if ((Keys)m.WParam == Keys.F8)
                {
                    FrmDmKhachHang frmDmKhachHang = new FrmDmKhachHang();
                    frmDmKhachHang.SET_HANDLER_STATUS_THEMMOI_KHACHHANG(BINDING_DATA_KHACHHANG_TO_THANHTOAN);
                    frmDmKhachHang.ShowDialog();
                }
            }
            if (m.Msg == WM_KEYDOWN_2)
            {
                if ((Keys)m.WParam == Keys.F10)
                {
                    if (btnF10_QuyDoi.Visible && btnF10_QuyDoi.Enabled)
                    {
                        txtThanhToan_TienQuyDoiDiem.Text = "";
                        txtThanhToan_TienQuyDoiDiem.Enabled = true;
                        txtThanhToan_TienQuyDoiDiem.Focus();
                        txtThanhToan_TienQuyDoiDiem.Focus();
                        txtThanhToan_TienQuyDoiDiem.SelectAll();
                    }
                }
            }
            return base.ProcessKeyPreview(ref m);
        }
        private void FrmThanhToan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
                _GIAODICH_DTO_GLOBAL = new GIAODICH_DTO();
                _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new GIAODICH_DTO();
                GLOBAL_TONGTIEN_HOADON_BANDAU = new decimal();
                LOG_THANHTOAN = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                decimal tienThanhToan = 0;
                decimal.TryParse(txtThanhToan_TienTraLai.Text.ToString(), out tienThanhToan);
                if (tienThanhToan >= 0)
                {
                    THANHTOAN_HOADON_BANLE();
                }
                else
                {
                    NotificationLauncher.ShowNotificationError("Thông báo", "Sai tiền !", 1, "0x1", "0x8", "normal");
                }
            }

            if (e.KeyCode == Keys.F9)
            {
                txtThanhToan_MaKhachHang.ReadOnly = false;
                txtThanhToan_MaKhachHang.Focus();
                txtThanhToan_TienQuyDoiDiem.Enabled = false;
            }
        }
        private void btnThanhToan_Save_Click(object sender, EventArgs e)
        {
            decimal tienThanhToan = 0;
            decimal.TryParse(txtThanhToan_TienTraLai.Text.ToString(), out tienThanhToan);
            if (tienThanhToan >= 0)
            {
                THANHTOAN_HOADON_BANLE();
            }
            else
            {
                NotificationLauncher.ShowNotificationError("Thông báo", "Sai tiền !", 1, "0x1", "0x8", "normal");
            }
        }
        private void txtThanhToan_TienMat_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (txtThanhToan_TienMat.Text.Trim().Length > 0)
            {
                decimal TIENMAT_KHACHTRA, TONGTIEN_HOADON, TIEN_TRALAI = 0;
                decimal.TryParse(txtThanhToan_TienMat.Text.Trim(), out TIENMAT_KHACHTRA);
                decimal.TryParse(txtThanhToan_TienPhaiTra.Text.Trim(), out TONGTIEN_HOADON);
                TIEN_TRALAI = TIENMAT_KHACHTRA - TONGTIEN_HOADON;
                if (TIEN_TRALAI >= 0 && TIEN_TRALAI < 1000000)
                {
                    txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(TIEN_TRALAI);
                    txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(TIEN_TRALAI);
                    btnThanhToan_Save.Focus();
                    _GIAODICH_DTO_GLOBAL.TIEN_TRALAI_KHACH = TIEN_TRALAI;
                    _GIAODICH_DTO_GLOBAL.TIENKHACH_TRA = TIENMAT_KHACHTRA;

                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIEN_TRALAI_KHACH = TIEN_TRALAI;
                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACH_TRA = TIENMAT_KHACHTRA;

                }
                else if (TIEN_TRALAI < 0)
                {
                    txtThanhToan_TienMat.SelectAll();
                }
                else
                {
                    DialogResult result = MessageBox.Show("SỐ TIỀN TRẢ LẠI QUÁ LỚN ?", "THAO TÁC SAI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (result == DialogResult.OK)
                    {
                        TIEN_TRALAI = TIEN_TRALAI / 10;
                        txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(TIEN_TRALAI);
                        txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(TIEN_TRALAI);
                        _GIAODICH_DTO_GLOBAL.TIEN_TRALAI_KHACH = TIEN_TRALAI;
                        _GIAODICH_DTO_GLOBAL.TIENKHACH_TRA = TIENMAT_KHACHTRA;

                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIEN_TRALAI_KHACH = TIEN_TRALAI;
                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACH_TRA = TIENMAT_KHACHTRA;
                    }
                }
            }
        }
        private void txtThanhToan_TienMat_KeyUp(object sender, KeyEventArgs e)
        {
            decimal TONGTIEN_KHACHTRA = 0; if (e.KeyCode != Keys.Enter)
            {
                if (!txtThanhToan_TienMat.Text.Equals(txtThanhToan_TienThanhToan.Text))
                {
                    string THANHTOAN_TIENMAT_STRING = txtThanhToan_TienMat.Text;
                    int start = txtThanhToan_TienMat.Text.Length - txtThanhToan_TienMat.SelectionStart;
                    THANHTOAN_TIENMAT_STRING = THANHTOAN_TIENMAT_STRING.Replace(",", "");
                    decimal THANHTOAN_TIENMAT = 0;
                    decimal.TryParse(THANHTOAN_TIENMAT_STRING, out THANHTOAN_TIENMAT);
                    TONGTIEN_KHACHTRA = THANHTOAN_TIENMAT;
                    decimal THANHTOAN_TONGTIEN = 0;
                    decimal.TryParse(txtThanhToan_TienPhaiTra.Text.Trim().Replace(",", ""), out THANHTOAN_TONGTIEN);
                    _GIAODICH_DTO_GLOBAL.TIENKHACH_TRA = THANHTOAN_TIENMAT;
                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACH_TRA = THANHTOAN_TIENMAT;
                    decimal THANHTOAN_TIEN_TRALAI_KHACH = THANHTOAN_TIENMAT - THANHTOAN_TONGTIEN;

                    txtThanhToan_TienMat.Text = FormatCurrency.FormatMoney(THANHTOAN_TIENMAT_STRING);
                    txtThanhToan_TienMat.SelectionStart = -start + txtThanhToan_TienMat.Text.Length;
                    if (THANHTOAN_TIEN_TRALAI_KHACH > 0)
                    {
                        txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(THANHTOAN_TIEN_TRALAI_KHACH);
                        txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(THANHTOAN_TIEN_TRALAI_KHACH);
                    }
                    else
                    {
                        txtThanhToan_TienTraLai.Text = "-" + FormatCurrency.FormatMoney(THANHTOAN_TIEN_TRALAI_KHACH);
                    }
                    _GIAODICH_DTO_GLOBAL.TIEN_TRALAI_KHACH = THANHTOAN_TIEN_TRALAI_KHACH;
                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIEN_TRALAI_KHACH = THANHTOAN_TIEN_TRALAI_KHACH;
                }
                else
                {
                    return;
                }
            }
            else
            {
                decimal tienThanhToan = 0;
                decimal.TryParse(txtThanhToan_TienTraLai.Text, out tienThanhToan);
                if (tienThanhToan >= 0)
                {
                    THANHTOAN_HOADON_BANLE();
                }
                else
                {
                    NotificationLauncher.ShowNotificationError("THÔNG BÁO", "SỐ TIỀN SAI !", 1, "0x1", "0x8", "normal");
                }
            }
        }

        private void INSERT_DATA_HANG_GIAODICHQUAY(GIAODICH_DTO _GIAODICH_DTO, ref int countCheckInsertSuccess)
        {
            if (_GIAODICH_DTO.LST_DETAILS.Count > 0)
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 1;
                            foreach (GIAODICH_CHITIET ITEM in _GIAODICH_DTO.LST_DETAILS)
                            {
                                OracleCommand command = new OracleCommand();
                                command.Connection = connection;
                                string queryInsertItem = string.Format(@"INSERT INTO GIAODICH_CHITIET (ID,MA_GIAODICH,MAHANG,MATHUE_RA,SOLUONG,GIABANLE_VAT,MA_KHUYENMAI,TYLE_KHUYENMAI,
                                TIEN_KHUYENMAI,TYLE_CHIETKHAU,TIEN_CHIETKHAU,TIENTHE_VIP,TIEN_VOUCHER,THANHTIEN,SAPXEP) VALUES (:ID,:MA_GIAODICH,:MAHANG,:MATHUE_RA,:SOLUONG,:GIABANLE_VAT,
                                :MA_KHUYENMAI,:TYLE_KHUYENMAI,:TIEN_KHUYENMAI,:TYLE_CHIETKHAU,:TIEN_CHIETKHAU,:TIENTHE_VIP,:TIEN_VOUCHER,:THANHTIEN,:SAPXEP)");
                                command.CommandText = queryInsertItem;
                                command.CommandType = CommandType.Text;
                                command.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = ITEM.ID;
                                command.Parameters.Add("MA_GIAODICH", OracleDbType.NVarchar2, 70).Value = ITEM.MA_GIAODICH;
                                command.Parameters.Add("MAHANG", OracleDbType.NVarchar2, 50).Value = ITEM.MAHANG;
                                command.Parameters.Add("MATHUE_RA", OracleDbType.NVarchar2, 50).Value = ITEM.MATHUE_RA;
                                command.Parameters.Add("SOLUONG", OracleDbType.Decimal).Value = ITEM.SOLUONG;
                                command.Parameters.Add("GIABANLE_VAT", OracleDbType.Decimal).Value = ITEM.GIABANLE_VAT;
                                command.Parameters.Add("MA_KHUYENMAI", OracleDbType.NVarchar2, 50).Value = ITEM.MA_KHUYENMAI;
                                command.Parameters.Add("TYLE_KHUYENMAI", OracleDbType.Decimal).Value = ITEM.TYLE_KHUYENMAI;
                                command.Parameters.Add("TIEN_KHUYENMAI", OracleDbType.Decimal).Value = ITEM.TIEN_KHUYENMAI;
                                command.Parameters.Add("TYLE_CHIETKHAU", OracleDbType.Decimal).Value = ITEM.TYLE_CHIETKHAU;
                                command.Parameters.Add("TIEN_CHIETKHAU", OracleDbType.Decimal).Value = ITEM.TIEN_CHIETKHAU;
                                command.Parameters.Add("TIENTHE_VIP", OracleDbType.Decimal).Value = ITEM.TIENTHE_VIP;
                                command.Parameters.Add("TIEN_VOUCHER", OracleDbType.Decimal).Value = ITEM.TIEN_VOUCHER;
                                command.Parameters.Add("THANHTIEN", OracleDbType.Decimal).Value = ITEM.THANHTIEN;
                                command.Parameters.Add("SAPXEP", OracleDbType.Int32).Value = i;
                                try
                                {
                                    if (command.ExecuteNonQuery() > 0) {
                                        countCheckInsertSuccess++;
                                        i++;
                                    } 
                                }
                                catch (Exception e)
                                {
                                    WriteLogs.LogError(e);
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
                        NotificationLauncher.ShowNotificationError("THÔNG BÁO", "KHÔNG CÓ KẾT NỐI VỚI CƠ SỞ DỮ LIỆU ORACLE", 1, "0x1", "0x8", "normal");
                    }
                }

            }
        }

        private void INSERT_DATA_HANG_GIAODICHQUAY_SQL(GIAODICH_DTO _GIAODICH_DTO, ref int countCheckInsertSuccess)
        {
            if (_GIAODICH_DTO.LST_DETAILS.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 1;
                            foreach (GIAODICH_CHITIET ITEM in _GIAODICH_DTO.LST_DETAILS)
                            {
                                SqlCommand command = new SqlCommand();
                                command.Connection = connection;
                                string queryInsertItem = string.Format(@"INSERT INTO GIAODICH_CHITIET (ID,MA_GIAODICH,MAHANG,MATHUE_RA,SOLUONG,GIABANLE_VAT,MA_KHUYENMAI,TYLE_KHUYENMAI,
                                TIEN_KHUYENMAI,TYLE_CHIETKHAU,TIEN_CHIETKHAU,TIENTHE_VIP,TIEN_VOUCHER,THANHTIEN,SAPXEP) VALUES (@ID,@MA_GIAODICH,@MAHANG,@MATHUE_RA,@SOLUONG,@GIABANLE_VAT,
                                @MA_KHUYENMAI,@TYLE_KHUYENMAI,@TIEN_KHUYENMAI,@TYLE_CHIETKHAU,@TIEN_CHIETKHAU,@TIENTHE_VIP,@TIEN_VOUCHER,@THANHTIEN,@SAPXEP)");
                                command.CommandText = queryInsertItem;
                                command.CommandType = CommandType.Text;
                                command.Parameters.Add("ID", SqlDbType.NVarChar, 50).Value = ITEM.ID;
                                command.Parameters.Add("MA_GIAODICH", SqlDbType.VarChar, 70).Value = ITEM.MA_GIAODICH;
                                command.Parameters.Add("MAHANG", SqlDbType.VarChar, 50).Value = ITEM.MAHANG;
                                command.Parameters.Add("MATHUE_RA", SqlDbType.VarChar, 50).Value = ITEM.MATHUE_RA;
                                command.Parameters.Add("SOLUONG", SqlDbType.Decimal).Value = ITEM.SOLUONG;
                                command.Parameters.Add("GIABANLE_VAT", SqlDbType.Decimal).Value = ITEM.GIABANLE_VAT;
                                command.Parameters.Add("MA_KHUYENMAI", SqlDbType.NVarChar, 50).Value = ITEM.MA_KHUYENMAI;
                                command.Parameters.Add("TYLE_KHUYENMAI", SqlDbType.Decimal).Value = ITEM.TYLE_KHUYENMAI;
                                command.Parameters.Add("TIEN_KHUYENMAI", SqlDbType.Decimal).Value = ITEM.TIEN_KHUYENMAI;
                                command.Parameters.Add("TYLE_CHIETKHAU", SqlDbType.Decimal).Value = ITEM.TYLE_CHIETKHAU;
                                command.Parameters.Add("TIEN_CHIETKHAU", SqlDbType.Decimal).Value = ITEM.TIEN_CHIETKHAU;
                                command.Parameters.Add("TIENTHE_VIP", SqlDbType.Decimal).Value = ITEM.TIENTHE_VIP;
                                command.Parameters.Add("TIEN_VOUCHER", SqlDbType.Decimal).Value = ITEM.TIEN_VOUCHER;
                                command.Parameters.Add("THANHTIEN", SqlDbType.Decimal).Value = ITEM.THANHTIEN;
                                command.Parameters.Add("SAPXEP", SqlDbType.Int).Value = i;
                                try
                                {
                                    if (command.ExecuteNonQuery() > 0)
                                    {
                                        countCheckInsertSuccess++;
                                        i++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    WriteLogs.LogError(e);
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
                        NotificationLauncher.ShowNotificationError("THÔNG BÁO", "KHÔNG CÓ KẾT NỐI VỚI CƠ SỞ DỮ LIỆU SQL", 1, "0x1", "0x8", "normal");
                    }
                }
            }
        }
        public void BINDING_DATA_CHANGE_FROM_GRID_TO_TEXT(KHACHHANG_DTO _KHACHHANG_DTO)
        {
            txtThanhToan_MaKhachHang.ReadOnly = false;
            txtThanhToan_MaKhachHang.Text = _KHACHHANG_DTO.MAKHACHHANG;

            txtThanhToan_TenKhachHang.Visible = true;
            txtThanhToan_TenKhachHang.Text = "TÊN: " + _KHACHHANG_DTO.TENKHACHHANG;

            txtThanhToan_SoDienThoai.Visible = true;
            txtThanhToan_SoDienThoai.Text = "SĐT: " + _KHACHHANG_DTO.DIENTHOAI;

            txtThanhToan_DiemTichLuy.Visible = true;
            txtThanhToan_DiemTichLuy.Text = "ĐIỂM: " + _KHACHHANG_DTO.SODIEM;

            txtThanhToan_DiaChi.Visible = true;
            txtThanhToan_DiaChi.Text = "Đ/C: " + _KHACHHANG_DTO.DIACHI;

            txtThanhToan_QuyDoi_ToiDa.Visible = true;
            _GIAODICH_DTO_GLOBAL.MAKHACHHANG = _KHACHHANG_DTO.MAKHACHHANG;
            _GIAODICH_DTO_GLOBAL.SODIEM = _KHACHHANG_DTO.SODIEM;
            _GIAODICH_DTO_GLOBAL.TONGTIEN_KHACHHANG = _KHACHHANG_DTO.TONGTIEN;
            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG = _KHACHHANG_DTO.MAKHACHHANG;
            _GIAODICH_DTO_GLOBAL.HANGKHACHHANG = _KHACHHANG_DTO.HANGKHACHHANG;
            HANGKHACHHANG_DTO _HANGKHACHHANG_DTO = new HANGKHACHHANG_DTO();
            if (!string.IsNullOrEmpty(_KHACHHANG_DTO.HANGKHACHHANG)) _HANGKHACHHANG_DTO = TINHTOAN_DULIEU_HANGKHACHHANG(_KHACHHANG_DTO.HANGKHACHHANG);
            if (!string.IsNullOrEmpty(_HANGKHACHHANG_DTO.MAHANG))
            {
                _GIAODICH_DTO_GLOBAL.QUYDOITIEN_THANH_DIEM = _HANGKHACHHANG_DTO.QUYDOITIEN_THANH_DIEM;
                _GIAODICH_DTO_GLOBAL.QUYDOIDIEM_THANH_TIEN = _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN;
                _GIAODICH_DTO_GLOBAL.HANGKHACHHANG_MOI = _HANGKHACHHANG_DTO.MAHANG;
                _GIAODICH_DTO_GLOBAL.SOTIEN_LENHANG = _HANGKHACHHANG_DTO.SOTIEN_LENHANG;
                decimal QUYDOI = decimal.Round(_KHACHHANG_DTO.SODIEM * _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN, 2);
                _GIAODICH_DTO_GLOBAL.QUYDOI_TOIDA = QUYDOI;
                txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : " + FormatCurrency.FormatMoney(QUYDOI) + " VNĐ";
                if (QUYDOI > 0)
                {
                    btnF10_QuyDoi.Visible = true;
                    btnF10_QuyDoi.Enabled = true;
                }
            }
            else
            {
                txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : 0 VNĐ";
                _GIAODICH_DTO_GLOBAL.HANGKHACHHANG = _HANGKHACHHANG_DTO.MAHANG;
                btnF10_QuyDoi.Visible = false;
                btnF10_QuyDoi.Enabled = false;
            }
            txtThanhToan_TienMat.Focus();
            txtThanhToan_TienMat.SelectAll();
            LOG_THANHTOAN = true;
        }

        public void BINDING_DATA_KHACHHANG_TO_THANHTOAN(KHACHHANG_DTO _KHACHHANG_DTO)
        {
            txtThanhToan_MaKhachHang.ReadOnly = false;
            txtThanhToan_MaKhachHang.Text = _KHACHHANG_DTO.MAKHACHHANG;

            txtThanhToan_TenKhachHang.Visible = true;
            txtThanhToan_TenKhachHang.Text = "TÊN: " + _KHACHHANG_DTO.TENKHACHHANG;

            txtThanhToan_SoDienThoai.Visible = true;
            txtThanhToan_SoDienThoai.Text = "SĐT: " + _KHACHHANG_DTO.DIENTHOAI;

            txtThanhToan_DiemTichLuy.Visible = true;
            txtThanhToan_DiemTichLuy.Text = "ĐIỂM: " + _KHACHHANG_DTO.SODIEM;

            txtThanhToan_DiaChi.Visible = true;
            txtThanhToan_DiaChi.Text = "Đ/C: " + _KHACHHANG_DTO.DIACHI;

            txtThanhToan_QuyDoi_ToiDa.Visible = true;
            _GIAODICH_DTO_GLOBAL.MAKHACHHANG = _KHACHHANG_DTO.MAKHACHHANG;
            _GIAODICH_DTO_GLOBAL.SODIEM = _KHACHHANG_DTO.SODIEM;
            _GIAODICH_DTO_GLOBAL.TONGTIEN_KHACHHANG = _KHACHHANG_DTO.TONGTIEN;
            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG = _KHACHHANG_DTO.MAKHACHHANG;
            _GIAODICH_DTO_GLOBAL.HANGKHACHHANG = _KHACHHANG_DTO.HANGKHACHHANG;
            HANGKHACHHANG_DTO _HANGKHACHHANG_DTO = new HANGKHACHHANG_DTO();
            if (!string.IsNullOrEmpty(_KHACHHANG_DTO.HANGKHACHHANG))  _HANGKHACHHANG_DTO = TINHTOAN_DULIEU_HANGKHACHHANG(_KHACHHANG_DTO.HANGKHACHHANG);
            if (!string.IsNullOrEmpty(_HANGKHACHHANG_DTO.MAHANG))
            {
                _GIAODICH_DTO_GLOBAL.QUYDOITIEN_THANH_DIEM = _HANGKHACHHANG_DTO.QUYDOITIEN_THANH_DIEM;
                _GIAODICH_DTO_GLOBAL.QUYDOIDIEM_THANH_TIEN = _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN;
                _GIAODICH_DTO_GLOBAL.HANGKHACHHANG_MOI = _HANGKHACHHANG_DTO.MAHANG;
                _GIAODICH_DTO_GLOBAL.SOTIEN_LENHANG = _HANGKHACHHANG_DTO.SOTIEN_LENHANG;
                decimal QUYDOI = decimal.Round(_KHACHHANG_DTO.SODIEM * _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN, 2);
                _GIAODICH_DTO_GLOBAL.QUYDOI_TOIDA = QUYDOI;
                txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : " + FormatCurrency.FormatMoney(QUYDOI) + " VNĐ";
                if (QUYDOI > 0)
                {
                    btnF10_QuyDoi.Visible = true;
                    btnF10_QuyDoi.Enabled = true;
                }
            }
            else
            {
                txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : 0 VNĐ";
                _GIAODICH_DTO_GLOBAL.HANGKHACHHANG = _HANGKHACHHANG_DTO.MAHANG;
                btnF10_QuyDoi.Visible = false;
                btnF10_QuyDoi.Enabled = false;
            }
            txtThanhToan_TienMat.Focus();
            txtThanhToan_TienMat.SelectAll();
            LOG_THANHTOAN = true;
        }
        private void txtThanhToan_MaKhachHang_KeyDown(object sender, KeyEventArgs e){
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtThanhToan_MaKhachHang.Text))
                {
                    string MaKhachHang = txtThanhToan_MaKhachHang.Text.Trim();
                    List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = FrmThanhToanService.TIMKIEM_KHACHHANG_FROM_ORACLE(MaKhachHang, 1, 0, Session.Session.CurrentUnitCode);
                    if (_LST_KHACHHANG_DTO.Count > 1)
                    {
                        FrmTimKiemKhachHang frmTimKiemKhachHang = new FrmTimKiemKhachHang(MaKhachHang);
                        frmTimKiemKhachHang.SetHanlerTimKiemKhachHang(BINDING_DATA_CHANGE_FROM_GRID_TO_TEXT);
                        frmTimKiemKhachHang.ShowDialog();
                        txtThanhToan_TienMat.Focus();
                        txtThanhToan_TienMat.SelectAll();
                    }
                    else if (_LST_KHACHHANG_DTO.Count == 1)
                    {
                        txtThanhToan_MaKhachHang.ReadOnly = false;
                        txtThanhToan_MaKhachHang.Text = _LST_KHACHHANG_DTO[0].MAKHACHHANG;

                        txtThanhToan_TenKhachHang.Visible = true;
                        txtThanhToan_TenKhachHang.Text = "TÊN: " + _LST_KHACHHANG_DTO[0].TENKHACHHANG;

                        txtThanhToan_SoDienThoai.Visible = true;
                        txtThanhToan_SoDienThoai.Text = "SĐT: " + _LST_KHACHHANG_DTO[0].DIENTHOAI;

                        txtThanhToan_DiemTichLuy.Visible = true;
                        txtThanhToan_DiemTichLuy.Text = "ĐIỂM: " + _LST_KHACHHANG_DTO[0].SODIEM;

                        txtThanhToan_DiaChi.Visible = true;
                        txtThanhToan_DiaChi.Text = "Đ/C: " + _LST_KHACHHANG_DTO[0].DIACHI;

                        txtThanhToan_QuyDoi_ToiDa.Visible = true;

                        _GIAODICH_DTO_GLOBAL.MAKHACHHANG = _LST_KHACHHANG_DTO[0].MAKHACHHANG;
                        _GIAODICH_DTO_GLOBAL.SODIEM = _LST_KHACHHANG_DTO[0].SODIEM;
                        _GIAODICH_DTO_GLOBAL.TONGTIEN_KHACHHANG = _LST_KHACHHANG_DTO[0].TONGTIEN;
                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG = _LST_KHACHHANG_DTO[0].MAKHACHHANG;
                        _GIAODICH_DTO_GLOBAL.HANGKHACHHANG = _LST_KHACHHANG_DTO[0].HANGKHACHHANG;
                        HANGKHACHHANG_DTO _HANGKHACHHANG_DTO = new HANGKHACHHANG_DTO();
                        if (!string.IsNullOrEmpty(_LST_KHACHHANG_DTO[0].HANGKHACHHANG)) _HANGKHACHHANG_DTO = TINHTOAN_DULIEU_HANGKHACHHANG(_LST_KHACHHANG_DTO[0].HANGKHACHHANG);
                        if (!string.IsNullOrEmpty(_HANGKHACHHANG_DTO.MAHANG))
                        {
                            _GIAODICH_DTO_GLOBAL.QUYDOITIEN_THANH_DIEM = _HANGKHACHHANG_DTO.QUYDOITIEN_THANH_DIEM;
                            _GIAODICH_DTO_GLOBAL.QUYDOIDIEM_THANH_TIEN = _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN;
                            _GIAODICH_DTO_GLOBAL.HANGKHACHHANG_MOI = _HANGKHACHHANG_DTO.MAHANG;
                            _GIAODICH_DTO_GLOBAL.SOTIEN_LENHANG = _HANGKHACHHANG_DTO.SOTIEN_LENHANG;
                            decimal QUYDOI = decimal.Round(_LST_KHACHHANG_DTO[0].SODIEM * _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN, 2);
                            _GIAODICH_DTO_GLOBAL.QUYDOI_TOIDA = QUYDOI;
                            txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : " + FormatCurrency.FormatMoney(QUYDOI) + " VNĐ";
                            if (QUYDOI > 0)
                            {
                                btnF10_QuyDoi.Visible = true;
                                btnF10_QuyDoi.Enabled = true;
                            }
                        }
                        else
                        {
                            txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : 0 VNĐ";
                            _GIAODICH_DTO_GLOBAL.HANGKHACHHANG = _HANGKHACHHANG_DTO.MAHANG;
                            btnF10_QuyDoi.Visible = false;
                            btnF10_QuyDoi.Enabled = false;
                        }
                        txtThanhToan_TienMat.Focus();
                        txtThanhToan_TienMat.SelectAll();
                    }
                    else
                    {
                        MessageBox.Show("THÔNG BÁO! KHÔNG TÌM THẤY THÔNG TIN KHÁCH HÀNG '" + txtThanhToan_MaKhachHang.Text + "' ");
                        FrmTimKiemKhachHang frmTimKiemKhachHang = new FrmTimKiemKhachHang(MaKhachHang);
                        frmTimKiemKhachHang.SetHanlerTimKiemKhachHang(BINDING_DATA_CHANGE_FROM_GRID_TO_TEXT);
                        frmTimKiemKhachHang.ShowDialog();
                        txtThanhToan_TienMat.Focus();
                        txtThanhToan_TienMat.SelectAll();
                    }
                }
            }
            LOG_THANHTOAN = true;
        }
        private void btnThanhToan_ThemKhachHang_Click(object sender, EventArgs e)
        {
            FrmDmKhachHang frmDmKhachHang = new FrmDmKhachHang();
            frmDmKhachHang.SET_HANDLER_STATUS_THEMMOI_KHACHHANG(BINDING_DATA_KHACHHANG_TO_THANHTOAN);
            frmDmKhachHang.ShowDialog();
        }

        private void btnF9KhachHang_Click(object sender, EventArgs e)
        {
            txtThanhToan_MaKhachHang.ReadOnly = false;
            txtThanhToan_MaKhachHang.Focus();
            txtThanhToan_TienQuyDoiDiem.Enabled = false;
        }

        private void btnF10_QuyDoi_Click(object sender, EventArgs e)
        {
            if (btnF10_QuyDoi.Visible && btnF10_QuyDoi.Enabled)
            {
                txtThanhToan_TienQuyDoiDiem.Text = "";
                txtThanhToan_TienQuyDoiDiem.Enabled = true;
                txtThanhToan_TienQuyDoiDiem.Focus();
                txtThanhToan_TienQuyDoiDiem.Focus();
                txtThanhToan_TienQuyDoiDiem.SelectAll();
            }
        }

        private void txtThanhToan_TienQuyDoiDiem_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                string THANHTOAN_TIENQUYDOI_STRING = txtThanhToan_TienQuyDoiDiem.Text;
                int start = txtThanhToan_TienQuyDoiDiem.Text.Length - txtThanhToan_TienQuyDoiDiem.SelectionStart;
                THANHTOAN_TIENQUYDOI_STRING = THANHTOAN_TIENQUYDOI_STRING.Replace(",", "");
                txtThanhToan_TienQuyDoiDiem.Text = FormatCurrency.FormatMoney(THANHTOAN_TIENQUYDOI_STRING);
                txtThanhToan_TienQuyDoiDiem.SelectionStart = -start + txtThanhToan_TienMat.Text.Length;
            }
            else
            {
                if (txtThanhToan_TienQuyDoiDiem.Text.Length > 0)
                {
                    decimal TIEN_QUYDOI = 0;
                    decimal.TryParse(txtThanhToan_TienQuyDoiDiem.Text != null ? txtThanhToan_TienQuyDoiDiem.Text : "", out TIEN_QUYDOI);
                    if (TIEN_QUYDOI > _GIAODICH_DTO_GLOBAL.QUYDOI_TOIDA)
                    {
                        txtThanhToan_TienQuyDoiDiem.Text = FormatCurrency.FormatMoney(_GIAODICH_DTO_GLOBAL.QUYDOI_TOIDA);
                    }
                    else
                    {
                        decimal TIEN_PHAITRA = GLOBAL_TONGTIEN_HOADON_BANDAU - TIEN_QUYDOI;
                        if (TIEN_PHAITRA != 0) txtThanhToan_TienPhaiTra.Text = FormatCurrency.FormatMoney(TIEN_PHAITRA);
                        txtThanhToan_TienMat.Text = FormatCurrency.FormatMoney(TIEN_PHAITRA);
                        string THANHTOAN_TIENMAT_STRING = txtThanhToan_TienMat.Text;
                        THANHTOAN_TIENMAT_STRING = THANHTOAN_TIENMAT_STRING.Replace(",", "");
                        _GIAODICH_DTO_GLOBAL.THANHTIEN = TIEN_PHAITRA;
                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.THANHTIEN = TIEN_PHAITRA;_GIAODICH_DTO_GLOBAL.TIENTHE = TIEN_QUYDOI;
                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENTHE = TIEN_QUYDOI;
                        decimal TIENMAT_KHACHTRA, TIEN_TRALAI = 0;

                        decimal.TryParse(THANHTOAN_TIENMAT_STRING, out TIENMAT_KHACHTRA);
                        TIEN_TRALAI = TIENMAT_KHACHTRA - TIEN_PHAITRA;
                        if (TIEN_TRALAI >= 0 && TIEN_TRALAI < 1000000)
                        {
                            txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(TIEN_TRALAI.ToString());
                            txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(TIEN_TRALAI);
                            btnThanhToan_Save.Focus();
                            _GIAODICH_DTO_GLOBAL.TIEN_TRALAI_KHACH = TIEN_TRALAI;
                            _GIAODICH_DTO_GLOBAL.TIENKHACH_TRA = TIENMAT_KHACHTRA;

                            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIEN_TRALAI_KHACH = TIEN_TRALAI;
                            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACH_TRA = TIENMAT_KHACHTRA;

                        }
                        else if (TIEN_TRALAI < 0)
                        {
                            txtThanhToan_TienQuyDoiDiem.SelectAll();
                        }
                        else
                        {
                            DialogResult result = MessageBox.Show("SỐ TIỀN TRẢ LẠI QUÁ LỚN ?", "THAO TÁC SAI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                      
                        decimal DIEMQUYDOI = 0; if (_GIAODICH_DTO_GLOBAL.QUYDOIDIEM_THANH_TIEN != 0)
                        {
                            DIEMQUYDOI = decimal.Round(TIEN_QUYDOI / _GIAODICH_DTO_GLOBAL.QUYDOIDIEM_THANH_TIEN, 2);
                        }
                        _GIAODICH_DTO_GLOBAL.DIEMQUYDOI = DIEMQUYDOI;
                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.DIEMQUYDOI = DIEMQUYDOI;
                        txtThanhToan_TienMat.Focus();
                        txtThanhToan_TienMat.SelectAll();
                    }
                }
                else
                {
                    txtThanhToan_TienPhaiTra.Text = FormatCurrency.FormatMoney(GLOBAL_TONGTIEN_HOADON_BANDAU);
                }
            }
        }
    }
}
