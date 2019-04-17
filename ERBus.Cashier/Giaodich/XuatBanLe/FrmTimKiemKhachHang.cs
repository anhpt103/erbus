using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ERBus.Cashier.Dto;
using System.Drawing;
using System.Globalization;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public partial class FrmTimKiemKhachHang : Form
    {
        public STATUS_TIMKIEM_KHACHHANG statusTimKiemKhachHang;
        public void BINDING_DATA_KHACHHANG_TO_GRIDVIEW(List<KHACHHANG_DTO> _LST_KHACHHANG_DTO)
        {
            if (_LST_KHACHHANG_DTO.Count > 0)
            {
                dgvThongTinKhachHang.Rows.Clear();
                dgvThongTinKhachHang.DataSource = null;
                dgvThongTinKhachHang.Refresh();
                foreach (KHACHHANG_DTO _KHACHHANG_DTO in _LST_KHACHHANG_DTO)
                {
                    int idx = dgvThongTinKhachHang.Rows.Add();
                    DataGridViewRow rowData = dgvThongTinKhachHang.Rows[idx];
                    rowData.Cells["STT"].Value = idx + 1;
                    rowData.Cells["MAKHACHHANG"].Value = _KHACHHANG_DTO.MAKHACHHANG;
                    rowData.Cells["MATHE"].Value = _KHACHHANG_DTO.MATHE;
                    rowData.Cells["TENKHACHHANG"].Value = _KHACHHANG_DTO.TENKHACHHANG;
                    rowData.Cells["DIACHI"].Value = _KHACHHANG_DTO.DIACHI;
                    rowData.Cells["DIENTHOAI"].Value = _KHACHHANG_DTO.DIENTHOAI;
                    rowData.Cells["CANCUOC_CONGDAN"].Value = _KHACHHANG_DTO.CANCUOC_CONGDAN;

                    if(_KHACHHANG_DTO.NGAYSINH != null)
                    {
                        string NGAYSINH_STR = _KHACHHANG_DTO.NGAYSINH.Value.ToString("dd/MM/yyyy");
                        rowData.Cells["NGAYSINH"].Value = NGAYSINH_STR;
                    }

                    if (_KHACHHANG_DTO.NGAYDACBIET != null)
                    {
                        string NGAYDACBIET_STR = _KHACHHANG_DTO.NGAYDACBIET.Value.ToString("dd/MM/yyyy");
                        rowData.Cells["NGAYDACBIET"].Value = _KHACHHANG_DTO.NGAYDACBIET;
                    }

                    rowData.Cells["SODIEM"].Value = _KHACHHANG_DTO.SODIEM;
                    rowData.Cells["TONGTIEN"].Value = _KHACHHANG_DTO.TONGTIEN;
                }
            }
        }
        public FrmTimKiemKhachHang(string P_KEYSEARCH)
        {
            InitializeComponent();
            dgvThongTinKhachHang.Columns["STT"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvThongTinKhachHang.Columns["STT"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongTinKhachHang.Columns["MAKHACHHANG"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvThongTinKhachHang.Columns["MAKHACHHANG"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongTinKhachHang.Columns["MATHE"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvThongTinKhachHang.Columns["MATHE"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongTinKhachHang.Columns["TENKHACHHANG"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvThongTinKhachHang.Columns["TENKHACHHANG"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongTinKhachHang.Columns["DIACHI"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvThongTinKhachHang.Columns["DIACHI"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongTinKhachHang.Columns["DIENTHOAI"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvThongTinKhachHang.Columns["DIENTHOAI"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongTinKhachHang.Columns["CANCUOC_CONGDAN"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvThongTinKhachHang.Columns["CANCUOC_CONGDAN"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongTinKhachHang.Columns["NGAYSINH"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvThongTinKhachHang.Columns["NGAYSINH"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongTinKhachHang.Columns["NGAYDACBIET"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvThongTinKhachHang.Columns["NGAYDACBIET"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongTinKhachHang.Columns["SODIEM"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvThongTinKhachHang.Columns["SODIEM"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongTinKhachHang.Columns["TONGTIEN"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvThongTinKhachHang.Columns["TONGTIEN"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvThongTinKhachHang.Columns["NGAYDACBIET"].Visible = false;
            dgvThongTinKhachHang.Columns["CANCUOC_CONGDAN"].Visible = false;
            dgvThongTinKhachHang.Columns["SODIEM"].Visible = false;
            dgvThongTinKhachHang.Columns["TONGTIEN"].Visible = false;
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã thẻ" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Mã khách hàng" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Tên khách hàng" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 3, TEXT = "Số điện thoại" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 4, TEXT = "Số chứng minh thư" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 5, TEXT = "Số điểm" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            cboDieuKienTimKiem.SelectedIndex = 0;
            txtDieuKienTimKiem.Text = P_KEYSEARCH;
            List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = new List<KHACHHANG_DTO>();
            _LST_KHACHHANG_DTO = FrmThanhToanService.TIMKIEM_KHACHHANG_FROM_ORACLE(P_KEYSEARCH, 1, cboDieuKienTimKiem.SelectedIndex, Session.Session.CurrentUnitCode);
            BINDING_DATA_KHACHHANG_TO_GRIDVIEW(_LST_KHACHHANG_DTO);

        }

        private void btnTimKiemKhachHang_Click(object sender, System.EventArgs e)
        {
            string P_KEYSEARCH = txtDieuKienTimKiem.Text;
            int P_USE_TIMKIEM_ALL = 0;
            int P_DIEUKIEN_TIMKIEM = cboDieuKienTimKiem.SelectedIndex;
            List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = new List<KHACHHANG_DTO>();
            _LST_KHACHHANG_DTO = FrmThanhToanService.TIMKIEM_KHACHHANG_FROM_ORACLE(P_KEYSEARCH, P_USE_TIMKIEM_ALL,P_DIEUKIEN_TIMKIEM, Session.Session.CurrentUnitCode);
            BINDING_DATA_KHACHHANG_TO_GRIDVIEW(_LST_KHACHHANG_DTO);
        }

        public void SetHanlerTimKiemKhachHang(STATUS_TIMKIEM_KHACHHANG BINDING_DATA_CHANGE_FROM_GRID_TO_TEXT)
        {
            statusTimKiemKhachHang = BINDING_DATA_CHANGE_FROM_GRID_TO_TEXT;
        }

        private void dgvThongTinKhachHang_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            KHACHHANG_DTO _KHACHHANG_DTO= new KHACHHANG_DTO();
            _KHACHHANG_DTO.MAKHACHHANG = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["MAKHACHHANG"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["MAKHACHHANG"].Value.ToString() : "";
            _KHACHHANG_DTO.TENKHACHHANG = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["TENKHACHHANG"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["TENKHACHHANG"].Value.ToString() : "";
            _KHACHHANG_DTO.DIACHI = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["DIACHI"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["DIACHI"].Value.ToString() : "";
            _KHACHHANG_DTO.DIENTHOAI = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["DIENTHOAI"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["DIENTHOAI"].Value.ToString() : "";
            _KHACHHANG_DTO.CANCUOC_CONGDAN = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["CANCUOC_CONGDAN"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["CANCUOC_CONGDAN"].Value.ToString() : "";
            DateTime NGAYSINH;
            if(dgvThongTinKhachHang.Rows[e.RowIndex].Cells["NGAYSINH"].Value != null)
            {
                NGAYSINH = DateTime.ParseExact(dgvThongTinKhachHang.Rows[e.RowIndex].Cells["NGAYSINH"].Value.ToString(), "dd/MM/yyyy", null);
                _KHACHHANG_DTO.NGAYSINH = NGAYSINH;
            }
            //DateTime NGAYDACBIET;
            //if (dgvThongTinKhachHang.Rows[e.RowIndex].Cells["NGAYDACBIET"].Value != null)
            //{
            //    NGAYDACBIET = DateTime.ParseExact(dgvThongTinKhachHang.Rows[e.RowIndex].Cells["NGAYDACBIET"].Value.ToString(), "dd/MM/yyyy", null);
            //    _KHACHHANG_DTO.NGAYDACBIET = NGAYDACBIET;
            //}
            decimal SODIEM = 0;
            if (dgvThongTinKhachHang.Rows[e.RowIndex].Cells["SODIEM"].Value != null)
            {
                decimal.TryParse(dgvThongTinKhachHang.Rows[e.RowIndex].Cells["SODIEM"].Value.ToString(), out SODIEM);
            }
            _KHACHHANG_DTO.SODIEM = SODIEM;
            decimal TONGTIEN = 0;
            if (dgvThongTinKhachHang.Rows[e.RowIndex].Cells["TONGTIEN"].Value != null)
            {
                decimal.TryParse(dgvThongTinKhachHang.Rows[e.RowIndex].Cells["TONGTIEN"].Value.ToString(), out TONGTIEN);
            }
            _KHACHHANG_DTO.TONGTIEN = TONGTIEN;
            statusTimKiemKhachHang(_KHACHHANG_DTO);
            this.Close();
            this.Dispose();
        }

        private void txtDieuKienTimKiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtDieuKienTimKiem.Text))
                {
                    string P_KEYSEARCH = txtDieuKienTimKiem.Text;
                    int P_USE_TIMKIEM_ALL = 1;
                    int P_DIEUKIEN_TIMKIEM = cboDieuKienTimKiem.SelectedIndex;
                    List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = new List<KHACHHANG_DTO>();
                    _LST_KHACHHANG_DTO = FrmThanhToanService.TIMKIEM_KHACHHANG_FROM_ORACLE(P_KEYSEARCH, P_USE_TIMKIEM_ALL, P_DIEUKIEN_TIMKIEM, Session.Session.CurrentUnitCode);
                    BINDING_DATA_KHACHHANG_TO_GRIDVIEW(_LST_KHACHHANG_DTO);
                }
            }
        }

        private void txtDieuKienTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDieuKienTimKiem.Text))
            {
                string P_KEYSEARCH = txtDieuKienTimKiem.Text;
                int P_USE_TIMKIEM_ALL = 1;
                int P_DIEUKIEN_TIMKIEM = cboDieuKienTimKiem.SelectedIndex;
                List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = new List<KHACHHANG_DTO>();
                _LST_KHACHHANG_DTO = FrmThanhToanService.TIMKIEM_KHACHHANG_FROM_ORACLE(P_KEYSEARCH, P_USE_TIMKIEM_ALL, P_DIEUKIEN_TIMKIEM, Session.Session.CurrentUnitCode);
                BINDING_DATA_KHACHHANG_TO_GRIDVIEW(_LST_KHACHHANG_DTO);
            }
        }
    }
}
