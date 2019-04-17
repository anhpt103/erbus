using ERBus.Entity.Database.Knowledge;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Knowledge.XuatBan
{
    public class XuatBanViewModel
    {
        public class Search : IDataSearch
        {
            public string MA_CHUNGTU { get; set; }
            public string LOAI_CHUNGTU { get; set; }
            public DateTime NGAY_CHUNGTU { get; set; }
            public string MAKHACHHANG { get; set; }
            public DateTime? NGAY_DUYETPHIEU { get; set; }
            public string THOIGIAN_TAO { get; set; }
            public string THOIGIAN_DUYET { get; set; }
            public DateTime? NGAY_DIEUDONG { get; set; }
            public string MAKHO_NHAP { get; set; }
            public string MAKHO_XUAT { get; set; }
            public string MADONVI_NHAP { get; set; }
            public string MADONVI_XUAT { get; set; }
            public decimal? TIEN_CHIETKHAU { get; set; }
            public string MATHUE_TOANDON { get; set; }
            public string LENH_DIEUDONG { get; set; }
            public string MA_LYDO { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new CHUNGTU().MA_CHUNGTU);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MA_CHUNGTU = summary;
            }
            public List<IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new CHUNGTU();
                if (!string.IsNullOrEmpty(this.MA_CHUNGTU))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MA_CHUNGTU),
                        Value = this.MA_CHUNGTU,
                        Method = FilterMethod.Like
                    });
                }
                return result;
            }
            public List<IQueryFilter> GetQuickFilters()
            {
                return null;
            }
        }
        public class Dto : DataInfoEntityDto
        {
            public Dto()
            {
                DataDetails = new List<DtoDetails>();
            }
            public string MA_CHUNGTU { get; set; }
            public string LOAI_CHUNGTU { get; set; }
            public DateTime NGAY_CHUNGTU { get; set; }
            public string MAKHACHHANG { get; set; }
            public DateTime? NGAY_DUYETPHIEU { get; set; }
            public string THOIGIAN_TAO { get; set; }
            public string THOIGIAN_DUYET { get; set; }
            public DateTime? NGAY_DIEUDONG { get; set; }
            public string MAKHO_NHAP { get; set; }
            public string MAKHO_XUAT { get; set; }
            public string MADONVI_NHAP { get; set; }
            public string MADONVI_XUAT { get; set; }
            public decimal? TIEN_CHIETKHAU { get; set; }
            public string MATHUE_TOANDON { get; set; }
            public string LENH_DIEUDONG { get; set; }
            public string MA_LYDO { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public List<DtoDetails> DataDetails { get; set; }
        }

        public class DtoDetails{
            public string ID { get; set; }
            public string MA_CHUNGTU { get; set; }
            public string MAHANG { get; set; }
            public string TENHANG { get; set; }
            public string MADONVITINH { get; set; }
            public decimal TYLE_LAILE { get; set; }
            public string MATHUE_RA { get; set; }
            public string MATHUE_VAO { get; set; }
            public decimal SOLUONG { get; set; }
            public decimal GIAMUA { get; set; }
            public decimal GIAMUA_VAT { get; set; }
            public decimal? TIEN_GIAMGIA { get; set; }
            public decimal THANHTIEN { get; set; }
            public int? INDEX { get; set; }
        }

        public class ParamApproval
        {
            public string ID { get; set; }
        }
    }
}
