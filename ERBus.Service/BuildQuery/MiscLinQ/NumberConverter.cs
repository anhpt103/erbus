﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERBus.Service.BuildQuery.Query.MiscLinQ
{
    public class NumberConverter : IConverter
    {
        public string MapTo(dynamic value)
        {
            var result = value.ToString();
            return result;
        }
    }
}