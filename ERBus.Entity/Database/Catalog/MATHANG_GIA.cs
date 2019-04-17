using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("MATHANG_GIA")]
    public class MATHANG_GIA : DataInfoEntity
    {
        [Required]
        [Column("MAHANG")]
        [Description("MÃ HÀNG HÓA _ KHÓA NGOẠI VỚI BẢNG MATHANG")]
        [StringLength(50)]
        public string MAHANG { get; set; }

        //GIÁ MUA

        [Required]
        [Column("GIAMUA")]
        [Description("GIÁ MUA CHƯA VAT")]
        public decimal GIAMUA { get; set; }

        [Required]
        [Column("GIAMUA_VAT")]
        [Description("GIÁ MUA CÓ VAT")]
        public decimal GIAMUA_VAT { get; set; }

        //GIÁ BÁN LẺ

        [Required]
        [Column("GIABANLE")]
        [Description("GIÁ BÁN LẺ CHƯA VAT")]
        public decimal GIABANLE { get; set; }

        [Required]
        [Column("GIABANLE_VAT")]
        [Description("GIÁ BÁN LẺ CÓ VAT")]
        public decimal GIABANLE_VAT { get; set; }

        //GIÁ BÁN BUÔN

        [Required]
        [Column("GIABANBUON")]
        [Description("GIÁ BÁN BUÔN CHƯA VAT")]
        public decimal GIABANBUON { get; set; }

        [Required]
        [Column("GIABANBUON_VAT")]
        [Description("GIÁ BÁN BUÔN CÓ VAT")]
        public decimal GIABANBUON_VAT { get; set; }

        //TỶ LỆ LÃI

        [Required]
        [Column("TYLE_LAILE")]
        [Description("TỶ LỆ LÃI LẺ")]
        public decimal TYLE_LAILE { get; set; }

        [Required]
        [Column("TYLE_LAIBUON")]
        [Description("TỶ LỆ LÃI BUÔN")]
        public decimal TYLE_LAIBUON { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
