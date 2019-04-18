using ERBus.Entity.Database.Knowledge;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Knowledge.XuatBanLeThuNgan
{
    public class XuatBanLeThuNganViewModel
    {
        public class Search : IDataSearch
        {
            public string MA_GIAODICH { get; set; }
            public string LOAI_GIAODICH { get; set; }
            public DateTime NGAY_GIAODICH { get; set; }
            public string MAKHACHHANG { get; set; }
            public string THOIGIAN_TAO { get; set; }
            public decimal TIENKHACH_TRA { get; set; }
            public decimal TIEN_TRALAI_KHACH { get; set; }
            public string MAKHO_XUAT { get; set; }
            public string MA_VOUCHER { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new GIAODICH().MA_GIAODICH);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MA_GIAODICH = summary;
            }
            public List<IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new GIAODICH();
                if (!string.IsNullOrEmpty(this.MA_GIAODICH))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MA_GIAODICH),
                        Value = this.MA_GIAODICH,
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

        public class View : DataInfoEntityDto
        {
            public string MA_GIAODICH { get; set; }
            public string LOAI_GIAODICH { get; set; }
            public DateTime NGAY_GIAODICH { get; set; }
            public string MAKHACHHANG { get; set; }
            public string THOIGIAN_TAO { get; set; }
            public decimal TIENKHACH_TRA { get; set; }
            public decimal TIEN_TRALAI_KHACH { get; set; }
            public string MAKHO_XUAT { get; set; }
            public string MA_VOUCHER { get; set; }
            public string DIENGIAI { get; set; }
            public decimal THANHTIEN { get; set; }
        }

        public class Dto : DataInfoEntityDto
        {
            public Dto()
            {
                DataDetails = new List<DtoDetails>();
            }
            public string MA_GIAODICH { get; set; }
            public string LOAI_GIAODICH { get; set; }
            public DateTime NGAY_GIAODICH { get; set; }
            public string MAKHACHHANG { get; set; }
            public string THOIGIAN_TAO { get; set; }
            public decimal TIENKHACH_TRA { get; set; }
            public decimal TIEN_TRALAI_KHACH { get; set; }
            public string MAKHO_XUAT { get; set; }
            public string MA_VOUCHER { get; set; }
            public string DIENGIAI { get; set; }
            public decimal THANHTIEN { get; set; }
            public List<DtoDetails> DataDetails { get; set; }
        }

        public class DtoDetails{
            public string ID { get; set; }
            public string MA_GIAODICH { get; set; }
            public string MAHANG { get; set; }
            public string TENHANG { get; set; }
            public string MADONVITINH { get; set; }
            public string MATHUE_RA { get; set; }
            public decimal SOLUONG { get; set; }
            public decimal GIABANLE_VAT { get; set; }
            public string MA_KHUYENMAI { get; set; }
            public decimal TYLE_KHUYENMAI { get; set; }
            public decimal TIEN_KHUYENMAI { get; set; }
            public decimal TYLE_CHIETKHAU { get; set; }
            public decimal TIEN_CHIETKHAU { get; set; }
            public decimal TIENTHE_VIP { get; set; }
            public decimal TIEN_VOUCHER { get; set; }
            public decimal THANHTIEN { get; set; }
            public int? SAPXEP { get; set; }
        }

        public class ParamApproval
        {
            public string ID { get; set; }
        }
    }
}
