using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.Model
{
    internal class Weapon
    {
        public string Name { get; set; } = string.Empty;
        public List<string> RampageSkills { get; set; } = new List<string>();
        public int Attack { get; set; }
        public Element ElementKind { get; set; }
        public int ElementValue { get; set; }
        public int CriticalRate { get; set; }
        public int Defence { get; set; }
        public Sharpness Sharpness { get; set; } = new Sharpness();
        public int Slot1 { get; set; }
        public int Slot2 { get; set; }
        public int Slot3 { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }
}
