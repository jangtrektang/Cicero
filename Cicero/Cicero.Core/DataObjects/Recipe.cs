using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Core.DataObjects
{
    public class Recipe
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public int UserID { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<Instruction> Instructions { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
