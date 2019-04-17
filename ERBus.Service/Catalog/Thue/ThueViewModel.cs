using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.Thue
{
    public class ThueViewModel
    {
        public class Search : IDataSearch
        {
            public string MATHUE { get; set; }
            public string TENTHUE { get; set; }
            public decimal GIATRI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new THUE().MATHUE);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MATHUE = summary;
                TENTHUE = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new THUE();

                if (!string.IsNullOrEmpty(this.MATHUE))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MATHUE),
                        Value = this.MATHUE,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENTHUE))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENTHUE),
                        Value = this.TENTHUE,
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
            public string MATHUE { get; set; }
            public string TENTHUE { get; set; }
            public decimal GIATRI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
