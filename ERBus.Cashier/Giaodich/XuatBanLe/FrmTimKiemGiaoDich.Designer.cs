namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    partial class FrmTimKiemGiaoDich
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTimKiemGiaoDich));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTimKiemGiaoDich = new System.Windows.Forms.Button();
            this.dateTimeDenNgay = new System.Windows.Forms.DateTimePicker();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dateTimeTuNgay = new System.Windows.Forms.DateTimePicker();
            this.lblNgayPhatSinh = new DevExpress.XtraEditors.LabelControl();
            this.cboDieuKienTimKiem = new System.Windows.Forms.ComboBox();
            this.lblDieuKien = new System.Windows.Forms.Label();
            this.txtDieuKienTimKiem = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvDanhSachGiaoDichChiTiet = new System.Windows.Forms.DataGridView();
            this.MA_GIAODICH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOAI_GIAODICH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NGAY_GIAODICH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.I_CREATE_BY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TIENKHACH_TRA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TIEN_TRALAI_KHACH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.THANHTIEN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.THOIGIAN_TAO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachGiaoDichChiTiet)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTimKiemGiaoDich);
            this.groupBox1.Controls.Add(this.dateTimeDenNgay);
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Controls.Add(this.dateTimeTuNgay);
            this.groupBox1.Controls.Add(this.lblNgayPhatSinh);
            this.groupBox1.Controls.Add(this.cboDieuKienTimKiem);
            this.groupBox1.Controls.Add(this.lblDieuKien);
            this.groupBox1.Controls.Add(this.txtDieuKienTimKiem);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1184, 66);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Điều kiện tìm kiếm";
            // 
            // btnTimKiemGiaoDich
            // 
            this.btnTimKiemGiaoDich.Location = new System.Drawing.Point(1039, 25);
            this.btnTimKiemGiaoDich.Name = "btnTimKiemGiaoDich";
            this.btnTimKiemGiaoDich.Size = new System.Drawing.Size(104, 27);
            this.btnTimKiemGiaoDich.TabIndex = 46;
            this.btnTimKiemGiaoDich.Text = "Tìm kiếm";
            this.btnTimKiemGiaoDich.UseVisualStyleBackColor = true;
            this.btnTimKiemGiaoDich.Click += new System.EventHandler(this.btnTimKiemGiaoDich_Click);
            // 
            // dateTimeDenNgay
            // 
            this.dateTimeDenNgay.CalendarFont = new System.Drawing.Font("Tunga", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeDenNgay.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeDenNgay.Location = new System.Drawing.Point(857, 25);
            this.dateTimeDenNgay.Name = "dateTimeDenNgay";
            this.dateTimeDenNgay.Size = new System.Drawing.Size(123, 27);
            this.dateTimeDenNgay.TabIndex = 45;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(752, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(77, 21);
            this.labelControl1.TabIndex = 44;
            this.labelControl1.Text = "Đến ngày";
            // 
            // dateTimeTuNgay
            // 
            this.dateTimeTuNgay.CalendarFont = new System.Drawing.Font("Tunga", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeTuNgay.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeTuNgay.Location = new System.Drawing.Point(604, 26);
            this.dateTimeTuNgay.Name = "dateTimeTuNgay";
            this.dateTimeTuNgay.Size = new System.Drawing.Size(136, 27);
            this.dateTimeTuNgay.TabIndex = 43;
            // 
            // lblNgayPhatSinh
            // 
            this.lblNgayPhatSinh.Appearance.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNgayPhatSinh.Appearance.Options.UseFont = true;
            this.lblNgayPhatSinh.Location = new System.Drawing.Point(512, 30);
            this.lblNgayPhatSinh.Name = "lblNgayPhatSinh";
            this.lblNgayPhatSinh.Size = new System.Drawing.Size(68, 21);
            this.lblNgayPhatSinh.TabIndex = 42;
            this.lblNgayPhatSinh.Text = "Từ ngày";
            // 
            // cboDieuKienTimKiem
            // 
            this.cboDieuKienTimKiem.FormattingEnabled = true;
            this.cboDieuKienTimKiem.Location = new System.Drawing.Point(372, 28);
            this.cboDieuKienTimKiem.Name = "cboDieuKienTimKiem";
            this.cboDieuKienTimKiem.Size = new System.Drawing.Size(129, 27);
            this.cboDieuKienTimKiem.TabIndex = 2;
            this.cboDieuKienTimKiem.SelectedValueChanged += new System.EventHandler(this.cboDieuKienTimKiem_SelectedValueChanged);
            // 
            // lblDieuKien
            // 
            this.lblDieuKien.AutoSize = true;
            this.lblDieuKien.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDieuKien.Location = new System.Drawing.Point(4, 30);
            this.lblDieuKien.Name = "lblDieuKien";
            this.lblDieuKien.Size = new System.Drawing.Size(83, 23);
            this.lblDieuKien.TabIndex = 1;
            this.lblDieuKien.Text = "Từ khóa";
            // 
            // txtDieuKienTimKiem
            // 
            this.txtDieuKienTimKiem.Location = new System.Drawing.Point(91, 28);
            this.txtDieuKienTimKiem.Name = "txtDieuKienTimKiem";
            this.txtDieuKienTimKiem.Size = new System.Drawing.Size(275, 27);
            this.txtDieuKienTimKiem.TabIndex = 0;
            this.txtDieuKienTimKiem.TextChanged += new System.EventHandler(this.txtDieuKienTimKiem_TextChanged);
            this.txtDieuKienTimKiem.Validating += new System.ComponentModel.CancelEventHandler(this.txtDieuKienTimKiem_Validating);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvDanhSachGiaoDichChiTiet);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1184, 298);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Danh sách giao dịch chi tiết";
            // 
            // dgvDanhSachGiaoDichChiTiet
            // 
            this.dgvDanhSachGiaoDichChiTiet.AllowUserToAddRows = false;
            this.dgvDanhSachGiaoDichChiTiet.AllowUserToDeleteRows = false;
            this.dgvDanhSachGiaoDichChiTiet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDanhSachGiaoDichChiTiet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MA_GIAODICH,
            this.LOAI_GIAODICH,
            this.NGAY_GIAODICH,
            this.I_CREATE_BY,
            this.TIENKHACH_TRA,
            this.TIEN_TRALAI_KHACH,
            this.THANHTIEN,
            this.THOIGIAN_TAO});
            this.dgvDanhSachGiaoDichChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDanhSachGiaoDichChiTiet.Location = new System.Drawing.Point(3, 22);
            this.dgvDanhSachGiaoDichChiTiet.Name = "dgvDanhSachGiaoDichChiTiet";
            this.dgvDanhSachGiaoDichChiTiet.ReadOnly = true;
            this.dgvDanhSachGiaoDichChiTiet.RowHeadersVisible = false;
            this.dgvDanhSachGiaoDichChiTiet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDanhSachGiaoDichChiTiet.Size = new System.Drawing.Size(1178, 273);
            this.dgvDanhSachGiaoDichChiTiet.TabIndex = 0;
            this.dgvDanhSachGiaoDichChiTiet.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDanhSachGiaoDichChiTiet_CellMouseDoubleClick);
            // 
            // MA_GIAODICH
            // 
            this.MA_GIAODICH.DataPropertyName = "MA_GIAODICH";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MA_GIAODICH.DefaultCellStyle = dataGridViewCellStyle1;
            this.MA_GIAODICH.HeaderText = "Mã GD";
            this.MA_GIAODICH.Name = "MA_GIAODICH";
            this.MA_GIAODICH.ReadOnly = true;
            // 
            // LOAI_GIAODICH
            // 
            this.LOAI_GIAODICH.DataPropertyName = "LOAI_GIAODICH";
            this.LOAI_GIAODICH.HeaderText = "Loại GD";
            this.LOAI_GIAODICH.MinimumWidth = 60;
            this.LOAI_GIAODICH.Name = "LOAI_GIAODICH";
            this.LOAI_GIAODICH.ReadOnly = true;
            this.LOAI_GIAODICH.Width = 60;
            // 
            // NGAY_GIAODICH
            // 
            this.NGAY_GIAODICH.DataPropertyName = "NGAY_GIAODICH";
            this.NGAY_GIAODICH.HeaderText = "Ngày phát sinh";
            this.NGAY_GIAODICH.Name = "NGAY_GIAODICH";
            this.NGAY_GIAODICH.ReadOnly = true;
            // 
            // I_CREATE_BY
            // 
            this.I_CREATE_BY.DataPropertyName = "I_CREATE_BY";
            this.I_CREATE_BY.HeaderText = "Mã người tạo";
            this.I_CREATE_BY.Name = "I_CREATE_BY";
            this.I_CREATE_BY.ReadOnly = true;
            // 
            // TIENKHACH_TRA
            // 
            this.TIENKHACH_TRA.DataPropertyName = "TIENKHACH_TRA";
            this.TIENKHACH_TRA.HeaderText = "Tiền khách đưa";
            this.TIENKHACH_TRA.Name = "TIENKHACH_TRA";
            this.TIENKHACH_TRA.ReadOnly = true;
            // 
            // TIEN_TRALAI_KHACH
            // 
            this.TIEN_TRALAI_KHACH.DataPropertyName = "TIEN_TRALAI_KHACH";
            this.TIEN_TRALAI_KHACH.HeaderText = "Tiền trả lại";
            this.TIEN_TRALAI_KHACH.Name = "TIEN_TRALAI_KHACH";
            this.TIEN_TRALAI_KHACH.ReadOnly = true;
            // 
            // THANHTIEN
            // 
            this.THANHTIEN.DataPropertyName = "THANHTIEN";
            this.THANHTIEN.HeaderText = "Tổng tiền";
            this.THANHTIEN.Name = "THANHTIEN";
            this.THANHTIEN.ReadOnly = true;
            // 
            // THOIGIAN_TAO
            // 
            this.THOIGIAN_TAO.DataPropertyName = "THOIGIAN_TAO";
            this.THOIGIAN_TAO.HeaderText = "Thời gian";
            this.THOIGIAN_TAO.Name = "THOIGIAN_TAO";
            this.THOIGIAN_TAO.ReadOnly = true;
            // 
            // FrmTimKiemGiaoDich
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 361);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1200, 400);
            this.MinimumSize = new System.Drawing.Size(1200, 400);
            this.Name = "FrmTimKiemGiaoDich";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Danh sách giao dịch";
            this.Load += new System.EventHandler(this.FrmTimKiemGiaoDich_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachGiaoDichChiTiet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblDieuKien;
        private System.Windows.Forms.TextBox txtDieuKienTimKiem;
        private System.Windows.Forms.ComboBox cboDieuKienTimKiem;
        private System.Windows.Forms.DataGridView dgvDanhSachGiaoDichChiTiet;
        private System.Windows.Forms.DateTimePicker dateTimeTuNgay;
        private DevExpress.XtraEditors.LabelControl lblNgayPhatSinh;
        private System.Windows.Forms.DateTimePicker dateTimeDenNgay;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Button btnTimKiemGiaoDich;
        private System.Windows.Forms.DataGridViewTextBoxColumn MA_GIAODICH;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOAI_GIAODICH;
        private System.Windows.Forms.DataGridViewTextBoxColumn NGAY_GIAODICH;
        private System.Windows.Forms.DataGridViewTextBoxColumn I_CREATE_BY;
        private System.Windows.Forms.DataGridViewTextBoxColumn TIENKHACH_TRA;
        private System.Windows.Forms.DataGridViewTextBoxColumn TIEN_TRALAI_KHACH;
        private System.Windows.Forms.DataGridViewTextBoxColumn THANHTIEN;
        private System.Windows.Forms.DataGridViewTextBoxColumn THOIGIAN_TAO;
    }
}