using ERBus.Entity.Database.Knowledge;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Promotion.TienTyLe
{
    public class TienTyLeViewModel
    {
        public class Search : IDataSearch
        {
            public string MA_KHUYENMAI { get; set; }
            public string LOAI_KHUYENMAI { get; set; }
            public DateTime TUNGAY { get; set; }
            public DateTime DENNGAY { get; set; }
            public string TUGIO { get; set; }
            public string DENGIO { get; set; }
            public string THOIGIAN_TAO { get; set; }
            public string THOIGIAN_DUYET { get; set; }
            public string MAKHO_KHUYENMAI { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new KHUYENMAI().MA_KHUYENMAI);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MA_KHUYENMAI = summary;
            }
            public List<IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new KHUYENMAI();
                if (!string.IsNullOrEmpty(this.MA_KHUYENMAI))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MA_KHUYENMAI),
                        Value = this.MA_KHUYENMAI,
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
            public Dto()
            {
                DataDetails = new List<DtoDetails>();
            }
            public string MA_KHUYENMAI { get; set; }
            public string LOAI_KHUYENMAI { get; set; }
            public DateTime TUNGAY { get; set; }
            public DateTime DENNGAY { get; set; }
            public string TUGIO { get; set; }
            public string DENGIO { get; set; }
            public string THOIGIAN_TAO { get; set; }
            public string THOIGIAN_DUYET { get; set; }
            public string MAKHO_KHUYENMAI { get; set; }
            public string DIENGIAI { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public List<DtoDetails> DataDetails { get; set; }
        }

        public class DtoDetails{
            public string ID { get; set; }
            public string MA_KHUYENMAI { get; set; }
            public string MAHANG { get; set; }
            public string TENHANG { get; set; }
            public string MADONVITINH { get; set; }
            public decimal GIABANLE_VAT { get; set; }
            public decimal SOLUONG { get; set; }
            public decimal? GIATRI_KHUYENMAI { get; set; }
            public int? INDEX { get; set; }

        }

        public class ParamApproval
        {
            public string ID { get; set; }
        }
    }
}
