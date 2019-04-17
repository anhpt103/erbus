using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using ERBus.Cashier.Dto;
using ERBus.Cashier.Common;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public partial class FrmThanhToanTraLai : Form
    {
        public XuLyThanhToan handler;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        private decimal TONGTIEN_KHACHTRA = 0;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        //FORM THANH TOÁN BÁN LẺ TRẢ LẠI
        GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = new GIAODICH_DTO();
        GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new GIAODICH_DTO();
        public FrmThanhToanTraLai(GIAODICH_DTO GIAODICH_DTO, GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_BILL)
        {
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = GIAODICH_DTO;
            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = _NVGDQUAY_ASYNCCLIENT_BILL;
            InitializeComponent();
            int _currentUcFrame = FrmMain._currentUcFrame;
            this.Text = "THANH TOÁN HÓA ĐƠN TRẢ LẠI " + (_currentUcFrame + 1);
            txtThanhToan_MaGiaoDich.Text = _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.MA_GIAODICH;
            txtThanhToan_TienThanhToan.Text = FormatCurrency.FormatMoney(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.THANHTIEN);
            txtThanhToan_TienMat.Text = FormatCurrency.FormatMoney(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.THANHTIEN);
            txtThanhToan_TienMat.Focus();
            this.ActiveControl = txtThanhToan_TienMat;
            txtThanhToan_TienMat.SelectAll();
            decimal.TryParse(txtThanhToan_TienMat.Text.Trim(), out TONGTIEN_KHACHTRA);
        }
        public void SetHanler(XuLyThanhToan xuLy)
        {
            this.handler = xuLy;
        }
       
        private void btnThanhToan_Exit_Click(object sender, EventArgs e)
        {
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = new GIAODICH_DTO();
            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new GIAODICH_DTO();
            this.Close();
        }
        /// <summary>
        /// Lưu dữ liệu bán lẻ
        /// </summary>
        /// <param name="header"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        private int SAVE_DATA_TO_ORACLE(GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_DTO)
        {
            int countCheckInsertSuccess = 0;
            if (_NVGDQUAY_ASYNCCLIENT_DTO != null)
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryInsert = "";
                                OracleCommand cmd = new OracleCommand();
                                cmd.Connection = connection;
                                queryInsert =
                                   string.Format(@"INSERT INTO GIAODICH (ID,MA_GIAODICH,LOAI_GIAODICH,NGAY_GIAODICH,MAKHACHHANG,THOIGIAN_TAO,TIENKHACH_TRA,TIEN_TRALAI_KHACH,
                                    MAKHO_XUAT,MA_VOUCHER,I_CREATE_DATE,I_CREATE_BY,I_STATE,UNITCODE) VALUES (
                                    :ID,:MA_GIAODICH,:LOAI_GIAODICH,:NGAY_GIAODICH,:MAKHACHHANG,:THOIGIAN_TAO,
                                    :TIENKHACH_TRA,:TIEN_TRALAI_KHACH,:MAKHO_XUAT,:MA_VOUCHER,:I_CREATE_DATE,:I_CREATE_BY,:I_STATE,:UNITCODE)");
                                cmd.CommandText = queryInsert;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.ID;
                                cmd.Parameters.Add("MA_GIAODICH", OracleDbType.Varchar2, 70).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                cmd.Parameters.Add("LOAI_GIAODICH", OracleDbType.Varchar2, 10).Value = _NVGDQUAY_ASYNCCLIENT_DTO.LOAI_GIAODICH;
                                cmd.Parameters.Add("NGAY_GIAODICH", OracleDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGAY_GIAODICH;
                                cmd.Parameters.Add("MAKHACHHANG", OracleDbType.Varchar2, 50).Value = !string.IsNullOrEmpty(_NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG) ? _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG : "KHACHLE";
                                cmd.Parameters.Add("THOIGIAN_TAO", OracleDbType.Varchar2, 12).Value = _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN_TAO;
                                cmd.Parameters.Add("TIENKHACH_TRA", OracleDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACH_TRA != 0 ? _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACH_TRA : _NVGDQUAY_ASYNCCLIENT_DTO.THANHTIEN;
                                cmd.Parameters.Add("TIEN_TRALAI_KHACH", OracleDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIEN_TRALAI_KHACH;
                                cmd.Parameters.Add("MAKHO_XUAT", OracleDbType.NVarchar2, 50).Value = !string.IsNullOrEmpty(_NVGDQUAY_ASYNCCLIENT_DTO.MAKHO_XUAT) ? _NVGDQUAY_ASYNCCLIENT_DTO.MAKHO_XUAT : Session.Session.CurrentWareHouse;
                                cmd.Parameters.Add("MA_VOUCHER", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MA_VOUCHER;
                                cmd.Parameters.Add("I_CREATE_DATE", OracleDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE;
                                cmd.Parameters.Add("I_CREATE_BY", OracleDbType.NVarchar2, 300).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY;
                                cmd.Parameters.Add("I_STATE", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_STATE;
                                cmd.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE;
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    countCheckInsertSuccess++;
                                    INSERT_DATA_HANG_GIAODICHQUAY(_NVGDQUAY_ASYNCCLIENT_DTO, ref countCheckInsertSuccess);

                                }
                                //chạy store tăng xuất nhập tồn        
                                if (!string.IsNullOrEmpty(Session.Session.CurrentTableNamePeriod))
                                {
                                    OracleCommand cmdTruTon = new OracleCommand();
                                    cmdTruTon.Connection = connection;
                                    cmdTruTon.CommandText = @"ERBUS.XUATNHAPTON.XNT_TANG_PHIEU";
                                    cmdTruTon.CommandType = CommandType.StoredProcedure;
                                    cmdTruTon.Parameters.Add("P_TABLENAME", OracleDbType.Varchar2, 50).Value = Session.Session.CurrentTableNamePeriod;
                                    cmdTruTon.Parameters.Add("P_NAM", OracleDbType.Varchar2, 50).Value = Session.Session.CurrentYear;
                                    cmdTruTon.Parameters.Add("P_KY", OracleDbType.Decimal).Value = decimal.Parse(Session.Session.CurrentPeriod);
                                    cmdTruTon.Parameters.Add("P_ID", OracleDbType.Varchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.ID;
                                    cmdTruTon.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLogs.LogError(ex);
                            }
                            finally
                            {
                                SAVE_DATA_TO_SQL_HAVE_INTERNET(_NVGDQUAY_ASYNCCLIENT_DTO);
                                connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
            return countCheckInsertSuccess;
        }
        private int SAVE_DATA_TO_SQL_HAVE_INTERNET(GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_DTO)
        {
            int countCheckInsertSuccess = 0;
            if (_NVGDQUAY_ASYNCCLIENT_DTO != null)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryInsert = "";
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = connection;
                                queryInsert = string.Format(@"INSERT INTO GIAODICH (ID,MA_GIAODICH,LOAI_GIAODICH,NGAY_GIAODICH,MAKHACHHANG,THOIGIAN_TAO,TIENKHACH_TRA,TIEN_TRALAI_KHACH,
                                    MAKHO_XUAT,MA_VOUCHER,I_CREATE_DATE,I_CREATE_BY,I_STATE,UNITCODE) VALUES (
                                    @ID,@MA_GIAODICH,@LOAI_GIAODICH,@NGAY_GIAODICH,@MAKHACHHANG,@THOIGIAN_TAO,
                                    @TIENKHACH_TRA,@TIEN_TRALAI_KHACH,@MAKHO_XUAT,@MA_VOUCHER,@I_CREATE_DATE,@I_CREATE_BY,@I_STATE,@UNITCODE)");
                                cmd.CommandText = queryInsert;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add("ID", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.ID;
                                cmd.Parameters.Add("MA_GIAODICH", SqlDbType.NVarChar, 70).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                cmd.Parameters.Add("LOAI_GIAODICH", SqlDbType.VarChar, 10).Value = _NVGDQUAY_ASYNCCLIENT_DTO.LOAI_GIAODICH;
                                cmd.Parameters.Add("NGAY_GIAODICH", SqlDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGAY_GIAODICH;
                                cmd.Parameters.Add("MAKHACHHANG", SqlDbType.VarChar, 50).Value = !string.IsNullOrEmpty(_NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG) ? _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG : "KHACHLE";
                                cmd.Parameters.Add("THOIGIAN_TAO", SqlDbType.VarChar, 12).Value = _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN_TAO;
                                cmd.Parameters.Add("TIENKHACH_TRA", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACH_TRA != 0 ? _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACH_TRA : _NVGDQUAY_ASYNCCLIENT_DTO.THANHTIEN;
                                cmd.Parameters.Add("TIEN_TRALAI_KHACH", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIEN_TRALAI_KHACH;
                                cmd.Parameters.Add("MAKHO_XUAT", SqlDbType.VarChar, 50).Value = !string.IsNullOrEmpty(_NVGDQUAY_ASYNCCLIENT_DTO.MAKHO_XUAT) ? _NVGDQUAY_ASYNCCLIENT_DTO.MAKHO_XUAT : Session.Session.CurrentWareHouse;
                                cmd.Parameters.Add("MA_VOUCHER", SqlDbType.VarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MA_VOUCHER;
                                cmd.Parameters.Add("I_CREATE_DATE", SqlDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE;
                                cmd.Parameters.Add("I_CREATE_BY", SqlDbType.VarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY;
                                cmd.Parameters.Add("I_STATE", SqlDbType.VarChar, 1).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_STATE;
                                cmd.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE;
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    countCheckInsertSuccess++;
                                    INSERT_DATA_HANG_GIAODICHQUAY_SQL(_NVGDQUAY_ASYNCCLIENT_DTO, ref countCheckInsertSuccess);
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLogs.LogError(ex);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
            return countCheckInsertSuccess;
        }

        private int SAVE_DATA_TO_SQL(GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_DTO)
        {
            int countCheckInsertSuccess = 0;
            if (_NVGDQUAY_ASYNCCLIENT_DTO != null)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryInsert = "";
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = connection;
                                queryInsert = string.Format(@"INSERT INTO GIAODICH (ID,MA_GIAODICH,LOAI_GIAODICH,NGAY_GIAODICH,MAKHACHHANG,THOIGIAN_TAO,TIENKHACH_TRA,TIEN_TRALAI_KHACH,
                                    MAKHO_XUAT,MA_VOUCHER,I_CREATE_DATE,I_CREATE_BY,I_STATE,UNITCODE) VALUES (
                                    @ID,@MA_GIAODICH,@LOAI_GIAODICH,@NGAY_GIAODICH,@MAKHACHHANG,@THOIGIAN_TAO,
                                    @TIENKHACH_TRA,@TIEN_TRALAI_KHACH,@MAKHO_XUAT,@MA_VOUCHER,@I_CREATE_DATE,@I_CREATE_BY,@I_STATE,@UNITCODE)");
                                cmd.CommandText = queryInsert;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add("ID", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.ID;
                                cmd.Parameters.Add("MA_GIAODICH", SqlDbType.NVarChar, 70).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MA_GIAODICH;
                                cmd.Parameters.Add("LOAI_GIAODICH", SqlDbType.VarChar, 10).Value = _NVGDQUAY_ASYNCCLIENT_DTO.LOAI_GIAODICH;
                                cmd.Parameters.Add("NGAY_GIAODICH", SqlDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGAY_GIAODICH;
                                cmd.Parameters.Add("MAKHACHHANG", SqlDbType.VarChar, 50).Value = !string.IsNullOrEmpty(_NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG) ? _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG : "KHACHLE";
                                cmd.Parameters.Add("THOIGIAN_TAO", SqlDbType.VarChar, 12).Value = _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN_TAO;
                                cmd.Parameters.Add("TIENKHACH_TRA", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACH_TRA != 0 ? _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACH_TRA : _NVGDQUAY_ASYNCCLIENT_DTO.THANHTIEN;
                                cmd.Parameters.Add("TIEN_TRALAI_KHACH", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIEN_TRALAI_KHACH;
                                cmd.Parameters.Add("MAKHO_XUAT", SqlDbType.VarChar, 50).Value = !string.IsNullOrEmpty(_NVGDQUAY_ASYNCCLIENT_DTO.MAKHO_XUAT) ? _NVGDQUAY_ASYNCCLIENT_DTO.MAKHO_XUAT : Session.Session.CurrentWareHouse;
                                cmd.Parameters.Add("MA_VOUCHER", SqlDbType.VarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MA_VOUCHER;
                                cmd.Parameters.Add("I_CREATE_DATE", SqlDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE;
                                cmd.Parameters.Add("I_CREATE_BY", SqlDbType.VarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY;
                                cmd.Parameters.Add("I_STATE", SqlDbType.VarChar, 1).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_STATE;
                                cmd.Parameters.Add("UNITCODE", SqlDbType.VarChar, 10).Value = _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE;
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    countCheckInsertSuccess++;
                                    INSERT_DATA_HANG_GIAODICHQUAY_SQL(_NVGDQUAY_ASYNCCLIENT_DTO, ref countCheckInsertSuccess);

                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLogs.LogError(ex);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
            return countCheckInsertSuccess;
        }

        private void LUU_DULIEU(GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_DTO)
        {
            try
            {
                int countSave = 0;
                if (Config.CheckConnectToServer())
                {
                    countSave = SAVE_DATA_TO_ORACLE(_NVGDQUAY_ASYNCCLIENT_DTO);
                }
                else
                {
                    countSave = SAVE_DATA_TO_SQL(_NVGDQUAY_ASYNCCLIENT_DTO);
                }
                if (countSave >= 2)
                {
                    NotificationLauncher.ShowNotification("Thông báo", "Hoàn thành giao dịch", 1, "0x1", "0x8", "normal");
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
            }
        }
        private void THANHTOAN_HOADON_BANLE_TRALAI()
        {
            bool success = false;
            string TONGTIEN_BANGCHU = ConvertSoThanhChu.ChuyenDoiSoThanhChu(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.THANHTIEN);
            try
            {
                LUU_DULIEU(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL);
            }
            catch
            {
                MessageBox.Show("CẢNH BÁO ! XẢY RA LỖI KHI LƯU HÓA ĐƠN NÀY, HÃY LƯU LẠI HÓA ĐƠN ĐỂ KIỂM TRA ! XIN CẢM ƠN ");
            }
            try
            {
                string MA_TEN_KHACHHANG = "";
                if (Config.CheckConnectToServer())
                {
                    MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_ORACLE(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG);
                }
                else
                {
                    MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_SQLSERVER(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG);
                }
                using (frmPrintBill_TraLai frmBanTraLai = new frmPrintBill_TraLai())
                {
                    try
                    {
                        BILL_DTO infoBill = new BILL_DTO()
                        {
                            ADDRESS = Session.Session.CurrentAddress,
                            CONLAI = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIEN_TRALAI_KHACH,
                            PHONE = Session.Session.CurrentPhone,
                            MAKH = MA_TEN_KHACHHANG,
                            DIEM = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.SODIEM,
                            INFOTHUNGAN = "THU NGÂN: " + Session.Session.CurrentTenNhanVien,
                            MA_GIAODICH = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MA_GIAODICH,
                            THANHTIENCHU = TONGTIEN_BANGCHU,
                            TIENKHACHTRA = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACH_TRA,
                            TENCUAHANG = Session.Session.CurrentNameStore
                        };
                        frmBanTraLai.PrintInvoice_BanLeTraLai(infoBill, _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        this.handler(true);
                        this.Dispose();
                        frmBanTraLai.Dispose();
                        frmBanTraLai.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
            }
        }
        private void FrmThanhToan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = new GIAODICH_DTO();
                _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new GIAODICH_DTO();
            }
            if (e.KeyCode == Keys.Enter)
            {
                decimal tienThanhToan = 0;
                decimal.TryParse(txtThanhToan_TienTraLai.Text, out tienThanhToan);
                if (tienThanhToan >= 0)
                {
                    THANHTOAN_HOADON_BANLE_TRALAI();
                }
                else
                {
                    NotificationLauncher.ShowNotificationError("Thông báo", "Sai tiền !", 1, "0x1", "0x8", "normal");
                }
            }
        }
        private void btnThanhToan_Save_Click(object sender, EventArgs e)
        {
            decimal tienThanhToan = 0;
            decimal.TryParse(txtThanhToan_TienTraLai.Text.ToString(), out tienThanhToan);
            if (tienThanhToan >= 0)
            {
                THANHTOAN_HOADON_BANLE_TRALAI();
            }
            else
            {
                NotificationLauncher.ShowNotificationError("Thông báo", "Sai tiền !", 1, "0x1", "0x8", "normal");
            }
        }
        private void txtThanhToan_TienMat_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (txtThanhToan_TienMat.Text.Trim().Length > 0)
            {
                decimal TIENMAT_KHACHTRA, TONGTIEN_HOADON, TIEN_TRALAI = 0;
                decimal.TryParse(txtThanhToan_TienMat.Text.Trim(), out TIENMAT_KHACHTRA);
                decimal.TryParse(txtThanhToan_TienThanhToan.Text.Trim(), out TONGTIEN_HOADON);
                TIEN_TRALAI = TIENMAT_KHACHTRA - TONGTIEN_HOADON;
                if (TIEN_TRALAI >= 0 && TIEN_TRALAI < 1000000)
                {
                    txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(TIEN_TRALAI);
                    txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(TIEN_TRALAI);
                    btnThanhToan_Save.Focus();
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.THANHTIEN = TIENMAT_KHACHTRA;
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIEN_TRALAI_KHACH = TIEN_TRALAI;
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENKHACH_TRA = TIENMAT_KHACHTRA;
                }
                else if (TIEN_TRALAI < 0)
                {
                    txtThanhToan_TienMat.SelectAll();
                }
                else
                {
                    DialogResult result = MessageBox.Show("SỐ TIỀN TRẢ LẠI QUÁ LỚN ?", "THAO TÁC SAI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (result == DialogResult.OK)
                    {
                        TIEN_TRALAI = TIEN_TRALAI / 10;
                        txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(TIEN_TRALAI);
                        txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(TIEN_TRALAI);
                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.THANHTIEN = TIENMAT_KHACHTRA;
                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIEN_TRALAI_KHACH = TIEN_TRALAI;
                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENKHACH_TRA = TIENMAT_KHACHTRA;
                    }
                }
            }
        }
        private void txtThanhToan_TienMat_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                if (!txtThanhToan_TienMat.Text.Equals(txtThanhToan_TienThanhToan.Text))
                {
                    decimal THANHTOAN_TIENTHE = 0;
                    string THANHTOAN_TIENMAT_STRING = txtThanhToan_TienMat.Text;
                    int start = txtThanhToan_TienMat.Text.Length - txtThanhToan_TienMat.SelectionStart;
                    THANHTOAN_TIENMAT_STRING = THANHTOAN_TIENMAT_STRING.Replace(",", "");
                    decimal THANHTOAN_TIENMAT = 0;
                    decimal.TryParse(THANHTOAN_TIENMAT_STRING, out THANHTOAN_TIENMAT);
                    TONGTIEN_KHACHTRA = THANHTOAN_TIENMAT;
                    decimal THANHTOAN_TONGTIEN = 0;
                    decimal.TryParse(txtThanhToan_TienThanhToan.Text.Trim().Replace(",", ""), out THANHTOAN_TONGTIEN);
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.THANHTIEN = THANHTOAN_TIENMAT;
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENKHACH_TRA = THANHTOAN_TIENMAT;
                    decimal THANHTOAN_TIENTRALAI = THANHTOAN_TIENTHE + THANHTOAN_TIENMAT - THANHTOAN_TONGTIEN;
                    txtThanhToan_TienMat.Text = FormatCurrency.FormatMoney(THANHTOAN_TIENMAT_STRING);
                    txtThanhToan_TienMat.SelectionStart = -start + txtThanhToan_TienMat.Text.Length;
                    if (THANHTOAN_TIENTRALAI > 0)
                    {
                        txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(THANHTOAN_TIENTRALAI);
                        txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(THANHTOAN_TIENTRALAI);
                    }
                    else
                    {
                        txtThanhToan_TienTraLai.Text = "-" + FormatCurrency.FormatMoney(THANHTOAN_TIENTRALAI);
                    }
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIEN_TRALAI_KHACH = THANHTOAN_TIENTRALAI;
                }
                else
                {
                    return;
                }
            }
            else
            {
                decimal tienThanhToan = 0;
                decimal.TryParse(txtThanhToan_TienTraLai.Text, out tienThanhToan);
                if (tienThanhToan >= 0)
                {
                    THANHTOAN_HOADON_BANLE_TRALAI();
                }
                else
                {
                    NotificationLauncher.ShowNotificationError("THÔNG BÁO", "SỐ TIỀN SAI !", 1, "0x1", "0x8", "normal");
                }
            }
        }
        
        private void INSERT_DATA_HANG_GIAODICHQUAY(GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_DTO, ref int countCheckInsertSuccess)
        {
            if (_NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Count > 0)
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 1;
                            foreach (GIAODICH_CHITIET ITEM in _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS)
                            {
                                OracleCommand command = new OracleCommand();
                                command.Connection = connection;
                                string queryInsertItem = string.Format(@"INSERT INTO GIAODICH_CHITIET (ID,MA_GIAODICH,MAHANG,MATHUE_RA,SOLUONG,GIABANLE_VAT,MA_KHUYENMAI,TYLE_KHUYENMAI,
                                TIEN_KHUYENMAI,TYLE_CHIETKHAU,TIEN_CHIETKHAU,TIENTHE_VIP,TIEN_VOUCHER,THANHTIEN,SAPXEP) VALUES (:ID,:MA_GIAODICH,:MAHANG,:MATHUE_RA,:SOLUONG,:GIABANLE_VAT,
                                :MA_KHUYENMAI,:TYLE_KHUYENMAI,:TIEN_KHUYENMAI,:TYLE_CHIETKHAU,:TIEN_CHIETKHAU,:TIENTHE_VIP,:TIEN_VOUCHER,:THANHTIEN,:SAPXEP)");
                                command.CommandText = queryInsertItem;
                                command.CommandType = CommandType.Text;
                                command.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = ITEM.ID;
                                command.Parameters.Add("MA_GIAODICH", OracleDbType.NVarchar2, 70).Value = ITEM.MA_GIAODICH;
                                command.Parameters.Add("MAHANG", OracleDbType.NVarchar2, 50).Value = ITEM.MAHANG;
                                command.Parameters.Add("MATHUE_RA", OracleDbType.NVarchar2, 50).Value = ITEM.MATHUE_RA;
                                command.Parameters.Add("SOLUONG", OracleDbType.Decimal).Value = ITEM.SOLUONG;
                                command.Parameters.Add("GIABANLE_VAT", OracleDbType.Decimal).Value = ITEM.GIABANLE_VAT;
                                command.Parameters.Add("MA_KHUYENMAI", OracleDbType.NVarchar2, 50).Value = ITEM.MA_KHUYENMAI;
                                command.Parameters.Add("TYLE_KHUYENMAI", OracleDbType.Decimal).Value = ITEM.TYLE_KHUYENMAI;
                                command.Parameters.Add("TIEN_KHUYENMAI", OracleDbType.Decimal).Value = ITEM.TIEN_KHUYENMAI;
                                command.Parameters.Add("TYLE_CHIETKHAU", OracleDbType.Decimal).Value = ITEM.TYLE_CHIETKHAU;
                                command.Parameters.Add("TIEN_CHIETKHAU", OracleDbType.Decimal).Value = ITEM.TIEN_CHIETKHAU;
                                command.Parameters.Add("TIENTHE_VIP", OracleDbType.Decimal).Value = ITEM.TIENTHE_VIP;
                                command.Parameters.Add("TIEN_VOUCHER", OracleDbType.Decimal).Value = ITEM.TIEN_VOUCHER;
                                command.Parameters.Add("THANHTIEN", OracleDbType.Decimal).Value = ITEM.THANHTIEN;
                                command.Parameters.Add("SAPXEP", OracleDbType.Int32).Value = i;
                                try
                                {
                                    if (command.ExecuteNonQuery() > 0)
                                    {
                                        countCheckInsertSuccess++;
                                        i++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    WriteLogs.LogError(e);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLogs.LogError(ex);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }
                    else
                    {
                        NotificationLauncher.ShowNotificationError("THÔNG BÁO", "KHÔNG CÓ KẾT NỐI VỚI CƠ SỞ DỮ LIỆU ORACLE", 1, "0x1", "0x8", "normal");
                    }
                }

            }
        }

        private void INSERT_DATA_HANG_GIAODICHQUAY_SQL(GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_DTO, ref int countCheckInsertSuccess)
        {
            if (_NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 1;
                            foreach (GIAODICH_CHITIET ITEM in _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS)
                            {
                                SqlCommand command = new SqlCommand();
                                command.Connection = connection;
                                string queryInsertItem = string.Format(@"INSERT INTO GIAODICH_CHITIET (ID,MA_GIAODICH,MAHANG,MATHUE_RA,SOLUONG,GIABANLE_VAT,MA_KHUYENMAI,TYLE_KHUYENMAI,
                                TIEN_KHUYENMAI,TYLE_CHIETKHAU,TIEN_CHIETKHAU,TIENTHE_VIP,TIEN_VOUCHER,THANHTIEN,SAPXEP) VALUES (@ID,@MA_GIAODICH,@MAHANG,@MATHUE_RA,@SOLUONG,@GIABANLE_VAT,
                                @MA_KHUYENMAI,@TYLE_KHUYENMAI,@TIEN_KHUYENMAI,@TYLE_CHIETKHAU,@TIEN_CHIETKHAU,@TIENTHE_VIP,@TIEN_VOUCHER,@THANHTIEN,@SAPXEP)");
                                command.CommandText = queryInsertItem;
                                command.CommandType = CommandType.Text;
                                command.Parameters.Add("ID", SqlDbType.NVarChar, 50).Value = ITEM.ID;
                                command.Parameters.Add("MA_GIAODICH", SqlDbType.VarChar, 70).Value = ITEM.MA_GIAODICH;
                                command.Parameters.Add("MAHANG", SqlDbType.VarChar, 50).Value = ITEM.MAHANG;
                                command.Parameters.Add("MATHUE_RA", SqlDbType.VarChar, 50).Value = ITEM.MATHUE_RA;
                                command.Parameters.Add("SOLUONG", SqlDbType.Decimal).Value = ITEM.SOLUONG;
                                command.Parameters.Add("GIABANLE_VAT", SqlDbType.Decimal).Value = ITEM.GIABANLE_VAT;
                                command.Parameters.Add("MA_KHUYENMAI", SqlDbType.NVarChar, 50).Value = ITEM.MA_KHUYENMAI;
                                command.Parameters.Add("TYLE_KHUYENMAI", SqlDbType.Decimal).Value = ITEM.TYLE_KHUYENMAI;
                                command.Parameters.Add("TIEN_KHUYENMAI", SqlDbType.Decimal).Value = ITEM.TIEN_KHUYENMAI;
                                command.Parameters.Add("TYLE_CHIETKHAU", SqlDbType.Decimal).Value = ITEM.TYLE_CHIETKHAU;
                                command.Parameters.Add("TIEN_CHIETKHAU", SqlDbType.Decimal).Value = ITEM.TIEN_CHIETKHAU;
                                command.Parameters.Add("TIENTHE_VIP", SqlDbType.Decimal).Value = ITEM.TIENTHE_VIP;
                                command.Parameters.Add("TIEN_VOUCHER", SqlDbType.Decimal).Value = ITEM.TIEN_VOUCHER;
                                command.Parameters.Add("THANHTIEN", SqlDbType.Decimal).Value = ITEM.THANHTIEN;
                                command.Parameters.Add("SAPXEP", SqlDbType.Int).Value = i;
                                try
                                {
                                    if (command.ExecuteNonQuery() > 0)
                                    {
                                        countCheckInsertSuccess++;
                                        i++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    WriteLogs.LogError(e);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLogs.LogError(ex);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }
                    else
                    {
                        NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                    }
                }

            }
        }
    }
}
