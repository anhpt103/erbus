using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("KEHANG")]
    public class KEHANG : DataInfoEntity
    {
        [Required]
        [Column("MAKEHANG")]
        [Description("MÃ KỆ HÀNG")]
        [StringLength(50)]
        public string MAKEHANG { get; set; }

        [Required]
        [Column("TENKEHANG")]
        [Description("TÊN KỆ HÀNG")]
        [StringLength(300)]
        public string TENKEHANG { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
