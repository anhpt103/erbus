using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ERBus.Cashier.Giaodich.XuatBanLe;
using Oracle.ManagedDataAccess.Client;
using ERBus.Cashier.Dto;

namespace ERBus.Cashier.Common
{
    public static class SYNCHRONIZE_DATA
    {
        public static string Get_TableName_XNT(int Nam, int Ky)
        {
            return string.Format("XNT_{0}_KY_{1}", Nam, Ky);
        }

        public static int GetNullableInt(this OracleDataReader reader, string name)
        {
            var colIndex = reader.GetOrdinal(name);
            return !reader.IsDBNull(colIndex) ? reader.GetInt32(colIndex) : default(int);
        }

        public static decimal GetNullableDecimal (this OracleDataReader reader, string name)
        {
            var colIndex = reader.GetOrdinal(name);
            return !reader.IsDBNull(colIndex) ? reader.GetDecimal(colIndex) : default(decimal);
        }

        public static DateTime? GetNullableDateTime(this OracleDataReader reader, string name)
        {
            var col = reader.GetOrdinal(name);
            return reader.IsDBNull(col) ?
                        (DateTime?)null :
                        (DateTime?)reader.GetDateTime(col);
        }

        public static void KHOASODULIEU()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmdOrcl = new OracleCommand();
                        cmdOrcl.Connection = connectionOrcl;
                        cmdOrcl.Parameters.Clear();
                        cmdOrcl.CommandType = CommandType.Text;
                        cmdOrcl.CommandText = string.Format(@"SELECT KYKETOAN AS KYCHUAKHOA,NAM,TUNGAY AS NGAYKETOAN FROM KYKETOAN WHERE
                        TO_DATE(DENNGAY,'DD-MM-YY') <= TO_DATE(SYSDATE,'DD-MM-YY') AND TO_DATE(TUNGAY,'DD-MM-YY') > TO_DATE((SELECT MAX(TUNGAY) AS NGAYKETOAN 
                        FROM KYKETOAN WHERE TRANGTHAI = 10 AND NAM = (SELECT MAX(NAM) FROM KYKETOAN) AND UNITCODE = '" + Session.Session.CurrentUnitCode + "' GROUP BY NAM),'DD-MM-YY') ORDER BY KYKETOAN");
                        OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                        if (dataReaderOrcl.HasRows)
                        {
                            while (dataReaderOrcl.Read())
                            {

                                KYKETOAN_DTO row = new KYKETOAN_DTO();
                                int KYCHUAKHOA = 0;
                                int.TryParse(dataReaderOrcl["KYCHUAKHOA"].ToString(), out KYCHUAKHOA);
                                row.KY = KYCHUAKHOA;
                                int NAM = 0;
                                int.TryParse(dataReaderOrcl["NAM"].ToString(), out NAM);
                                row.NAM = NAM;
                                string CurrentTableName = Get_TableName_XNT(row.NAM, row.KY);
                                string PreviousTableName = null;
                                cmdOrcl.Parameters.Clear();
                                cmdOrcl.CommandType = CommandType.Text;
                                cmdOrcl.CommandText = string.Format(@"SELECT KYKETOAN,NAM FROM KYKETOAN WHERE NAM = " + row.NAM + " AND KYKETOAN = " + (row.KY - 1) + " AND ROWNUM = 1");
                                OracleDataReader dataReaderOrclKyKeToan = null;
                                dataReaderOrclKyKeToan = cmdOrcl.ExecuteReader();
                                if (dataReaderOrclKyKeToan.HasRows)
                                {
                                    while (dataReaderOrclKyKeToan.Read())
                                    {
                                        int NAM_TRUOC = 0;
                                        int.TryParse(dataReaderOrclKyKeToan["NAM"].ToString(), out NAM_TRUOC);
                                        int KY_TRUOC = 0;
                                        int.TryParse(dataReaderOrclKyKeToan["KYKETOAN"].ToString(), out KY_TRUOC);
                                        PreviousTableName = Get_TableName_XNT(NAM_TRUOC, KY_TRUOC);
                                    }
                                    dataReaderOrclKyKeToan.Dispose();
                                }
                                cmdOrcl.CommandType = CommandType.StoredProcedure;
                                cmdOrcl.CommandText = "ERBUS.XUATNHAPTON.XNT_KHOASO";
                                cmdOrcl.Parameters.Clear();
                                cmdOrcl.Parameters.Add("P_TABLENAME_KYTRUOC", OracleDbType.Varchar2, 15, PreviousTableName, ParameterDirection.Input);
                                cmdOrcl.Parameters.Add("P_TABLENAME", OracleDbType.Varchar2, 15, CurrentTableName, ParameterDirection.Input);
                                cmdOrcl.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2, 10, Session.Session.CurrentUnitCode, ParameterDirection.Input);
                                cmdOrcl.Parameters.Add("P_NAM", OracleDbType.Int32, row.NAM, ParameterDirection.Input);
                                cmdOrcl.Parameters.Add("P_KY", OracleDbType.Int32, row.KY, ParameterDirection.Input);
                                cmdOrcl.ExecuteNonQuery();
                                cmdOrcl.Parameters.Clear();
                                cmdOrcl.CommandType = CommandType.Text;
                                cmdOrcl.CommandText = string.Format(@"UPDATE KYKETOAN SET TRANGTHAI = 10 WHERE NAM = " + row.NAM + " AND KYKETOAN = " + row.KY + " AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                                cmdOrcl.ExecuteNonQuery();
                            }
                            dataReaderOrcl.Dispose();
                        }
                        //Mở thì đóng
                        connectionOrcl.Close();
                        connectionOrcl.Dispose();
                    }
                    catch (Exception ex)
                    {
                        connectionOrcl.Close();
                        connectionOrcl.Dispose();
                        WriteLogs.LogError(ex);
                    }
                }
            }
        }


        public static void SYNCHRONIZE_KHOASO()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    int countExists = 0;
                    string QUERY_CREATE_TABLE = string.Empty;
                    string TABLE_NAME = FrmXuatBanLeService.GET_TABLE_NAME_NGAYHACHTOAN_CSDL_ORACLE();
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT UNITCODE,NAM,KY,MAKHO,MAHANG,GIAVON,TONDAUKYSL,TONDAUKYGT,NHAPSL,NHAPGT,XUATSL,XUATGT,TONCUOIKYSL,TONCUOIKYGT FROM " + TABLE_NAME + " WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    SqlCommand cmdCommonSql = new SqlCommand();
                                    cmdCommonSql.Connection = connectionSa;
                                    cmdCommonSql.CommandText = string.Format(@"SELECT COUNT(*) AS TABLE_EXISTS FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'" + TABLE_NAME + "'");
                                    SqlDataReader dataReaderSql = cmdCommonSql.ExecuteReader();
                                    if (dataReaderSql.HasRows)
                                    {
                                        while (dataReaderSql.Read())
                                        {
                                            int.TryParse(dataReaderSql["TABLE_EXISTS"].ToString(), out countExists);
                                        }
                                        dataReaderSql.Close();
                                    }

                                    if(countExists == 0)
                                    {
                                        
                                        QUERY_CREATE_TABLE = string.Format(@"CREATE TABLE dbo."+ TABLE_NAME + " ");
                                        QUERY_CREATE_TABLE += string.Format(@"( UNITCODE VARCHAR(10), 
	                                                                            NAM INT, 
	                                                                            KY INT, 
	                                                                            MAKHO VARCHAR(50), 
	                                                                            MAHANG VARCHAR(50), 
	                                                                            GIAVON DECIMAL DEFAULT 0, 
	                                                                            TONDAUKYSL DECIMAL DEFAULT 0, 
	                                                                            TONDAUKYGT DECIMAL DEFAULT 0, 
	                                                                            NHAPSL DECIMAL DEFAULT 0, 
	                                                                            NHAPGT DECIMAL DEFAULT 0, 
	                                                                            XUATSL DECIMAL DEFAULT 0, 
	                                                                            XUATGT DECIMAL DEFAULT 0, 
	                                                                            TONCUOIKYSL DECIMAL DEFAULT 0, 
	                                                                            TONCUOIKYGT DECIMAL DEFAULT 0
                                                                               )
                                                                            ");
                                    }
                                    else
                                    {
                                        QUERY_CREATE_TABLE = string.Format(@"DELETE dbo." + TABLE_NAME + " WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                                    }

                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.Parameters.Clear();
                                            cmdCommonSql.CommandText = QUERY_CREATE_TABLE;
                                            if (!string.IsNullOrEmpty(QUERY_CREATE_TABLE))
                                            {
                                                cmdCommonSql.ExecuteNonQuery();
                                            }
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo."+ TABLE_NAME + "(UNITCODE,NAM,KY,MAKHO,MAHANG,GIAVON,TONDAUKYSL,TONDAUKYGT,NHAPSL,NHAPGT,XUATSL,XUATGT,TONCUOIKYSL,TONCUOIKYGT) VALUES (@UNITCODE,@NAM,@KY,@MAKHO,@MAHANG,@GIAVON,@TONDAUKYSL,@TONDAUKYGT,@NHAPSL,@NHAPGT,@XUATSL,@XUATGT,@TONCUOIKYSL,@TONCUOIKYGT)");
                                                cmdCommonSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("NAM", SqlDbType.Int).Value = GetNullableInt(dataReaderOrcl, "NAM");
                                                cmdCommonSql.Parameters.Add("KY", SqlDbType.Int).Value = GetNullableInt(dataReaderOrcl, "KY");
                                                cmdCommonSql.Parameters.Add("MAKHO", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAKHO"] != null ? dataReaderOrcl["MAKHO"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MAHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAHANG"] != null ? dataReaderOrcl["MAHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIAVON", SqlDbType.Decimal).Value = GetNullableDecimal(dataReaderOrcl, "GIAVON");
                                                cmdCommonSql.Parameters.Add("TONDAUKYSL", SqlDbType.Decimal).Value = GetNullableDecimal(dataReaderOrcl, "TONDAUKYSL");
                                                cmdCommonSql.Parameters.Add("TONDAUKYGT", SqlDbType.Decimal).Value = GetNullableDecimal(dataReaderOrcl, "TONDAUKYGT");
                                                cmdCommonSql.Parameters.Add("NHAPSL", SqlDbType.Decimal).Value = GetNullableDecimal(dataReaderOrcl, "NHAPSL");
                                                cmdCommonSql.Parameters.Add("NHAPGT", SqlDbType.Decimal).Value = GetNullableDecimal(dataReaderOrcl, "NHAPGT");
                                                cmdCommonSql.Parameters.Add("XUATSL", SqlDbType.Decimal).Value = GetNullableDecimal(dataReaderOrcl, "XUATSL");
                                                cmdCommonSql.Parameters.Add("XUATGT", SqlDbType.Decimal).Value = GetNullableDecimal(dataReaderOrcl, "XUATGT");
                                                cmdCommonSql.Parameters.Add("TONCUOIKYSL", SqlDbType.Decimal).Value = GetNullableDecimal(dataReaderOrcl, "TONCUOIKYSL");
                                                cmdCommonSql.Parameters.Add("TONCUOIKYGT", SqlDbType.Decimal).Value = GetNullableDecimal(dataReaderOrcl, "TONCUOIKYGT");
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }

        public static void SYNCHRONIZE_KYKETOAN()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,KYKETOAN,TENKY,TUNGAY,DENNGAY,NAM,TRANGTHAI,UNITCODE FROM KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"DELETE KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.KYKETOAN(ID,KYKETOAN,TENKY,TUNGAY,DENNGAY,NAM,TRANGTHAI,UNITCODE) VALUES (@ID,@KYKETOAN,@TENKY,@TUNGAY,@DENNGAY,@NAM,@TRANGTHAI,@UNITCODE)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = dataReaderOrcl["ID"] != null ? dataReaderOrcl["ID"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("KYKETOAN", SqlDbType.Int).Value = GetNullableInt(dataReaderOrcl, "KYKETOAN");
                                                cmdCommonSql.Parameters.Add("TENKY", SqlDbType.NVarChar, 150).Value = dataReaderOrcl["TENKY"] != null ? dataReaderOrcl["TENKY"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TUNGAY", SqlDbType.Date).Value = GetNullableDateTime(dataReaderOrcl, "TUNGAY");
                                                cmdCommonSql.Parameters.Add("DENNGAY", SqlDbType.Date).Value = GetNullableDateTime(dataReaderOrcl, "DENNGAY");
                                                cmdCommonSql.Parameters.Add("NAM", SqlDbType.Int).Value = GetNullableInt(dataReaderOrcl, "NAM");
                                                cmdCommonSql.Parameters.Add("TRANGTHAI", SqlDbType.Int).Value = GetNullableInt(dataReaderOrcl, "TRANGTHAI");
                                                cmdCommonSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }

        public static void SYNCHRONIZE_NHACUNGCAP()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,MANHACUNGCAP,TENNHACUNGCAP,DIACHI,MASOTHUE,DIENTHOAI,DIENGIAI,TRANGTHAI,UNITCODE FROM NHACUNGCAP WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"DELETE NHACUNGCAP");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.NHACUNGCAP(ID,MANHACUNGCAP,TENNHACUNGCAP,DIACHI,MASOTHUE,DIENTHOAI,DIENGIAI,TRANGTHAI,UNITCODE) VALUES (@ID,@MANHACUNGCAP,@TENNHACUNGCAP,@DIACHI,@MASOTHUE,@DIENTHOAI,@DIENGIAI,@TRANGTHAI,@UNITCODE)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = dataReaderOrcl["ID"] != null ? dataReaderOrcl["ID"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MANHACUNGCAP", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MANHACUNGCAP"] != null ? dataReaderOrcl["MANHACUNGCAP"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TENNHACUNGCAP", SqlDbType.NVarChar, 300).Value = dataReaderOrcl["TENNHACUNGCAP"] != null ? dataReaderOrcl["TENNHACUNGCAP"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("DIACHI", SqlDbType.NVarChar, 300).Value = dataReaderOrcl["DIACHI"] != null ? dataReaderOrcl["DIACHI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MASOTHUE ", SqlDbType.VarChar, 80).Value = dataReaderOrcl["MASOTHUE"] != null ? dataReaderOrcl["MASOTHUE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("DIENTHOAI", SqlDbType.VarChar, 20).Value = dataReaderOrcl["DIENTHOAI"] != null ? dataReaderOrcl["DIENTHOAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("DIENGIAI", SqlDbType.NVarChar, 300).Value = dataReaderOrcl["DIENGIAI"] != null ? dataReaderOrcl["DIENGIAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TRANGTHAI", SqlDbType.Int).Value = GetNullableInt(dataReaderOrcl, "TRANGTHAI");
                                                cmdCommonSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }


        public static void SYNCHRONIZE_NGUOIDUNG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,USERNAME,PASSWORD,MANHANVIEN,TENNHANVIEN,SODIENTHOAI,CHUNGMINHTHU,GIOITINH,UNITCODE FROM NGUOIDUNG WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"DELETE NGUOIDUNG");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.NGUOIDUNG(ID,USERNAME,PASSWORD,MANHANVIEN,TENNHANVIEN,SODIENTHOAI,CHUNGMINHTHU,GIOITINH,UNITCODE) VALUES (@ID,@USERNAME,@PASSWORD,@MANHANVIEN,@TENNHANVIEN,@SODIENTHOAI,@CHUNGMINHTHU,@GIOITINH,@UNITCODE)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("USERNAME", SqlDbType.VarChar, 50).Value = dataReaderOrcl["USERNAME"] != null ? dataReaderOrcl["USERNAME"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("PASSWORD", SqlDbType.VarChar, 50).Value = dataReaderOrcl["PASSWORD"] != null ? dataReaderOrcl["PASSWORD"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MANHANVIEN", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MANHANVIEN"] != null ? dataReaderOrcl["MANHANVIEN"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TENNHANVIEN ", SqlDbType.NVarChar, 200).Value = dataReaderOrcl["TENNHANVIEN"] != null ? dataReaderOrcl["TENNHANVIEN"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("SODIENTHOAI", SqlDbType.VarChar, 20).Value = dataReaderOrcl["SODIENTHOAI"] != null ? dataReaderOrcl["SODIENTHOAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("CHUNGMINHTHU", SqlDbType.VarChar, 20).Value = dataReaderOrcl["CHUNGMINHTHU"] != null ? dataReaderOrcl["CHUNGMINHTHU"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIOITINH", SqlDbType.Int).Value = dataReaderOrcl["GIOITINH"] != DBNull.Value ? int.Parse(dataReaderOrcl["GIOITINH"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }
        public static void SYNCHRONIZE_KHACHHANG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,TONGTIEN,DIENGIAI,MAHANG,UNITCODE FROM KHACHHANG WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"DELETE KHACHHANG");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.KHACHHANG(ID,MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,TONGTIEN,DIENGIAI,MAHANG,UNITCODE) VALUES (@ID,@MAKHACHHANG,@TENKHACHHANG,@DIACHI,@DIENTHOAI,@CANCUOC_CONGDAN,@NGAYSINH,@NGAYDACBIET,@MATHE,@SODIEM,@TONGTIEN,@DIENGIAI,@MAHANG,@UNITCODE)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MAKHACHHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAKHACHHANG"] != null ? dataReaderOrcl["MAKHACHHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TENKHACHHANG", SqlDbType.NVarChar, 300).Value = dataReaderOrcl["TENKHACHHANG"] != null ? dataReaderOrcl["TENKHACHHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("DIACHI", SqlDbType.NVarChar, 300).Value = dataReaderOrcl["DIACHI"] != null ? dataReaderOrcl["DIACHI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("DIENTHOAI", SqlDbType.VarChar, 20).Value = dataReaderOrcl["DIENTHOAI"] != null ? dataReaderOrcl["DIENTHOAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("CANCUOC_CONGDAN", SqlDbType.VarChar, 20).Value = dataReaderOrcl["CANCUOC_CONGDAN"] != null ? dataReaderOrcl["CANCUOC_CONGDAN"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("NGAYSINH", SqlDbType.DateTime).Value = dataReaderOrcl["NGAYSINH"] != DBNull.Value ? DateTime.Parse(dataReaderOrcl["NGAYSINH"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("NGAYDACBIET", SqlDbType.DateTime).Value = dataReaderOrcl["NGAYDACBIET"] != DBNull.Value ? DateTime.Parse(dataReaderOrcl["NGAYDACBIET"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MATHE", SqlDbType.VarChar, 20).Value = dataReaderOrcl["MATHE"].ToString();
                                                cmdCommonSql.Parameters.Add("SODIEM", SqlDbType.Decimal).Value = dataReaderOrcl["SODIEM"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["SODIEM"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TONGTIEN", SqlDbType.Decimal).Value = dataReaderOrcl["TONGTIEN"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["TONGTIEN"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("DIENGIAI", SqlDbType.NVarChar, 300).Value = dataReaderOrcl["DIENGIAI"].ToString();
                                                cmdCommonSql.Parameters.Add("MAHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAHANG"].ToString();
                                                cmdCommonSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }
        public static void SYNCHRONIZE_KHUYENMAI()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT MA_KHUYENMAI,LOAI_KHUYENMAI,TUNGAY,DENNGAY,TUGIO,DENGIO,MAKHO_KHUYENMAI,DIENGIAI,UNITCODE FROM KHUYENMAI WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"DELETE KHUYENMAI");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.KHUYENMAI(ID,MA_KHUYENMAI,LOAI_KHUYENMAI,TUNGAY,DENNGAY,TUGIO,DENGIO,MAKHO_KHUYENMAI,DIENGIAI,UNITCODE) VALUES (@ID,@MA_KHUYENMAI,@LOAI_KHUYENMAI,@TUNGAY,@DENNGAY,@TUGIO,@DENGIO,@MAKHO_KHUYENMAI,@DIENGIAI,@UNITCODE)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MA_KHUYENMAI", SqlDbType.VarChar, 30).Value = dataReaderOrcl["MA_KHUYENMAI"] != null ? dataReaderOrcl["MA_KHUYENMAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("LOAI_KHUYENMAI", SqlDbType.VarChar, 20).Value = dataReaderOrcl["LOAI_KHUYENMAI"] != null ? dataReaderOrcl["LOAI_KHUYENMAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TUNGAY", SqlDbType.Date).Value = dataReaderOrcl["TUNGAY"] != DBNull.Value ? DateTime.Parse(dataReaderOrcl["TUNGAY"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("DENNGAY", SqlDbType.Date).Value = dataReaderOrcl["DENNGAY"] != DBNull.Value ? DateTime.Parse(dataReaderOrcl["DENNGAY"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TUGIO", SqlDbType.VarChar, 10).Value = dataReaderOrcl["TUGIO"] != null ? dataReaderOrcl["TUGIO"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("DENGIO", SqlDbType.VarChar, 10).Value = dataReaderOrcl["DENGIO"] != null ? dataReaderOrcl["DENGIO"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MAKHO_KHUYENMAI", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAKHO_KHUYENMAI"] != null ? dataReaderOrcl["MAKHO_KHUYENMAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("DIENGIAI", SqlDbType.NVarChar, 500).Value = dataReaderOrcl["DIENGIAI"] != null ? dataReaderOrcl["DIENGIAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }
        public static void SYNCHRONIZE_KHUYENMAI_CHITIET()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT MA_KHUYENMAI,MAHANG,SOLUONG,GIATRI_KHUYENMAI FROM KHUYENMAI_CHITIET");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"DELETE KHUYENMAI_CHITIET");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.KHUYENMAI_CHITIET(ID,MA_KHUYENMAI,MAHANG,SOLUONG,GIATRI_KHUYENMAI) VALUES (@ID,@MA_KHUYENMAI,@MAHANG,@SOLUONG,@GIATRI_KHUYENMAI)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MA_KHUYENMAI", SqlDbType.VarChar, 30).Value = dataReaderOrcl["MA_KHUYENMAI"] != null ? dataReaderOrcl["MA_KHUYENMAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MAHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAHANG"] != DBNull.Value ? dataReaderOrcl["MAHANG"].ToString() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("SOLUONG", SqlDbType.Decimal).Value = dataReaderOrcl["SOLUONG"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["SOLUONG"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIATRI_KHUYENMAI", SqlDbType.Decimal).Value = dataReaderOrcl["GIATRI_KHUYENMAI"] != null ? decimal.Parse(dataReaderOrcl["GIATRI_KHUYENMAI"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }
        public static void SYNCHRONIZE_THAMSOHETHONG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,MA_THAMSO,TEN_THAMSO,GIATRI_SO,GIATRI_CHU FROM THAMSOHETHONG WHERE GIATRI_SO = 10 AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"DELETE THAMSOHETHONG");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.THAMSOHETHONG(ID,MA_THAMSO,TEN_THAMSO,GIATRI_SO,GIATRI_CHU) VALUES (@ID,@MA_THAMSO,@TEN_THAMSO,@GIATRI_SO,@GIATRI_CHU)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MA_THAMSO", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MA_THAMSO"] != null ? dataReaderOrcl["MA_THAMSO"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TEN_THAMSO", SqlDbType.NVarChar, 200).Value = dataReaderOrcl["TEN_THAMSO"] != null ? dataReaderOrcl["TEN_THAMSO"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIATRI_SO", SqlDbType.Int).Value = dataReaderOrcl["GIATRI_SO"] != DBNull.Value ? int.Parse(dataReaderOrcl["GIATRI_SO"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIATRI_CHU", SqlDbType.VarChar, 50).Value = dataReaderOrcl["GIATRI_CHU"] != DBNull.Value ? dataReaderOrcl["GIATRI_CHU"].ToString() : (object)DBNull.Value;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }

        public static void SYNCHRONIZE_LOAIHANG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT MALOAI,TENLOAI,UNITCODE FROM LOAIHANG WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"TRUNCATE TABLE dbo.LOAIHANG");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.LOAIHANG(ID,MALOAI,TENLOAI,UNITCODE) VALUES (@ID,@MALOAI,@TENLOAI,@UNITCODE)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MALOAI", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MALOAI"] != null ? dataReaderOrcl["MALOAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TENLOAI", SqlDbType.NVarChar, 300).Value = dataReaderOrcl["TENLOAI"] != null ? dataReaderOrcl["TENLOAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }


        public static void SYNCHRONIZE_NHOMHANG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT MALOAI,MANHOM,TENNHOM,UNITCODE FROM NHOMHANG WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"TRUNCATE TABLE dbo.NHOMHANG");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.NHOMHANG(ID,MALOAI,MANHOM,TENNHOM,UNITCODE) VALUES (@ID,@MALOAI,@MANHOM,@TENNHOM,@UNITCODE)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MALOAI", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MALOAI"] != null ? dataReaderOrcl["MALOAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MANHOM", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MANHOM"] != null ? dataReaderOrcl["MANHOM"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TENNHOM", SqlDbType.NVarChar, 300).Value = dataReaderOrcl["TENNHOM"] != null ? dataReaderOrcl["TENNHOM"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }


        public static void SYNCHRONIZE_MATHANG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT MAHANG,TENHANG,MANHACUNGCAP,MALOAI,MANHOM,MATHUE_VAO,MATHUE_RA,MADONVITINH,MAKEHANG,MABAOBI,BARCODE,ITEMCODE,UNITCODE FROM MATHANG WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"TRUNCATE TABLE dbo.MATHANG");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.MATHANG(ID,MAHANG,TENHANG,MANHACUNGCAP,MALOAI,MANHOM,MATHUE_VAO,MATHUE_RA,MADONVITINH,MAKEHANG,MABAOBI,BARCODE,ITEMCODE,UNITCODE) VALUES (@ID,@MAHANG,@TENHANG,@MANHACUNGCAP,@MALOAI,@MANHOM,@MATHUE_VAO,@MATHUE_RA,@MADONVITINH,@MAKEHANG,@MABAOBI,@BARCODE,@ITEMCODE,@UNITCODE)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MAHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAHANG"] != null ? dataReaderOrcl["MAHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TENHANG", SqlDbType.NVarChar, 300).Value = dataReaderOrcl["TENHANG"] != null ? dataReaderOrcl["TENHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MANHACUNGCAP", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MANHACUNGCAP"] != null ? dataReaderOrcl["MANHACUNGCAP"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MALOAI", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MALOAI"] != null ? dataReaderOrcl["MALOAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MANHOM", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MANHOM"] != null ? dataReaderOrcl["MANHOM"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MATHUE_VAO", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MATHUE_VAO"] != null ? dataReaderOrcl["MATHUE_VAO"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MATHUE_RA", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MATHUE_RA"] != null ? dataReaderOrcl["MATHUE_RA"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MADONVITINH", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MADONVITINH"] != null ? dataReaderOrcl["MADONVITINH"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MAKEHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAKEHANG"] != null ? dataReaderOrcl["MAKEHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("MABAOBI", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MABAOBI"] != null ? dataReaderOrcl["MABAOBI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("BARCODE", SqlDbType.VarChar, 2000).Value = dataReaderOrcl["BARCODE"] != null ? dataReaderOrcl["BARCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("ITEMCODE", SqlDbType.VarChar, 10).Value = dataReaderOrcl["ITEMCODE"] != null ? dataReaderOrcl["ITEMCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }
        public static void SYNCHRONIZE_MATHANG_GIA()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    string tableName = FrmXuatBanLeService.GET_TABLE_NAME_NGAYHACHTOAN_CSDL_ORACLE();
                    cmdOrcl.CommandText = string.Format(@"SELECT a.MAHANG,a.GIAMUA,a.GIAMUA_VAT,a.GIABANLE,a.GIABANLE_VAT,a.GIABANBUON,a.GIABANBUON_VAT,a.TYLE_LAILE,a.TYLE_LAIBUON,b.GIAVON,a.UNITCODE FROM MATHANG_GIA a INNER JOIN " + tableName + " b ON a.MAHANG = b.MAHANG WHERE a.UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND a.TRANGTHAI = 10 AND b.MAKHO = '" + Session.Session.CurrentWareHouse + "' ");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"TRUNCATE TABLE dbo.MATHANG_GIA");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.MATHANG_GIA(ID,MAHANG,GIAMUA,GIAMUA_VAT,GIABANLE,GIABANLE_VAT,GIABANBUON,GIABANBUON_VAT,TYLE_LAILE,TYLE_LAIBUON,GIAVON,UNITCODE) VALUES (@ID,@MAHANG,@GIAMUA,@GIAMUA_VAT,@GIABANLE,@GIABANLE_VAT,@GIABANBUON,@GIABANBUON_VAT,@TYLE_LAILE,@TYLE_LAIBUON,@GIAVON,@UNITCODE)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MAHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAHANG"] != null ? dataReaderOrcl["MAHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIAMUA", SqlDbType.Decimal).Value = dataReaderOrcl["GIAMUA"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["GIAMUA"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIAMUA_VAT", SqlDbType.Decimal).Value = dataReaderOrcl["GIAMUA_VAT"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["GIAMUA_VAT"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIABANLE", SqlDbType.Decimal).Value = dataReaderOrcl["GIABANLE"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["GIABANLE"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIABANLE_VAT", SqlDbType.Decimal).Value = dataReaderOrcl["GIABANLE_VAT"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["GIABANLE_VAT"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIABANBUON", SqlDbType.Decimal).Value = dataReaderOrcl["GIABANBUON"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["GIABANBUON"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIABANBUON_VAT", SqlDbType.Decimal).Value = dataReaderOrcl["GIABANBUON_VAT"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["GIABANBUON_VAT"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TYLE_LAILE", SqlDbType.Decimal).Value = dataReaderOrcl["TYLE_LAILE"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["TYLE_LAILE"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TYLE_LAIBUON", SqlDbType.Decimal).Value = dataReaderOrcl["TYLE_LAIBUON"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["TYLE_LAIBUON"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIAVON", SqlDbType.Decimal).Value = dataReaderOrcl["GIAVON"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["GIAVON"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }
        public static void SYNCHRONIZE_CUAHANG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT MA_CUAHANG,TEN_CUAHANG,DIACHI,UNITCODE FROM CUAHANG WHERE MA_CUAHANG = '" + Session.Session.CurrentUnitCode + "'");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"TRUNCATE TABLE dbo.CUAHANG");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.CUAHANG(ID,MA_CUAHANG,TEN_CUAHANG,DIACHI,UNITCODE) VALUES (@ID,@MA_CUAHANG,@TEN_CUAHANG,@DIACHI,@UNITCODE)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MA_CUAHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MA_CUAHANG"] != null ? dataReaderOrcl["MA_CUAHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TEN_CUAHANG", SqlDbType.NVarChar, 200).Value = dataReaderOrcl["TEN_CUAHANG"] != null ? dataReaderOrcl["TEN_CUAHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("DIACHI", SqlDbType.NVarChar, 200).Value = dataReaderOrcl["DIACHI"] != null ? dataReaderOrcl["DIACHI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }
        public static void SYNCHRONIZE_DONVITINH()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT MADONVITINH,TENDONVITINH FROM DONVITINH WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"TRUNCATE TABLE dbo.DONVITINH");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.DONVITINH(ID,MADONVITINH,TENDONVITINH) VALUES (@ID,@MADONVITINH,@TENDONVITINH)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MADONVITINH", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MADONVITINH"] != null ? dataReaderOrcl["MADONVITINH"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TENDONVITINH", SqlDbType.NVarChar, 200).Value = dataReaderOrcl["TENDONVITINH"] != null ? dataReaderOrcl["TENDONVITINH"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }


        public static void SYNCHRONIZE_THUE()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID, MATHUE,TENTHUE,GIATRI FROM THUE WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"TRUNCATE TABLE dbo.THUE");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.THUE(ID,MATHUE,TENTHUE,GIATRI) VALUES (@ID,@MATHUE,@TENTHUE,@GIATRI)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MATHUE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MATHUE"] != null ? dataReaderOrcl["MATHUE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TENTHUE", SqlDbType.NVarChar, 200).Value = dataReaderOrcl["TENTHUE"] != null ? dataReaderOrcl["TENTHUE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("GIATRI", SqlDbType.Decimal).Value = dataReaderOrcl["GIATRI"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["GIATRI"].ToString()) : 0;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }

        public static void SYNCHRONIZE_HANGKHACHHANG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,MAHANG,TENHANG,TYLE_SINHNHAT,TYLE_DACBIET,QUYDOITIEN_THANH_DIEM, QUYDOIDIEM_THANH_TIEN,HANG_KHOIDAU,SOTIEN_LENHANG FROM HANGKHACHHANG WHERE TRANGTHAI = 10 AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdCommonSql = new SqlCommand();
                                            cmdCommonSql.Connection = connectionSa;
                                            cmdCommonSql.CommandText = string.Format(@"TRUNCATE TABLE dbo.HANGKHACHHANG");
                                            cmdCommonSql.Transaction = tranSa;
                                            cmdCommonSql.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                cmdCommonSql.Parameters.Clear();
                                                cmdCommonSql.CommandText = string.Format(@"INSERT INTO dbo.HANGKHACHHANG(ID,MAHANG,TENHANG,TYLE_SINHNHAT,TYLE_DACBIET,QUYDOITIEN_THANH_DIEM,QUYDOIDIEM_THANH_TIEN,HANG_KHOIDAU,SOTIEN_LENHANG) VALUES (@ID,@MAHANG,@TENHANG,@TYLE_SINHNHAT,@TYLE_DACBIET,@QUYDOITIEN_THANH_DIEM,@QUYDOIDIEM_THANH_TIEN,@HANG_KHOIDAU,@SOTIEN_LENHANG)");
                                                cmdCommonSql.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdCommonSql.Parameters.Add("MAHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAHANG"] != null ? dataReaderOrcl["MAHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TENHANG", SqlDbType.NVarChar, 100).Value = dataReaderOrcl["TENHANG"] != null ? dataReaderOrcl["TENHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TYLE_SINHNHAT", SqlDbType.Decimal).Value = dataReaderOrcl["TYLE_SINHNHAT"] != null ? decimal.Parse(dataReaderOrcl["TYLE_SINHNHAT"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("TYLE_DACBIET", SqlDbType.Decimal).Value = dataReaderOrcl["TYLE_DACBIET"] != null ? decimal.Parse(dataReaderOrcl["TYLE_DACBIET"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("QUYDOITIEN_THANH_DIEM", SqlDbType.Decimal).Value = dataReaderOrcl["QUYDOITIEN_THANH_DIEM"] != null ? decimal.Parse(dataReaderOrcl["QUYDOITIEN_THANH_DIEM"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("QUYDOIDIEM_THANH_TIEN", SqlDbType.Decimal).Value = dataReaderOrcl["QUYDOIDIEM_THANH_TIEN"] != null ? decimal.Parse(dataReaderOrcl["QUYDOIDIEM_THANH_TIEN"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("HANG_KHOIDAU", SqlDbType.Int).Value = dataReaderOrcl["HANG_KHOIDAU"] != DBNull.Value ? int.Parse(dataReaderOrcl["HANG_KHOIDAU"].ToString()) : (object)DBNull.Value;
                                                cmdCommonSql.Parameters.Add("SOTIEN_LENHANG", SqlDbType.Decimal).Value = dataReaderOrcl["SOTIEN_LENHANG"] != DBNull.Value ? decimal.Parse(dataReaderOrcl["SOTIEN_LENHANG"].ToString()) : 0;
                                                cmdCommonSql.Transaction = tranSa;
                                                if (cmdCommonSql.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            dataReaderOrcl.Dispose();
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }
    }
}
