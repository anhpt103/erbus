using System;
using System.ComponentModel.DataAnnotations;

namespace ERBus.Service
{
    public class DataInfoEntityDto
    {
        [StringLength(50)]
        public string ID { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public virtual DateTime? I_CREATE_DATE { get; set; }

        [StringLength(25)]
        public virtual string I_CREATE_BY { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public virtual DateTime? I_UPDATE_DATE { get; set; }

        [StringLength(25)]
        public virtual string I_UPDATE_BY { get; set; }

        [StringLength(1)]
        public virtual string I_STATE { get; set; }

    }
}
