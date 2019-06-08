using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.Phong
{
    public class PhongViewModel
    {
        public class Search : IDataSearch
        {
            public string MAPHONG { get; set; }
            public string TENPHONG { get; set; }
            public string VITRI { get; set; }
            public int? TANG { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new PHONG().MAPHONG);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MAPHONG = summary;
                TENPHONG = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new PHONG();

                if (!string.IsNullOrEmpty(this.MAPHONG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MAPHONG),
                        Value = this.MAPHONG,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENPHONG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENPHONG),
                        Value = this.TENPHONG,
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
            public string MAPHONG { get; set; }
            public string TENPHONG { get; set; }
            public string VITRI { get; set; }
            public int? TANG { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
