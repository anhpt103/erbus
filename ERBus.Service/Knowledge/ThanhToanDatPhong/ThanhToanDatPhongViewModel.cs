using ERBus.Entity.Database.Knowledge;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Knowledge.ThanhToanDatPhong
{
    public class ThanhToanDatPhongViewModel
    {
        public class Search : IDataSearch
        {
            public string MAPHONG { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new THANHTOAN_DATPHONG().MAPHONG);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MAPHONG = summary;
            }
            public List<IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new THANHTOAN_DATPHONG();
                
                if (!string.IsNullOrEmpty(this.MAPHONG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MAPHONG),
                        Value = this.MAPHONG,
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
                DtoDetails = new List<DtoDetail>();
            }
            public string MA_DATPHONG { get; set; }
            public string MAPHONG { get; set; }
            public DateTime? NGAY_DATPHONG { get; set; }
            public string THOIGIAN_DATPHONG { get; set; }
            public string THOIGIAN_THANHTOAN { get; set; }
            public string THOIGIAN_SUDUNG { get; set; }
            public int? DONVI_THOIGIAN_TINHTIEN { get; set; }
            public string TEN_KHACHHANG { get; set; }
            public string DIENTHOAI { get; set; }
            public string CANCUOC_CONGDAN { get; set; }
            public string DIENGIAI { get; set; }
            public string MABOHANG { get; set; }
            public string MAHANG { get; set; }
            public string MAHANG_DICHVU { get; set; }
            public List<DtoDetail> DtoDetails { get; set; }
        }

        public class DtoDetail
        {
            public string MABOHANG { get; set; }
            public string MAHANG { get; set; }
            public string TENHANG { get; set; }
            public string DONVITINH { get; set; }
            public decimal SOLUONG { get; set; }
            public decimal CHIETKHAU { get; set; }
            public string MATHUE_RA { get; set; }
            public decimal GIABANLE_VAT { get; set; }
            public string UNITCODE { get; set; }
            public int? SAPXEP { get; set; }
        }

        public class LichSuDatPhong_Dto
        {
            public string ID { get; set; }
            public string MA_DATPHONG { get; set; }
            public string MAPHONG { get; set; }
            public DateTime NGAY_DATPHONG { get; set; }
            public string THOIGIAN_DATPHONG { get; set; }
            public string TEN_KHACHHANG { get; set; }
            public string DIENTHOAI { get; set; }
            public string CANCUOC_CONGDAN { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public DateTime? I_CREATE_DATE { get; set; }
            public string I_CREATE_BY { get; set; }
            public string UNITCODE { get; set; }
        }

        public class ObjSendGmail
        {
            public string MA_DATPHONG { get; set; }
            public string MAPHONG { get; set; }
            public string UNITCODE { get; set; }
            public string BODY { get; set; }
        }
    }
}
