using ERBus.Entity.Database.Authorize;
using ERBus.Service;
using System.Collections.Generic;

namespace ERBus.Service.Authorize.NguoiDungNhomQuyen
{
    public class NguoiDungNhomQuyenViewModel
    {
        public class ViewModel 
        {
            public string ID { get; set; }
            public string USERNAME { get; set; }
            public string MANHOMQUYEN { get; set; }
            public string TENNHOMQUYEN { get; set; }
        }
        public class ConfigModel
        {
            public string USERNAME { get; set; }
            public List<NGUOIDUNG_NHOMQUYEN> LstAdd { get; set; }
            public List<NGUOIDUNG_NHOMQUYEN> LstDelete { get; set; }
        }
    }
}
