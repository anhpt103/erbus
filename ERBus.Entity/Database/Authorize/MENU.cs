using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Authorize
{
    [Table("MENU")]
    public class MENU : DataInfoEntity
    {
        [Column("MENU_CHA")]
        [Description("MÃ MENU CHA")]
        [StringLength(50)]
        public string MENU_CHA { get; set; }

        [Column("MA_MENU")]
        [Description("MÃ MENU")]
        [StringLength(50)]
        public string MA_MENU { get; set; }

        [Column("TIEUDE")]
        [Description("TIÊU ĐỀ MENU")]
        [StringLength(200)]
        public string TIEUDE { get; set; }
      
        [Column("SAPXEP")]
        [Description("SẮP XẾP")]
        public int SAPXEP { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }

        [Column("ICON")]
        [Description("ICON FONT AWESOME")]
        [StringLength(50)]
        public string ICON { get; set; }
    }
}
