using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity
{
    public class DataInfoEntity : EntityBase
    {
        [Key]
        [Column("ID")]
        [Description("KHÓA CHÍNH")]
        [StringLength(50)]
        public string ID { get; set; }

        [Column("I_CREATE_DATE")]
        [Description("NGÀY TẠO DỮ LIỆU")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime? I_CREATE_DATE { get; set; }

        [Column("I_CREATE_BY")]
        [Description("NGƯỜI TẠO DỮ LIỆU")]
        [StringLength(25)]
        public string I_CREATE_BY { get; set; }

        [Column("I_UPDATE_DATE")]
        [Description("NGÀY CẬP NHẬT DỮ LIỆU")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime? I_UPDATE_DATE { get; set; }

        [Column("I_UPDATE_BY")]
        [Description("NGƯỜI CẬP NHẬT DỮ LIỆU")]
        [StringLength(25)]
        public string I_UPDATE_BY { get; set; }

        [Column("I_STATE")]
        [Description("TRẠNG THÁI DỮ LIỆU: MẶC ĐỊNH LÀ A - S đồng bộ lên")]
        [StringLength(1)]
        public virtual string I_STATE { get; set; }

        [Column("UNITCODE")]
        [Description("MÃ ĐƠN VỊ SỬ DỤNG")]
        [StringLength(10)]
        public string UNITCODE { get; set; }
    }
}
