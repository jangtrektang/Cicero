using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Core.DataObjects
{
    public class Ingredient
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int RecipeID { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
