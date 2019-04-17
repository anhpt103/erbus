using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using ERBus.Cashier.Dto;
using ERBus.Cashier.Common;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public partial class FrmTimKiemGiaoDich : Form
    {
        public Status_TimKiem statusSearch;
        public DataTable dtLoadSearch;
        public RePrintBill GetBillOld;
        private int VALUE_SELECTED_CHANGE = 0;
        private bool printBill = false;
        public FrmTimKiemGiaoDich()
        {
            InitializeComponent();
            //khởi tạo giá trị chọn lựa điều kiện tìm kiếm
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã giao dịch" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Số tiền" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Thu ngân tạo" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            //default combobox
            cboDieuKienTimKiem.SelectedIndex = VALUE_SELECTED_CHANGE;

            dateTimeDenNgay.Format = DateTimePickerFormat.Custom;
            dateTimeDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeDenNgay.Value = DateTime.Now;
            dateTimeTuNgay.Format = DateTimePickerFormat.Custom;
            dateTimeTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeTuNgay.Value = DateTime.Now.AddDays(-7);  
        }

        public FrmTimKiemGiaoDich(bool rePrintBill)
        {
            InitializeComponent();
            //khởi tạo giá trị chọn lựa điều kiện tìm kiếm
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã giao dịch" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Số tiền" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Thu ngân tạo" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            //default combobox
            cboDieuKienTimKiem.SelectedIndex = VALUE_SELECTED_CHANGE;
            printBill = rePrintBill;
            dateTimeDenNgay.Format = DateTimePickerFormat.Custom;
            dateTimeDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeDenNgay.Value = DateTime.Now;
            dateTimeTuNgay.Format = DateTimePickerFormat.Custom;
            dateTimeTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeTuNgay.Value = DateTime.Now.AddDays(-2);
            StartForm();
        }

        public void SetHandlerBill(RePrintBill handler)
        {
            this.GetBillOld = handler;
        }
        public FrmTimKiemGiaoDich(string KeySearch, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc)
        {
            InitializeComponent();
            //khởi tạo giá trị chọn lựa điều kiện tìm kiếm
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã giao dịch" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Số tiền" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Thu ngân tạo" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            //default combobox
            cboDieuKienTimKiem.SelectedIndex = DieuKienLoc;

            dateTimeDenNgay.Format = DateTimePickerFormat.Custom;
            dateTimeDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeDenNgay.Value = DenNgay;
            dateTimeTuNgay.Format = DateTimePickerFormat.Custom;
            dateTimeTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeTuNgay.Value = TuNgay;

            txtDieuKienTimKiem.Text = KeySearch.Trim();
            try
            {
                using (
                    var connection =
                        new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    connection.OpenAsync();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.InitialLONGFetchSize = 1000;
                    cmd.CommandText = "TIMKIEM_GIAODICHQUAY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("P_KEYSEARCH", OracleDbType.Varchar2).Value = KeySearch;
                    cmd.Parameters.Add("P_TUNGAY", OracleDbType.Date).Value = TuNgay;
                    cmd.Parameters.Add("P_DENNGAY", OracleDbType.Date).Value = DenNgay;
                    cmd.Parameters.Add("P_DIEUKIENLOC", OracleDbType.Int32).Value = DieuKienLoc;
                    cmd.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2).Value = Session.Session.CurrentUnitCode;
                    cmd.Parameters.Add("CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader dataReader = null;
                    dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dgvDanhSachGiaoDichChiTiet.Rows.Clear();
                        dgvDanhSachGiaoDichChiTiet.DataSource = null;
                        dgvDanhSachGiaoDichChiTiet.Refresh();
                        while (dataReader.Read())
                        {
                            int LOAI_GIAODICH = 0;
                            int idx = dgvDanhSachGiaoDichChiTiet.Rows.Add();
                            DataGridViewRow rowData = dgvDanhSachGiaoDichChiTiet.Rows[idx];
                            rowData.Cells["MA_GIAODICH"].Value = dataReader["MA_GIAODICH"];
                            int.TryParse(dataReader["LOAI_GIAODICH"].ToString(),out LOAI_GIAODICH);
                            rowData.Cells["LOAI_GIAODICH"].Value = LOAI_GIAODICH == 1 ? "XBLE" : "TRL";
                            rowData.Cells["NGAY_GIAODICH"].Value = dataReader["NGAY_GIAODICH"];
                            rowData.Cells["I_CREATE_BY"].Value = dataReader["I_CREATE_BY"];
                            rowData.Cells["NGUOITAO"].Value = dataReader["NGUOITAO"];
                            rowData.Cells["MAQUAYBAN"].Value = dataReader["MAQUAYBAN"];
                            rowData.Cells["HINHTHUCTHANHTOAN"].Value = dataReader["HINHTHUCTHANHTOAN"];
                            rowData.Cells["TIENKHACH_TRA"].Value = FormatCurrency.FormatMoney(dataReader["TIENKHACH_TRA"]);
                            rowData.Cells["TIEN_TRALAI_KHACH"].Value = FormatCurrency.FormatMoney(dataReader["TIEN_TRALAI_KHACH"]);
                            rowData.Cells["THANHTIEN"].Value = FormatCurrency.FormatMoney(dataReader["THANHTIEN"]);
                            rowData.Cells["THOIGIAN_TAO"].Value = FormatCurrency.FormatMoney(dataReader["THOIGIAN_TAO"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
                NotificationLauncher.ShowNotification("Thông báo", "Không tìm thấy", 1, "0x1", "0x8", "normal");
            }
        }
        public static List<GIAODICH_DTO> TIMKIEM_GIAODICHQUAY(string KeySearch, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc)
        {
            List<GIAODICH_DTO> listReturn = new List<GIAODICH_DTO>();
            try
            {
                using (var connection =new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    connection.OpenAsync();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.InitialLONGFetchSize = 1000;
                    cmd.CommandText = "TIMKIEM_GIAODICHQUAY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("P_KEYSEARCH", OracleDbType.Varchar2).Value = KeySearch;
                    cmd.Parameters.Add("P_TUNGAY", OracleDbType.Date).Value = TuNgay;
                    cmd.Parameters.Add("P_DENNGAY", OracleDbType.Date).Value = DenNgay;
                    cmd.Parameters.Add("P_DIEUKIENLOC", OracleDbType.Int32).Value = DieuKienLoc;
                    cmd.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2).Value = Session.Session.CurrentUnitCode;
                    cmd.Parameters.Add("CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader dataReader = null;
                    dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            GIAODICH_DTO GDQUAY_DTO = new GIAODICH_DTO();
                            decimal TIENKHACH_TRA, TIENTHE, TIEN_TRALAI_KHACH,THANHTIEN = 0;
                            GDQUAY_DTO.MA_GIAODICH = dataReader["MA_GIAODICH"].ToString();
                            GDQUAY_DTO.LOAI_GIAODICH = dataReader["LOAI_GIAODICH"].ToString();
                            GDQUAY_DTO.I_CREATE_DATE = DateTime.Parse(dataReader["I_CREATE_DATE"].ToString());
                            GDQUAY_DTO.I_CREATE_BY = dataReader["I_CREATE_BY"].ToString();
                            GDQUAY_DTO.NGAY_GIAODICH = DateTime.Parse(dataReader["NGAY_GIAODICH"].ToString());
                            GDQUAY_DTO.MA_VOUCHER = dataReader["MA_VOUCHER"].ToString();
                            decimal.TryParse(dataReader["TIENKHACH_TRA"].ToString(), out TIENKHACH_TRA);
                            GDQUAY_DTO.TIENKHACH_TRA = TIENKHACH_TRA;
                            decimal.TryParse(dataReader["TIENTHE"].ToString(), out TIENTHE);
                            GDQUAY_DTO.TIENTHE = TIENTHE;
                            decimal.TryParse(dataReader["TIEN_TRALAI_KHACH"].ToString(), out TIEN_TRALAI_KHACH);
                            GDQUAY_DTO.TIEN_TRALAI_KHACH = TIEN_TRALAI_KHACH;
                            decimal.TryParse(dataReader["TIENTHE"].ToString(), out TIENTHE);
                            GDQUAY_DTO.TIENTHE = TIENTHE;
                            decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIEN);
                            GDQUAY_DTO.THANHTIEN = THANHTIEN;
                            GDQUAY_DTO.THOIGIAN_TAO = dataReader["THOIGIAN_TAO"].ToString();
                            GDQUAY_DTO.MAKHACHHANG = dataReader["MAKHACHHANG"].ToString();
                            GDQUAY_DTO.UNITCODE = dataReader["UNITCODE"].ToString();
                            listReturn.Add(GDQUAY_DTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
                NotificationLauncher.ShowNotification("Thông báo", "Xảy ra lỗi", 1, "0x1", "0x8", "normal");
            }
            return listReturn;
        }
        private void txtDieuKienTimKiem_TextChanged(object sender, EventArgs e)
        {
            if(txtDieuKienTimKiem.Text.Length > 3)
            {
                List<GIAODICH_DTO> dataReturn = new List<GIAODICH_DTO>();
                dataReturn = TIMKIEM_GIAODICHQUAY(txtDieuKienTimKiem.Text.Trim(), dateTimeTuNgay.Value,dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                if(dataReturn.Count > 0)
                {
                    dgvDanhSachGiaoDichChiTiet.Rows.Clear();
                    dgvDanhSachGiaoDichChiTiet.Refresh();
                    foreach (GIAODICH_DTO dataRow in dataReturn)
                    {
                        decimal TIENKHACH_TRA, TIEN_TRALAI_KHACH, THANHTIEN = 0;
                        int idx = dgvDanhSachGiaoDichChiTiet.Rows.Add();
                        DataGridViewRow rowData = dgvDanhSachGiaoDichChiTiet.Rows[idx];
                        rowData.Cells["MA_GIAODICH"].Value = dataRow.MA_GIAODICH;
                        rowData.Cells["LOAI_GIAODICH"].Value = dataRow.LOAI_GIAODICH.ToString();
                        rowData.Cells["NGAY_GIAODICH"].Value = dataRow.NGAY_GIAODICH.ToString("dd/MM/yyyy");
                        rowData.Cells["I_CREATE_BY"].Value = dataRow.I_CREATE_BY;
                        decimal.TryParse(dataRow.TIENKHACH_TRA.ToString(), out TIENKHACH_TRA);
                        rowData.Cells["TIENKHACH_TRA"].Value = FormatCurrency.FormatMoney(TIENKHACH_TRA);
                        decimal.TryParse(dataRow.TIEN_TRALAI_KHACH.ToString(), out TIEN_TRALAI_KHACH);
                        rowData.Cells["TIEN_TRALAI_KHACH"].Value = FormatCurrency.FormatMoney(TIEN_TRALAI_KHACH);
                        decimal.TryParse(dataRow.THANHTIEN.ToString(), out THANHTIEN);
                        rowData.Cells["THANHTIEN"].Value = FormatCurrency.FormatMoney(THANHTIEN);
                        rowData.Cells["THOIGIAN_TAO"].Value = dataRow.THOIGIAN_TAO;
                    }
                }
            }
        }
        private void cboDieuKienTimKiem_SelectedValueChanged(object sender, EventArgs e)
        {
            VALUE_SELECTED_CHANGE = (int)cboDieuKienTimKiem.SelectedIndex;
        }
        public void SetHanlerGiaoDichSearch(Status_TimKiem LoadDataSearch)
        {
            statusSearch = LoadDataSearch;
        }
        //DOUBLE CLICK BY DATA TO UC_FRAME_BANLE_TRALAI
        private void dgvDanhSachGiaoDichChiTiet_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string MaGiaoDichChange = dgvDanhSachGiaoDichChiTiet.Rows[e.RowIndex].Cells["MA_GIAODICH"].Value.ToString().Trim();
            if (!printBill)
            {
                statusSearch(MaGiaoDichChange, dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
            }
            else
            {
                this.GetBillOld(MaGiaoDichChange, dateTimeTuNgay.Value, dateTimeDenNgay.Value);
            }
            this.Close();
        }
        private void txtDieuKienTimKiem_Validating(object sender, CancelEventArgs e)
        {
            if (txtDieuKienTimKiem.Text.Length > 3)
            {
                List<GIAODICH_DTO> dataReturn = new List<GIAODICH_DTO>();
                if (Config.CheckConnectToServer())
                {
                    dataReturn = TIMKIEM_GIAODICHQUAY(txtDieuKienTimKiem.Text.Trim(), dateTimeTuNgay.Value,
                        dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                    
                }
                else
                {
                    dataReturn = UC_Frame_TraLai.TIMKIEM_GIAODICHQUAY_FROM_SQL(txtDieuKienTimKiem.Text.Trim(), dateTimeTuNgay.Value,
                        dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                }
                if (dataReturn.Count > 0)
                {
                    dgvDanhSachGiaoDichChiTiet.Rows.Clear();
                    dgvDanhSachGiaoDichChiTiet.Refresh();
                    foreach (GIAODICH_DTO dataRow in dataReturn)
                    {
                        decimal TIENKHACH_TRA, TIEN_TRALAI_KHACH, THANHTIEN = 0;
                        int idx = dgvDanhSachGiaoDichChiTiet.Rows.Add();
                        DataGridViewRow rowData = dgvDanhSachGiaoDichChiTiet.Rows[idx];
                        rowData.Cells["MA_GIAODICH"].Value = dataRow.MA_GIAODICH;
                        rowData.Cells["LOAI_GIAODICH"].Value = dataRow.LOAI_GIAODICH.ToString();
                        rowData.Cells["NGAY_GIAODICH"].Value = dataRow.NGAY_GIAODICH.ToString("dd/MM/yyyy");
                        rowData.Cells["I_CREATE_BY"].Value = dataRow.I_CREATE_BY;
                        decimal.TryParse(dataRow.TIENKHACH_TRA.ToString(), out TIENKHACH_TRA);
                        rowData.Cells["TIENKHACH_TRA"].Value = FormatCurrency.FormatMoney(TIENKHACH_TRA);
                        decimal.TryParse(dataRow.TIEN_TRALAI_KHACH.ToString(), out TIEN_TRALAI_KHACH);
                        rowData.Cells["TIEN_TRALAI_KHACH"].Value = FormatCurrency.FormatMoney(TIEN_TRALAI_KHACH);
                        decimal.TryParse(dataRow.THANHTIEN.ToString(), out THANHTIEN);
                        rowData.Cells["THANHTIEN"].Value = FormatCurrency.FormatMoney(THANHTIEN);
                        rowData.Cells["THOIGIAN_TAO"].Value = dataRow.THOIGIAN_TAO;
                    }
                }
            }
        }

        private void StartForm()
        {
            List<GIAODICH_DTO> dataReturn = new List<GIAODICH_DTO>();
            if (Config.CheckConnectToServer())
            {
                dataReturn = TIMKIEM_GIAODICHQUAY("", dateTimeTuNgay.Value,
                    dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
            }
            else
            {
                dataReturn = UC_Frame_TraLai.TIMKIEM_GIAODICHQUAY_FROM_SQL("", dateTimeTuNgay.Value,
                    dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
            }
            if (dataReturn.Count > 0)
            {
                dgvDanhSachGiaoDichChiTiet.Rows.Clear();
                dgvDanhSachGiaoDichChiTiet.Refresh();
                foreach (GIAODICH_DTO dataRow in dataReturn)
                {
                    decimal TIENKHACH_TRA, TIEN_TRALAI_KHACH, THANHTIEN = 0;
                    int idx = dgvDanhSachGiaoDichChiTiet.Rows.Add();
                    DataGridViewRow rowData = dgvDanhSachGiaoDichChiTiet.Rows[idx];
                    rowData.Cells["MA_GIAODICH"].Value = dataRow.MA_GIAODICH;
                    rowData.Cells["LOAI_GIAODICH"].Value = dataRow.LOAI_GIAODICH.ToString();
                    rowData.Cells["NGAY_GIAODICH"].Value = dataRow.NGAY_GIAODICH.ToString("dd/MM/yyyy");
                    rowData.Cells["I_CREATE_BY"].Value = dataRow.I_CREATE_BY;
                    decimal.TryParse(dataRow.TIENKHACH_TRA.ToString(), out TIENKHACH_TRA);
                    rowData.Cells["TIENKHACH_TRA"].Value = FormatCurrency.FormatMoney(TIENKHACH_TRA);
                    decimal.TryParse(dataRow.TIEN_TRALAI_KHACH.ToString(), out TIEN_TRALAI_KHACH);
                    rowData.Cells["TIEN_TRALAI_KHACH"].Value = FormatCurrency.FormatMoney(TIEN_TRALAI_KHACH);
                    decimal.TryParse(dataRow.THANHTIEN.ToString(), out THANHTIEN);
                    rowData.Cells["THANHTIEN"].Value = FormatCurrency.FormatMoney(THANHTIEN);
                    rowData.Cells["THOIGIAN_TAO"].Value = dataRow.THOIGIAN_TAO;
                }
            }
        }

        private void btnTimKiemGiaoDich_Click(object sender, EventArgs e)
        {
            if (txtDieuKienTimKiem.Text.Length > 3)
            {
                List<GIAODICH_DTO> dataReturn = new List<GIAODICH_DTO>();
                if (Config.CheckConnectToServer())
                {
                    dataReturn = TIMKIEM_GIAODICHQUAY(txtDieuKienTimKiem.Text.Trim(), dateTimeTuNgay.Value,
                        dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                }
                else
                {
                    dataReturn = UC_Frame_TraLai.TIMKIEM_GIAODICHQUAY_FROM_SQL(txtDieuKienTimKiem.Text.Trim(), dateTimeTuNgay.Value,
                        dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                }
                if (dataReturn.Count > 0)
                {
                    dgvDanhSachGiaoDichChiTiet.Rows.Clear();
                    dgvDanhSachGiaoDichChiTiet.Refresh();
                    foreach (GIAODICH_DTO dataRow in dataReturn)
                    {
                        decimal TIENKHACH_TRA, TIEN_TRALAI_KHACH, THANHTIEN = 0;
                        int idx = dgvDanhSachGiaoDichChiTiet.Rows.Add();
                        DataGridViewRow rowData = dgvDanhSachGiaoDichChiTiet.Rows[idx];
                        rowData.Cells["MA_GIAODICH"].Value = dataRow.MA_GIAODICH;
                        rowData.Cells["LOAI_GIAODICH"].Value = dataRow.LOAI_GIAODICH.ToString();
                        rowData.Cells["NGAY_GIAODICH"].Value = dataRow.NGAY_GIAODICH.ToString("dd/MM/yyyy");
                        rowData.Cells["I_CREATE_BY"].Value = dataRow.I_CREATE_BY;
                        decimal.TryParse(dataRow.TIENKHACH_TRA.ToString(), out TIENKHACH_TRA);
                        rowData.Cells["TIENKHACH_TRA"].Value = FormatCurrency.FormatMoney(TIENKHACH_TRA);
                        decimal.TryParse(dataRow.TIEN_TRALAI_KHACH.ToString(), out TIEN_TRALAI_KHACH);
                        rowData.Cells["TIEN_TRALAI_KHACH"].Value = FormatCurrency.FormatMoney(TIEN_TRALAI_KHACH);
                        decimal.TryParse(dataRow.THANHTIEN.ToString(), out THANHTIEN);
                        rowData.Cells["THANHTIEN"].Value = FormatCurrency.FormatMoney(THANHTIEN);
                        rowData.Cells["THOIGIAN_TAO"].Value = dataRow.THOIGIAN_TAO;
                    }
                }
            }
        }

        private void FrmTimKiemGiaoDich_Load(object sender, EventArgs e)
        {
            dgvDanhSachGiaoDichChiTiet.Columns["TIENKHACH_TRA"].Visible = false;
            dgvDanhSachGiaoDichChiTiet.Columns["TIEN_TRALAI_KHACH"].Visible = false;
            dgvDanhSachGiaoDichChiTiet.Columns["THANHTIEN"].Visible = false;
        }
    }
}
