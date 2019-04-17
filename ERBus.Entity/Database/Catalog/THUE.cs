using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("THUE")]
    public class THUE : DataInfoEntity
    {
        [Required]
        [Column("MATHUE")]
        [Description("MÃ THUẾ")]
        [StringLength(50)]
        public string MATHUE { get; set; }

        [Required]
        [Column("TENTHUE")]
        [Description("TÊN THUẾ")]
        [StringLength(300)]
        public string TENTHUE { get; set; }

        [Required]
        [Column("GIATRI")]
        [Description("GIÁ TRỊ THUẾ")]
        public decimal GIATRI { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
