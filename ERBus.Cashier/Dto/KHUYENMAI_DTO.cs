using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERBus.Cashier.Dto
{
    public class KHUYENMAI_DTO
    {
        public string MA_KHUYENMAI { get; set; }
        public string LOAI_KHUYENMAI { get; set; }
        public string TUNGAY { get; set; }
        public string DENNGAY { get; set; }
        public string TUGIO{ get; set; }
        public string DENGIO{ get; set; }
        public string DIENGIAI { get; set; }
        public string MAHANG { get; set; }
        public decimal SOLUONG { get; set; }
        public decimal GIATRI_KHUYENMAI { get; set; }
    }
}
