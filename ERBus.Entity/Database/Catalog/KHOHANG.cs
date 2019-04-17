using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("KHOHANG")]
    public class KHOHANG : DataInfoEntity
    {
        [Required]
        [Column("MAKHO")]
        [Description("MÃ KHO HÀNG")]
        [StringLength(50)]
        public string MAKHO { get; set; }

        [Required]
        [Column("TENKHO")]
        [Description("TÊN KHO HÀNG")]
        [StringLength(150)]
        public string TENKHO { get; set; }

        [Column("DIENGIAI")]
        [Description("DIỄN GIẢI KHO HÀNG")]
        [StringLength(300)]
        public string DIENGIAI { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
