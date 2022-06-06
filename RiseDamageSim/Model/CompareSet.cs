using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.Model
{
    internal class CompareSet
    {
        public string Name { get; set; } = string.Empty;
        public List<Equipment> Equipment { get; set; } = new();

        public override string ToString()
        {
            return Name;
        }
    }
}
