namespace ERBus.Cashier
{
    partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnXuatBanLe = new DevExpress.XtraBars.BarButtonItem();
            this.menuXuatBanLe = new DevExpress.XtraBars.BarButtonItem();
            this.barLinkContainerItem1 = new DevExpress.XtraBars.BarLinkContainerItem();
            this.btnGoToHome = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.btnBanTraLai = new DevExpress.XtraBars.BarButtonItem();
            this.btnTraLai = new DevExpress.XtraBars.BarButtonItem();
            this.btnMenuDongBoGiaoDichQuay = new DevExpress.XtraBars.BarButtonItem();
            this.btnDongBoGiaoDich = new DevExpress.XtraBars.BarButtonItem();
            this.btnHome = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage3 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.btnXuatBanBuon = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ribbonPageCategory1 = new DevExpress.XtraBars.Ribbon.RibbonPageCategory();
            this.lblInfoThuNgan = new System.Windows.Forms.Label();
            this.tabControlXuatBanLe = new DevExpress.XtraTab.XtraTabControl();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlXuatBanLe)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.btnXuatBanLe,
            this.menuXuatBanLe,
            this.barLinkContainerItem1,
            this.btnGoToHome,
            this.barButtonItem2,
            this.btnBanTraLai,
            this.btnTraLai,
            this.btnMenuDongBoGiaoDichQuay,
            this.btnDongBoGiaoDich,
            this.btnHome});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 13;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage3});
            this.ribbonControl1.QuickToolbarItemLinks.Add(this.btnGoToHome);
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.OfficeUniversal;
            this.ribbonControl1.Size = new System.Drawing.Size(1032, 54);
            // 
            // btnXuatBanLe
            // 
            this.btnXuatBanLe.Caption = "btnXuatBanLe";
            this.btnXuatBanLe.Id = 1;
            this.btnXuatBanLe.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnXuatBanLe.ImageOptions.Image")));
            this.btnXuatBanLe.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnXuatBanLe.ImageOptions.LargeImage")));
            this.btnXuatBanLe.LargeWidth = 10;
            this.btnXuatBanLe.Name = "btnXuatBanLe";
            this.btnXuatBanLe.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // menuXuatBanLe
            // 
            this.menuXuatBanLe.Caption = "Xuất bán";
            this.menuXuatBanLe.Id = 3;
            this.menuXuatBanLe.ImageOptions.Image = global::ERBus.Cashier.Properties.Resources.retail_shop_icon_24px;
            this.menuXuatBanLe.ImageOptions.ImageIndex = 2;
            this.menuXuatBanLe.Name = "menuXuatBanLe";
            this.menuXuatBanLe.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.menuXuatBanLe.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuXuatBanLe_ItemClick);
            // 
            // barLinkContainerItem1
            // 
            this.barLinkContainerItem1.Caption = "barLinkContainerItem1";
            this.barLinkContainerItem1.Id = 4;
            this.barLinkContainerItem1.Name = "barLinkContainerItem1";
            // 
            // btnGoToHome
            // 
            this.btnGoToHome.Caption = "Hệ thống siêu thị DABACO";
            this.btnGoToHome.Id = 5;
            this.btnGoToHome.Name = "btnGoToHome";
            this.btnGoToHome.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.Id = 6;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // btnBanTraLai
            // 
            this.btnBanTraLai.Caption = "Nhập bán trả lại";
            this.btnBanTraLai.Id = 7;
            this.btnBanTraLai.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnBanTraLai.ImageOptions.Image")));
            this.btnBanTraLai.Name = "btnBanTraLai";
            this.btnBanTraLai.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            // 
            // btnTraLai
            // 
            this.btnTraLai.Caption = "Bán trả lại";
            this.btnTraLai.Id = 8;
            this.btnTraLai.ImageOptions.Image = global::ERBus.Cashier.Properties.Resources.return_Purchase_icon_24px;
            this.btnTraLai.ImageOptions.ImageIndex = 1;
            this.btnTraLai.Name = "btnTraLai";
            this.btnTraLai.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTraLai_ItemClick);
            // 
            // btnMenuDongBoGiaoDichQuay
            // 
            this.btnMenuDongBoGiaoDichQuay.Caption = "Đồng bộ giao dịch quầy";
            this.btnMenuDongBoGiaoDichQuay.Id = 10;
            this.btnMenuDongBoGiaoDichQuay.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnMenuDongBoGiaoDichQuay.ImageOptions.Image")));
            this.btnMenuDongBoGiaoDichQuay.Name = "btnMenuDongBoGiaoDichQuay";
            this.btnMenuDongBoGiaoDichQuay.Tag = "Đồng bộ giao dịch quầy";
            // 
            // btnDongBoGiaoDich
            // 
            this.btnDongBoGiaoDich.Caption = "Đồng bộ giao dịch";
            this.btnDongBoGiaoDich.Id = 11;
            this.btnDongBoGiaoDich.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDongBoGiaoDich.ImageOptions.Image")));
            this.btnDongBoGiaoDich.ImageOptions.ImageIndex = 3;
            this.btnDongBoGiaoDich.Name = "btnDongBoGiaoDich";
            this.btnDongBoGiaoDich.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDongBoGiaoDich_ItemClick);
            // 
            // btnHome
            // 
            this.btnHome.Caption = "Trang chủ";
            this.btnHome.Id = 12;
            this.btnHome.ImageOptions.Image = global::ERBus.Cashier.Properties.Resources.house_icon_24px;
            this.btnHome.Name = "btnHome";
            this.btnHome.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHome_ItemClick);
            // 
            // ribbonPage3
            // 
            this.ribbonPage3.Appearance.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.ribbonPage3.Appearance.Options.UseFont = true;
            this.ribbonPage3.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup4});
            this.ribbonPage3.Name = "ribbonPage3";
            this.ribbonPage3.Text = "Giao dịch";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.btnHome, true);
            this.ribbonPageGroup4.ItemLinks.Add(this.btnDongBoGiaoDich);
            this.ribbonPageGroup4.ItemLinks.Add(this.menuXuatBanLe);
            this.ribbonPageGroup4.ItemLinks.Add(this.btnTraLai);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            // 
            // btnXuatBanBuon
            // 
            this.btnXuatBanBuon.Caption = "Xuất bán buôn";
            this.btnXuatBanBuon.Id = 2;
            this.btnXuatBanBuon.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnXuatBanBuon.ImageOptions.Image")));
            this.btnXuatBanBuon.Name = "btnXuatBanBuon";
            this.btnXuatBanBuon.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Xuất bán buôn";
            this.barButtonItem1.Id = 2;
            this.barButtonItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.Image")));
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            // 
            // ribbonPageCategory1
            // 
            this.ribbonPageCategory1.Name = "ribbonPageCategory1";
            this.ribbonPageCategory1.Text = "ribbonPageCategory1";
            // 
            // lblInfoThuNgan
            // 
            this.lblInfoThuNgan.AutoSize = true;
            this.lblInfoThuNgan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(201)))), ((int)(((byte)(208)))));
            this.lblInfoThuNgan.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoThuNgan.ForeColor = System.Drawing.Color.Blue;
            this.lblInfoThuNgan.Location = new System.Drawing.Point(891, 0);
            this.lblInfoThuNgan.Name = "lblInfoThuNgan";
            this.lblInfoThuNgan.Size = new System.Drawing.Size(144, 23);
            this.lblInfoThuNgan.TabIndex = 11;
            this.lblInfoThuNgan.Text = "lblInfoThuNgan";
            // 
            // tabControlXuatBanLe
            // 
            this.tabControlXuatBanLe.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlXuatBanLe.Appearance.Options.UseFont = true;
            this.tabControlXuatBanLe.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders;
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.tabControlXuatBanLe.CustomHeaderButtons.AddRange(new DevExpress.XtraTab.Buttons.CustomHeaderButton[] {
            new DevExpress.XtraTab.Buttons.CustomHeaderButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "Add", -1, true, true, editorButtonImageOptions1, serializableAppearanceObject1, "", null, null)});
            this.tabControlXuatBanLe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlXuatBanLe.Location = new System.Drawing.Point(0, 54);
            this.tabControlXuatBanLe.Name = "tabControlXuatBanLe";
            this.tabControlXuatBanLe.Size = new System.Drawing.Size(1032, 678);
            this.tabControlXuatBanLe.TabIndex = 7;
            this.tabControlXuatBanLe.Visible = false;
            this.tabControlXuatBanLe.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tabControlXuatBanLe_SelectedPageChanged);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(368, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 309);
            this.panel1.TabIndex = 0;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 732);
            this.Controls.Add(this.lblInfoThuNgan);
            this.Controls.Add(this.tabControlXuatBanLe);
            this.Controls.Add(this.ribbonControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MinimumSize = new System.Drawing.Size(1022, 666);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ỨNG DỤNG BÁN LẺ HÀNG HÓA";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlXuatBanLe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarButtonItem btnXuatBanLe;
        private DevExpress.XtraBars.BarButtonItem menuXuatBanLe;
        private DevExpress.XtraBars.BarButtonItem btnXuatBanBuon;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarLinkContainerItem barLinkContainerItem1;
        private DevExpress.XtraBars.BarButtonItem btnGoToHome;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DevExpress.XtraBars.BarButtonItem btnBanTraLai;
        private DevExpress.XtraBars.BarButtonItem btnTraLai;
        private DevExpress.XtraBars.Ribbon.RibbonPageCategory ribbonPageCategory1;
        private System.Windows.Forms.Label lblInfoThuNgan;
        private DevExpress.XtraBars.BarButtonItem btnMenuDongBoGiaoDichQuay;
        private DevExpress.XtraTab.XtraTabControl tabControlXuatBanLe;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraBars.BarButtonItem btnDongBoGiaoDich;
        private DevExpress.XtraBars.BarButtonItem btnHome;
    }
}