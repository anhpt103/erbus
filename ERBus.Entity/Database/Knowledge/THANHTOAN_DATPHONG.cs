using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Knowledge
{
    [Table("THANHTOAN_DATPHONG")]
    public class THANHTOAN_DATPHONG : DataInfoEntity
    {
        [Required]
        [Column("MA_DATPHONG")]
        [Description("MÃ ĐẶT PHÒNG")]
        [StringLength(70)]
        public string MA_DATPHONG { get; set; }

        [Required]
        [Column("MAPHONG")]
        [Description("MÃ PHÒNG")]
        [StringLength(50)]
        public string MAPHONG { get; set; }

        [Required]
        [Column("NGAY_DATPHONG")]
        [Description("NGÀY ĐẶT PHÒNG")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime NGAY_DATPHONG { get; set; }

        [Required]
        [Column("THOIGIAN_DATPHONG")]
        [Description("THỜI GIAN ĐẶT PHÒNG")]
        [StringLength(12)]
        public string THOIGIAN_DATPHONG { get; set; }

        [Required]
        [Column("NGAY_THANHTOAN")]
        [Description("NGÀY THANH TOÁN")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime NGAY_THANHTOAN { get; set; }

        [Required]
        [Column("THOIGIAN_THANHTOAN")]
        [Description("THỜI GIAN THANH TOÁN")]
        [StringLength(12)]
        public string THOIGIAN_THANHTOAN { get; set; }

        [Required]
        [Column("THOIGIAN_SUDUNG")]
        [Description("THỜI GIAN SỬ DỤNG DỊCH VỤ")]
        public string THOIGIAN_SUDUNG { get; set; }

        [Column("DONVI_THOIGIAN_TINHTIEN")]
        [Description("ĐƠN VỊ SỐ PHÚT TÍNH TIỀN")]
        public int? DONVI_THOIGIAN_TINHTIEN { get; set; }

        [Column("TEN_KHACHHANG")]
        [Description("TÊN KHÁCH HÀNG")]
        [StringLength(100)]
        public string TEN_KHACHHANG { get; set; }

        [Column("DIENTHOAI")]
        [Description("SỐ ĐIỆN THOẠI KHÁCH HÀNG")]
        [StringLength(20)]
        public string DIENTHOAI { get; set; }

        [Column("CANCUOC_CONGDAN")]
        [Description("MÃ CĂN CƯỚC CÔNG DÂN")]
        [StringLength(20)]
        public string CANCUOC_CONGDAN { get; set; }

        [Column("DIENGIAI")]
        [Description("DIỄN GIẢI THÊM THÔNG TIN")]
        [StringLength(200)]
        public string DIENGIAI { get; set; }

        [Column("MABOHANG")]
        [Description("MÃ BÓ HÀNG MẶC ĐỊNH CHO PHÒNG")]
        [StringLength(50)]
        public string MABOHANG { get; set; }

        [Required]
        [Column("MAKHO")]
        [Description("MÃ KHO HÀNG")]
        [StringLength(50)]
        public string MAKHO { get; set; }
    }
}
