﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERBus.Service.BuildQuery.Query.MiscLinQ
{
    public class DateTimeConverter : IConverter
    {
        public string MapTo(dynamic value)
        {
            var data = (DateTime) value();
            var result = string.Format("DateTime({0},{1},{2})",
                data.Year,
                data.Month,
                data.Date);
            return result;
        }
    }
}