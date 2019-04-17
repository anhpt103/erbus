using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Authorize
{
    [Table("THAMSOHETHONG")]
    public class THAMSOHETHONG : DataInfoEntity
    {
        [Required]
        [Column("MA_THAMSO")]
        [Description("MÃ THAM SỐ")]
        [StringLength(50)]
        public string MA_THAMSO { get; set; }

        [Required]
        [Column("TEN_THAMSO")]
        [Description("TÊN THAM SỐ")]
        [StringLength(200)]
        public string TEN_THAMSO { get; set; }

        [DefaultValue(0)]
        [Column("GIATRI_SO")]
        [Description("GIÁ TRỊ BẰNG SỐ")]
        public int GIATRI_SO { get; set; }

        [DefaultValue(null)]
        [Column("GIATRI_CHU")]
        [Description("GIÁ TRỊ BẰNG CHỮ")]
        [StringLength(100)]
        public string GIATRI_CHU { get; set; }

        [DefaultValue(true)]
        [Column("ISDIABLED_GIATRI_CHU")]
        [Description("ẨN CỘT GIÁ TRỊ BẰNG CHỮ")]
        public bool ISDIABLED_GIATRI_CHU { get; set; }

        [DefaultValue(null)]
        [Column("PLACEHOLDER")]
        [Description("PLACEHOLDER CỘT GIÁ TRỊ BẰNG CHỮ")]
        [StringLength(150)]
        public string PLACEHOLDER { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }
    }
}
