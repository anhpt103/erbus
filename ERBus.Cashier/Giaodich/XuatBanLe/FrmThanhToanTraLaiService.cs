using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.SqlClient;
using ERBus.Cashier.Dto;
using ERBus.Cashier.Common;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public class FrmThanhToanTraLaiService
    {
        public static decimal LAY_SOLUONG_MATHANG_TRONGBO_FROM_ORACLE(string MaBoHang, string MaHang)
        {
            decimal SOLUONG_RESULT = 0;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText =
                            string.Format(
                                @"SELECT b.SOLUONG FROM DM_BOHANG a INNER JOIN DM_BOHANGCHITIET b ON a.MABOHANG = b.MABOHANG WHERE a.MABOHANG = '" + MaBoHang + "' AND b.MAHANG = '" + MaHang + "' AND a.UNITCODE = '" + Session.Session.CurrentUnitCode + "' ");
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {while (dataReader.Read())
                            {
                                if (dataReader["SOLUONG"] != null)
                                {
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG_RESULT);
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("XẢY RA LỖI KHI LẤY DỮ LIỆU MẶT HÀNG TRONG BÓ BÁN TRẢ LẠI ONLINE");
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
            return SOLUONG_RESULT;
        }

        public static decimal LAY_SOLUONG_MATHANG_TRONGBO_FROM_SQLSERVER(string MaBoHang, string MaHang)
        {
            decimal SOLUONG_RESULT = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText =
                            string.Format(
                                @"SELECT DM_BOHANGCHITIET.SOLUONG FROM dbo.DM_BOHANG INNER JOIN dbo.DM_BOHANGCHITIET ON DM_BOHANG.MABOHANG = DM_BOHANGCHITIET.MABOHANG WHERE DM_BOHANG.MABOHANG = '" + MaBoHang + "' AND DM_BOHANGCHITIET.MAHANG = '" + MaHang + "' AND DM_BOHANG.UNITCODE = '" + Session.Session.CurrentUnitCode + "' ");
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["SOLUONG"] != null)
                                {
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG_RESULT);
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("XẢY RA LỖI KHI LẤY DỮ LIỆU MẶT HÀNG TRONG BÓ BÁN TRẢ LẠI OFFLINE");
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
            return SOLUONG_RESULT;
        }

        public static GIAODICH_DTO KHOITAO_DULIEU_THANHTOAN_BANTRALAI_FROM_ORACLE(string MaGiaoDichTraLai,string TongTienTraLai,DataGridView dgvTraLai)
        {
            GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_DTO = new GIAODICH_DTO();
            if (dgvTraLai.RowCount > 0)
            {
                _NVGDQUAY_ASYNCCLIENT_DTO.ID = Guid.NewGuid() + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Hour + DateTime.Now.Minute;
                _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH = MaGiaoDichTraLai.Trim();
                _NVGDQUAY_ASYNCCLIENT_DTO.LOAI_GIAODICH = "XBAN_TRALAI";
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY = Session.Session.CurrentMaNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAY_GIAODICH = Session.Session.CurrentNgayPhatSinh;
                _NVGDQUAY_ASYNCCLIENT_DTO.MA_VOUCHER = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACH_TRA = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIEN_TRALAI_KHACH = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE = 0;
                decimal THANHTIEN = 0;
                decimal.TryParse(TongTienTraLai.Trim(), out THANHTIEN);
                _NVGDQUAY_ASYNCCLIENT_DTO.THANHTIEN = THANHTIEN;
                _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN_TAO = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY = Session.Session.CurrentUserName;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_UPDATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_UPDATE_BY = Session.Session.CurrentUserName;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_STATE = "T";
                _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE = Session.Session.CurrentUnitCode;
                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS = new List<GIAODICH_CHITIET>();
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 0;
                            foreach (DataGridViewRow rowData in dgvTraLai.Rows)
                            {
                                string MAHANG = rowData.Cells["MAHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                                if (MAHANG.Contains("BH"))
                                {
                                    string MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                    OracleCommand cmdGiaoDichQuayChiTiet = new OracleCommand();
                                    cmdGiaoDichQuayChiTiet.Connection = connection;
                                    cmdGiaoDichQuayChiTiet.CommandText = string.Format(@"SELECT MAHANG,MABOPK,MAKHACHHANG,MAKEHANG,TENHANG,MA_KHUYENMAI,SOLUONG,THANHTIEN,BARCODE,GIATRI_THUE_RA,GIABANLE_VAT,TIEN_CHIETKHAU,TYLE_CHIETKHAU,TYLE_KHUYENMAI,TIEN_KHUYENMAI,TYLEVOUCHER,TIEN_VOUCHER,TYLELAILE,GIAVON,MAVAT FROM GIAODICH_CHITIET WHERE MAGDQUAYPK = :MAGDQUAYPK AND UNITCODE = :UNITCODE AND MABOPK = :MABOPK");
                                    cmdGiaoDichQuayChiTiet.CommandType = CommandType.Text;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("MA_GIAODICH", OracleDbType.NVarchar2, 50).Value = MA_GIAODICH;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("MABOPK", OracleDbType.NVarchar2, 50).Value = MAHANG;
                                    OracleDataReader dataReaderGiaoDichQuayChiTiet = null;
                                    dataReaderGiaoDichQuayChiTiet = cmdGiaoDichQuayChiTiet.ExecuteReader();
                                    if (dataReaderGiaoDichQuayChiTiet.HasRows)
                                    {
                                        while (dataReaderGiaoDichQuayChiTiet.Read())
                                        {
                                            GIAODICH_CHITIET VatTuDto = new GIAODICH_CHITIET();
                                            VatTuDto.ID = Guid.NewGuid() + "-" + i;
                                            decimal SOLUONG, GIATRI_THUE_RA, GIABANLE_VAT, TYLELAILE = 0;
                                            decimal TIEN_CHIETKHAU = 0;
                                            decimal TIEN_KHUYENMAI = 0;
                                            decimal TYLE_CHIETKHAU = 0;
                                            decimal TYLE_KHUYENMAI = 0;
                                            decimal TYLEVOUCHER = 0;
                                            decimal TIEN_VOUCHER = 0;
                                            decimal GIAVON = 0;
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["SOLUONG"].ToString(), out SOLUONG);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["GIATRI_THUE_RA"].ToString(), out GIATRI_THUE_RA);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIEN_CHIETKHAU"].ToString(), out TIEN_CHIETKHAU);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLE_CHIETKHAU"].ToString(), out TYLE_CHIETKHAU);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLE_KHUYENMAI"].ToString(), out TYLE_KHUYENMAI);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIEN_KHUYENMAI"].ToString(), out TIEN_KHUYENMAI);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLEVOUCHER"].ToString(), out TYLEVOUCHER);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIEN_VOUCHER"].ToString(), out TIEN_VOUCHER);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLELAILE"].ToString(), out TYLELAILE);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["GIAVON"].ToString(), out GIAVON);
                                            VatTuDto.TENHANG = dataReaderGiaoDichQuayChiTiet["TENHANG"].ToString();
                                            VatTuDto.MAHANG = dataReaderGiaoDichQuayChiTiet["MAHANG"].ToString();
                                            VatTuDto.DONVITINH = "";
                                            VatTuDto.MABOPK = MAHANG;
                                            decimal SOLUONG_TRALAI = 0;
                                            decimal SOLUONGHANG_TRONGBOHANG = LAY_SOLUONG_MATHANG_TRONGBO_FROM_ORACLE(VatTuDto.MABOPK, VatTuDto.MAHANG);
                                            decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG_TRALAI);
                                            VatTuDto.SOLUONG = SOLUONG_TRALAI * SOLUONGHANG_TRONGBOHANG;
                                            VatTuDto.THANHTIEN = GIABANLE_VAT * VatTuDto.SOLUONG - (TIEN_CHIETKHAU / SOLUONG) * VatTuDto.SOLUONG - (TIEN_KHUYENMAI / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.GIABANLE_VAT = GIABANLE_VAT;
                                            VatTuDto.MA_KHUYENMAI = dataReaderGiaoDichQuayChiTiet["MA_KHUYENMAI"].ToString();
                                            VatTuDto.TIEN_CHIETKHAU = (TIEN_CHIETKHAU / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.TYLE_CHIETKHAU = 0;
                                            VatTuDto.TYLE_KHUYENMAI = 0;
                                            VatTuDto.TIEN_KHUYENMAI = (TIEN_KHUYENMAI / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.TIEN_VOUCHER = (TIEN_VOUCHER / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.GIAVON = GIAVON;
                                            VatTuDto.MATHUE_RA = dataReaderGiaoDichQuayChiTiet["MATHUE_RA"].ToString();
                                            VatTuDto.GIATRI_THUE_RA = GIATRI_THUE_RA;
                                            _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(VatTuDto);
                                            i++;
                                        }
                                    }
                                }
                                else
                                {
                                    GIAODICH_CHITIET VatTuDto = new GIAODICH_CHITIET();
                                    VatTuDto.ID = Guid.NewGuid() + "-" + i;
                                    VatTuDto.MAHANG = MAHANG;
                                    VatTuDto.MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                    VatTuDto.DONVITINH = rowData.Cells["DONVITINH_TRALAI"].Value.ToString().ToUpper().Trim();
                                    VatTuDto.TENHANG = rowData.Cells["TENHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                                    VatTuDto.MABOPK = "BH";
                                    decimal SOLUONG = 0;
                                    decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG);
                                    VatTuDto.SOLUONG = SOLUONG;
                                    decimal DONGIA = 0;
                                    decimal.TryParse(rowData.Cells["GIABANLE_VAT_TRALAI"].Value.ToString(), out DONGIA);
                                    VatTuDto.GIABANLE_VAT = DONGIA;
                                    decimal GIAVON = 0;
                                    decimal.TryParse(rowData.Cells["GIAVON_TRALAI"].Value.ToString(), out GIAVON);
                                    VatTuDto.GIAVON = GIAVON;
                                    string MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                    OracleCommand cmdGiaoDichQuayDetails = new OracleCommand();
                                    cmdGiaoDichQuayDetails.Connection = connection;
                                    cmdGiaoDichQuayDetails.CommandText = string.Format(@"SELECT a.MAHANG,b.TENHANG,a.MA_KHUYENMAI,a.SOLUONG,a.THANHTIEN,
                                    c.GIATRI AS GIATRI_THUE_RA,a.GIABANLE_VAT,a.TIEN_CHIETKHAU,a.TYLE_CHIETKHAU,a.TYLE_KHUYENMAI,
                                    a.TIEN_KHUYENMAI,a.TIEN_VOUCHER,a.MATHUE_RA 
                                    FROM GIAODICH_CHITIET a INNER JOIN MATHANG b ON a.MAHANG = b.MAHANG INNER JOIN THUE c ON a.MATHUE_RA = c.MATHUE AND a.MA_GIAODICH = :MA_GIAODICH 
                                    AND b.UNITCODE = :UNITCODE AND a.MAHANG = :MAHANG");
                                    cmdGiaoDichQuayDetails.CommandType = CommandType.Text;
                                    cmdGiaoDichQuayDetails.Parameters.Clear();
                                    cmdGiaoDichQuayDetails.Parameters.Add("MA_GIAODICH", OracleDbType.NVarchar2, 50).Value = MA_GIAODICH.Substring(0, MA_GIAODICH.LastIndexOf('-'));
                                    cmdGiaoDichQuayDetails.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                    cmdGiaoDichQuayDetails.Parameters.Add("MAHANG", OracleDbType.NVarchar2, 50).Value = MAHANG;
                                    OracleDataReader dataReaderGdQuayDetails = null;
                                    dataReaderGdQuayDetails = cmdGiaoDichQuayDetails.ExecuteReader();
                                    decimal SOLUONG_TRONG_GIAODICH = 0;
                                    if (dataReaderGdQuayDetails.HasRows)
                                    {
                                        while (dataReaderGdQuayDetails.Read())
                                        {
                                            VatTuDto.MA_KHUYENMAI = dataReaderGdQuayDetails["MA_KHUYENMAI"].ToString().Trim();
                                            decimal TIEN_CHIETKHAU, TYLE_CHIETKHAU, TYLE_KHUYENMAI, TIEN_VOUCHER, TIENKHUYENMAI_TRONG_GD, GIABANLECOVAT_TRONG_GD = 0;
                                            decimal.TryParse(dataReaderGdQuayDetails["TIEN_CHIETKHAU"].ToString(), out TIEN_CHIETKHAU);
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLE_CHIETKHAU"].ToString(), out TYLE_CHIETKHAU);
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLE_KHUYENMAI"].ToString(), out TYLE_KHUYENMAI);
                                            decimal.TryParse(dataReaderGdQuayDetails["TIEN_VOUCHER"].ToString(), out TIEN_VOUCHER);
                                            decimal.TryParse(dataReaderGdQuayDetails["SOLUONG"].ToString(), out SOLUONG_TRONG_GIAODICH);
                                            decimal.TryParse(dataReaderGdQuayDetails["TIEN_KHUYENMAI"].ToString(), out TIENKHUYENMAI_TRONG_GD);
                                            decimal.TryParse(dataReaderGdQuayDetails["GIABANLE_VAT"].ToString(), out GIABANLECOVAT_TRONG_GD);
                                            VatTuDto.TIEN_CHIETKHAU = (TIEN_CHIETKHAU / SOLUONG_TRONG_GIAODICH) * VatTuDto.SOLUONG;
                                            VatTuDto.TIEN_KHUYENMAI = (TIENKHUYENMAI_TRONG_GD / SOLUONG_TRONG_GIAODICH) * VatTuDto.SOLUONG;
                                            VatTuDto.TYLE_CHIETKHAU = 0;
                                            VatTuDto.TYLE_KHUYENMAI = 0;
                                            decimal GIATRI_THUE_RA = 0;
                                            decimal.TryParse(dataReaderGdQuayDetails["GIATRI_THUE_RA"].ToString(), out GIATRI_THUE_RA);
                                            VatTuDto.GIATRI_THUE_RA = GIATRI_THUE_RA;
                                            VatTuDto.MATHUE_RA = dataReaderGdQuayDetails["MATHUE_RA"].ToString().Trim();
                                            VatTuDto.TIEN_VOUCHER = TIEN_VOUCHER;
                                            VatTuDto.THANHTIEN = GIABANLECOVAT_TRONG_GD * VatTuDto.SOLUONG - VatTuDto.TIEN_KHUYENMAI - VatTuDto.TIEN_CHIETKHAU;
                                        }
                                    }
                                    _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(VatTuDto);
                                    i++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("XẢY RA LỖI KHI KHỞI TẠO DỮ LIỆU LƯU TRỮ BÁN TRẢ LẠI ONLINE");
                            WriteLogs.LogError(ex);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }
                }
            }
            return _NVGDQUAY_ASYNCCLIENT_DTO;
        }


        public static GIAODICH_DTO KHOITAO_DULIEU_THANHTOAN_BANTRALAI_FROM_SQLSERVER(string MaGiaoDichTraLai, string TongTienTraLai, DataGridView dgvTraLai)
        {
            GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_DTO = new GIAODICH_DTO();
            if (dgvTraLai.RowCount > 0)
            {
                _NVGDQUAY_ASYNCCLIENT_DTO.ID = Guid.NewGuid() + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Hour + DateTime.Now.Minute;
                _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH = MaGiaoDichTraLai.Trim();
                _NVGDQUAY_ASYNCCLIENT_DTO.LOAI_GIAODICH = "XBAN_TRALAI";
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY = Session.Session.CurrentMaNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAY_GIAODICH = Session.Session.CurrentNgayPhatSinh;
                _NVGDQUAY_ASYNCCLIENT_DTO.MA_VOUCHER = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACH_TRA = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIEN_TRALAI_KHACH = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE = 0;
                decimal THANHTIEN = 0;
                decimal.TryParse(TongTienTraLai.Trim(), out THANHTIEN);
                _NVGDQUAY_ASYNCCLIENT_DTO.THANHTIEN = THANHTIEN;
                _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN_TAO = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY = Session.Session.CurrentUserName;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_UPDATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_UPDATE_BY = Session.Session.CurrentUserName;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_STATE = "T";
                _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE = Session.Session.CurrentUnitCode;
                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS = new List<GIAODICH_CHITIET>();
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 0;
                            foreach (DataGridViewRow rowData in dgvTraLai.Rows)
                            {
                                string MAHANG = rowData.Cells["MAHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                                if (MAHANG.Contains("BH"))
                                {
                                    string MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                    SqlCommand cmdGiaoDichQuayChiTiet = new SqlCommand();
                                    cmdGiaoDichQuayChiTiet.Connection = connection;
                                    cmdGiaoDichQuayChiTiet.CommandText = string.Format(@"SELECT [MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAHANG],[MANGUOITAO],[NGUOITAO],[MABOPK],[NGAYTAO],[NGAY_GIAODICH] ,[SOLUONG],[THANHTIEN],[GIABANLE_VAT],[TYLE_CHIETKHAU]
                                    ,[TIEN_CHIETKHAU],[TYLE_KHUYENMAI],[TIEN_KHUYENMAI],[TYLEVOUCHER],[TIEN_VOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[GIATRI_THUE_RA],[MA_KHUYENMAI],[UNITCODE] FROM [dbo].[GIAODICH_CHITIET] WHERE MAGDQUAYPK = @MAGDQUAYPK AND MADONVI = @MADONVI AND MABOPK = @MABOPK");
                                    cmdGiaoDichQuayChiTiet.CommandType = CommandType.Text;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("MA_GIAODICH", SqlDbType.NVarChar, 50).Value = MA_GIAODICH;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("MABOPK", SqlDbType.NVarChar, 50).Value = MAHANG;
                                    SqlDataReader dataReaderGiaoDichQuayChiTiet = null;
                                    dataReaderGiaoDichQuayChiTiet = cmdGiaoDichQuayChiTiet.ExecuteReader();
                                    if (dataReaderGiaoDichQuayChiTiet.HasRows)
                                    {
                                        while (dataReaderGiaoDichQuayChiTiet.Read())
                                        {
                                            GIAODICH_CHITIET VatTuDto = new GIAODICH_CHITIET();
                                            VatTuDto.ID = Guid.NewGuid() + "-" + i;
                                            VatTuDto.MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                            decimal SOLUONG, GIATRI_THUE_RA, GIABANLE_VAT, TYLELAILE = 0;
                                            decimal TIEN_CHIETKHAU = 0;
                                            decimal TIEN_KHUYENMAI = 0;
                                            decimal TYLE_CHIETKHAU = 0;
                                            decimal TYLE_KHUYENMAI = 0;
                                            decimal TYLEVOUCHER = 0;
                                            decimal TIEN_VOUCHER = 0;
                                            decimal GIAVON = 0;
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["SOLUONG"].ToString(), out SOLUONG);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["GIATRI_THUE_RA"].ToString(), out GIATRI_THUE_RA);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIEN_CHIETKHAU"].ToString(), out TIEN_CHIETKHAU);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLE_CHIETKHAU"].ToString(), out TYLE_CHIETKHAU);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLE_KHUYENMAI"].ToString(), out TYLE_KHUYENMAI);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIEN_KHUYENMAI"].ToString(), out TIEN_KHUYENMAI);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLEVOUCHER"].ToString(), out TYLEVOUCHER);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIEN_VOUCHER"].ToString(), out TIEN_VOUCHER);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLELAILE"].ToString(), out TYLELAILE);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["GIAVON"].ToString(), out GIAVON);
                                            VatTuDto.MAHANG = dataReaderGiaoDichQuayChiTiet["MAHANG"].ToString();
                                            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
                                            _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(VatTuDto.MAHANG, _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE);
                                            VatTuDto.TENHANG = _EXTEND_VATTU_DTO.TENHANG;
                                            VatTuDto.DONVITINH = _EXTEND_VATTU_DTO.DONVITINH;
                                            VatTuDto.MABOPK = MAHANG;
                                            decimal SOLUONG_TRALAI = 0;
                                            decimal SOLUONGHANG_TRONGBOHANG = LAY_SOLUONG_MATHANG_TRONGBO_FROM_SQLSERVER(VatTuDto.MABOPK, VatTuDto.MAHANG);
                                            decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG_TRALAI);
                                            VatTuDto.SOLUONG = SOLUONG_TRALAI * SOLUONGHANG_TRONGBOHANG;
                                            VatTuDto.THANHTIEN = GIABANLE_VAT * VatTuDto.SOLUONG - (TIEN_CHIETKHAU / SOLUONG) * VatTuDto.SOLUONG - (TIEN_KHUYENMAI / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.GIABANLE_VAT = GIABANLE_VAT;
                                            VatTuDto.MA_KHUYENMAI = dataReaderGiaoDichQuayChiTiet["MA_KHUYENMAI"].ToString();
                                            VatTuDto.TIEN_CHIETKHAU = (TIEN_CHIETKHAU / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.TYLE_CHIETKHAU = 0;
                                            VatTuDto.TYLE_KHUYENMAI = 0;
                                            VatTuDto.TIEN_KHUYENMAI = (TIEN_KHUYENMAI / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.TIEN_VOUCHER = (TIEN_VOUCHER / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.GIAVON = GIAVON;
                                            VatTuDto.MATHUE_RA = dataReaderGiaoDichQuayChiTiet["MAVAT"].ToString();
                                            VatTuDto.GIATRI_THUE_RA = GIATRI_THUE_RA;
                                            _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(VatTuDto);
                                            i++;
                                        }
                                    }
                                }
                                else
                                {
                                    GIAODICH_CHITIET VatTuDto = new GIAODICH_CHITIET();
                                    VatTuDto.ID = Guid.NewGuid() + "-" + i;
                                    VatTuDto.MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                    VatTuDto.MAHANG = MAHANG;
                                    VatTuDto.DONVITINH = rowData.Cells["DONVITINH_TRALAI"].Value.ToString().ToUpper().Trim();
                                    VatTuDto.TENHANG = rowData.Cells["TENHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                                    VatTuDto.MABOPK = "BH";
                                    decimal SOLUONG = 0;
                                    decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG);
                                    VatTuDto.SOLUONG = SOLUONG;
                                    decimal DONGIA = 0;
                                    decimal.TryParse(rowData.Cells["GIABANLE_VAT_TRALAI"].Value.ToString(), out DONGIA);
                                    VatTuDto.GIABANLE_VAT = DONGIA;
                                    decimal GIAVON = 0;
                                    decimal.TryParse(rowData.Cells["GIAVON_TRALAI"].Value.ToString(), out GIAVON);
                                    VatTuDto.GIAVON = GIAVON;
                                    string MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                    SqlCommand cmdGiaoDichQuayDetails = new SqlCommand();
                                    cmdGiaoDichQuayDetails.Connection = connection;
                                    cmdGiaoDichQuayDetails.CommandText = string.Format(@"SELECT [MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAHANG],[MANGUOITAO],[NGUOITAO],[MABOPK],[NGAYTAO],[NGAY_GIAODICH] ,[SOLUONG],[THANHTIEN],[GIABANLE_VAT],[TYLE_CHIETKHAU]
                                    ,[TIEN_CHIETKHAU],[TYLE_KHUYENMAI],[TIEN_KHUYENMAI],[TYLEVOUCHER],[TIEN_VOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[GIATRI_THUE_RA],[MA_KHUYENMAI],[UNITCODE] 
                                    FROM [dbo].[GIAODICH_CHITIET] WHERE MAGDQUAYPK = :MAGDQUAYPK AND MADONVI = @MADONVI AND MAHANG = @MAHANG");
                                    cmdGiaoDichQuayDetails.CommandType = CommandType.Text;
                                    cmdGiaoDichQuayDetails.Parameters.Add("MA_GIAODICH", SqlDbType.NVarChar, 50).Value = MA_GIAODICH.Substring(0, MA_GIAODICH.LastIndexOf('-'));
                                    cmdGiaoDichQuayDetails.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                                    cmdGiaoDichQuayDetails.Parameters.Add("MAHANG", SqlDbType.NVarChar, 50).Value = MAHANG;
                                    SqlDataReader dataReaderGdQuayDetails = null;
                                    dataReaderGdQuayDetails = cmdGiaoDichQuayDetails.ExecuteReader();
                                    decimal SOLUONG_TRONG_GIAODICH = 0;
                                    if (dataReaderGdQuayDetails.HasRows)
                                    {
                                        while (dataReaderGdQuayDetails.Read())
                                        {
                                            if (dataReaderGdQuayDetails["MA_KHUYENMAI"] != null)
                                            {
                                                VatTuDto.MA_KHUYENMAI = dataReaderGdQuayDetails["MA_KHUYENMAI"].ToString().Trim();
                                            }
                                            decimal TIEN_CHIETKHAU, TYLE_CHIETKHAU, TYLE_KHUYENMAI, TYLEVOUCHER, TIEN_VOUCHER, TIENKHUYENMAI_TRONG_GD, GIABANLECOVAT_TRONG_GD = 0;
                                            decimal.TryParse(dataReaderGdQuayDetails["TIEN_CHIETKHAU"].ToString(), out TIEN_CHIETKHAU);
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLE_CHIETKHAU"].ToString(), out TYLE_CHIETKHAU);
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLE_KHUYENMAI"].ToString(), out TYLE_KHUYENMAI);
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLEVOUCHER"].ToString(), out TYLEVOUCHER);
                                            decimal.TryParse(dataReaderGdQuayDetails["TIEN_VOUCHER"].ToString(), out TIEN_VOUCHER);
                                            decimal.TryParse(dataReaderGdQuayDetails["SOLUONG"].ToString(), out SOLUONG_TRONG_GIAODICH);
                                            decimal.TryParse(dataReaderGdQuayDetails["TIEN_KHUYENMAI"].ToString(), out TIENKHUYENMAI_TRONG_GD);
                                            decimal.TryParse(dataReaderGdQuayDetails["GIABANLE_VAT"].ToString(), out GIABANLECOVAT_TRONG_GD);
                                            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
                                            _EXTEND_VATTU_DTO =
                                                FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(MAHANG,
                                                    _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE);
                                            VatTuDto.TIEN_CHIETKHAU = (TIEN_CHIETKHAU / SOLUONG_TRONG_GIAODICH) * VatTuDto.SOLUONG;
                                            VatTuDto.TIEN_KHUYENMAI = (TIENKHUYENMAI_TRONG_GD / SOLUONG_TRONG_GIAODICH) * VatTuDto.SOLUONG;
                                            VatTuDto.TYLE_CHIETKHAU = 0;
                                            VatTuDto.TYLE_KHUYENMAI = 0;
                                            decimal TYLEVATRA = 0;
                                            decimal.TryParse(dataReaderGdQuayDetails["GIATRI_THUE_RA"].ToString(), out TYLEVATRA);
                                            VatTuDto.GIATRI_THUE_RA = TYLEVATRA;
                                            VatTuDto.MATHUE_RA = dataReaderGdQuayDetails["MATHUE_RA"].ToString().Trim();
                                            VatTuDto.TIEN_VOUCHER = TIEN_VOUCHER;
                                            VatTuDto.THANHTIEN = GIABANLECOVAT_TRONG_GD * VatTuDto.SOLUONG - VatTuDto.TIEN_KHUYENMAI - VatTuDto.TIEN_CHIETKHAU;
                                        }
                                    }
                                    _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(VatTuDto);
                                    i++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("XẢY RA LỖI KHI KHỞI TẠO DỮ LIỆU LƯU TRỮ BÁN TRẢ LẠI OFFLINE");
                            WriteLogs.LogError(ex);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }
                }
            }
            return _NVGDQUAY_ASYNCCLIENT_DTO;
        }

        public static GIAODICH_DTO KHOITAO_BILL_THANHTOAN_BANTRALAI_FROM_ORACLE(string MaGiaoDichTraLai, string TongTienTraLai, DataGridView dgvTraLai)
        {
            GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_DTO = new GIAODICH_DTO();
            if (dgvTraLai.RowCount > 0)
            {
                _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH = MaGiaoDichTraLai.Trim();
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY = Session.Session.CurrentMaNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAY_GIAODICH = Session.Session.CurrentNgayPhatSinh;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACH_TRA = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIEN_TRALAI_KHACH = 0;
                decimal THANHTIEN = 0;
                decimal.TryParse(TongTienTraLai, out THANHTIEN);
                _NVGDQUAY_ASYNCCLIENT_DTO.THANHTIEN = THANHTIEN;
                _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN_TAO = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE = Session.Session.CurrentUnitCode;
                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS = new List<GIAODICH_CHITIET>();
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 0;
                            foreach (DataGridViewRow rowData in dgvTraLai.Rows)
                            {
                                GIAODICH_CHITIET _NVHANGGDQUAY_ASYNCCLIENT = new GIAODICH_CHITIET();
                                _NVHANGGDQUAY_ASYNCCLIENT.ID = Guid.NewGuid() + "-" + i;
                                _NVHANGGDQUAY_ASYNCCLIENT.MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                _NVHANGGDQUAY_ASYNCCLIENT.MAHANG = rowData.Cells["MAHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                                _NVHANGGDQUAY_ASYNCCLIENT.DONVITINH = rowData.Cells["DONVITINH_TRALAI"].Value.ToString().ToUpper().Trim();
                                _NVHANGGDQUAY_ASYNCCLIENT.TENHANG = rowData.Cells["TENHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                                decimal SOLUONG = 0;
                                decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG);
                                _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG = SOLUONG;
                                decimal.TryParse(rowData.Cells["THANHTIEN_TRALAI"].Value.ToString(), out THANHTIEN);
                                _NVHANGGDQUAY_ASYNCCLIENT.THANHTIEN = THANHTIEN;
                                decimal DONGIA = 0;
                                decimal.TryParse(rowData.Cells["GIABANLE_VAT_TRALAI"].Value.ToString(), out DONGIA);
                                _NVHANGGDQUAY_ASYNCCLIENT.GIABANLE_VAT = DONGIA;
                                decimal TIENCK = 0;
                                decimal.TryParse(rowData.Cells["TIEN_KHUYENMAI_TRALAI"].Value.ToString(), out TIENCK);
                                _NVHANGGDQUAY_ASYNCCLIENT.TIEN_KHUYENMAI = TIENCK;
                                decimal GIAVON = 0;
                                decimal.TryParse(rowData.Cells["GIAVON_TRALAI"].Value.ToString(), out GIAVON);
                                _NVHANGGDQUAY_ASYNCCLIENT.GIAVON = GIAVON;
                                string MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                OracleCommand cmdGiaoDichQuayDetails = new OracleCommand();
                                cmdGiaoDichQuayDetails.Connection = connection;
                                cmdGiaoDichQuayDetails.CommandText = string.Format(@"SELECT a.MAHANG,b.TENHANG,a.MA_KHUYENMAI,a.SOLUONG,a.THANHTIEN,
                                    c.GIATRI AS GIATRI_THUE_RA,a.GIABANLE_VAT,a.TIEN_CHIETKHAU,a.TYLE_CHIETKHAU,a.TYLE_KHUYENMAI,
                                    a.TIEN_KHUYENMAI,a.TIEN_VOUCHER,a.MATHUE_RA 
                                    FROM GIAODICH_CHITIET a INNER JOIN MATHANG b ON a.MAHANG = b.MAHANG INNER JOIN THUE c ON a.MATHUE_RA = c.MATHUE AND a.MA_GIAODICH = :MA_GIAODICH 
                                    AND b.UNITCODE = :UNITCODE AND a.MAHANG = :MAHANG");
                                cmdGiaoDichQuayDetails.CommandType = CommandType.Text;
                                cmdGiaoDichQuayDetails.Parameters.Add("MA_GIAODICH", OracleDbType.NVarchar2, 50).Value = MA_GIAODICH.Substring(0, MA_GIAODICH.LastIndexOf('-'));
                                cmdGiaoDichQuayDetails.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                cmdGiaoDichQuayDetails.Parameters.Add("MAHANG", OracleDbType.NVarchar2, 50).Value = _NVHANGGDQUAY_ASYNCCLIENT.MAHANG;
                                OracleDataReader dataReaderGdQuayDetails = null;
                                dataReaderGdQuayDetails = cmdGiaoDichQuayDetails.ExecuteReader();
                                if (dataReaderGdQuayDetails.HasRows)
                                {
                                    while (dataReaderGdQuayDetails.Read())
                                    {
                                        decimal TYLEVATRA, TIEN_KHUYENMAI_GD, TIEN_CK_GD, SOLUONG_GD, GIABANLECOVAT_GD, GIAVON_GD = 0;
                                        decimal.TryParse(dataReaderGdQuayDetails["GIATRI_THUE_RA"].ToString(), out TYLEVATRA);
                                        _NVHANGGDQUAY_ASYNCCLIENT.GIATRI_THUE_RA = TYLEVATRA;
                                        _NVHANGGDQUAY_ASYNCCLIENT.MATHUE_RA = dataReaderGdQuayDetails["MATHUE_RA"].ToString().Trim();
                                        decimal.TryParse(dataReaderGdQuayDetails["TIEN_KHUYENMAI"].ToString(), out TIEN_KHUYENMAI_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["TIEN_CHIETKHAU"].ToString(), out TIEN_CK_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["SOLUONG"].ToString(), out SOLUONG_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["GIABANLE_VAT"].ToString(), out GIABANLECOVAT_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.THANHTIEN = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * GIABANLECOVAT_GD - _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_KHUYENMAI_GD / SOLUONG_GD) - _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_CK_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.GIABANLE_VAT = GIABANLECOVAT_GD;
                                        _NVHANGGDQUAY_ASYNCCLIENT.TIEN_KHUYENMAI = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_KHUYENMAI_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.TIEN_CHIETKHAU = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_CK_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.GIAVON = GIAVON_GD;
                                    }
                                }
                                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(_NVHANGGDQUAY_ASYNCCLIENT);
                                i++;
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
                }
            }
            return _NVGDQUAY_ASYNCCLIENT_DTO;
        }

        public static GIAODICH_DTO KHOITAO_BILL_THANHTOAN_BANTRALAI_FROM_SQLSERVER(string MaGiaoDichTraLai, string TongTienTraLai, DataGridView dgvTraLai)
        {
            GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_DTO = new GIAODICH_DTO();
            if (dgvTraLai.RowCount > 0)
            {
                _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH = MaGiaoDichTraLai.Trim();
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY = Session.Session.CurrentMaNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAY_GIAODICH = Session.Session.CurrentNgayPhatSinh;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACH_TRA = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIEN_TRALAI_KHACH = 0;
                decimal THANHTIEN = 0;
                decimal.TryParse(TongTienTraLai, out THANHTIEN);
                _NVGDQUAY_ASYNCCLIENT_DTO.THANHTIEN = THANHTIEN;
                _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN_TAO = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE = Session.Session.CurrentUnitCode;
                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS = new List<GIAODICH_CHITIET>();
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 0;
                            foreach (DataGridViewRow rowData in dgvTraLai.Rows)
                            {
                                GIAODICH_CHITIET _NVHANGGDQUAY_ASYNCCLIENT = new GIAODICH_CHITIET();
                                _NVHANGGDQUAY_ASYNCCLIENT.ID = Guid.NewGuid() + "-" + i;
                                _NVHANGGDQUAY_ASYNCCLIENT.MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                _NVHANGGDQUAY_ASYNCCLIENT.MAHANG = rowData.Cells["MAHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                                _NVHANGGDQUAY_ASYNCCLIENT.DONVITINH = rowData.Cells["DONVITINH_TRALAI"].Value.ToString().ToUpper().Trim();
                                _NVHANGGDQUAY_ASYNCCLIENT.TENHANG = rowData.Cells["TENHANG_TRALAI"].Value.ToString().ToUpper().Trim();
                                decimal SOLUONG = 0;
                                decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG);
                                _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG = SOLUONG;
                                decimal.TryParse(rowData.Cells["THANHTIEN_TRALAI"].Value.ToString(), out THANHTIEN);
                                _NVHANGGDQUAY_ASYNCCLIENT.THANHTIEN = THANHTIEN;
                                decimal DONGIA = 0;
                                decimal.TryParse(rowData.Cells["GIABANLE_VAT_TRALAI"].Value.ToString(), out DONGIA);
                                _NVHANGGDQUAY_ASYNCCLIENT.GIABANLE_VAT = DONGIA;
                                decimal TIENCK = 0;
                                decimal.TryParse(rowData.Cells["TIEN_KHUYENMAI_TRALAI"].Value.ToString(), out TIENCK);
                                _NVHANGGDQUAY_ASYNCCLIENT.TIEN_KHUYENMAI = TIENCK;
                                decimal GIAVON = 0;
                                decimal.TryParse(rowData.Cells["GIAVON_TRALAI"].Value.ToString(), out GIAVON);
                                _NVHANGGDQUAY_ASYNCCLIENT.GIAVON = GIAVON;
                                string MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                SqlCommand cmdGiaoDichQuayDetails = new SqlCommand();
                                cmdGiaoDichQuayDetails.Connection = connection;
                                cmdGiaoDichQuayDetails.CommandText = string.Format(@"SELECT [MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAHANG],[MANGUOITAO],[NGUOITAO],[MABOPK],[NGAYTAO],
                                    [NGAY_GIAODICH] ,[SOLUONG],[THANHTIEN],[GIABANLE_VAT],[TYLE_CHIETKHAU]
                                  ,[TIEN_CHIETKHAU],[TYLE_KHUYENMAI],[TIEN_KHUYENMAI],[TYLEVOUCHER],[TIEN_VOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[GIATRI_THUE_RA],[MA_KHUYENMAI],[UNITCODE] FROM [dbo].[GIAODICH_CHITIET]
                                    WHERE MAGDQUAYPK = @MAGDQUAYPK AND MADONVI = @MADONVI AND MAHANG = @MAHANG");
                                cmdGiaoDichQuayDetails.CommandType = CommandType.Text;
                                cmdGiaoDichQuayDetails.Parameters.Add("MA_GIAODICH", SqlDbType.NVarChar, 50).Value = MA_GIAODICH.Substring(0, MA_GIAODICH.LastIndexOf('-'));
                                cmdGiaoDichQuayDetails.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                                cmdGiaoDichQuayDetails.Parameters.Add("MAHANG", SqlDbType.NVarChar, 50).Value = _NVHANGGDQUAY_ASYNCCLIENT.MAHANG;
                                SqlDataReader dataReaderGdQuayDetails = null;
                                dataReaderGdQuayDetails = cmdGiaoDichQuayDetails.ExecuteReader();
                                if (dataReaderGdQuayDetails.HasRows)
                                {
                                    while (dataReaderGdQuayDetails.Read())
                                    {
                                        decimal TYLEVATRA, TYLELAILE, TIEN_KHUYENMAI_GD, TIEN_CK_GD, SOLUONG_GD, GIABANLECOVAT_GD, GIAVON_GD = 0;
                                        decimal.TryParse(dataReaderGdQuayDetails["GIATRI_THUE_RA"].ToString(), out TYLEVATRA);
                                        _NVHANGGDQUAY_ASYNCCLIENT.GIATRI_THUE_RA = TYLEVATRA;
                                        decimal.TryParse(dataReaderGdQuayDetails["TYLELAILE"].ToString(), out TYLELAILE);
                                        _NVHANGGDQUAY_ASYNCCLIENT.MATHUE_RA = dataReaderGdQuayDetails["MATHUE_RA"].ToString().Trim();
                                        decimal.TryParse(dataReaderGdQuayDetails["TIEN_KHUYENMAI"].ToString(), out TIEN_KHUYENMAI_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["TIEN_CHIETKHAU"].ToString(), out TIEN_CK_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["SOLUONG"].ToString(), out SOLUONG_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["GIABANLE_VAT"].ToString(), out GIABANLECOVAT_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["GIAVON"].ToString(), out GIAVON_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.THANHTIEN = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * GIABANLECOVAT_GD - _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_KHUYENMAI_GD / SOLUONG_GD) - _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_CK_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.GIABANLE_VAT = GIABANLECOVAT_GD;
                                        _NVHANGGDQUAY_ASYNCCLIENT.TIEN_KHUYENMAI = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_KHUYENMAI_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.TIEN_CHIETKHAU = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_CK_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.GIAVON = GIAVON_GD;
                                    }
                                }
                                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(_NVHANGGDQUAY_ASYNCCLIENT);
                                i++;
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
                }
            }
            return _NVGDQUAY_ASYNCCLIENT_DTO;
        }
    }
}
