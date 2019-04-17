namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    partial class frmTimKiemVatTu
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTimKiemVatTu));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.cboDieuKienTimKiem = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilterSearch = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvResultSearch = new System.Windows.Forms.DataGridView();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAHANG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MACON = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TENHANG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DONVITINH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MALOAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GIABANLE_VAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEMCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BARCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MANHOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MANHACUNGCAP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TENNHACUNGCAP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnTimKiem);
            this.panel1.Controls.Add(this.cboDieuKienTimKiem);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtFilterSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1012, 51);
            this.panel1.TabIndex = 0;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimKiem.ForeColor = System.Drawing.Color.Blue;
            this.btnTimKiem.Location = new System.Drawing.Point(654, 12);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(79, 29);
            this.btnTimKiem.TabIndex = 4;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // cboDieuKienTimKiem
            // 
            this.cboDieuKienTimKiem.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDieuKienTimKiem.FormattingEnabled = true;
            this.cboDieuKienTimKiem.Location = new System.Drawing.Point(482, 13);
            this.cboDieuKienTimKiem.Name = "cboDieuKienTimKiem";
            this.cboDieuKienTimKiem.Size = new System.Drawing.Size(166, 26);
            this.cboDieuKienTimKiem.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(57, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tìm kiếm";
            // 
            // txtFilterSearch
            // 
            this.txtFilterSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilterSearch.Location = new System.Drawing.Point(134, 13);
            this.txtFilterSearch.Name = "txtFilterSearch";
            this.txtFilterSearch.Size = new System.Drawing.Size(332, 26);
            this.txtFilterSearch.TabIndex = 0;
            this.txtFilterSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFilterSearch.TextChanged += new System.EventHandler(this.txtFilterSearch_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.dgvResultSearch);
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1012, 472);
            this.panel2.TabIndex = 1;
            // 
            // dgvResultSearch
            // 
            this.dgvResultSearch.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvResultSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvResultSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResultSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.MAHANG,
            this.MACON,
            this.TENHANG,
            this.DONVITINH,
            this.MALOAI,
            this.GIABANLE_VAT,
            this.ITEMCODE,
            this.BARCODE,
            this.MANHOM,
            this.MANHACUNGCAP,
            this.TENNHACUNGCAP});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvResultSearch.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvResultSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResultSearch.Location = new System.Drawing.Point(0, 0);
            this.dgvResultSearch.Name = "dgvResultSearch";
            this.dgvResultSearch.RowHeadersVisible = false;
            this.dgvResultSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResultSearch.Size = new System.Drawing.Size(1012, 472);
            this.dgvResultSearch.TabIndex = 0;
            this.dgvResultSearch.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvResultSearch_CellMouseDoubleClick);
            // 
            // STT
            // 
            this.STT.DataPropertyName = "STT";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.STT.DefaultCellStyle = dataGridViewCellStyle2;
            this.STT.HeaderText = "STT";
            this.STT.MinimumWidth = 50;
            this.STT.Name = "STT";
            this.STT.Width = 50;
            // 
            // MAHANG
            // 
            this.MAHANG.DataPropertyName = "MAHANG";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MAHANG.DefaultCellStyle = dataGridViewCellStyle3;
            this.MAHANG.HeaderText = "Mã hàng";
            this.MAHANG.MinimumWidth = 100;
            this.MAHANG.Name = "MAHANG";
            // 
            // MACON
            // 
            this.MACON.DataPropertyName = "MACON";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MACON.DefaultCellStyle = dataGridViewCellStyle4;
            this.MACON.HeaderText = "Mã con";
            this.MACON.MinimumWidth = 100;
            this.MACON.Name = "MACON";
            // 
            // TENHANG
            // 
            this.TENHANG.DataPropertyName = "TENHANG";
            this.TENHANG.HeaderText = "Tên hàng hóa";
            this.TENHANG.MinimumWidth = 300;
            this.TENHANG.Name = "TENHANG";
            this.TENHANG.Width = 300;
            // 
            // DONVITINH
            // 
            this.DONVITINH.DataPropertyName = "DONVITINH";
            this.DONVITINH.HeaderText = "Đ.vị tính";
            this.DONVITINH.MinimumWidth = 100;
            this.DONVITINH.Name = "DONVITINH";
            // 
            // MALOAI
            // 
            this.MALOAI.DataPropertyName = "MALOAI";
            this.MALOAI.HeaderText = "Mã loại";
            this.MALOAI.MinimumWidth = 80;
            this.MALOAI.Name = "MALOAI";
            this.MALOAI.Width = 80;
            // 
            // GIABANLE_VAT
            // 
            this.GIABANLE_VAT.DataPropertyName = "GIABANLE_VAT";
            this.GIABANLE_VAT.HeaderText = "Giá bán";
            this.GIABANLE_VAT.Name = "GIABANLE_VAT";
            // 
            // ITEMCODE
            // 
            this.ITEMCODE.DataPropertyName = "ITEMCODE";
            this.ITEMCODE.HeaderText = "ITEMCODE";
            this.ITEMCODE.Name = "ITEMCODE";
            // 
            // BARCODE
            // 
            this.BARCODE.DataPropertyName = "BARCODE";
            this.BARCODE.HeaderText = "BARCODE";
            this.BARCODE.MinimumWidth = 200;
            this.BARCODE.Name = "BARCODE";
            this.BARCODE.Width = 200;
            // 
            // MANHOM
            // 
            this.MANHOM.DataPropertyName = "MANHOM";
            this.MANHOM.HeaderText = "Mã nhóm";
            this.MANHOM.MinimumWidth = 100;
            this.MANHOM.Name = "MANHOM";
            // 
            // MANHACUNGCAP
            // 
            this.MANHACUNGCAP.DataPropertyName = "MANHACUNGCAP";
            this.MANHACUNGCAP.HeaderText = "Mã NCC";
            this.MANHACUNGCAP.Name = "MANHACUNGCAP";
            // 
            // TENNHACUNGCAP
            // 
            this.TENNHACUNGCAP.DataPropertyName = "TENNHACUNGCAP";
            this.TENNHACUNGCAP.HeaderText = "Tên NCC";
            this.TENNHACUNGCAP.Name = "TENNHACUNGCAP";
            // 
            // frmTimKiemVatTu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 540);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1252, 579);
            this.MinimumSize = new System.Drawing.Size(1022, 579);
            this.Name = "frmTimKiemVatTu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TÌM KIẾM VẬT TƯ, BÓ HÀNG";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtFilterSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvResultSearch;
        private System.Windows.Forms.ComboBox cboDieuKienTimKiem;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAHANG;
        private System.Windows.Forms.DataGridViewTextBoxColumn MACON;
        private System.Windows.Forms.DataGridViewTextBoxColumn TENHANG;
        private System.Windows.Forms.DataGridViewTextBoxColumn DONVITINH;
        private System.Windows.Forms.DataGridViewTextBoxColumn MALOAI;
        private System.Windows.Forms.DataGridViewTextBoxColumn GIABANLE_VAT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEMCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn BARCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn MANHOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn MANHACUNGCAP;
        private System.Windows.Forms.DataGridViewTextBoxColumn TENNHACUNGCAP;
    }
}