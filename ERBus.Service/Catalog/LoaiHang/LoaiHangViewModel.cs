using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.LoaiHang
{
    public class LoaiHangViewModel
    {
        public class Search : IDataSearch
        {
            public string MALOAI { get; set; }
            public string TENLOAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new LOAIHANG().MALOAI);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MALOAI = summary;
                TENLOAI = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new LOAIHANG();

                if (!string.IsNullOrEmpty(this.MALOAI))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MALOAI),
                        Value = this.MALOAI,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENLOAI))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENLOAI),
                        Value = this.TENLOAI,
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
            public string MALOAI { get; set; }
            public string TENLOAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
        }
    }
}
