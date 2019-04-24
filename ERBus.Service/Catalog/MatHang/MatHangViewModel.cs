using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
namespace ERBus.Service.Catalog.MatHang
{
    public class MatHangViewModel
    {
        public class Search : IDataSearch
        {
            public string MAHANG { get; set; }
            public string TENHANG { get; set; }
            public string MANHACUNGCAP { get; set; }
            public string MALOAI { get; set; }
            public string MANHOM { get; set; }
            public string MATHUE_VAO { get; set; }
            public string MATHUE_RA { get; set; }
            public string MADONVITINH { get; set; }
            public string MAKEHANG { get; set; }
            public string MABAOBI { get; set; }
            public string BARCODE { get; set; }
            public string ITEMCODE { get; set; }
            public string DUONGDAN { get; set; }
            public byte[] AVATAR { get; set; }
            public string IMAGE { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }
            public string TABLE_NAME { get; set; }
            public string MAKHO { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new MATHANG().MAHANG);
                }
            }
            public void LoadGeneralParam(string summary)
            {
                MAHANG = summary;
                TENHANG = summary;
            }
            public List<BuildQuery.IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new MATHANG();

                if (!string.IsNullOrEmpty(this.MAHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MAHANG),
                        Value = this.MAHANG,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TENHANG))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TENHANG),
                        Value = this.TENHANG,
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
        public class InfoUpload
        {
            public string FILENAME { get; set; }
            public string DUONGDAN { get; set; }
        }
        public class Dto : DataInfoEntityDto
        {
            public string MAHANG { get; set; }
            public string TENHANG { get; set; }
            public string MANHACUNGCAP { get; set; }
            public string MALOAI { get; set; }
            public string MANHOM { get; set; }
            public string MATHUE_VAO { get; set; }
            public string MATHUE_RA { get; set; }
            public string MADONVITINH { get; set; }
            public string MAKEHANG { get; set; }
            public string MABAOBI { get; set; }
            public string BARCODE { get; set; }
            public string ITEMCODE { get; set; }
            public string DUONGDAN { get; set; }
            public byte[] AVATAR { get; set; }
            public string IMAGE { get; set; }
            public string AVATAR_NAME { get; set; }
            public Nullable<int> TRANGTHAI { get; set; }

            public decimal GIAMUA { get; set; }
            public decimal GIAMUA_VAT { get; set; }
            public decimal GIABANLE { get; set; }
            public decimal GIABANLE_VAT { get; set; }
            public decimal GIABANBUON { get; set; }
            public decimal GIABANBUON_VAT { get; set; }
            public decimal TYLE_LAILE { get; set; }
            public decimal TYLE_LAIBUON { get; set; }
        }
        public class PARAM_NHAPMUA_OBJ
        {
            public string MAHANG { get; set; }
            public string MAKHO_NHAP { get; set; }
            public string MAKHO_XUAT { get; set; }
            public string TABLE_NAME { get; set; }
            public string UNITCODE { get; set; }
        }

        public class PARAM_SEARCH_OBJ
        {
            public string KEYSEARCH { get; set; }
            public string UNITCODE { get; set; }
        }

        public class VIEW_MODEL
        {
            public string ID { get; set; }
            public string MAHANG { get; set; }
            public string TENHANG { get; set; }
            public string MALOAI { get; set; }
            public string MANHOM { get; set; }
            public string MANHACUNGCAP { get; set; }
            public string MATHUE_VAO { get; set; }
            public string MATHUE_RA { get; set; }
            public string MADONVITINH { get; set; }
            public string BARCODE { get; set; }
            public decimal GIAMUA { get; set; }
            public decimal GIAMUA_VAT { get; set; }
            public decimal GIABANLE { get; set; }
            public decimal GIABANLE_VAT { get; set; }
            public decimal GIABANBUON { get; set; }
            public decimal GIABANBUON_VAT { get; set; }
            public decimal TYLE_LAILE { get; set; }
            public decimal TYLE_LAIBUON { get; set; }
            public decimal GIAVON { get; set; }
            public decimal TONCUOIKYSL { get; set; }
            public int TRANGTHAI { get; set; }
            public string UNITCODE { get; set; }
        }
    }
}
