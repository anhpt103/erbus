using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.DonViTinh
{
    public class DonViTinhViewModel
    {
        public class Search : IDataSearch
        {
            public string MADONVITINH { get; set; }
            public string TENDONVITINH { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new DONVITINH().MADONVITINH);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MADONVITINH = summary;
                TENDONVITINH = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new DONVITINH();
                if (!string.IsNullOrEmpty(this.MADONVITINH))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MADONVITINH),
                        Value = this.MADONVITINH,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENDONVITINH))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENDONVITINH),
                        Value = this.TENDONVITINH,
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
            public string MADONVITINH { get; set; }
            public string TENDONVITINH { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
