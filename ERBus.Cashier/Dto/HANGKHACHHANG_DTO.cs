using System;
namespace ERBus.Cashier.Dto
{
    public class HANGKHACHHANG_DTO
    {
        public string ID { get; set; }
        public string MAHANG { get; set; }
        public string TENHANG { get; set; }
        public decimal SOTIEN_LENHANG { get; set; }
        public decimal TYLE_SINHNHAT { get; set; }
        public decimal TYLE_DACBIET { get; set; }
        public int TRANGTHAI { get; set; }
        public DateTime I_CREATE_DATE { get; set; }
        public string I_CREATE_BY { get; set; }
        public DateTime I_UPDATE_DATE { get; set; }
        public string I_UPDATE_BY { get; set; }
        public string I_STATE { get; set; }
        public string UNITCODE { get; set; }
        public decimal QUYDOITIEN_THANH_DIEM { get; set; }
        public decimal QUYDOIDIEM_THANH_TIEN { get; set; }
    }
}
