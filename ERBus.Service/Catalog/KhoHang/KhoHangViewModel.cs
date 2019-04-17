using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.KhoHang
{
    public class KhoHangViewModel
    {
        public class Search : IDataSearch
        {
            public string MAKHO { get; set; }
            public string TENKHO { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new KHOHANG().MAKHO);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MAKHO = summary;
                TENKHO = summary;
                DIENGIAI = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new KHOHANG();

                if (!string.IsNullOrEmpty(this.MAKHO))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENKHO),
                        Value = this.MAKHO,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENKHO))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENKHO),
                        Value = this.TENKHO,
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
            public string MAKHO { get; set; }
            public string TENKHO { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
