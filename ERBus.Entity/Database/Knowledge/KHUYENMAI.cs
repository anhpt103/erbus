using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Knowledge
{
    [Table("KHUYENMAI")]
    public class KHUYENMAI : DataInfoEntity
    {
        [Required]
        [Column("MA_KHUYENMAI")]
        [Description("MÃ CHƯƠNG TRÌNH KHUYẾN MÃI")]
        [StringLength(30)]
        public string MA_KHUYENMAI { get; set; }

        [Required]
        [Column("LOAI_KHUYENMAI")]
        [Description("LOẠI CHƯƠNG TRÌNH KHUYẾN MÃI")]
        [StringLength(20)]
        public string LOAI_KHUYENMAI { get; set; }

        [Required]
        [Column("TUNGAY")]
        [Description("TỪ NGÀY")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime TUNGAY { get; set; }

        [Required]
        [Column("DENNGAY")]
        [Description("ĐẾN NGÀY")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime DENNGAY { get; set; }

        [Column("TUGIO")]
        [Description("TỪ GIỜ")]
        [StringLength(10)]
        public string TUGIO { get; set; }

        [Column("DENGIO")]
        [Description("ĐẾN GIỜ")]
        [StringLength(10)]
        public string DENGIO { get; set; }

        [Required]
        [Column("THOIGIAN_TAO")]
        [Description("THỜI GIAN TẠO PHIẾU")]
        [StringLength(12)]
        public string THOIGIAN_TAO { get; set; }

        [Column("THOIGIAN_DUYET")]
        [Description("THỜI GIAN DUYỆT PHIẾU")]
        [StringLength(12)]
        public string THOIGIAN_DUYET { get; set; }

        [Column("MAKHO_KHUYENMAI")]
        [Description("MÃ KHO KHUYẾN MÃI")]
        [StringLength(50)]
        public string MAKHO_KHUYENMAI { get; set; }

        [Column("DIENGIAI")]
        [Description("DIỄN GIẢI")]
        [StringLength(500)]
        public string DIENGIAI { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
