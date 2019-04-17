using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("NHACUNGCAP")]
    public class NHACUNGCAP : DataInfoEntity
    {
        [Required]
        [Column("MANHACUNGCAP")]
        [Description("MÃ NHÀ CUNG CẤP")]
        [StringLength(50)]
        public string MANHACUNGCAP { get; set; }

        [Required]
        [Column("TENNHACUNGCAP")]
        [Description("TÊN NHÀ CUNG CẤP")]
        [StringLength(300)]
        public string TENNHACUNGCAP { get; set; }

        [Column("DIACHI")]
        [Description("ĐỊA CHỈ NHÀ CUNG CẤP")]
        [StringLength(300)]
        public string DIACHI { get; set; }

        [Column("MASOTHUE")]
        [Description("MÃ SỐ THUẾ NHÀ CUNG CẤP")]
        [StringLength(80)]
        public string MASOTHUE { get; set; }

        [Column("DIENTHOAI")]
        [Description("SỐ ĐIỆN THOẠI NHÀ CUNG CẤP")]
        [StringLength(20)]
        public string DIENTHOAI { get; set; }

        [Column("DIENGIAI")]
        [Description("DIỄN GIẢI THÔNG TIN NHÀ CUNG CẤP")]
        [StringLength(300)]
        public string DIENGIAI { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
