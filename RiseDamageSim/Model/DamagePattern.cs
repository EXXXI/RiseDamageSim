using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.Model
{
    internal class DamagePattern
    {
        /// <summary>
        /// 確率
        /// </summary>
        public double Probability { get; set; }
        
        /// <summary>
        /// 武器+百竜スキルの攻撃力
        /// </summary>
        public double WeaponAttack { get; set; }

        /// <summary>
        /// ほとんどの攻撃力乗算補正
        /// </summary>
        public double AttackModifier1 { get; set; } = 1.0;

        /// <summary>
        /// ほとんどの攻撃力加算補正
        /// </summary>
        public double AdditionalAttack1 { get; set; }

        /// <summary>
        /// 笛などの攻撃力乗算補正
        /// </summary>
        public double AttackModifier2 { get; set; } = 1.0;

        /// <summary>
        /// 特殊な攻撃力加算補正(抜刀【力】のみ)
        /// </summary>
        public int AdditionalAttack2 { get; set; }

        /// <summary>
        /// 物理切れ味補正
        /// </summary>
        public double PhysicsSharpnessModifier { get; set; } = 1.0;

        /// <summary>
        /// 会心率
        /// </summary>
        public int CriticalRate { get; set; }

        /// <summary>
        /// 会心補正値
        /// </summary>
        public double PhysicsCriticalModifier { get; set; } = 1.25;

        /// <summary>
        /// ダメージ乗算補正
        /// </summary>
        public double PhysicsOtherModifier { get; set; } = 1.0;

        /// <summary>
        /// モーション値
        /// </summary>
        public int Motion { get; set; }

        /// <summary>
        /// 物理属性
        /// </summary>
        public PhysicsElement PhysicsElement { get; set; }

        /// <summary>
        /// 物理肉質
        /// </summary>
        public int PhysicsPhysiology { get; set; }

        /// <summary>
        /// 属性値(百竜スキル込み)
        /// </summary>
        public double ElementValue { get; set; }

        /// <summary>
        /// 属性強化乗算補正
        /// </summary>
        public double ElementModifier1 { get; set; } = 1.0;

        /// <summary>
        /// 属性強化加算補正
        /// </summary>
        public double AdditionalElement { get; set; }

        /// <summary>
        /// 笛の属性乗算補正
        /// </summary>
        public double ElementModifier2 { get; set; } = 1.0;

        /// <summary>
        /// 最後にかかる属性乗算補正
        /// </summary>
        public double ElementOtherModifier { get; set; } = 1.0;

        /// <summary>
        /// 会心撃属性の倍率
        /// </summary>
        public double ElementCriticalModifier { get; set; } = 1.0;

        /// <summary>
        /// 属性切れ味補正
        /// </summary>
        public double ElementSharpnessModifier { get; set; } = 1.0;

        /// <summary>
        /// 属性肉質
        /// </summary>
        public int ElementPhysiology { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public Element ElementKind { get; set; }

        /// <summary>
        /// 痛恨の一撃
        /// </summary>
        public bool BrutalStrike { get; set; } = false;

        public int PhysicsDamage {
            get
            {
                int attack = Round89((((Round89(WeaponAttack) * AttackModifier1) + AdditionalAttack1) * AttackModifier2) + AdditionalAttack2);
                double critRate = Math.Max(Math.Min(CriticalRate, 100), -100) / 100.0;
                double expectedCriticalModifier;
                if (critRate < 0)
                {
                    if (BrutalStrike)
                    {
                        // 27%とすると期待値0.9525=1-0.0475
                        // 30%とすると0.975=1-0.025
                        // 33.3...%とすると期待値1
                        expectedCriticalModifier = 1.0 + (critRate * 0.0475);
                    }
                    else
                    {
                        expectedCriticalModifier = 1.0 + (critRate * 0.25);
                    }
                }
                else
                {
                    expectedCriticalModifier = 1.0 + (critRate * (PhysicsCriticalModifier - 1.0));
                }

                return Round45(attack * PhysicsSharpnessModifier * expectedCriticalModifier * PhysicsOtherModifier * Motion / 100.0 * PhysicsPhysiology / 100.0);
            }
        }

        public int ElementDamage
        {
            get
            {
                int element = Round89(Round89((ElementValue * ElementModifier1) + AdditionalElement) * ElementModifier2);
                double critRate = Math.Max(Math.Min(CriticalRate, 100), -100) / 100.0;
                double expectedCriticalModifier = 1.0;
                if (critRate > 0 && ElementCriticalModifier > 1.0)
                {
                    expectedCriticalModifier = 1.0 + (critRate * (ElementCriticalModifier - 1.0));
                }
                return Round45(element * ElementSharpnessModifier * expectedCriticalModifier * ElementOtherModifier * ElementPhysiology / 100.0);
            }
        }



        public DamagePattern Clone()
        {
            return (DamagePattern)MemberwiseClone();
        }




        static private int Round89(double value)
        {
            return (int)(value + 0.1);
        }
        static private int Round45(double value)
        {
            return (int)(value + 0.5);
        }
    }
}
