using ERBus.Entity.Database.Authorize;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Authorize.CuaHang
{
    public class CuaHangViewModel
    {
        public class Search : IDataSearch
        {
            public string MA_CUAHANG { get; set; }
            public string TEN_CUAHANG { get; set; }
            public string DIACHI { get; set; }
            public string MA_CUAHANG_CHA { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new CUAHANG().MA_CUAHANG);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MA_CUAHANG = summary;
                TEN_CUAHANG = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new CUAHANG();

                if (!string.IsNullOrEmpty(this.MA_CUAHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MA_CUAHANG),
                        Value = this.MA_CUAHANG,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TEN_CUAHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TEN_CUAHANG),
                        Value = this.TEN_CUAHANG,
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
            public string MA_CUAHANG { get; set; }
            public string TEN_CUAHANG { get; set; }
            public string DIACHI { get; set; }
            public string MA_CUAHANG_CHA { get; set; }
        }
    }
}
