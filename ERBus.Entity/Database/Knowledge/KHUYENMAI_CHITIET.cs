using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Knowledge
{
    [Table("KHUYENMAI_CHITIET")]
    public class KHUYENMAI_CHITIET : EntityBase
    {
        [Key]
        [Column("ID")]
        [Description("KHÓA CHÍNH")]
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [Column("MA_KHUYENMAI")]
        [Description("MÃ CHƯƠNG TRÌNH KHUYẾN MÃI")]
        [StringLength(30)]
        public string MA_KHUYENMAI { get; set; }

        [Required]
        [Column("MAHANG")]
        [Description("MÃ HÀNG HÓA _ KHÓA NGOẠI VỚI BẢNG MATHANG")]
        [StringLength(50)]
        public string MAHANG { get; set; }

        [Required]
        [Column("SOLUONG")]
        [Description("SỐ LƯỢNG")]
        public decimal SOLUONG { get; set; }

        [Column("GIATRI_KHUYENMAI")]
        [Description("GIÁ TRỊ KHUYẾN MÃI <= 100 LÀ TỶ LỆ; > 100 LÀ TIỀN")]
        public decimal? GIATRI_KHUYENMAI { get; set; }

        [Column("INDEX")]
        public int? INDEX { get; set; }
    }
}
