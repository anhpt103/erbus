using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("HANGKHACHHANG")]
    public class HANGKHACHHANG : DataInfoEntity
    {
        [Required]
        [Column("MAHANG")]
        [Description("MÃ HẠNG KHÁCH HÀNG")]
        [StringLength(50)]
        public string MAHANG { get; set; }

        [Required]
        [Column("TENHANG")]
        [Description("TÊN HẠNG KHÁCH HÀNG")]
        [StringLength(100)]
        public string TENHANG { get; set; }

        [Column("TYLE_SINHNHAT")]
        [Description("TỶ LỆ GIẢM GIÁ SINH NHẬT")]
        public decimal TYLE_SINHNHAT { get; set; }

        [Column("TYLE_DACBIET")]
        [Description("TỶ LỆ GIẢM GIÁ NGÀY ĐẶC BIỆT")]
        public decimal TYLE_DACBIET { get; set; }

        [Column("QUYDOITIEN_THANH_DIEM")]
        [Description("QUY ĐỔI TIỀN THÀNH ĐIỂM")]
        public decimal QUYDOITIEN_THANH_DIEM { get; set; }

        [Column("QUYDOIDIEM_THANH_TIEN")]
        [Description("QUY ĐỔI ĐIỂM THÀNH TIỀN")]
        public decimal QUYDOIDIEM_THANH_TIEN { get; set; }

        [Column("HANG_KHOIDAU")]
        [Description("SETUP HẠNG KHỞI ĐẦU ĐỔI VỚI KHÁCH HÀNG CHƯA CÓ HẠNG")]
        public int? HANG_KHOIDAU { get; set; }

        [Column("SOTIEN_LENHANG")]
        [Description("SỐ TIỀN ĐẠT ĐƯỢC ĐỂ LÊN HẠNG TIẾP THEO")]
        public decimal SOTIEN_LENHANG { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
