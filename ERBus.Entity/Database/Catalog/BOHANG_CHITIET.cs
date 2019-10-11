using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("BOHANG_CHITIET")]
    public class BOHANG_CHITIET : DataInfoEntity
    {
        [Required]
        [Column("MABOHANG")]
        [Description("MÃ BÓ HÀNG")]
        [StringLength(50)]
        public string MABOHANG { get; set; }

        [Required]
        [Column("MAHANG")]
        [Description("MÃ HÀNG HÓA _ LIÊN KẾT VỚI BẢNG HÀNG HÓA VẬT TƯ")]
        [StringLength(50)]
        public string MAHANG { get; set; }

        [Required]
        [Column("SOLUONG")]
        [Description("SỐ LƯỢNG MẶT HÀNG TRONG BÓ")]
        public decimal SOLUONG { get; set; }

        [Required]
        [Column("CHIETKHAU")]
        [Description("TỶ LỆ CHIẾT KHẤU MẶT HÀNG TRONG BÓ")]
        public decimal CHIETKHAU { get; set; }

        [Column("DIENGIAI")]
        [Description("DIỄN GIẢI MẶT HÀNG TRONG BÓ HÀNG")]
        [StringLength(300)]
        public string DIENGIAI { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }

        [Column("SAPXEP")]
        [Description("SẮP XẾP THỨ TỰ HÀNG HÓA TRONG BÓ")]
        public Nullable<int> SAPXEP { get; set; }
    }
}
