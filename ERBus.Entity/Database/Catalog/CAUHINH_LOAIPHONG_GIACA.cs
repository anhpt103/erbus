using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Catalog
{
    [Table("CAUHINH_LOAIPHONG_GIACA")]
    public class CAUHINH_LOAIPHONG_GIACA : DataInfoEntity
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
        [Column("GIABANLE_VAT")]
        [Description("GIÁ BÁN LẺ CÓ VAT")]
        public decimal GIABANLE_VAT { get; set; }
    }
}
