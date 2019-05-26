using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using ERBus.Cashier.Common;
using ERBus.Cashier.Dto;
using Oracle.ManagedDataAccess.Client;
using EnumCommon = ERBus.Cashier.Common.EnumCommon;
using System.Data.SqlClient;
using static ERBus.Cashier.Dto.VATTU_DTO;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public class FrmXuatBanLeService
    {
        static string KeyMaCan = "20";

        public static List<VATTU_DTO> BAN_THEO_MACAN_DIENTU_ORACLE(string MaHang, string TABLE_NAME, EnumCommon.METHOD_PRICE PhuongThucTinhGia)
        {
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();

            try
            {
                string itemCode = string.Empty; if (!string.IsNullOrEmpty(MaHang)) itemCode = MaHang.Substring(2, 5);
                string soLuongItemCode = ""; if (!string.IsNullOrEmpty(MaHang)) soLuongItemCode = MaHang.Substring(7, 5);
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = string.Format(@"SELECT A.MAHANG,A.TENHANG,E.TENDONVITINH AS DONVITINH,A.BARCODE,A.MANHACUNGCAP,A.MATHUE_RA,NVL(D.GIATRI, 0) AS GIATRI_THUE_RA,A.ITEMCODE,B.GIABANBUON_VAT,B.GIABANLE_VAT,C.GIAVON,C.TONCUOIKYSL FROM MATHANG A INNER JOIN MATHANG_GIA B ON A.MAHANG = B.MAHANG INNER JOIN " + TABLE_NAME + " C ON A.MAHANG = C.MAHANG INNER JOIN THUE D ON A.MATHUE_RA = D.MATHUE INNER JOIN DONVITINH E ON A.MADONVITINH = E.MADONVITINH AND C.MAKHO = :MAKHO AND A.ITEMCODE = :ITEMCODE AND A.UNITCODE = :UNITCODE");
                            OracleDataReader dataReader = cmd.ExecuteReader();
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add("MAKHO", OracleDbType.Varchar2, 50, Session.Session.CurrentWareHouse, ParameterDirection.Input);
                            cmd.Parameters.Add("ITEMCODE", OracleDbType.Varchar2, 50, itemCode, ParameterDirection.Input);
                            cmd.Parameters.Add("UNITCODE", OracleDbType.Varchar2, 10, Session.Session.CurrentUnitCode, ParameterDirection.Input);
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    decimal SOLUONG = 0;
                                   
                                    VATTU_DTO dataDto = new VATTU_DTO();
                                    dataDto.MAHANG = dataReader["MAHANG"] != DBNull.Value ? dataReader["MAHANG"].ToString() : "";
                                    dataDto.TENHANG = dataReader["TENHANG"] != DBNull.Value ? dataReader["TENHANG"].ToString() : "";
                                    dataDto.DONVITINH = dataReader["DONVITINH"] != DBNull.Value ? dataReader["DONVITINH"].ToString() : "";
                                    dataDto.BARCODE = dataReader["BARCODE"] != DBNull.Value ? dataReader["BARCODE"].ToString() : "";
                                    dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"] != DBNull.Value ? dataReader["MANHACUNGCAP"].ToString() : "";
                                    dataDto.MATHUE_RA = dataReader["MATHUE_RA"] != DBNull.Value ? dataReader["MATHUE_RA"].ToString() : "";
                                    dataDto.GIATRI_THUE_RA = dataReader["GIATRI_THUE_RA"] != DBNull.Value ? decimal.Parse(dataReader["GIATRI_THUE_RA"].ToString()) : 0;
                                    dataDto.GIABANBUON_VAT = dataReader["GIABANBUON_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANBUON_VAT"].ToString()) : 0;
                                    dataDto.GIABANLE_VAT = dataReader["GIABANLE_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANLE_VAT"].ToString()) : 0;
                                    dataDto.GIAVON = dataReader["GIAVON"] != DBNull.Value ? decimal.Parse(dataReader["GIAVON"].ToString()) : 0;
                                    dataDto.TONCUOIKYSL = dataReader["TONCUOIKYSL"] != DBNull.Value ? decimal.Parse(dataReader["TONCUOIKYSL"].ToString()) : 0;
                                    dataDto.ITEMCODE = dataReader["ITEMCODE"] != DBNull.Value ? dataReader["ITEMCODE"].ToString() : "";
                                    decimal.TryParse(soLuongItemCode, out SOLUONG);
                                    dataDto.SOLUONG = SOLUONG;
                                    dataDto.LAMACAN = true;
                                    if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                    {
                                        dataDto.GIABANLE_VAT = dataDto.GIABANBUON_VAT;
                                    }
                                    else if (Session.Session.CurrentLoaiGiaoDich == "GIAVON")
                                    {
                                        dataDto.GIABANLE_VAT = dataDto.GIAVON * (1 + dataDto.GIATRI_THUE_RA / 100);
                                    }

                                    listDataDto.Add(dataDto);
                                }
                                dataReader.Close();
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
            }
            catch
            {
                string NOTIFICATION_WARNING = string.Format(@"MÃ CÂN '{0}' KHÔNG HỢP LỆ", MaHang);
                MessageBox.Show(NOTIFICATION_WARNING);
            }
            return listDataDto;
        }

        public static List<VATTU_DTO> BAN_THEO_MABOHANG_ORACLE(string MaHang, string TABLE_NAME, EnumCommon.METHOD_PRICE PhuongThucTinhGia)
        {
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"A.MABOHANG AS MAHANG,A.TENBOHANG  AS TENHANG,'Bó' AS DONVITINH,'' AS MANHACUNGCAP, SUM(B.TONGTIEN) AS GIABANBUON_VAT, SUM(B.TONGTIEN) AS GIABANLE_VAT, '' AS ITEMCODE FROM BOHANG a, BOHANG_CHITIET b WHERE a.MABOHANG = b.MABOHANG AND a.MABOHANG = '" + MaHang + "' AND a.UNITCODE = '" + Session.Session.CurrentUnitCode + "' GROUP BY a.MABOHANG,a.TENBOHANG");
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                decimal GIABANBUON_VAT, GIABANLE_VAT, SOLUONG = 0;
                                VATTU_DTO dataDto = new VATTU_DTO();
                                dataDto.MAHANG = dataReader["MAHANG"].ToString();
                                dataDto.MABO = dataDto.MAHANG;
                                dataDto.TENHANG = dataReader["TENHANG"].ToString();
                                dataDto.DONVITINH = dataReader["DONVITINH"].ToString();
                                dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                decimal.TryParse(dataReader["GIABANBUON_VAT"].ToString(), out GIABANBUON_VAT);
                                dataDto.GIABANBUON_VAT = GIABANBUON_VAT;
                                decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                dataDto.GIABANLE_VAT = GIABANLE_VAT;
                                dataDto.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                dataDto.SOLUONG = SOLUONG;
                                dataDto.LAMACAN = false;
                                cmd.Parameters.Clear();
                                cmd.CommandText = "SELECT MAHANG, SOLUONG FROM BOHANG_CHITIET WHERE MABOHANG = '" + dataDto.MAHANG + "' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'";
                                OracleDataReader dataReaderBoHangChiTiet = cmd.ExecuteReader();
                                EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();
                                if (Config.CheckConnectToServer())
                                {
                                    _EXTEND_VAT_BOHANG = LAYDULIEU_VAT_BOHANG_FROM_DATABASE_ORACLE(dataDto.MAHANG, Session.Session.CurrentUnitCode);
                                    dataDto.MATHUE_RA = _EXTEND_VAT_BOHANG.MATHUE_RA;
                                    dataDto.GIATRI_THUE_RA = _EXTEND_VAT_BOHANG.GIATRI_THUE_RA;
                                }
                                else
                                {
                                    _EXTEND_VAT_BOHANG = LAYDULIEU_VAT_BOHANG_FROM_DATABASE_SQLSERVER(dataDto.MAHANG, Session.Session.CurrentUnitCode);
                                    dataDto.MATHUE_RA = _EXTEND_VAT_BOHANG.MATHUE_RA;
                                    dataDto.GIATRI_THUE_RA = _EXTEND_VAT_BOHANG.GIATRI_THUE_RA;
                                }
                                if (dataReaderBoHangChiTiet.HasRows)
                                {
                                    while (dataReaderBoHangChiTiet.Read())
                                    {
                                        decimal GiaVon = 0;
                                        decimal TONCUOIKYSL = 0;
                                        string maVatTuBoHang = dataReaderBoHangChiTiet["MAHANG"] != null ? dataReaderBoHangChiTiet["MAHANG"].ToString().ToUpper().Trim() : "";
                                        decimal.TryParse(dataReaderBoHangChiTiet["SOLUONG"].ToString(), out SOLUONG);
                                        //dataDto.SOLUONG = SOLUONG;
                                        OracleCommand cmdVatTu = new OracleCommand();
                                        cmdVatTu.Connection = connection;
                                        cmdVatTu.CommandText = "SELECT b.GIAVON,b.TONCUOIKYSL FROM V_VATTU_GIABAN a JOIN " + TABLE_NAME + " b ON a.MAHANG = b.MAHANG AND b.MAKHO = '" + Session.Session.CurrentWareHouse + "' WHERE a.MAHANG = '" + maVatTuBoHang + "' AND a.MADONVI = '" + Session.Session.CurrentUnitCode + "'";
                                        OracleDataReader dataReaderVatTu = cmdVatTu.ExecuteReader();
                                        if (dataReaderVatTu.HasRows)
                                        {
                                            while (dataReaderVatTu.Read())
                                            {
                                                decimal.TryParse(dataReaderVatTu["GIAVON"].ToString(), out GiaVon);
                                                decimal.TryParse(dataReaderVatTu["TONCUOIKYSL"].ToString(), out TONCUOIKYSL);
                                            }
                                            dataDto.GIAVON += GiaVon * SOLUONG;
                                            dataDto.TONCUOIKYSL += TONCUOIKYSL;
                                        }
                                    }
                                }

                                if (PhuongThucTinhGia == EnumCommon.METHOD_PRICE.GIABANLE_VAT)
                                {
                                    dataDto.GIABANLE_VAT = GIABANLE_VAT;
                                }
                                else
                                {
                                    if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                    {
                                        dataDto.GIABANLE_VAT = GIABANBUON_VAT;
                                    }
                                    else
                                    {
                                        dataDto.GIABANLE_VAT = dataDto.GIAVON * (1 + dataDto.GIATRI_THUE_RA / 100);

                                    }
                                }
                                dataDto.GIAVON = dataDto.GIAVON * (1 + dataDto.GIATRI_THUE_RA / 100);
                                listDataDto.Add(dataDto);
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
            return listDataDto;
        }

        public static List<VATTU_DTO> BAN_THEO_MAHANG_ORACLE(string MaHang, string TABLE_NAME, EnumCommon.METHOD_PRICE PhuongThucTinhGia, decimal SoLuong)
        {
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = string.Format(@"SELECT A.MAHANG,A.TENHANG,E.TENDONVITINH AS DONVITINH,A.BARCODE,A.MANHACUNGCAP,A.MATHUE_RA,NVL(D.GIATRI, 0) AS GIATRI_THUE_RA,A.ITEMCODE,B.GIABANBUON_VAT,B.GIABANLE_VAT,C.GIAVON,C.TONCUOIKYSL FROM MATHANG A INNER JOIN MATHANG_GIA B ON A.MAHANG = B.MAHANG INNER JOIN "+ TABLE_NAME + " C ON A.MAHANG = C.MAHANG INNER JOIN THUE D ON A.MATHUE_RA = D.MATHUE INNER JOIN DONVITINH E ON A.MADONVITINH = E.MADONVITINH AND C.MAKHO = :MAKHO AND A.MAHANG = :MAHANG AND A.UNITCODE = :UNITCODE");
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("MAKHO", OracleDbType.Varchar2, 50, Session.Session.CurrentWareHouse, ParameterDirection.Input);
                        cmd.Parameters.Add("MAHANG", OracleDbType.Varchar2, 50,  MaHang, ParameterDirection.Input);
                        cmd.Parameters.Add("UNITCODE", OracleDbType.Varchar2, 10, Session.Session.CurrentUnitCode, ParameterDirection.Input);
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                VATTU_DTO dataDto = new VATTU_DTO();
                                dataDto.MAHANG = dataReader["MAHANG"] != DBNull.Value ? dataReader["MAHANG"].ToString() : "";
                                dataDto.TENHANG = dataReader["TENHANG"] != DBNull.Value ? dataReader["TENHANG"].ToString() : "";
                                dataDto.DONVITINH = dataReader["DONVITINH"] != DBNull.Value ? dataReader["DONVITINH"].ToString() : "";
                                dataDto.BARCODE = dataReader["BARCODE"] != DBNull.Value ? dataReader["BARCODE"].ToString() : ""; 
                                dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"] != DBNull.Value ? dataReader["MANHACUNGCAP"].ToString() : "";
                                dataDto.MATHUE_RA = dataReader["MATHUE_RA"] != DBNull.Value ? dataReader["MATHUE_RA"].ToString() : "";
                                dataDto.GIATRI_THUE_RA = dataReader["GIATRI_THUE_RA"] != DBNull.Value ? decimal.Parse(dataReader["GIATRI_THUE_RA"].ToString()) : 0;
                                dataDto.GIABANBUON_VAT = dataReader["GIABANBUON_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANBUON_VAT"].ToString()) : 0;
                                dataDto.GIABANLE_VAT = dataReader["GIABANLE_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANLE_VAT"].ToString()) : 0;
                                dataDto.GIAVON = dataReader["GIAVON"] != DBNull.Value ? decimal.Parse(dataReader["GIAVON"].ToString()) : 0;
                                dataDto.TONCUOIKYSL = dataReader["TONCUOIKYSL"] != DBNull.Value ? decimal.Parse(dataReader["TONCUOIKYSL"].ToString()) : 0;
                                dataDto.ITEMCODE = dataReader["ITEMCODE"] != DBNull.Value ? dataReader["ITEMCODE"].ToString() : "";
                                dataDto.SOLUONG = SoLuong;
                                dataDto.LAMACAN = false;
                                if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                {
                                    dataDto.GIABANLE_VAT = dataDto.GIABANBUON_VAT;
                                }
                                else if(Session.Session.CurrentLoaiGiaoDich == "GIAVON")
                                {
                                    dataDto.GIABANLE_VAT = dataDto.GIAVON * (1 + dataDto.GIATRI_THUE_RA / 100);
                                }
                               
                                listDataDto.Add(dataDto);
                            }
                            dataReader.Close();
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
            return listDataDto;
        }


        public static List<VATTU_DTO> BAN_THEO_BARCODE_ORACLE(string MaHang, string TABLE_NAME, EnumCommon.METHOD_PRICE PhuongThucTinhGia, decimal SoLuong)
        {
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = string.Format(@"SELECT A.MAHANG,A.TENHANG,E.TENDONVITINH AS DONVITINH,A.BARCODE,A.MANHACUNGCAP,A.MATHUE_RA,NVL(D.GIATRI, 0) AS GIATRI_THUE_RA,A.ITEMCODE,B.GIABANBUON_VAT,B.GIABANLE_VAT,C.GIAVON,C.TONCUOIKYSL FROM MATHANG A INNER JOIN MATHANG_GIA B ON A.MAHANG = B.MAHANG INNER JOIN " + TABLE_NAME + " C ON A.MAHANG = C.MAHANG INNER JOIN THUE D ON A.MATHUE_RA = D.MATHUE INNER JOIN DONVITINH E ON A.MADONVITINH = E.MADONVITINH AND C.MAKHO = :MAKHO AND A.BARCODE LIKE '%" + MaHang + "%' AND A.UNITCODE = :UNITCODE");
                        cmd.Parameters.Add("MAKHO", OracleDbType.Varchar2, 50, Session.Session.CurrentWareHouse, ParameterDirection.Input);
                        cmd.Parameters.Add("UNITCODE", OracleDbType.Varchar2, 10, Session.Session.CurrentUnitCode, ParameterDirection.Input);
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                VATTU_DTO dataDto = new VATTU_DTO();
                                dataDto.MAHANG = dataReader["MAHANG"] != DBNull.Value ? dataReader["MAHANG"].ToString() : "";
                                dataDto.TENHANG = dataReader["TENHANG"] != DBNull.Value ? dataReader["TENHANG"].ToString() : "";
                                dataDto.DONVITINH = dataReader["DONVITINH"] != DBNull.Value ? dataReader["DONVITINH"].ToString() : "";
                                dataDto.BARCODE = dataReader["BARCODE"] != DBNull.Value ? dataReader["BARCODE"].ToString() : "";
                                dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"] != DBNull.Value ? dataReader["MANHACUNGCAP"].ToString() : "";
                                dataDto.MATHUE_RA = dataReader["MATHUE_RA"] != DBNull.Value ? dataReader["MATHUE_RA"].ToString() : "";
                                dataDto.GIATRI_THUE_RA = dataReader["GIATRI_THUE_RA"] != DBNull.Value ? decimal.Parse(dataReader["GIATRI_THUE_RA"].ToString()) : 0;
                                dataDto.GIABANBUON_VAT = dataReader["GIABANBUON_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANBUON_VAT"].ToString()) : 0;
                                dataDto.GIABANLE_VAT = dataReader["GIABANLE_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANLE_VAT"].ToString()) : 0;
                                dataDto.GIAVON = dataReader["GIAVON"] != DBNull.Value ? decimal.Parse(dataReader["GIAVON"].ToString()) : 0;
                                dataDto.TONCUOIKYSL = dataReader["TONCUOIKYSL"] != DBNull.Value ? decimal.Parse(dataReader["TONCUOIKYSL"].ToString()) : 0;
                                dataDto.ITEMCODE = dataReader["ITEMCODE"] != DBNull.Value ? dataReader["ITEMCODE"].ToString() : "";
                                dataDto.SOLUONG = SoLuong;
                                dataDto.LAMACAN = false;
                                if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                {
                                    dataDto.GIABANLE_VAT = dataDto.GIABANBUON_VAT;
                                }
                                else if (Session.Session.CurrentLoaiGiaoDich == "GIAVON")
                                {
                                    dataDto.GIABANLE_VAT = dataDto.GIAVON * (1 + dataDto.GIATRI_THUE_RA / 100);
                                }

                                listDataDto.Add(dataDto);
                            }
                            dataReader.Close();
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
            return listDataDto;
        }

        public static List<VATTU_DTO> BAN_THEO_MACAN_DIENTU_SQLSERVER(string MaHang, string TABLE_NAME, EnumCommon.METHOD_PRICE PhuongThucTinhGia)
        {
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();
            try
            {
                string itemCode = string.Empty; if (!string.IsNullOrEmpty(MaHang)) itemCode = MaHang.Substring(2, 5);
                string soLuongItemCode = ""; if (!string.IsNullOrEmpty(MaHang)) soLuongItemCode = MaHang.Substring(7, 5);
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = string.Format(@"SELECT A.MAHANG,A.TENHANG,E.TENDONVITINH AS DONVITINH,A.BARCODE,A.MANHACUNGCAP,A.MATHUE_RA,NVL(D.GIATRI, 0) AS GIATRI_THUE_RA,A.ITEMCODE,B.GIABANBUON_VAT,B.GIABANLE_VAT,C.GIAVON,C.TONCUOIKYSL FROM dbo.MATHANG A INNER JOINdbo. MATHANG_GIA B ON A.MAHANG = B.MAHANG INNER JOIN " + TABLE_NAME + " C ON A.MAHANG = C.MAHANG INNER JOIN dbo.THUE D ON A.MATHUE_RA = D.MATHUE INNER JOIN dbo.DONVITINH E ON A.MADONVITINH = E.MADONVITINH AND C.MAKHO = @MAKHO AND A.ITEMCODE = @ITEMCODE AND A.UNITCODE = @UNITCODE");
                            SqlDataReader dataReader = cmd.ExecuteReader();
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add("MAKHO", SqlDbType.VarChar, 50, Session.Session.CurrentWareHouse);
                            cmd.Parameters.Add("ITEMCODE", SqlDbType.VarChar, 50, itemCode);
                            cmd.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10, Session.Session.CurrentUnitCode);
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    decimal SOLUONG = 0;

                                    VATTU_DTO dataDto = new VATTU_DTO();
                                    dataDto.MAHANG = dataReader["MAHANG"] != DBNull.Value ? dataReader["MAHANG"].ToString() : "";
                                    dataDto.TENHANG = dataReader["TENHANG"] != DBNull.Value ? dataReader["TENHANG"].ToString() : "";
                                    dataDto.DONVITINH = dataReader["DONVITINH"] != DBNull.Value ? dataReader["DONVITINH"].ToString() : "";
                                    dataDto.BARCODE = dataReader["BARCODE"] != DBNull.Value ? dataReader["BARCODE"].ToString() : "";
                                    dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"] != DBNull.Value ? dataReader["MANHACUNGCAP"].ToString() : "";
                                    dataDto.MATHUE_RA = dataReader["MATHUE_RA"] != DBNull.Value ? dataReader["MATHUE_RA"].ToString() : "";
                                    dataDto.GIATRI_THUE_RA = dataReader["GIATRI_THUE_RA"] != DBNull.Value ? decimal.Parse(dataReader["GIATRI_THUE_RA"].ToString()) : 0;
                                    dataDto.GIABANBUON_VAT = dataReader["GIABANBUON_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANBUON_VAT"].ToString()) : 0;
                                    dataDto.GIABANLE_VAT = dataReader["GIABANLE_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANLE_VAT"].ToString()) : 0;
                                    dataDto.GIAVON = dataReader["GIAVON"] != DBNull.Value ? decimal.Parse(dataReader["GIAVON"].ToString()) : 0;
                                    dataDto.TONCUOIKYSL = dataReader["TONCUOIKYSL"] != DBNull.Value ? decimal.Parse(dataReader["TONCUOIKYSL"].ToString()) : 0;
                                    dataDto.ITEMCODE = dataReader["ITEMCODE"] != DBNull.Value ? dataReader["ITEMCODE"].ToString() : "";
                                    decimal.TryParse(soLuongItemCode, out SOLUONG);
                                    dataDto.SOLUONG = SOLUONG;
                                    dataDto.LAMACAN = true;
                                    if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                    {
                                        dataDto.GIABANLE_VAT = dataDto.GIABANBUON_VAT;
                                    }
                                    else if (Session.Session.CurrentLoaiGiaoDich == "GIAVON")
                                    {
                                        dataDto.GIABANLE_VAT = dataDto.GIAVON * (1 + dataDto.GIATRI_THUE_RA / 100);
                                    }

                                    listDataDto.Add(dataDto);
                                }
                                dataReader.Close();
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
            }
            catch
            {
                string NOTIFICATION_WARNING = string.Format(@"MÃ CÂN '{0}' KHÔNG HỢP LỆ", MaHang);
                MessageBox.Show(NOTIFICATION_WARNING);
            }
            return listDataDto;
        }

        public static List<VATTU_DTO> BAN_THEO_MABOHANG_SQLSERVER(string MaHang, string TABLE_NAME, EnumCommon.METHOD_PRICE PhuongThucTinhGia)
        {
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"A.MABOHANG AS MAHANG,A.TENBOHANG  AS TENHANG,'Bó' AS DONVITINH,'' AS MANHACUNGCAP, SUM(B.TONGTIEN) AS GIABANBUON_VAT, SUM(B.TONGTIEN) AS GIABANLE_VAT, '' AS ITEMCODE FROM BOHANG a, BOHANG_CHITIET b WHERE a.MABOHANG = b.MABOHANG AND a.MABOHANG = '" + MaHang + "' AND a.UNITCODE = '" + Session.Session.CurrentUnitCode + "' GROUP BY a.MABOHANG,a.TENBOHANG");
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                decimal GIABANBUON_VAT, GIABANLE_VAT, SOLUONG = 0;
                                VATTU_DTO dataDto = new VATTU_DTO();
                                dataDto.MAHANG = dataReader["MAHANG"].ToString();
                                dataDto.MABO = dataDto.MAHANG;
                                dataDto.TENHANG = dataReader["TENHANG"].ToString();
                                dataDto.DONVITINH = dataReader["DONVITINH"].ToString();
                                dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                decimal.TryParse(dataReader["GIABANBUON_VAT"].ToString(), out GIABANBUON_VAT);
                                dataDto.GIABANBUON_VAT = GIABANBUON_VAT;
                                decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                dataDto.GIABANLE_VAT = GIABANLE_VAT;
                                dataDto.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                dataDto.SOLUONG = SOLUONG;
                                dataDto.LAMACAN = false;
                                cmd.Parameters.Clear();
                                cmd.CommandText = "SELECT MAHANG, SOLUONG FROM BOHANG_CHITIET WHERE MABOHANG = '" + dataDto.MAHANG + "' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'";
                                SqlDataReader dataReaderBoHangChiTiet = cmd.ExecuteReader();
                                EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();
                                if (Config.CheckConnectToServer())
                                {
                                    _EXTEND_VAT_BOHANG = LAYDULIEU_VAT_BOHANG_FROM_DATABASE_ORACLE(dataDto.MAHANG, Session.Session.CurrentUnitCode);
                                    dataDto.MATHUE_RA = _EXTEND_VAT_BOHANG.MATHUE_RA;
                                    dataDto.GIATRI_THUE_RA = _EXTEND_VAT_BOHANG.GIATRI_THUE_RA;
                                }
                                else
                                {
                                    _EXTEND_VAT_BOHANG = LAYDULIEU_VAT_BOHANG_FROM_DATABASE_SQLSERVER(dataDto.MAHANG, Session.Session.CurrentUnitCode);
                                    dataDto.MATHUE_RA = _EXTEND_VAT_BOHANG.MATHUE_RA;
                                    dataDto.GIATRI_THUE_RA = _EXTEND_VAT_BOHANG.GIATRI_THUE_RA;
                                }
                                if (dataReaderBoHangChiTiet.HasRows)
                                {
                                    while (dataReaderBoHangChiTiet.Read())
                                    {
                                        decimal GiaVon = 0;
                                        decimal TONCUOIKYSL = 0;
                                        string maVatTuBoHang = dataReaderBoHangChiTiet["MAHANG"] != null ? dataReaderBoHangChiTiet["MAHANG"].ToString().ToUpper().Trim() : "";
                                        decimal.TryParse(dataReaderBoHangChiTiet["SOLUONG"].ToString(), out SOLUONG);
                                        //dataDto.SOLUONG = SOLUONG;
                                        SqlCommand cmdVatTu = new SqlCommand();
                                        cmdVatTu.Connection = connection;
                                        cmdVatTu.CommandText = "SELECT b.GIAVON,b.TONCUOIKYSL FROM V_VATTU_GIABAN a JOIN " + TABLE_NAME + " b ON a.MAHANG = b.MAHANG AND b.MAKHO = '" + Session.Session.CurrentWareHouse + "' WHERE a.MAHANG = '" + maVatTuBoHang + "' AND a.MADONVI = '" + Session.Session.CurrentUnitCode + "'";
                                        SqlDataReader dataReaderVatTu = cmdVatTu.ExecuteReader();
                                        if (dataReaderVatTu.HasRows)
                                        {
                                            while (dataReaderVatTu.Read())
                                            {
                                                decimal.TryParse(dataReaderVatTu["GIAVON"].ToString(), out GiaVon);
                                                decimal.TryParse(dataReaderVatTu["TONCUOIKYSL"].ToString(), out TONCUOIKYSL);
                                            }
                                            dataDto.GIAVON += GiaVon * SOLUONG;
                                            dataDto.TONCUOIKYSL += TONCUOIKYSL;
                                        }
                                    }
                                }

                                if (PhuongThucTinhGia == EnumCommon.METHOD_PRICE.GIABANLE_VAT)
                                {
                                    dataDto.GIABANLE_VAT = GIABANLE_VAT;
                                }
                                else
                                {
                                    if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                    {
                                        dataDto.GIABANLE_VAT = GIABANBUON_VAT;
                                    }
                                    else
                                    {
                                        dataDto.GIABANLE_VAT = dataDto.GIAVON * (1 + dataDto.GIATRI_THUE_RA / 100);

                                    }
                                }
                                dataDto.GIAVON = dataDto.GIAVON * (1 + dataDto.GIATRI_THUE_RA / 100);
                                listDataDto.Add(dataDto);
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
            return listDataDto;
        }

        public static List<VATTU_DTO> BAN_THEO_MAHANG_SQLSERVER(string MaHang, string TABLE_NAME, EnumCommon.METHOD_PRICE PhuongThucTinhGia, decimal SoLuong)
        {
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Clear();
                        cmd.CommandText = string.Format(@"SELECT A.MAHANG,A.TENHANG,E.TENDONVITINH AS DONVITINH,A.BARCODE,A.MANHACUNGCAP,A.MATHUE_RA,ISNULL(D.GIATRI, 0) AS GIATRI_THUE_RA,A.ITEMCODE,B.GIABANBUON_VAT,B.GIABANLE_VAT,C.GIAVON,C.TONCUOIKYSL FROM dbo.MATHANG A INNER JOIN dbo.MATHANG_GIA B ON A.MAHANG = B.MAHANG INNER JOIN " + TABLE_NAME + " C ON A.MAHANG = C.MAHANG INNER JOIN dbo.THUE D ON A.MATHUE_RA = D.MATHUE INNER JOIN dbo.DONVITINH E ON A.MADONVITINH = E.MADONVITINH AND C.MAKHO = '"+ Session.Session.CurrentWareHouse + "' AND A.MAHANG = '"+ MaHang + "' AND A.UNITCODE = '"+ Session.Session.CurrentUnitCode + "'");
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                VATTU_DTO dataDto = new VATTU_DTO();
                                dataDto.MAHANG = dataReader["MAHANG"] != DBNull.Value ? dataReader["MAHANG"].ToString() : "";
                                dataDto.TENHANG = dataReader["TENHANG"] != DBNull.Value ? dataReader["TENHANG"].ToString() : "";
                                dataDto.DONVITINH = dataReader["DONVITINH"] != DBNull.Value ? dataReader["DONVITINH"].ToString() : "";
                                dataDto.BARCODE = dataReader["BARCODE"] != DBNull.Value ? dataReader["BARCODE"].ToString() : "";
                                dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"] != DBNull.Value ? dataReader["MANHACUNGCAP"].ToString() : "";
                                dataDto.MATHUE_RA = dataReader["MATHUE_RA"] != DBNull.Value ? dataReader["MATHUE_RA"].ToString() : "";
                                dataDto.GIATRI_THUE_RA = dataReader["GIATRI_THUE_RA"] != DBNull.Value ? decimal.Parse(dataReader["GIATRI_THUE_RA"].ToString()) : 0;
                                dataDto.GIABANBUON_VAT = dataReader["GIABANBUON_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANBUON_VAT"].ToString()) : 0;
                                dataDto.GIABANLE_VAT = dataReader["GIABANLE_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANLE_VAT"].ToString()) : 0;
                                dataDto.GIAVON = dataReader["GIAVON"] != DBNull.Value ? decimal.Parse(dataReader["GIAVON"].ToString()) : 0;
                                dataDto.TONCUOIKYSL = dataReader["TONCUOIKYSL"] != DBNull.Value ? decimal.Parse(dataReader["TONCUOIKYSL"].ToString()) : 0;
                                dataDto.ITEMCODE = dataReader["ITEMCODE"] != DBNull.Value ? dataReader["ITEMCODE"].ToString() : "";
                                dataDto.SOLUONG = SoLuong;
                                dataDto.LAMACAN = false;
                                if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                {
                                    dataDto.GIABANLE_VAT = dataDto.GIABANBUON_VAT;
                                }
                                else if (Session.Session.CurrentLoaiGiaoDich == "GIAVON")
                                {
                                    dataDto.GIABANLE_VAT = dataDto.GIAVON * (1 + dataDto.GIATRI_THUE_RA / 100);
                                }

                                listDataDto.Add(dataDto);
                            }
                            dataReader.Close();
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
            return listDataDto;
        }


        public static List<VATTU_DTO> BAN_THEO_BARCODE_SQLSERVER(string MaHang, string TABLE_NAME, EnumCommon.METHOD_PRICE PhuongThucTinhGia, decimal SoLuong)
        {
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Clear();
                        cmd.CommandText = string.Format(@"SELECT A.MAHANG,A.TENHANG,E.TENDONVITINH AS DONVITINH,A.BARCODE,A.MANHACUNGCAP,A.MATHUE_RA,ISNULL(D.GIATRI, 0) AS GIATRI_THUE_RA,A.ITEMCODE,B.GIABANBUON_VAT,B.GIABANLE_VAT,C.GIAVON,C.TONCUOIKYSL FROM dbo.MATHANG A INNER JOIN dbo.MATHANG_GIA B ON A.MAHANG = B.MAHANG INNER JOIN " + TABLE_NAME + " C ON A.MAHANG = C.MAHANG INNER JOIN dbo.THUE D ON A.MATHUE_RA = D.MATHUE INNER JOIN dbo.DONVITINH E ON A.MADONVITINH = E.MADONVITINH AND C.MAKHO = '"+ Session.Session.CurrentWareHouse + "' AND A.BARCODE LIKE '%" + MaHang + "%' AND A.UNITCODE = '"+ Session.Session.CurrentUnitCode + "'");
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                VATTU_DTO dataDto = new VATTU_DTO();
                                dataDto.MAHANG = dataReader["MAHANG"] != DBNull.Value ? dataReader["MAHANG"].ToString() : "";
                                dataDto.TENHANG = dataReader["TENHANG"] != DBNull.Value ? dataReader["TENHANG"].ToString() : "";
                                dataDto.DONVITINH = dataReader["DONVITINH"] != DBNull.Value ? dataReader["DONVITINH"].ToString() : "";
                                dataDto.BARCODE = dataReader["BARCODE"] != DBNull.Value ? dataReader["BARCODE"].ToString() : "";
                                dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"] != DBNull.Value ? dataReader["MANHACUNGCAP"].ToString() : "";
                                dataDto.MATHUE_RA = dataReader["MATHUE_RA"] != DBNull.Value ? dataReader["MATHUE_RA"].ToString() : "";
                                dataDto.GIATRI_THUE_RA = dataReader["GIATRI_THUE_RA"] != DBNull.Value ? decimal.Parse(dataReader["GIATRI_THUE_RA"].ToString()) : 0;
                                dataDto.GIABANBUON_VAT = dataReader["GIABANBUON_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANBUON_VAT"].ToString()) : 0;
                                dataDto.GIABANLE_VAT = dataReader["GIABANLE_VAT"] != DBNull.Value ? decimal.Parse(dataReader["GIABANLE_VAT"].ToString()) : 0;
                                dataDto.GIAVON = dataReader["GIAVON"] != DBNull.Value ? decimal.Parse(dataReader["GIAVON"].ToString()) : 0;
                                dataDto.TONCUOIKYSL = dataReader["TONCUOIKYSL"] != DBNull.Value ? decimal.Parse(dataReader["TONCUOIKYSL"].ToString()) : 0;
                                dataDto.ITEMCODE = dataReader["ITEMCODE"] != DBNull.Value ? dataReader["ITEMCODE"].ToString() : "";
                                dataDto.SOLUONG = SoLuong;
                                dataDto.LAMACAN = false;
                                if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                {
                                    dataDto.GIABANLE_VAT = dataDto.GIABANBUON_VAT;
                                }
                                else if (Session.Session.CurrentLoaiGiaoDich == "GIAVON")
                                {
                                    dataDto.GIABANLE_VAT = dataDto.GIAVON * (1 + dataDto.GIATRI_THUE_RA / 100);
                                }

                                listDataDto.Add(dataDto);
                            }
                            dataReader.Close();
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
            return listDataDto;
        }


        /// <summary>
        /// TÌM KIẾM MÃ HÀNG TỪ CƠ SỞ DỮ LIỆU SERVER
        /// </summary>
        /// <param name="MaHang"></param>
        /// <param name="PhuongThucTinhGia"></param>
        /// <returns></returns>
        public static List<VATTU_DTO> GET_DATA_VATTU_FROM_CSDL_ORACLE(string MaHang, EnumCommon.METHOD_PRICE PhuongThucTinhGia, decimal SoLuong)
        {
            string TABLE_NAME = GET_TABLE_NAME_NGAYHACHTOAN_CSDL_ORACLE();
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();
            string beginCharacter = string.Empty;
            if (MaHang.Length >= 4)
            {
                if (!string.IsNullOrEmpty(MaHang)) beginCharacter = MaHang.Substring(0, 2);
                //TRƯỜNG HỢP BÁN MÃ CÂN ĐIỆN TỬ
                if (beginCharacter.Equals(KeyMaCan) && MaHang.Length > 9)
                {
                    listDataDto = BAN_THEO_MACAN_DIENTU_ORACLE(MaHang, TABLE_NAME, PhuongThucTinhGia);
                }
                //BÁN MÃ BÓ HÀNG
                if (beginCharacter.Equals("BH"))
                {
                    listDataDto = BAN_THEO_MABOHANG_ORACLE(MaHang, TABLE_NAME, PhuongThucTinhGia);
                }
                else if (MaHang.Length == 7)
                {
                    listDataDto = BAN_THEO_MAHANG_ORACLE(MaHang, TABLE_NAME, PhuongThucTinhGia, SoLuong);
                }
                else
                {
                    listDataDto = BAN_THEO_BARCODE_ORACLE(MaHang, TABLE_NAME, PhuongThucTinhGia, SoLuong);
                }
            }
            return listDataDto;
        }


        public static List<VATTU_DTO> GET_DATA_VATTU_FROM_CSDL_SQLSERVER(string MaHang, EnumCommon.METHOD_PRICE PhuongThucTinhGia, decimal SoLuong)
        {
            string TABLE_NAME = GET_TABLE_NAME_NGAYHACHTOAN_CSDL_SQLSERVER();
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();
            string beginCharacter = string.Empty;
            if (MaHang.Length >= 4)
            {
                if (!string.IsNullOrEmpty(MaHang)) beginCharacter = MaHang.Substring(0, 2);
                //TRƯỜNG HỢP BÁN MÃ CÂN ĐIỆN TỬ
                if (beginCharacter.Equals(KeyMaCan) && MaHang.Length > 9)
                {
                    listDataDto = BAN_THEO_MACAN_DIENTU_SQLSERVER(MaHang, TABLE_NAME, PhuongThucTinhGia);
                }
                //BÁN MÃ BÓ HÀNG
                if (beginCharacter.Equals("BH"))
                {
                    listDataDto = BAN_THEO_MABOHANG_SQLSERVER(MaHang, TABLE_NAME, PhuongThucTinhGia);
                }
                else if (MaHang.Length == 7)
                {
                    listDataDto = BAN_THEO_MAHANG_SQLSERVER(MaHang, TABLE_NAME, PhuongThucTinhGia, SoLuong);
                }
                else
                {
                    listDataDto = BAN_THEO_BARCODE_SQLSERVER(MaHang, TABLE_NAME, PhuongThucTinhGia, SoLuong);
                }
            }
            return listDataDto;
        }

        //TÍNH TOÁN KHUYẾN MÃI
        public static CAL_KHUYENMAI_OBJ TINHTOAN_KHUYENMAI(string maVatTu, EnumCommon.METHOD_PRICE method)
        {
            CAL_KHUYENMAI_OBJ RESULT = new CAL_KHUYENMAI_OBJ();
            if (Config.CheckConnectToServer()) // nếu có mạng lan
            {
                CAL_KHUYENMAI_OBJ RESULT_NHACUNGCAP = CACULATION_KHUYENMAI_CHIETKHAU_NHACUNGCAP_ORACLE(maVatTu);
                CAL_KHUYENMAI_OBJ RESULT_LOAIHANG = CACULATION_KHUYENMAI_CHIETKHAU_LOAIHANG_ORACLE(maVatTu);
                CAL_KHUYENMAI_OBJ RESULT_NHOMHANG = CACULATION_KHUYENMAI_CHIETKHAU_NHOMHANG_ORACLE(maVatTu);
                if ((RESULT_NHACUNGCAP != null && RESULT_LOAIHANG != null) || (RESULT_LOAIHANG != null && RESULT_NHOMHANG != null) || (RESULT_NHACUNGCAP != null && RESULT_NHOMHANG != null))
                {
                    RESULT = null;
                    NotificationLauncher.ShowNotificationError("Thông báo", "Trùng lặp chương trình khuyến mãi", 1, "0x1", "0x8", "normal");
                }
                else if (RESULT_NHACUNGCAP != null) RESULT = RESULT_NHACUNGCAP;
                else if (RESULT_LOAIHANG != null) RESULT = RESULT_LOAIHANG;
                else if (RESULT_NHOMHANG != null) RESULT = RESULT_NHOMHANG;
                else RESULT = null;
            }
            else
            {
                CAL_KHUYENMAI_OBJ RESULT_NHACUNGCAP = CACULATION_KHUYENMAI_CHIETKHAU_NHACUNGCAP_SQLSERVER(maVatTu);
                CAL_KHUYENMAI_OBJ RESULT_LOAIHANG = CACULATION_KHUYENMAI_CHIETKHAU_LOAIHANG_SQLSERVER(maVatTu);
                CAL_KHUYENMAI_OBJ RESULT_NHOMHANG = CACULATION_KHUYENMAI_CHIETKHAU_NHOMHANG_SQLSERVER(maVatTu);
                if ((RESULT_NHACUNGCAP != null && RESULT_LOAIHANG != null) || (RESULT_LOAIHANG != null && RESULT_NHOMHANG != null) || (RESULT_NHACUNGCAP != null && RESULT_NHOMHANG != null))
                {
                    RESULT = null;
                    NotificationLauncher.ShowNotificationError("Thông báo", "Trùng lặp chương trình khuyến mãi", 1, "0x1", "0x8", "normal");
                }
                else if (RESULT_NHACUNGCAP != null) RESULT = RESULT_NHACUNGCAP;
                else if (RESULT_LOAIHANG != null) RESULT = RESULT_LOAIHANG;
                else if (RESULT_NHOMHANG != null) RESULT = RESULT_NHOMHANG;
                else RESULT = null;
            }
            return RESULT;
        }


        //Lấy ngày hạch toán , Số giao dịch
        public static DateTime GET_NGAYHACHTOAN_CSDL_ORACLE()
        {
            DateTime result = DateTime.Now;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT KY,NGAYHACHTOAN FROM (SELECT MAX(KYKETOAN) AS KY,TUNGAY AS NGAYHACHTOAN FROM KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10 AND NAM = (SELECT MAX(NAM) FROM KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "') GROUP BY KYKETOAN,TUNGAY ORDER BY KY DESC) WHERE ROWNUM = 1");
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result = (DateTime)dataReader["NGAYHACHTOAN"];
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
            return result;
        }

        //Lấy ngày hạch toán , Số giao dịch
        public static DateTime GET_NGAYHACHTOAN_CSDL_SQLSERVER()
        {
            DateTime result = DateTime.Now;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT TOP 1 MAX(KYKETOAN) AS KY,TUNGAY AS NGAYHACHTOAN FROM dbo.KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10 AND NAM = (SELECT MAX(NAM) FROM KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "') GROUP BY KYKETOAN,TUNGAY ORDER BY KY DESC");
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result = (DateTime)dataReader["NGAYHACHTOAN"];
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
            return result;
        }
        //Lấy table name Ngày hạch toán
        public static string GET_TABLE_NAME_NGAYHACHTOAN_CSDL_ORACLE()
        {
            string result = string.Empty;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {

                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT KY,NAM FROM (SELECT MAX(KYKETOAN) AS KY,NAM FROM KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10 AND NAM = (SELECT MAX(NAM) FROM KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "') GROUP BY KYKETOAN,NAM ORDER BY KY DESC) WHERE ROWNUM = 1");
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            string KY = string.Empty;
                            string NAM = string.Empty;
                            while (dataReader.Read())
                            {
                                KY = dataReader["KY"].ToString();
                                Session.Session.CurrentPeriod = KY;
                                NAM = dataReader["NAM"].ToString();
                                Session.Session.CurrentYear = NAM;
                            }
                            result = ("XNT_" + NAM + "_KY_" + KY).ToUpper().Trim();
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
            return result;
        }


        public static string GET_TABLE_NAME_NGAYHACHTOAN_CSDL_SQLSERVER()
        {
            string result = string.Empty;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {

                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT TOP 1 MAX(KYKETOAN) AS KY,NAM FROM dbo.KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10 AND NAM = (SELECT MAX(NAM) FROM dbo.KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "') GROUP BY KYKETOAN,NAM ORDER BY KY DESC");
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            string KY = string.Empty;
                            string NAM = string.Empty;
                            while (dataReader.Read())
                            {
                                KY = dataReader["KY"].ToString();
                                Session.Session.CurrentPeriod = KY;
                                NAM = dataReader["NAM"].ToString();
                                Session.Session.CurrentYear = NAM;
                            }
                            result = ("XNT_" + NAM + "_KY_" + KY).ToUpper().Trim();
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
            return result;
        }

        // TÍNH KHUYẾN MẠI FROM ORACLE
        public static CAL_KHUYENMAI_OBJ CACULATION_KHUYENMAI_CHIETKHAU_NHACUNGCAP_ORACLE(string MaHang)
        {
            CAL_KHUYENMAI_OBJ result = new CAL_KHUYENMAI_OBJ();
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT A.MA_KHUYENMAI,A.TUGIO,A.DENGIO,A.DIENGIAI,D.MAHANG,B.SOLUONG,B.GIATRI_KHUYENMAI 
                        FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI  INNER JOIN NHACUNGCAP C ON B.MAHANG = C.MANHACUNGCAP INNER JOIN MATHANG D ON C.MANHACUNGCAP = D.MANHACUNGCAP 
                        WHERE TO_DATE('" + DateTime.Now.ToString("dd-MM-yyyy") + "', 'DD/MM/YY') BETWEEN TO_DATE(A.TUNGAY, 'DD/MM/YY') AND TO_DATE(A.DENNGAY, 'DD/MM/YY') AND A.TRANGTHAI = 10 AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGNHACUNGCAP' AND D.MAHANG = '" + MaHang + "' AND A.TUNGAY = (SELECT MAX(A.TUNGAY) FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI INNER JOIN NHACUNGCAP C ON B.MAHANG = C.MANHACUNGCAP INNER JOIN MATHANG D ON C.MANHACUNGCAP = D.MANHACUNGCAP WHERE TO_DATE('" + DateTime.Now.ToString("dd-MM-yyyy") + "', 'DD/MM/YY') BETWEEN TO_DATE(A.TUNGAY, 'DD/MM/YY') AND TO_DATE(A.DENNGAY, 'DD/MM/YY') AND A.TRANGTHAI = 10 AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGNHACUNGCAP' AND D.MAHANG = '" + MaHang + "') ");
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                decimal SOLUONG, GIATRI_KHUYENMAI = 0;
                                KHUYENMAI_DTO dto = new KHUYENMAI_DTO();
                                dto.MA_KHUYENMAI = dataReader["MA_KHUYENMAI"].ToString();
                                dto.TUGIO = dataReader["TUGIO"].ToString();
                                dto.DENGIO = dataReader["DENGIO"].ToString();
                                dto.DIENGIAI = dataReader["DIENGIAI"].ToString();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                decimal.TryParse(dataReader["GIATRI_KHUYENMAI"].ToString(), out GIATRI_KHUYENMAI);
                                if (!string.IsNullOrEmpty(dto.TUGIO) && !string.IsNullOrEmpty(dto.DENGIO))
                                {
                                    int getHour = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                                    string[] tugio = dto.TUGIO.Split(':');
                                    string[] dengio = dto.DENGIO.Split(':');
                                    int minuteTuGio = Int32.Parse(tugio[0]) * 60 + Int32.Parse(tugio[1]);
                                    int minuteDenGio = Int32.Parse(dengio[0]) * 60 + Int32.Parse(dengio[1]);
                                    if (minuteTuGio <= getHour && getHour <= minuteDenGio)
                                    {
                                        result.MA_KHUYENMAI = dto.MA_KHUYENMAI;
                                        result.GIATRI_KHUYENMAI = GIATRI_KHUYENMAI;
                                    }
                                }
                            }
                            dataReader.Close();
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
            if(result != null && string.IsNullOrEmpty(result.MA_KHUYENMAI))
            {
                result = null;
            }
            return result;
        }

        // TÍNH KHUYẾN MẠI FROM ORACLE
        public static CAL_KHUYENMAI_OBJ CACULATION_KHUYENMAI_CHIETKHAU_LOAIHANG_ORACLE(string MaHang)
        {
            CAL_KHUYENMAI_OBJ result = new CAL_KHUYENMAI_OBJ();
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT A.MA_KHUYENMAI,A.TUGIO,A.DENGIO,A.DIENGIAI,D.MAHANG,B.SOLUONG,B.GIATRI_KHUYENMAI 
                        FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI  INNER JOIN LOAIHANG C ON B.MAHANG = C.MALOAI INNER JOIN MATHANG D ON C.MALOAI = D.MALOAI 
                        WHERE TO_DATE('" + DateTime.Now.ToString("dd-MM-yyyy") + "', 'DD/MM/YY') BETWEEN TO_DATE(A.TUNGAY, 'DD/MM/YY') AND TO_DATE(A.DENNGAY, 'DD/MM/YY') AND A.TRANGTHAI = 10 AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGLOAIHANG' AND D.MAHANG = '" + MaHang + "' AND A.TUNGAY = (SELECT MAX(A.TUNGAY) FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI INNER JOIN LOAIHANG C ON B.MAHANG = C.MALOAI INNER JOIN MATHANG D ON C.MALOAI = D.MALOAI WHERE TO_DATE('" + DateTime.Now.ToString("dd-MM-yyyy") + "', 'DD/MM/YY') BETWEEN TO_DATE(A.TUNGAY, 'DD/MM/YY') AND TO_DATE(A.DENNGAY, 'DD/MM/YY') AND A.TRANGTHAI = 10 AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGLOAIHANG' AND D.MAHANG = '" + MaHang + "') ");
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                decimal SOLUONG, GIATRI_KHUYENMAI = 0;
                                KHUYENMAI_DTO dto = new KHUYENMAI_DTO();
                                dto.MA_KHUYENMAI = dataReader["MA_KHUYENMAI"].ToString();
                                dto.TUGIO = dataReader["TUGIO"].ToString();
                                dto.DENGIO = dataReader["DENGIO"].ToString();
                                dto.DIENGIAI = dataReader["DIENGIAI"].ToString();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                decimal.TryParse(dataReader["GIATRI_KHUYENMAI"].ToString(), out GIATRI_KHUYENMAI);
                                if (!string.IsNullOrEmpty(dto.TUGIO) && !string.IsNullOrEmpty(dto.DENGIO))
                                {
                                    int getHour = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                                    string[] tugio = dto.TUGIO.Split(':');
                                    string[] dengio = dto.DENGIO.Split(':');
                                    int minuteTuGio = Int32.Parse(tugio[0]) * 60 + Int32.Parse(tugio[1]);
                                    int minuteDenGio = Int32.Parse(dengio[0]) * 60 + Int32.Parse(dengio[1]);
                                    if (minuteTuGio <= getHour && getHour <= minuteDenGio)
                                    {
                                        result.MA_KHUYENMAI = dto.MA_KHUYENMAI;
                                        result.GIATRI_KHUYENMAI = GIATRI_KHUYENMAI;
                                    }
                                }
                            }
                            dataReader.Close();
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
            if (result != null && string.IsNullOrEmpty(result.MA_KHUYENMAI))
            {
                result = null;
            }
            return result;
        }

        // TÍNH KHUYẾN MẠI FROM ORACLE
        public static CAL_KHUYENMAI_OBJ CACULATION_KHUYENMAI_CHIETKHAU_NHOMHANG_ORACLE(string MaHang)
        {
            CAL_KHUYENMAI_OBJ result = new CAL_KHUYENMAI_OBJ();
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT A.MA_KHUYENMAI,A.TUGIO,A.DENGIO,A.DIENGIAI,D.MAHANG,B.SOLUONG,B.GIATRI_KHUYENMAI 
                        FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI  INNER JOIN NHOMHANG C ON B.MAHANG = C.MANHOM INNER JOIN MATHANG D ON C.MANHOM = D.MANHOM 
                        WHERE TO_DATE('" + DateTime.Now.ToString("dd-MM-yyyy") + "', 'DD/MM/YY') BETWEEN TO_DATE(A.TUNGAY, 'DD/MM/YY') AND TO_DATE(A.DENNGAY, 'DD/MM/YY') AND A.TRANGTHAI = 10 AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGNHOMHANG' AND D.MAHANG = '" + MaHang + "' AND A.TUNGAY = (SELECT MAX(A.TUNGAY) FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI INNER JOIN NHOMHANG C ON B.MAHANG = C.MANHOM INNER JOIN MATHANG D ON C.MANHOM = D.MANHOM WHERE TO_DATE('" + DateTime.Now.ToString("dd-MM-yyyy") + "', 'DD/MM/YY') BETWEEN TO_DATE(A.TUNGAY, 'DD/MM/YY') AND TO_DATE(A.DENNGAY, 'DD/MM/YY') AND A.TRANGTHAI = 10 AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGNHOMHANG' AND D.MAHANG = '" + MaHang + "') ");
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                decimal SOLUONG, GIATRI_KHUYENMAI = 0;
                                KHUYENMAI_DTO dto = new KHUYENMAI_DTO();
                                dto.MA_KHUYENMAI = dataReader["MA_KHUYENMAI"].ToString();
                                dto.TUGIO = dataReader["TUGIO"].ToString();
                                dto.DENGIO = dataReader["DENGIO"].ToString();
                                dto.DIENGIAI = dataReader["DIENGIAI"].ToString();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                decimal.TryParse(dataReader["GIATRI_KHUYENMAI"].ToString(), out GIATRI_KHUYENMAI);
                                if (!string.IsNullOrEmpty(dto.TUGIO) && !string.IsNullOrEmpty(dto.DENGIO))
                                {
                                    int getHour = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                                    string[] tugio = dto.TUGIO.Split(':');
                                    string[] dengio = dto.DENGIO.Split(':');
                                    int minuteTuGio = Int32.Parse(tugio[0]) * 60 + Int32.Parse(tugio[1]);
                                    int minuteDenGio = Int32.Parse(dengio[0]) * 60 + Int32.Parse(dengio[1]);
                                    if (minuteTuGio <= getHour && getHour <= minuteDenGio)
                                    {
                                        result.MA_KHUYENMAI = dto.MA_KHUYENMAI;
                                        result.GIATRI_KHUYENMAI = GIATRI_KHUYENMAI;
                                    }
                                }
                            }
                            dataReader.Close();
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
            if (result != null && string.IsNullOrEmpty(result.MA_KHUYENMAI))
            {
                result = null;
            }
            return result;
        }


        // TÍNH KHUYẾN MẠI FROM ORACLE
        public static CAL_KHUYENMAI_OBJ CACULATION_KHUYENMAI_CHIETKHAU_NHACUNGCAP_SQLSERVER(string MaHang)
        {
            CAL_KHUYENMAI_OBJ result = new CAL_KHUYENMAI_OBJ();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT A.MA_KHUYENMAI,A.TUGIO,A.DENGIO,A.DIENGIAI,D.MAHANG,B.SOLUONG,B.GIATRI_KHUYENMAI 
                        FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI  INNER JOIN NHACUNGCAP C ON B.MAHANG = C.MANHACUNGCAP INNER JOIN MATHANG D ON C.MANHACUNGCAP = D.MANHACUNGCAP 
                        WHERE '" + DateTime.Now.ToString("yyyy-MM-dd") + "' BETWEEN A.TUNGAY AND A.DENNGAY AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGNHACUNGCAP' AND D.MAHANG = '" + MaHang + "' AND A.TUNGAY = (SELECT MAX(A.TUNGAY) FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI INNER JOIN NHACUNGCAP C ON B.MAHANG = C.MANHACUNGCAP INNER JOIN MATHANG D ON C.MANHACUNGCAP = D.MANHACUNGCAP WHERE '" + DateTime.Now.ToString("yyyy-MM-dd") + "' BETWEEN A.TUNGAY AND A.DENNGAY AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGNHACUNGCAP' AND D.MAHANG = '" + MaHang + "') ");
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                decimal SOLUONG, GIATRI_KHUYENMAI = 0;
                                KHUYENMAI_DTO dto = new KHUYENMAI_DTO();
                                dto.MA_KHUYENMAI = dataReader["MA_KHUYENMAI"].ToString();
                                dto.TUGIO = dataReader["TUGIO"].ToString();
                                dto.DENGIO = dataReader["DENGIO"].ToString();
                                dto.DIENGIAI = dataReader["DIENGIAI"].ToString();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                decimal.TryParse(dataReader["GIATRI_KHUYENMAI"].ToString(), out GIATRI_KHUYENMAI);
                                if (!string.IsNullOrEmpty(dto.TUGIO) && !string.IsNullOrEmpty(dto.DENGIO))
                                {
                                    int getHour = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                                    string[] tugio = dto.TUGIO.Split(':');
                                    string[] dengio = dto.DENGIO.Split(':');
                                    int minuteTuGio = Int32.Parse(tugio[0]) * 60 + Int32.Parse(tugio[1]);
                                    int minuteDenGio = Int32.Parse(dengio[0]) * 60 + Int32.Parse(dengio[1]);
                                    if (minuteTuGio <= getHour && getHour <= minuteDenGio)
                                    {
                                        result.MA_KHUYENMAI = dto.MA_KHUYENMAI;
                                        result.GIATRI_KHUYENMAI = GIATRI_KHUYENMAI;
                                    }
                                }
                            }
                            dataReader.Close();
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
            if (result != null && string.IsNullOrEmpty(result.MA_KHUYENMAI))
            {
                result = null;
            }
            return result;
        }

        // TÍNH KHUYẾN MẠI FROM ORACLE
        public static CAL_KHUYENMAI_OBJ CACULATION_KHUYENMAI_CHIETKHAU_LOAIHANG_SQLSERVER(string MaHang)
        {
            CAL_KHUYENMAI_OBJ result = new CAL_KHUYENMAI_OBJ();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT A.MA_KHUYENMAI,A.TUGIO,A.DENGIO,A.DIENGIAI,D.MAHANG,B.SOLUONG,B.GIATRI_KHUYENMAI 
                        FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI  INNER JOIN LOAIHANG C ON B.MAHANG = C.MALOAI INNER JOIN MATHANG D ON C.MALOAI = D.MALOAI 
                        WHERE '" + DateTime.Now.ToString("yyyy-MM-dd") + "' BETWEEN A.TUNGAY AND A.DENNGAY AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGLOAIHANG' AND D.MAHANG = '" + MaHang + "' AND A.TUNGAY = (SELECT MAX(A.TUNGAY) FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI INNER JOIN LOAIHANG C ON B.MAHANG = C.MALOAI INNER JOIN MATHANG D ON C.MALOAI = D.MALOAI WHERE '" + DateTime.Now.ToString("yyyy-MM-dd") + "' BETWEEN A.TUNGAY AND A.DENNGAY AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGLOAIHANG' AND D.MAHANG = '" + MaHang + "') ");
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                decimal SOLUONG, GIATRI_KHUYENMAI = 0;
                                KHUYENMAI_DTO dto = new KHUYENMAI_DTO();
                                dto.MA_KHUYENMAI = dataReader["MA_KHUYENMAI"].ToString();
                                dto.TUGIO = dataReader["TUGIO"].ToString();
                                dto.DENGIO = dataReader["DENGIO"].ToString();
                                dto.DIENGIAI = dataReader["DIENGIAI"].ToString();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                decimal.TryParse(dataReader["GIATRI_KHUYENMAI"].ToString(), out GIATRI_KHUYENMAI);
                                if (!string.IsNullOrEmpty(dto.TUGIO) && !string.IsNullOrEmpty(dto.DENGIO))
                                {
                                    int getHour = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                                    string[] tugio = dto.TUGIO.Split(':');
                                    string[] dengio = dto.DENGIO.Split(':');
                                    int minuteTuGio = Int32.Parse(tugio[0]) * 60 + Int32.Parse(tugio[1]);
                                    int minuteDenGio = Int32.Parse(dengio[0]) * 60 + Int32.Parse(dengio[1]);
                                    if (minuteTuGio <= getHour && getHour <= minuteDenGio)
                                    {
                                        result.MA_KHUYENMAI = dto.MA_KHUYENMAI;
                                        result.GIATRI_KHUYENMAI = GIATRI_KHUYENMAI;
                                    }
                                }
                            }
                            dataReader.Close();
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
            if (result != null && string.IsNullOrEmpty(result.MA_KHUYENMAI))
            {
                result = null;
            }
            return result;
        }

        // TÍNH KHUYẾN MẠI FROM SQLSERVER
        public static CAL_KHUYENMAI_OBJ CACULATION_KHUYENMAI_CHIETKHAU_NHOMHANG_SQLSERVER(string MaHang)
        {
            CAL_KHUYENMAI_OBJ result = new CAL_KHUYENMAI_OBJ();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT A.MA_KHUYENMAI,A.TUGIO,A.DENGIO,A.DIENGIAI,D.MAHANG,B.SOLUONG,B.GIATRI_KHUYENMAI 
                        FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI  INNER JOIN NHOMHANG C ON B.MAHANG = C.MANHOM INNER JOIN MATHANG D ON C.MANHOM = D.MANHOM 
                        WHERE '" + DateTime.Now.ToString("yyyy-MM-dd") + "' BETWEEN A.TUNGAY AND A.DENNGAY AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGNHOMHANG' AND D.MAHANG = '" + MaHang + "' AND A.TUNGAY = (SELECT MAX(A.TUNGAY) FROM KHUYENMAI a INNER JOIN KHUYENMAI_CHITIET b ON A.MA_KHUYENMAI = B.MA_KHUYENMAI INNER JOIN NHOMHANG C ON B.MAHANG = C.MANHOM INNER JOIN MATHANG D ON C.MANHOM = D.MANHOM WHERE '" + DateTime.Now.ToString("yyyy-MM-dd") + "' BETWEEN A.TUNGAY AND A.DENNGAY AND A.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND A.LOAI_KHUYENMAI = 'GGNHOMHANG' AND D.MAHANG = '" + MaHang + "') ");
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                decimal SOLUONG, GIATRI_KHUYENMAI = 0;
                                KHUYENMAI_DTO dto = new KHUYENMAI_DTO();
                                dto.MA_KHUYENMAI = dataReader["MA_KHUYENMAI"].ToString();
                                dto.TUGIO = dataReader["TUGIO"].ToString();
                                dto.DENGIO = dataReader["DENGIO"].ToString();
                                dto.DIENGIAI = dataReader["DIENGIAI"].ToString();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                decimal.TryParse(dataReader["GIATRI_KHUYENMAI"].ToString(), out GIATRI_KHUYENMAI);
                                if (!string.IsNullOrEmpty(dto.TUGIO) && !string.IsNullOrEmpty(dto.DENGIO))
                                {
                                    int getHour = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                                    string[] tugio = dto.TUGIO.Split(':');
                                    string[] dengio = dto.DENGIO.Split(':');
                                    int minuteTuGio = Int32.Parse(tugio[0]) * 60 + Int32.Parse(tugio[1]);
                                    int minuteDenGio = Int32.Parse(dengio[0]) * 60 + Int32.Parse(dengio[1]);
                                    if (minuteTuGio <= getHour && getHour <= minuteDenGio)
                                    {
                                        result.MA_KHUYENMAI = dto.MA_KHUYENMAI;
                                        result.GIATRI_KHUYENMAI = GIATRI_KHUYENMAI;
                                    }
                                }
                            }
                            dataReader.Close();
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
            if (result != null && string.IsNullOrEmpty(result.MA_KHUYENMAI))
            {
                result = null;
            }
            return result;
        }

        //Lấy ngày hạch toán , Số giao dịch
        public static string INIT_CODE_TRADE()
        {
            string result = string.Empty;
            string machineName = Environment.MachineName;
            string time = DateTime.Now.ToString("yyMMddHHmmss");
            result = machineName + "-" + time;
            return result;
        }

        public static string CONVERT_MACAN_TO_MAHANG_ORACLE(string KEY, string UNITCODE)
        {
            string _MAHANG = "";
            if (!string.IsNullOrEmpty(KEY))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT a.MAHANG FROM MATHANG a INNER JOIN MATHANG_GIA b WHERE a.ITEMCODE = '" + KEY + "' AND a.UNITCODE = '" + UNITCODE + "' ");
                            OracleCommand commamdVatTu = new OracleCommand();
                            commamdVatTu.Connection = connection;
                            commamdVatTu.CommandText = querrySelect;
                            OracleDataReader dataReaderVatTu = commamdVatTu.ExecuteReader();
                            if (dataReaderVatTu.HasRows)
                            {
                                while (dataReaderVatTu.Read())
                                {
                                    if (dataReaderVatTu["MAHANG"] != null)
                                    {
                                        _MAHANG = dataReaderVatTu["MAHANG"].ToString().ToUpper().Trim();
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
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return _MAHANG;
        }

        public static string CONVERT_MACAN_TO_MAHANG_SQLSERVER(string KEY, string UNITCODE)
        {
            string _MAHANG = "";
            if (!string.IsNullOrEmpty(KEY))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT a.MAHANG FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG WHERE a.ITEMCODE = '" + KEY + "' AND a.UNITCODE = '" + UNITCODE + "' ");
                            SqlCommand commamdVatTu = new SqlCommand();
                            commamdVatTu.Connection = connection;
                            commamdVatTu.CommandText = querrySelect;
                            SqlDataReader dataReaderVatTu = commamdVatTu.ExecuteReader();
                            if (dataReaderVatTu.HasRows)
                            {
                                while (dataReaderVatTu.Read())
                                {
                                    if (dataReaderVatTu["MAHANG"] != null)
                                    {
                                        _MAHANG = dataReaderVatTu["MAHANG"].ToString().ToUpper().Trim();
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
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return _MAHANG;
        }

        public static string CONVERT_BARCODE_TO_MAHANG_ORACLE(string KEY, string UNITCODE)
        {
            string _MAHANG = "";
            if (!string.IsNullOrEmpty(KEY))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT a.MAHANG FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG WHERE a.BARCODE LIKE '%" + KEY + "%' AND a.UNITCODE = '" + UNITCODE + "' ");
                            OracleCommand commamdVatTu = new OracleCommand();
                            commamdVatTu.Connection = connection;
                            commamdVatTu.CommandText = querrySelect;
                            OracleDataReader dataReaderVatTu = commamdVatTu.ExecuteReader();
                            if (dataReaderVatTu.HasRows)
                            {
                                while (dataReaderVatTu.Read())
                                {
                                    if (dataReaderVatTu["MAHANG"] != null)
                                    {
                                        _MAHANG = dataReaderVatTu["MAHANG"].ToString().ToUpper().Trim();
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
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return _MAHANG;
        }

        public static string CONVERT_BARCODE_TO_MAHANG_SQLSERVER(string KEY, string UNITCODE)
        {
            string _MAHANG = "";
            if (!string.IsNullOrEmpty(KEY))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT a.MAHANG FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG WHERE a.BARCODE LIKE '%" + KEY + "%' AND a.UNITCODE = '" + UNITCODE + "' ");
                            SqlCommand commamdVatTu = new SqlCommand();
                            commamdVatTu.Connection = connection;
                            commamdVatTu.CommandText = querrySelect;
                            SqlDataReader dataReaderVatTu = commamdVatTu.ExecuteReader();
                            if (dataReaderVatTu.HasRows)
                            {
                                while (dataReaderVatTu.Read())
                                {
                                    if (dataReaderVatTu["MAHANG"] != null)
                                    {
                                        _MAHANG = dataReaderVatTu["MAHANG"].ToString().ToUpper().Trim();
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
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return _MAHANG;
        }

        public static EXTEND_VATTU_DTO LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(string MaHang, string UnitCode)
        {
            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
            if (!string.IsNullOrEmpty(MaHang))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT A.TENHANG,D.TENDONVITINH AS DONVITINH,a.BARCODE,a.MATHUE_RA,C.GIATRI AS GIATRI_THUE_RA,B.TYLE_LAILE,b.GIABANLE_VAT FROM MATHANG a 
                            INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN THUE C ON A.MATHUE_RA = C.MATHUE 
                            INNER JOIN DONVITINH d ON a.MADONVITINH = D.MADONVITINH WHERE a.MAHANG = '" + MaHang + "' AND a.UNITCODE = '" + UnitCode + "'");
                            SqlCommand commamdSelectVatTu = new SqlCommand();
                            commamdSelectVatTu.Connection = connection;
                            commamdSelectVatTu.CommandText = querrySelect;
                            SqlDataReader dataReaderSelectVatTu = commamdSelectVatTu.ExecuteReader();
                            if (dataReaderSelectVatTu.HasRows)
                            {
                                while (dataReaderSelectVatTu.Read())
                                {
                                    if (dataReaderSelectVatTu["TENHANG"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.TENHANG = dataReaderSelectVatTu["TENHANG"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.TENHANG = "";
                                    }
                                    if (dataReaderSelectVatTu["DONVITINH"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.DONVITINH = dataReaderSelectVatTu["DONVITINH"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.DONVITINH = "";
                                    }
                                    if (dataReaderSelectVatTu["BARCODE"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.BARCODE = dataReaderSelectVatTu["BARCODE"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.BARCODE = "";
                                    }
                                    if (dataReaderSelectVatTu["MATHUE_RA"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.MATHUE_RA = dataReaderSelectVatTu["MATHUE_RA"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.MATHUE_RA = "";
                                    }
                                    decimal GIATRI_THUE_RA = 0;
                                    if (dataReaderSelectVatTu["GIATRI_THUE_RA"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["GIATRI_THUE_RA"].ToString(), out GIATRI_THUE_RA);
                                        _EXTEND_VATTU_DTO.GIATRI_THUE_RA = GIATRI_THUE_RA;
                                    }
                                    decimal TYLE_LAILE = 0;
                                    if (dataReaderSelectVatTu["TYLE_LAILE"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["TYLE_LAILE"].ToString(), out TYLE_LAILE);
                                        _EXTEND_VATTU_DTO.TYLE_LAILE = TYLE_LAILE;
                                    }
                                    decimal GIABANLE_VAT = 0;
                                    if (dataReaderSelectVatTu["GIABANLE_VAT"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                        _EXTEND_VATTU_DTO.GIABANLE_VAT = GIABANLE_VAT;
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
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return _EXTEND_VATTU_DTO;
        }
        public static List<EXTEND_BOHANGCHITIET_DTO> LAYDULIEU_BOHANGCHITIET_FROM_DATABASE_SQLSERVER(string MaBoHang, string UnitCode)
        {
            List<EXTEND_BOHANGCHITIET_DTO> _LST_EXTEND_BOHANGCHITIET_DTO = new List<EXTEND_BOHANGCHITIET_DTO>();
            if (!string.IsNullOrEmpty(MaBoHang))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            SqlCommand commandBoHang = new SqlCommand();
                            commandBoHang.Connection = connection;
                            commandBoHang.CommandText = string.Format(@"SELECT DM_BOHANGCHITIET.MAHANG,DM_BOHANGCHITIET.SOLUONG,DM_BOHANGCHITIET.TYLECKLE,DM_BOHANGCHITIET.TONGLE FROM dbo.DM_BOHANG INNER JOIN dbo.DM_BOHANGCHITIET ON DM_BOHANG.MABOHANG = DM_BOHANGCHITIET.MABOHANG WHERE DM_BOHANG.MABOHANG = '" + MaBoHang + "' AND DM_BOHANG.UNITCODE = '" + UnitCode + "'");
                            SqlDataReader dataReaderBoHang = commandBoHang.ExecuteReader();
                            if (dataReaderBoHang.HasRows)
                            {
                                while (dataReaderBoHang.Read())
                                {
                                    EXTEND_BOHANGCHITIET_DTO _EXTEND_BOHANGCHITIET_DTO = new EXTEND_BOHANGCHITIET_DTO();
                                    if (dataReaderBoHang["MAHANG"] != null)
                                    {
                                        _EXTEND_BOHANGCHITIET_DTO.MAHANG = dataReaderBoHang["MAHANG"].ToString();
                                    }

                                    decimal SOLUONG = 0;
                                    if (dataReaderBoHang["SOLUONG"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["SOLUONG"].ToString(), out SOLUONG);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.SOLUONG = SOLUONG;

                                    decimal TYLECKLE = 0;
                                    if (dataReaderBoHang["TYLECKLE"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["TYLECKLE"].ToString(), out TYLECKLE);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.TYLECKLE = TYLECKLE;

                                    decimal TONGLE = 0;
                                    if (dataReaderBoHang["TONGLE"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["TONGLE"].ToString(), out TONGLE);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.TONGLE = TONGLE;
                                    _LST_EXTEND_BOHANGCHITIET_DTO.Add(_EXTEND_BOHANGCHITIET_DTO);
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return _LST_EXTEND_BOHANGCHITIET_DTO;
        }
        public static EXTEND_VAT_BOHANG LAYDULIEU_VAT_BOHANG_FROM_DATABASE_ORACLE(string MaBoHang, string UnitCode)
        {
            EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();
            if (!string.IsNullOrEmpty(MaBoHang))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT
                                                                MAX(c.MATHUE_RA) AS MATHUE_RA,
                                                                MAX(d.GIATRI) AS GIATRI_THUE_RA
                                                            FROM
                                                                BOHANG a INNER JOIN BOHANG_CHITIET b ON a.MABOHANG = b.MABOHANG
                                                                INNER JOIN MATHANG c ON b.MAHANG = c.MAHANG
                                                                INNER JOIN THUE d on c.MATHUE_RA = d.MATHUE
                                                                WHERE a.MABOHANG = '" + MaBoHang + "' AND a.UNITCODE = '" + UnitCode + "' ");
                            OracleCommand commamdSelectVatBoHang = new OracleCommand();
                            commamdSelectVatBoHang.Connection = connection;
                            commamdSelectVatBoHang.CommandText = querrySelect;
                            OracleDataReader dataReaderSelectVatBoHang = commamdSelectVatBoHang.ExecuteReader();
                            if (dataReaderSelectVatBoHang.HasRows)
                            {
                                while (dataReaderSelectVatBoHang.Read())
                                {
                                    _EXTEND_VAT_BOHANG.MATHUE_RA = "";
                                    if (dataReaderSelectVatBoHang["MATHUE_RA"] != null)
                                    {
                                        _EXTEND_VAT_BOHANG.MATHUE_RA = dataReaderSelectVatBoHang["MATHUE_RA"].ToString();
                                    }
                                    decimal GIATRI_THUE_RA = 0;
                                    if (dataReaderSelectVatBoHang["GIATRI_THUE_RA"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatBoHang["GIATRI_THUE_RA"].ToString(), out GIATRI_THUE_RA);
                                    }
                                    _EXTEND_VAT_BOHANG.GIATRI_THUE_RA = GIATRI_THUE_RA;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return _EXTEND_VAT_BOHANG;
        }
        public static EXTEND_VAT_BOHANG LAYDULIEU_VAT_BOHANG_FROM_DATABASE_SQLSERVER(string MaBoHang, string UnitCode)
        {
            EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();
            if (!string.IsNullOrEmpty(MaBoHang))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT
                                                                MAX(c.MATHUE_RA) AS MATHUE_RA,
                                                                MAX(d.GIATRI) AS GIATRI_THUE_RA
                                                            FROM
                                                                BOHANG a INNER JOIN BOHANG_CHITIET b ON a.MABOHANG = b.MABOHANG
                                                                INNER JOIN MATHANG c ON b.MAHANG = c.MAHANG
                                                                INNER JOIN THUE d on c.MATHUE_RA = d.MATHUE
                                                                WHERE a.MABOHANG = '" + MaBoHang + "' AND a.UNITCODE = '" + UnitCode + "' ");
                            SqlCommand commamdSelectVatBoHang = new SqlCommand();
                            commamdSelectVatBoHang.Connection = connection;
                            commamdSelectVatBoHang.CommandText = querrySelect;
                            SqlDataReader dataReaderSelectVatBoHang = commamdSelectVatBoHang.ExecuteReader();
                            if (dataReaderSelectVatBoHang.HasRows)
                            {
                                while (dataReaderSelectVatBoHang.Read())
                                {
                                    _EXTEND_VAT_BOHANG.MATHUE_RA = "";
                                    if (dataReaderSelectVatBoHang["MATHUE_RA"] != null)
                                    {
                                        _EXTEND_VAT_BOHANG.MATHUE_RA = dataReaderSelectVatBoHang["MATHUE_RA"].ToString();
                                    }
                                    decimal GIATRI_THUE_RA = 0;
                                    if (dataReaderSelectVatBoHang["GIATRI_THUE_RA"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatBoHang["GIATRI_THUE_RA"].ToString(), out GIATRI_THUE_RA);
                                    }
                                    _EXTEND_VAT_BOHANG.GIATRI_THUE_RA = GIATRI_THUE_RA;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return _EXTEND_VAT_BOHANG;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaHang"></param>
        /// <param name="UnitCode"></param>
        /// <returns></returns>
        public static EXTEND_VATTU_DTO LAYDULIEU_HANGHOA_FROM_DATABASE_ORACLE(string MaHang, string UnitCode)
        {
            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
            if (!string.IsNullOrEmpty(MaHang))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT A.TENHANG,D.TENDONVITINH AS DONVITINH,a.BARCODE,a.MATHUE_RA,C.GIATRI AS GIATRI_THUE_RA,B.TYLE_LAILE,b.GIABANLE_VAT FROM MATHANG a 
                            INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN THUE C ON A.MATHUE_RA = C.MATHUE 
                            INNER JOIN DONVITINH d ON a.MADONVITINH = D.MADONVITINH WHERE a.MAHANG = '" + MaHang + "' AND a.UNITCODE = '" + UnitCode + "'");
                            OracleCommand commamdSelectVatTu = new OracleCommand();
                            commamdSelectVatTu.Connection = connection;
                            commamdSelectVatTu.CommandText = querrySelect;
                            OracleDataReader dataReaderSelectVatTu = commamdSelectVatTu.ExecuteReader();
                            if (dataReaderSelectVatTu.HasRows)
                            {
                                while (dataReaderSelectVatTu.Read())
                                {
                                    if (dataReaderSelectVatTu["TENHANG"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.TENHANG = dataReaderSelectVatTu["TENHANG"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.TENHANG = "";
                                    }
                                    if (dataReaderSelectVatTu["DONVITINH"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.DONVITINH = dataReaderSelectVatTu["DONVITINH"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.DONVITINH = "";
                                    }
                                    if (dataReaderSelectVatTu["BARCODE"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.BARCODE = dataReaderSelectVatTu["BARCODE"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.BARCODE = "";
                                    }
                                    if (dataReaderSelectVatTu["MATHUE_RA"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.MATHUE_RA = dataReaderSelectVatTu["MATHUE_RA"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.MATHUE_RA = "";
                                    }
                                    decimal GIATRI_THUE_RA = 0;
                                    if (dataReaderSelectVatTu["GIATRI_THUE_RA"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["GIATRI_THUE_RA"].ToString(), out GIATRI_THUE_RA);
                                        _EXTEND_VATTU_DTO.GIATRI_THUE_RA = GIATRI_THUE_RA;
                                    }
                                    decimal TYLE_LAILE = 0;
                                    if (dataReaderSelectVatTu["TYLE_LAILE"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["TYLE_LAILE"].ToString(), out TYLE_LAILE);
                                        _EXTEND_VATTU_DTO.TYLE_LAILE = TYLE_LAILE;
                                    }
                                    decimal GIABANLE_VAT = 0;
                                    if (dataReaderSelectVatTu["GIABANLE_VAT"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                        _EXTEND_VATTU_DTO.GIABANLE_VAT = GIABANLE_VAT;
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
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return _EXTEND_VATTU_DTO;
        }
        public static List<EXTEND_BOHANGCHITIET_DTO> LAYDULIEU_BOHANGCHITIET_FROM_DATABASE_ORACLE(string MaBoHang, string UnitCode)
        {
            List<EXTEND_BOHANGCHITIET_DTO> _LST_EXTEND_BOHANGCHITIET_DTO = new List<EXTEND_BOHANGCHITIET_DTO>();
            if (!string.IsNullOrEmpty(MaBoHang))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand commandBoHang = new OracleCommand();
                            commandBoHang.Connection = connection;
                            commandBoHang.CommandText = string.Format(@"SELECT b.MAHANG,b.SOLUONG,b.TYLECKLE,b.TONGLE FROM DM_BOHANG a INNER JOIN DM_BOHANGCHITIET b ON a.MABOHANG = b.MABOHANG WHERE  a.MABOHANG = '" + MaBoHang + "' AND a.UNITCODE = '" + UnitCode + "'");
                            OracleDataReader dataReaderBoHang = commandBoHang.ExecuteReader();
                            if (dataReaderBoHang.HasRows)
                            {
                                while (dataReaderBoHang.Read())
                                {
                                    EXTEND_BOHANGCHITIET_DTO _EXTEND_BOHANGCHITIET_DTO = new EXTEND_BOHANGCHITIET_DTO();
                                    if (dataReaderBoHang["MAHANG"] != null)
                                    {
                                        _EXTEND_BOHANGCHITIET_DTO.MAHANG = dataReaderBoHang["MAHANG"].ToString();
                                    }

                                    decimal SOLUONG = 0;
                                    if (dataReaderBoHang["SOLUONG"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["SOLUONG"].ToString(), out SOLUONG);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.SOLUONG = SOLUONG;

                                    decimal TYLECKLE = 0;
                                    if (dataReaderBoHang["TYLECKLE"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["TYLECKLE"].ToString(), out TYLECKLE);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.TYLECKLE = TYLECKLE;

                                    decimal TONGLE = 0;
                                    if (dataReaderBoHang["TONGLE"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["TONGLE"].ToString(), out TONGLE);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.TONGLE = TONGLE;
                                    _LST_EXTEND_BOHANGCHITIET_DTO.Add(_EXTEND_BOHANGCHITIET_DTO);
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return _LST_EXTEND_BOHANGCHITIET_DTO;
        }


        public static int GET_THAMSO_SUDUNG_MANHINH_LCD_FROM_ORACLE()
        {
            int GIATRI_THAMSO = 0;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        string querrySelect = string.Format(@"SELECT GIATRI_SO FROM THAMSOHETHONG WHERE MA_THAMSO = 'SUDUNG_LCD' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "' ");
                        OracleCommand commamdThamSo = new OracleCommand();
                        commamdThamSo.Connection = connection;
                        commamdThamSo.CommandText = querrySelect;
                        OracleDataReader dataReaderThamSo = commamdThamSo.ExecuteReader();
                        if (dataReaderThamSo.HasRows)
                        {
                            while (dataReaderThamSo.Read())
                            {
                                if (dataReaderThamSo["GIATRI_SO"] != null)
                                {
                                    int.TryParse(dataReaderThamSo["GIATRI_SO"].ToString(), out GIATRI_THAMSO);
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
                    connection.Close();
                    connection.Dispose();
                }
            }
            return GIATRI_THAMSO;
        }

        public static int GET_THAMSO_KHOABANAM_FROM_ORACLE()
        {
            int GIATRI_THAMSO = 0;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        string querrySelect = string.Format(@"SELECT GIATRI_SO FROM THAMSOHETHONG WHERE MA_THAMSO = 'KHOA_BANAM' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "' ");
                        OracleCommand commamdThamSo = new OracleCommand();
                        commamdThamSo.Connection = connection;
                        commamdThamSo.CommandText = querrySelect;
                        OracleDataReader dataReaderThamSo = commamdThamSo.ExecuteReader();
                        if (dataReaderThamSo.HasRows)
                        {
                            while (dataReaderThamSo.Read())
                            {
                                if (dataReaderThamSo["GIATRI_SO"] != null)
                                {
                                    int.TryParse(dataReaderThamSo["GIATRI_SO"].ToString(), out GIATRI_THAMSO);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    GIATRI_THAMSO = 0;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return GIATRI_THAMSO;
        }

        public static int GET_THAMSO_KHOABANAM_FROM_SQLSERVER()
        {
            int GIATRI_THAMSO = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        string querrySelect = string.Format(@"SELECT GIATRI_SO FROM dbo.THAMSOHETHONG WHERE MA_THAMSO = 'KHOA_BANAM'");
                        SqlCommand commamdThamSo = new SqlCommand();
                        commamdThamSo.Connection = connection;
                        commamdThamSo.CommandText = querrySelect;
                        SqlDataReader dataReaderThamSo = commamdThamSo.ExecuteReader();
                        if (dataReaderThamSo.HasRows)
                        {
                            while (dataReaderThamSo.Read())
                            {
                                if (dataReaderThamSo["GIATRI_SO"] != null)
                                {
                                    int.TryParse(dataReaderThamSo["GIATRI_SO"].ToString(), out GIATRI_THAMSO);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    GIATRI_THAMSO = 0;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return GIATRI_THAMSO;
        }
    }
}
