using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Core.DataObjects
{
    public class ErrorLog
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }    
        public string URL { get; set; }
        public string Source { get; set; }
    }
}
