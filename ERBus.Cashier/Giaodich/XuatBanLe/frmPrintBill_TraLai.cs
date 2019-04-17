using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using ERBus.Cashier.Common;
using ERBus.Cashier.Dto;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public partial class frmPrintBill_TraLai : DevExpress.XtraEditors.XtraForm
    {
        public bool done { get; set; }
        public frmPrintBill_TraLai()
        {
            InitializeComponent();
        }
        /// <summary>
        /// fill data to bill
        /// </summary>
        /// <param name="objBillDto"></param>
        /// <param name="data"></param>
        /// GIAO DỊCH BÁN LẺ
        public void PrintInvoice_BanLeTraLai(BILL_DTO objBillDto , GIAODICH_DTO _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL)
        {
            done = true;
            try
            {
                ReportBill_TraLai report = new ReportBill_TraLai();
                foreach (Parameter p in report.Parameters)
                {
                    p.Visible = false;
                }
                report.InitDataBillBanLeTraLai(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL, objBillDto);
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