using System;

namespace ERBus.Cashier.Dto
{
    public class BILL_DTO
    {
        public string TENCUAHANG { get; set; }
        public int LOAI_GIAODICH { get; set; }
        public DateTime NgayTao { get; set; }
        public string PHONE { get; set; }
        public string ADDRESS { get; set; }
        public string MA_GIAODICH { get; set; }
        public string INFOTHUNGAN { get; set; }
        public string MAKH { get; set; }
        public string THANHTIENCHU { get; set; }
        public decimal CONLAI { get; set; }
        public decimal TIENKHACHTRA { get; set; }
        public decimal DIEM { get; set; }
        public string QUAYHANG { get; set; }
    }
}
