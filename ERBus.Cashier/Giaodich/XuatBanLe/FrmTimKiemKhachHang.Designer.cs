namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    partial class FrmTimKiemKhachHang
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTimKiemKhachHang));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTimKiemKhachHang = new System.Windows.Forms.Button();
            this.cboDieuKienTimKiem = new System.Windows.Forms.ComboBox();
            this.lblDieuKien = new System.Windows.Forms.Label();
            this.txtDieuKienTimKiem = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvThongTinKhachHang = new System.Windows.Forms.DataGridView();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAKHACHHANG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TENKHACHHANG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MATHE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DIENTHOAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NGAYSINH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DIACHI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NGAYDACBIET = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CANCUOC_CONGDAN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SODIEM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TONGTIEN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongTinKhachHang)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTimKiemKhachHang);
            this.groupBox1.Controls.Add(this.cboDieuKienTimKiem);
            this.groupBox1.Controls.Add(this.lblDieuKien);
            this.groupBox1.Controls.Add(this.txtDieuKienTimKiem);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1129, 66);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tìm kiếm";
            // 
            // btnTimKiemKhachHang
            // 
            this.btnTimKiemKhachHang.ForeColor = System.Drawing.Color.CadetBlue;
            this.btnTimKiemKhachHang.Location = new System.Drawing.Point(799, 24);
            this.btnTimKiemKhachHang.Name = "btnTimKiemKhachHang";
            this.btnTimKiemKhachHang.Size = new System.Drawing.Size(93, 29);
            this.btnTimKiemKhachHang.TabIndex = 3;
            this.btnTimKiemKhachHang.Text = "Tìm kiếm";
            this.btnTimKiemKhachHang.UseVisualStyleBackColor = true;
            this.btnTimKiemKhachHang.Click += new System.EventHandler(this.btnTimKiemKhachHang_Click);
            // 
            // cboDieuKienTimKiem
            // 
            this.cboDieuKienTimKiem.FormattingEnabled = true;
            this.cboDieuKienTimKiem.Location = new System.Drawing.Point(600, 26);
            this.cboDieuKienTimKiem.Name = "cboDieuKienTimKiem";
            this.cboDieuKienTimKiem.Size = new System.Drawing.Size(182, 27);
            this.cboDieuKienTimKiem.TabIndex = 2;
            // 
            // lblDieuKien
            // 
            this.lblDieuKien.AutoSize = true;
            this.lblDieuKien.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDieuKien.ForeColor = System.Drawing.Color.CadetBlue;
            this.lblDieuKien.Location = new System.Drawing.Point(127, 28);
            this.lblDieuKien.Name = "lblDieuKien";
            this.lblDieuKien.Size = new System.Drawing.Size(83, 23);
            this.lblDieuKien.TabIndex = 1;
            this.lblDieuKien.Text = "Từ khóa";
            // 
            // txtDieuKienTimKiem
            // 
            this.txtDieuKienTimKiem.Location = new System.Drawing.Point(223, 26);
            this.txtDieuKienTimKiem.Name = "txtDieuKienTimKiem";
            this.txtDieuKienTimKiem.Size = new System.Drawing.Size(371, 27);
            this.txtDieuKienTimKiem.TabIndex = 0;
            this.txtDieuKienTimKiem.TextChanged += new System.EventHandler(this.txtDieuKienTimKiem_TextChanged);
            this.txtDieuKienTimKiem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDieuKienTimKiem_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvThongTinKhachHang);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1129, 407);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Chi tiết";
            // 
            // dgvThongTinKhachHang
            // 
            this.dgvThongTinKhachHang.AllowUserToAddRows = false;
            this.dgvThongTinKhachHang.AllowUserToDeleteRows = false;
            this.dgvThongTinKhachHang.AllowUserToResizeColumns = false;
            this.dgvThongTinKhachHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThongTinKhachHang.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.MAKHACHHANG,
            this.TENKHACHHANG,
            this.MATHE,
            this.DIENTHOAI,
            this.NGAYSINH,
            this.DIACHI,
            this.NGAYDACBIET,
            this.CANCUOC_CONGDAN,
            this.SODIEM,
            this.TONGTIEN});
            this.dgvThongTinKhachHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThongTinKhachHang.Location = new System.Drawing.Point(3, 22);
            this.dgvThongTinKhachHang.MaximumSize = new System.Drawing.Size(1123, 382);
            this.dgvThongTinKhachHang.MinimumSize = new System.Drawing.Size(1123, 382);
            this.dgvThongTinKhachHang.Name = "dgvThongTinKhachHang";
            this.dgvThongTinKhachHang.ReadOnly = true;
            this.dgvThongTinKhachHang.RowHeadersVisible = false;
            this.dgvThongTinKhachHang.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThongTinKhachHang.Size = new System.Drawing.Size(1123, 382);
            this.dgvThongTinKhachHang.TabIndex = 0;
            this.dgvThongTinKhachHang.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvThongTinKhachHang_CellMouseDoubleClick);
            // 
            // STT
            // 
            this.STT.DataPropertyName = "STT";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STT.DefaultCellStyle = dataGridViewCellStyle1;
            this.STT.HeaderText = "STT";
            this.STT.MinimumWidth = 50;
            this.STT.Name = "STT";
            this.STT.ReadOnly = true;
            this.STT.Width = 50;
            // 
            // MAKHACHHANG
            // 
            this.MAKHACHHANG.DataPropertyName = "MAKHACHHANG";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MAKHACHHANG.DefaultCellStyle = dataGridViewCellStyle2;
            this.MAKHACHHANG.HeaderText = "Mã VIP";
            this.MAKHACHHANG.MinimumWidth = 130;
            this.MAKHACHHANG.Name = "MAKHACHHANG";
            this.MAKHACHHANG.ReadOnly = true;
            this.MAKHACHHANG.Width = 130;
            // 
            // TENKHACHHANG
            // 
            this.TENKHACHHANG.DataPropertyName = "TENKHACHHANG";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TENKHACHHANG.DefaultCellStyle = dataGridViewCellStyle3;
            this.TENKHACHHANG.HeaderText = "Tên khách hàng";
            this.TENKHACHHANG.MinimumWidth = 250;
            this.TENKHACHHANG.Name = "TENKHACHHANG";
            this.TENKHACHHANG.ReadOnly = true;
            this.TENKHACHHANG.Width = 250;
            // 
            // MATHE
            // 
            this.MATHE.DataPropertyName = "MATHE";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MATHE.DefaultCellStyle = dataGridViewCellStyle4;
            this.MATHE.HeaderText = "Mã thẻ";
            this.MATHE.Name = "MATHE";
            this.MATHE.ReadOnly = true;
            // 
            // DIENTHOAI
            // 
            this.DIENTHOAI.DataPropertyName = "DIENTHOAI";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DIENTHOAI.DefaultCellStyle = dataGridViewCellStyle5;
            this.DIENTHOAI.HeaderText = "Điện thoại";
            this.DIENTHOAI.MinimumWidth = 110;
            this.DIENTHOAI.Name = "DIENTHOAI";
            this.DIENTHOAI.ReadOnly = true;
            this.DIENTHOAI.Width = 110;
            // 
            // NGAYSINH
            // 
            this.NGAYSINH.DataPropertyName = "NGAYSINH";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.NGAYSINH.DefaultCellStyle = dataGridViewCellStyle6;
            this.NGAYSINH.HeaderText = "Ngày sinh";
            this.NGAYSINH.MinimumWidth = 100;
            this.NGAYSINH.Name = "NGAYSINH";
            this.NGAYSINH.ReadOnly = true;
            // 
            // DIACHI
            // 
            this.DIACHI.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DIACHI.DataPropertyName = "DIACHI";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.DIACHI.DefaultCellStyle = dataGridViewCellStyle7;
            this.DIACHI.HeaderText = "Địa chỉ";
            this.DIACHI.Name = "DIACHI";
            this.DIACHI.ReadOnly = true;
            // 
            // NGAYDACBIET
            // 
            this.NGAYDACBIET.DataPropertyName = "NGAYDACBIET";
            this.NGAYDACBIET.HeaderText = "Ngày đặc biệt";
            this.NGAYDACBIET.MinimumWidth = 100;
            this.NGAYDACBIET.Name = "NGAYDACBIET";
            this.NGAYDACBIET.ReadOnly = true;
            // 
            // CANCUOC_CONGDAN
            // 
            this.CANCUOC_CONGDAN.DataPropertyName = "CANCUOC_CONGDAN";
            this.CANCUOC_CONGDAN.HeaderText = "Căn cước";
            this.CANCUOC_CONGDAN.Name = "CANCUOC_CONGDAN";
            this.CANCUOC_CONGDAN.ReadOnly = true;
            // 
            // SODIEM
            // 
            this.SODIEM.DataPropertyName = "SODIEM";
            this.SODIEM.HeaderText = "Số điểm";
            this.SODIEM.Name = "SODIEM";
            this.SODIEM.ReadOnly = true;
            // 
            // TONGTIEN
            // 
            this.TONGTIEN.DataPropertyName = "TONGTIEN";
            this.TONGTIEN.HeaderText = "Tổng tiền";
            this.TONGTIEN.Name = "TONGTIEN";
            this.TONGTIEN.ReadOnly = true;
            // 
            // FrmTimKiemKhachHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 473);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmTimKiemKhachHang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin khách hàng";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongTinKhachHang)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblDieuKien;
        private System.Windows.Forms.TextBox txtDieuKienTimKiem;
        private System.Windows.Forms.ComboBox cboDieuKienTimKiem;
        private System.Windows.Forms.DataGridView dgvThongTinKhachHang;
        private System.Windows.Forms.Button btnTimKiemKhachHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAKHACHHANG;
        private System.Windows.Forms.DataGridViewTextBoxColumn TENKHACHHANG;
        private System.Windows.Forms.DataGridViewTextBoxColumn MATHE;
        private System.Windows.Forms.DataGridViewTextBoxColumn DIENTHOAI;
        private System.Windows.Forms.DataGridViewTextBoxColumn NGAYSINH;
        private System.Windows.Forms.DataGridViewTextBoxColumn DIACHI;
        private System.Windows.Forms.DataGridViewTextBoxColumn NGAYDACBIET;
        private System.Windows.Forms.DataGridViewTextBoxColumn CANCUOC_CONGDAN;
        private System.Windows.Forms.DataGridViewTextBoxColumn SODIEM;
        private System.Windows.Forms.DataGridViewTextBoxColumn TONGTIEN;
    }
}