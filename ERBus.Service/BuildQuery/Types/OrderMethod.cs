﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ERBus.Service.BuildQuery.Query.Types
{
    [Serializable]
    [DataContract]
    public enum OrderMethod
    {
        [DataMember]
        ASC,
        [DataMember]
        DESC
    }
}
