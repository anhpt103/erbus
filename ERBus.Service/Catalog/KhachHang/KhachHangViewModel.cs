using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.KhachHang
{
    public class KhachHangViewModel
    {
        public class Search : IDataSearch
        {
            public string MAKHACHHANG { get; set; }
            public string TENKHACHHANG { get; set; }
            public string DIACHI { get; set; }
            public string DIENTHOAI { get; set; }
            public string CANCUOC_CONGDAN { get; set; }
            public DateTime? NGAYSINH { get; set; }
            public DateTime? NGAYDACBIET { get; set; }
            public string MATHE { get; set; }
            public decimal SODIEM { get; set; }
            public decimal TONGTIEN { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new KHACHHANG().MAKHACHHANG);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MAKHACHHANG = summary;
                TENKHACHHANG = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new KHACHHANG();
                if (!string.IsNullOrEmpty(this.MAKHACHHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MAKHACHHANG),
                        Value = this.MAKHACHHANG,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENKHACHHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENKHACHHANG),
                        Value = this.TENKHACHHANG,
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
            public string MAKHACHHANG { get; set; }
            public string TENKHACHHANG { get; set; }
            public string DIACHI { get; set; }
            public string DIENTHOAI { get; set; }
            public string CANCUOC_CONGDAN { get; set; }
            public DateTime? NGAYSINH { get; set; }
            public DateTime? NGAYDACBIET { get; set; }
            public string MATHE { get; set; }
            public decimal SODIEM { get; set; }
            public decimal TONGTIEN { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
