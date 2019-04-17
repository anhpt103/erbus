using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity
{
    public class DetailInfoEntity : EntityBase
    {
        [Key]
        [Column("ID")]
        [Description("KHÓA CHÍNH")]
        [StringLength(50)]
        public string ID { get; set; }
    }
}
