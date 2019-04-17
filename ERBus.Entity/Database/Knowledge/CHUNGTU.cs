using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Knowledge
{
    [Table("CHUNGTU")]
    public class CHUNGTU : DataInfoEntity
    {
        [Required]
        [Column("MA_CHUNGTU")]
        [Description("MÃ CHỨNG TỪ")]
        [StringLength(30)]
        public string MA_CHUNGTU { get; set; }

        [Required]
        [Column("LOAI_CHUNGTU")]
        [Description("LOẠI CHỨNG TỪ")]
        [StringLength(10)]
        public string LOAI_CHUNGTU { get; set; }

        [Required]
        [Column("NGAY_CHUNGTU")]
        [Description("NGÀY CHỨNG TỪ")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime NGAY_CHUNGTU { get; set; }

        [Required]
        [Column("MANHACUNGCAP")]
        [Description("MÃ NHÀ CUNG CẤP")]
        [StringLength(50)]
        public string MANHACUNGCAP { get; set; }

        [Required]
        [Column("MAKHACHHANG")]
        [Description("MÃ KHÁCH HÀNG")]
        [StringLength(50)]
        public string MAKHACHHANG { get; set; }

        [Column("NGAY_DUYETPHIEU")]
        [Description("NGÀY DUYỆT PHIẾU")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_DUYETPHIEU { get; set; }

        [Required]
        [Column("THOIGIAN_TAO")]
        [Description("THỜI GIAN TẠO PHIẾU")]
        [StringLength(12)]
        public string THOIGIAN_TAO { get; set; }

        [Column("THOIGIAN_DUYET")]
        [Description("THỜI GIAN DUYỆT PHIẾU")]
        [StringLength(12)]
        public string THOIGIAN_DUYET { get; set; }

        [Column("NGAY_DIEUDONG")]
        [Description("NGÀY ĐIỀU ĐỘNG")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_DIEUDONG { get; set; }

        [Column("MAKHO_NHAP")]
        [Description("MÃ KHO NHẬP")]
        [StringLength(50)]
        public string MAKHO_NHAP { get; set; }

        [Column("MAKHO_XUAT")]
        [Description("MÃ KHO XUẤT")]
        [StringLength(50)]
        public string MAKHO_XUAT { get; set; }

        [Column("MADONVI_NHAP")]
        [Description("MÃ ĐƠN VỊ NHẬP")]
        [StringLength(10)]
        public string MADONVI_NHAP { get; set; }

        [Column("MADONVI_XUAT")]
        [Description("MÃ ĐƠN VỊ XUẤT")]
        [StringLength(10)]
        public string MADONVI_XUAT { get; set; }

        [Column("TIEN_CHIETKHAU")]
        [Description("TIỀN CHIẾT KHẤU TOÀN ĐƠN")]
        public decimal? TIEN_CHIETKHAU { get; set; }

        [Column("MATHUE_TOANDON")]
        [Description("MÃ THUẾ TOÀN ĐƠN")]
        [StringLength(50)]
        public string MATHUE_TOANDON { get; set; }

        [Column("LENH_DIEUDONG")]
        [Description("LỆNH ĐIỀU ĐỘNG TỪ PHIẾU")]
        [StringLength(50)]
        public string LENH_DIEUDONG { get; set; }

        [Column("MA_LYDO")]
        [Description("MÃ LÝ DO")]
        [StringLength(10)]
        public string MA_LYDO { get; set; }

        [Column("DIENGIAI")]
        [Description("DIỄN GIẢI")]
        [StringLength(500)]
        public string DIENGIAI { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
