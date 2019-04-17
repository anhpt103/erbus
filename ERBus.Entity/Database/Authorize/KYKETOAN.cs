using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Authorize
{
    [Table("KYKETOAN")]
    public class KYKETOAN : DataInfoEntity
    {
        [Required]
        [Column("KYKETOAN")]
        [Description("KỲ KẾ TOÁN")]
        public int KY { get; set; }

        [Column("TENKY")]
        [StringLength(150)]
        [Description("TÊN KỲ KẾ TOÁN")]
        public string TENKY { get; set; }

        [Column("TUNGAY")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime TUNGAY { get; set; }

        [Column("DENNGAY")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime DENNGAY { get; set; }

        [Column("NAM")]
        public int NAM { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
