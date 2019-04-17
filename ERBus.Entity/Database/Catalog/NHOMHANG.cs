using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("NHOMHANG")]
    public class NHOMHANG : DataInfoEntity
    {
        [Required]
        [Column("MANHOM")]
        [Description("MÃ NHÓM")]
        [StringLength(50)]
        public string MANHOM { get; set; }

        [Required]
        [Column("TENNHOM")]
        [Description("TÊN NHÓM")]
        [StringLength(300)]
        public string TENNHOM { get; set; }

        [Required]
        [Column("MALOAI")]
        [Description("KHÓA NGOẠI ĐẾN BẢNG DANH MỤC LOẠI")]
        [StringLength(50)]
        public string MALOAI { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
