namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    partial class frmGuiDuLieuBan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGuiDuLieuBan));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DongBoDuLieuBan = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.GuiDuLieuBan = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimeNgayBan = new System.Windows.Forms.DateTimePicker();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.DongBoDuLieuBan);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(4, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(391, 100);
            this.groupBox2.TabIndex = 524;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Đồng bộ  tất cả dữ liệu bán";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 16);
            this.label2.TabIndex = 522;
            this.label2.Text = "Đồng bộ tất cả dữ liệu bán ";
            // 
            // DongBoDuLieuBan
            // 
            this.DongBoDuLieuBan.Location = new System.Drawing.Point(250, 44);
            this.DongBoDuLieuBan.Name = "DongBoDuLieuBan";
            this.DongBoDuLieuBan.Size = new System.Drawing.Size(92, 23);
            this.DongBoDuLieuBan.TabIndex = 521;
            this.DongBoDuLieuBan.Text = "Đồng bộ";
            this.DongBoDuLieuBan.UseVisualStyleBackColor = true;
            this.DongBoDuLieuBan.Click += new System.EventHandler(this.DongBoDuLieuBan_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.GuiDuLieuBan);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.dateTimeNgayBan);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(4, 126);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(391, 85);
            this.groupBox3.TabIndex = 525;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Đồng bộ dữ liệu bán theo ngày";
            // 
            // GuiDuLieuBan
            // 
            this.GuiDuLieuBan.Location = new System.Drawing.Point(250, 34);
            this.GuiDuLieuBan.Name = "GuiDuLieuBan";
            this.GuiDuLieuBan.Size = new System.Drawing.Size(92, 23);
            this.GuiDuLieuBan.TabIndex = 521;
            this.GuiDuLieuBan.Text = "Gửi dữ liệu";
            this.GuiDuLieuBan.UseVisualStyleBackColor = true;
            this.GuiDuLieuBan.Click += new System.EventHandler(this.GuiDuLieuBan_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 16);
            this.label3.TabIndex = 520;
            this.label3.Text = "Ngày :";
            // 
            // dateTimeNgayBan
            // 
            this.dateTimeNgayBan.CustomFormat = "dd/MM/yyyy";
            this.dateTimeNgayBan.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimeNgayBan.Location = new System.Drawing.Point(109, 34);
            this.dateTimeNgayBan.Name = "dateTimeNgayBan";
            this.dateTimeNgayBan.Size = new System.Drawing.Size(119, 22);
            this.dateTimeNgayBan.TabIndex = 519;
            this.dateTimeNgayBan.TabStop = false;
            // 
            // frmGuiDuLieuBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 218);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGuiDuLieuBan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đồng bộ dữ liệu từ máy thu ngân";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DongBoDuLieuBan;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button GuiDuLieuBan;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimeNgayBan;
    }
}