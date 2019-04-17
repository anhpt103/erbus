using System.Collections.Generic;
using System.Windows.Forms;
using ERBus.Cashier.Dto;

namespace ERBus.Cashier.Giaodich.XuatBanLe
{
    public partial class frmSelectVatTu : Form
    {
        public SELECT_VATTU _handler;
        private List<VATTU_DTO> lstData;
        public frmSelectVatTu()
        {
            InitializeComponent();
        }
        public frmSelectVatTu(List<VATTU_DTO> listDataDto)
        {
            InitializeComponent();
            lstData = listDataDto;
            int indexRowNew = 1;
            foreach (VATTU_DTO item in lstData)
            {
                dgvSelect.Rows.Add((indexRowNew++), item.MAHANG,item.TENHANG);
            }
        }

        private void dgvSelect_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this._handler(e.RowIndex);
            }
        }

        public void setHandler(SELECT_VATTU handler)
        {
            this._handler = handler;
        }
    }
}
