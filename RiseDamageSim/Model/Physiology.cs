using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RiseDamageSim.Model
{
    internal class Physiology
    {
        [JsonIgnore]
        public double Frequency { get; set; }
        [JsonIgnore]
        public double RageModifier { get; set; } = 1.0;
        [JsonIgnore]
        public bool IsAerial { get; set; } = false;
        [JsonIgnore]
        public bool IsAquatic { get; set; } = false;
        [JsonIgnore]
        public bool IsWyvern { get; set; } = false;
        [JsonIgnore]
        public bool IsSmall { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public int State { get; set; }
        public string StateName { get; set; } = "通常";
        public int Sever { get; set; }
        public int Blunt { get; set; }
        public int Shot { get; set; }
        public int Fire { get; set; }
        public int Water { get; set; }
        public int Thunder { get; set; }
        public int Ice { get; set; }
        public int Dragon { get; set; }
        public int Stun { get; set; }
        public int RageSever { get; set; }
        public int RageBlunt { get; set; }
        public int RageShot { get; set; }
        public int RageFire { get; set; }
        public int RageWater { get; set; }
        public int RageThunder { get; set; }
        public int RageIce { get; set; }
        public int RageDragon { get; set; }
        public int RageStun { get; set; }

        public int GetValue(PhysicsElement element)
        {
            return GetValue(element, false);
        }

        public int GetValue(PhysicsElement element, bool isRage)
        {
            if (isRage)
            {
                return element switch
                {
                    PhysicsElement.Sever => RageSever,
                    PhysicsElement.Blunt => RageBlunt,
                    PhysicsElement.Shot => RageShot,
                    _ => 0,
                };
            }
            else
            {
                return element switch
                {
                    PhysicsElement.Sever => Sever,
                    PhysicsElement.Blunt => Blunt,
                    PhysicsElement.Shot => Shot,
                    _ => 0,
                };
            }
        }

        public int GetValue(Element element)
        {
            return GetValue(element, false);
        }

        public int GetValue(Element element, bool isRage)
        {
            if (isRage)
            {
                return element switch
                {
                    Element.Fire => RageFire,
                    Element.Water => RageWater,
                    Element.Thunder => RageThunder,
                    Element.Ice => RageIce,
                    Element.Dragon => RageDragon,
                    _ => 0,
                };
            }
            else
            {
                return element switch
                {
                    Element.Fire => Fire,
                    Element.Water => Water,
                    Element.Thunder => Thunder,
                    Element.Ice => Ice,
                    Element.Dragon => Dragon,
                    _ => 0,
                };
            }
        }
    }
}
