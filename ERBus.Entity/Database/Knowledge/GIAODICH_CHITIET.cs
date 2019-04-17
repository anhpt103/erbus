using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Knowledge
{
    [Table("GIAODICH_CHITIET")]
    public class GIAODICH_CHITIET : EntityBase
    {
        [Key]
        [Column("ID")]
        [Description("KHÓA CHÍNH")]
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [Column("MA_GIAODICH")]
        [Description("MÃ GIAO DỊCH")]
        [StringLength(70)]
        public string MA_GIAODICH { get; set; }

        [Required]
        [Column("MAHANG")]
        [Description("MÃ HÀNG HÓA _ KHÓA NGOẠI VỚI BẢNG MATHANG")]
        [StringLength(50)]
        public string MAHANG { get; set; }

        [Required]
        [Column("MATHUE_RA")]
        [Description("MÃ THUẾ RA HÀNG HÓA TẠI THỜI ĐIỂM")]
        [StringLength(50)]
        public string MATHUE_RA { get; set; }

        [Required]
        [Column("SOLUONG")]
        [Description("SỐ LƯỢNG")]
        public decimal SOLUONG { get; set; }

        [Required]
        [Column("GIABANLE_VAT")]
        [Description("GIÁ BÁN LẺ CÓ VAT TẠI THỜI ĐIỂM")]
        public decimal GIABANLE_VAT { get; set; }

        [Column("MA_KHUYENMAI")]
        [Description("MA_KHUYENMAI")]
        [StringLength(30)]
        public string MA_KHUYENMAI { get; set; }

        [DefaultValue(0)]
        [Column("TYLE_KHUYENMAI")]
        [Description("TỶ LỆ KHUYẾN MÃI")]
        public decimal TYLE_KHUYENMAI { get; set; }

        [DefaultValue(0)]
        [Column("TIEN_KHUYENMAI")]
        [Description("TIỀN KHUYẾN MÃI")]
        public decimal TIEN_KHUYENMAI { get; set; }

        [DefaultValue(0)]
        [Column("TYLE_CHIETKHAU")]
        [Description("TỶ LỆ CHIẾT KHẤU")]
        public decimal TYLE_CHIETKHAU { get; set; }

        [DefaultValue(0)]
        [Column("TIEN_CHIETKHAU")]
        [Description("TIỀN CHIẾT KHẤU")]
        public decimal TIEN_CHIETKHAU { get; set; }

        [DefaultValue(0)]
        [Column("TIENTHE_VIP")]
        [Description("TIỀN THẺ VIP")]
        public decimal TIENTHE_VIP { get; set; }

        [DefaultValue(0)]
        [Column("TIEN_VOUCHER")]
        [Description("TIỀN PHIẾU VOUCHER")]
        public decimal TIEN_VOUCHER { get; set; }

        [Required]
        [Column("THANHTIEN")]
        [Description("THÀNH TIỀN")]
        public decimal THANHTIEN { get; set; }

        [Column("SAPXEP")]
        public int? SAPXEP { get; set; }
    }
}
