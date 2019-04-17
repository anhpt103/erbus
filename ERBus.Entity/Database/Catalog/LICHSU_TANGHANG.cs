using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("LICHSU_TANGHANG")]
    public class LICHSU_TANGHANG : EntityBase
    {
        [Key]
        [Column("ID")]
        [Description("KHÓA CHÍNH")]
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [Column("MAKHACHHANG")]
        [Description("MÃ KHÁCH HÀNG")]
        [StringLength(50)]
        public string MAKHACHHANG { get; set; }

        [Required]
        [Column("MAHANG_CU")]
        [Description("MÃ HẠNG CŨ CỦA KHÁCH HÀNG")]
        [StringLength(50)]
        public string MAHANG_CU { get; set; }

        [Required]
        [Column("MAHANG_MOI")]
        [Description("MÃ HẠNG MỚI CỦA KHÁCH HÀNG")]
        [StringLength(50)]
        public string MAHANG_MOI { get; set; }

        [Column("NGAY_LENHANG")]
        [Description("NGÀY LÊN HẠNG")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime NGAY_LENHANG { get; set; }

        [Column("THOIGIAN_LENHANG")]
        [Description("THỜI GIAN LÊN HẠNG")]
        [StringLength(50)]
        public string THOIGIAN_LENHANG { get; set; }

        [Column("MA_GIAODICH_LENHANG")]
        [Description("MÃ GIAO DỊCH THANH TOÁN LÊN HẠNG")]
        [StringLength(70)]
        public string MA_GIAODICH_LENHANG { get; set; }
    }
}
