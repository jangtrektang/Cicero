using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Core.DataObjects
{
    public class Instruction
    {
        public int ID { get; set; }
        public string Content { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
