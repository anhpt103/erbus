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
            public string MA_DATPHONG { get; set; }
            public string MAPHONG { get; set; }
            public DateTime NGAY_DATPHONG { get; set; }
            public string THOIGIAN_DATPHONG { get; set; }
            public string TEN_KHACHHANG { get; set; }
            public string DIENTHOAI { get; set; }
            public string CANCUOC_CONGDAN { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
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
                MA_DATPHONG = summary;
            }
            public List<IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new DATPHONG();

                if (!string.IsNullOrEmpty(this.MA_DATPHONG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MA_DATPHONG),
                        Value = this.MA_DATPHONG,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.MAPHONG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MAPHONG),
                        Value = this.MAPHONG,
                        Method = FilterMethod.Like
                    });
                }
                if (this.NGAY_DATPHONG != null)
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.NGAY_DATPHONG),
                        Value = this.NGAY_DATPHONG.Date,
                        Method = FilterMethod.GreaterThanOrEqualTo
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
            public string MA_DATPHONG { get; set; }
            public string MAPHONG { get; set; }
            public DateTime NGAY_DATPHONG { get; set; }
            public string THOIGIAN_DATPHONG { get; set; }
            public string TEN_KHACHHANG { get; set; }
            public string DIENTHOAI { get; set; }
            public string CANCUOC_CONGDAN { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
