using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("KHACHHANG")]
    public class KHACHHANG : DataInfoEntity
    {
        [Required]
        [Column("MAKHACHHANG")]
        [Description("MÃ KHÁCH HÀNG")]
        [StringLength(50)]
        public string MAKHACHHANG { get; set; }

        [Required]
        [Column("TENKHACHHANG")]
        [Description("TÊN KHÁCH HÀNG")]
        [StringLength(300)]
        public string TENKHACHHANG { get; set; }

        [Column("DIACHI")]
        [Description("ĐỊA CHỈ KHÁCH HÀNG")]
        [StringLength(300)]
        public string DIACHI { get; set; }

        [Column("DIENTHOAI")]
        [Description("SỐ ĐIỆN THOẠI KHÁCH HÀNG")]
        [StringLength(20)]
        public string DIENTHOAI { get; set; }

        [Column("CANCUOC_CONGDAN")]
        [Description("MÃ CĂN CƯỚC CÔNG DÂN")]
        [StringLength(20)]
        public string CANCUOC_CONGDAN { get; set; }

        [Column("NGAYSINH")]
        [Description("NGÀY SINH")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime? NGAYSINH { get; set; }

        [Column("NGAYDACBIET")]
        [Description("NGÀY ĐẶC BIỆT")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime? NGAYDACBIET { get; set; }

        [Column("MATHE")]
        [Description("MÃ THẺ KHÁCH HÀNG")]
        [StringLength(20)]
        public string MATHE { get; set; }

        [Column("SODIEM")]
        [Description("SỐ ĐIỂM KHÁCH HÀNG")]
        public decimal SODIEM { get; set; }

        [Column("TONGTIEN")]
        [Description("SỐ TIỀN KHÁCH HÀNG ĐÃ MUA")]
        public decimal TONGTIEN { get; set; }

        [Column("DIENGIAI")]
        [Description("DIỄN GIẢI THÔNG TIN KHÁCH HÀNG")]
        [StringLength(300)]
        public string DIENGIAI { get; set; }

        [Column("MAHANG")]
        [Description("MÃ HẠNG KHÁCH HÀNG")]
        [StringLength(50)]
        public string MAHANG { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
