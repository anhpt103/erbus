using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("BOHANG")]
    public class BOHANG : DataInfoEntity
    {
        [Required]
        [Column("MABOHANG")]
        [Description("MÃ BÓ HÀNG")]
        [StringLength(50)]
        public string MABOHANG { get; set; }

        [Required]
        [Column("TENBOHANG")]
        [Description("TÊN BÓ HÀNG")]
        [StringLength(300)]
        public string TENBOHANG { get; set; }

        [Column("DIENGIAI")]
        [Description("DIỄN GIẢI BÓ HÀNG")]
        [StringLength(300)]
        public string DIENGIAI { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
