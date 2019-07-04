using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Authorize
{
    [Table("NHOMQUYEN")]
    public class NHOMQUYEN : DataInfoEntity
    {
        [Required]
        [Column("MANHOMQUYEN")]
        [Description("MÃ NHÓM QUYỀN")]
        [StringLength(10)]
        public string MANHOMQUYEN { get; set; }

        [Required]
        [Column("TENNHOMQUYEN")]
        [Description("TÊN NHÓM QUYỀN")]
        [StringLength(200)]
        public string TENNHOMQUYEN { get; set; }

        [Column("DIENGIAI")]
        [Description("DIỄN GIẢI NHÓM QUYỀN")]
        [StringLength(300)]
        public string DIENGIAI { get; set; }

    }
}
