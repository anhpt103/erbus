using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("LOAIHANG")]
    public class LOAIHANG : DataInfoEntity
    {
        [Required]
        [Column("MALOAI")]
        [Description("MÃ LOẠI")]
        [StringLength(50)]
        public string MALOAI { get; set; }

        [Required]
        [Column("TENLOAI")]
        [Description("TÊN LOẠI")]
        [StringLength(300)]
        public string TENLOAI { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
