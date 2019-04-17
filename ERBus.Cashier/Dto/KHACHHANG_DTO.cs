using System;

namespace ERBus.Cashier.Dto
{
    public class KHACHHANG_DTO
    {
        public string MAKHACHHANG { get; set; }
        public string TENKHACHHANG { get; set; }
        public string DIACHI { get; set; }
        public string DIENTHOAI { get; set; }
        public string CANCUOC_CONGDAN { get; set; }
        public DateTime? NGAYSINH { get; set; }
        public DateTime? NGAYDACBIET { get; set; }
        public string MATHE { get; set; }
        public decimal SODIEM { get; set; }
        public decimal TONGTIEN { get; set; }
        public string DIENGIAI { get; set; }
        public Nullable<int> TRANGTHAI { get; set; }
        public DateTime? I_CREATE_DATE { get; set; }
        public string I_CREATE_BY { get; set; }
        public DateTime? I_UPDATE_DATE { get; set; }
        public string I_UPDATE_BY { get; set; }
        public virtual string I_STATE { get; set; }
        public string HANGKHACHHANG { get; set; }
        public string HANGKHACHHANGCU { get; set; }
        public string UNITCODE { get; set; }
    }
}
