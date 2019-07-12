using ERBus.Entity.Database.Knowledge;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Knowledge.DatPhong
{
    public class DatPhongViewModel
    {
        public class Search : IDataSearch
        {
            public string MAPHONG { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new DATPHONG().MAPHONG);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MAPHONG = summary;
            }
            public List<IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new DATPHONG();
                
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

        public class ViewModel
        {
            public string ID { get; set; }
            public string MA_DATPHONG { get; set; }
            public string MAPHONG { get; set; }
            public string MALOAIPHONG { get; set; }
            public DateTime? NGAY_DATPHONG { get; set; }
            public string THOIGIAN_DATPHONG { get; set; }
            public string TEN_KHACHHANG { get; set; }
            public string DIENTHOAI { get; set; }
            public string CANCUOC_CONGDAN { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string UNITCODE { get; set; }
        }

        public class Dto : DataInfoEntityDto
        {
            public string MA_DATPHONG { get; set; }
            public string MAPHONG { get; set; }
            public DateTime? NGAY_DATPHONG { get; set; }
            public string THOIGIAN_DATPHONG { get; set; }
            public string TEN_KHACHHANG { get; set; }
            public string DIENTHOAI { get; set; }
            public string CANCUOC_CONGDAN { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }

        public class DatPhongPayDto
        {
            public string MA_DATPHONG { get; set; }
            public string MAPHONG { get; set; }
            public string TENPHONG { get; set; }
            public int? TANG { get; set; }
            public string VITRI { get; set; }
            public string MAKHO { get; set; }
            public string MALOAIPHONG { get; set; }
            public string MABOHANG { get; set; }
            public string TENLOAIPHONG { get; set; }
            public DateTime? NGAY_DATPHONG { get; set; }
            public string THOIGIAN_DATPHONG { get; set; }
            public string TEN_KHACHHANG { get; set; }
            public string DIENTHOAI { get; set; }
            public string CANCUOC_CONGDAN { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string I_CREATE_BY { get; set; }
        }
    }
}
