using ERBus.Cashier.Common;
using ERBus.Cashier.Dto;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public partial class frmPrintBill : DevExpress.XtraEditors.XtraForm
    {
        public bool done { get; set; }
        public frmPrintBill()
        {
            InitializeComponent();
        }
        /// <summary>
        /// fill data to bill
        /// </summary>
        /// <param name="objBillDto"></param>
        /// <param name="data"></param>
        /// GIAO DỊCH BÁN LẺ
        public void PrintInvoice_BanLe(BILL_DTO objBillDto , GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL)
        {
            done = true;
            try
            {
                ReportBill report = new ReportBill();
                foreach (Parameter p in report.Parameters)
                {
                    p.Visible = false;
                }
                report.InitDataBillBanLe(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL, objBillDto);
                dcmvContent.DocumentSource = report;
                report.CreateDocument();
                report.Print();
            }
            catch
            {
                NotificationLauncher.ShowNotificationError("THÔNG BÁO", "KHÔNG TÌM THẤY MÁY IN", 1, "0x1", "0x8", "normal");
            }
           
        }
    }
}