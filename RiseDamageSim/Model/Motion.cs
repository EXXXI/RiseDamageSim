using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RiseDamageSim.Model
{
    internal class Motion
    {
        [JsonIgnore]
        public double Frequency { get; set; }
        [JsonIgnore]
        public double PunishingDrawProbability { get; set; }
        [JsonIgnore]
        public double CriticalDrawProbability { get; set; }

        public string Name { get; set; }
        public int MotionValue { get; set; }
        public double SharpnessModifier { get; set; } = 1.0;
        public PhysicsElement PhysicsElement { get; set; }
        public double ElementModifier { get; set; } = 1.0;
        public bool IsSilkbind { get; set; } = false;


        public override string ToString()
        {
            return Name;
        }
    }
}
