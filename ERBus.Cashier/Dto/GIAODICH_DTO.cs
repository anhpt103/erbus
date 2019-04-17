using System;
using System.Collections.Generic;

namespace ERBus.Cashier.Dto
{
    [Serializable]
    public class GIAODICH_DTO
    {
        public GIAODICH_DTO()
        {
            LST_DETAILS = new List<GIAODICH_CHITIET>();
        }
        public string ID { get; set; }
        public string MA_GIAODICH { get; set; }
        public string LOAI_GIAODICH { get; set; }
        public DateTime NGAY_GIAODICH { get; set; }
        public string MAKHACHHANG { get; set; }
        public string THOIGIAN_TAO { get; set; }
        public decimal TIENKHACH_TRA { get; set; }
        public decimal TIEN_TRALAI_KHACH { get; set; }
        public string MAKHO_XUAT { get; set; }
        public string MA_VOUCHER { get; set; }
        public string DIENGIAI { get; set; }
        public DateTime? I_CREATE_DATE { get; set; }
        public string I_CREATE_BY { get; set; }
        public DateTime? I_UPDATE_DATE { get; set; }
        public string I_UPDATE_BY { get; set; }
        public virtual string I_STATE { get; set; }
        public string UNITCODE { get; set; }
        public decimal THANHTIEN { get; set; }
        public decimal TONGSOLUONG { get; set; }
        public decimal DIEMQUYDOI { get; set; }
        public decimal SODIEM { get; set; }
        public string HANGKHACHHANG { get; set; }
        public string HANGKHACHHANG_MOI { get; set; }
        public decimal SOTIEN_LENHANG { get; set; }
        public decimal TONGTIEN_KHACHHANG { get; set; }
        public decimal TIENTHE { get; set; }
        public decimal QUYDOITIEN_THANH_DIEM { get; set; }
        public decimal QUYDOIDIEM_THANH_TIEN { get; set; }
        public decimal QUYDOI_TOIDA { get; set; }
        public List<GIAODICH_CHITIET> LST_DETAILS { get; set; }

    }
    [Serializable]
    public class GIAODICH_CHITIET
    {
        public string ID { get; set; }
        public string MA_GIAODICH { get; set; }
        public string DONVITINH { get; set; }
        public string MAHANG { get; set; }
        public string TENHANG { get; set; }
        public string MATHUE_RA { get; set; }
        public decimal SOLUONG { get; set; }
        public decimal GIABANLE_VAT { get; set; }
        public string MA_KHUYENMAI { get; set; }
        public decimal TYLE_KHUYENMAI { get; set; }
        public decimal TIEN_KHUYENMAI { get; set; }
        public decimal TYLE_CHIETKHAU { get; set; }
        public decimal TIEN_CHIETKHAU { get; set; }
        public decimal TIENTHE_VIP { get; set; }
        public decimal TIEN_VOUCHER { get; set; }
        public decimal THANHTIEN { get; set; }
        public decimal THANHTIEN_CHUA_GIAMGIA { get; set; }
        public decimal GIAVON { get; set; }
        public string THANHTIEN_TEXT { get; set; }
        public string MABOPK { get; set; }
        public decimal GIATRI_THUE_RA { get; set; }
        public int? SAPXEP { get; set; }
        public string THANHTIENFULL { get; set; }
        public string MAVATTAT { get; set; }
    }
}
