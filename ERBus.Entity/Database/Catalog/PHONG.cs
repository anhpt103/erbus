using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("PHONG")]
    public class PHONG : DataInfoEntity
    {
        [Required]
        [Column("MAPHONG")]
        [Description("MÃ PHÒNG")]
        [StringLength(50)]
        public string MAPHONG { get; set; }

        [Required]
        [Column("TENPHONG")]
        [Description("TÊN PHÒNG")]
        [StringLength(300)]
        public string TENPHONG { get; set; }

        [Column("TANG")]
        [Description("TẦNG")]
        public int? TANG { get; set; }

        [Column("VITRI")]
        [Description("VỊ TRÍ")]
        [StringLength(200)]
        public string VITRI { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
