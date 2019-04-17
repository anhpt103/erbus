using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.HangKhachHang
{
    public class HangKhachHangViewModel
    {
        public class Search : IDataSearch
        {
            public string MAHANG { get; set; }
            public string TENHANG { get; set; }
            public decimal SOTIEN_LENHANG { get; set; }
            public decimal TYLE_SINHNHAT { get; set; }
            public decimal TYLE_DACBIET { get; set; }
            public decimal QUYDOITIEN_THANH_DIEM { get; set; }
            public decimal QUYDOIDIEM_THANH_TIEN { get; set; }
            public int? HANG_KHOIDAU { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new HANGKHACHHANG().MAHANG);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MAHANG = summary;
                TENHANG = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new HANGKHACHHANG();
                if (!string.IsNullOrEmpty(this.MAHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MAHANG),
                        Value = this.MAHANG,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENHANG),
                        Value = this.TENHANG,
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
            public string MAHANG { get; set; }
            public string TENHANG { get; set; }
            public decimal SOTIEN_LENHANG { get; set; }
            public decimal TYLE_SINHNHAT { get; set; }
            public decimal TYLE_DACBIET { get; set; }
            public decimal QUYDOITIEN_THANH_DIEM { get; set; }
            public decimal QUYDOIDIEM_THANH_TIEN { get; set; }
            public int? HANG_KHOIDAU { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
