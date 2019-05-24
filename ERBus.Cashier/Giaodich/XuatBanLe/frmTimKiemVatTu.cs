using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using ERBus.Cashier.Common;
using Oracle.ManagedDataAccess.Client;
using ERBus.Cashier.Dto;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public partial class frmTimKiemVatTu : Form
    {
        public SearchVatTu searchVatTu;
        public frmTimKiemVatTu()
        {
            InitializeComponent();
            dgvResultSearch.Columns["MANHACUNGCAP"].Visible = false;
            dgvResultSearch.Columns["TENNHACUNGCAP"].Visible = false;
            dgvResultSearch.Columns["MALOAI"].Visible = false;
            dgvResultSearch.Columns["MANHOM"].Visible = false;
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã vật tư" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Barcode" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Itemcode" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 3, TEXT = "Tên vật tư" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 4, TEXT = "Mã bó hàng" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 5, TEXT = "Mã hàng trong bó" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 6, TEXT = "Giá bán" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            cboDieuKienTimKiem.SelectedIndex = 0;
        }
        public frmTimKiemVatTu(string MaHang)
        {
            InitializeComponent();
            dgvResultSearch.Columns["MANHACUNGCAP"].Visible = false;
            dgvResultSearch.Columns["TENNHACUNGCAP"].Visible = false;
            dgvResultSearch.Columns["MALOAI"].Visible = false;
            dgvResultSearch.Columns["MANHOM"].Visible = false;

            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã vật tư" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Barcode" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Itemcode" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 3, TEXT = "Tên vật tư" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 4, TEXT = "Mã bó hàng" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 5, TEXT = "Mã hàng trong bó" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 6, TEXT = "Giá bán" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            cboDieuKienTimKiem.SelectedIndex = 0;
            txtFilterSearch.Text = MaHang;
            List<TIMKIEM_HANGHOA_DTO> LST_TIMKIEM_VATTU_DTO = new List<TIMKIEM_HANGHOA_DTO>();
            if (Config.CheckConnectToServer())
            {
                LST_TIMKIEM_VATTU_DTO = TIMKIEM_DULIEU_HANGHOA_DATABASE_ORACLE(MaHang, 1, cboDieuKienTimKiem.SelectedIndex, Session.Session.CurrentUnitCode);
            }
            else
            {
                LST_TIMKIEM_VATTU_DTO = TIMKIEM_DULIEU_HANGHOA_DATABASE_SQLSERVER(MaHang, 1, cboDieuKienTimKiem.SelectedIndex, Session.Session.CurrentUnitCode);
            }

            BINDING_DATA_TO_GRIDVIEW(LST_TIMKIEM_VATTU_DTO);
        }
        private const int WM_KEYDOWN = 256;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                if ((Keys)m.WParam == Keys.Escape)
                {
                    this.Close();
                    this.Dispose();
                }
            }
            return base.ProcessKeyPreview(ref m);
        }
        public void handlerSearchVatTu(SearchVatTu search)
        {
            this.searchVatTu = search;
        }
        public List<TIMKIEM_HANGHOA_DTO> TIMKIEM_DULIEU_HANGHOA_DATABASE_ORACLE(string DIEUKIENLOC, int SUDUNG_TIMKIEM_ALL, int DIEUKIENCHON, string UNITCODE)
        {
            List<TIMKIEM_HANGHOA_DTO> LST_TIMKIEM_VATTU_DTO = new List<TIMKIEM_HANGHOA_DTO>();
            if (!string.IsNullOrEmpty(DIEUKIENLOC))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand command = new OracleCommand();
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = @"BANLE_TIMKIEM_BOHANG_MAHANG";
                            command.Parameters.Add(@"P_MADONVI", OracleDbType.NVarchar2, 50).Value = UNITCODE;
                            command.Parameters.Add(@"P_TUKHOA", OracleDbType.NVarchar2, 50).Value = DIEUKIENLOC.ToString().ToUpper().Trim();
                            command.Parameters.Add(@"P_SUDUNG_TIMKIEM_ALL", OracleDbType.Int32).Value = SUDUNG_TIMKIEM_ALL;
                            command.Parameters.Add(@"P_DIEUKIENCHON", OracleDbType.Int32).Value = DIEUKIENCHON;
                            command.Parameters.Add(@"CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                            OracleDataReader dataReader = command.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    TIMKIEM_HANGHOA_DTO TIMKIEM_HANGHOA_DTO = new TIMKIEM_HANGHOA_DTO();
                                    if (dataReader["MAHANG"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.MAHANG = dataReader["MAHANG"].ToString();
                                    }
                                    if (dataReader["MACON"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.MACON = dataReader["MACON"].ToString();
                                    }
                                    if (dataReader["TENHANG"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.TENHANG = dataReader["TENHANG"].ToString();
                                    }
                                    if (dataReader["MALOAI"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.MALOAI = dataReader["MALOAI"].ToString();
                                    }
                                    if (dataReader["MANHOM"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.MANHOM = dataReader["MANHOM"].ToString();
                                    }
                                    if (dataReader["DONVITINH"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.DONVITINH = dataReader["DONVITINH"].ToString();
                                    }
                                    if (dataReader["MANHACUNGCAP"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                    }
                                    if (dataReader["TENNHACUNGCAP"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.TENNHACUNGCAP = dataReader["TENNHACUNGCAP"].ToString();
                                    }
                                    decimal GIABANLE_VAT = 0;
                                    if (dataReader["GIABANLE_VAT"] != null)
                                    {
                                        decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                    }
                                    TIMKIEM_HANGHOA_DTO.GIABANLE_VAT = GIABANLE_VAT;
                                    if (dataReader["ITEMCODE"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                    }
                                    if (dataReader["BARCODE"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.BARCODE = dataReader["BARCODE"].ToString();
                                    }
                                    LST_TIMKIEM_VATTU_DTO.Add(TIMKIEM_HANGHOA_DTO);
                                }
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
            }
            return LST_TIMKIEM_VATTU_DTO;
        }

        public List<TIMKIEM_HANGHOA_DTO> TIMKIEM_DULIEU_HANGHOA_DATABASE_SQLSERVER(string DIEUKIENLOC, int SUDUNG_TIMKIEM_ALL, int DIEUKIENCHON, string UNITCODE)
        {
            List<TIMKIEM_HANGHOA_DTO> LST_TIMKIEM_VATTU_DTO = new List<TIMKIEM_HANGHOA_DTO>();
            if (!string.IsNullOrEmpty(DIEUKIENLOC))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ERBusCashier"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            SqlCommand command = new SqlCommand();
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = @"BANLE_TIMKIEM_BOHANG_MAHANG";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@P_MADONVI", UNITCODE);
                            command.Parameters.AddWithValue("@P_TUKHOA", DIEUKIENLOC.ToString().ToUpper().Trim());
                            command.Parameters.AddWithValue("@P_SUDUNG_TIMKIEM_ALL", SUDUNG_TIMKIEM_ALL);
                            command.Parameters.AddWithValue("@P_DIEUKIENCHON", DIEUKIENCHON);
                            SqlDataReader dataReader = command.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    TIMKIEM_HANGHOA_DTO TIMKIEM_HANGHOA_DTO = new TIMKIEM_HANGHOA_DTO();
                                    if (dataReader["MAHANG"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.MAHANG = dataReader["MAHANG"].ToString();
                                    }
                                    if (dataReader["MACON"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.MACON = dataReader["MACON"].ToString();
                                    }
                                    if (dataReader["TENHANG"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.TENHANG = dataReader["TENHANG"].ToString();
                                    }
                                    if (dataReader["MALOAI"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.MALOAI = dataReader["MALOAI"].ToString();
                                    }
                                    if (dataReader["MANHOM"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.MANHOM = dataReader["MANHOM"].ToString();
                                    }
                                    if (dataReader["DONVITINH"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.DONVITINH = dataReader["DONVITINH"].ToString();
                                    }
                                    if (dataReader["MANHACUNGCAP"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                    }
                                    if (dataReader["TENNHACUNGCAP"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.TENNHACUNGCAP = dataReader["TENNHACUNGCAP"].ToString();
                                    }
                                    decimal GIABANLE_VAT = 0;
                                    if (dataReader["GIABANLE_VAT"] != null)
                                    {
                                        decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                    }
                                    TIMKIEM_HANGHOA_DTO.GIABANLE_VAT = GIABANLE_VAT;
                                    if (dataReader["ITEMCODE"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                    }
                                    if (dataReader["BARCODE"] != null)
                                    {
                                        TIMKIEM_HANGHOA_DTO.BARCODE = dataReader["BARCODE"].ToString();
                                    }
                                    LST_TIMKIEM_VATTU_DTO.Add(TIMKIEM_HANGHOA_DTO);
                                }
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
            }
            return LST_TIMKIEM_VATTU_DTO;
        }

        public void BINDING_DATA_TO_GRIDVIEW(List<TIMKIEM_HANGHOA_DTO> LST_TIMKIEM_VATTU_DTO)
        {
            if (LST_TIMKIEM_VATTU_DTO.Count > 0)
            {
                dgvResultSearch.Rows.Clear();
                dgvResultSearch.DataSource = null;
                dgvResultSearch.Refresh();
                foreach (TIMKIEM_HANGHOA_DTO TIMKIEM_HANGHOA_DTO in LST_TIMKIEM_VATTU_DTO)
                {
                    int idx = dgvResultSearch.Rows.Add();
                    DataGridViewRow rowData = dgvResultSearch.Rows[idx];
                    rowData.Cells["STT"].Value = idx + 1;
                    rowData.Cells["MAHANG"].Value = TIMKIEM_HANGHOA_DTO.MAHANG;
                    rowData.Cells["MACON"].Value = TIMKIEM_HANGHOA_DTO.MACON;
                    rowData.Cells["TENHANG"].Value = TIMKIEM_HANGHOA_DTO.TENHANG;
                    rowData.Cells["MALOAI"].Value = TIMKIEM_HANGHOA_DTO.MALOAI;
                    rowData.Cells["MANHOM"].Value = TIMKIEM_HANGHOA_DTO.MANHOM;
                    rowData.Cells["DONVITINH"].Value = TIMKIEM_HANGHOA_DTO.DONVITINH;
                    rowData.Cells["MANHACUNGCAP"].Value = TIMKIEM_HANGHOA_DTO.MANHACUNGCAP;
                    rowData.Cells["TENNHACUNGCAP"].Value = TIMKIEM_HANGHOA_DTO.TENNHACUNGCAP;
                    rowData.Cells["GIABANLE_VAT"].Value = FormatCurrency.FormatMoney(TIMKIEM_HANGHOA_DTO.GIABANLE_VAT);
                    rowData.Cells["ITEMCODE"].Value = TIMKIEM_HANGHOA_DTO.ITEMCODE;
                    rowData.Cells["BARCODE"].Value = TIMKIEM_HANGHOA_DTO.BARCODE;
                }
            }
        }
        private void txtFilterSearch_TextChanged(object sender, EventArgs e)
        {
            int P_DIEUKIEN_TIMKIEM = cboDieuKienTimKiem.SelectedIndex;
            if (Config.CheckConnectToServer()) // nếu có mạng lan
            {
                List<TIMKIEM_HANGHOA_DTO> LST_TIMKIEM_VATTU_DTO = TIMKIEM_DULIEU_HANGHOA_DATABASE_ORACLE(txtFilterSearch.Text, 0, P_DIEUKIEN_TIMKIEM, Session.Session.CurrentUnitCode);
                if (LST_TIMKIEM_VATTU_DTO.Count > 0)
                {
                    BINDING_DATA_TO_GRIDVIEW(LST_TIMKIEM_VATTU_DTO);
                }
            }
            else
            {
                List<TIMKIEM_HANGHOA_DTO> LST_TIMKIEM_VATTU_DTO = TIMKIEM_DULIEU_HANGHOA_DATABASE_SQLSERVER(txtFilterSearch.Text, 0, P_DIEUKIEN_TIMKIEM, Session.Session.CurrentUnitCode);
                if (LST_TIMKIEM_VATTU_DTO.Count > 0)
                {
                    BINDING_DATA_TO_GRIDVIEW(LST_TIMKIEM_VATTU_DTO);
                }
            }
        }

        private void dgvResultSearch_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string maHang = dgvResultSearch.Rows[e.RowIndex].Cells["MAHANG"].Value.ToString();
            searchVatTu(maHang);
            Dispose();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            int P_DIEUKIEN_TIMKIEM = cboDieuKienTimKiem.SelectedIndex;
            if (Config.CheckConnectToServer()) // nếu có mạng lan
            {
                List<TIMKIEM_HANGHOA_DTO> LST_TIMKIEM_VATTU_DTO = TIMKIEM_DULIEU_HANGHOA_DATABASE_ORACLE(txtFilterSearch.Text, 0, P_DIEUKIEN_TIMKIEM, Session.Session.CurrentUnitCode);
                if (LST_TIMKIEM_VATTU_DTO.Count > 0)
                {
                    BINDING_DATA_TO_GRIDVIEW(LST_TIMKIEM_VATTU_DTO);
                }
            }
            else
            {
                List<TIMKIEM_HANGHOA_DTO> LST_TIMKIEM_VATTU_DTO = TIMKIEM_DULIEU_HANGHOA_DATABASE_SQLSERVER(txtFilterSearch.Text, 0, P_DIEUKIEN_TIMKIEM, Session.Session.CurrentUnitCode);
                if (LST_TIMKIEM_VATTU_DTO.Count > 0)
                {
                    BINDING_DATA_TO_GRIDVIEW(LST_TIMKIEM_VATTU_DTO);
                }
            }
        }
    }
}
