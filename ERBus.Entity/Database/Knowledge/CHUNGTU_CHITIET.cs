using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Knowledge
{
    [Table("CHUNGTU_CHITIET")]
    public class CHUNGTU_CHITIET : EntityBase
    {
        [Key]
        [Column("ID")]
        [Description("KHÓA CHÍNH")]
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [Column("MA_CHUNGTU")]
        [Description("MÃ CHỨNG TỪ")]
        [StringLength(30)]
        public string MA_CHUNGTU { get; set; }

        [Required]
        [Column("MAHANG")]
        [Description("MÃ HÀNG HÓA _ KHÓA NGOẠI VỚI BẢNG MATHANG")]
        [StringLength(50)]
        public string MAHANG { get; set; }

        [Required]
        [Column("MATHUE_VAO")]
        [Description("MÃ THUẾ VÀO HÀNG HÓA TẠI THỜI ĐIỂM")]
        [StringLength(50)]
        public string MATHUE_VAO { get; set; }

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
        [Column("GIAMUA")]
        [Description("GIÁ MUA CHƯA VAT TẠI THỜI ĐIỂM")]
        public decimal GIAMUA { get; set; }

        [Required]
        [Column("GIAMUA_VAT")]
        [Description("GIÁ MUA CÓ VAT TẠI THỜI ĐIỂM")]
        public decimal GIAMUA_VAT { get; set; }

        [Column("TIEN_GIAMGIA")]
        [Description("TIỀN GIẢM GIÁ _ ĐỐI VỚI NGHIỆP VỤ XUẤT BÁN BUÔN")]
        public decimal? TIEN_GIAMGIA { get; set; }

        [Required]
        [Column("THANHTIEN")]
        [Description("THÀNH TIỀN")]
        public decimal THANHTIEN { get; set; }

        [Column("INDEX")]
        public int? INDEX { get; set; }
    }
}
