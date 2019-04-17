using System;
using ERBus.Cashier.Dto;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Linq;
using ERBus.Cashier.Common;
using System.Drawing;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public partial class ReportBill_TraLai : XtraReport
    {
        public ReportBill_TraLai()
        {
            InitializeComponent();
            lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        // GIAO DỊCH BÁN LẺ
        public void InitDataBillBanLeTraLai(GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL, BILL_DTO objecBillDto)
        {
            p_Phone.Value = objecBillDto.PHONE;
            p_Address.Value = objecBillDto.ADDRESS;
            p_MaGiaoDich.Value = objecBillDto.MA_GIAODICH;
            p_InfoThuNgan.Value = objecBillDto.INFOTHUNGAN;
            p_MaKH.Value = objecBillDto.MAKH;
            p_Diem.Value = objecBillDto.DIEM;
            p_ThanhTienChu.Value = objecBillDto.THANHTIENCHU;
            p_ConLai.Value = objecBillDto.CONLAI;
            p_TienKhachTra.Value = objecBillDto.TIENKHACHTRA;
            p_TenCuaHang.Value = objecBillDto.TENCUAHANG;
            if (_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS.Count > 0)
            {
                List<VATTU_DTO.OBJ_VAT> obj_Vat = new List<VATTU_DTO.OBJ_VAT>();
                foreach (var rowData in _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS)
                {
                    var existVat = obj_Vat.FirstOrDefault(x => x.MATHUE_RA == rowData.MATHUE_RA);
                    if (existVat != null)
                    {
                        existVat.CHUACO_GTGT += rowData.THANHTIEN / (1 + (rowData.GIATRI_THUE_RA / 100));
                        existVat.CO_GTGT += (rowData.THANHTIEN / (1 + (rowData.GIATRI_THUE_RA / 100))) * (rowData.GIATRI_THUE_RA / 100);
                    }
                    else
                    {
                        VATTU_DTO.OBJ_VAT vat = new VATTU_DTO.OBJ_VAT();
                        vat.MATHUE_RA = rowData.MATHUE_RA;
                        vat.GIATRI_THUE_RA = rowData.GIATRI_THUE_RA;
                        vat.CHUACO_GTGT = rowData.THANHTIEN / (1 + (rowData.GIATRI_THUE_RA / 100));
                        vat.CO_GTGT = vat.CHUACO_GTGT * (vat.GIATRI_THUE_RA / 100);
                        obj_Vat.Add(vat);
                    }
                    decimal tempKM = 0;
                    try
                    {
                        tempKM = rowData.TIEN_KHUYENMAI + rowData.TIEN_CHIETKHAU;
                    }
                    catch (Exception) { }
                    if (tempKM != 0)
                    {
                        rowData.THANHTIENFULL = string.Format(@"KM " + FormatCurrency.FormatMoney(tempKM) + " " + FormatCurrency.FormatMoney(rowData.THANHTIEN));
                    }
                    else
                    {
                        rowData.THANHTIENFULL = FormatCurrency.FormatMoney(rowData.THANHTIEN);
                    }
                    int vattat = 0;
                    int.TryParse(rowData.MATHUE_RA, out vattat);
                    rowData.MAVATTAT = vattat.ToString();
                }
                if (obj_Vat.Count > 0)
                {
                    decimal TONGCHUAVAT = 0, TONGCOVAT = 0;
                    foreach (VATTU_DTO.OBJ_VAT item in obj_Vat)
                    {
                        int vattat = 0;
                        int.TryParse(item.MATHUE_RA, out vattat);
                        XRTableRow row = new XRTableRow();
                        row.Cells.Add(new XRTableCell()
                        {
                            Text = vattat.ToString() + " > " + item.GIATRI_THUE_RA.ToString("#0'%'"),
                            TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                            Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                        });
                        row.Cells.Add(new XRTableCell()
                        {
                            Text = item.CHUACO_GTGT.ToString("##,###"),
                            TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                            Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                        });
                        row.Cells.Add(new XRTableCell()
                        {
                            Text = item.CO_GTGT.ToString("##,###"),
                            TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                            Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                        });
                        xtVAT.Rows.Add(row);
                        TONGCHUAVAT += item.CHUACO_GTGT;
                        TONGCOVAT += item.CO_GTGT;
                    }
                    XRTableRow rowData = new XRTableRow();
                    rowData.Cells.Add(new XRTableCell()
                    {
                        Text = "Tổng : ",
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                        Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                    });
                    rowData.Cells.Add(new XRTableCell()
                    {
                        Text = TONGCHUAVAT.ToString("##,###"),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                        Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                    });
                    rowData.Cells.Add(new XRTableCell()
                    {
                        Text = TONGCOVAT.ToString("##,###"),
                        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                        Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                    });
                    xtTong.Rows.Add(rowData);
                }
            }
            objectDataSource1.DataSource = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS;
        }
    }
}
