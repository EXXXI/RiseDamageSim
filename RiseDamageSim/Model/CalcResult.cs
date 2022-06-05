using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.Model
{
    internal class CalcResult
    {
        public Equipment Equipment { get; set; }
        public string Name
        { 
            get
            {
                return Equipment.Name;
            }
        }
        public double PhysicsDamage { get; set; }
        public double ElementDamage { get; set; }
        public double ExpectedCriticalRate { get; set; }
        public double FullDamage 
        {
            get
            {
                return PhysicsDamage + ElementDamage;
            }
        }

    }
}
