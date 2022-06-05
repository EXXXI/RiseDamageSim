using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.Model
{
    internal enum PhysicsElement
    {
        Sever,
        Blunt,
        Shot
    }

    public static class PhysicsElementExt
    {
        // 日本語名を返す
        internal static string Str(this PhysicsElement physicsElement)
        {
            return physicsElement switch
            {
                PhysicsElement.Sever => "斬",
                PhysicsElement.Blunt => "打",
                PhysicsElement.Shot => "弾",
                _ => String.Empty,
            };
        }
    }
}
