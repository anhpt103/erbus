using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Authorize
{
    [Table("CUAHANG")]
    public class CUAHANG : DataInfoEntity
    {
        [Required]
        [Column("MA_CUAHANG")]
        [Description("MÃ CỬA HÀNG")]
        [StringLength(50)]
        public string MA_CUAHANG { get; set; }

        [Required]
        [Column("TEN_CUAHANG")]
        [Description("TÊN CỬA HÀNG")]
        [StringLength(200)]
        public string TEN_CUAHANG { get; set; }

        [Column("SODIENTHOAI")]
        [Description("SỐ ĐIỆN THOẠI")]
        [StringLength(16)]
        public string SODIENTHOAI { get; set; }

        [Column("DIACHI")]
        [Description("ĐỊA CHỈ CỬA HÀNG")]
        [StringLength(200)]
        public string DIACHI { get; set; }

        [Column("MA_CUAHANG_CHA")]
        [Description("MÃ CỬA HÀNG CHA")]
        [StringLength(50)]
        public string MA_CUAHANG_CHA { get; set; }
    }
}
