using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("DONVITINH")]
    public class DONVITINH : DataInfoEntity
    {
        [Required]
        [Column("MADONVITINH")]
        [Description("MÃ ĐƠN VỊ TÍNH")]
        [StringLength(50)]
        public string MADONVITINH { get; set; }

        [Required]
        [Column("TENDONVITINH")]
        [Description("TÊN ĐƠN VỊ TÍNH")]
        [StringLength(300)]
        public string TENDONVITINH { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
