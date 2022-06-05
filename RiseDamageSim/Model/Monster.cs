using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.Model
{
    internal class Monster
    {
        public string Name { get; set; } = string.Empty;
        public List<Physiology> Physiologies { get; set; } = new List<Physiology>();

        public Physiology? GetPhysiology(string name, int state)
        {
            foreach (var physiology in Physiologies)
            {
                if (physiology.Name == name && physiology.State == state)
                {
                    return physiology;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
