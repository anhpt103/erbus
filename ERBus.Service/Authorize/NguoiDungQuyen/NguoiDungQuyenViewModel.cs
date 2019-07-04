using System;
using System.Collections.Generic;

namespace ERBus.Service.Authorize.NguoiDungNhomQuyen
{
    public class NguoiDungQuyenViewModel
    {
        public class ViewModel 
        {
            public ViewModel()
            {
                ID = Guid.NewGuid().ToString();
                XEM = false;
                THEM = false;
                SUA = false;
                XOA = false;
                DUYET = false;
                GIAMUA = false;
                GIABAN = false;
                GIAVON = false;
                TYLELAI = false;
                BANCHIETKHAU = false;
                BANBUON = false;
                BANTRALAI = false;
            }
            public string ID { get; set; }
            public string MA_MENU { get; set; }
            public string TIEUDE { get; set; }
            public int SAPXEP { get; set; }
            public string USERNAME { get; set; }
            public bool XEM { get; set; }
            public bool THEM { get; set; }
            public bool SUA { get; set; }
            public bool XOA { get; set; }
            public bool DUYET { get; set; }
            public bool GIAMUA { get; set; }
            public bool GIABAN { get; set; }
            public bool GIAVON { get; set; }
            public bool TYLELAI { get; set; }
            public bool BANCHIETKHAU { get; set; }
            public bool BANBUON { get; set; }
            public bool BANTRALAI { get; set; }
        }
        public class ConfigModel
        {
            public string USERNAME { get; set; }
            public string UNITCODE { get; set; }
            public List<ViewModel> LstAdd { get; set; }
            public List<ViewModel> LstEdit { get; set; }
            public List<ViewModel> LstDelete { get; set; }
        }
    }
}
