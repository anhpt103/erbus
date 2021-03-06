﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Knowledge
{
    [Table("DATPHONG")]
    public class DATPHONG : DataInfoEntity
    {
        [Required]
        [Column("MA_DATPHONG")]
        [Description("MÃ ĐẶT PHÒNG")]
        [StringLength(70)]
        public string MA_DATPHONG { get; set; }

        [Required]
        [Column("MAPHONG")]
        [Description("MÃ PHÒNG")]
        [StringLength(50)]
        public string MAPHONG { get; set; }

        [Required]
        [Column("NGAY_DATPHONG")]
        [Description("NGÀY ĐẶT PHÒNG")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime NGAY_DATPHONG { get; set; }

        [Required]
        [Column("THOIGIAN_DATPHONG")]
        [Description("THỜI GIAN ĐẶT PHÒNG")]
        [StringLength(12)]
        public string THOIGIAN_DATPHONG { get; set; }

        [Column("TEN_KHACHHANG")]
        [Description("TÊN KHÁCH HÀNG")]
        [StringLength(100)]
        public string TEN_KHACHHANG { get; set; }

        [Column("DIENTHOAI")]
        [Description("SỐ ĐIỆN THOẠI KHÁCH HÀNG")]
        [StringLength(20)]
        public string DIENTHOAI { get; set; }

        [Column("CANCUOC_CONGDAN")]
        [Description("MÃ CĂN CƯỚC CÔNG DÂN")]
        [StringLength(20)]
        public string CANCUOC_CONGDAN { get; set; }

        [Column("DIENGIAI")]
        [Description("DIỄN GIẢI THÊM THÔNG TIN")]
        [StringLength(200)]
        public string DIENGIAI { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
