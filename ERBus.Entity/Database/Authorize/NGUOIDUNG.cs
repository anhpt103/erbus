using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERBus.Entity.Database.Authorize
{
    [Table("NGUOIDUNG")]
    public class NGUOIDUNG : DataInfoEntity
    {
        [Required]
        [Column("USERNAME")]
        [Description("TÀI KHOẢN NGƯỜI DÙNG")]
        [StringLength(50)]
        public string USERNAME { get; set; }

        [Required]
        [Column("PASSWORD")]
        [Description("MẬT KHẨU NGƯỜI DÙNG")]
        [StringLength(50, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string PASSWORD { get; set; }

        [Column("MANHANVIEN")]
        [Description("MÃ NHÂN VIÊN")]
        [StringLength(10)]
        public string MANHANVIEN { get; set; }

        [Column("TENNHANVIEN")]
        [Description("NGÀY TẠO DỮ LIỆU")]
        [StringLength(100)]
        public string TENNHANVIEN { get; set; }

        [Column("SODIENTHOAI")]
        [Description("SỐ ĐIỆN THOẠI")]
        [StringLength(16)]
        public string SODIENTHOAI { get; set; }

        [Column("CHUNGMINHTHU")]
        [Description("CHỨNG MINH THƯ NHÂN DÂN")]
        [StringLength(20)]
        public string CHUNGMINHTHU { get; set; }

        [Column("GIOITINH")]
        [Description("GIỚI TÍNH 0: NỮ _ 1: NAM")]
        public Nullable<int> GIOITINH { get; set; }

        [Column("VAITRO")]
        [Description("VAI TRÒ NHÂN VIÊN 0: NGƯỜI DÙNG BÊN NGOÀI _ 1: NHÂN VIÊN THU NGÂN _ 2: NHÂN VIÊN QUẢN TRỊ _ 3: SỬ DỤNG CẢ THU NGÂN VÀ QUẢN TRỊ")]
        public Nullable<int> VAITRO { get; set; }

        [Column("TRANGTHAI")]
        [Description("TRẠNG THÁI DỮ LIỆU 0: CHƯA DUYỆT _ 10: ĐÃ DUYỆT _ 20: LƯU TẠM _ 30: TRẠNG THÁI MỞ RỘNG")]
        public Nullable<int> TRANGTHAI { get; set; }

        [Column("PARENT_UNITCODE")]
        [Description("MÃ ĐƠN VỊ CHA")]
        [StringLength(10)]
        public string PARENT_UNITCODE { get; set; }
    }
}
