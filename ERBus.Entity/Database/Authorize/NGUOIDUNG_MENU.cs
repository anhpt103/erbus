using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Authorize
{
    [Table("NGUOIDUNG_MENU")]
    public class NGUOIDUNG_MENU : EntityBase
    {
        [Key]
        [Column("ID")]
        [Description("KHÓA CHÍNH")]
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [Column("MA_MENU")]
        [Description("MÃ MENU")]
        [StringLength(50)]
        public string MA_MENU { get; set; }

        [Required]
        [Column("USERNAME")]
        [Description("TÀI KHOẢN NGƯỜI DÙNG")]
        [StringLength(50)]
        public string USERNAME { get; set; }

        [Required]
        [Column("XEM")]
        [Description("QUYỀN XEM")]
        public bool XEM { get; set; }

        [Required]
        [Column("THEM")]
        [Description("QUYỀN THÊM")]
        public bool THEM { get; set; }

        [Required]
        [Column("SUA")]
        [Description("QUYỀN SỬA")]
        public bool SUA { get; set; }

        [Required]
        [Column("XOA")]
        [Description("QUYỀN XÓA")]
        public bool XOA { get; set; }

        [Required]
        [Column("DUYET")]
        [Description("QUYỀN DUYỆT")]
        public bool DUYET { get; set; }

        [Required]
        [Column("GIAMUA")]
        [Description("QUYỀN THAO TÁC VỚI GIÁ MUA")]
        public bool GIAMUA { get; set; }

        [Required]
        [Column("GIABAN")]
        [Description("QUYỀN THAO TÁC VỚI GIÁ BÁN BAO GỒM GIÁ BÁN LẺ CHƯA VAT VÀ CÓ VAT")]
        public bool GIABAN { get; set; }

        [Required]
        [Column("GIAVON")]
        [Description("QUYỀN THAO TÁC VỚI GIÁ VỐN BAO GỒM GIÁ VỐN CHƯA VAT VÀ CÓ VAT")]
        public bool GIAVON { get; set; }

        [Required]
        [Column("TYLELAI")]
        [Description("QUYỀN THAO TÁC VỚI TỶ LỆ LÃI")]
        public bool TYLELAI { get; set; }

        [Required]
        [Column("BANCHIETKHAU")]
        [Description("QUYỀN ĐƯỢC BÁN CHIẾT KHẤU")]
        public bool BANCHIETKHAU { get; set; }

        [Required]
        [Column("BANBUON")]
        [Description("QUYỀN ĐƯỢC BÁN BUÔN")]
        public bool BANBUON { get; set; }

        [Required]
        [Column("BANTRALAI")]
        [Description("QUYỀN ĐƯỢC BÁN TRẢ LẠI")]
        public bool BANTRALAI { get; set; }

        [Column("UNITCODE")]
        [Description("MÃ ĐƠN VỊ SỬ DỤNG")]
        [StringLength(10)]
        public string UNITCODE { get; set; }
    }
}
