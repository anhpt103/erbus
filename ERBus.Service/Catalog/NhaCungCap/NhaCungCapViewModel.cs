using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.NhaCungCap
{
    public class NhaCungCapViewModel
    {
        public class Search : IDataSearch
        {
            public string MANHACUNGCAP { get; set; }
            public string TENNHACUNGCAP { get; set; }
            public string DIACHI { get; set; }
            public string MASOTHUE { get; set; }
            public string DIENTHOAI { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new NHACUNGCAP().MANHACUNGCAP);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MANHACUNGCAP = summary;
                TENNHACUNGCAP = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new NHACUNGCAP();
                if (!string.IsNullOrEmpty(this.MANHACUNGCAP))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MANHACUNGCAP),
                        Value = this.MANHACUNGCAP,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENNHACUNGCAP))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENNHACUNGCAP),
                        Value = this.TENNHACUNGCAP,
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
            public string MANHACUNGCAP { get; set; }
            public string TENNHACUNGCAP { get; set; }
            public string DIACHI { get; set; }
            public string MASOTHUE { get; set; }
            public string DIENTHOAI { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
