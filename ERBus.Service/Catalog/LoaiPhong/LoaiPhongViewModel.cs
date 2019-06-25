using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.LoaiPhong
{
    public class LoaiPhongViewModel
    {
        public class Search : IDataSearch
        {
            public string MALOAIPHONG { get; set; }
            public string TENLOAIPHONG { get; set; }
            public string MABOHANG { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new LOAIPHONG().MALOAIPHONG);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MALOAIPHONG = summary;
                TENLOAIPHONG = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new LOAIPHONG();

                if (!string.IsNullOrEmpty(this.MALOAIPHONG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MALOAIPHONG),
                        Value = this.MALOAIPHONG,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENLOAIPHONG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENLOAIPHONG),
                        Value = this.TENLOAIPHONG,
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
            public string TENLOAIPHONG { get; set; }
            public string MABOHANG { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string BACKGROUND_NAME { get; set; }
            public string ICON_NAME { get; set; }
            public byte[] BACKGROUND { get; set; }
            public byte[] ICON { get; set; }
            public string MAHANG { get; set; }
            public int SOPHUT { get; set; }
        }

        public class DtoCauHinh 
        {
            public string MALOAIPHONG { get; set; }
            public string TENLOAIPHONG { get; set; }
            public string MABOHANG { get; set; }
            public string MAHANG { get; set; }
            public int SOPHUT { get; set; }
            public string UNITCODE { get; set; }
        }

        public class InfoUpload
        {
            public string FILENAME { get; set; }
        }
    }
}
