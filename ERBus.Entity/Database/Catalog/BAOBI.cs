using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("BAOBI")]
    public class BAOBI : DataInfoEntity
    {
        [Required]
        [Column("MABAOBI")]
        [Description("MÃ BAO BÌ")]
        [StringLength(50)]
        public string MABAOBI { get; set; }

        [Required]
        [Column("TENBAOBI")]
        [Description("TÊN BAO BÌ")]
        [StringLength(300)]
        public string TENBAOBI { get; set; }

        [Required]
        [Column("SOLUONG")]
        [Description("ĐƠN VỊ SỐ LƯỢNG / BAO")]
        public decimal SOLUONG { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
