using ERBus.Entity.Database.Authorize;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Authorize.KyKeToan
{
    public class KyKeToanViewModel
    {
        public class Search : IDataSearch
        {
            public int KY { get; set; }
            public string TENKY { get; set; }
            public DateTime TUNGAY { get; set; }
            public DateTime DENNGAY { get; set; }
            public int NAM { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new MENU().MA_MENU);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                TENKY = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new MENU();

                if (!string.IsNullOrEmpty(this.TENKY))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MA_MENU),
                        Value = this.TENKY,
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
            public int KY { get; set; }
            public string TENKY { get; set; }
            public DateTime TUNGAY { get; set; }
            public DateTime DENNGAY { get; set; }
            public int NAM { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }

        public class ViewModel
        {
            public int KY { get; set; }
            public int NAM { get; set; }
            public DateTime NGAYKETOAN { get; set; }
            public string TABLE_NAME { get; set; }
        }

        public class ParamKyKeToan
        {
            public DateTime TUNGAY { get; set; }
            public DateTime DENNGAY { get; set; }
        }
    }
}
