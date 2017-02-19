using Cicero.Core.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Core.Helpers.ErrorLogging
{
    public static class ErrorHelper
    {
        public static void LogException(Exception ex, string url)
        {
            ErrorLog log = new ErrorLog()
            {
                Message = ex.Message,
                Type = ex.GetType().Name.ToString(),
                Source = ex.StackTrace,
                URL = url
            };

            CiceroContext db = new CiceroContext();
            db.ErrorLogs.Add(log);
            db.SaveChanges();
        }
    }
}
