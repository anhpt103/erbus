using System;
using System.Windows.Forms;
using ERBus.Cashier.Dto;
using ERBus.Cashier.Giaodich.XuatBanLe;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
namespace ERBus.Cashier
{
    public partial class FrmMain : Form
    {
        private static int _i = 0;
        public static int _currentUcFrame = 0;
        public FrmMain()
        {
            InitializeComponent();
            try
            {
                ROLE_STATE GET_ACCESS_MENU = Session.Session.GET_ROLE_BY_USERNAME(Session.Session.CurrentUnitCode,
                    Session.Session.CurrentUserName, "banLeQuayThuNgan");
                if (GET_ACCESS_MENU != null)
                {
                    //if (!GET_ACCESS_MENU.Bantralai) btnTraLai.Enabled = false;
                }
            }
            catch
            {
                
            }
            tabControlXuatBanLe.CustomHeaderButtonClick += TabControlXuatBanLeOnCustomHeaderButtonClick;
            tabControlXuatBanLe.CloseButtonClick += TabControlXuatBanLeOnCloseButtonClick;
            tabControlXuatBanLe.Dock = DockStyle.Fill;
            lblInfoThuNgan.Text = "Mã: " + Session.Session.CurrentMaNhanVien.Trim() + "     Họ và tên: " + Session.Session.CurrentTenNhanVien.Trim();
        }
        private void InitializeTabPage(int index)
        {
            XtraTabPage tabPage = new XtraTabPage();
            tabPage.Name = "tabPage_" + index;
            tabPage.Text = "Xuất bán lẻ " + (index + 1);
            tabControlXuatBanLe.TabPages.Add(tabPage);
            _currentUcFrame = index;
            UC_Frame_BanLe ucFrameBanLe = new UC_Frame_BanLe();
            ucFrameBanLe.Dock = DockStyle.Fill;
            //tabControlXuatBanLe.TabPages[index].Controls.Add(ucFrameBanLe);
            tabPage.Controls.Add(ucFrameBanLe);
            tabControlXuatBanLe.SelectedTabPageIndex = index;
            ucFrameBanLe.Focus();
        }
        private void InitializeTabPageReturn(int index)
        {
            XtraTabPage tabPage = new XtraTabPage();
            tabPage.Name = "tabPage_" + index;
            tabPage.Text = "Xuất bán trả lại ";
            tabControlXuatBanLe.TabPages.Add(tabPage);
            _currentUcFrame = index;
            UC_Frame_TraLai ucFrameTraLai = new UC_Frame_TraLai();
            ucFrameTraLai.Dock = DockStyle.Fill;
            //tabControlXuatBanLe.TabPages[index].Controls.Add(ucFrameBanLe);
            tabPage.Controls.Add(ucFrameTraLai);
            tabControlXuatBanLe.SelectedTabPageIndex = index;
            ucFrameTraLai.Focus();
        }
        private void TabControlXuatBanLeOnCloseButtonClick(object sender, EventArgs eventArgs)
        {
            try
            {
                ClosePageButtonEventArgs obj = (ClosePageButtonEventArgs) eventArgs;
                XtraTabPage page = (XtraTabPage)obj.Page;
                tabControlXuatBanLe.SelectedTabPageIndex = page.TabIndex-1;
                tabControlXuatBanLe.TabPages.Remove(page);
            }
            catch (Exception){
                MessageBox.Show("Không thể xóa tab!");
            }
        }
        private void TabControlXuatBanLeOnCustomHeaderButtonClick(object sender, CustomHeaderButtonEventArgs customHeaderButtonEventArgs)
        {
            if (tabControlXuatBanLe.TabPages.Count == 0)
            {
                _i = 0;
            }
            else
            {
                foreach (XtraTabPage page in tabControlXuatBanLe.TabPages)
                {
                    if (page.TabIndex > _i)
                        _i = page.TabIndex;
                }
                _i += 1;
            }
            InitializeTabPage(_i);
        }
        private void menuXuatBanLe_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            tabControlXuatBanLe.Visible = true;
            InitializeTabPage(_i);
            menuXuatBanLe.Enabled = false;
        }
        private void tabControlXuatBanLe_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            try
            {
                _currentUcFrame = e.Page.TabIndex;
            }
            catch (Exception){}
        }

        private void btnTraLai_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            tabControlXuatBanLe.Visible = true;
            if (tabControlXuatBanLe.TabPages.Count == 0)
            {
                _i = 0;
            }
            else
            {
                foreach (XtraTabPage page in tabControlXuatBanLe.TabPages)
                {
                    if (page.TabIndex > _i)
                        _i = page.TabIndex;
                }
                _i += 1;
            }
            InitializeTabPageReturn(_i);
        }

        private void btnMenuDongBoGiaoDichQuay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmGuiDuLieuBan frmGuiDuLieuBan = new frmGuiDuLieuBan();
            frmGuiDuLieuBan.ShowDialog();}
    }
}
