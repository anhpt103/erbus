using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ERBus.Cashier.Common
{
    public class FormatCurrency
    {
        public static string FormatMoney(object money)
        {
            decimal tien = 0;
            decimal.TryParse(money.ToString(), out tien);
            tien = Decimal.Round(tien);
            string str = tien.ToString();
            string pattern = @"(?<a>\d*)(?<b>\d{3})*";
            Match m = Regex.Match(str, pattern, RegexOptions.RightToLeft);
            StringBuilder sb = new StringBuilder();
            foreach (Capture i in m.Groups["b"].Captures)
            {
                sb.Insert(0, "," + i.Value);
            }
            sb.Insert(0, m.Groups["a"].Value);
            return sb.ToString().Trim(',');
        }
    }
}
