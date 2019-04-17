using ERBus.Entity.Database.Authorize;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System.Collections.Generic;
namespace ERBus.Service.Authorize.NhomQuyen
{
    public class NhomQuyenViewModel
    {
        public class Search : IDataSearch
        {
            public string MANHOMQUYEN { get; set; }
            public string TENNHOMQUYEN { get; set; }
            public string DIENGIAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new NHOMQUYEN().MANHOMQUYEN);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MANHOMQUYEN = summary;
                TENNHOMQUYEN = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new NHOMQUYEN();

                if (!string.IsNullOrEmpty(this.MANHOMQUYEN))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MANHOMQUYEN),
                        Value = this.MANHOMQUYEN,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENNHOMQUYEN))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENNHOMQUYEN),
                        Value = this.TENNHOMQUYEN,
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
            public string MANHOMQUYEN { get; set; }
            public string TENNHOMQUYEN { get; set; }
            public string DIENGIAI { get; set; }
        }
    }
}
