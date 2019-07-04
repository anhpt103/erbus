using ERBus.Entity.Database.Authorize;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Authorize.NguoiDung
{
    public class NguoiDungViewModel
    {
        public class Search : IDataSearch
        {
            public string USERNAME { get; set; }
            public string PASSWORD { get; set; }
            public string MANHANVIEN { get; set; }
            public string TENNHANVIEN { get; set; }
            public string SODIENTHOAI { get; set; }
            public string CHUNGMINHTHU { get; set; }
            public Nullable<int> GIOITINH { get; set; }
            public Nullable<int> VAITRO { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string PARENT_UNITCODE { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new NGUOIDUNG().USERNAME);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                USERNAME = summary;
                TENNHANVIEN = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new NGUOIDUNG();

                if (!string.IsNullOrEmpty(this.USERNAME))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.USERNAME),
                        Value = this.USERNAME,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENNHANVIEN))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENNHANVIEN),
                        Value = this.TENNHANVIEN,
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
            public string USERNAME { get; set; }
            public string PASSWORD { get; set; }
            public string MANHANVIEN { get; set; }
            public string TENNHANVIEN { get; set; }
            public string SODIENTHOAI { get; set; }
            public string CHUNGMINHTHU { get; set; }
            public Nullable<int> GIOITINH { get; set; }
            public Nullable<int> VAITRO { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string UNITCODE { get; set; }
            public string PARENT_UNITCODE { get; set; }
        }
    }
}
