using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Authorize
{
    [Table("NGUOIDUNG_NHOMQUYEN")]
    public class NGUOIDUNG_NHOMQUYEN : EntityBase
    {
        [Key]
        [Column("ID")]
        [Description("KHÓA CHÍNH")]
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [Column("USERNAME")]
        [Description("TÀI KHOẢN NGƯỜI DÙNG")]
        [StringLength(50)]
        public string USERNAME { get; set; }

        [Column("MANHOMQUYEN")]
        [Description("MÃ NHÓM QUYỀN")]
        [StringLength(10)]
        public string MANHOMQUYEN { get; set; }

        [Column("UNITCODE")]
        [Description("MÃ ĐƠN VỊ SỬ DỤNG")]
        [StringLength(10)]
        public string UNITCODE { get; set; }
    }
}
