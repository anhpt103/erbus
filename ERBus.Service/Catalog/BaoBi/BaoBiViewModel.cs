using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.BaoBi
{
    public class BaoBiViewModel
    {
        public class Search : IDataSearch
        {
            public string MABAOBI { get; set; }
            public string TENBAOBI { get; set; }
            public decimal SOLUONG { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new BAOBI().MABAOBI);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MABAOBI = summary;
                TENBAOBI = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new BAOBI();

                if (!string.IsNullOrEmpty(this.MABAOBI))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MABAOBI),
                        Value = this.MABAOBI,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENBAOBI))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENBAOBI),
                        Value = this.TENBAOBI,
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
            public string MABAOBI { get; set; }
            public string TENBAOBI { get; set; }
            public decimal SOLUONG { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
