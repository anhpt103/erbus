using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("CAUHINH_LOAIPHONG")]
    public class CAUHINH_LOAIPHONG : DataInfoEntity
    {
        [Required]
        [Column("MALOAIPHONG")]
        [Description("MÃ LOẠI PHÒNG")]
        [StringLength(50)]
        public string MALOAIPHONG { get; set; }

        [Required]
        [Column("MAHANG")]
        [Description("MÃ HÀNG TRONG BÓ HÀNG THUỘC LOẠI PHÒNG")]
        [StringLength(50)]
        public string MAHANG { get; set; }

        [Required]
        [Column("SOPHUT")]
        [Description("SỐ PHÚT TÍNH TIỀN")]
        public int SOPHUT { get; set; }
    }
}
