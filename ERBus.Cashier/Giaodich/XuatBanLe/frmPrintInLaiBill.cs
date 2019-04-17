using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using ERBus.Cashier.Common;
using ERBus.Cashier.Dto;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public partial class frmPrintInLaiBill : DevExpress.XtraEditors.XtraForm
    {
        public bool done { get; set; }
        public frmPrintInLaiBill()
        {
            InitializeComponent();
        }
        /// <summary>
        /// fill data to bill
        /// </summary>
        /// <param name="objBillDto"></param>
        /// <param name="data"></param>
        /// GIAO DỊCH BÁN LẺ
        public void PrintInvoice_BanLeInLai(BILL_DTO objBillDto , GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL)
        {
            done = true;
            try
            {
                ReportInLaiBill reportInLai = new ReportInLaiBill();
                foreach (Parameter p in reportInLai.Parameters)
                {
                    p.Visible = false;
                }
                reportInLai.InitDataInLaiBillBanLe(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL, objBillDto);
                dcmvContent.DocumentSource = reportInLai;
                reportInLai.CreateDocument();
                reportInLai.Print();
            }
            catch
            {
                NotificationLauncher.ShowNotificationError("THÔNG BÁO", "KHÔNG TÌM THẤY MÁY IN", 1, "0x1", "0x8", "normal");
            }
           
        }
    }
}