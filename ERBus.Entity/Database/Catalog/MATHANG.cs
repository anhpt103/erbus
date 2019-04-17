using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("MATHANG")]
    public class MATHANG : DataInfoEntity
    {
        [Required]
        [Column("MAHANG")]
        [Description("MÃ HÀNG HÓA")]
        [StringLength(50)]
        public string MAHANG { get; set; }

        [Required]
        [Column("TENHANG")]
        [Description("TÊN HÀNG HÓA")]
        [StringLength(300)]
        public string TENHANG { get; set; }

        [Required]
        [Column("MANHACUNGCAP")]
        [Description("MÃ NHÀ CUNG CẤP")]
        [StringLength(50)]
        public string MANHACUNGCAP { get; set; }

        [Required]
        [Column("MALOAI")]
        [Description("MÃ LOẠI")]
        [StringLength(50)]
        public string MALOAI { get; set; }

        [Required]
        [Column("MANHOM")]
        [Description("MÃ NHÓM")]
        [StringLength(50)]
        public string MANHOM { get; set; }

        [Required]
        [Column("MATHUE_VAO")]
        [Description("MÃ THUẾ VÀO")]
        [StringLength(50)]
        public string MATHUE_VAO { get; set; }

        [Required]
        [Column("MATHUE_RA")]
        [Description("MÃ THUẾ RA")]
        [StringLength(50)]
        public string MATHUE_RA { get; set; }

        [Column("MADONVITINH")]
        [Description("MÃ ĐƠN VỊ TÍNH")]
        [StringLength(50)]
        public string MADONVITINH { get; set; }

        [Column("MAKEHANG")]
        [Description("MÃ KỆ HÀNG")]
        [StringLength(50)]
        public string MAKEHANG { get; set; }

        [Column("MABAOBI")]
        [Description("MÃ BAO BÌ")]
        [StringLength(50)]
        public string MABAOBI { get; set; }

        [Column("BARCODE")]
        [Description("MÃ BARCODE")]
        [StringLength(2000)]
        public string BARCODE { get; set; }

        [Column("ITEMCODE")]
        [Description("MÃ CÂN ITEMCODE")]
        [StringLength(10)]
        public string ITEMCODE { get; set; }

        [Column("DUONGDAN")]
        [StringLength(300)]
        [Description("ĐƯỜNG DẪN TỚI FOLDER ẢNH")]
        public string DUONGDAN { get; set; }

        [Column("AVATAR")]
        [Description("AVATAR _ ẢNH ĐẠI DIỆN")]
        public byte[] AVATAR { get; set; }

        [Column("IMAGE")]
        [StringLength(2000)]
        [Description("IMAGE _ TÊN ẢNH _ (CÓ THỂ NHIỀU ẢNH)")]
        public string IMAGE { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
