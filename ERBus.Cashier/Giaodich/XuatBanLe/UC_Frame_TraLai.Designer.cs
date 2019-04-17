using System.Linq;
using System.Windows.Forms;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    partial class UC_Frame_TraLai
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dateTimeTuNgay = new System.Windows.Forms.DateTimePicker();
            this.dateTimeDenNgay = new System.Windows.Forms.DateTimePicker();
            this.cboDieuKienChon = new System.Windows.Forms.ComboBox();
            this.btnTimKiem = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtKeySearch = new System.Windows.Forms.TextBox();
            this.lblMaGiaoDich = new DevExpress.XtraEditors.LabelControl();
            this.lblNgayPhatSinh = new DevExpress.XtraEditors.LabelControl();
            this.dgvDetails_Tab = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtChiTiet_ThoiGian = new System.Windows.Forms.TextBox();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.txtChiTiet_ThuNgan = new System.Windows.Forms.TextBox();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.txtChiTiet_TienTraLai = new System.Windows.Forms.TextBox();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtChiTiet_TienChietKhau = new System.Windows.Forms.TextBox();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtChiTiet_TongTien = new System.Windows.Forms.TextBox();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtChiTiet_TongSoLuong = new System.Windows.Forms.TextBox();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnF2ThemMoiTraLai = new DevExpress.XtraEditors.SimpleButton();
            this.btnF4TangHang = new DevExpress.XtraEditors.SimpleButton();
            this.btnF3GiamHang = new DevExpress.XtraEditors.SimpleButton();
            this.btnF6TimKiem = new DevExpress.XtraEditors.SimpleButton();
            this.btnF5LamMoiGiaoDich = new DevExpress.XtraEditors.SimpleButton();
            this.btnF1ThanhToanTraLai = new DevExpress.XtraEditors.SimpleButton();
            this.txtTraLai_MaGiaoDich = new System.Windows.Forms.TextBox();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.txtTraLai_SoLuong = new System.Windows.Forms.TextBox();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.txtTraLai_MaVatTu = new System.Windows.Forms.TextBox();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.txtTraLai_ThuNgan = new System.Windows.Forms.TextBox();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.txtTraLai_TongTien = new System.Windows.Forms.TextBox();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.txtTraLai_TongSoLuong = new System.Windows.Forms.TextBox();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.dgvTraLai = new System.Windows.Forms.DataGridView();
            this.KEY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAHANG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SOLUONG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GIABANLE_VAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.THANHTIEN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TENHANG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DONVITINH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GIAVON = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TIEN_KHUYENMAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KEY_TRALAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAHANG_TRALAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TENHANG_TRALAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SOLUONG_TRALAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GIABANLE_VAT_TRALAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TIEN_KHUYENMAI_TRALAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.THANHTIEN_TRALAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DONVITINH_TRALAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GIAVON_TRALAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails_Tab)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraLai)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelHeader.Controls.Add(this.labelControl2);
            this.panelHeader.Controls.Add(this.dateTimeTuNgay);
            this.panelHeader.Controls.Add(this.dateTimeDenNgay);
            this.panelHeader.Controls.Add(this.cboDieuKienChon);
            this.panelHeader.Controls.Add(this.btnTimKiem);
            this.panelHeader.Controls.Add(this.labelControl1);
            this.panelHeader.Controls.Add(this.txtKeySearch);
            this.panelHeader.Controls.Add(this.lblMaGiaoDich);
            this.panelHeader.Controls.Add(this.lblNgayPhatSinh);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1366, 71);
            this.panelHeader.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(933, 22);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(74, 22);
            this.labelControl2.TabIndex = 42;
            this.labelControl2.Text = "Lọc theo";
            // 
            // dateTimeTuNgay
            // 
            this.dateTimeTuNgay.CalendarFont = new System.Drawing.Font("Tunga", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeTuNgay.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeTuNgay.Location = new System.Drawing.Point(462, 18);
            this.dateTimeTuNgay.Name = "dateTimeTuNgay";
            this.dateTimeTuNgay.Size = new System.Drawing.Size(181, 26);
            this.dateTimeTuNgay.TabIndex = 41;
            // 
            // dateTimeDenNgay
            // 
            this.dateTimeDenNgay.CalendarFont = new System.Drawing.Font("Tunga", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeDenNgay.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeDenNgay.Location = new System.Drawing.Point(740, 19);
            this.dateTimeDenNgay.Name = "dateTimeDenNgay";
            this.dateTimeDenNgay.Size = new System.Drawing.Size(179, 26);
            this.dateTimeDenNgay.TabIndex = 40;
            // 
            // cboDieuKienChon
            // 
            this.cboDieuKienChon.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDieuKienChon.FormattingEnabled = true;
            this.cboDieuKienChon.Location = new System.Drawing.Point(1017, 17);
            this.cboDieuKienChon.Name = "cboDieuKienChon";
            this.cboDieuKienChon.Size = new System.Drawing.Size(148, 26);
            this.cboDieuKienChon.TabIndex = 39;
            this.cboDieuKienChon.SelectedValueChanged += new System.EventHandler(this.cboDieuKienChon_SelectedValueChanged);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Appearance.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimKiem.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnTimKiem.Appearance.Options.UseFont = true;
            this.btnTimKiem.Appearance.Options.UseForeColor = true;
            this.btnTimKiem.Location = new System.Drawing.Point(1171, 14);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(104, 31);
            this.btnTimKiem.TabIndex = 36;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(653, 22);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(80, 22);
            this.labelControl1.TabIndex = 34;
            this.labelControl1.Text = "Đến ngày";
            // 
            // txtKeySearch
            // 
            this.txtKeySearch.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeySearch.Location = new System.Drawing.Point(94, 17);
            this.txtKeySearch.Name = "txtKeySearch";
            this.txtKeySearch.Size = new System.Drawing.Size(281, 26);
            this.txtKeySearch.TabIndex = 32;
            this.txtKeySearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeySearch_KeyDown);
            this.txtKeySearch.Validating += new System.ComponentModel.CancelEventHandler(this.txtKeySearch_Validating);
            // 
            // lblMaGiaoDich
            // 
            this.lblMaGiaoDich.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaGiaoDich.Appearance.Options.UseFont = true;
            this.lblMaGiaoDich.Location = new System.Drawing.Point(10, 22);
            this.lblMaGiaoDich.Name = "lblMaGiaoDich";
            this.lblMaGiaoDich.Size = new System.Drawing.Size(70, 22);
            this.lblMaGiaoDich.TabIndex = 30;
            this.lblMaGiaoDich.Text = "Từ khóa";
            // 
            // lblNgayPhatSinh
            // 
            this.lblNgayPhatSinh.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayPhatSinh.Appearance.Options.UseFont = true;
            this.lblNgayPhatSinh.Location = new System.Drawing.Point(385, 21);
            this.lblNgayPhatSinh.Name = "lblNgayPhatSinh";
            this.lblNgayPhatSinh.Size = new System.Drawing.Size(70, 22);
            this.lblNgayPhatSinh.TabIndex = 31;
            this.lblNgayPhatSinh.Text = "Từ ngày";
            // 
            // dgvDetails_Tab
            // 
            this.dgvDetails_Tab.AllowUserToAddRows = false;
            this.dgvDetails_Tab.AllowUserToDeleteRows = false;
            this.dgvDetails_Tab.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetails_Tab.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dgvDetails_Tab.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDetails_Tab.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDetails_Tab.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetails_Tab.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.KEY,
            this.MAHANG,
            this.SOLUONG,
            this.GIABANLE_VAT,
            this.THANHTIEN,
            this.TENHANG,
            this.DONVITINH,
            this.GIAVON,
            this.TIEN_KHUYENMAI});
            this.dgvDetails_Tab.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvDetails_Tab.Location = new System.Drawing.Point(3, 23);
            this.dgvDetails_Tab.MultiSelect = false;
            this.dgvDetails_Tab.Name = "dgvDetails_Tab";
            this.dgvDetails_Tab.ReadOnly = true;
            this.dgvDetails_Tab.RowHeadersVisible = false;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDetails_Tab.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvDetails_Tab.RowTemplate.Height = 30;
            this.dgvDetails_Tab.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetails_Tab.Size = new System.Drawing.Size(483, 327);
            this.dgvDetails_Tab.TabIndex = 2;
            this.dgvDetails_Tab.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDetails_Tab_CellMouseDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.txtChiTiet_ThoiGian);
            this.groupBox1.Controls.Add(this.labelControl8);
            this.groupBox1.Controls.Add(this.txtChiTiet_ThuNgan);
            this.groupBox1.Controls.Add(this.labelControl7);
            this.groupBox1.Controls.Add(this.txtChiTiet_TienTraLai);
            this.groupBox1.Controls.Add(this.labelControl6);
            this.groupBox1.Controls.Add(this.txtChiTiet_TienChietKhau);
            this.groupBox1.Controls.Add(this.labelControl5);
            this.groupBox1.Controls.Add(this.txtChiTiet_TongTien);
            this.groupBox1.Controls.Add(this.labelControl4);
            this.groupBox1.Controls.Add(this.txtChiTiet_TongSoLuong);
            this.groupBox1.Controls.Add(this.labelControl3);
            this.groupBox1.Controls.Add(this.dgvDetails_Tab);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(0, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(489, 511);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chi tiết giao dịch";
            // 
            // txtChiTiet_ThoiGian
            // 
            this.txtChiTiet_ThoiGian.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChiTiet_ThoiGian.Enabled = false;
            this.txtChiTiet_ThoiGian.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChiTiet_ThoiGian.ForeColor = System.Drawing.Color.Blue;
            this.txtChiTiet_ThoiGian.Location = new System.Drawing.Point(346, 461);
            this.txtChiTiet_ThoiGian.Name = "txtChiTiet_ThoiGian";
            this.txtChiTiet_ThoiGian.ReadOnly = true;
            this.txtChiTiet_ThoiGian.Size = new System.Drawing.Size(126, 29);
            this.txtChiTiet_ThoiGian.TabIndex = 51;
            this.txtChiTiet_ThoiGian.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelControl8
            // 
            this.labelControl8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Enabled = false;
            this.labelControl8.Location = new System.Drawing.Point(241, 465);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(78, 22);
            this.labelControl8.TabIndex = 52;
            this.labelControl8.Text = "Thời gian";
            // 
            // txtChiTiet_ThuNgan
            // 
            this.txtChiTiet_ThuNgan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChiTiet_ThuNgan.Enabled = false;
            this.txtChiTiet_ThuNgan.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChiTiet_ThuNgan.ForeColor = System.Drawing.Color.Blue;
            this.txtChiTiet_ThuNgan.Location = new System.Drawing.Point(113, 465);
            this.txtChiTiet_ThuNgan.Name = "txtChiTiet_ThuNgan";
            this.txtChiTiet_ThuNgan.ReadOnly = true;
            this.txtChiTiet_ThuNgan.Size = new System.Drawing.Size(96, 29);
            this.txtChiTiet_ThuNgan.TabIndex = 49;
            this.txtChiTiet_ThuNgan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelControl7
            // 
            this.labelControl7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Enabled = false;
            this.labelControl7.Location = new System.Drawing.Point(4, 468);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(97, 22);
            this.labelControl7.TabIndex = 50;
            this.labelControl7.Text = "THU NGÂN";
            // 
            // txtChiTiet_TienTraLai
            // 
            this.txtChiTiet_TienTraLai.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChiTiet_TienTraLai.Enabled = false;
            this.txtChiTiet_TienTraLai.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChiTiet_TienTraLai.ForeColor = System.Drawing.Color.Blue;
            this.txtChiTiet_TienTraLai.Location = new System.Drawing.Point(346, 416);
            this.txtChiTiet_TienTraLai.Name = "txtChiTiet_TienTraLai";
            this.txtChiTiet_TienTraLai.ReadOnly = true;
            this.txtChiTiet_TienTraLai.Size = new System.Drawing.Size(126, 29);
            this.txtChiTiet_TienTraLai.TabIndex = 47;
            this.txtChiTiet_TienTraLai.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelControl6
            // 
            this.labelControl6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Enabled = false;
            this.labelControl6.Location = new System.Drawing.Point(240, 419);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(85, 22);
            this.labelControl6.TabIndex = 48;
            this.labelControl6.Text = "Tiền trả lại";
            // 
            // txtChiTiet_TienChietKhau
            // 
            this.txtChiTiet_TienChietKhau.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChiTiet_TienChietKhau.Enabled = false;
            this.txtChiTiet_TienChietKhau.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChiTiet_TienChietKhau.ForeColor = System.Drawing.Color.Blue;
            this.txtChiTiet_TienChietKhau.Location = new System.Drawing.Point(113, 416);
            this.txtChiTiet_TienChietKhau.Name = "txtChiTiet_TienChietKhau";
            this.txtChiTiet_TienChietKhau.ReadOnly = true;
            this.txtChiTiet_TienChietKhau.Size = new System.Drawing.Size(96, 29);
            this.txtChiTiet_TienChietKhau.TabIndex = 45;
            this.txtChiTiet_TienChietKhau.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelControl5
            // 
            this.labelControl5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Enabled = false;
            this.labelControl5.Location = new System.Drawing.Point(6, 419);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(73, 22);
            this.labelControl5.TabIndex = 46;
            this.labelControl5.Text = "Tiền C.K";
            // 
            // txtChiTiet_TongTien
            // 
            this.txtChiTiet_TongTien.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChiTiet_TongTien.Enabled = false;
            this.txtChiTiet_TongTien.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChiTiet_TongTien.ForeColor = System.Drawing.Color.Blue;
            this.txtChiTiet_TongTien.Location = new System.Drawing.Point(346, 367);
            this.txtChiTiet_TongTien.Name = "txtChiTiet_TongTien";
            this.txtChiTiet_TongTien.ReadOnly = true;
            this.txtChiTiet_TongTien.Size = new System.Drawing.Size(126, 29);
            this.txtChiTiet_TongTien.TabIndex = 43;
            this.txtChiTiet_TongTien.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelControl4
            // 
            this.labelControl4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Enabled = false;
            this.labelControl4.Location = new System.Drawing.Point(240, 371);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(79, 22);
            this.labelControl4.TabIndex = 44;
            this.labelControl4.Text = "Tổng tiền";
            // 
            // txtChiTiet_TongSoLuong
            // 
            this.txtChiTiet_TongSoLuong.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChiTiet_TongSoLuong.Enabled = false;
            this.txtChiTiet_TongSoLuong.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChiTiet_TongSoLuong.ForeColor = System.Drawing.Color.Blue;
            this.txtChiTiet_TongSoLuong.Location = new System.Drawing.Point(113, 367);
            this.txtChiTiet_TongSoLuong.Name = "txtChiTiet_TongSoLuong";
            this.txtChiTiet_TongSoLuong.ReadOnly = true;
            this.txtChiTiet_TongSoLuong.Size = new System.Drawing.Size(96, 29);
            this.txtChiTiet_TongSoLuong.TabIndex = 42;
            this.txtChiTiet_TongSoLuong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Enabled = false;
            this.labelControl3.Location = new System.Drawing.Point(6, 370);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(78, 22);
            this.labelControl3.TabIndex = 42;
            this.labelControl3.Text = "Tổng S.L";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox2.Controls.Add(this.btnF2ThemMoiTraLai);
            this.groupBox2.Controls.Add(this.btnF4TangHang);
            this.groupBox2.Controls.Add(this.btnF3GiamHang);
            this.groupBox2.Controls.Add(this.btnF6TimKiem);
            this.groupBox2.Controls.Add(this.btnF5LamMoiGiaoDich);
            this.groupBox2.Controls.Add(this.btnF1ThanhToanTraLai);
            this.groupBox2.Controls.Add(this.txtTraLai_MaGiaoDich);
            this.groupBox2.Controls.Add(this.labelControl14);
            this.groupBox2.Controls.Add(this.txtTraLai_SoLuong);
            this.groupBox2.Controls.Add(this.labelControl13);
            this.groupBox2.Controls.Add(this.txtTraLai_MaVatTu);
            this.groupBox2.Controls.Add(this.labelControl12);
            this.groupBox2.Controls.Add(this.txtTraLai_ThuNgan);
            this.groupBox2.Controls.Add(this.labelControl11);
            this.groupBox2.Controls.Add(this.txtTraLai_TongTien);
            this.groupBox2.Controls.Add(this.labelControl10);
            this.groupBox2.Controls.Add(this.txtTraLai_TongSoLuong);
            this.groupBox2.Controls.Add(this.labelControl9);
            this.groupBox2.Controls.Add(this.dgvTraLai);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Red;
            this.groupBox2.Location = new System.Drawing.Point(515, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(852, 511);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Giao dịch trả lại";
            // 
            // btnF2ThemMoiTraLai
            // 
            this.btnF2ThemMoiTraLai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnF2ThemMoiTraLai.Appearance.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnF2ThemMoiTraLai.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnF2ThemMoiTraLai.Appearance.Options.UseFont = true;
            this.btnF2ThemMoiTraLai.Appearance.Options.UseForeColor = true;
            this.btnF2ThemMoiTraLai.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.btnF2ThemMoiTraLai.Enabled = false;
            this.btnF2ThemMoiTraLai.Location = new System.Drawing.Point(262, 479);
            this.btnF2ThemMoiTraLai.Name = "btnF2ThemMoiTraLai";
            this.btnF2ThemMoiTraLai.Size = new System.Drawing.Size(126, 31);
            this.btnF2ThemMoiTraLai.TabIndex = 67;
            this.btnF2ThemMoiTraLai.Text = "F2->Thêm mới";
            this.btnF2ThemMoiTraLai.Click += new System.EventHandler(this.btnF2ThemMoiTraLai_Click);
            // 
            // btnF4TangHang
            // 
            this.btnF4TangHang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnF4TangHang.Appearance.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnF4TangHang.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnF4TangHang.Appearance.Options.UseFont = true;
            this.btnF4TangHang.Appearance.Options.UseForeColor = true;
            this.btnF4TangHang.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.btnF4TangHang.Enabled = false;
            this.btnF4TangHang.Location = new System.Drawing.Point(514, 479);
            this.btnF4TangHang.Name = "btnF4TangHang";
            this.btnF4TangHang.Size = new System.Drawing.Size(119, 31);
            this.btnF4TangHang.TabIndex = 66;
            this.btnF4TangHang.Text = "F4->Tăng hàng";
            // 
            // btnF3GiamHang
            // 
            this.btnF3GiamHang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnF3GiamHang.Appearance.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnF3GiamHang.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnF3GiamHang.Appearance.Options.UseFont = true;
            this.btnF3GiamHang.Appearance.Options.UseForeColor = true;
            this.btnF3GiamHang.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.btnF3GiamHang.Enabled = false;
            this.btnF3GiamHang.Location = new System.Drawing.Point(388, 479);
            this.btnF3GiamHang.Name = "btnF3GiamHang";
            this.btnF3GiamHang.Size = new System.Drawing.Size(126, 31);
            this.btnF3GiamHang.TabIndex = 65;
            this.btnF3GiamHang.Text = "F3->Giảm hàng";
            // 
            // btnF6TimKiem
            // 
            this.btnF6TimKiem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnF6TimKiem.Appearance.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnF6TimKiem.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnF6TimKiem.Appearance.Options.UseFont = true;
            this.btnF6TimKiem.Appearance.Options.UseForeColor = true;
            this.btnF6TimKiem.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.btnF6TimKiem.Location = new System.Drawing.Point(743, 479);
            this.btnF6TimKiem.Name = "btnF6TimKiem";
            this.btnF6TimKiem.Size = new System.Drawing.Size(108, 31);
            this.btnF6TimKiem.TabIndex = 64;
            this.btnF6TimKiem.Text = "F6->Tìm kiếm";
            // 
            // btnF5LamMoiGiaoDich
            // 
            this.btnF5LamMoiGiaoDich.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnF5LamMoiGiaoDich.Appearance.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnF5LamMoiGiaoDich.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnF5LamMoiGiaoDich.Appearance.Options.UseFont = true;
            this.btnF5LamMoiGiaoDich.Appearance.Options.UseForeColor = true;
            this.btnF5LamMoiGiaoDich.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.btnF5LamMoiGiaoDich.Enabled = false;
            this.btnF5LamMoiGiaoDich.Location = new System.Drawing.Point(633, 479);
            this.btnF5LamMoiGiaoDich.Name = "btnF5LamMoiGiaoDich";
            this.btnF5LamMoiGiaoDich.Size = new System.Drawing.Size(111, 31);
            this.btnF5LamMoiGiaoDich.TabIndex = 63;
            this.btnF5LamMoiGiaoDich.Text = "F5->Làm mới";
            // 
            // btnF1ThanhToanTraLai
            // 
            this.btnF1ThanhToanTraLai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnF1ThanhToanTraLai.Appearance.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnF1ThanhToanTraLai.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.btnF1ThanhToanTraLai.Appearance.Options.UseFont = true;
            this.btnF1ThanhToanTraLai.Appearance.Options.UseForeColor = true;
            this.btnF1ThanhToanTraLai.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.btnF1ThanhToanTraLai.Enabled = false;
            this.btnF1ThanhToanTraLai.Location = new System.Drawing.Point(136, 479);
            this.btnF1ThanhToanTraLai.Name = "btnF1ThanhToanTraLai";
            this.btnF1ThanhToanTraLai.Size = new System.Drawing.Size(126, 31);
            this.btnF1ThanhToanTraLai.TabIndex = 43;
            this.btnF1ThanhToanTraLai.Text = "F1->Thanh toán";
            // 
            // txtTraLai_MaGiaoDich
            // 
            this.txtTraLai_MaGiaoDich.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTraLai_MaGiaoDich.Enabled = false;
            this.txtTraLai_MaGiaoDich.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTraLai_MaGiaoDich.ForeColor = System.Drawing.Color.Red;
            this.txtTraLai_MaGiaoDich.Location = new System.Drawing.Point(521, 382);
            this.txtTraLai_MaGiaoDich.Name = "txtTraLai_MaGiaoDich";
            this.txtTraLai_MaGiaoDich.ReadOnly = true;
            this.txtTraLai_MaGiaoDich.Size = new System.Drawing.Size(325, 26);
            this.txtTraLai_MaGiaoDich.TabIndex = 61;
            this.txtTraLai_MaGiaoDich.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelControl14
            // 
            this.labelControl14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl14.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl14.Appearance.Options.UseFont = true;
            this.labelControl14.Location = new System.Drawing.Point(423, 385);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(80, 22);
            this.labelControl14.TabIndex = 62;
            this.labelControl14.Text = "Giao dịch";
            // 
            // txtTraLai_SoLuong
            // 
            this.txtTraLai_SoLuong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTraLai_SoLuong.Enabled = false;
            this.txtTraLai_SoLuong.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTraLai_SoLuong.ForeColor = System.Drawing.Color.Red;
            this.txtTraLai_SoLuong.Location = new System.Drawing.Point(760, 334);
            this.txtTraLai_SoLuong.Name = "txtTraLai_SoLuong";
            this.txtTraLai_SoLuong.Size = new System.Drawing.Size(86, 29);
            this.txtTraLai_SoLuong.TabIndex = 2;
            this.txtTraLai_SoLuong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTraLai_SoLuong.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTraLai_SoLuong_KeyDown);
            this.txtTraLai_SoLuong.Validating += new System.ComponentModel.CancelEventHandler(this.txtTraLai_SoLuong_Validating);
            // 
            // labelControl13
            // 
            this.labelControl13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl13.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl13.Appearance.Options.UseFont = true;
            this.labelControl13.Location = new System.Drawing.Point(670, 336);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(79, 22);
            this.labelControl13.TabIndex = 60;
            this.labelControl13.Text = "Số lượng";
            // 
            // txtTraLai_MaVatTu
            // 
            this.txtTraLai_MaVatTu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTraLai_MaVatTu.Enabled = false;
            this.txtTraLai_MaVatTu.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTraLai_MaVatTu.ForeColor = System.Drawing.Color.Red;
            this.txtTraLai_MaVatTu.Location = new System.Drawing.Point(521, 334);
            this.txtTraLai_MaVatTu.Name = "txtTraLai_MaVatTu";
            this.txtTraLai_MaVatTu.Size = new System.Drawing.Size(137, 29);
            this.txtTraLai_MaVatTu.TabIndex = 1;
            this.txtTraLai_MaVatTu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelControl12
            // 
            this.labelControl12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl12.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl12.Appearance.Options.UseFont = true;
            this.labelControl12.Location = new System.Drawing.Point(424, 338);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(77, 22);
            this.labelControl12.TabIndex = 54;
            this.labelControl12.Text = "Mã vật tư";
            // 
            // txtTraLai_ThuNgan
            // 
            this.txtTraLai_ThuNgan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTraLai_ThuNgan.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTraLai_ThuNgan.ForeColor = System.Drawing.Color.Red;
            this.txtTraLai_ThuNgan.Location = new System.Drawing.Point(138, 439);
            this.txtTraLai_ThuNgan.Name = "txtTraLai_ThuNgan";
            this.txtTraLai_ThuNgan.ReadOnly = true;
            this.txtTraLai_ThuNgan.Size = new System.Drawing.Size(290, 26);
            this.txtTraLai_ThuNgan.TabIndex = 57;
            this.txtTraLai_ThuNgan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelControl11
            // 
            this.labelControl11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl11.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl11.Appearance.Options.UseFont = true;
            this.labelControl11.Location = new System.Drawing.Point(27, 443);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(78, 22);
            this.labelControl11.TabIndex = 58;
            this.labelControl11.Text = "Thu ngân";
            // 
            // txtTraLai_TongTien
            // 
            this.txtTraLai_TongTien.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTraLai_TongTien.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTraLai_TongTien.ForeColor = System.Drawing.Color.Red;
            this.txtTraLai_TongTien.Location = new System.Drawing.Point(139, 398);
            this.txtTraLai_TongTien.Name = "txtTraLai_TongTien";
            this.txtTraLai_TongTien.ReadOnly = true;
            this.txtTraLai_TongTien.Size = new System.Drawing.Size(115, 29);
            this.txtTraLai_TongTien.TabIndex = 55;
            this.txtTraLai_TongTien.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelControl10
            // 
            this.labelControl10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl10.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl10.Appearance.Options.UseFont = true;
            this.labelControl10.Location = new System.Drawing.Point(18, 402);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(105, 22);
            this.labelControl10.TabIndex = 56;
            this.labelControl10.Text = "Tổng tiền trả";
            // 
            // txtTraLai_TongSoLuong
            // 
            this.txtTraLai_TongSoLuong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTraLai_TongSoLuong.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTraLai_TongSoLuong.ForeColor = System.Drawing.Color.Red;
            this.txtTraLai_TongSoLuong.Location = new System.Drawing.Point(139, 362);
            this.txtTraLai_TongSoLuong.Name = "txtTraLai_TongSoLuong";
            this.txtTraLai_TongSoLuong.ReadOnly = true;
            this.txtTraLai_TongSoLuong.Size = new System.Drawing.Size(115, 29);
            this.txtTraLai_TongSoLuong.TabIndex = 53;
            this.txtTraLai_TongSoLuong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelControl9
            // 
            this.labelControl9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Location = new System.Drawing.Point(19, 365);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(103, 22);
            this.labelControl9.TabIndex = 54;
            this.labelControl9.Text = "Tổng S.L trả";
            // 
            // dgvTraLai
            // 
            this.dgvTraLai.AllowUserToAddRows = false;
            this.dgvTraLai.AllowUserToDeleteRows = false;
            this.dgvTraLai.BackgroundColor = System.Drawing.Color.White;
            this.dgvTraLai.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dgvTraLai.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTraLai.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvTraLai.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTraLai.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.KEY_TRALAI,
            this.MAHANG_TRALAI,
            this.TENHANG_TRALAI,
            this.SOLUONG_TRALAI,
            this.GIABANLE_VAT_TRALAI,
            this.TIEN_KHUYENMAI_TRALAI,
            this.THANHTIEN_TRALAI,
            this.DONVITINH_TRALAI,
            this.GIAVON_TRALAI});
            this.dgvTraLai.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvTraLai.Location = new System.Drawing.Point(3, 23);
            this.dgvTraLai.MultiSelect = false;
            this.dgvTraLai.Name = "dgvTraLai";
            this.dgvTraLai.ReadOnly = true;
            this.dgvTraLai.RowHeadersVisible = false;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTraLai.RowsDefaultCellStyle = dataGridViewCellStyle20;
            this.dgvTraLai.RowTemplate.Height = 30;
            this.dgvTraLai.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTraLai.Size = new System.Drawing.Size(846, 327);
            this.dgvTraLai.TabIndex = 3;
            this.dgvTraLai.SelectionChanged += new System.EventHandler(this.dgvTraLai_SelectionChanged);
            // 
            // KEY
            // 
            this.KEY.DataPropertyName = "KEY";
            this.KEY.HeaderText = "STT";
            this.KEY.MinimumWidth = 60;
            this.KEY.Name = "KEY";
            this.KEY.ReadOnly = true;
            this.KEY.Width = 60;
            // 
            // MAHANG
            // 
            this.MAHANG.DataPropertyName = "MAHANG";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 15.75F);
            this.MAHANG.DefaultCellStyle = dataGridViewCellStyle2;
            this.MAHANG.HeaderText = "Mã hàng";
            this.MAHANG.MinimumWidth = 110;
            this.MAHANG.Name = "MAHANG";
            this.MAHANG.ReadOnly = true;
            this.MAHANG.Width = 110;
            // 
            // SOLUONG
            // 
            this.SOLUONG.DataPropertyName = "SOLUONG";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 15.75F);
            this.SOLUONG.DefaultCellStyle = dataGridViewCellStyle3;
            this.SOLUONG.HeaderText = "Số lượng";
            this.SOLUONG.MinimumWidth = 100;
            this.SOLUONG.Name = "SOLUONG";
            this.SOLUONG.ReadOnly = true;
            // 
            // GIABANLE_VAT
            // 
            this.GIABANLE_VAT.DataPropertyName = "GIABANLE_VAT";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 15.75F);
            this.GIABANLE_VAT.DefaultCellStyle = dataGridViewCellStyle4;
            this.GIABANLE_VAT.HeaderText = "Đơn giá";
            this.GIABANLE_VAT.MinimumWidth = 100;
            this.GIABANLE_VAT.Name = "GIABANLE_VAT";
            this.GIABANLE_VAT.ReadOnly = true;
            // 
            // THANHTIEN
            // 
            this.THANHTIEN.DataPropertyName = "THANHTIEN";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.THANHTIEN.DefaultCellStyle = dataGridViewCellStyle5;
            this.THANHTIEN.HeaderText = "Thành tiền";
            this.THANHTIEN.MinimumWidth = 110;
            this.THANHTIEN.Name = "THANHTIEN";
            this.THANHTIEN.ReadOnly = true;
            this.THANHTIEN.Width = 110;
            // 
            // TENHANG
            // 
            this.TENHANG.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TENHANG.DataPropertyName = "TENHANG";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 15.75F);
            this.TENHANG.DefaultCellStyle = dataGridViewCellStyle6;
            this.TENHANG.HeaderText = "Tên hàng";
            this.TENHANG.MinimumWidth = 200;
            this.TENHANG.Name = "TENHANG";
            this.TENHANG.ReadOnly = true;
            // 
            // DONVITINH
            // 
            this.DONVITINH.DataPropertyName = "DONVITINH";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 15.75F);
            this.DONVITINH.DefaultCellStyle = dataGridViewCellStyle7;
            this.DONVITINH.HeaderText = "Đ.v tính";
            this.DONVITINH.MinimumWidth = 140;
            this.DONVITINH.Name = "DONVITINH";
            this.DONVITINH.ReadOnly = true;
            this.DONVITINH.Width = 140;
            // 
            // GIAVON
            // 
            this.GIAVON.DataPropertyName = "GIAVON";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 15.75F);
            this.GIAVON.DefaultCellStyle = dataGridViewCellStyle8;
            this.GIAVON.HeaderText = "Giá vốn";
            this.GIAVON.MinimumWidth = 150;
            this.GIAVON.Name = "GIAVON";
            this.GIAVON.ReadOnly = true;
            this.GIAVON.Width = 150;
            // 
            // TIEN_KHUYENMAI
            // 
            this.TIEN_KHUYENMAI.DataPropertyName = "TIEN_KHUYENMAI";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 15.75F);
            this.TIEN_KHUYENMAI.DefaultCellStyle = dataGridViewCellStyle9;
            this.TIEN_KHUYENMAI.HeaderText = "Tiền K.M";
            this.TIEN_KHUYENMAI.MinimumWidth = 150;
            this.TIEN_KHUYENMAI.Name = "TIEN_KHUYENMAI";
            this.TIEN_KHUYENMAI.ReadOnly = true;
            this.TIEN_KHUYENMAI.Width = 150;
            // 
            // KEY_TRALAI
            // 
            this.KEY_TRALAI.DataPropertyName = "KEY_TRALAI";
            this.KEY_TRALAI.HeaderText = "STT";
            this.KEY_TRALAI.Name = "KEY_TRALAI";
            this.KEY_TRALAI.ReadOnly = true;
            this.KEY_TRALAI.Width = 50;
            // 
            // MAHANG_TRALAI
            // 
            this.MAHANG_TRALAI.DataPropertyName = "MAHANG_TRALAI";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Arial", 15.75F);
            this.MAHANG_TRALAI.DefaultCellStyle = dataGridViewCellStyle12;
            this.MAHANG_TRALAI.HeaderText = "Mã hàng";
            this.MAHANG_TRALAI.MinimumWidth = 110;
            this.MAHANG_TRALAI.Name = "MAHANG_TRALAI";
            this.MAHANG_TRALAI.ReadOnly = true;
            this.MAHANG_TRALAI.Width = 110;
            // 
            // TENHANG_TRALAI
            // 
            this.TENHANG_TRALAI.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TENHANG_TRALAI.DataPropertyName = "TENHANG_TRALAI";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Arial", 15.75F);
            this.TENHANG_TRALAI.DefaultCellStyle = dataGridViewCellStyle13;
            this.TENHANG_TRALAI.HeaderText = "Tên hàng";
            this.TENHANG_TRALAI.MinimumWidth = 240;
            this.TENHANG_TRALAI.Name = "TENHANG_TRALAI";
            this.TENHANG_TRALAI.ReadOnly = true;
            // 
            // SOLUONG_TRALAI
            // 
            this.SOLUONG_TRALAI.DataPropertyName = "SOLUONG_TRALAI";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Arial", 15.75F);
            this.SOLUONG_TRALAI.DefaultCellStyle = dataGridViewCellStyle14;
            this.SOLUONG_TRALAI.HeaderText = "Số lượng";
            this.SOLUONG_TRALAI.MinimumWidth = 100;
            this.SOLUONG_TRALAI.Name = "SOLUONG_TRALAI";
            this.SOLUONG_TRALAI.ReadOnly = true;
            // 
            // GIABANLE_VAT_TRALAI
            // 
            this.GIABANLE_VAT_TRALAI.DataPropertyName = "GIABANLE_VAT_TRALAI";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Arial", 15.75F);
            this.GIABANLE_VAT_TRALAI.DefaultCellStyle = dataGridViewCellStyle15;
            this.GIABANLE_VAT_TRALAI.HeaderText = "Đơn giá";
            this.GIABANLE_VAT_TRALAI.MinimumWidth = 100;
            this.GIABANLE_VAT_TRALAI.Name = "GIABANLE_VAT_TRALAI";
            this.GIABANLE_VAT_TRALAI.ReadOnly = true;
            // 
            // TIEN_KHUYENMAI_TRALAI
            // 
            this.TIEN_KHUYENMAI_TRALAI.DataPropertyName = "TIEN_KHUYENMAI_TRALAI";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Arial", 15.75F);
            this.TIEN_KHUYENMAI_TRALAI.DefaultCellStyle = dataGridViewCellStyle16;
            this.TIEN_KHUYENMAI_TRALAI.HeaderText = "Tiền K.M";
            this.TIEN_KHUYENMAI_TRALAI.MinimumWidth = 100;
            this.TIEN_KHUYENMAI_TRALAI.Name = "TIEN_KHUYENMAI_TRALAI";
            this.TIEN_KHUYENMAI_TRALAI.ReadOnly = true;
            // 
            // THANHTIEN_TRALAI
            // 
            this.THANHTIEN_TRALAI.DataPropertyName = "THANHTIEN_TRALAI";
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Arial", 15.75F);
            this.THANHTIEN_TRALAI.DefaultCellStyle = dataGridViewCellStyle17;
            this.THANHTIEN_TRALAI.HeaderText = "Thành tiền";
            this.THANHTIEN_TRALAI.MinimumWidth = 100;
            this.THANHTIEN_TRALAI.Name = "THANHTIEN_TRALAI";
            this.THANHTIEN_TRALAI.ReadOnly = true;
            // 
            // DONVITINH_TRALAI
            // 
            this.DONVITINH_TRALAI.DataPropertyName = "DONVITINH_TRALAI";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Arial", 15.75F);
            this.DONVITINH_TRALAI.DefaultCellStyle = dataGridViewCellStyle18;
            this.DONVITINH_TRALAI.HeaderText = "Đ.v tính";
            this.DONVITINH_TRALAI.MinimumWidth = 100;
            this.DONVITINH_TRALAI.Name = "DONVITINH_TRALAI";
            this.DONVITINH_TRALAI.ReadOnly = true;
            // 
            // GIAVON_TRALAI
            // 
            this.GIAVON_TRALAI.DataPropertyName = "GIAVON_TRALAI";
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Arial", 15.75F);
            this.GIAVON_TRALAI.DefaultCellStyle = dataGridViewCellStyle19;
            this.GIAVON_TRALAI.HeaderText = "Giá vốn";
            this.GIAVON_TRALAI.MinimumWidth = 100;
            this.GIAVON_TRALAI.Name = "GIAVON_TRALAI";
            this.GIAVON_TRALAI.ReadOnly = true;
            // 
            // UC_Frame_TraLai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panelHeader);
            this.Name = "UC_Frame_TraLai";
            this.Size = new System.Drawing.Size(1366, 582);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails_Tab)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraLai)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.DataGridView dgvDetails_Tab;
        private DevExpress.XtraEditors.SimpleButton btnTimKiem;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.TextBox txtKeySearch;
        private DevExpress.XtraEditors.LabelControl lblMaGiaoDich;
        private DevExpress.XtraEditors.LabelControl lblNgayPhatSinh;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvTraLai;
        private System.Windows.Forms.ComboBox cboDieuKienChon;
        private System.Windows.Forms.DateTimePicker dateTimeTuNgay;
        private System.Windows.Forms.DateTimePicker dateTimeDenNgay;
        private System.Windows.Forms.TextBox txtChiTiet_ThoiGian;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private System.Windows.Forms.TextBox txtChiTiet_ThuNgan;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private System.Windows.Forms.TextBox txtChiTiet_TienTraLai;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private System.Windows.Forms.TextBox txtChiTiet_TienChietKhau;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private System.Windows.Forms.TextBox txtChiTiet_TongTien;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.TextBox txtChiTiet_TongSoLuong;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.TextBox txtTraLai_ThuNgan;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private System.Windows.Forms.TextBox txtTraLai_TongTien;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private System.Windows.Forms.TextBox txtTraLai_TongSoLuong;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private System.Windows.Forms.TextBox txtTraLai_SoLuong;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private System.Windows.Forms.TextBox txtTraLai_MaVatTu;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private TextBox txtTraLai_MaGiaoDich;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.SimpleButton btnF6TimKiem;
        private DevExpress.XtraEditors.SimpleButton btnF5LamMoiGiaoDich;
        private DevExpress.XtraEditors.SimpleButton btnF1ThanhToanTraLai;
        private DevExpress.XtraEditors.SimpleButton btnF4TangHang;
        private DevExpress.XtraEditors.SimpleButton btnF3GiamHang;
        private DevExpress.XtraEditors.SimpleButton btnF2ThemMoiTraLai;
        private DataGridViewTextBoxColumn KEY;
        private DataGridViewTextBoxColumn MAHANG;
        private DataGridViewTextBoxColumn SOLUONG;
        private DataGridViewTextBoxColumn GIABANLE_VAT;
        private DataGridViewTextBoxColumn THANHTIEN;
        private DataGridViewTextBoxColumn TENHANG;
        private DataGridViewTextBoxColumn DONVITINH;
        private DataGridViewTextBoxColumn GIAVON;
        private DataGridViewTextBoxColumn TIEN_KHUYENMAI;
        private DataGridViewTextBoxColumn KEY_TRALAI;
        private DataGridViewTextBoxColumn MAHANG_TRALAI;
        private DataGridViewTextBoxColumn TENHANG_TRALAI;
        private DataGridViewTextBoxColumn SOLUONG_TRALAI;
        private DataGridViewTextBoxColumn GIABANLE_VAT_TRALAI;
        private DataGridViewTextBoxColumn TIEN_KHUYENMAI_TRALAI;
        private DataGridViewTextBoxColumn THANHTIEN_TRALAI;
        private DataGridViewTextBoxColumn DONVITINH_TRALAI;
        private DataGridViewTextBoxColumn GIAVON_TRALAI;
    }
}
