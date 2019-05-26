using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ERBus.Cashier.Common;
using ERBus.Cashier.Dto;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Text;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public delegate void XuLyThanhToan(bool state);
    public delegate void SearchVatTu(string filter);
    public delegate void RePrintBill(string BillId, DateTime toDate, DateTime fromDate);
    public delegate void SELECT_VATTU(int index);

    public partial class UC_Frame_BanLe : UserControl
    {
        private Keys CurrentKey = new Keys();
        private bool FlagTangHang = true; private int CurentIndexDgv = 0;
        private int MethodPrice = 0;
        public static decimal CURRENT_ROW_GIABANLE_VAT = 0;
        public static decimal THANHTOAN_TONGTIEN_THANHTOAN = 0;
        public static string THANHTOAN_MAGIAODICH = string.Empty;
        public static DataGridView gridDataTemp = new DataGridView();
        public static int LOAIGIAODICH = 1;
        private List<VATTU_DTO> listData_TrungMa = new List<VATTU_DTO>();
        private FrmThanhToan frmThanhToan;
        private FrmTimKiemGiaoDich frmSearch;
        private frmSelectVatTu frmSelect;
        private int USE_LCD;
        public static decimal CURRENT_SOLUONG_MACAN = 0;
        public UC_Frame_BanLe()
        {
            InitializeComponent();
            try
            {
                if (Config.CheckConnectToServer())
                {
                    lblNgayPhatSinhPeriod.Text = "TRẠNG THÁI BÁN: ONLINE";
                    lblNgayPhatSinhPeriod.ForeColor = Color.ForestGreen;
                    //ROLE_STATE GET_ACCESS_MENU = Session.Session.GET_ROLE_BY_USERNAME(Session.Session.CurrentUnitCode, Session.Session.CurrentUserName, "banLeQuayThuNgan");
                    //if (GET_ACCESS_MENU != null)
                    //{
                    //    if (!GET_ACCESS_MENU.Banbuon) cbbLoaiGiaoDich.Items.Remove("Bán buôn");
                    //    if (!GET_ACCESS_MENU.Giavon) cbbLoaiGiaoDich.Items.Remove("Giá vốn");
                    //    if (!GET_ACCESS_MENU.Banchietkhau)
                    //    {
                    //        lblCKDon.Visible = false;
                    //        lblCKLe.Visible = false;
                    //        lblChietKhauLe.Visible = false;
                    //        txtChietKhauToanDon.Visible = false;
                    //        dgvDetails_Tab.Columns["CHIETKHAU"].Visible = false;
                    //    }
                    //    if (!GET_ACCESS_MENU.Approve)
                    //    {
                    //        btnF7.Visible = false;
                    //        //btnF7.Visible = true;
                    //    }
                    //    //tạm thời khóa chức năng bán giá vốn
                    //    cbbLoaiGiaoDich.Items.Remove("Giá vốn");
                    //}
                }
                else
                {
                    lblNgayPhatSinhPeriod.Text = "TRẠNG THÁI BÁN: OFFLINE";
                    lblNgayPhatSinhPeriod.ForeColor = Color.Red;
                    //lblCKDon.Visible = false;
                    //lblCKLe.Visible = false;
                    //lblChietKhauLe.Visible = false;
                    //txtChietKhauToanDon.Visible = false;
                    //dgvDetails_Tab.Columns["CHIETKHAU"].Visible = false;
                    btnF7.Visible = false;
                    cbbLoaiGiaoDich.Items.Remove("Giá vốn");
                    cbbLoaiGiaoDich.Items.Remove("Bán buôn");
                }
            }
            catch
            {

            }
            lblTenCuaHang.Text = Session.Session.CurrentNameStore;
            dgvDetails_Tab.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvDetails_Tab.RowTemplate.Height = 30;
            dgvDetails_Tab.Columns["STT"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["MAHANG"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["TENHANG"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["DONVITINH"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["GIABANLE_VAT"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["SOLUONG"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["GIAVON"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["TIENKM"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["CHIETKHAU"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["THANHTIEN"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["LAMACAN"].Visible = false;
            dgvDetails_Tab.Columns["TONCUOIKYSL"].Visible = false;
            dgvDetails_Tab.Columns["MA_KHUYENMAI"].Visible = false;
            //KHỞI TẠO DROPDOW BUTTON CHỌN HÌNH THỨC GIÁ BÁN
            btnF1ThanhToan.Enabled = false;
            this.btnF1ThanhToan.BackColor = Color.Gray;
            btnF2ThemMoi.Enabled = true;
            this.btnF2ThemMoi.BackColor = Color.CadetBlue;
            btnF3GiamHang.Enabled = false;
            this.btnF3GiamHang.BackColor = Color.Gray;
            btnF4TangHang.Enabled = false;
            this.btnF4TangHang.BackColor = Color.Gray;
            btnF5LamMoi.Enabled = false;
            this.btnF5LamMoi.BackColor = Color.Gray;
            btnSearchAll.Enabled = false;
            this.btnSearchAll.BackColor = Color.Gray;
            btnF7.Enabled = true;
            this.btnF7.BackColor = Color.CadetBlue;
            lblNgayPhatSinhPeriod.Text = Session.Session.CurrentNgayPhatSinh.ToString("dd/MM/yyyy");
            //Mặc định chọn Bán theo giá bán lẻ vat
            cbbLoaiGiaoDich.SelectedIndex = 0;
            MethodPrice = 1;
            LOAIGIAODICH = 1;
            dgvDetails_Tab.Columns["GIAVON"].Visible = false;
            Session.Session.CurrentLoaiGiaoDich = "BANLE";
            USE_LCD = FrmXuatBanLeService.GET_THAMSO_SUDUNG_MANHINH_LCD_FROM_ORACLE();
        }

        private decimal SUM_TTIENCOVAT_THANHTOAN(DataGridView dgvCheck)
        {
            decimal SUM_TTIENCOVAT = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal THANHTIEN_ROW = 0;
                    decimal.TryParse(rowCheck.Cells["THANHTIEN"].Value.ToString(), out THANHTIEN_ROW);
                    SUM_TTIENCOVAT = SUM_TTIENCOVAT + THANHTIEN_ROW;
                }
            }
            return SUM_TTIENCOVAT;
        }

        private decimal SUM_SOLUONG_BAN(DataGridView dgvCheck)
        {
            decimal SUM_SOLUONG = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal SOLUONG_ROW = 0;
                    decimal.TryParse(rowCheck.Cells["SOLUONG"].Value.ToString(), out SOLUONG_ROW);
                    SUM_SOLUONG = SUM_SOLUONG + SOLUONG_ROW;
                }
            }
            return SUM_SOLUONG;
        }

        private decimal SUM_TONGTIEN_KHUYENMAI(DataGridView dgvCheck)
        {
            decimal SUM_TIEN_KHUYENMAI = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal TIEN_KHUYENMAI_ROW = 0;
                    decimal.TryParse(rowCheck.Cells["TIENKM"].Value.ToString(), out TIEN_KHUYENMAI_ROW);
                    SUM_TIEN_KHUYENMAI = SUM_TIEN_KHUYENMAI + TIEN_KHUYENMAI_ROW;
                }
            }
            return SUM_TIEN_KHUYENMAI;
        }
        private void SUM_TONGTIEN_CHIETKHAU_LE(DataGridView dgvCheck)
        {

            decimal SUM_TIEN_CHIETKHAU = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal TIENCK = 0;
                    decimal TIEN_CHIETKHAU_ROW = 0;
                    decimal SOLUONG = 0;
                    decimal GIABANLE_VAT = 0;
                    decimal THANHTIEN = 0;
                    decimal TIENKM = 0;

                    string CHIETKHAU_PERCENT = string.Empty;
                    if (rowCheck.Cells["CHIETKHAU"].Value != null)
                    {
                        CHIETKHAU_PERCENT = rowCheck.Cells["CHIETKHAU"].Value.ToString().Trim();
                    }
                    else
                    {
                        CHIETKHAU_PERCENT = "0";
                        rowCheck.Cells["CHIETKHAU"].Value = CHIETKHAU_PERCENT;
                    }
                    if (CHIETKHAU_PERCENT.Contains('%'))
                    {
                        CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                    }
                    decimal.TryParse(CHIETKHAU_PERCENT, out TIEN_CHIETKHAU_ROW);

                    //item.TIENCK = item.SOLUONG * item.GIABANLE_VAT * chietKhau / 100;
                    decimal.TryParse(rowCheck.Cells["SOLUONG"].Value.ToString(), out SOLUONG);
                    decimal.TryParse(rowCheck.Cells["GIABANLE_VAT"].Value.ToString(), out GIABANLE_VAT);
                    decimal.TryParse(rowCheck.Cells["THANHTIEN"].Value.ToString(), out THANHTIEN);
                    decimal.TryParse(rowCheck.Cells["TIENKM"].Value.ToString(), out TIENKM);
                    if (TIEN_CHIETKHAU_ROW <= 100 && TIEN_CHIETKHAU_ROW > 0 && TIEN_CHIETKHAU_ROW <= THANHTIEN)
                    {
                        TIENCK = SOLUONG * GIABANLE_VAT * TIEN_CHIETKHAU_ROW / 100;
                    }
                    else if (TIEN_CHIETKHAU_ROW > THANHTIEN)
                    {
                        MessageBox.Show("CHIẾT KHẤU KHÔNG HỢP LỆ !");
                        TIENCK = 0;
                        rowCheck.Cells["CHIETKHAU"].Value = TIENCK;
                    }
                    else //Tiền chiết khấu
                    {
                        TIENCK = TIEN_CHIETKHAU_ROW;
                    }
                    rowCheck.Cells["THANHTIEN"].Value = FormatCurrency.FormatMoney(GIABANLE_VAT * SOLUONG - TIENKM - TIENCK);
                    SUM_TIEN_CHIETKHAU = SUM_TIEN_CHIETKHAU + TIENCK;
                }
                lblChietKhauLe.Text = FormatCurrency.FormatMoney(SUM_TIEN_CHIETKHAU);
            }
        }
        public string CONVERT_FROM_KEY_TO_CODEVATTU(string KEY)
        {
            string _RESULT = "";
            if (KEY.Length >= 4)
            {
                string beginCharacter = string.Empty;
                if (!string.IsNullOrEmpty(KEY)) beginCharacter = KEY.Substring(0, 2);
                if (beginCharacter.Equals(20) && KEY.Length > 9)
                {
                    string itemCode = string.Empty; if (!string.IsNullOrEmpty(KEY)) itemCode = KEY.Substring(2, 5);
                    if (Config.CheckConnectToServer())
                    {
                        _RESULT = FrmXuatBanLeService.CONVERT_MACAN_TO_MAHANG_ORACLE(itemCode, Session.Session.CurrentUnitCode);
                    }
                    else
                    {
                        _RESULT = FrmXuatBanLeService.CONVERT_MACAN_TO_MAHANG_SQLSERVER(itemCode, Session.Session.CurrentUnitCode);
                    }
                }
                else if (beginCharacter == "BH")
                {
                    _RESULT = KEY;
                }
                else
                {
                    if (Config.CheckConnectToServer())
                    {
                        _RESULT = FrmXuatBanLeService.CONVERT_BARCODE_TO_MAHANG_ORACLE(KEY, Session.Session.CurrentUnitCode);
                    }
                    else
                    {
                        _RESULT = FrmXuatBanLeService.CONVERT_BARCODE_TO_MAHANG_SQLSERVER(KEY, Session.Session.CurrentUnitCode);

                    }
                }
            }
            return _RESULT;
        }
        private int CHECK_ROW_EXIST_DATAGRIDVIEW(DataGridView dgvCheck, string maVatTu)
        {
            int result = -1;
            if (maVatTu.Length != 7)
            {
                maVatTu = CONVERT_FROM_KEY_TO_CODEVATTU(maVatTu);
            }
            foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
            {
                string code = string.Empty;
                code = rowCheck.Cells["MAHANG"].Value.ToString();
                if (!string.IsNullOrEmpty(code) && code.Trim().ToUpper() == maVatTu.Trim().ToUpper())
                {
                    result = rowCheck.Index;
                }
            }
            return result;
        }

        private void TINHTOAN_TONGTIEN_TOANHOADON(DataGridView gridView)
        {
            lblTongTienThanhToan.Text = FormatCurrency.FormatMoney(SUM_TTIENCOVAT_THANHTOAN(gridView).ToString());
            lblSumSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN(gridView).ToString());
            lblTongTienKhuyenMai.Text = FormatCurrency.FormatMoney(SUM_TONGTIEN_KHUYENMAI(gridView).ToString());
            if (gridView.Rows.Count == 0)
            {
                txtSoLuong.Text = "0";
                txtMaHang.Text = string.Empty;
            }
        }

        //KHỞI TẠO DỮ LIỆU PASSING SANG FORM THANH TOÁN

        public GIAODICH_DTO KHOITAO_DULIEU_BILL_BANLE(DataGridView DataGridViewBanLe)
        {
            GIAODICH_DTO NVGDQUAY_ASYNCCLIENT_BILL = new GIAODICH_DTO();
            NVGDQUAY_ASYNCCLIENT_BILL.MA_GIAODICH = lblMaGiaoDichQuay.Text.Trim();
            NVGDQUAY_ASYNCCLIENT_BILL.LOAI_GIAODICH = "XBAN_LE";
            NVGDQUAY_ASYNCCLIENT_BILL.I_CREATE_DATE = DateTime.Now;
            NVGDQUAY_ASYNCCLIENT_BILL.NGAY_GIAODICH = Session.Session.CurrentNgayPhatSinh;
            NVGDQUAY_ASYNCCLIENT_BILL.THOIGIAN_TAO = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            NVGDQUAY_ASYNCCLIENT_BILL.I_CREATE_BY = Session.Session.CurrentMaNhanVien;
            NVGDQUAY_ASYNCCLIENT_BILL.TIENKHACH_TRA = 0;
            NVGDQUAY_ASYNCCLIENT_BILL.TIEN_TRALAI_KHACH = 0;
            decimal THANHTIEN = 0; decimal.TryParse(lblTongTienThanhToan.Text, out THANHTIEN);
            NVGDQUAY_ASYNCCLIENT_BILL.THANHTIEN = THANHTIEN;
            NVGDQUAY_ASYNCCLIENT_BILL.MAKHACHHANG = "";
            decimal TONGSOLUONG = 0; decimal.TryParse(lblSumSoLuong.Text, out TONGSOLUONG);
            NVGDQUAY_ASYNCCLIENT_BILL.TONGSOLUONG = TONGSOLUONG;
            if (DataGridViewBanLe.RowCount > 0)
            {
                foreach (DataGridViewRow dataRow in DataGridViewBanLe.Rows)
                {
                    GIAODICH_CHITIET GIAODICH_CHITIET_BILL = new GIAODICH_CHITIET();
                    GIAODICH_CHITIET_BILL.MA_GIAODICH = NVGDQUAY_ASYNCCLIENT_BILL.MA_GIAODICH;
                    GIAODICH_CHITIET_BILL.MAHANG = dataRow.Cells["MAHANG"].Value != null ? dataRow.Cells["MAHANG"].Value.ToString().ToUpper().Trim() : "";
                    GIAODICH_CHITIET_BILL.TENHANG = dataRow.Cells["TENHANG"].Value != null ? dataRow.Cells["TENHANG"].Value.ToString().ToUpper().Trim() : "";
                    decimal SOLUONG = 0;
                    if (dataRow.Cells["SOLUONG"].Value != null)
                    {
                        decimal.TryParse(dataRow.Cells["SOLUONG"].Value.ToString(), out SOLUONG);
                    }
                    GIAODICH_CHITIET_BILL.SOLUONG = SOLUONG;
                    decimal TTIENCOVAT_DETAIL = 0;
                    if (dataRow.Cells["THANHTIEN"].Value != null)
                    {
                        decimal.TryParse(dataRow.Cells["THANHTIEN"].Value.ToString(), out TTIENCOVAT_DETAIL);
                    }
                    GIAODICH_CHITIET_BILL.THANHTIEN = TTIENCOVAT_DETAIL;
                    decimal GIABANLECOVAT_DETAIL = 0;
                    if (dataRow.Cells["GIABANLE_VAT"].Value != null)
                    {
                        decimal.TryParse(dataRow.Cells["GIABANLE_VAT"].Value.ToString(), out GIABANLECOVAT_DETAIL);
                    }
                    GIAODICH_CHITIET_BILL.GIABANLE_VAT = GIABANLECOVAT_DETAIL;

                    decimal TIEN_CHIETKHAU_DETAIL = 0;
                    if (dataRow.Cells["CHIETKHAU"].Value != null)
                    {
                        string CHIETKHAU_PERCENT = dataRow.Cells["CHIETKHAU"].Value.ToString().Trim();
                        if (CHIETKHAU_PERCENT.Contains('%'))
                        {
                            CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out TIEN_CHIETKHAU_DETAIL);
                        }
                        else
                        {
                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out TIEN_CHIETKHAU_DETAIL);
                        }
                    }



                    decimal TIEN_KHUYENMAI_DETAIL = 0;
                    if (dataRow.Cells["TIENKM"].Value != null)
                    {
                        decimal.TryParse(dataRow.Cells["TIENKM"].Value.ToString(), out TIEN_KHUYENMAI_DETAIL);
                    }
                    GIAODICH_CHITIET_BILL.TIEN_KHUYENMAI = TIEN_KHUYENMAI_DETAIL;

                    if (TIEN_CHIETKHAU_DETAIL <= 100)
                    {
                        GIAODICH_CHITIET_BILL.TYLE_CHIETKHAU = TIEN_CHIETKHAU_DETAIL;
                        GIAODICH_CHITIET_BILL.TIEN_CHIETKHAU = decimal.Round((TIEN_CHIETKHAU_DETAIL / 100) * (GIAODICH_CHITIET_BILL.GIABANLE_VAT * GIAODICH_CHITIET_BILL.SOLUONG - GIAODICH_CHITIET_BILL.TIEN_KHUYENMAI), 2);
                    }
                    else
                    {
                        GIAODICH_CHITIET_BILL.TIEN_CHIETKHAU = TIEN_CHIETKHAU_DETAIL;
                        GIAODICH_CHITIET_BILL.TYLE_CHIETKHAU = decimal.Round((100 * TIEN_CHIETKHAU_DETAIL) / (GIAODICH_CHITIET_BILL.GIABANLE_VAT * GIAODICH_CHITIET_BILL.SOLUONG - GIAODICH_CHITIET_BILL.TIEN_KHUYENMAI), 2);
                    }
                    if (dataRow.Cells["MAHANG"].Value != null)
                    {
                        string MAHANG = dataRow.Cells["MAHANG"].Value.ToString().ToUpper().Trim();
                        if (MAHANG.Substring(0, 2).Equals("BH"))
                        {
                            GIAODICH_CHITIET_BILL.MABOPK = MAHANG;
                            EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();
                            if (Config.CheckConnectToServer())
                            {
                                _EXTEND_VAT_BOHANG =
                                    FrmXuatBanLeService.LAYDULIEU_VAT_BOHANG_FROM_DATABASE_ORACLE(
                                        GIAODICH_CHITIET_BILL.MABOPK, Session.Session.CurrentUnitCode);

                            }
                            else
                            {
                                _EXTEND_VAT_BOHANG = FrmXuatBanLeService.LAYDULIEU_VAT_BOHANG_FROM_DATABASE_SQLSERVER(GIAODICH_CHITIET_BILL.MABOPK, Session.Session.CurrentUnitCode);
                            }
                            GIAODICH_CHITIET_BILL.MATHUE_RA = _EXTEND_VAT_BOHANG.MATHUE_RA;
                        }
                        else
                        {

                            GIAODICH_CHITIET_BILL.MABOPK = "BH";
                            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
                            if (Config.CheckConnectToServer())
                            {
                                _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_ORACLE(GIAODICH_CHITIET_BILL.MAHANG, Session.Session.CurrentUnitCode);
                            }
                            else
                            {
                                _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(GIAODICH_CHITIET_BILL.MAHANG, Session.Session.CurrentUnitCode);
                            }

                            GIAODICH_CHITIET_BILL.MATHUE_RA = _EXTEND_VATTU_DTO.MATHUE_RA;
                            GIAODICH_CHITIET_BILL.GIATRI_THUE_RA = _EXTEND_VATTU_DTO.GIATRI_THUE_RA;
                        }
                    }

                    if (GIAODICH_CHITIET_BILL.TIEN_KHUYENMAI != 0)
                    {
                        if (GIAODICH_CHITIET_BILL.TIEN_CHIETKHAU != 0)
                        {
                            GIAODICH_CHITIET_BILL.THANHTIEN_TEXT = string.Format(@"KM:" + FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.TIEN_KHUYENMAI) + ";" + "CK:" + FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.TIEN_CHIETKHAU) + " " + FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.THANHTIEN));
                        }
                        else
                        {
                            GIAODICH_CHITIET_BILL.THANHTIEN_TEXT = string.Format(@"KM:" + FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.TIEN_KHUYENMAI) + " " + FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.THANHTIEN));
                        }
                    }
                    else
                    {
                        GIAODICH_CHITIET_BILL.THANHTIEN_TEXT = FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.THANHTIEN);
                    }

                    if (GIAODICH_CHITIET_BILL.TIEN_CHIETKHAU != 0)
                    {
                        if (GIAODICH_CHITIET_BILL.TIEN_KHUYENMAI != 0)
                        {
                            GIAODICH_CHITIET_BILL.THANHTIEN_TEXT = string.Format(@"KM:" + FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.TIEN_KHUYENMAI) + ";" + "CK:" + FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.TIEN_CHIETKHAU) + " " + FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.THANHTIEN));
                        }
                        else
                        {
                            GIAODICH_CHITIET_BILL.THANHTIEN_TEXT = string.Format(@"CK " + FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.TIEN_CHIETKHAU) + " " + FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.THANHTIEN));
                        }
                    }
                    else
                    {
                        GIAODICH_CHITIET_BILL.THANHTIEN_TEXT = FormatCurrency.FormatMoney(GIAODICH_CHITIET_BILL.THANHTIEN);
                    }
                    GIAODICH_CHITIET_BILL.THANHTIEN_CHUA_GIAMGIA = GIAODICH_CHITIET_BILL.GIABANLE_VAT * GIAODICH_CHITIET_BILL.SOLUONG;
                    NVGDQUAY_ASYNCCLIENT_BILL.LST_DETAILS.Add(GIAODICH_CHITIET_BILL);
                }
            }
            return NVGDQUAY_ASYNCCLIENT_BILL;
        }

        public GIAODICH_DTO KHOITAO_DULIEU_THANHTOAN_BANLE(DataGridView DataGridViewBanLe)
        {
            GIAODICH_DTO GIAODICH_DTO = new GIAODICH_DTO();
            GIAODICH_DTO.ID = Guid.NewGuid().ToString();
            GIAODICH_DTO.MA_GIAODICH = lblMaGiaoDichQuay.Text.Trim();
            GIAODICH_DTO.LOAI_GIAODICH = "XBAN_LE";
            GIAODICH_DTO.I_CREATE_DATE = DateTime.Now;
            GIAODICH_DTO.I_CREATE_BY = Session.Session.CurrentMaNhanVien;
            GIAODICH_DTO.NGAY_GIAODICH = Session.Session.CurrentNgayPhatSinh;
            GIAODICH_DTO.MA_VOUCHER = "";
            GIAODICH_DTO.TIENKHACH_TRA = 0;
            GIAODICH_DTO.TIEN_TRALAI_KHACH = 0;
            decimal THANHTIEN = 0;
            decimal.TryParse(lblTongTienThanhToan.Text, out THANHTIEN);
            GIAODICH_DTO.THANHTIEN = THANHTIEN;
            GIAODICH_DTO.THOIGIAN_TAO = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            GIAODICH_DTO.MAKHACHHANG = "";
            GIAODICH_DTO.I_UPDATE_DATE = DateTime.Now;
            GIAODICH_DTO.I_UPDATE_BY = Session.Session.CurrentMaNhanVien;
            GIAODICH_DTO.I_STATE = "X";
            GIAODICH_DTO.UNITCODE = Session.Session.CurrentUnitCode;
            decimal TONGSOLUONG = 0; decimal.TryParse(lblSumSoLuong.Text, out TONGSOLUONG);
            GIAODICH_DTO.TONGSOLUONG = TONGSOLUONG;
            //KHỞI TẠO DỮ LIỆU DÒNG CHI TIẾT TỪ DATAGRIDVIEW
            int i = 0;
            if (DataGridViewBanLe.RowCount > 0)
            {
                foreach (DataGridViewRow dataRow in DataGridViewBanLe.Rows)
                {
                    if (dataRow.Cells["MAHANG"].Value != null)
                    {
                        string MAHANG_MABO = dataRow.Cells["MAHANG"].Value.ToString().ToUpper().Trim();
                        if (!MAHANG_MABO.Substring(0, 2).Equals("BH"))
                        {
                            GIAODICH_CHITIET GIAODICH_CHITIET_DTO = new GIAODICH_CHITIET();
                            GIAODICH_CHITIET_DTO.ID = Guid.NewGuid().ToString() + "-" + i;
                            GIAODICH_CHITIET_DTO.MAHANG = MAHANG_MABO;
                            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
                            if (Config.CheckConnectToServer())
                            {
                                _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_ORACLE(MAHANG_MABO, Session.Session.CurrentUnitCode);

                            }
                            else
                            {
                                _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(MAHANG_MABO, Session.Session.CurrentUnitCode);
                            }
                            GIAODICH_CHITIET_DTO.DONVITINH = _EXTEND_VATTU_DTO.DONVITINH;
                            GIAODICH_CHITIET_DTO.TENHANG = _EXTEND_VATTU_DTO.TENHANG;
                            GIAODICH_CHITIET_DTO.MABOPK = "BH";
                            decimal SOLUONG_DETAILS = 0;
                            if (dataRow.Cells["SOLUONG"].Value != null)
                            {
                                decimal.TryParse(dataRow.Cells["SOLUONG"].Value.ToString(), out SOLUONG_DETAILS);
                            }
                            GIAODICH_CHITIET_DTO.SOLUONG = SOLUONG_DETAILS;
                            decimal TTIENCOVAT_DETAILS = 0;
                            if (dataRow.Cells["THANHTIEN"].Value != null)
                            {
                                decimal.TryParse(dataRow.Cells["THANHTIEN"].Value.ToString(), out TTIENCOVAT_DETAILS);
                            }
                            GIAODICH_CHITIET_DTO.THANHTIEN = TTIENCOVAT_DETAILS;
                            decimal GIABANLE_VAT = 0;
                            if (dataRow.Cells["GIABANLE_VAT"].Value != null)
                            {
                                decimal.TryParse(dataRow.Cells["GIABANLE_VAT"].Value.ToString(), out GIABANLE_VAT);
                            }
                            GIAODICH_CHITIET_DTO.GIABANLE_VAT = GIABANLE_VAT;
                            GIAODICH_CHITIET_DTO.MA_KHUYENMAI = dataRow.Cells["MA_KHUYENMAI"].Value != null ? dataRow.Cells["MA_KHUYENMAI"].Value.ToString() : "";
                            decimal TIEN_KHUYENMAI = 0;
                            if (dataRow.Cells["TIENKM"].Value != null)
                            {
                                decimal.TryParse(dataRow.Cells["TIENKM"].Value.ToString(), out TIEN_KHUYENMAI);
                            }
                            GIAODICH_CHITIET_DTO.TIEN_KHUYENMAI = TIEN_KHUYENMAI;
                            decimal TYLE_KHUYENMAI_DETAILS = 0;
                            if (GIAODICH_CHITIET_DTO.GIABANLE_VAT != 0)
                            {
                                TYLE_KHUYENMAI_DETAILS = GIAODICH_CHITIET_DTO.SOLUONG *
                                                        (GIAODICH_CHITIET_DTO.TIEN_KHUYENMAI /
                                                         GIAODICH_CHITIET_DTO.GIABANLE_VAT);
                            }
                            else
                            {
                                GIAODICH_CHITIET_DTO.TYLE_KHUYENMAI = 0;
                                string NOTIFI_WARNING = string.Format(@"CẢNH BÁO ! MÃ HÀNG '{0}' CÓ GIÁ BÁN LẺ BẰNG 0", GIAODICH_CHITIET_DTO.MAHANG);
                                MessageBox.Show(NOTIFI_WARNING);
                            }
                            decimal CHIETKHAU = 0;
                            if (dataRow.Cells["CHIETKHAU"].Value != null)
                            {
                                string CHIETKHAU_PERCENT = dataRow.Cells["CHIETKHAU"].Value.ToString().Trim();
                                if (CHIETKHAU_PERCENT.Contains('%'))
                                {
                                    CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                                    decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out CHIETKHAU);
                                }
                                else
                                {
                                    decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out CHIETKHAU);
                                }
                            }
                            if (CHIETKHAU <= 100)
                            {
                                GIAODICH_CHITIET_DTO.TYLE_CHIETKHAU = CHIETKHAU;
                                GIAODICH_CHITIET_DTO.TIEN_CHIETKHAU = decimal.Round((GIAODICH_CHITIET_DTO.TYLE_CHIETKHAU / 100) * (GIAODICH_CHITIET_DTO.GIABANLE_VAT * GIAODICH_CHITIET_DTO.SOLUONG - GIAODICH_CHITIET_DTO.TIEN_KHUYENMAI), 2);
                            }
                            else
                            {
                                GIAODICH_CHITIET_DTO.TIEN_CHIETKHAU = CHIETKHAU;
                                GIAODICH_CHITIET_DTO.TYLE_CHIETKHAU = decimal.Round((100 * GIAODICH_CHITIET_DTO.TIEN_CHIETKHAU) / (GIAODICH_CHITIET_DTO.GIABANLE_VAT * GIAODICH_CHITIET_DTO.SOLUONG - GIAODICH_CHITIET_DTO.TIEN_KHUYENMAI), 2);
                            }

                            GIAODICH_CHITIET_DTO.TYLE_KHUYENMAI = TYLE_KHUYENMAI_DETAILS;
                            GIAODICH_CHITIET_DTO.TIEN_VOUCHER = 0;
                            decimal GIAVON = 0;
                            if (dataRow.Cells["GIAVON"].Value != null)
                            {
                                decimal.TryParse(dataRow.Cells["GIAVON"].Value.ToString(), out GIAVON);
                            }
                            GIAODICH_CHITIET_DTO.GIAVON = GIAVON;
                            GIAODICH_CHITIET_DTO.MATHUE_RA = _EXTEND_VATTU_DTO.MATHUE_RA;
                            GIAODICH_CHITIET_DTO.THANHTIEN_CHUA_GIAMGIA = GIAODICH_CHITIET_DTO.GIABANLE_VAT * GIAODICH_CHITIET_DTO.SOLUONG;
                            GIAODICH_CHITIET_DTO.MA_GIAODICH = GIAODICH_DTO.MA_GIAODICH;
                            GIAODICH_DTO.LST_DETAILS.Add(GIAODICH_CHITIET_DTO);
                            i++;
                        }
                        else
                        {
                            //LÀ MÃ BÓ HÀNG
                            List<EXTEND_BOHANGCHITIET_DTO> _LST_EXTEND_BOHANGCHITIET_DTO = new List<EXTEND_BOHANGCHITIET_DTO>();
                            if (Config.CheckConnectToServer())
                            {
                                _LST_EXTEND_BOHANGCHITIET_DTO = FrmXuatBanLeService.LAYDULIEU_BOHANGCHITIET_FROM_DATABASE_ORACLE(MAHANG_MABO, GIAODICH_DTO.UNITCODE);

                            }
                            else
                            {
                                _LST_EXTEND_BOHANGCHITIET_DTO = FrmXuatBanLeService.LAYDULIEU_BOHANGCHITIET_FROM_DATABASE_SQLSERVER(MAHANG_MABO, GIAODICH_DTO.UNITCODE);
                            }
                            if (_LST_EXTEND_BOHANGCHITIET_DTO.Count > 0)
                            {
                                foreach (EXTEND_BOHANGCHITIET_DTO rowBoHang in _LST_EXTEND_BOHANGCHITIET_DTO)
                                {
                                    GIAODICH_CHITIET GIAODICH_CHITIET_DTO = new GIAODICH_CHITIET();
                                    GIAODICH_CHITIET_DTO.ID = Guid.NewGuid().ToString() + "-" + i;
                                    GIAODICH_CHITIET_DTO.MAHANG = rowBoHang.MAHANG;
                                    EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
                                    if (Config.CheckConnectToServer())
                                    {
                                        _EXTEND_VATTU_DTO =
                                            FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_ORACLE(
                                                rowBoHang.MAHANG, Session.Session.CurrentUnitCode);

                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(rowBoHang.MAHANG, Session.Session.CurrentUnitCode);
                                    }
                                    GIAODICH_CHITIET_DTO.DONVITINH = _EXTEND_VATTU_DTO.DONVITINH;
                                    GIAODICH_CHITIET_DTO.TENHANG = _EXTEND_VATTU_DTO.TENHANG;
                                    GIAODICH_CHITIET_DTO.MABOPK = MAHANG_MABO;
                                    decimal SOLUONG_DETAILS = 0;
                                    if (dataRow.Cells["SOLUONG"].Value != null)
                                    {
                                        decimal.TryParse(dataRow.Cells["SOLUONG"].Value.ToString(), out SOLUONG_DETAILS);
                                    }
                                    //SỐ LƯỢNG MÃ HÀNG TRONG BÓ = SỐ LƯỢNG BÓ MUA * SỐ LƯỢNG MẶT HÀNG TRONG BÓ (DM_BOHANGCHITIET)
                                    GIAODICH_CHITIET_DTO.SOLUONG = SOLUONG_DETAILS * rowBoHang.SOLUONG;
                                    GIAODICH_CHITIET_DTO.GIABANLE_VAT = _EXTEND_VATTU_DTO.GIABANLE_VAT;
                                    GIAODICH_CHITIET_DTO.MA_KHUYENMAI = "";
                                    GIAODICH_CHITIET_DTO.TYLE_CHIETKHAU = 0;
                                    //KHUYẾN MÃI MÃ HÀNG TRONG BÓ (DM_BOHANGCHITIET)
                                    GIAODICH_CHITIET_DTO.TIEN_KHUYENMAI = SOLUONG_DETAILS * (_EXTEND_VATTU_DTO.GIABANLE_VAT * rowBoHang.SOLUONG - rowBoHang.TONGLE);
                                    if (rowBoHang.TONGLE != 0)
                                    {
                                        GIAODICH_CHITIET_DTO.TYLE_KHUYENMAI = decimal.Round((100 * (SOLUONG_DETAILS * (_EXTEND_VATTU_DTO.GIABANLE_VAT * rowBoHang.SOLUONG - rowBoHang.TONGLE))) / rowBoHang.TONGLE, 2);
                                    }
                                    else
                                    {
                                        GIAODICH_CHITIET_DTO.TYLE_KHUYENMAI = 0;
                                        string NOTIFI_WARNING = string.Format(@"CẢNH BÁO ! MÃ HÀNG '{0}' TRONG BÓ '{1}' CÓ GIÁ BÁN LẺ BẰNG 0", rowBoHang.MAHANG, MAHANG_MABO);
                                        MessageBox.Show(NOTIFI_WARNING);
                                    }

                                    decimal TIEN_CHIETKHAU = 0;
                                    if (dataRow.Cells["CHIETKHAU"].Value != null)
                                    {
                                        string CHIETKHAU_PERCENT = dataRow.Cells["CHIETKHAU"].Value.ToString().Trim();
                                        if (CHIETKHAU_PERCENT.Contains('%'))
                                        {
                                            CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out TIEN_CHIETKHAU);
                                        }
                                        else
                                        {
                                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out TIEN_CHIETKHAU);
                                        }
                                    }
                                    if (TIEN_CHIETKHAU <= 100)
                                    {
                                        GIAODICH_CHITIET_DTO.TYLE_CHIETKHAU = TIEN_CHIETKHAU;
                                        GIAODICH_CHITIET_DTO.TIEN_CHIETKHAU = decimal.Round((TIEN_CHIETKHAU / 100) * (SOLUONG_DETAILS * rowBoHang.SOLUONG * GIAODICH_CHITIET_DTO.GIABANLE_VAT - GIAODICH_CHITIET_DTO.TIEN_KHUYENMAI), 2);
                                    }
                                    else
                                    {
                                        GIAODICH_CHITIET_DTO.TIEN_CHIETKHAU = TIEN_CHIETKHAU;
                                        GIAODICH_CHITIET_DTO.TYLE_CHIETKHAU = decimal.Round((100 * TIEN_CHIETKHAU) / (SOLUONG_DETAILS * rowBoHang.SOLUONG * GIAODICH_CHITIET_DTO.GIABANLE_VAT - GIAODICH_CHITIET_DTO.TIEN_KHUYENMAI), 2);
                                    }

                                    //TÍNH TIỀN MÃ HÀNG TRONG BÓ HÀNG
                                    decimal TTIENCOVAT_DETAILS = 0;
                                    TTIENCOVAT_DETAILS = _EXTEND_VATTU_DTO.GIABANLE_VAT * rowBoHang.SOLUONG * SOLUONG_DETAILS - GIAODICH_CHITIET_DTO.TIEN_KHUYENMAI - GIAODICH_CHITIET_DTO.TIEN_CHIETKHAU;
                                    GIAODICH_CHITIET_DTO.THANHTIEN = TTIENCOVAT_DETAILS;
                                    //
                                    GIAODICH_CHITIET_DTO.TIEN_VOUCHER = 0;
                                    decimal GIAVON = 0;
                                    if (dataRow.Cells["GIAVON"].Value != null)
                                    {
                                        decimal.TryParse(dataRow.Cells["GIAVON"].Value.ToString(), out GIAVON);
                                    }
                                    GIAODICH_CHITIET_DTO.GIAVON = GIAVON;
                                    GIAODICH_CHITIET_DTO.MATHUE_RA = _EXTEND_VATTU_DTO.MATHUE_RA;
                                    GIAODICH_CHITIET_DTO.THANHTIEN_CHUA_GIAMGIA = GIAODICH_CHITIET_DTO.GIABANLE_VAT * GIAODICH_CHITIET_DTO.SOLUONG;
                                    GIAODICH_CHITIET_DTO.MA_GIAODICH = GIAODICH_DTO.MA_GIAODICH;
                                    GIAODICH_DTO.LST_DETAILS.Add(GIAODICH_CHITIET_DTO);
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("LỖI KHÔNG KHỞI TẠO ĐƯỢC DỮ LIỆU ĐỂ LƯU TRỮ VÀO HỆ THỐNG");
            }
            return GIAODICH_DTO;
        }

        private const int WM_KEYDOWN = 0x0101;
        //const int WM_KEYUP = 0x0100;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                if ((Keys)m.WParam == Keys.F2 && btnF2ThemMoi.Enabled)
                {
                    lblTongTienThanhToan.Text = "";
                    lblSumSoLuong.Text = ""; lblTongTienKhuyenMai.Text = ""; lblChietKhauLe.Text = ""; lblTongTienThanhToan.Text = "";
                    dgvDetails_Tab.Rows.Clear();
                    dgvDetails_Tab.Refresh();
                    CurrentKey = Keys.F2;
                    btnF2ThemMoi.Enabled = false;
                    btnF2ThemMoi.BackColor = Color.Gray;
                    lblMaGiaoDichQuay.Text = FrmXuatBanLeService.INIT_CODE_TRADE();
                    txtMaHang.Enabled = true;
                    txtMaHang.Focus();
                    btnF1ThanhToan.Enabled = true;
                    btnF1ThanhToan.BackColor = Color.CadetBlue;
                    btnF3GiamHang.Enabled = true;
                    btnF3GiamHang.BackColor = Color.CadetBlue;
                    btnF4TangHang.Enabled = true;
                    btnF4TangHang.BackColor = Color.CadetBlue;
                    btnF5LamMoi.Enabled = true;
                    btnF5LamMoi.BackColor = Color.CadetBlue;
                    btnSearchAll.Enabled = true;
                    btnSearchAll.BackColor = Color.CadetBlue;
                    btnF7.Enabled = true;
                    this.btnF7.BackColor = Color.CadetBlue;
                }
                if ((Keys)m.WParam == Keys.F3 && !btnF2ThemMoi.Enabled && dgvDetails_Tab.RowCount > 0)
                {
                    FlagTangHang = false;
                    if (dgvDetails_Tab.Rows.Count > 0)
                    {
                        if (CurrentKey == Keys.F4 || (CurrentKey == Keys.F2))
                        {
                            CurrentKey = Keys.F3;
                            btnStatus.Text = "- GIẢM HÀNG";
                            btnStatus.BackColor = Color.DarkOrange;
                        }
                    }
                }
                if ((Keys)m.WParam == Keys.F4 && !btnF2ThemMoi.Enabled && dgvDetails_Tab.RowCount > 0)
                {
                    FlagTangHang = true;
                    TangHangGridView();
                }
                if ((Keys)m.WParam == Keys.F5 && !btnF2ThemMoi.Enabled && dgvDetails_Tab.RowCount > 0)
                {
                    CurrentKey = Keys.F4;
                    btnStatus.Text = "+ TĂNG HÀNG";
                    btnStatus.BackColor = Color.DarkTurquoise;
                    FlagTangHang = true;
                    if (dgvDetails_Tab.Rows.Count > 0)
                    {
                        DialogResult result = MessageBox.Show("THAO TÁC NÀY SẼ XÓA TOÀN BỘ CÁC MÃ ĐANG SCAN ! BẠN CÓ CHẮC CHẮN ?", "THÔNG BÁO", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            dgvDetails_Tab.Rows.Clear();
                            TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
                            HienThiManHinhLCD("", "0", "0", "0");
                        }
                        lblChietKhauLe.Text = "0";
                        txtMaHang.Focus();
                    }
                }
                if ((Keys)m.WParam == Keys.F1 && !btnF2ThemMoi.Enabled)
                {
                    if (dgvDetails_Tab.Rows.Count > 0)
                    {
                        decimal.TryParse(lblTongTienThanhToan.Text.Trim(), out THANHTOAN_TONGTIEN_THANHTOAN);
                        THANHTOAN_MAGIAODICH = lblMaGiaoDichQuay.Text.Trim();
                        GIAODICH_DTO GIAODICH_DTO = new GIAODICH_DTO();
                        GIAODICH_DTO = KHOITAO_DULIEU_THANHTOAN_BANLE(dgvDetails_Tab);
                        GIAODICH_DTO GIAODICH_BILL = new GIAODICH_DTO();
                        GIAODICH_BILL = KHOITAO_DULIEU_BILL_BANLE(dgvDetails_Tab);
                        HienThiManHinhLCD("Tiền phải trả:", "", "", FormatCurrency.FormatMoney(GIAODICH_DTO.THANHTIEN));
                        frmThanhToan = new FrmThanhToan(GIAODICH_DTO, GIAODICH_BILL, EnumCommon.LOAI_GIAODICH.XBAN_LE, GIAODICH_DTO.THANHTIEN);
                        frmThanhToan.SetHanler(ResetState); //SỰ KIỆN KHI THANH TOÁN XONG
                        frmThanhToan.ShowDialog();
                    }
                }
                if ((Keys)m.WParam == Keys.Down && !btnF2ThemMoi.Enabled) //Sự kiện phím xuống
                {
                    int index = 0;
                    if (dgvDetails_Tab.SelectedRows[0].Index == dgvDetails_Tab.Rows.Count - 1)
                    {
                        dgvDetails_Tab.Rows[index].Selected = true;
                    }
                    else
                    {
                        index = dgvDetails_Tab.SelectedRows[0].Index + 1;
                        dgvDetails_Tab.Rows[index].Selected = true;
                    }
                }
                if ((Keys)m.WParam == Keys.Up && !btnF2ThemMoi.Enabled) //Sự kiện phím xuống
                {
                    int index = 0;
                    if (dgvDetails_Tab.SelectedRows[0].Index == 0)
                    {
                        index = dgvDetails_Tab.Rows.Count - 1;
                        dgvDetails_Tab.Rows[index].Selected = true;
                    }
                    else
                    {
                        index = dgvDetails_Tab.SelectedRows[0].Index - 1;
                        dgvDetails_Tab.Rows[index].Selected = true;
                    }
                }
                if ((Keys)m.WParam == Keys.F6 && !btnF2ThemMoi.Enabled)
                {
                    frmTimKiemVatTu frm = new frmTimKiemVatTu();
                    frm.handlerSearchVatTu(SelectedSearch);
                    frm.ShowDialog();
                }
                if ((Keys)m.WParam == Keys.F7 && btnF2ThemMoi.Enabled)
                {
                    FrmTimKiemGiaoDich frmSearch = new FrmTimKiemGiaoDich(true);
                    frmSearch.SetHandlerBill(TimKIemGiaoDichQuayBanLe);
                    frmSearch.ShowDialog();
                }
            }
            //int _currentUcFrame = FrmMain._currentUcFrame;
            return base.ProcessKeyPreview(ref m);
        }

        private void btnF1ThanhToan_Click(object sender, EventArgs e)
        {
            if (!btnF2ThemMoi.Enabled)
            {
                if (dgvDetails_Tab.Rows.Count > 0)
                {
                    decimal.TryParse(lblTongTienThanhToan.Text.Trim(), out THANHTOAN_TONGTIEN_THANHTOAN);
                    THANHTOAN_MAGIAODICH = lblMaGiaoDichQuay.Text.Trim();
                    GIAODICH_DTO GIAODICH_DTO = new GIAODICH_DTO();
                    GIAODICH_DTO = KHOITAO_DULIEU_THANHTOAN_BANLE(dgvDetails_Tab);
                    GIAODICH_DTO NVGDQUAY_ASYNCCLIENT_BILL = new GIAODICH_DTO();
                    NVGDQUAY_ASYNCCLIENT_BILL = KHOITAO_DULIEU_BILL_BANLE(dgvDetails_Tab);
                    frmThanhToan = new FrmThanhToan(GIAODICH_DTO, NVGDQUAY_ASYNCCLIENT_BILL, EnumCommon.LOAI_GIAODICH.XBAN_LE, GIAODICH_DTO.THANHTIEN);
                    frmThanhToan.SetHanler(ResetState); //Set sự kiện khi đóng form Thanh toán 
                    frmThanhToan.ShowDialog();
                }
            }
        }
        /// <summary>
        /// Set lại các thuộc tính khi thanh toán xong 
        /// </summary>
        /// <param name="state"></param>
        public void ResetState(bool state) //set lại trạng thái trước khi thanh toán 
        {
            if (state)
            {
                btnF1ThanhToan.Enabled = false;
                btnF1ThanhToan.BackColor = Color.Gray;
                btnF1ThanhToan.ForeColor = Color.Black;
                btnF2ThemMoi.Enabled = true;
                btnF2ThemMoi.BackColor = Color.CadetBlue;
                btnF2ThemMoi.Text = "F2-> Thêm mới";
                btnF3GiamHang.Enabled = false;
                btnF3GiamHang.BackColor = Color.Gray;
                btnF3GiamHang.ForeColor = Color.Black;
                btnF4TangHang.Enabled = false;
                btnF4TangHang.BackColor = Color.Gray;
                btnF4TangHang.ForeColor = Color.Black;
                btnF5LamMoi.Enabled = false;
                btnF5LamMoi.BackColor = Color.Gray;
                btnF5LamMoi.ForeColor = Color.Black;
                btnSearchAll.Enabled = false;
                btnSearchAll.BackColor = Color.Gray;
                btnSearchAll.ForeColor = Color.Black;
                CurrentKey = Keys.F4;
                btnStatus.Text = "+ TĂNG HÀNG";
                btnStatus.BackColor = Color.DarkTurquoise;
                btnF7.BackColor = Color.CadetBlue;
                btnF1ThanhToan.ForeColor = Color.White;
                FlagTangHang = true;
            }
        }
        private void dgvDetails_Tab_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDetails_Tab.Rows.Count > 1)
            {
                decimal START_SOLUONG = 0;
                decimal.TryParse(dgvDetails_Tab.CurrentRow.Cells["SOLUONG"].Value.ToString(), out START_SOLUONG);
                txtSoLuong.Text = START_SOLUONG.ToString();
            }
            else
            {
                btnStatus.Text = "+ TĂNG HÀNG";
                btnStatus.BackColor = Color.DarkTurquoise;
                FlagTangHang = true;
            }
        }
        private void cbbLoaiGiaoDich_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = this.cbbLoaiGiaoDich.GetItemText(this.cbbLoaiGiaoDich.SelectedItem);
            int index = 0;
            if (selected.Equals("Bán lẻ"))
            {
                index = (int)EnumCommon.METHOD_PRICE.GIABANLE_VAT;
                Session.Session.CurrentLoaiGiaoDich = "BANLE";
            }
            else
            {
                index = (int)EnumCommon.METHOD_PRICE.GIABANBUON_VAT;
                if (selected.Equals("Bán buôn"))
                {
                    Session.Session.CurrentLoaiGiaoDich = "BANBUON";
                }
                else
                {
                    Session.Session.CurrentLoaiGiaoDich = "GIAVON";
                }
            }
            MethodPrice = index;
            LOAIGIAODICH = index;
            if (MethodPrice == (int)EnumCommon.METHOD_PRICE.GIABANLE_VAT) // Trường hợp bán lẻ
            {
                dgvDetails_Tab.Columns["GIAVON"].Visible = false;
            }
            else if (MethodPrice == (int)EnumCommon.METHOD_PRICE.GIABANBUON_VAT)
            {
                dgvDetails_Tab.Columns["GIAVON"].Visible = true;

            }
        }

        private void GHI_DULIEU_RA_GRIDVIEW()
        {
            List<VATTU_DTO> listData = new List<VATTU_DTO>();
            decimal SoLuong = 1;
            string MaVatTu = txtMaHang.Text.Trim().ToUpper();
            if (Config.CheckConnectToServer()) //Nếu có mạng lan
            {
                
                listData = FrmXuatBanLeService.GET_DATA_VATTU_FROM_CSDL_ORACLE(MaVatTu, (EnumCommon.METHOD_PRICE) MethodPrice, SoLuong);
                txtSoLuong.Focus();
                StatusConnect.Text = "TRẠNG THÁI BÁN: ONLINE";
                StatusConnect.ForeColor = Color.ForestGreen;
            }
            else
            {
                listData = FrmXuatBanLeService.GET_DATA_VATTU_FROM_CSDL_SQLSERVER(MaVatTu, (EnumCommon.METHOD_PRICE) MethodPrice, SoLuong);
                txtSoLuong.Focus(); //Bán SQL
                StatusConnect.Text = "TRẠNG THÁI BÁN: OFFLINE";
                StatusConnect.ForeColor = Color.Red;
            }
            if (listData.Count > 1)
            {
                listData_TrungMa = listData;
                frmSelect = new frmSelectVatTu(listData);
                frmSelect.setHandler(INSERT_VATTU_GRIDVIEW);
                frmSelect.ShowDialog();
            }
            else if (listData.Count == 1)
            {
                if (listData[0].GIABANLE_VAT == 0)
                {
                    string NOTIFICATION_WARNING = string.Format(@"CẢNH BÁO MÃ '{0}' CÓ GIÁ BÁN BẰNG 0 ! KHÔNG THỂ BÁN", listData[0].MAHANG);
                    MessageBox.Show(NOTIFICATION_WARNING);
                    return;
                }
                else
                {
                    if (Config.CheckConnectToServer() && listData[0].TONCUOIKYSL <= 0)
                    {
                        if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                        {
                            NotificationLauncher.ShowNotificationWarning("Thông báo", "Hết hàng trong kho ! Không thể bán", 1, "0x1", "0x8", "normal");
                            return;
                        }
                    }
                    else
                    {
                        if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_SQLSERVER() == 1)
                        {
                            NotificationLauncher.ShowNotificationWarning("Thông báo", "Hết hàng trong kho ! Không thể bán", 1, "0x1", "0x8", "normal");
                            return;
                        }
                    }
                    INSERT_DULIEU_DATAGRIDVIEW(listData[0]);
                    if (listData[0].LAMACAN)
                    {
                        CURRENT_SOLUONG_MACAN = listData[0].SOLUONG;
                    }
                    txtSoLuong.Text = SoLuong.ToString();
                }
                HienThiManHinhLCD(listData[0].TENHANG, FormatCurrency.FormatMoney(SoLuong), FormatCurrency.FormatMoney(listData[0].GIABANLE_VAT), FormatCurrency.FormatMoney(SoLuong * listData[0].GIABANLE_VAT));
            }
            else
            {
                //string NOTIFICATION_WARNING = string.Format(@"THÔNG BÁO ! KHÔNG TÌM THẤY HÀNG HÓA, VẬT TƯ '{0}'", MaVatTu);
                //MessageBox.Show(NOTIFICATION_WARNING);
                if (txtMaHang.Text != "" && txtMaHang.Text.Length > 2)
                {
                    frmTimKiemVatTu frm = new frmTimKiemVatTu(txtMaHang.Text.Trim().ToUpper());
                    frm.handlerSearchVatTu(SelectedSearch);
                    frm.ShowDialog();
                }
                txtMaHang.Text = "";
            }
            if (this.dgvDetails_Tab.Rows.Count > 0)
            {
                txtSoLuong.Enabled = true;
                lblTongTienKhuyenMai.Enabled = true;
                lblSumSoLuong.Enabled = true;
                lblTongTienThanhToan.Enabled = true;
                txtChietKhauToanDon.Enabled = true;
                this.dgvDetails_Tab.Sort(this.dgvDetails_Tab.Columns["STT"], ListSortDirection.Descending);
                this.dgvDetails_Tab.ClearSelection();
                this.dgvDetails_Tab.Rows[0].Selected = true;
            }
            TINHTOAN_TONGTIEN_TOANHOADON(this.dgvDetails_Tab);
        }

        private void txtMaHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (!btnF2ThemMoi.Enabled)
                {
                    if (FlagTangHang)
                    {
                        GHI_DULIEU_RA_GRIDVIEW();
                        txtMaHang.Text = string.Empty;
                        txtMaHang.Focus();
                    }
                    else
                    {
                        int indexExist = CHECK_ROW_EXIST_DATAGRIDVIEW(dgvDetails_Tab, txtMaHang.Text.Trim());
                        if (indexExist >= 0)
                        {
                            dgvDetails_Tab.Rows[indexExist].Selected = true;
                            GiamHangGridView();
                            txtMaHang.Text = string.Empty;
                            txtMaHang.Focus();
                        }
                        else
                        {
                            NotificationLauncher.ShowNotificationError("CHÚ Ý", "ĐANG GIẢM HÀNG", 1, "0x1", "0x8", "normal");
                        }
                    }
                }
                else
                {
                    NotificationLauncher.ShowNotification("Thông báo", "Nhấn F2 thêm mới giao dịch", 1, "0x1", "0x8", "normal");
                }
            }
        }
        private void txtMaHang_Validating(object sender, CancelEventArgs e)
        {
            txtSoLuong.SelectAll();
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            frmTimKiemVatTu frm = new frmTimKiemVatTu();
            frm.handlerSearchVatTu(SelectedSearch);
            frm.ShowDialog();
        }

        public void SelectedSearch(string maHang)
        {
            txtMaHang.Text = maHang;
            GHI_DULIEU_RA_GRIDVIEW();
            txtMaHang.Text = "";
        }

        private void dgvDetails_Tab_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dgvDetails_Tab.SelectedRows[0].Index == dgvDetails_Tab.Rows.Count)
            {

            }
        }

        private void txtMaHang_TextChanged(object sender, EventArgs e)
        {
            txtMaHang.Focus();
        }

        private void txtSoLuong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    decimal CURENT_ROW_TONCUOIKYSL = 0;
                    decimal CURRENT_ROW_TIENKM = 0;
                    string CURENT_ROW_MAHANG = "";
                    decimal.TryParse(dgvDetails_Tab.SelectedRows[0].Cells["TONCUOIKYSL"].Value.ToString(), out CURENT_ROW_TONCUOIKYSL);
                    if (dgvDetails_Tab.SelectedRows[0].Cells["MAHANG"] != null)
                    {
                        CURENT_ROW_MAHANG = dgvDetails_Tab.SelectedRows[0].Cells["MAHANG"].Value.ToString();
                        VATTU_DTO.CAL_KHUYENMAI_OBJ KHUYENMAI = new VATTU_DTO.CAL_KHUYENMAI_OBJ();
                        KHUYENMAI = FrmXuatBanLeService.TINHTOAN_KHUYENMAI(CURENT_ROW_MAHANG, (EnumCommon.METHOD_PRICE)MethodPrice);
                        if (KHUYENMAI != null)
                        {
                            CURRENT_ROW_TIENKM = KHUYENMAI.GIATRI_KHUYENMAI;
                        }
                    }
                    decimal SUM_TONGCHIETKHAU_LE = 0;

                    decimal CURENT_ROW_SOLUONG = 1;
                    decimal CHIETKHAU = 0;
                    decimal CURRENT_ROW_TTIENCOVAT = 0;
                    decimal CURRENT_ROW_GIABANLE_VAT = 0;
                    decimal.TryParse(txtSoLuong.Text.Trim(), out CURENT_ROW_SOLUONG);
                    if (CURENT_ROW_SOLUONG > CURENT_ROW_TONCUOIKYSL)
                    {
                        if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                        {
                            NotificationLauncher.ShowNotificationWarning("Thông báo", "Chỉ còn " + CURENT_ROW_TONCUOIKYSL + " sản phẩm trong kho", 1, "0x1", "0x8", "normal");
                            CURENT_ROW_SOLUONG = CURENT_ROW_TONCUOIKYSL;
                            txtSoLuong.Text = CURENT_ROW_TONCUOIKYSL.ToString();
                        }
                    }
                    dgvDetails_Tab.SelectedRows[0].Cells["SOLUONG"].Value = CURENT_ROW_SOLUONG.ToString();
                    decimal.TryParse(dgvDetails_Tab.SelectedRows[0].Cells["THANHTIEN"].Value.ToString(), out CURRENT_ROW_TTIENCOVAT);
                    decimal.TryParse(dgvDetails_Tab.SelectedRows[0].Cells["GIABANLE_VAT"].Value.ToString(), out CURRENT_ROW_GIABANLE_VAT);
                    string CHIETKHAU_PERCENT = dgvDetails_Tab.SelectedRows[0].Cells["CHIETKHAU"].Value.ToString().Trim();
                    if (CHIETKHAU_PERCENT.Contains('%'))
                    {
                        CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                    }
                    decimal.TryParse(CHIETKHAU_PERCENT, out CHIETKHAU);
                    //decimal.TryParse(dgvDetails_Tab.SelectedRows[0].Cells["TIENKM"].Value.ToString(), out CURRENT_ROW_TIENKM);
                    //Lưu lại số lượng vào trong lst vattu
                    if (CURRENT_ROW_TIENKM == 0)
                    {
                        if (CHIETKHAU <= 100)
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["THANHTIEN"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLE_VAT - CURRENT_ROW_TIENKM -
                                                           CHIETKHAU * CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLE_VAT / 100);
                        }
                        else
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["THANHTIEN"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLE_VAT - CURRENT_ROW_TIENKM -
                                                           CHIETKHAU * CURENT_ROW_SOLUONG);
                        }
                    }
                    else if (CURRENT_ROW_TIENKM < 100)
                    {
                        decimal SOTIEN_KHUYENMAI = (CURENT_ROW_SOLUONG * CURRENT_ROW_TIENKM * CURRENT_ROW_GIABANLE_VAT) / 100;
                        dgvDetails_Tab.SelectedRows[0].Cells["TIENKM"].Value = FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                        if (CHIETKHAU <= 100)
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["THANHTIEN"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLE_VAT - SOTIEN_KHUYENMAI -
                                                           CHIETKHAU * CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLE_VAT / 100);
                        }
                        else
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["THANHTIEN"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLE_VAT - SOTIEN_KHUYENMAI -
                                                           CHIETKHAU * CURENT_ROW_SOLUONG);
                        }
                    }
                    else
                    {
                        decimal SOTIEN_KHUYENMAI = 0;
                        SOTIEN_KHUYENMAI = CURRENT_ROW_TIENKM * CURENT_ROW_SOLUONG;
                        dgvDetails_Tab.SelectedRows[0].Cells["TIENKM"].Value =
                            FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                        if (CHIETKHAU <= 100)
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["THANHTIEN"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLE_VAT - SOTIEN_KHUYENMAI - CHIETKHAU * CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLE_VAT / 100);
                        }
                        else
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["THANHTIEN"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLE_VAT - SOTIEN_KHUYENMAI -
                                                           CHIETKHAU * CURENT_ROW_SOLUONG);
                        }
                    }
                    lblChietKhauLe.Text = FormatCurrency.FormatMoney(SUM_TONGCHIETKHAU_LE);
                    TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
                    HienThiManHinhLCD(dgvDetails_Tab.SelectedRows[0].Cells["TENHANG"].Value.ToString(), FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG), FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLE_VAT), FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLE_VAT));
                    txtMaHang.Focus();
                }
                catch (Exception)
                {
                    NotificationLauncher.ShowNotification("Thông báo", "Nhập sai số lượng", 1, "0x1", "0x8", "normal");
                }
            }
        }

        /// <summary>
        /// Sửa chiết khấu trên gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private bool flagChangeCk = false;
        private void dgvDetails_Tab_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dgvDetails_Tab.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name;
            if (columnName.Equals("CHIETKHAU") && e.RowIndex >= 0 && !flagChangeCk)
            {
                flagChangeCk = true;
                SUM_TONGTIEN_CHIETKHAU_LE(dgvDetails_Tab);
            }
            TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
            flagChangeCk = false;
            lblChietKhauLe.Enabled = true;
            txtMaHang.Focus();
        }

        /// <summary>
        /// Sự kiện giảm hàng
        /// </summary>
        private void GiamHangGridView()
        {
            if (CurrentKey == Keys.F4 || (CurrentKey == Keys.F2))
            {
                CurrentKey = Keys.F3;
                btnStatus.Text = "- GIẢM HÀNG";
                btnStatus.BackColor = Color.DarkOrange;
            }
            else
            {
                decimal SUM_TONGCHIETKHAU_LE = 0;
                if (dgvDetails_Tab.Rows.Count > 0)
                {
                    decimal CURRENT_ROW_SOLUONG_NEW = 0;
                    decimal CURRENT_ROW_SOLUONG,
                        CURRENT_ROW_GIABANLE_VAT,
                        CURRENT_TIEN_KHUYENMAI_NEW,
                        CURRENT_ROW_GIATRI,
                        CHIETKHAU = 0;
                    int rowIndex = dgvDetails_Tab.SelectedRows[0].Index;
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["GIABANLE_VAT"].Value.ToString(),
                        out CURRENT_ROW_GIABANLE_VAT);
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["SOLUONG"].Value.ToString(),
                        out CURRENT_ROW_SOLUONG);
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["TIENKM"].Value.ToString(),
                        out CURRENT_ROW_GIATRI);
                    if (dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value != null)
                    {
                        string CHIETKHAU_PERCENT = dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value.ToString().Trim();
                        if (CHIETKHAU_PERCENT.Contains('%'))
                        {
                            CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out CHIETKHAU);
                        }
                    }
                    else
                    {
                        dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value = CHIETKHAU;
                    }
                    CurrentKey = Keys.F3;
                    btnStatus.Text = "- GIẢM HÀNG";
                    btnStatus.BackColor = Color.DarkOrange;
                    if (dgvDetails_Tab.Rows[rowIndex].Cells["LAMACAN"] != null)
                    {
                        string laMaCan = dgvDetails_Tab.Rows[rowIndex].Cells["LAMACAN"].Value.ToString();
                        if (laMaCan.Equals("True")) CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG - CURRENT_SOLUONG_MACAN;
                        else
                        {
                            CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG - 1;
                        }
                    }
                    else
                    {
                        CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG - 1;
                    }

                    if (CURRENT_ROW_SOLUONG_NEW > 0)
                    {
                        CURRENT_TIEN_KHUYENMAI_NEW = (CURRENT_ROW_GIATRI / CURRENT_ROW_SOLUONG) * CURRENT_ROW_SOLUONG_NEW;
                        dgvDetails_Tab.Rows[rowIndex].Cells["TIENKM"].Value =
                            FormatCurrency.FormatMoney(CURRENT_TIEN_KHUYENMAI_NEW);
                        dgvDetails_Tab.Rows[rowIndex].Cells["SOLUONG"].Value =
                            FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW);
                        if (CHIETKHAU <= 100)
                        {
                            dgvDetails_Tab.Rows[rowIndex].Cells["THANHTIEN"].Value =
                                FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLE_VAT * CURRENT_ROW_SOLUONG_NEW -
                                                           CURRENT_TIEN_KHUYENMAI_NEW -
                                                           CHIETKHAU * CURRENT_ROW_SOLUONG_NEW * CURRENT_ROW_GIABANLE_VAT /
                                                           100);
                        }
                        else
                        {
                            dgvDetails_Tab.Rows[rowIndex].Cells["THANHTIEN"].Value =
                                FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLE_VAT * CURRENT_ROW_SOLUONG_NEW -
                                                           CURRENT_TIEN_KHUYENMAI_NEW - CHIETKHAU * CURRENT_ROW_SOLUONG_NEW);
                            dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value = CHIETKHAU * CURRENT_ROW_SOLUONG_NEW;
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Bạn muốn xóa mã này?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            int maxIndex = int.Parse(dgvDetails_Tab.Rows[0].Cells["STT"].Value.ToString());
                            string maVatTu = dgvDetails_Tab.Rows[rowIndex].Cells["MAHANG"].Value.ToString();
                            dgvDetails_Tab.Rows.RemoveAt(rowIndex);
                            if (rowIndex > 0)
                            {
                                for (int i = 0; i < rowIndex; i++) // đánh lại số thứ tự trên dgv.
                                {
                                    maxIndex = maxIndex - 1;
                                    dgvDetails_Tab.Rows[i].Cells[0].Value = maxIndex;
                                }
                            }
                        }
                        else if (result == DialogResult.No)
                        {
                            //code for No
                        }
                        else if (result == DialogResult.Cancel)
                        {
                            //code for Cancel
                        }
                        if (this.dgvDetails_Tab.Rows.Count > 0)
                        {
                            this.dgvDetails_Tab.Sort(this.dgvDetails_Tab.Columns["STT"], ListSortDirection.Descending);
                            this.dgvDetails_Tab.ClearSelection();
                            this.dgvDetails_Tab.Rows[0].Selected = true;
                        }
                    }
                    txtSoLuong.Text = CURRENT_ROW_SOLUONG_NEW.ToString();
                    if (this.dgvDetails_Tab.Rows.Count > 0)
                    {
                        HienThiManHinhLCD(dgvDetails_Tab.CurrentRow.Cells["TENHANG"].Value.ToString(), FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW), FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLE_VAT), FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW * CURRENT_ROW_GIABANLE_VAT));
                    }
                    else
                    {
                        HienThiManHinhLCD("", "0", "0", "0");
                    }
                    txtMaHang.Text = "";
                }
                TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
                lblChietKhauLe.Text = FormatCurrency.FormatMoney(SUM_TONGCHIETKHAU_LE);
            }
        }


        /// <summary>
        /// Sự kiện tăng hàng
        /// </summary>
        private void TangHangGridView()
        {
            if (dgvDetails_Tab.Rows.Count > 0)
            {
                if (CurrentKey == Keys.F3 || (CurrentKey == Keys.F2))
                {
                    CurrentKey = Keys.F4;
                    btnStatus.Text = "+ TĂNG HÀNG";
                    btnStatus.BackColor = Color.DarkTurquoise;
                }
                else
                {
                    decimal SUM_TONGCHIETKHAU_LE = 0;
                    decimal CURRENT_ROW_SOLUONG_NEW = 0;
                    decimal CURRENT_TONCUOIKYSL = 0;
                    decimal CURRENT_ROW_SOLUONG,
                        CURRENT_ROW_GIABANLE_VAT,
                        CURRENT_TYLE_KHUYENMAI_NEW,
                        CURRENT_TIEN_KHUYENMAI_NEW,
                        CURRENT_ROW_GIATRI,
                        CHIETKHAU = 0;
                    int rowIndex = dgvDetails_Tab.SelectedRows[0].Index;
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["GIABANLE_VAT"].Value.ToString(),
                        out CURRENT_ROW_GIABANLE_VAT);
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["SOLUONG"].Value.ToString(),
                        out CURRENT_ROW_SOLUONG);
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["TIENKM"].Value.ToString(),
                        out CURRENT_ROW_GIATRI);
                    if (dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value != null)
                    {
                        string CHIETKHAU_PERCENT = dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value.ToString().Trim();
                        if (CHIETKHAU_PERCENT.Contains('%'))
                        {
                            CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out CHIETKHAU);
                        }
                    }
                    else
                    {
                        dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value = CHIETKHAU;
                    }
                    CurrentKey = Keys.F4;
                    btnStatus.Text = "+ TĂNG HÀNG";
                    btnStatus.BackColor = Color.DarkTurquoise;
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["TONCUOIKYSL"].Value.ToString(), out CURRENT_TONCUOIKYSL);
                    if (dgvDetails_Tab.Rows[rowIndex].Cells["LAMACAN"] != null)
                    {
                        string laMaCan = dgvDetails_Tab.Rows[rowIndex].Cells["LAMACAN"].Value.ToString();
                        if (laMaCan.Equals("True"))
                        {
                            if (CURRENT_ROW_SOLUONG + CURRENT_SOLUONG_MACAN > CURRENT_TONCUOIKYSL)
                            {
                                if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                                {
                                    NotificationLauncher.ShowNotificationWarning("Thông báo", "Hết hàng trong kho ! Không thể bán", 1, "0x1", "0x8", "normal");
                                    return;
                                }
                            }
                            CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG + CURRENT_SOLUONG_MACAN;
                        }
                        else
                        {
                            if (CURRENT_ROW_SOLUONG + 1 > CURRENT_TONCUOIKYSL)
                            {
                                if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                                {
                                    NotificationLauncher.ShowNotificationWarning("Thông báo", "Hết hàng trong kho ! Không thể bán", 1, "0x1", "0x8", "normal");
                                    return;
                                }
                            }
                            CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG + 1;
                        }
                    }
                    else
                    {
                        if (CURRENT_ROW_SOLUONG + 1 > CURRENT_TONCUOIKYSL)
                        {
                            if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                            {
                                NotificationLauncher.ShowNotificationWarning("Thông báo", "Hết hàng trong kho ! Không thể bán", 1, "0x1", "0x8", "normal");
                                return;
                            }
                        }
                        CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG + 1;
                    }
                    dgvDetails_Tab.Rows[rowIndex].Cells["SOLUONG"].Value = FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW);
                    CURRENT_TYLE_KHUYENMAI_NEW = 100 * ((CURRENT_ROW_GIATRI / CURRENT_ROW_SOLUONG) * CURRENT_ROW_SOLUONG_NEW) / (CURRENT_ROW_GIABANLE_VAT * CURRENT_ROW_SOLUONG_NEW);
                    CURRENT_TIEN_KHUYENMAI_NEW = (CURRENT_ROW_GIATRI / CURRENT_ROW_SOLUONG) * CURRENT_ROW_SOLUONG_NEW;
                    dgvDetails_Tab.Rows[rowIndex].Cells["TIENKM"].Value = FormatCurrency.FormatMoney(CURRENT_TIEN_KHUYENMAI_NEW);
                    if (CHIETKHAU <= 100)
                    {
                        dgvDetails_Tab.Rows[rowIndex].Cells["THANHTIEN"].Value =
                            FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLE_VAT * CURRENT_ROW_SOLUONG_NEW - CURRENT_TIEN_KHUYENMAI_NEW - CHIETKHAU * CURRENT_ROW_SOLUONG_NEW * CURRENT_ROW_GIABANLE_VAT / 100);
                    }
                    else
                    {
                        dgvDetails_Tab.Rows[rowIndex].Cells["THANHTIEN"].Value =
                            FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLE_VAT * CURRENT_ROW_SOLUONG_NEW -
                                                       CURRENT_TIEN_KHUYENMAI_NEW - CHIETKHAU * CURRENT_ROW_SOLUONG_NEW);
                        dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value = CHIETKHAU * CURRENT_ROW_SOLUONG_NEW;
                    }
                    TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
                    if (this.dgvDetails_Tab.Rows.Count > 0)
                    {
                        HienThiManHinhLCD(dgvDetails_Tab.Rows[rowIndex].Cells["TENHANG"].Value.ToString(), FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW), FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLE_VAT), FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW * CURRENT_ROW_GIABANLE_VAT));
                    }
                    else
                    {
                        HienThiManHinhLCD("", "0", "0", "0");
                    }
                    lblChietKhauLe.Text = FormatCurrency.FormatMoney(SUM_TONGCHIETKHAU_LE);
                    txtSoLuong.Text = CURRENT_ROW_SOLUONG_NEW.ToString();
                }
            }
        }

        private void txtChietKhauToanDon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                decimal TONGTIEN_SAUCT = 0;
                decimal TONGTIEN_HOADON = 0;
                decimal CHIETKHAU_TOANDON = 0;
                decimal.TryParse(lblTongTienThanhToan.Text.ToString(), out TONGTIEN_HOADON);
                decimal.TryParse(txtChietKhauToanDon.Text.ToString(), out CHIETKHAU_TOANDON);
                if (TONGTIEN_HOADON > 0 && dgvDetails_Tab.RowCount > 0)
                {
                    decimal SUM_TONGCHIETKHAU_LE = 0;
                    if (CHIETKHAU_TOANDON <= 100) //Phần trăm chiết khấu
                    {
                        TONGTIEN_SAUCT = TONGTIEN_HOADON - TONGTIEN_HOADON * CHIETKHAU_TOANDON / 100;
                        lblTongTienThanhToan.Text = FormatCurrency.FormatMoney(TONGTIEN_SAUCT);
                        if (dgvDetails_Tab.RowCount > 0)
                        {
                            foreach (DataGridViewRow rowData in dgvDetails_Tab.Rows)
                            {
                                decimal CURRENT_SOLUONG = 0; decimal.TryParse(rowData.Cells["SOLUONG"].Value.ToString(), out CURRENT_SOLUONG);
                                decimal CURRENT_GIABANLECOVAT = 0; decimal.TryParse(rowData.Cells["GIABANLE_VAT"].Value.ToString(), out CURRENT_GIABANLECOVAT);
                                decimal CURRENT_TIENKM = 0; decimal.TryParse(rowData.Cells["TIENKM"].Value.ToString(), out CURRENT_TIENKM);
                                decimal TYLE_CHIETKHAU_TOANDON = decimal.Round(CHIETKHAU_TOANDON / 100, 2);
                                decimal TIEN_CHIETKHAU_TOANDON = TYLE_CHIETKHAU_TOANDON * (CURRENT_SOLUONG * CURRENT_GIABANLECOVAT - CURRENT_TIENKM);
                                decimal TIEN_TTIENCOVAT = (CURRENT_SOLUONG * CURRENT_GIABANLECOVAT - CURRENT_TIENKM) * (1 - TYLE_CHIETKHAU_TOANDON);
                                rowData.Cells["CHIETKHAU"].Value = (TYLE_CHIETKHAU_TOANDON * 100) + "%";
                                rowData.Cells["THANHTIEN"].Value = FormatCurrency.FormatMoney(TIEN_TTIENCOVAT);
                                SUM_TONGCHIETKHAU_LE += TIEN_CHIETKHAU_TOANDON;
                            }
                        }
                    }
                    else
                    {
                        if (dgvDetails_Tab.RowCount > 0)
                        {
                            foreach (DataGridViewRow rowData in dgvDetails_Tab.Rows)
                            {
                                decimal CURRENT_SOLUONG = 0; decimal.TryParse(rowData.Cells["SOLUONG"].Value.ToString(), out CURRENT_SOLUONG);
                                decimal CURRENT_GIABANLECOVAT = 0; decimal.TryParse(rowData.Cells["GIABANLE_VAT"].Value.ToString(), out CURRENT_GIABANLECOVAT);
                                decimal CURRENT_TIENKM = 0; decimal.TryParse(rowData.Cells["TIENKM"].Value.ToString(), out CURRENT_TIENKM);
                                decimal TYLE_CHIETKHAU_TOANDON = decimal.Round(CHIETKHAU_TOANDON / TONGTIEN_HOADON, 2);
                                decimal TIEN_CHIETKHAU_TOANDON = CHIETKHAU_TOANDON;
                                decimal TIEN_TTIENCOVAT = (CURRENT_SOLUONG * CURRENT_GIABANLECOVAT - CURRENT_TIENKM) * (1 - TYLE_CHIETKHAU_TOANDON);
                                rowData.Cells["CHIETKHAU"].Value = (TYLE_CHIETKHAU_TOANDON * 100) + "%";
                                rowData.Cells["THANHTIEN"].Value = FormatCurrency.FormatMoney(TIEN_TTIENCOVAT);
                                SUM_TONGCHIETKHAU_LE += TIEN_CHIETKHAU_TOANDON;
                            }
                        }
                    }
                    lblChietKhauLe.Text = FormatCurrency.FormatMoney(SUM_TONGCHIETKHAU_LE);
                }
            }
            TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
        }

        private void btnF2ThemMoi_Click(object sender, EventArgs e)
        {
            lblTongTienThanhToan.Text = "";
            lblTongTienKhuyenMai.Text = "";
            lblSumSoLuong.Text = "";
            dgvDetails_Tab.Rows.Clear();
            dgvDetails_Tab.Refresh();
            CurrentKey = Keys.F2;
            btnF2ThemMoi.Enabled = false;
            btnF2ThemMoi.BackColor = Color.Gray;
            lblMaGiaoDichQuay.Text = FrmXuatBanLeService.INIT_CODE_TRADE();
            txtMaHang.Enabled = true;
            txtMaHang.Focus();
            btnF1ThanhToan.Enabled = true;
            btnF1ThanhToan.BackColor = Color.CadetBlue;
            btnF3GiamHang.Enabled = true;
            btnF3GiamHang.BackColor = Color.CadetBlue;
            btnF4TangHang.Enabled = true;
            btnF4TangHang.BackColor = Color.CadetBlue;
            btnF5LamMoi.Enabled = true;
            btnF5LamMoi.BackColor = Color.CadetBlue;
            btnSearchAll.Enabled = true;
            btnSearchAll.BackColor = Color.CadetBlue;
        }

        private void btnF3GiamHang_Click(object sender, EventArgs e)
        {
            if (!btnF2ThemMoi.Enabled && dgvDetails_Tab.RowCount > 0)
            {
                FlagTangHang = false;
                GiamHangGridView();
            }
        }

        private void btnF4TangHang_Click(object sender, EventArgs e)
        {
            if (!btnF2ThemMoi.Enabled && dgvDetails_Tab.RowCount > 0)
            {
                FlagTangHang = true;
                TangHangGridView();
            }
        }

        private void btnF5LamMoi_Click(object sender, EventArgs e)
        {
            CurrentKey = Keys.F4;
            btnStatus.Text = "+ TĂNG HÀNG";
            btnStatus.BackColor = Color.DarkTurquoise;
            FlagTangHang = true;
            if (dgvDetails_Tab.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Thao tác này sẽ xóa toàn bộ mã hàng đang scan ! Bạn có chắc chắn ?", "Warning",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    dgvDetails_Tab.Rows.Clear();
                    HienThiManHinhLCD("", "0", "0", "0");
                    TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
                }
                else if (result == DialogResult.No)
                {
                    //code for No
                }
                else if (result == DialogResult.Cancel)
                {
                    //code for Cancel
                }
                lblChietKhauLe.Text = "0";
                txtMaHang.Focus();
            }
        }

        private void btnSearchAll_Click_1(object sender, EventArgs e)
        {
            frmTimKiemVatTu frm = new frmTimKiemVatTu();
            frm.handlerSearchVatTu(SelectedSearch);
            frm.ShowDialog();
        }

        private void btnF7_Click(object sender, EventArgs e)
        {
            frmSearch = new FrmTimKiemGiaoDich(true);
            frmSearch.SetHandlerBill(TimKIemGiaoDichQuayBanLe);
            frmSearch.ShowDialog();
        }
        public GIAODICH_DTO KHOITAO_DULIEU_INLAI_HOADON_FROM_ORACLE(string MAGIAODICHQUAYPK)
        {
            GIAODICH_DTO _DATA_INLAI_HOADON = new GIAODICH_DTO();
            OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string KieuThanhToan = null;
                    OracleCommand cmdParent = new OracleCommand();
                    cmdParent.Connection = connection;
                    cmdParent.CommandText = string.Format(@"SELECT * FROM NVGDQUAY_ASYNCCLIENT WHERE MAGIAODICHQUAYPK = :MAGIAODICHQUAYPK AND UNITCODE = :UNITCODE");
                    cmdParent.CommandType = CommandType.Text;
                    cmdParent.Parameters.Add("MAGIAODICHQUAYPK", OracleDbType.NVarchar2, 50).Value = MAGIAODICHQUAYPK.Trim();
                    cmdParent.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                    OracleDataReader dataReaderParent = null;
                    dataReaderParent = cmdParent.ExecuteReader();
                    if (dataReaderParent.HasRows)
                    {
                        while (dataReaderParent.Read())
                        {
                            _DATA_INLAI_HOADON.MA_GIAODICH = dataReaderParent["MA_GIAODICH"].ToString();
                            if (dataReaderParent["NGAYTAO"] != null)
                            {
                                string NGAYTAO = dataReaderParent["I_CREATE_DATE"].ToString();
                                _DATA_INLAI_HOADON.I_CREATE_DATE = DateTime.Parse(NGAYTAO);
                            }
                            else
                            {
                                _DATA_INLAI_HOADON.I_CREATE_DATE = DateTime.Now;
                            }
                            if (dataReaderParent["NGAYPHATSINH"] != null)
                            {
                                string NGAYPHATSINH = dataReaderParent["NGAYPHATSINH"].ToString();
                                _DATA_INLAI_HOADON.NGAY_GIAODICH = DateTime.Parse(NGAYPHATSINH);
                            }
                            else
                            {
                                _DATA_INLAI_HOADON.NGAY_GIAODICH = DateTime.Now;
                            }
                            _DATA_INLAI_HOADON.I_CREATE_BY = dataReaderParent["I_CREATE_BY"] != null ? dataReaderParent["I_CREATE_BY"].ToString() : "";
                            decimal TIENKHACH_TRA = 0; decimal.TryParse(dataReaderParent["TIENKHACH_TRA"].ToString(), out TIENKHACH_TRA);
                            _DATA_INLAI_HOADON.TIENKHACH_TRA = TIENKHACH_TRA;
                            decimal TIEN_TRALAI_KHACH = 0; decimal.TryParse(dataReaderParent["TIEN_TRALAI_KHACH"].ToString(), out TIEN_TRALAI_KHACH);
                            _DATA_INLAI_HOADON.TIEN_TRALAI_KHACH = TIEN_TRALAI_KHACH;
                            decimal THANHTIEN = 0; decimal.TryParse(dataReaderParent["THANHTIEN"].ToString(), out THANHTIEN);
                            _DATA_INLAI_HOADON.THANHTIEN = THANHTIEN;
                        }
                    }
                    OracleCommand cmdChildren = new OracleCommand();
                    cmdChildren.Connection = connection;
                    cmdChildren.CommandText = string.Format(@"SELECT * FROM GIAODICH_CHITIET WHERE MAGDQUAYPK = :MAGDQUAYPK AND MADONVI = :MADONVI");
                    cmdChildren.CommandType = CommandType.Text;
                    cmdChildren.Parameters.Add("MAGDQUAYPK", OracleDbType.NVarchar2, 50).Value = MAGIAODICHQUAYPK;
                    cmdChildren.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                    OracleDataReader dataReader = null;
                    dataReader = cmdChildren.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        List<BOHANG_DTO> listBoHang = new List<BOHANG_DTO>();
                        while (dataReader.Read())
                        {
                            decimal GIABANLE_VAT, VATBAN, TIEN_CHIETKHAU, TYLE_CHIETKHAU, TYLE_KHUYENMAI, TIEN_KHUYENMAI, THANHTIEN, SOLUONG = 0;
                            GIAODICH_CHITIET item = new GIAODICH_CHITIET();
                            string MaBoHangPk = dataReader["MABOPK"].ToString().Trim();
                            if (!string.IsNullOrEmpty(MaBoHangPk) && !MaBoHangPk.Equals("BH"))
                            {
                                decimal SOLUONG_BOHANG, GIABANLECOVAT_BOHANG, THANHTIENCOVAT_BOHANG = 0;
                                string MaVatTu = dataReader["MAHANG"].ToString().ToUpper().Trim();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG_BOHANG);
                                decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLECOVAT_BOHANG);
                                decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIENCOVAT_BOHANG);
                                List<BOHANG_DETAILS_DTO> listBoHangMatHangExist = new List<BOHANG_DETAILS_DTO>();
                                BOHANG_DTO boHangExist = listBoHang.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk));
                                if (boHangExist == null)
                                {
                                    BOHANG_DTO boHang = new BOHANG_DTO();
                                    boHang.MABOHANG = MaBoHangPk;
                                    boHang.THANHTIEN = THANHTIENCOVAT_BOHANG;
                                    boHang.TONGSL = SOLUONG_BOHANG;
                                    BOHANG_DETAILS_DTO MatHangExist = listBoHangMatHangExist.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAHANG.Equals(MaVatTu));
                                    if (MatHangExist == null)
                                    {
                                        BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                        mathang.MABOHANG = MaBoHangPk;
                                        mathang.MAHANG = MaVatTu;
                                        mathang.SOLUONG = SOLUONG_BOHANG;
                                        mathang.GIABANLE_VAT = GIABANLECOVAT_BOHANG;
                                        mathang.THANHTIEN = THANHTIENCOVAT_BOHANG;
                                        boHang.MATHANG_BOHANG.Add(mathang);
                                    }
                                    listBoHang.Add(boHang);
                                }
                                else
                                {
                                    boHangExist.THANHTIEN += THANHTIENCOVAT_BOHANG;
                                    boHangExist.TONGSL += SOLUONG_BOHANG;
                                    BOHANG_DETAILS_DTO MatHangExist = boHangExist.MATHANG_BOHANG.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAHANG.Equals(MaVatTu));
                                    if (MatHangExist == null)
                                    {
                                        BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                        mathang.MABOHANG = MaBoHangPk;
                                        mathang.MAHANG = MaVatTu;
                                        mathang.SOLUONG = SOLUONG_BOHANG;
                                        mathang.GIABANLE_VAT = GIABANLECOVAT_BOHANG;
                                        mathang.THANHTIEN = THANHTIENCOVAT_BOHANG;
                                        boHangExist.MATHANG_BOHANG.Add(mathang);
                                    }
                                }
                            }
                            else
                            {
                                item.MAHANG = dataReader["MAHANG"].ToString();
                                item.TENHANG = dataReader["TENHANG"].ToString();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                item.SOLUONG = SOLUONG;
                                decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                item.GIABANLE_VAT = GIABANLE_VAT;
                                decimal.TryParse(dataReader["TIEN_CHIETKHAU"].ToString(), out TIEN_CHIETKHAU);
                                item.TIEN_CHIETKHAU = TIEN_CHIETKHAU;
                                decimal.TryParse(dataReader["TYLE_CHIETKHAU"].ToString(), out TYLE_CHIETKHAU);
                                item.TYLE_CHIETKHAU = TYLE_CHIETKHAU;
                                decimal.TryParse(dataReader["TYLE_KHUYENMAI"].ToString(), out TYLE_KHUYENMAI);
                                item.TYLE_KHUYENMAI = TYLE_KHUYENMAI;
                                decimal.TryParse(dataReader["TIEN_KHUYENMAI"].ToString(), out TIEN_KHUYENMAI);
                                item.TIEN_KHUYENMAI = TIEN_KHUYENMAI;
                                decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIEN);
                                item.THANHTIEN = THANHTIEN;
                                item.MATHUE_RA = dataReader["MATHUE_RA"].ToString();
                                _DATA_INLAI_HOADON.LST_DETAILS.Add(item);
                            }
                        }
                        //Add mã bó hàng vào list
                        if (listBoHang.Count > 0)
                        {
                            foreach (BOHANG_DTO row in listBoHang)
                            {
                                decimal SOLUONG_BOHANG_BAN = 0;
                                GIAODICH_CHITIET item = new GIAODICH_CHITIET();
                                decimal TONGLE = 0;
                                decimal SUM_SOLUONG_BO = 0;
                                item.MAHANG = row.MABOHANG;
                                OracleCommand commamdBoHang = new OracleCommand();
                                commamdBoHang.Connection = connection;
                                commamdBoHang.CommandText = string.Format(@"SELECT a.MABOHANG,a.TENBOHANG,SUM(b.SOLUONG) AS TONGSOLUONG,SUM(b.TONGLE) AS TONGLE FROM DM_BOHANG a INNER JOIN DM_BOHANGCHITIET b ON a.MABOHANG = b.MABOHANG WHERE a.MABOHANG = :MABOHANG AND a.UNITCODE = :UNITCODE GROUP BY a.MABOHANG,a.TENBOHANG");
                                commamdBoHang.Parameters.Add("MABOHANG", OracleDbType.NVarchar2, 50).Value = row.MABOHANG;
                                commamdBoHang.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                OracleDataReader dataReaderBoHang = commamdBoHang.ExecuteReader();
                                if (dataReaderBoHang.HasRows)
                                {
                                    while (dataReaderBoHang.Read())
                                    {
                                        item.TENHANG = dataReaderBoHang["TENBOHANG"].ToString();
                                        decimal.TryParse(dataReaderBoHang["TONGSOLUONG"].ToString(), out SUM_SOLUONG_BO);
                                        decimal.TryParse(dataReaderBoHang["TONGLE"].ToString(), out TONGLE);
                                    }
                                }
                                decimal.TryParse((row.TONGSL / SUM_SOLUONG_BO).ToString(), out SOLUONG_BOHANG_BAN);
                                item.SOLUONG = SOLUONG_BOHANG_BAN;
                                item.GIABANLE_VAT = TONGLE;
                                EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();
                                _EXTEND_VAT_BOHANG = FrmXuatBanLeService.LAYDULIEU_VAT_BOHANG_FROM_DATABASE_ORACLE(row.MABOHANG, Session.Session.CurrentUnitCode);
                                if (row.MATHANG_BOHANG.Count > 0)
                                {
                                    decimal SUM_TTIENCOVAT_MATHANG_BOHANG = 0;
                                    foreach (BOHANG_DETAILS_DTO _BOHANG_DETAILS_DTO in row.MATHANG_BOHANG)
                                    {
                                        SUM_TTIENCOVAT_MATHANG_BOHANG += _BOHANG_DETAILS_DTO.THANHTIEN;
                                    }
                                    item.THANHTIEN = SUM_TTIENCOVAT_MATHANG_BOHANG;
                                }
                                item.GIAVON = 0;
                                item.THANHTIEN = row.THANHTIEN;
                                item.MATHUE_RA = _EXTEND_VAT_BOHANG.MATHUE_RA;
                                _DATA_INLAI_HOADON.LST_DETAILS.Add(item);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return _DATA_INLAI_HOADON;
        }

        public GIAODICH_DTO KHOITAO_DULIEU_INLAI_HOADON_FROM_SQLSERVER(string MAGIAODICHQUAYPK)
        {
            GIAODICH_DTO _DATA_INLAI_HOADON = new GIAODICH_DTO();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlCommand cmdParent = new SqlCommand();
                    cmdParent.Connection = connection;
                    cmdParent.CommandText = string.Format(@"SELECT [MAGIAODICH],[MAGIAODICHQUAYPK],[MADONVI],[NGAYTAO],[NGAYPHATSINH],[MANGUOITAO],[NGUOITAO],[MAQUAYBAN],[LOAIGIAODICH]
                    ,[HINHTHUCTHANHTOAN],[TIENKHACH_TRA],[TIEN_VOUCHER],[TIENTHEVIP],[TIEN_TRALAI_KHACH],[TIENTHE],[TIENCOD],[TIENMAT],[THANHTIEN],[THOIGIAN],[MAKHACHHANG],[UNITCODE] FROM [dbo].[NVGDQUAY_ASYNCCLIENT] WHERE MAGIAODICHQUAYPK = @MAGIAODICHQUAYPK AND UNITCODE = @UNITCODE");
                    cmdParent.CommandType = CommandType.Text;
                    cmdParent.Parameters.Add("MAGIAODICHQUAYPK", SqlDbType.NVarChar, 50).Value = MAGIAODICHQUAYPK.Trim();
                    cmdParent.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                    SqlDataReader dataReaderParent = null;
                    dataReaderParent = cmdParent.ExecuteReader();
                    if (dataReaderParent.HasRows)
                    {
                        while (dataReaderParent.Read())
                        {
                            _DATA_INLAI_HOADON.MA_GIAODICH = dataReaderParent["MA_GIAODICH"].ToString();
                            if (dataReaderParent["I_CREATE_DATE"] != null)
                            {
                                string I_CREATE_DATE = dataReaderParent["I_CREATE_DATE"].ToString();
                                _DATA_INLAI_HOADON.I_CREATE_DATE = DateTime.Parse(I_CREATE_DATE);
                            }
                            else
                            {
                                _DATA_INLAI_HOADON.I_CREATE_DATE = DateTime.Now;
                            }
                            if (dataReaderParent["NGAY_GIAODICH"] != null)
                            {
                                string NGAY_GIAODICH = dataReaderParent["NGAY_GIAODICH"].ToString();
                                _DATA_INLAI_HOADON.NGAY_GIAODICH = DateTime.Parse(NGAY_GIAODICH);
                            }
                            else
                            {
                                _DATA_INLAI_HOADON.NGAY_GIAODICH = DateTime.Now;
                            }
                            decimal TIENKHACH_TRA = 0; decimal.TryParse(dataReaderParent["TIENKHACH_TRA"].ToString(), out TIENKHACH_TRA);
                            _DATA_INLAI_HOADON.TIENKHACH_TRA = TIENKHACH_TRA;
                            decimal TIEN_TRALAI_KHACH = 0; decimal.TryParse(dataReaderParent["TIEN_TRALAI_KHACH"].ToString(), out TIEN_TRALAI_KHACH);
                            _DATA_INLAI_HOADON.TIEN_TRALAI_KHACH = TIEN_TRALAI_KHACH;
                            decimal THANHTIEN = 0; decimal.TryParse(dataReaderParent["THANHTIEN"].ToString(), out THANHTIEN);
                            _DATA_INLAI_HOADON.THANHTIEN = THANHTIEN;
                            decimal TIENMAT = 0; decimal.TryParse(dataReaderParent["TIENMAT"].ToString(), out TIENMAT);
                            _DATA_INLAI_HOADON.UNITCODE = dataReaderParent["UNITCODE"] != null ? dataReaderParent["UNITCODE"].ToString() : Session.Session.CurrentUnitCode;
                        }
                    }
                    SqlCommand cmdChildren = new SqlCommand();
                    cmdChildren.Connection = connection;
                    cmdChildren.CommandText = string.Format(@"SELECT [MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAHANG],[MANGUOITAO],[NGUOITAO],[MABOPK],[NGAYTAO],[NGAYPHATSINH] ,[SOLUONG],[THANHTIEN],[GIABANLE_VAT],[TYLE_CHIETKHAU]
                    ,[TIEN_CHIETKHAU],[TYLE_KHUYENMAI],[TIEN_KHUYENMAI],[TYLEVOUCHER],[TIEN_VOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[VATBAN],[MACHUONGTRINHKM],[UNITCODE] FROM [dbo].[GIAODICH_CHITIET] WHERE MAGDQUAYPK = @MAGDQUAYPK AND MADONVI = @MADONVI");
                    cmdChildren.CommandType = CommandType.Text;
                    cmdChildren.Parameters.Add("MAGDQUAYPK", SqlDbType.NVarChar, 50).Value = MAGIAODICHQUAYPK;
                    cmdChildren.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value =
                        Session.Session.CurrentUnitCode;
                    SqlDataReader dataReader = null;
                    dataReader = cmdChildren.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        List<BOHANG_DTO> listBoHang = new List<BOHANG_DTO>();
                        while (dataReader.Read())
                        {
                            decimal GIABANLE_VAT, VATBAN, TIEN_CHIETKHAU, TYLE_CHIETKHAU, TYLE_KHUYENMAI, TIEN_KHUYENMAI, THANHTIEN, SOLUONG = 0;
                            GIAODICH_CHITIET item = new GIAODICH_CHITIET();
                            string MaBoHangPk = dataReader["MABOPK"].ToString().Trim();
                            if (!string.IsNullOrEmpty(MaBoHangPk) && !MaBoHangPk.Equals("BH"))
                            {
                                decimal SOLUONG_BOHANG, GIABANLECOVAT_BOHANG, THANHTIENCOVAT_BOHANG = 0;
                                string MaVatTu = dataReader["MAHANG"].ToString().ToUpper().Trim();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG_BOHANG);
                                decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLECOVAT_BOHANG);
                                decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIENCOVAT_BOHANG);
                                List<BOHANG_DETAILS_DTO> listBoHangMatHangExist = new List<BOHANG_DETAILS_DTO>();
                                BOHANG_DTO boHangExist = listBoHang.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk));
                                if (boHangExist == null)
                                {
                                    BOHANG_DTO boHang = new BOHANG_DTO();
                                    boHang.MABOHANG = MaBoHangPk;
                                    boHang.THANHTIEN = THANHTIENCOVAT_BOHANG;
                                    boHang.TONGSL = SOLUONG_BOHANG;
                                    BOHANG_DETAILS_DTO MatHangExist = listBoHangMatHangExist.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAHANG.Equals(MaVatTu));
                                    if (MatHangExist == null)
                                    {
                                        BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                        mathang.MABOHANG = MaBoHangPk;
                                        mathang.MAHANG = MaVatTu;
                                        mathang.SOLUONG = SOLUONG_BOHANG;
                                        mathang.GIABANLE_VAT = GIABANLECOVAT_BOHANG;
                                        mathang.THANHTIEN = THANHTIENCOVAT_BOHANG;
                                        boHang.MATHANG_BOHANG.Add(mathang);
                                    }
                                    listBoHang.Add(boHang);
                                }
                                else
                                {
                                    boHangExist.THANHTIEN += THANHTIENCOVAT_BOHANG;
                                    boHangExist.TONGSL += SOLUONG_BOHANG;
                                    BOHANG_DETAILS_DTO MatHangExist = boHangExist.MATHANG_BOHANG.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAHANG.Equals(MaVatTu));
                                    if (MatHangExist == null)
                                    {
                                        BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                        mathang.MABOHANG = MaBoHangPk;
                                        mathang.MAHANG = MaVatTu;
                                        mathang.SOLUONG = SOLUONG_BOHANG;
                                        mathang.GIABANLE_VAT = GIABANLECOVAT_BOHANG;
                                        mathang.THANHTIEN = THANHTIENCOVAT_BOHANG;
                                        boHangExist.MATHANG_BOHANG.Add(mathang);
                                    }
                                }
                            }
                            else
                            {
                                item.MAHANG = dataReader["MAHANG"].ToString();
                                EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
                                _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(item.MAHANG, _DATA_INLAI_HOADON.UNITCODE);
                                item.TENHANG = _EXTEND_VATTU_DTO.TENHANG;
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                item.SOLUONG = SOLUONG;
                                decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                item.GIABANLE_VAT = GIABANLE_VAT;
                                decimal.TryParse(dataReader["TIEN_CHIETKHAU"].ToString(), out TIEN_CHIETKHAU);
                                item.TIEN_CHIETKHAU = TIEN_CHIETKHAU;
                                decimal.TryParse(dataReader["TYLE_CHIETKHAU"].ToString(), out TYLE_CHIETKHAU);
                                item.TYLE_CHIETKHAU = TYLE_CHIETKHAU;
                                decimal.TryParse(dataReader["TYLE_KHUYENMAI"].ToString(), out TYLE_KHUYENMAI);
                                item.TYLE_KHUYENMAI = TYLE_KHUYENMAI;
                                decimal.TryParse(dataReader["TIEN_KHUYENMAI"].ToString(), out TIEN_KHUYENMAI);
                                item.TIEN_KHUYENMAI = TIEN_KHUYENMAI;
                                decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIEN);
                                item.THANHTIEN = THANHTIEN;
                                item.MATHUE_RA = dataReader["MATHUE_RA"].ToString();
                                _DATA_INLAI_HOADON.LST_DETAILS.Add(item);
                            }
                        }
                        //Add mã bó hàng vào list
                        if (listBoHang.Count > 0)
                        {
                            foreach (BOHANG_DTO row in listBoHang)
                            {
                                decimal SOLUONG_BOHANG_BAN = 0;
                                GIAODICH_CHITIET item = new GIAODICH_CHITIET();
                                decimal TONGLE = 0;
                                decimal SUM_SOLUONG_BO = 0;
                                item.MAHANG = row.MABOHANG;
                                SqlCommand commamdBoHang = new SqlCommand();
                                commamdBoHang.Connection = connection;
                                commamdBoHang.CommandText = string.Format(@"SELECT DM_BOHANG.MABOHANG,DM_BOHANG.TENBOHANG,SUM(DM_BOHANGCHITIET.SOLUONG) AS TONGSOLUONG,SUM(DM_BOHANGCHITIET.TONGLE) AS TONGLE FROM DM_BOHANG INNER JOIN DM_BOHANGCHITIET ON DM_BOHANG.MABOHANG = DM_BOHANGCHITIET.MABOHANG WHERE DM_BOHANG.MABOHANG = @MABOHANG AND DM_BOHANG.UNITCODE = @UNITCODE GROUP BY DM_BOHANG.MABOHANG,DM_BOHANG.TENBOHANG");
                                commamdBoHang.Parameters.Add("MABOHANG", SqlDbType.NVarChar, 50).Value = row.MABOHANG;
                                commamdBoHang.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = _DATA_INLAI_HOADON.UNITCODE;
                                SqlDataReader dataReaderBoHang = commamdBoHang.ExecuteReader();
                                if (dataReaderBoHang.HasRows)
                                {
                                    while (dataReaderBoHang.Read())
                                    {
                                        item.TENHANG = dataReaderBoHang["TENBOHANG"].ToString();
                                        decimal.TryParse(dataReaderBoHang["TONGSOLUONG"].ToString(), out SUM_SOLUONG_BO);
                                        decimal.TryParse(dataReaderBoHang["TONGLE"].ToString(), out TONGLE);
                                    }
                                }
                                decimal.TryParse((row.TONGSL / SUM_SOLUONG_BO).ToString(), out SOLUONG_BOHANG_BAN);
                                item.SOLUONG = SOLUONG_BOHANG_BAN;
                                item.GIABANLE_VAT = TONGLE;
                                EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();
                                _EXTEND_VAT_BOHANG = FrmXuatBanLeService.LAYDULIEU_VAT_BOHANG_FROM_DATABASE_SQLSERVER(row.MABOHANG, Session.Session.CurrentUnitCode);
                                if (row.MATHANG_BOHANG.Count > 0)
                                {
                                    decimal SUM_TTIENCOVAT_MATHANG_BOHANG = 0;
                                    foreach (BOHANG_DETAILS_DTO _BOHANG_DETAILS_DTO in row.MATHANG_BOHANG)
                                    {
                                        SUM_TTIENCOVAT_MATHANG_BOHANG += _BOHANG_DETAILS_DTO.THANHTIEN;
                                    }
                                    item.THANHTIEN = SUM_TTIENCOVAT_MATHANG_BOHANG;
                                }
                                item.GIAVON = 0;
                                item.THANHTIEN = row.THANHTIEN;
                                item.MATHUE_RA = _EXTEND_VAT_BOHANG.MATHUE_RA;
                                _DATA_INLAI_HOADON.LST_DETAILS.Add(item);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return _DATA_INLAI_HOADON;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BillId"></param>
        /// <param name="toDate"></param>
        /// <param name="fromDate"></param>
        public void TimKIemGiaoDichQuayBanLe(string BillId, DateTime toDate, DateTime fromDate)
        {
            BILL_DTO headerBill = new BILL_DTO();
            string MaGiaoDichQuayPk = BillId.Trim() + "." + Session.Session.CurrentUnitCode.Split('-')[1];
            GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_BILL = new GIAODICH_DTO();
            string MA_TEN_KHACHHANG = "";
            if (Config.CheckConnectToServer())
            {
                _NVGDQUAY_ASYNCCLIENT_BILL = KHOITAO_DULIEU_INLAI_HOADON_FROM_ORACLE(MaGiaoDichQuayPk);
                MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_ORACLE(_NVGDQUAY_ASYNCCLIENT_BILL.MAKHACHHANG);
            }
            else
            {
                _NVGDQUAY_ASYNCCLIENT_BILL = KHOITAO_DULIEU_INLAI_HOADON_FROM_SQLSERVER(MaGiaoDichQuayPk);
                MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_SQLSERVER(_NVGDQUAY_ASYNCCLIENT_BILL.MAKHACHHANG);
            }
            using (frmPrintInLaiBill frmInLai = new frmPrintInLaiBill())
            {
                try
                {
                    BILL_DTO infoBill = new BILL_DTO()
                    {
                        ADDRESS = Session.Session.CurrentAddress,
                        CONLAI = _NVGDQUAY_ASYNCCLIENT_BILL.TIEN_TRALAI_KHACH,
                        PHONE = Session.Session.CurrentPhone,
                        MAKH = MA_TEN_KHACHHANG,
                        DIEM = 0,
                        INFOTHUNGAN = "THU NGÂN: " + _NVGDQUAY_ASYNCCLIENT_BILL.I_CREATE_BY + "\t QUẦY: ",
                        MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_BILL.MA_GIAODICH,
                        THANHTIENCHU = ConvertSoThanhChu.ChuyenDoiSoThanhChu(_NVGDQUAY_ASYNCCLIENT_BILL.THANHTIEN),
                        TIENKHACHTRA = _NVGDQUAY_ASYNCCLIENT_BILL.TIENKHACH_TRA,
                        QUAYHANG = "TEST",
                    };
                    frmInLai.PrintInvoice_BanLeInLai(infoBill, _NVGDQUAY_ASYNCCLIENT_BILL);
                }
                catch
                {
                }
                finally
                {
                    frmInLai.Close();
                    frmInLai.Dispose();
                    frmInLai.Refresh();
                }
            }
        }

        public void INSERT_VATTU_GRIDVIEW(int index)
        {
            VATTU_DTO vattu = new VATTU_DTO();
            vattu = listData_TrungMa[index];
            INSERT_DULIEU_DATAGRIDVIEW(vattu);
            frmSelect.Dispose();
        }

        private void INSERT_DULIEU_DATAGRIDVIEW(VATTU_DTO vattuDto)
        {
            decimal SoLuong = 1, RETURN_TIEN_KHUYENMAI = 0;
            btnStatus.Text = "+ TĂNG HÀNG";
            btnStatus.BackColor = Color.DarkTurquoise; CurrentKey = Keys.F4;
            decimal TEMP_SOLUONG = 0, TEMP_GIABANLE_VAT = 0, SOTIEN_KHUYENMAI = 0;
            string maVatTuCheck = vattuDto.MAHANG.Trim().ToUpper();
            //TÍNH TOÁN KHUYẾN MÃI -- CHỈ ĐỐI VỚI BÁN GIÁ BÁN LẺ -- VỚI BÁN GIÁ BÁN BUÔN VÀ GIÁ VỐN THÌ KHÔNG TÍNH KHUYẾN MẠI
            if (MethodPrice == (int)EnumCommon.METHOD_PRICE.GIABANLE_VAT)
            {
                VATTU_DTO.CAL_KHUYENMAI_OBJ KHUYENMAI = new VATTU_DTO.CAL_KHUYENMAI_OBJ();
                KHUYENMAI = FrmXuatBanLeService.TINHTOAN_KHUYENMAI(maVatTuCheck, (EnumCommon.METHOD_PRICE)MethodPrice);
                if (KHUYENMAI != null)
                {
                    RETURN_TIEN_KHUYENMAI = KHUYENMAI.GIATRI_KHUYENMAI;
                    vattuDto.MA_KHUYENMAI = KHUYENMAI.MA_KHUYENMAI;
                    vattuDto.TIEN_KHUYENMAIRETURN = RETURN_TIEN_KHUYENMAI;
                }
            }
            //CHECK TRÙNG TRONG GRIDVIEW
            int indexRowExists = CHECK_ROW_EXIST_DATAGRIDVIEW(dgvDetails_Tab, maVatTuCheck);
            if (indexRowExists != -1)
            {
                dgvDetails_Tab.Rows[indexRowExists].Cells["MA_KHUYENMAI"].Value = vattuDto.MA_KHUYENMAI;
                dgvDetails_Tab.Rows[indexRowExists].Cells["LAMACAN"].Value = vattuDto.LAMACAN;
                dgvDetails_Tab.Rows[indexRowExists].Cells["TONCUOIKYSL"].Value = vattuDto.TONCUOIKYSL;
                decimal SOLUONG_OLD, SOLUONG_NEW = 0;
                decimal.TryParse(dgvDetails_Tab.Rows[indexRowExists].Cells["SOLUONG"].Value.ToString(),
                    out SOLUONG_OLD);
                dgvDetails_Tab.Rows[indexRowExists].Cells["GIABANLE_VAT"].Value = FormatCurrency.FormatMoney(vattuDto.GIABANLE_VAT);
                if (vattuDto.SOLUONG == 0)
                {
                    TEMP_SOLUONG = SoLuong;
                }
                else
                {
                    TEMP_SOLUONG = vattuDto.SOLUONG;
                }
                if (SOLUONG_OLD + TEMP_SOLUONG > vattuDto.TONCUOIKYSL)
                {
                    if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                    {
                        NotificationLauncher.ShowNotificationWarning("Thông báo", "Hết hàng trong kho ! Không thể bán", 1, "0x1", "0x8", "normal");
                        return;
                    }
                }
                SOLUONG_NEW = SOLUONG_OLD + TEMP_SOLUONG;
                dgvDetails_Tab.Rows[indexRowExists].Cells["SOLUONG"].Value = FormatCurrency.FormatMoney(SOLUONG_NEW);
                if (RETURN_TIEN_KHUYENMAI < 100)
                {
                    SOTIEN_KHUYENMAI = vattuDto.GIABANLE_VAT * RETURN_TIEN_KHUYENMAI * SOLUONG_NEW / 100;
                    dgvDetails_Tab.Rows[indexRowExists].Cells["TIENKM"].Value =
                        FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                }
                else
                {
                    SOTIEN_KHUYENMAI = RETURN_TIEN_KHUYENMAI * SOLUONG_NEW;
                    dgvDetails_Tab.Rows[indexRowExists].Cells["TIENKM"].Value =
                        FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                }
                dgvDetails_Tab.Rows[indexRowExists].Cells["THANHTIEN"].Value =
                    FormatCurrency.FormatMoney(SOLUONG_NEW * vattuDto.GIABANLE_VAT - SOTIEN_KHUYENMAI);
                string index = dgvDetails_Tab.Rows[indexRowExists].Cells[0].Value.ToString();
                int temp = 0;
                dgvDetails_Tab.Rows[indexRowExists].Cells["STT"].Value = dgvDetails_Tab.Rows.Count;
                //txtSoLuong.Text = TEMP_SOLUONG.ToString();
                for (int i = dgvDetails_Tab.Rows.Count; i > int.Parse(index); i--)
                {
                    dgvDetails_Tab.Rows[temp++].Cells[0].Value = i - 1;
                }
                dgvDetails_Tab.Sort(dgvDetails_Tab.Columns["STT"], ListSortDirection.Ascending);
                txtSoLuong.Text = SOLUONG_NEW.ToString();
            }
            else
            {
                if (vattuDto.GIABANLE_VAT != 0)
                {
                    if (TEMP_SOLUONG == 0)
                    {
                        TEMP_SOLUONG = SoLuong;
                    }
                    if (vattuDto.SOLUONG == 0)
                    {
                        TEMP_SOLUONG = SoLuong;
                    }
                    else
                    {
                        TEMP_SOLUONG = vattuDto.SOLUONG;
                    }
                    if (RETURN_TIEN_KHUYENMAI < 100)
                    {
                        SOTIEN_KHUYENMAI = vattuDto.GIABANLE_VAT * RETURN_TIEN_KHUYENMAI * TEMP_SOLUONG / 100;
                    }
                    else
                    {
                        SOTIEN_KHUYENMAI = RETURN_TIEN_KHUYENMAI * TEMP_SOLUONG;
                    }
                }
                else
                {
                    NotificationLauncher.ShowNotification("Thông báo", "Lỗi khi thêm mã hàng", 1, "0x1", "0x8", "normal");
                    return;
                }
                int indexRowNew = 1;
                if (dgvDetails_Tab.Rows.Count > 0)
                {
                    indexRowNew = int.Parse(dgvDetails_Tab.Rows[0].Cells["STT"].Value.ToString()) + 1;
                }
                int idx = dgvDetails_Tab.Rows.Add();
                DataGridViewRow rowData = dgvDetails_Tab.Rows[idx];
                rowData.Cells["STT"].Value = indexRowNew;
                rowData.Cells["MA_KHUYENMAI"].Value = vattuDto.MA_KHUYENMAI;
                rowData.Cells["LAMACAN"].Value = vattuDto.LAMACAN;
                rowData.Cells["TONCUOIKYSL"].Value = vattuDto.TONCUOIKYSL;
                rowData.Cells["MAHANG"].Value = vattuDto.MAHANG.Trim().ToUpper();
                rowData.Cells["TENHANG"].Value = vattuDto.TENHANG.Trim();
                rowData.Cells["DONVITINH"].Value = vattuDto.DONVITINH.Trim().ToUpper();
                rowData.Cells["GIABANLE_VAT"].Value = FormatCurrency.FormatMoney(vattuDto.GIABANLE_VAT);
                if (vattuDto.SOLUONG == 0)
                {
                    TEMP_SOLUONG = SoLuong;
                }
                else
                {
                    TEMP_SOLUONG = vattuDto.SOLUONG;
                }
                rowData.Cells["SOLUONG"].Value = FormatCurrency.FormatMoney(TEMP_SOLUONG);
                rowData.Cells["GIAVON"].Value = FormatCurrency.FormatMoney(vattuDto.GIAVON);
                if (RETURN_TIEN_KHUYENMAI < 100)
                {
                    SOTIEN_KHUYENMAI = (vattuDto.GIABANLE_VAT * RETURN_TIEN_KHUYENMAI * TEMP_SOLUONG) / 100;
                    rowData.Cells["TIENKM"].Value = FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                }
                else
                {
                    SOTIEN_KHUYENMAI = RETURN_TIEN_KHUYENMAI * TEMP_SOLUONG;
                    rowData.Cells["TIENKM"].Value = FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                }
                TEMP_GIABANLE_VAT = TEMP_SOLUONG * vattuDto.GIABANLE_VAT - SOTIEN_KHUYENMAI;
                rowData.Cells["THANHTIEN"].Value = FormatCurrency.FormatMoney(TEMP_GIABANLE_VAT);
                rowData.Cells["CHIETKHAU"].Value = 0;
                lblChietKhauLe.Text = "0";
                txtSoLuong.Text = TEMP_SOLUONG.ToString();
            }
        }
        private System.Drawing.Printing.PrintDocument docToPrint = new System.Drawing.Printing.PrintDocument();
        private void btnPrintDialog_Click(object sender, EventArgs e)
        {
            printDialog.AllowSomePages = true;
            printDialog.ShowHelp = true;
            printDialog.Document = docToPrint;
            DialogResult result = printDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                docToPrint.Print();
            }
        }

        #region Hiển thị màn hình LCD
        static SerialPort _serialPort;
        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public void HienThiManHinhLCD(string TenVatTu, string SoLuong, string GiaBanLeVat, string ThanhTien)
        {
            try
            {
                if (USE_LCD == 10) DisplayLCD20x2(TenVatTu, SoLuong, GiaBanLeVat, ThanhTien);
            }
            catch { }
        }
        public void DisplayLCD20x2(string TenHang, string SoLuong, string DonGia, string ThanhTien)
        {
            try
            {
                SettingSerialPort();
                string Row2 = "";
                string Row1 = convertToUnSign3(TenHang);
                Row2 = SoLuong + ";" + DonGia + ";" + ThanhTien;
                if (DonGia == "") Row2 = ThanhTien;
                var lineOne = new char[20];
                var lineSecond = new char[20];
                var arrayOne = Row1.ToArray();
                var arraySecond = Row2.ToArray();
                for (int i = 0; i < 20; i++)
                {
                    lineOne[i] = (arrayOne.Length > i + 1) ? arrayOne[i] : ' ';
                    lineSecond[i] = (arraySecond.Length > i + 1) ? arraySecond[i] : ' ';
                }
                _serialPort.Write("\f");
                string result1 = String.Join("", lineOne);
                string result2 = String.Join("", lineSecond);
                _serialPort.Write(result1);
                _serialPort.Write(result2);
                _serialPort.Close();
                _serialPort.Close();
            }
            catch
            {
            }
        }
        public static void ClearDisplay()
        {
            _serialPort.Write("                                                 ");
            _serialPort.WriteLine("                                             ");
        }
        public static void SettingSerialPort()
        {
            string GetPortCOM = "";
            _serialPort = new SerialPort();
            foreach (string s in SerialPort.GetPortNames())
            {
                if (s != null)
                {
                    GetPortCOM = s;
                    _serialPort.PortName = GetPortCOM;
                }
            }
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.Encoding = Encoding.GetEncoding("UTF-8");
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            if (GetPortCOM != "") _serialPort.Open();
            else return;
        }
        #endregion

    }
}
