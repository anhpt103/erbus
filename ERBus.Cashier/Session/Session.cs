using System;
using System.Configuration;
using System.Data;
using ERBus.Cashier.Dto;
using Oracle.ManagedDataAccess.Client;

namespace ERBus.Cashier.Session
{
    public class Session
    {
        public static DateTime CurrentNgayPhatSinh;
        public static string CurrentUserName;
        public static string CurrentMaNhanVien;
        public static string CurrentTenNhanVien;
        public static string CurrentUnitCode;
        public static string CurrentCodeStore;
        public static string CurrentWareHouse;
        public static string CurrentAddress;
        public static string CurrentPhone;
        public static string CurrentNameStore;
        public static string CurrentPeriod;
        public static string CurrentYear;
        public static string CurrentLoaiGiaoDich;
        public static string CurrentTableNamePeriod;
        public static bool SESSION_ONLINE = true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitCode"></param>
        /// <param name="UserName"></param>
        /// <param name="MaChucNang">banLeQuayThuNgan</param>
        /// <returns></returns>
        public static ROLE_STATE GET_ROLE_BY_USERNAME(string UnitCode, string UserName, string MaChucNang)
        {
            ROLE_STATE roleState = new ROLE_STATE();
            roleState.STATE = MaChucNang;
            if (UserName == "admin")
            {
                roleState = new ROLE_STATE()
                {
                    STATE = MaChucNang,
                    View = true,
                    Approve = true,
                    Delete = true,
                    Add = true,
                    Edit = true,
                    Giamua = true,
                    Giaban = true,
                    Giavon = true,
                    Tylelai = true,
                    Banchietkhau = true,
                    Banbuon = true,
                    Bantralai = true
                };
            }
            else
            {
                using (
                    var connection =
                        new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    connection.OpenAsync();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.InitialLONGFetchSize = 1000;
                    cmd.CommandText =
                        @"SELECT AU_NHOMQUYEN_CHUCNANG.XEM,AU_NHOMQUYEN_CHUCNANG.THEM,AU_NHOMQUYEN_CHUCNANG.SUA,AU_NHOMQUYEN_CHUCNANG.XOA,AU_NHOMQUYEN_CHUCNANG.DUYET,AU_NHOMQUYEN_CHUCNANG.GIAMUA,AU_NHOMQUYEN_CHUCNANG.GIABAN,AU_NHOMQUYEN_CHUCNANG.GIAVON,AU_NHOMQUYEN_CHUCNANG.TYLELAI,AU_NHOMQUYEN_CHUCNANG.BANCHIETKHAU,AU_NHOMQUYEN_CHUCNANG.BANBUON,AU_NHOMQUYEN_CHUCNANG.BANTRALAI FROM AU_NHOMQUYEN_CHUCNANG WHERE UNITCODE = '" + UnitCode +
                        "' AND MACHUCNANG='" + MaChucNang +
                        "' AND MANHOMQUYEN IN (SELECT MANHOMQUYEN FROM AU_NGUOIDUNG_NHOMQUYEN WHERE UNITCODE='" +
                        UnitCode + "' AND USERNAME='" +
                        UserName +
                        "') UNION " +
                        "SELECT AU_NGUOIDUNG_QUYEN.XEM,AU_NGUOIDUNG_QUYEN.THEM,AU_NGUOIDUNG_QUYEN.SUA,AU_NGUOIDUNG_QUYEN.XOA,AU_NGUOIDUNG_QUYEN.DUYET,AU_NGUOIDUNG_QUYEN.GIAMUA,AU_NGUOIDUNG_QUYEN.GIABAN,AU_NGUOIDUNG_QUYEN.GIAVON,AU_NGUOIDUNG_QUYEN.TYLELAI,AU_NGUOIDUNG_QUYEN.BANCHIETKHAU,AU_NGUOIDUNG_QUYEN.BANBUON,AU_NGUOIDUNG_QUYEN.BANTRALAI " +
                        "FROM AU_NGUOIDUNG_QUYEN WHERE AU_NGUOIDUNG_QUYEN.UNITCODE = '" + UnitCode +
                        "' AND AU_NGUOIDUNG_QUYEN.MACHUCNANG = '" + MaChucNang + "' AND AU_NGUOIDUNG_QUYEN.USERNAME = '" +
                        UserName + "'";
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader oracleDataReader = null;
                    oracleDataReader = cmd.ExecuteReader();
                    if (!oracleDataReader.HasRows)
                    {
                        roleState = new ROLE_STATE()
                        {
                            STATE = string.Empty,
                            View = false,
                            Approve = false,
                            Delete = false,
                            Add = false,
                            Edit = false,
                            Giamua = false,
                            Giaban = false,
                            Giavon = false,
                            Tylelai = false,
                            Banchietkhau = false,
                            Banbuon = false,
                            Bantralai = false
                        };
                    }
                    else
                    {
                        while (oracleDataReader.Read())
                        {
                            roleState.STATE = MaChucNang;
                            int objXem = Int32.Parse(oracleDataReader["XEM"].ToString());
                            if (objXem == 1)
                            {
                                roleState.View = true;
                            }
                            int objThem = Int32.Parse(oracleDataReader["THEM"].ToString());
                            if (objThem == 1)
                            {
                                roleState.Add = true;
                            }
                            int objSua = Int32.Parse(oracleDataReader["SUA"].ToString());
                            if (objSua == 1)
                            {
                                roleState.Edit = true;
                            }
                            int objXoa = Int32.Parse(oracleDataReader["XOA"].ToString());
                            if (objXoa == 1)
                            {
                                roleState.Delete = true;
                            }
                            int objDuyet = Int32.Parse(oracleDataReader["DUYET"].ToString());
                            if (objDuyet == 1)
                            {
                                roleState.Approve = true;
                            }
                            int objGiamua = Int32.Parse(oracleDataReader["GIAMUA"].ToString());
                            if (objGiamua == 1)
                            {
                                roleState.Giamua = true;
                            }
                            int objGiaban = Int32.Parse(oracleDataReader["GIABAN"].ToString());
                            if (objGiaban == 1)
                            {
                                roleState.Giaban = true;
                            }
                            int objGiavon = Int32.Parse(oracleDataReader["GIAVON"].ToString());
                            if (objGiavon == 1)
                            {
                                roleState.Giavon = true;
                            }
                            int objTylelai = Int32.Parse(oracleDataReader["TYLELAI"].ToString());
                            if (objTylelai == 1)
                            {
                                roleState.Tylelai = true;
                            }
                            int objBanchietkhau = Int32.Parse(oracleDataReader["BANCHIETKHAU"].ToString());
                            if (objBanchietkhau == 1)
                            {
                                roleState.Banchietkhau = true;
                            }
                            int objBanbuon = Int32.Parse(oracleDataReader["BANBUON"].ToString());
                            if (objBanbuon == 1)
                            {
                                roleState.Banbuon = true;
                            }
                            int objBantralai = Int32.Parse(oracleDataReader["BANTRALAI"].ToString());
                            if (objBantralai == 1)
                            {
                                roleState.Bantralai = true;
                            }
                        }
                    }
                }
            }
            return roleState;
        }
    }
}
