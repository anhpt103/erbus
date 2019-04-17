namespace ERBus.Cashier.Dto
{
    public class VATTU_DTO
    {
        public string MAHANG { get; set; }
        public string TENHANG { get; set; }
        public string DONVITINH { get; set; }
        public string MANHACUNGCAP { get; set; }
        public string BARCODE { get; set; }
        public decimal GIABANBUON_VAT { get; set; }
        public decimal GIABANLE_VAT { get; set; }
        public decimal GIAVON { get; set; }
        public decimal SOLUONG { get; set; }
        public string ITEMCODE { get; set; }
        public string MATHUE_RA { get; set; }
        public decimal GIATRI_THUE_RA { get; set; }
        public string MABO { get; set; }
        public decimal TIEN_KHUYENMAIRETURN { get; set; }
        public decimal TONGLEBOHANG { get; set; }
        public decimal TYLE_LAILE { get; set; }
        public string MA_KHUYENMAI { get; set; }
        public bool LAMACAN { get; set; }
        public decimal TONCUOIKYSL { get; set; }

        public class OBJ_VAT
        {
            public string MATHUE_RA { get; set; }
            public decimal GIATRI_THUE_RA { get; set; }
            public decimal CHUACO_GTGT { get; set; }
            public decimal CO_GTGT { get; set; }
        }

        public class CAL_KHUYENMAI_OBJ
        {
            public string MA_KHUYENMAI { get; set; }
            public decimal GIATRI_KHUYENMAI { get; set; }
        }
    }
}
