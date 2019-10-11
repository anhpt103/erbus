using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ERBus.Service.ModelExtend.BoHang
{
    public class BoHangExtend : ChoiceObject
    {
        [DataMember]
        public int? SAPXEP { get; set; }
    }
}
