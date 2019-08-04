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
            this.groupBox2.SuspendLayout();
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
            this.groupBox2.Text = "Đồng bộ  dữ liệu bán";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 16);
            this.label2.TabIndex = 522;
            this.label2.Text = "Đồng bộ  dữ liệu bán ";
            // 
            // DongBoDuLieuBan
            // 
            this.DongBoDuLieuBan.Location = new System.Drawing.Point(176, 41);
            this.DongBoDuLieuBan.Name = "DongBoDuLieuBan";
            this.DongBoDuLieuBan.Size = new System.Drawing.Size(92, 23);
            this.DongBoDuLieuBan.TabIndex = 521;
            this.DongBoDuLieuBan.Text = "Đồng bộ";
            this.DongBoDuLieuBan.UseVisualStyleBackColor = true;
            this.DongBoDuLieuBan.Click += new System.EventHandler(this.DongBoDuLieuBan_Click);
            // 
            // frmGuiDuLieuBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 111);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(335, 150);
            this.MinimumSize = new System.Drawing.Size(335, 150);
            this.Name = "frmGuiDuLieuBan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đồng bộ dữ liệu từ máy bán";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DongBoDuLieuBan;
    }
}