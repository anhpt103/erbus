namespace ERBus.Cashier
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassWord = new System.Windows.Forms.TextBox();
            this.txtNgayPhatSinh = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnModifieldPassword = new System.Windows.Forms.Button();
            this.btnCreateConnect = new System.Windows.Forms.Button();
            this.splashScreenManagerLogin = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::ERBus.Cashier.WaitForm1), true, true);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkCyan;
            this.label1.Location = new System.Drawing.Point(280, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 22);
            this.label1.TabIndex = 7;
            this.label1.Text = "Tài khoản";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkCyan;
            this.label2.Location = new System.Drawing.Point(282, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 22);
            this.label2.TabIndex = 9;
            this.label2.Text = "Mật khẩu";
            // 
            // txtUserName
            // 
            this.txtUserName.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(385, 64);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(200, 29);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserName_KeyDown);
            // 
            // txtPassWord
            // 
            this.txtPassWord.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassWord.Location = new System.Drawing.Point(385, 111);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.Size = new System.Drawing.Size(200, 29);
            this.txtPassWord.TabIndex = 2;
            this.txtPassWord.UseSystemPasswordChar = true;
            this.txtPassWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassWord_KeyDown);
            // 
            // txtNgayPhatSinh
            // 
            this.txtNgayPhatSinh.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNgayPhatSinh.Location = new System.Drawing.Point(385, 19);
            this.txtNgayPhatSinh.Name = "txtNgayPhatSinh";
            this.txtNgayPhatSinh.Size = new System.Drawing.Size(200, 29);
            this.txtNgayPhatSinh.TabIndex = 12;
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Image = global::ERBus.Cashier.Properties.Resources.login_icon_24px;
            this.btnLogin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogin.Location = new System.Drawing.Point(305, 163);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(152, 31);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Image = global::ERBus.Cashier.Properties.Resources.exit_icon_24px;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(463, 163);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(122, 31);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Thoát";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.btnModifieldPassword);
            this.groupBox2.Controls.Add(this.btnCreateConnect);
            this.groupBox2.Controls.Add(this.txtUserName);
            this.groupBox2.Controls.Add(this.txtNgayPhatSinh);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnExit);
            this.groupBox2.Controls.Add(this.btnLogin);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtPassWord);
            this.groupBox2.Location = new System.Drawing.Point(2, -1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(599, 261);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Image = global::ERBus.Cashier.Properties.Resources.user_login_icon;
            this.pictureBox1.Location = new System.Drawing.Point(0, 6);
            this.pictureBox1.MaximumSize = new System.Drawing.Size(276, 248);
            this.pictureBox1.MinimumSize = new System.Drawing.Size(276, 248);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(276, 248);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // btnModifieldPassword
            // 
            this.btnModifieldPassword.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModifieldPassword.ForeColor = System.Drawing.Color.Navy;
            this.btnModifieldPassword.Image = global::ERBus.Cashier.Properties.Resources.change_pass_icon_24px;
            this.btnModifieldPassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModifieldPassword.Location = new System.Drawing.Point(470, 229);
            this.btnModifieldPassword.Name = "btnModifieldPassword";
            this.btnModifieldPassword.Size = new System.Drawing.Size(128, 32);
            this.btnModifieldPassword.TabIndex = 14;
            this.btnModifieldPassword.Text = "Đổi mật khẩu";
            this.btnModifieldPassword.UseVisualStyleBackColor = true;
            this.btnModifieldPassword.Click += new System.EventHandler(this.btnModifieldPassword_Click);
            // 
            // btnCreateConnect
            // 
            this.btnCreateConnect.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateConnect.ForeColor = System.Drawing.Color.Navy;
            this.btnCreateConnect.Image = global::ERBus.Cashier.Properties.Resources.sign_sync_icon_24px;
            this.btnCreateConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateConnect.Location = new System.Drawing.Point(339, 229);
            this.btnCreateConnect.Name = "btnCreateConnect";
            this.btnCreateConnect.Size = new System.Drawing.Size(132, 32);
            this.btnCreateConnect.TabIndex = 13;
            this.btnCreateConnect.Text = "Cấu hình kết nối";
            this.btnCreateConnect.UseVisualStyleBackColor = true;
            this.btnCreateConnect.Click += new System.EventHandler(this.btnCreateConnect_Click);
            // 
            // splashScreenManagerLogin
            // 
            this.splashScreenManagerLogin.ClosingDelay = 500;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(602, 260);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Navy;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(618, 299);
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassWord;
        private System.Windows.Forms.TextBox txtNgayPhatSinh;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManagerLogin;
        private System.Windows.Forms.Button btnCreateConnect;
        private System.Windows.Forms.Button btnModifieldPassword;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

