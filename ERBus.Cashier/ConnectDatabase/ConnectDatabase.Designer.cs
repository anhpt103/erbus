namespace ERBus.Cashier.ConnectDatabase
{
    partial class FrmConnectDatabase
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtServiceName = new System.Windows.Forms.TextBox();
            this.lblDatabaseName = new System.Windows.Forms.Label();
            this.txtHostName = new System.Windows.Forms.TextBox();
            this.lblServerName = new System.Windows.Forms.Label();
            this.btnCheckConnect = new System.Windows.Forms.Button();
            this.btnCreateConnect = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtHostnameSql = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPasswordSql = new System.Windows.Forms.TextBox();
            this.txtDatabaseNameSql = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCreateConnectSql = new System.Windows.Forms.Button();
            this.txtUsernameSql = new System.Windows.Forms.TextBox();
            this.btnCheckConnectSql = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtServiceName);
            this.groupBox1.Controls.Add(this.lblDatabaseName);
            this.groupBox1.Controls.Add(this.txtHostName);
            this.groupBox1.Controls.Add(this.lblServerName);
            this.groupBox1.Controls.Add(this.btnCheckConnect);
            this.groupBox1.Controls.Add(this.btnCreateConnect);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 201);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cấu hình kết nối máy chủ ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(314, 133);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 18);
            this.label9.TabIndex = 15;
            this.label9.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(314, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 18);
            this.label8.TabIndex = 14;
            this.label8.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(314, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 18);
            this.label7.TabIndex = 13;
            this.label7.Text = "*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(314, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 18);
            this.label5.TabIndex = 12;
            this.label5.Tag = "Bắt buộc nhập !";
            this.label5.Text = "*";
            // 
            // txtPassword
            // 
            this.txtPassword.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPassword.Location = new System.Drawing.Point(134, 132);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(174, 21);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.Text = "R0q5c3rewQ";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtUsername.Location = new System.Drawing.Point(134, 96);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(174, 21);
            this.txtUsername.TabIndex = 3;
            this.txtUsername.Text = "ERBBUS";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "User Name";
            // 
            // txtServiceName
            // 
            this.txtServiceName.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtServiceName.Location = new System.Drawing.Point(134, 58);
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.Size = new System.Drawing.Size(174, 21);
            this.txtServiceName.TabIndex = 2;
            this.txtServiceName.Text = "ORCL";
            // 
            // lblDatabaseName
            // 
            this.lblDatabaseName.AutoSize = true;
            this.lblDatabaseName.Location = new System.Drawing.Point(31, 61);
            this.lblDatabaseName.Name = "lblDatabaseName";
            this.lblDatabaseName.Size = new System.Drawing.Size(84, 15);
            this.lblDatabaseName.TabIndex = 6;
            this.lblDatabaseName.Text = "Service Name";
            // 
            // txtHostName
            // 
            this.txtHostName.Location = new System.Drawing.Point(134, 23);
            this.txtHostName.Name = "txtHostName";
            this.txtHostName.Size = new System.Drawing.Size(174, 21);
            this.txtHostName.TabIndex = 1;
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(36, 26);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(70, 15);
            this.lblServerName.TabIndex = 4;
            this.lblServerName.Text = "Host Name";
            // 
            // btnCheckConnect
            // 
            this.btnCheckConnect.Location = new System.Drawing.Point(236, 165);
            this.btnCheckConnect.Name = "btnCheckConnect";
            this.btnCheckConnect.Size = new System.Drawing.Size(103, 23);
            this.btnCheckConnect.TabIndex = 6;
            this.btnCheckConnect.Text = "Kiểm tra kết nối";
            this.btnCheckConnect.UseVisualStyleBackColor = true;
            this.btnCheckConnect.Click += new System.EventHandler(this.btnCheckConnect_Click);
            // 
            // btnCreateConnect
            // 
            this.btnCreateConnect.Location = new System.Drawing.Point(154, 165);
            this.btnCreateConnect.Name = "btnCreateConnect";
            this.btnCreateConnect.Size = new System.Drawing.Size(75, 23);
            this.btnCreateConnect.TabIndex = 5;
            this.btnCreateConnect.Text = "Tạo kết nối";
            this.btnCreateConnect.UseVisualStyleBackColor = true;
            this.btnCreateConnect.Click += new System.EventHandler(this.btnCreateConnect_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.txtHostnameSql);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtPasswordSql);
            this.groupBox2.Controls.Add(this.txtDatabaseNameSql);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnCreateConnectSql);
            this.groupBox2.Controls.Add(this.txtUsernameSql);
            this.groupBox2.Controls.Add(this.btnCheckConnectSql);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 264);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(345, 192);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cấu hình kết nối máy bán";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(314, 62);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(14, 18);
            this.label13.TabIndex = 26;
            this.label13.Text = "*";
            // 
            // txtHostnameSql
            // 
            this.txtHostnameSql.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtHostnameSql.Location = new System.Drawing.Point(134, 59);
            this.txtHostnameSql.Name = "txtHostnameSql";
            this.txtHostnameSql.Size = new System.Drawing.Size(174, 20);
            this.txtHostnameSql.TabIndex = 8;
            this.txtHostnameSql.Text = ".\\";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(34, 62);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Host Name";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(314, 129);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(14, 18);
            this.label12.TabIndex = 23;
            this.label12.Text = "*";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(314, 93);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 18);
            this.label11.TabIndex = 22;
            this.label11.Text = "*";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(314, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 18);
            this.label10.TabIndex = 16;
            this.label10.Text = "*";
            // 
            // txtPasswordSql
            // 
            this.txtPasswordSql.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPasswordSql.Location = new System.Drawing.Point(134, 128);
            this.txtPasswordSql.Name = "txtPasswordSql";
            this.txtPasswordSql.Size = new System.Drawing.Size(174, 20);
            this.txtPasswordSql.TabIndex = 10;
            this.txtPasswordSql.Text = "1r0q5c3rewqA";
            this.txtPasswordSql.UseSystemPasswordChar = true;
            // 
            // txtDatabaseNameSql
            // 
            this.txtDatabaseNameSql.Location = new System.Drawing.Point(134, 25);
            this.txtDatabaseNameSql.Name = "txtDatabaseNameSql";
            this.txtDatabaseNameSql.Size = new System.Drawing.Size(174, 20);
            this.txtDatabaseNameSql.TabIndex = 7;
            this.txtDatabaseNameSql.Text = "ERBUS_CASHIER";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Password";
            // 
            // btnCreateConnectSql
            // 
            this.btnCreateConnectSql.Location = new System.Drawing.Point(154, 163);
            this.btnCreateConnectSql.Name = "btnCreateConnectSql";
            this.btnCreateConnectSql.Size = new System.Drawing.Size(75, 23);
            this.btnCreateConnectSql.TabIndex = 11;
            this.btnCreateConnectSql.Text = "Tạo kết nối";
            this.btnCreateConnectSql.UseVisualStyleBackColor = true;
            this.btnCreateConnectSql.Click += new System.EventHandler(this.btnCreateConnectSql_Click);
            // 
            // txtUsernameSql
            // 
            this.txtUsernameSql.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtUsernameSql.Location = new System.Drawing.Point(134, 90);
            this.txtUsernameSql.Name = "txtUsernameSql";
            this.txtUsernameSql.Size = new System.Drawing.Size(174, 20);
            this.txtUsernameSql.TabIndex = 9;
            this.txtUsernameSql.Text = "sa";
            // 
            // btnCheckConnectSql
            // 
            this.btnCheckConnectSql.Location = new System.Drawing.Point(236, 163);
            this.btnCheckConnectSql.Name = "btnCheckConnectSql";
            this.btnCheckConnectSql.Size = new System.Drawing.Size(103, 23);
            this.btnCheckConnectSql.TabIndex = 12;
            this.btnCheckConnectSql.Text = "Kiểm tra kết nối";
            this.btnCheckConnectSql.UseVisualStyleBackColor = true;
            this.btnCheckConnectSql.Click += new System.EventHandler(this.btnCheckConnectSql_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "User Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Database Name";
            // 
            // FrmConnectDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 456);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmConnectDatabase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cấu hình kết nối tới Cơ sở dữ liệu";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCheckConnect;
        private System.Windows.Forms.Button btnCreateConnect;
        private System.Windows.Forms.TextBox txtHostName;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.TextBox txtServiceName;
        private System.Windows.Forms.Label lblDatabaseName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtPasswordSql;
        private System.Windows.Forms.TextBox txtDatabaseNameSql;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCreateConnectSql;
        private System.Windows.Forms.TextBox txtUsernameSql;
        private System.Windows.Forms.Button btnCheckConnectSql;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtHostnameSql;
        private System.Windows.Forms.Label label14;
    }
}