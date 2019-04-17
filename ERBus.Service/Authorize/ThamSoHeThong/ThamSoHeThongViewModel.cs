using ERBus.Entity.Database.Authorize;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System.Collections.Generic;
namespace ERBus.Service.Authorize.ThamSoHeThong
{
    public class ThamSoHeThongViewModel
    {
        public class Search : IDataSearch
        {
            public string MA_THAMSO { get; set; }
            public string TEN_THAMSO { get; set; }
            public int TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new THAMSOHETHONG().MA_THAMSO);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MA_THAMSO = summary;
                TEN_THAMSO = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new THAMSOHETHONG();
                if (!string.IsNullOrEmpty(this.MA_THAMSO))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MA_THAMSO),
                        Value = this.MA_THAMSO,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TEN_THAMSO))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TEN_THAMSO),
                        Value = this.TEN_THAMSO,
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
            public string MA_THAMSO { get; set; }
            public string TEN_THAMSO { get; set; }
            public int TRANGTHAI { get; set; }
            public int GIATRI_SO { get; set; }
            public string GIATRI_CHU { get; set; }
            public string UNITCODE { get; set; }
            public bool ISDIABLED_GIATRI_CHU { get; set; }
            public string PLACEHOLDER { get; set; }
        }
    }
}
