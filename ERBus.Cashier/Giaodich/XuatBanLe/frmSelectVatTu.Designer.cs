namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    partial class frmSelectVatTu
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
            this.dgvSelect = new System.Windows.Forms.DataGridView();
            this.clmSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmMaVatTu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTenVatTu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSelect
            // 
            this.dgvSelect.AllowUserToAddRows = false;
            this.dgvSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelect.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmSTT,
            this.clmMaVatTu,
            this.clmTenVatTu});
            this.dgvSelect.Location = new System.Drawing.Point(-1, 0);
            this.dgvSelect.Name = "dgvSelect";
            this.dgvSelect.RowHeadersVisible = false;
            this.dgvSelect.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelect.Size = new System.Drawing.Size(500, 162);
            this.dgvSelect.TabIndex = 0;
            this.dgvSelect.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelect_CellContentClick);
            // 
            // clmSTT
            // 
            this.clmSTT.DataPropertyName = "clmSTT";
            this.clmSTT.HeaderText = "STT";
            this.clmSTT.Name = "clmSTT";
            this.clmSTT.ReadOnly = true;
            this.clmSTT.Width = 50;
            // 
            // clmMaVatTu
            // 
            this.clmMaVatTu.DataPropertyName = "clmMaVatTu";
            this.clmMaVatTu.HeaderText = "Mã hàng";
            this.clmMaVatTu.Name = "clmMaVatTu";
            this.clmMaVatTu.ReadOnly = true;
            // 
            // clmTenVatTu
            // 
            this.clmTenVatTu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmTenVatTu.DataPropertyName = "clmTenVatTu";
            this.clmTenVatTu.FillWeight = 5.686043F;
            this.clmTenVatTu.HeaderText = "Tên hàng";
            this.clmTenVatTu.Name = "clmTenVatTu";
            this.clmTenVatTu.ReadOnly = true;
            // 
            // frmSelectVatTu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 161);
            this.Controls.Add(this.dgvSelect);
            this.Name = "frmSelectVatTu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Danh sách mặt hàng trùng mã";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSTT;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmMaVatTu;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTenVatTu;
    }
}