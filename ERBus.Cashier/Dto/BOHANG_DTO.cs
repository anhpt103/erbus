using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERBus.Cashier.Dto
{
    public class BOHANG_DTO
    {
        public BOHANG_DTO()
        {
            MATHANG_BOHANG = new List<BOHANG_DETAILS_DTO>();
        }
        public string MABOHANG { get; set; }
        public decimal THANHTIEN { get; set; }
        public decimal TONGSL { get; set; }
        public string MAVAT_BO { get; set; }
        public decimal TYLEVAT_BO { get; set; }
        public List<BOHANG_DETAILS_DTO> MATHANG_BOHANG { get; set; }
    }
    public class BOHANG_DETAILS_DTO
    {
        public string MABOHANG { get; set; }
        public string MAHANG { get; set; }
        public decimal SOLUONG { get; set; }
        public decimal GIABANLE_VAT { get; set; }
        public decimal THANHTIEN { get; set; }
    }
}
