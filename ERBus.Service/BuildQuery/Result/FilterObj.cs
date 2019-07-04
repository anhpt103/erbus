using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERBus.Service.BuildQuery
{
    public class FilterObj : FilterObj<object>
    {
    }

    public class FilterObj<T>
    {
        public string Summary { get; set; }
        public bool IsAdvance { get; set; }
        public string OrderBy { get; set; }
        public string OrderType { get; set; }
        public T AdvanceData { get; set; }
        public string UNITCODE { get; set; }
        public string PARENT_UNITCODE { get; set; }
    }
}