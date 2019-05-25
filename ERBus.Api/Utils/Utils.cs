using ERBus.Entity;
using System;
using System.Linq;

namespace ERBus.Api.Utils
{
    public class Utils
    {
        public static string NgayIn()
        {
            return DateTime.Now.ToString("dd/MM/yyyy");
        }
        public static string ChuyenDoiNgayThangNam(Nullable<DateTime> date)
        {
            if (date != null)
            {
                return date.Value.ToString("dd/MM/yyyy");
            }
            return DateTime.Now.ToString("dd/MM/yyyy");
        }

        public static string TenCuaHang(string unitCode)
        {
            string result = "";
            if (!string.IsNullOrEmpty(unitCode))
            {
                using (var context = new ERBusContext())
                {
                    var donvis = context.CUAHANGs.FirstOrDefault(x => x.MA_CUAHANG == unitCode);
                    if (donvis != null)
                    {
                        result = donvis.TEN_CUAHANG;
                    }
                }
            }
            return result;
        }

        public static string DiaChiCuaHang(string unitCode)
        {
            string result = "";
            if (!string.IsNullOrEmpty(unitCode))
            {
                using (var context = new ERBusContext())
                {
                    var donvis = context.CUAHANGs.FirstOrDefault(x => x.MA_CUAHANG == unitCode);
                    if (donvis != null)
                    {
                        result = donvis.DIACHI;
                    }
                }
            }
            return result;
        }

        public static string DieuKienNhom(string dieuKien)
        {
            string result = "";
            if (dieuKien == "KHOHANG") { result = "MÃ KHO"; }
            else if (dieuKien == "LOAIHANG") { result = "LOẠI HÀNG"; }
            else if (dieuKien == "NHOMHANG") { result = "NHÓM HÀNG"; }
            else if (dieuKien == "NHACUNGCAP") { result = "NHÀ CUNG CẤP"; }
            else if (dieuKien == "KHACHHANG") { result = "KHÁCH HÀNG"; }
            else if (dieuKien == "MATHANG") { result = "MẶT HÀNG"; }
            return result;
        }

        public static string DieuKienLoc(string MAKHO, string MALOAI, string MANHOM, string MANHACUNGCAP, string MAKHACHHANG, string MAHANG)
        {
            string result = "";
            if (!string.IsNullOrEmpty(MAKHO))
            {
                result += "Mã kho: " + MAKHO.ToString() + "; ";
            }
            if (!string.IsNullOrEmpty(MALOAI))
            {
                result += "Mã loại: " + MAKHO + "; ";
            }
            if (!string.IsNullOrEmpty(MANHOM))
            {
                result += "Mã nhóm: " + MAKHO + "; ";
            }
            if (!string.IsNullOrEmpty(MANHACUNGCAP))
            {
                result += "Nhà cung cấp: " + MAKHO + "; ";
            }
            if (!string.IsNullOrEmpty(MAKHACHHANG))
            {
                result += "Khách hàng: " + MAKHACHHANG + "; ";
            }
            if (!string.IsNullOrEmpty(MAHANG))
            {
                result += "Mã hàng: " + MAHANG + "; ";
            }
            if (string.IsNullOrEmpty(MAKHO) && string.IsNullOrEmpty(MALOAI) && string.IsNullOrEmpty(MANHOM) && string.IsNullOrEmpty(MANHACUNGCAP) && string.IsNullOrEmpty(MAHANG)) result = "Không lọc";
            return result;
        }
    }
}