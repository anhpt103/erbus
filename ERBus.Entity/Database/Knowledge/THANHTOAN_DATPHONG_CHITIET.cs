using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Knowledge
{
    [Table("THANHTOAN_DATPHONG_CHITIET")]
    public class THANHTOAN_DATPHONG_CHITIET : EntityBase
    {
        [Key]
        [Column("ID")]
        [Description("KHÓA CHÍNH")]
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [Column("MA_DATPHONG")]
        [Description("MÃ ĐẶT PHÒNG")]
        [StringLength(70)]
        public string MA_DATPHONG { get; set; }

        [Column("MABOHANG")]
        [Description("MÃ BÓ HÀNG")]
        [StringLength(50)]
        public string MABOHANG { get; set; }

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
        [Description("GIÁ BÁN LẺ CÓ VAT TẠI THỜI ĐIỂM SAU KHI ĐÃ TRỪ CHIẾT KHẤU")]
        public decimal GIABANLE_VAT { get; set; }

        [Column("CHIETKHAU")]
        [Description("TỶ LỆ CHIẾT KHẤU MẶT HÀNG TRONG BÓ")]
        public decimal? CHIETKHAU { get; set; }

        [DefaultValue(0)]
        [Column("TYLE_CHIETKHAU")]
        [Description("TỶ LỆ CHIẾT KHẤU")]
        public decimal TYLE_CHIETKHAU { get; set; }

        [DefaultValue(0)]
        [Column("TIEN_CHIETKHAU")]
        [Description("TIỀN CHIẾT KHẤU")]
        public decimal TIEN_CHIETKHAU { get; set; }

        [Column("SAPXEP")]
        public int? SAPXEP { get; set; }

        [Required]
        [Column("UNITCODE")]
        [Description("MÃ ĐƠN VỊ")]
        [StringLength(10)]
        public string UNITCODE { get; set; }
    }
}
