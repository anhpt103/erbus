using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Knowledge
{
    [Table("GIAODICH")]
    public class GIAODICH : DataInfoEntity
    {
        [Required]
        [Column("MA_GIAODICH")]
        [Description("MÃ GIAO DỊCH")]
        [StringLength(70)]
        public string MA_GIAODICH { get; set; }

        [Required]
        [Column("LOAI_GIAODICH")]
        [Description("LOẠI GIAO DỊCH - BÁN LẺ - BÁN TRẢ LẠI")]
        [StringLength(15)]
        public string LOAI_GIAODICH { get; set; }

        [Required]
        [Column("NGAY_GIAODICH")]
        [Description("NGÀY GIAO DỊCH")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime NGAY_GIAODICH { get; set; }

        [Required]
        [Column("MAKHACHHANG")]
        [Description("MÃ KHÁCH HÀNG")]
        [StringLength(50)]
        public string MAKHACHHANG { get; set; }

        [Required]
        [Column("THOIGIAN_TAO")]
        [Description("THỜI GIAN GIAO DỊCH")]
        [StringLength(12)]
        public string THOIGIAN_TAO { get; set; }

        [Required]
        [Column("TIENKHACH_TRA")]
        [Description("TIỀN KHÁCH TRẢ")]
        public decimal TIENKHACH_TRA { get; set; }

        [DefaultValue(0)]
        [Column("TIEN_TRALAI_KHACH")]
        [Description("TIỀN TRẢ LẠI KHÁCH")]
        public decimal TIEN_TRALAI_KHACH { get; set; }

        [Required]
        [Column("MAKHO_XUAT")]
        [Description("MÃ KHO XUẤT")]
        [StringLength(50)]
        public string MAKHO_XUAT { get; set; }

        [Column("MA_VOUCHER")]
        [Description("MÃ PHIẾU VOUCHER")]
        [StringLength(50)]
        public string MA_VOUCHER { get; set; }

        [Column("DIENGIAI")]
        [Description("DIỄN GIẢI")]
        [StringLength(300)]
        public string DIENGIAI { get; set; }

    }
}
