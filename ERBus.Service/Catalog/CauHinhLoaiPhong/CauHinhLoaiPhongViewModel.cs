using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.CauHinhLoaiPhong
{
    public class CauHinhLoaiPhongViewModel
    {
        public class Search : IDataSearch
        {
            public string MALOAIPHONG { get; set; }
            public string MAHANG { get; set; }
            public int SOPHUT { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new CAUHINH_LOAIPHONG().MALOAIPHONG);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MALOAIPHONG = summary;
                MAHANG = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new CAUHINH_LOAIPHONG();

                if (!string.IsNullOrEmpty(this.MALOAIPHONG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MALOAIPHONG),
                        Value = this.MALOAIPHONG,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.MAHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MAHANG),
                        Value = this.MAHANG,
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
            public string MALOAIPHONG { get; set; }
            public string MAHANG { get; set; }
            public int SOPHUT { get; set; }
            public string UNITCODE { get; set; }
        }
    }
}
