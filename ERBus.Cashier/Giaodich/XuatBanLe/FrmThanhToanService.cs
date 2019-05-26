using System;
using ERBus.Cashier.Dto;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ERBus.Cashier.Common;
using Oracle.ManagedDataAccess.Client;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public class FrmThanhToanService
    {
        public static List<KHACHHANG_DTO> TIMKIEM_KHACHHANG_FROM_ORACLE(string P_KEYSEARCH, int P_USE_TIMKIEM_ALL, int P_DIEUKIEN_TIMKIEM, string UNITCODE)
        {
            List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = new List<KHACHHANG_DTO>();
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
                            cmd.CommandText = "TIMKIEM_KHACHHANG";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("P_KEYSEARCH", OracleDbType.Varchar2).Value = P_KEYSEARCH;
                            cmd.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2).Value = UNITCODE;
                            cmd.Parameters.Add("P_USE_TIMKIEM_ALL", OracleDbType.Int32).Value = P_USE_TIMKIEM_ALL;
                            cmd.Parameters.Add("P_DIEUKIEN_TIMKIEM", OracleDbType.Int32).Value = P_DIEUKIEN_TIMKIEM;
                            cmd.Parameters.Add("CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                            OracleDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    KHACHHANG_DTO _KHACHHANG_DTO = new KHACHHANG_DTO();
                                    _KHACHHANG_DTO.MAKHACHHANG = dataReader["MAKHACHHANG"] != null ? dataReader["MAKHACHHANG"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.TENKHACHHANG = dataReader["TENKHACHHANG"] != null ? dataReader["TENKHACHHANG"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.DIACHI = dataReader["DIACHI"] != null ? dataReader["DIACHI"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.DIENTHOAI = dataReader["DIENTHOAI"] != null ? dataReader["DIENTHOAI"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.CANCUOC_CONGDAN = dataReader["CANCUOC_CONGDAN"] != null ? dataReader["CANCUOC_CONGDAN"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.NGAYSINH = dataReader["NGAYSINH"] != DBNull.Value ? DateTime.Parse(dataReader["NGAYSINH"].ToString()) : (DateTime?)null;
                                    _KHACHHANG_DTO.NGAYDACBIET = dataReader["NGAYDACBIET"] != DBNull.Value ? DateTime.Parse(dataReader["NGAYDACBIET"].ToString()) : (DateTime?)null;
                                    _KHACHHANG_DTO.MATHE = dataReader["MATHE"] != null ? dataReader["MATHE"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.HANGKHACHHANG = dataReader["MAHANG"] != null ? dataReader["MAHANG"].ToString().Trim() : "";
                                    decimal SODIEM = 0;
                                    if (dataReader["SODIEM"] != null)
                                    {
                                        decimal.TryParse(dataReader["SODIEM"].ToString(), out SODIEM);
                                    }
                                    _KHACHHANG_DTO.SODIEM = SODIEM;
                                    decimal TONGTIEN = 0;
                                    if (dataReader["TONGTIEN"] != null)
                                    {
                                        decimal.TryParse(dataReader["TONGTIEN"].ToString(), out TONGTIEN);
                                    }
                                    _KHACHHANG_DTO.TONGTIEN = TONGTIEN;
                                    _KHACHHANG_DTO.DIENGIAI = dataReader["DIENGIAI"] != null ? dataReader["DIENGIAI"].ToString().Trim() : "";
                                    _LST_KHACHHANG_DTO.Add(_KHACHHANG_DTO);
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN KHÁCH HÀNG", 1, "0x1", "0x8", "normal");
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
            return _LST_KHACHHANG_DTO;
        }

        public static string LAY_MA_TEN_KHACHHANG_FROM_ORACLE(string MAKHACHHANG)
        {
            string RESULT = "";
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
                            cmd.CommandText = "SELECT MAKHACHHANG || ' - ' || TENKHACHHANG AS KHACHHANG FROM KHACHHANG WHERE MAKHACHHANG = '" + MAKHACHHANG + "'";
                            cmd.CommandType = CommandType.Text;
                            OracleDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["KHACHHANG"] != null)
                                    {
                                        RESULT = dataReader["KHACHHANG"].ToString();
                                    }
                                    else
                                    {
                                        RESULT = "";
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN KHÁCH HÀNG", 1, "0x1", "0x8", "normal");
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
            return RESULT;
        }

        public static string LAY_MA_TEN_KHACHHANG_FROM_SQLSERVER(string MAKHACHHANG)
        {
            string RESULT = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT CONCAT(MAKHACHHANG,' - ',TENKHACHHANG) AS KHACHHANG FROM dbo.KHACHHANG WHERE MAKHACHHANG = '" + MAKHACHHANG + "'";
                            cmd.CommandType = CommandType.Text;
                            SqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["KHACHHANG"] != null)
                                    {
                                        RESULT = dataReader["KHACHHANG"].ToString();
                                    }
                                    else
                                    {
                                        RESULT = "";
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN KHÁCH HÀNG", 1, "0x1", "0x8", "normal");
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
            return RESULT;
        }

        public static HANGKHACHHANG_DTO LAY_QUYDOI_TIEN_THANH_DIEM_HANGKHACHHANG_KHOIDAU_FROM_ORACLE()
        {
            HANGKHACHHANG_DTO RESULT_QUYDOITIEN_THANH_DIEM = new HANGKHACHHANG_DTO();
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
                            cmd.CommandText = string.Format(@"SELECT
                                                            MAHANG,
                                                            TENHANG,
                                                            SOTIEN_LENHANG,
                                                            TYLE_SINHNHAT,
                                                            TYLE_DACBIET,
                                                            QUYDOITIEN_THANH_DIEM,
                                                            QUYDOIDIEM_THANH_TIEN
                                                        FROM
                                                            HANGKHACHHANG
                                                        WHERE
                                                            TRANGTHAI = 10
                                                            AND HANG_KHOIDAU = 1
                                                            AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                            cmd.CommandType = CommandType.Text;
                            OracleDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    decimal SOTIEN_LENHANG = 0;
                                    decimal TYLE_SINHNHAT = 0;
                                    decimal TYLE_DACBIET = 0;
                                    decimal QUYDOITIEN_THANH_DIEM = 0;
                                    decimal QUYDOIDIEM_THANH_TIEN = 0;
                                    RESULT_QUYDOITIEN_THANH_DIEM.MAHANG = dataReader["MAHANG"] != null ? dataReader["MAHANG"].ToString() : "";
                                    RESULT_QUYDOITIEN_THANH_DIEM.TENHANG = dataReader["TENHANG"] != null ? dataReader["TENHANG"].ToString() : "";
                                    decimal.TryParse(dataReader["SOTIEN_LENHANG"] != null ? dataReader["SOTIEN_LENHANG"].ToString() : "", out SOTIEN_LENHANG);
                                    decimal.TryParse(dataReader["TYLE_SINHNHAT"] != null ? dataReader["TYLE_SINHNHAT"].ToString() : "", out TYLE_SINHNHAT);
                                    decimal.TryParse(dataReader["TYLE_DACBIET"] != null ? dataReader["TYLE_DACBIET"].ToString() : "", out TYLE_DACBIET);
                                    decimal.TryParse(dataReader["QUYDOITIEN_THANH_DIEM"] != null ? dataReader["QUYDOITIEN_THANH_DIEM"].ToString() : "", out QUYDOITIEN_THANH_DIEM);
                                    decimal.TryParse(dataReader["QUYDOIDIEM_THANH_TIEN"] != null ? dataReader["QUYDOIDIEM_THANH_TIEN"].ToString() : "", out QUYDOIDIEM_THANH_TIEN);
                                    RESULT_QUYDOITIEN_THANH_DIEM.SOTIEN_LENHANG = SOTIEN_LENHANG;
                                    RESULT_QUYDOITIEN_THANH_DIEM.TYLE_SINHNHAT = TYLE_SINHNHAT;
                                    RESULT_QUYDOITIEN_THANH_DIEM.TYLE_DACBIET = TYLE_DACBIET;
                                    RESULT_QUYDOITIEN_THANH_DIEM.QUYDOITIEN_THANH_DIEM = QUYDOITIEN_THANH_DIEM;
                                    RESULT_QUYDOITIEN_THANH_DIEM.QUYDOIDIEM_THANH_TIEN = QUYDOIDIEM_THANH_TIEN;
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN KHỞI ĐẦU HẠNG", 1, "0x1", "0x8", "normal");
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
            return RESULT_QUYDOITIEN_THANH_DIEM;
        }


        public static HANGKHACHHANG_DTO LAY_QUYDOI_TIEN_THANH_DIEM_HANGKHACHHANG_KHOIDAU_FROM_SQLSERVER()
        {
            HANGKHACHHANG_DTO RESULT_QUYDOITIEN_THANH_DIEM = new HANGKHACHHANG_DTO();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = string.Format(@"SELECT
                                                            MAHANG,
                                                            TENHANG,
                                                            SOTIEN_LENHANG,
                                                            TYLE_SINHNHAT,
                                                            TYLE_DACBIET,
                                                            QUYDOITIEN_THANH_DIEM,
                                                            QUYDOIDIEM_THANH_TIEN
                                                        FROM
                                                            dbo.HANGKHACHHANG
                                                        WHERE
                                                            TRANGTHAI = 10
                                                            AND HANG_KHOIDAU = 1
                                                            AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                            cmd.CommandType = CommandType.Text;
                            SqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    decimal SOTIEN_LENHANG = 0;
                                    decimal TYLE_SINHNHAT = 0;
                                    decimal TYLE_DACBIET = 0;
                                    decimal QUYDOITIEN_THANH_DIEM = 0;
                                    decimal QUYDOIDIEM_THANH_TIEN = 0;
                                    RESULT_QUYDOITIEN_THANH_DIEM.MAHANG = dataReader["MAHANG"] != null ? dataReader["MAHANG"].ToString() : "";
                                    RESULT_QUYDOITIEN_THANH_DIEM.TENHANG = dataReader["TENHANG"] != null ? dataReader["TENHANG"].ToString() : "";
                                    decimal.TryParse(dataReader["SOTIEN_LENHANG"] != null ? dataReader["SOTIEN_LENHANG"].ToString() : "", out SOTIEN_LENHANG);
                                    decimal.TryParse(dataReader["TYLE_SINHNHAT"] != null ? dataReader["TYLE_SINHNHAT"].ToString() : "", out TYLE_SINHNHAT);
                                    decimal.TryParse(dataReader["TYLE_DACBIET"] != null ? dataReader["TYLE_DACBIET"].ToString() : "", out TYLE_DACBIET);
                                    decimal.TryParse(dataReader["QUYDOITIEN_THANH_DIEM"] != null ? dataReader["QUYDOITIEN_THANH_DIEM"].ToString() : "", out QUYDOITIEN_THANH_DIEM);
                                    decimal.TryParse(dataReader["QUYDOIDIEM_THANH_TIEN"] != null ? dataReader["QUYDOIDIEM_THANH_TIEN"].ToString() : "", out QUYDOIDIEM_THANH_TIEN);
                                    RESULT_QUYDOITIEN_THANH_DIEM.SOTIEN_LENHANG = SOTIEN_LENHANG;
                                    RESULT_QUYDOITIEN_THANH_DIEM.TYLE_SINHNHAT = TYLE_SINHNHAT;
                                    RESULT_QUYDOITIEN_THANH_DIEM.TYLE_DACBIET = TYLE_DACBIET;
                                    RESULT_QUYDOITIEN_THANH_DIEM.QUYDOITIEN_THANH_DIEM = QUYDOITIEN_THANH_DIEM;
                                    RESULT_QUYDOITIEN_THANH_DIEM.QUYDOIDIEM_THANH_TIEN = QUYDOIDIEM_THANH_TIEN;
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN KHỞI ĐẦU HẠNG", 1, "0x1", "0x8", "normal");
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
            return RESULT_QUYDOITIEN_THANH_DIEM;
        }

        public static HANGKHACHHANG_DTO LAY_QUYDOI_THEOHANGKH_FROM_ORACLE(string MAHANGKHACHHANG)
        {
            HANGKHACHHANG_DTO RESULT = new HANGKHACHHANG_DTO();
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
                            cmd.CommandText = string.Format(@"SELECT
                                                            MAHANG,
                                                            TENHANG,
                                                            SOTIEN_LENHANG,
                                                            TYLE_SINHNHAT,
                                                            TYLE_DACBIET,
                                                            QUYDOITIEN_THANH_DIEM,
                                                            QUYDOIDIEM_THANH_TIEN
                                                        FROM
                                                            HANGKHACHHANG WHERE TRANGTHAI = 10 AND MAHANG = '" + MAHANGKHACHHANG + "' AND UNITCODE = '" +Session.Session.CurrentUnitCode+ "'");
                            cmd.CommandType = CommandType.Text;
                            OracleDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    decimal SOTIEN_LENHANG = 0;
                                    decimal TYLE_SINHNHAT = 0;
                                    decimal TYLE_DACBIET = 0;
                                    decimal QUYDOITIEN_THANH_DIEM = 0;
                                    decimal QUYDOIDIEM_THANH_TIEN = 0;
                                    RESULT.MAHANG = dataReader["MAHANG"] != null ? dataReader["MAHANG"].ToString() : "";
                                    RESULT.TENHANG = dataReader["TENHANG"] != null ? dataReader["TENHANG"].ToString() : "";
                                    decimal.TryParse(dataReader["SOTIEN_LENHANG"] != null ? dataReader["SOTIEN_LENHANG"].ToString() : "",  out SOTIEN_LENHANG);
                                    decimal.TryParse(dataReader["TYLE_SINHNHAT"] != null ? dataReader["TYLE_SINHNHAT"].ToString() : "", out TYLE_SINHNHAT);
                                    decimal.TryParse(dataReader["TYLE_DACBIET"] != null ? dataReader["TYLE_DACBIET"].ToString() : "", out TYLE_DACBIET);
                                    decimal.TryParse(dataReader["QUYDOITIEN_THANH_DIEM"] != null ? dataReader["QUYDOITIEN_THANH_DIEM"].ToString() : "", out QUYDOITIEN_THANH_DIEM);
                                    decimal.TryParse(dataReader["QUYDOIDIEM_THANH_TIEN"] != null ? dataReader["QUYDOIDIEM_THANH_TIEN"].ToString() : "", out QUYDOIDIEM_THANH_TIEN);
                                    RESULT.SOTIEN_LENHANG = SOTIEN_LENHANG;
                                    RESULT.TYLE_SINHNHAT = TYLE_SINHNHAT;
                                    RESULT.TYLE_DACBIET = TYLE_DACBIET;
                                    RESULT.QUYDOITIEN_THANH_DIEM = QUYDOITIEN_THANH_DIEM;
                                    RESULT.QUYDOIDIEM_THANH_TIEN = QUYDOIDIEM_THANH_TIEN;
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN HẠNG KHÁCH HÀNG", 1, "0x1", "0x8", "normal");
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
            return RESULT;
        }


        public static HANGKHACHHANG_DTO LAY_QUYDOI_THEOHANGKH_FROM_SQLSERVER(string MAHANGKHACHHANG)
        {
            HANGKHACHHANG_DTO RESULT = new HANGKHACHHANG_DTO();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = string.Format(@"SELECT
                                                            MAHANG,
                                                            TENHANG,
                                                            SOTIEN_LENHANG,
                                                            TYLE_SINHNHAT,
                                                            TYLE_DACBIET,
                                                            QUYDOITIEN_THANH_DIEM,
                                                            QUYDOIDIEM_THANH_TIEN
                                                        FROM
                                                            dbo.HANGKHACHHANG WHERE TRANGTHAI = 10 AND MAHANG = '" + MAHANGKHACHHANG + "' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                            cmd.CommandType = CommandType.Text;
                            SqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    decimal SOTIEN_LENHANG = 0;
                                    decimal TYLE_SINHNHAT = 0;
                                    decimal TYLE_DACBIET = 0;
                                    decimal QUYDOITIEN_THANH_DIEM = 0;
                                    decimal QUYDOIDIEM_THANH_TIEN = 0;
                                    RESULT.MAHANG = dataReader["MAHANG"] != null ? dataReader["MAHANG"].ToString() : "";
                                    RESULT.TENHANG = dataReader["TENHANG"] != null ? dataReader["TENHANG"].ToString() : "";
                                    decimal.TryParse(dataReader["SOTIEN_LENHANG"] != null ? dataReader["SOTIEN_LENHANG"].ToString() : "", out SOTIEN_LENHANG);
                                    decimal.TryParse(dataReader["TYLE_SINHNHAT"] != null ? dataReader["TYLE_SINHNHAT"].ToString() : "", out TYLE_SINHNHAT);
                                    decimal.TryParse(dataReader["TYLE_DACBIET"] != null ? dataReader["TYLE_DACBIET"].ToString() : "", out TYLE_DACBIET);
                                    decimal.TryParse(dataReader["QUYDOITIEN_THANH_DIEM"] != null ? dataReader["QUYDOITIEN_THANH_DIEM"].ToString() : "", out QUYDOITIEN_THANH_DIEM);
                                    decimal.TryParse(dataReader["QUYDOIDIEM_THANH_TIEN"] != null ? dataReader["QUYDOIDIEM_THANH_TIEN"].ToString() : "", out QUYDOIDIEM_THANH_TIEN);
                                    RESULT.SOTIEN_LENHANG = SOTIEN_LENHANG;
                                    RESULT.TYLE_SINHNHAT = TYLE_SINHNHAT;
                                    RESULT.TYLE_DACBIET = TYLE_DACBIET;
                                    RESULT.QUYDOITIEN_THANH_DIEM = QUYDOITIEN_THANH_DIEM;
                                    RESULT.QUYDOIDIEM_THANH_TIEN = QUYDOIDIEM_THANH_TIEN;
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN HẠNG KHÁCH HÀNG", 1, "0x1", "0x8", "normal");
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
            return RESULT;
        }
    }
}
