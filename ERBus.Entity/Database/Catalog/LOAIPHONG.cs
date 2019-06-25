using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("LOAIPHONG")]
    public class LOAIPHONG : DataInfoEntity
    {
        [Required]
        [Column("MALOAIPHONG")]
        [Description("MÃ LOẠI PHÒNG")]
        [StringLength(50)]
        public string MALOAIPHONG { get; set; }

        [Required]
        [Column("TENLOAIPHONG")]
        [Description("TÊN LOẠI PHÒNG")]
        [StringLength(100)]
        public string TENLOAIPHONG { get; set; }

        [Column("MABOHANG")]
        [Description("MÃ BÓ HÀNG MẶC ĐỊNH CHO PHÒNG")]
        [StringLength(50)]
        public string MABOHANG { get; set; }

        [Column("BACKGROUND")]
        [Description("BACKGROUND ĐẠI DIỆN")]
        public byte[] BACKGROUND { get; set; }

        [Column("ICON")]
        [Description("ICON ĐẠI DIỆN")]
        public byte[] ICON { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
