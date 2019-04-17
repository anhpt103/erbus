using System;
using log4net;

namespace ERBus.Cashier.Common
{
    public class WriteLogs
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void LogError(Exception ex)
        {
            log.Error(ex);
        }

        public static void LogWarning(Exception ex)
        {
            log.Warn(ex);
        }

        public static void LogInfo(Exception ex)
        {
            log.Info(ex);
        }

        public static void LogFatal(Exception ex)
        {
            log.Fatal(ex);
        }
    }
}
