using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.NhomHang
{
    public class NhomHangViewModel
    {
        public class Search : IDataSearch
        {
            public string MANHOM { get; set; }
            public string TENNHOM { get; set; }
            public string MALOAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new NHOMHANG().MANHOM);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MANHOM = summary;
                TENNHOM = summary;
                MALOAI = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new NHOMHANG();
                if (!string.IsNullOrEmpty(this.MALOAI))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MALOAI),
                        Value = this.MALOAI,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.MANHOM))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MANHOM),
                        Value = this.MANHOM,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENNHOM))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENNHOM),
                        Value = this.TENNHOM,
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
            public string MANHOM { get; set; }
            public string TENNHOM { get; set; }
            public string MALOAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
