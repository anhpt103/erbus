using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.KeHang
{
    public class KeHangViewModel
    {
        public class Search : IDataSearch
        {
            public string MAKEHANG { get; set; }
            public string TENKEHANG { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new KEHANG().MAKEHANG);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MAKEHANG = summary;
                TENKEHANG = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new KEHANG();

                if (!string.IsNullOrEmpty(this.MAKEHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MAKEHANG),
                        Value = this.MAKEHANG,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENKEHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENKEHANG),
                        Value = this.TENKEHANG,
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
            public string MAKEHANG { get; set; }
            public string TENKEHANG { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
