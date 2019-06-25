using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.BoHang
{
    public class BoHangViewModel
    {
        public class Search : IDataSearch
        {
            public string MABOHANG { get; set; }
            public string TENBOHANG { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new BOHANG().MABOHANG);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MABOHANG = summary;
                TENBOHANG = summary;
                DIENGIAI = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new BOHANG();

                if (!string.IsNullOrEmpty(this.MABOHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENBOHANG),
                        Value = this.MABOHANG,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENBOHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENBOHANG),
                        Value = this.TENBOHANG,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.DIENGIAI))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.DIENGIAI),
                        Value = this.DIENGIAI,
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
                DataDetails = new List<DataDetails>();
            }
            public string MABOHANG { get; set; }
            public string TENBOHANG { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public List<DataDetails> DataDetails { get; set; }
            public string UNITCODE { get; set; }
        }

        public class DataDetails : DataInfoEntityDto
        {
            public string MABOHANG { get; set; }
            public string MAHANG { get; set; }
            public string TENHANG { get; set; }
            public string MADONVITINH { get; set; }
            public decimal GIABANLE_VAT { get; set; }
            public decimal SOLUONG { get; set; }
            public decimal CHIETKHAU { get; set; }
            public decimal TONGTIEN { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string UNITCODE { get; set; }
        }
    }
}
