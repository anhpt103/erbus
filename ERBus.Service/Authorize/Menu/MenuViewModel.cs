using ERBus.Entity.Database.Authorize;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Authorize.Menu
{
    public class MenuViewModel
    {
        public class Search : IDataSearch
        {
            public string MENU_CHA { get; set; }
            public string MA_MENU { get; set; }
            public string TIEUDE { get; set; }
            public int SAPXEP { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string ICON { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new MENU().MA_MENU);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MA_MENU = summary;
                TIEUDE = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new MENU();

                if (!string.IsNullOrEmpty(this.MA_MENU))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MA_MENU),
                        Value = this.MA_MENU,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TIEUDE))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TIEUDE),
                        Value = this.TIEUDE,
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
            public string MENU_CHA { get; set; }
            public string MA_MENU { get; set; }
            public string TIEUDE { get; set; }
            public int SAPXEP { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string ICON { get; set; }
        }
    }
}
