using RiseDamageSim.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.Domain
{
    internal class Simulator
    {
        public List<Equipment> Equipments { get; set; }
        public List<Physiology> Physiologies { get; set; }
        public List<Motion> Motions { get; set; }
        public BattleStyle Style { get; set; }

        public Simulator(List<Equipment> equipments, List<Physiology> physiologies, List<Motion> motions, BattleStyle style)
        {
            Equipments = equipments;
            Physiologies = physiologies;
            Motions = motions;
            Style = style;
        }

        internal List<CalcResult> CalcDamageAll()
        {
            List<CalcResult> results = new();
            foreach (var equip in Equipments)
            {
                results.Add(CalcDamage(equip));
            }
            results.Sort(CompareFullDamage);
            return results;
        }

        private CalcResult CalcDamage(Equipment equip)
        {
            DamagePattern init = GetInitDamagePattern(equip);

            List<DamagePattern> damages = new();
            damages.Add(init);

            // モーション計算
            damages = CalcMotion(damages, equip);

            // 肉質計算
            damages = CalcPhysiology(damages, equip);

            // 会心計算
            damages = CalcCritical(damages, equip);
            double expectedCriticalRate = 0.0;
            foreach (var damage in damages)
            {
                expectedCriticalRate += Math.Min(Math.Max(damage.CriticalRate, -100), 100) * damage.Probability;
            }

            // 切れ味計算
            damages = CalcSharpness(damages, equip, expectedCriticalRate);

            // その他計算
            damages = CalcOthers(damages, equip);

            // 集計
            double expectedPhysicsDamage = 0.0;
            foreach (var damage in damages)
            {
                expectedPhysicsDamage += damage.PhysicsDamage * damage.Probability;
            }
            double expectedElementDamage = 0.0;
            foreach (var damage in damages)
            {
                expectedElementDamage += damage.ElementDamage * damage.Probability;
            }

            return new CalcResult()
            {
                Equipment = equip,
                PhysicsDamage = expectedPhysicsDamage,
                ElementDamage = expectedElementDamage,
                ExpectedCriticalRate = expectedCriticalRate,
            };
        }

        private DamagePattern GetInitDamagePattern(Equipment equip)
        {
            DamagePattern init = new()
            {
                Probability = 1.0,
                WeaponAttack = equip.Weapon.Attack,
                ElementValue = equip.Weapon.ElementValue,
                CriticalRate = equip.Weapon.CriticalRate,
                ElementKind = equip.Weapon.ElementKind
            };

            // 属性付与【火】
            if (GetRanpageLv(equip, "属性付与【火】") > 0)
            {
                int level = GetRanpageLv(equip, "属性付与【火】");
                init.ElementKind = Element.Fire;
                switch (level)
                {
                    case 1:
                        init.ElementValue = 10;
                        break;
                    case 2:
                        init.ElementValue = 15;
                        break;
                    case 3:
                        init.ElementValue = 20;
                        init.WeaponAttack -= 5;
                        break;
                    case 4:
                        init.ElementValue = 30;
                        init.WeaponAttack -= 10;
                        break;
                    default:
                        break;
                }
            }

            // 属性付与【水】
            if (GetRanpageLv(equip, "属性付与【水】") > 0)
            {
                int level = GetRanpageLv(equip, "属性付与【水】");
                init.ElementKind = Element.Water;
                switch (level)
                {
                    case 1:
                        init.ElementValue = 10;
                        break;
                    case 2:
                        init.ElementValue = 15;
                        break;
                    case 3:
                        init.ElementValue = 20;
                        init.WeaponAttack -= 5;
                        break;
                    case 4:
                        init.ElementValue = 30;
                        init.WeaponAttack -= 10;
                        break;
                    default:
                        break;
                }
            }

            // 属性付与【雷】
            if (GetRanpageLv(equip, "属性付与【雷】") > 0)
            {
                int level = GetRanpageLv(equip, "属性付与【雷】");
                init.ElementKind = Element.Thunder;
                switch (level)
                {
                    case 1:
                        init.ElementValue = 10;
                        break;
                    case 2:
                        init.ElementValue = 15;
                        break;
                    case 3:
                        init.ElementValue = 20;
                        init.WeaponAttack -= 5;
                        break;
                    case 4:
                        init.ElementValue = 30;
                        init.WeaponAttack -= 10;
                        break;
                    default:
                        break;
                }
            }

            // 属性付与【氷】
            if (GetRanpageLv(equip, "属性付与【氷】") > 0)
            {
                int level = GetRanpageLv(equip, "属性付与【氷】");
                init.ElementKind = Element.Ice;
                switch (level)
                {
                    case 1:
                        init.ElementValue = 10;
                        break;
                    case 2:
                        init.ElementValue = 15;
                        break;
                    case 3:
                        init.ElementValue = 20;
                        init.WeaponAttack -= 5;
                        break;
                    case 4:
                        init.ElementValue = 30;
                        init.WeaponAttack -= 10;
                        break;
                    default:
                        break;
                }
            }

            // 属性付与【龍】
            if (GetRanpageLv(equip, "属性付与【龍】") > 0)
            {
                int level = GetRanpageLv(equip, "属性付与【龍】");
                init.ElementKind = Element.Dragon;
                switch (level)
                {
                    case 1:
                        init.ElementValue = 10;
                        break;
                    case 2:
                        init.ElementValue = 15;
                        break;
                    case 3:
                        init.ElementValue = 20;
                        init.WeaponAttack -= 5;
                        break;
                    case 4:
                        init.ElementValue = 30;
                        init.WeaponAttack -= 10;
                        break;
                    default:
                        break;
                }
            }

            return init;
        }


        /// <summary>
        /// モーション値の計算
        /// </summary>
        /// <param name="damages"></param>
        /// <returns></returns>
        private List<DamagePattern> CalcMotion(List<DamagePattern> damages, Equipment equip)
        {
            List<DamagePattern> oldDamages;
            List<DamagePattern> newDamages;

            double sumFrequency = 0.0;
            foreach (var motion in Motions)
            {
                sumFrequency += motion.Frequency;
            }

            oldDamages = damages;
            newDamages = new();
            foreach (var oldDamage in oldDamages)
            {
                foreach (var motion in Motions)
                {
                    if (motion.Frequency > 0)
                    {

                        List<DamagePattern> motionOldDamages;
                        List<DamagePattern> motionNewDamages = new();

                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * motion.Frequency / sumFrequency;
                        newDamage.Motion = motion.MotionValue;
                        newDamage.PhysicsElement = motion.PhysicsElement;
                        newDamage.PhysicsSharpnessModifier = motion.SharpnessModifier;
                        newDamage.ElementOtherModifier *= motion.ElementModifier;
                        motionNewDamages.Add(newDamage);

                        // 鉄蟲糸技強化
                        if (HasRanpage(equip, "鉄蟲糸技強化") && motion.IsSilkbind)
                        {
                            newDamage.PhysicsOtherModifier *= 1.1;
                        }

                        // 抜刀力
                        if (equip.PunishingDrawLv > 0 && motion.PunishingDrawProbability > 0)
                        {
                            motionOldDamages = motionNewDamages;
                            motionNewDamages = new();
                            foreach (var motionOldDamage in motionOldDamages)
                            {
                                double punishingDrawProb = motion.PunishingDrawProbability / 100.0;
                                // 通常
                                if (punishingDrawProb < 1)
                                {
                                    DamagePattern motionNewDamage = motionOldDamage.Clone();
                                    motionNewDamage.Probability = motionOldDamage.Probability * (1 - punishingDrawProb);
                                    motionNewDamages.Add(motionNewDamage);
                                }
                                // 抜刀力
                                if (punishingDrawProb > 0)
                                {
                                    DamagePattern motionNewDamage = motionOldDamage.Clone();
                                    motionNewDamage.Probability = motionOldDamage.Probability * punishingDrawProb;
                                    switch (equip.PunishingDrawLv)
                                    {
                                        case 1:
                                            motionNewDamage.AdditionalAttack2 += 3;
                                            break;
                                        case 2:
                                            motionNewDamage.AdditionalAttack2 += 5;
                                            break;
                                        case >= 3:
                                            motionNewDamage.AdditionalAttack2 += 7;
                                            break;
                                        default:
                                            break;
                                    }
                                    motionNewDamages.Add(motionNewDamage);
                                }
                            }
                        }

                        // 抜刀技
                        if (equip.CriticalDrawLv > 0 && motion.CriticalDrawProbability > 0)
                        {
                            motionOldDamages = motionNewDamages;
                            motionNewDamages = new();
                            foreach (var motionOldDamage in motionOldDamages)
                            {
                                double criticalDrawProb = motion.CriticalDrawProbability / 100.0;
                                // 通常
                                if (criticalDrawProb < 1)
                                {
                                    DamagePattern motionNewDamage = motionOldDamage.Clone();
                                    motionNewDamage.Probability = motionOldDamage.Probability * (1 - criticalDrawProb);
                                    motionNewDamages.Add(motionNewDamage);
                                }
                                // 抜刀技
                                if (criticalDrawProb > 0)
                                {
                                    DamagePattern motionNewDamage = motionOldDamage.Clone();
                                    motionNewDamage.Probability = motionOldDamage.Probability * criticalDrawProb;
                                    switch (equip.CriticalDrawLv)
                                    {
                                        case 1:
                                            motionNewDamage.CriticalRate += 15;
                                            break;
                                        case 2:
                                            motionNewDamage.CriticalRate += 30;
                                            break;
                                        case >= 3:
                                            motionNewDamage.CriticalRate += 60;
                                            break;
                                        default:
                                            break;
                                    }
                                    motionNewDamages.Add(motionNewDamage);
                                }
                            }
                        }

                        // チャージマスター
                        if (equip.ChargeMasterLv > 0 && motion.ChargeLevel > 0)
                        {
                            double modifier = 1;
                            switch (equip.ChargeMasterLv)
                            {
                                case 1:
                                    modifier = 1.1;
                                    break;
                                case 2:
                                    modifier = 1.3;
                                    break;
                                case >= 3:
                                    modifier = 1.5;
                                    break;
                                default:
                                    break;
                            }

                            foreach (var motionNewDamage in motionNewDamages)
                            {
                                motionNewDamage.ElementOtherModifier *= modifier;
                            }
                        }

                        // 整理
                        foreach (var damage in motionNewDamages)
                        {
                            newDamages.Add(damage);
                        }
                    }
                }
            }
            return newDamages;
        }

        /// <summary>
        /// 肉質が変化する要因に対する計算
        /// 怒り(肉質変化・怒り倍率・挑戦者)・水やられ(肉質変化・水やられ特効)
        /// </summary>
        /// <param name="oldDamages"></param>
        /// <returns></returns>
        private List<DamagePattern> CalcPhysiology(List<DamagePattern> damages, Equipment equip)
        {
            List<DamagePattern> oldDamages;
            List<DamagePattern> newDamages = damages;

            // 肉質計算&怒り計算
            double sumFrequency = 0.0;
            foreach (var physiorogy in Physiologies)
            {
                sumFrequency += physiorogy.Frequency;
            }

            oldDamages = newDamages;
            newDamages = new();
            foreach (var oldDamage in oldDamages)
            {
                foreach (var physiology in Physiologies)
                {
                    if(physiology.Frequency > 0)
                    {
                        // 種族特効計算
                        double exploitModifier = 1.0;
                        if (physiology.IsAerial && HasRanpage(equip, "空棲系特効"))
                        {
                            exploitModifier *= 1.05;
                        }
                        if (physiology.IsAquatic && HasRanpage(equip, "水棲系特効"))
                        {
                            exploitModifier *= 1.1;
                        }
                        if (physiology.IsSmall && HasRanpage(equip, "小型特効"))
                        {
                            exploitModifier *= 1.50;
                        }
                        if (physiology.IsWyvern && HasRanpage(equip, "竜種特効"))
                        {
                            exploitModifier *= 1.05;
                        }

                        double rageProb = Style.RageProb / 100.0;
                        // 非怒り
                        if (rageProb < 1)
                        {
                            DamagePattern newDamage = oldDamage.Clone();
                            newDamage.Probability = oldDamage.Probability * physiology.Frequency * (1 - rageProb) / sumFrequency;
                            newDamage.PhysicsPhysiology = physiology.GetValue(newDamage.PhysicsElement);
                            newDamage.ElementPhysiology = physiology.GetValue(newDamage.ElementKind);
                            newDamage.PhysicsOtherModifier *= exploitModifier;
                            newDamages.Add(newDamage);
                        }
                        // 怒り
                        if (rageProb > 0)
                        {
                            DamagePattern newDamage = oldDamage.Clone();
                            newDamage.Probability = oldDamage.Probability * physiology.Frequency * rageProb / sumFrequency;
                            newDamage.PhysicsPhysiology = physiology.GetValue(newDamage.PhysicsElement, true);
                            newDamage.ElementPhysiology = physiology.GetValue(newDamage.ElementKind, true);
                            newDamage.PhysicsOtherModifier *= exploitModifier;
                            newDamage.PhysicsOtherModifier *= physiology.RageModifier;
                            newDamage.ElementOtherModifier *= physiology.RageModifier;
                            switch (equip.AgitatorLv)
                            {
                                case 1:
                                    newDamage.AdditionalAttack1 += 4;
                                    newDamage.CriticalRate += 3;
                                    break;
                                case 2:
                                    newDamage.AdditionalAttack1 += 8;
                                    newDamage.CriticalRate += 5;
                                    break;
                                case 3:
                                    newDamage.AdditionalAttack1 += 12;
                                    newDamage.CriticalRate += 7;
                                    break;
                                case 4:
                                    newDamage.AdditionalAttack1 += 16;
                                    newDamage.CriticalRate += 10;
                                    break;
                                case >= 5:
                                    newDamage.AdditionalAttack1 += 20;
                                    newDamage.CriticalRate += 15;
                                    break;
                                default:
                                    break;
                            }
                            newDamages.Add(newDamage);
                        }
                    }
                }
            }

            // 水やられ&水やられ特効
            if (Style.WaterBlightProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double waterBlightProb = Style.WaterBlightProb / 100.0;
                    // 通常
                    if (waterBlightProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - waterBlightProb) / sumFrequency;
                        newDamages.Add(newDamage);
                    }
                    // 水やられ
                    if (waterBlightProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * waterBlightProb / sumFrequency;
                        if (newDamage.PhysicsPhysiology > 60)
                        {
                            newDamage.PhysicsPhysiology += 3;
                        }
                        else
                        {
                            newDamage.PhysicsPhysiology = (int)((60 - newDamage.PhysicsPhysiology) * 0.37 + 3 + newDamage.PhysicsPhysiology);
                        }
                        if (HasRanpage(equip, "水やられ特効"))
                        {
                            newDamage.PhysicsOtherModifier *= 1.1;
                        }
                        newDamages.Add(newDamage);
                    }
                }
            }


            return newDamages;
        }

        /// <summary>
        /// 会心に関係する補正を計算
        /// </summary>
        /// <param name="damages"></param>
        /// <param name="equip"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private List<DamagePattern> CalcCritical(List<DamagePattern> damages, Equipment equip)
        {
            List<DamagePattern> oldDamages;
            List<DamagePattern> newDamages = damages;

            // 見切り
            if (equip.CriticalEyeLv > 0)
            {
                foreach (var newDamage in newDamages)
                {
                    switch (equip.CriticalEyeLv)
                    {
                        case 1:
                            newDamage.CriticalRate += 5;
                            break;
                        case 2:
                            newDamage.CriticalRate += 10;
                            break;
                        case 3:
                            newDamage.CriticalRate += 15;
                            break;
                        case 4:
                            newDamage.CriticalRate += 20;
                            break;
                        case 5:
                            newDamage.CriticalRate += 25;
                            break;
                        case 6:
                            newDamage.CriticalRate += 30;
                            break;
                        case 7:
                            newDamage.CriticalRate += 40;
                            break;
                        default:
                            break;
                    }
                }
            }

            // 弱点特効
            if (equip.WeaknessExploitLv > 0)
            {
                int add = 0;
                switch (equip.WeaknessExploitLv)
                {
                    case 1:
                        add = 15;
                        break;
                    case 2:
                        add = 30;
                        break;
                    case >= 3:
                        add = 50;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    if (newDamage.PhysicsPhysiology >= 45)
                    {
                        newDamage.CriticalRate += add;
                    }
                }
            }

            // 渾身
            if (equip.MaximumMightLv > 0 && Style.MaximumMightProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double maximumMightProb = Style.MaximumMightProb / 100.0;
                    // 通常
                    if (maximumMightProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - maximumMightProb);
                        newDamages.Add(newDamage);
                    }
                    // 渾身
                    if (maximumMightProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * maximumMightProb;
                        switch (equip.MaximumMightLv)
                        {
                            case 1:
                                newDamage.CriticalRate += 10;
                                break;
                            case 2:
                                newDamage.CriticalRate += 20;
                                break;
                            case >= 3:
                                newDamage.CriticalRate += 30;
                                break;
                            default:
                                break;
                        }

                        newDamages.Add(newDamage);
                    }
                }
            }

            // 力の開放
            if (equip.LatentPowerLv > 0 && Style.LatentPowerProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double latentPowerProb = Style.LatentPowerProb / 100.0;
                    // 通常
                    if (latentPowerProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - latentPowerProb);
                        newDamages.Add(newDamage);
                    }
                    // 力の開放
                    if (latentPowerProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * latentPowerProb;
                        switch (equip.LatentPowerLv)
                        {
                            case 1:
                                newDamage.CriticalRate += 10;
                                break;
                            case 2:
                                newDamage.CriticalRate += 20;
                                break;
                            case 3:
                                newDamage.CriticalRate += 30;
                                break;
                            case 4:
                                newDamage.CriticalRate += 40;
                                break;
                            case >= 5:
                                newDamage.CriticalRate += 50;
                                break;
                            default:
                                break;
                        }

                        newDamages.Add(newDamage);
                    }
                }
            }

            // 滑走強化
            if (equip.AffinitySlidingLv > 0 && Style.AffinitySlidingProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double affinitySlidingProb = Style.AffinitySlidingProb / 100.0;
                    // 通常
                    if (affinitySlidingProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - affinitySlidingProb);
                        newDamages.Add(newDamage);
                    }
                    // 滑走強化
                    if (affinitySlidingProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * affinitySlidingProb;
                        newDamage.CriticalRate += 30;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 会心旋律
            if (Style.SongofCriticalProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double songofCriticalProb = Style.SongofCriticalProb / 100.0;
                    // 通常
                    if (songofCriticalProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - songofCriticalProb);
                        newDamages.Add(newDamage);
                    }
                    // 会心旋律
                    if (songofCriticalProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * songofCriticalProb;
                        newDamage.CriticalRate += 20;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // アミキリアカネ
            if (Style.CutterflyProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double cutterflyProb = Style.CutterflyProb / 100.0;
                    // 通常
                    if (cutterflyProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - cutterflyProb);
                        newDamages.Add(newDamage);
                    }
                    // アミキリアカネ
                    if (cutterflyProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * cutterflyProb;
                        newDamage.CriticalRate += 50;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // シラヌイカ
            if (Style.LampsquidProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double lampsquidProb = Style.LampsquidProb / 100.0;
                    // 通常
                    if (lampsquidProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - lampsquidProb);
                        newDamages.Add(newDamage);
                    }
                    // シラヌイカ
                    if (lampsquidProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * lampsquidProb;
                        newDamage.CriticalRate += 50;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 強化咆哮
            if (Style.RousingRoarProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double rousingRoarProb = Style.RousingRoarProb / 100.0;
                    // 通常
                    if (rousingRoarProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - rousingRoarProb);
                        newDamages.Add(newDamage);
                    }
                    // 強化咆哮
                    if (rousingRoarProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * rousingRoarProb;
                        newDamage.CriticalRate += 30;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 会心率強化
            if (equip.CritAugLv > 0)
            {
                int level = equip.CritAugLv;
                int add = 0;
                switch (level)
                {
                    case 1:
                        add = 5;
                        break;
                    case 2:
                        add = 10;
                        break;
                    case 3:
                        add = 15;
                        break;
                    case 4:
                        add = 20;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    newDamage.CriticalRate += add;
                }
            }

            // 会心率強化
            if (GetRanpageLv(equip, "会心率強化") > 0)
            {
                int level = GetRanpageLv(equip, "会心率強化");
                int add = 0;
                switch (level)
                {
                    case 1:
                        add = 4;
                        break;
                    case 2:
                        add = 6;
                        break;
                    case 3:
                        add = 8;
                        break;
                    case 4:
                        add = 10;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    newDamage.CriticalRate += add;
                }
            }

            // 会心率激化
            if (HasRanpage(equip, "会心率激化"))
            {
                foreach (var newDamage in newDamages)
                {
                    newDamage.CriticalRate += 20;
                    newDamage.WeaponAttack -= 10;
                }
            }

            // 攻撃力激化
            if (HasRanpage(equip, "攻撃力激化"))
            {
                foreach (var newDamage in newDamages)
                {
                    newDamage.CriticalRate -= 30;
                    newDamage.WeaponAttack += 20;
                }
            }

            // 鋼龍の魂・連撃
            if ((HasRanpage(equip, "鋼龍の魂") || equip.ChainCritLv > 0) &&
                (Style.KushalaDaoraSoulProb > 0 || Style.KushalaDaoraSoul5Prob > 0))
            {
                oldDamages = newDamages;
                newDamages = new();
                bool hasRanpage = HasRanpage(equip, "鋼龍の魂");
                foreach (var oldDamage in oldDamages)
                {
                    double kushalaDaoraSoul5Prob = Style.KushalaDaoraSoul5Prob / 100.0;
                    double kushalaDaoraSoulProb = Math.Max(Style.KushalaDaoraSoulProb / 100.0 - kushalaDaoraSoul5Prob, 0.0);
                    // 通常
                    if (kushalaDaoraSoulProb + kushalaDaoraSoul5Prob < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - kushalaDaoraSoulProb - kushalaDaoraSoul5Prob);
                        newDamages.Add(newDamage);
                    }
                    // 1-4
                    if (kushalaDaoraSoulProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * kushalaDaoraSoulProb;
                        if (hasRanpage)
                        {
                            newDamage.CriticalRate += 25;
                        }
                        switch (equip.ChainCritLv)
                        {
                            case 1:
                                newDamage.AdditionalAttack1 += 5;
                                newDamage.AdditionalElement += 5;
                                break;
                            case 2:
                                newDamage.AdditionalAttack1 += 5;
                                newDamage.AdditionalElement += 5;
                                break;
                            case >= 3:
                                newDamage.AdditionalAttack1 += 5;
                                newDamage.AdditionalElement += 5;
                                break;
                            default:
                                break;
                        }
                        newDamages.Add(newDamage);
                    }
                    // 5-
                    if (kushalaDaoraSoul5Prob > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * kushalaDaoraSoul5Prob;
                        if (hasRanpage)
                        {
                            newDamage.CriticalRate += 30;
                        }
                        switch (equip.ChainCritLv)
                        {
                            case 1:
                                newDamage.AdditionalAttack1 += 10;
                                newDamage.AdditionalElement += 8;
                                break;
                            case 2:
                                newDamage.AdditionalAttack1 += 12;
                                newDamage.AdditionalElement += 10;
                                break;
                            case >= 3:
                                newDamage.AdditionalAttack1 += 15;
                                newDamage.AdditionalElement += 15;
                                break;
                            default:
                                break;
                        }
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 雷神龍の魂
            if (equip.IbushiLv > 0 && HasRanpage(equip, "雷神龍の魂"))
            {
                foreach (var newDamage in newDamages)
                {
                    switch (equip.IbushiLv)
                    {
                        case 1:
                            newDamage.CriticalRate += 4;
                            break;
                        case 2:
                            newDamage.CriticalRate += 6;
                            break;
                        case 3:
                            newDamage.CriticalRate += 10;
                            break;
                        case 4:
                            newDamage.CriticalRate += 12;
                            break;
                        case >= 5:
                            newDamage.CriticalRate += 40;
                            break;
                        default:
                            break;
                    }
                }
            }

            // 狂竜症
            if (Style.FrenzyInfectProb > 0 || Style.FrenzyTreatProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double frenzyTreatProb = Style.FrenzyTreatProb / 100.0;
                    double frenzyInfectProb = 0;
                    if (equip.BloodlustLv > 0)
                    {
                        frenzyInfectProb = Math.Max(Style.FrenzyInfectProb / 100.0 - frenzyTreatProb, 0.0);
                    }

                    // 通常
                    if (frenzyTreatProb + frenzyInfectProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - frenzyTreatProb - frenzyInfectProb);
                        newDamages.Add(newDamage);
                    }
                    // 感染状態(狂竜症【蝕】のみ)
                    if (frenzyInfectProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * frenzyInfectProb;
                        switch (equip.BloodlustLv)
                        {
                            case 1:
                                newDamage.AdditionalAttack1 += 10;
                                newDamage.AdditionalElement += 5;
                                break;
                            case 2:
                                newDamage.AdditionalAttack1 += 15;
                                newDamage.AdditionalElement += 7;
                                break;
                            case >= 3:
                                newDamage.AdditionalAttack1 += 20;
                                newDamage.AdditionalElement += 10;
                                break;
                            default:
                                break;
                        }
                        newDamages.Add(newDamage);
                    }
                    // 克服状態
                    if (frenzyTreatProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * frenzyTreatProb;
                        switch (equip.BloodlustLv)
                        {
                            case 0:
                                newDamage.CriticalRate += 15;
                                break;
                            case 1:
                                newDamage.CriticalRate += 20;
                                break;
                            case 2:
                                newDamage.CriticalRate += 25;
                                break;
                            case >= 3:
                                newDamage.CriticalRate += 25;
                                break;
                            default:
                                break;
                        }
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 攻勢
            if (equip.ForayLv > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double blightProb = 0;
                    blightProb += Style.FireBlightProb / 100.0;
                    blightProb += Style.WaterBlightProb / 100.0;
                    blightProb += Style.IceBlightProb / 100.0;
                    blightProb += Style.ThunderBlightProb / 100.0;
                    blightProb = Math.Min(blightProb, 1.0);
                    double poisonProb = Style.PoisonProb / 100.0;
                    double paralysisProb = Style.ParalysisProb / 100.0;

                    double forayProb = 1.0 - (1.0 - blightProb) * (1.0 - poisonProb) * (1.0 - paralysisProb);

                    // 通常
                    if (forayProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - forayProb);
                        newDamages.Add(newDamage);
                    }
                    // 攻勢
                    if (forayProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * forayProb;
                        switch (equip.ForayLv)
                        {
                            case 1:
                                newDamage.AdditionalAttack1 += 10;
                                break;
                            case 2:
                                newDamage.AdditionalAttack1 += 10;
                                newDamage.CriticalRate += 10;
                                break;
                            case >= 3:
                                newDamage.AdditionalAttack1 += 15;
                                newDamage.CriticalRate += 20;
                                break;
                            default:
                                break;
                        }
                        newDamages.Add(newDamage);
                    }
                }
            }

            return newDamages;
        }

        /// <summary>
        /// 切れ味関連の計算
        /// </summary>
        /// <param name="damages"></param>
        /// <param name="equip"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private List<DamagePattern> CalcSharpness(List<DamagePattern> damages, Equipment equip, double expectedCriticalRate)
        {
            List<DamagePattern> oldDamages;
            List<DamagePattern> newDamages = damages;
            double count = Style.AttackCount;

            // 匠
            double handicraftAdditional = 0.0;
            if (equip.HandicraftLv > 0)
            {
                handicraftAdditional = equip.HandicraftLv * 10.0;
            }

            // 斬れ味錬成
            double augAdditional = 0.0;
            if (equip.SharpAugLv > 0)
            {
                augAdditional = equip.SharpAugLv * 10.0;
            }

            // 業物
            if (equip.RazorSharpLv > 0)
            {
                switch (equip.RazorSharpLv)
                {
                    case 1:
                        count *= 1 - 0.10;
                        break;
                    case 2:
                        count *= 1 - 0.25;
                        break;
                    case >= 3:
                        count *= 1 - 0.50;
                        break;
                    default:
                        break;
                }
            }

            // 達人芸
            if (equip.MastersTouchLv > 0)
            {
                switch (equip.MastersTouchLv)
                {
                    case 1:
                        count *= 1 - 0.20 * expectedCriticalRate / 100.0;
                        break;
                    case 2:
                        count *= 1 - 0.40 * expectedCriticalRate / 100.0;
                        break;
                    case >= 3:
                        count *= 1 - 0.80 * expectedCriticalRate / 100.0;
                        break;
                    default:
                        break;
                }
            }
            
            // 斬れ味変更
            var weaponSharpness = equip.Weapon.Sharpness;
            if (HasRanpage(equip, "斬れ味変更【壱型】"))
            {
                weaponSharpness = new();
                weaponSharpness.SetSharpness(new List<double>() { 100, 150, 50, 20, 30, 50 });
                weaponSharpness.Max = 350;
            }
            else if (HasRanpage(equip, "斬れ味変更【弐型】"))
            {
                weaponSharpness = new();
                weaponSharpness.SetSharpness(new List<double>() { 20, 80, 150, 100, 50 });
                weaponSharpness.Max = 350;
            }
            else if (HasRanpage(equip, "斬れ味変更【参型】"))
            {
                weaponSharpness = new();
                weaponSharpness.SetSharpness(new List<double>() { 70, 70, 30, 30, 150 });
                weaponSharpness.Max = 300;
                foreach (var newDamage in newDamages)
                {
                    newDamage.WeaponAttack -= 10;
                }
            }
            else if (HasRanpage(equip, "斬れ味変更【肆型】"))
            {
                weaponSharpness = new();
                weaponSharpness.SetSharpness(new List<double>() { 50, 80, 70, 160, 10, 30 });
                weaponSharpness.Max = 400;
                foreach (var newDamage in newDamages)
                {
                    newDamage.WeaponAttack -= 20;
                }
            }

            var useSharpness = weaponSharpness.GetUseSharpness(count, handicraftAdditional, augAdditional);

            // 切れ味計算&鈍器使い
            oldDamages = newDamages;
            newDamages = new();
            foreach (var oldDamage in oldDamages)
            {
                if (useSharpness.Red > 0)
                {
                    DamagePattern newDamage = oldDamage.Clone();
                    newDamage.Probability = oldDamage.Probability * useSharpness.Red / count;
                    newDamage.PhysicsSharpnessModifier *= 0.5;
                    newDamage.ElementSharpnessModifier *= 0.25;
                    // 鈍器使い
                    if (equip.BludgeonerLv == 1)
                    {
                        newDamage.PhysicsOtherModifier *= 1.05;
                    }
                    else if (equip.BludgeonerLv >= 2)
                    {
                        newDamage.PhysicsOtherModifier *= 1.1;
                    }
                    newDamages.Add(newDamage);
                    // 鈍刃の一撃
                    if (HasRanpage(equip, "鈍刃の一撃"))
                    {
                        DamagePattern dullingDamage = newDamage.Clone();
                        newDamage.Probability *= 0.9;
                        dullingDamage.Probability *= 0.1;
                        dullingDamage.PhysicsOtherModifier *= 1.2;
                        newDamages.Add(dullingDamage);
                    }
                }
                if (useSharpness.Orange > 0)
                {
                    DamagePattern newDamage = oldDamage.Clone();
                    newDamage.Probability = oldDamage.Probability * useSharpness.Orange / count;
                    newDamage.PhysicsSharpnessModifier *= 0.75;
                    newDamage.ElementSharpnessModifier *= 0.50;
                    // 鈍器使い
                    if (equip.BludgeonerLv == 1)
                    {
                        newDamage.PhysicsOtherModifier *= 1.05;
                    }
                    else if (equip.BludgeonerLv >= 2)
                    {
                        newDamage.PhysicsOtherModifier *= 1.1;
                    }
                    newDamages.Add(newDamage);
                    // 鈍刃の一撃
                    if (HasRanpage(equip, "鈍刃の一撃"))
                    {
                        DamagePattern dullingDamage = newDamage.Clone();
                        newDamage.Probability *= 0.9;
                        dullingDamage.Probability *= 0.1;
                        dullingDamage.PhysicsOtherModifier *= 1.2;
                        newDamages.Add(dullingDamage);
                    }
                }
                if (useSharpness.Yellow > 0)
                {
                    DamagePattern newDamage = oldDamage.Clone();
                    newDamage.Probability = oldDamage.Probability * useSharpness.Yellow / count;
                    newDamage.PhysicsSharpnessModifier *= 1.0;
                    newDamage.ElementSharpnessModifier *= 0.75;
                    // 鈍器使い
                    if (equip.BludgeonerLv == 1)
                    {
                        newDamage.PhysicsOtherModifier *= 1.05;
                    }
                    else if (equip.BludgeonerLv >= 2)
                    {
                        newDamage.PhysicsOtherModifier *= 1.1;
                    }
                    newDamages.Add(newDamage);
                    // 鈍刃の一撃
                    if (HasRanpage(equip, "鈍刃の一撃"))
                    {
                        DamagePattern dullingDamage = newDamage.Clone();
                        newDamage.Probability *= 0.9;
                        dullingDamage.Probability *= 0.1;
                        dullingDamage.PhysicsOtherModifier *= 1.2;
                        newDamages.Add(dullingDamage);
                    }
                }
                if (useSharpness.Green > 0)
                {
                    DamagePattern newDamage = oldDamage.Clone();
                    newDamage.Probability = oldDamage.Probability * useSharpness.Green / count;
                    newDamage.PhysicsSharpnessModifier *= 1.05;
                    newDamage.ElementSharpnessModifier *= 1.00;
                    // 鈍器使い
                    if (equip.BludgeonerLv >= 3)
                    {
                        newDamage.PhysicsOtherModifier *= 1.1;
                    }
                    newDamages.Add(newDamage);
                    // 鈍刃の一撃
                    if (HasRanpage(equip, "鈍刃の一撃"))
                    {
                        DamagePattern dullingDamage = newDamage.Clone();
                        newDamage.Probability *= 0.9;
                        dullingDamage.Probability *= 0.1;
                        dullingDamage.PhysicsOtherModifier *= 1.2;
                        newDamages.Add(dullingDamage);
                    }
                }
                if (useSharpness.Blue > 0)
                {
                    DamagePattern newDamage = oldDamage.Clone();
                    newDamage.Probability = oldDamage.Probability * useSharpness.Blue / count;
                    newDamage.PhysicsSharpnessModifier *= 1.2;
                    newDamage.ElementSharpnessModifier *= 1.0625;
                    newDamages.Add(newDamage);
                }
                if (useSharpness.White > 0)
                {
                    DamagePattern newDamage = oldDamage.Clone();
                    newDamage.Probability = oldDamage.Probability * useSharpness.White / count;
                    newDamage.PhysicsSharpnessModifier *= 1.32;
                    newDamage.ElementSharpnessModifier *= 1.15;
                    newDamages.Add(newDamage);
                }
                if (useSharpness.Purple > 0)
                {
                    DamagePattern newDamage = oldDamage.Clone();
                    newDamage.Probability = oldDamage.Probability * useSharpness.Purple / count;
                    newDamage.PhysicsSharpnessModifier *= 1.39;
                    newDamage.ElementSharpnessModifier *= 1.25;
                    newDamages.Add(newDamage);
                }
            }

            // 研磨術【鋭】
            if (equip.GrinderLv > 0 && Style.GrinderProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double grinderProb = Style.GrinderProb / 100.0;
                    // 通常
                    if (grinderProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - grinderProb);
                        newDamages.Add(newDamage);
                    }
                    // 研磨術【鋭】
                    if (grinderProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * grinderProb;
                        newDamage.PhysicsSharpnessModifier *= 1.1;
                        newDamage.ElementSharpnessModifier *= 1.07;
                        newDamages.Add(newDamage);
                    }
                }
            }

            return newDamages;
        }

        /// <summary>
        /// その他の補正
        /// </summary>
        /// <param name="damages"></param>
        /// <param name="equip"></param>
        /// <returns></returns>
        private List<DamagePattern> CalcOthers(List<DamagePattern> damages, Equipment equip)
        {
            List<DamagePattern> oldDamages;
            List<DamagePattern> newDamages = damages;

            // 攻撃
            if (equip.AttackBoostLv > 0)
            {
                foreach (var newDamage in newDamages)
                {
                    switch (equip.AttackBoostLv)
                    {
                        case 1:
                            newDamage.AdditionalAttack1 += 3;
                            break;
                        case 2:
                            newDamage.AdditionalAttack1 += 6;
                            break;
                        case 3:
                            newDamage.AdditionalAttack1 += 9;
                            break;
                        case 4:
                            newDamage.AdditionalAttack1 += 7;
                            newDamage.AttackModifier1 *= 1.05;
                            break;
                        case 5:
                            newDamage.AdditionalAttack1 += 8;
                            newDamage.AttackModifier1 *= 1.06;
                            break;
                        case 6:
                            newDamage.AdditionalAttack1 += 9;
                            newDamage.AttackModifier1 *= 1.08;
                            break;
                        case 7:
                            newDamage.AdditionalAttack1 += 10;
                            newDamage.AttackModifier1 *= 1.10;
                            break;
                        default:
                            break;
                    }
                }
            }

            // フルチャージ
            if (equip.PeakPerformanceLv > 0 && Style.PeakPerformanceProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double peakPerformanceProb = Style.PeakPerformanceProb / 100.0;
                    // 通常
                    if (peakPerformanceProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - peakPerformanceProb);
                        newDamages.Add(newDamage);
                    }
                    // フルチャ
                    if (peakPerformanceProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * peakPerformanceProb;
                        switch (equip.PeakPerformanceLv)
                        {
                            case 1:
                                newDamage.AdditionalAttack1 += 5;
                                break;
                            case 2:
                                newDamage.AdditionalAttack1 += 10;
                                break;
                            case >= 3:
                                newDamage.AdditionalAttack1 += 20;
                                break;
                            default:
                                break;
                        }

                        newDamages.Add(newDamage);
                    }
                }
            }

            // 攻めの守勢
            if (equip.OffensiveGuardLv > 0 && Style.OffensiveGuardProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double offensiveGuardProb = Style.OffensiveGuardProb / 100.0;
                    // 通常
                    if (offensiveGuardProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - offensiveGuardProb);
                        newDamages.Add(newDamage);
                    }
                    // 攻めの守勢
                    if (offensiveGuardProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * offensiveGuardProb;
                        switch (equip.OffensiveGuardLv)
                        {
                            case 1:
                                newDamage.AttackModifier1 *= 1.05;
                                break;
                            case 2:
                                newDamage.AttackModifier1 *= 1.10;
                                break;
                            case >= 3:
                                newDamage.AttackModifier1 *= 1.15;
                                break;
                            default:
                                break;
                        }

                        newDamages.Add(newDamage);
                    }
                }
            }

            // 龍気活性&天彗龍の魂
            if (equip.DragonheartLv > 0 && Style.DragonheartProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double dragonheartProb = Style.DragonheartProb / 100.0;
                    // 通常
                    if (dragonheartProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - dragonheartProb);
                        newDamages.Add(newDamage);
                    }
                    // 龍気活性
                    if (dragonheartProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * dragonheartProb;
                        if (newDamage.ElementKind != Element.Dragon)
                        {
                            newDamage.ElementOtherModifier *= 0.0;
                        }
                        switch (equip.DragonheartLv)
                        {
                            case 4:
                                newDamage.AttackModifier1 *= 1.05;
                                break;
                            case >= 5:
                                newDamage.AttackModifier1 *= 1.10;
                                break;
                            default:
                                break;
                        }
                        // 天彗龍の魂
                        if (HasRanpage(equip, "天彗龍の魂"))
                        {
                            newDamage.ElementModifier1 *= 1.2;
                        }

                        newDamages.Add(newDamage);
                    }
                }
            }

            // 逆襲
            if (equip.CounterstrikeLv > 0 && Style.CounterstrikeProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double counterstrikeProb = Style.CounterstrikeProb / 100.0;
                    // 通常
                    if (counterstrikeProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - counterstrikeProb);
                        newDamages.Add(newDamage);
                    }
                    // 逆襲
                    if (counterstrikeProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * counterstrikeProb;
                        switch (equip.CounterstrikeLv)
                        {
                            case 1:
                                newDamage.AdditionalAttack1 += 10;
                                break;
                            case 2:
                                newDamage.AdditionalAttack1 += 15;
                                break;
                            case >= 3:
                                newDamage.AdditionalAttack1 += 25;
                                break;
                            default:
                                break;
                        }

                        newDamages.Add(newDamage);
                    }
                }
            }

            // 逆恨み
            if (equip.ResentmentLv > 0 && Style.ResentmentProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double resentmentProb = Style.ResentmentProb / 100.0;
                    // 通常
                    if (resentmentProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - resentmentProb);
                        newDamages.Add(newDamage);
                    }
                    // 逆恨み
                    if (resentmentProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * resentmentProb;
                        switch (equip.ResentmentLv)
                        {
                            case 1:
                                newDamage.AdditionalAttack1 += 5;
                                break;
                            case 2:
                                newDamage.AdditionalAttack1 += 10;
                                break;
                            case 3:
                                newDamage.AdditionalAttack1 += 15;
                                break;
                            case 4:
                                newDamage.AdditionalAttack1 += 20;
                                break;
                            case >= 5:
                                newDamage.AdditionalAttack1 += 25;
                                break;
                            default:
                                break;
                        }

                        newDamages.Add(newDamage);
                    }
                }
            }

            // 逆襲
            if (equip.ResuscitateLv > 0 && Style.ResuscitateProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double resuscitateProb = Style.ResuscitateProb / 100.0;
                    // 通常
                    if (resuscitateProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - resuscitateProb);
                        newDamages.Add(newDamage);
                    }
                    // 逆襲
                    if (resuscitateProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * resuscitateProb;
                        switch (equip.ResuscitateLv)
                        {
                            case 1:
                                newDamage.AdditionalAttack1 += 5;
                                break;
                            case 2:
                                newDamage.AdditionalAttack1 += 10;
                                break;
                            case >= 3:
                                newDamage.AdditionalAttack1 += 20;
                                break;
                            default:
                                break;
                        }

                        newDamages.Add(newDamage);
                    }
                }
            }

            // 火事場力&ネコ火事
            if ((equip.HeroicsLv > 0 && Style.HeroicsProb > 0) ||
                Style.DangoAdrenalineProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double dangoAdrenalineProb = Style.DangoAdrenalineProb / 100.0;
                    double heroicsProb = Math.Max(Style.HeroicsProb / 100.0 - dangoAdrenalineProb, 0.0);
                    // 通常
                    if (heroicsProb + dangoAdrenalineProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - heroicsProb - dangoAdrenalineProb);
                        newDamages.Add(newDamage);
                    }
                    // 火事場力
                    if (heroicsProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * heroicsProb;
                        switch (equip.HeroicsLv)
                        {
                            case 2:
                                newDamage.AttackModifier1 *= 1.05;
                                break;
                            case 3:
                                newDamage.AttackModifier1 *= 1.05;
                                break;
                            case 4:
                                newDamage.AttackModifier1 *= 1.10;
                                break;
                            case >= 5:
                                newDamage.AttackModifier1 *= 1.30;
                                break;
                            default:
                                break;
                        }

                        newDamages.Add(newDamage);
                    }
                    // ネコ火事
                    if (dangoAdrenalineProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * dangoAdrenalineProb;
                        newDamage.AttackModifier1 *= 1.35;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 壁面移動
            if (equip.WallRunnerLv >= 3 && Style.WallRunnerProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double wallRunnerProb = Style.WallRunnerProb / 100.0;
                    // 通常
                    if (wallRunnerProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - wallRunnerProb);
                        newDamages.Add(newDamage);
                    }
                    // 壁面移動
                    if (wallRunnerProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * wallRunnerProb;
                        newDamage.AdditionalAttack1 += 20;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 不屈
            if (equip.FortifyLv > 0 && 
                (Style.FortifyProb > 0 || Style.Fortify2Prob > 0))
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double fortify2Prob = Style.Fortify2Prob / 100.0;
                    double fortify1Prob = Math.Max(Style.FortifyProb / 100.0 - fortify2Prob, 0.0);
                    // 通常
                    if (fortify1Prob + fortify2Prob < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - fortify1Prob - fortify2Prob);
                        newDamages.Add(newDamage);
                    }
                    // 不屈1
                    if (fortify1Prob > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * fortify1Prob;
                        newDamage.AttackModifier1 *= 1.10;
                        newDamages.Add(newDamage);
                    }
                    // 不屈2
                    if (fortify2Prob > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * fortify2Prob;
                        newDamage.AttackModifier1 *= 1.20;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 超会心
            if (equip.CriticalBoostLv > 0)
            {
                double modifier = 1.25;
                switch (equip.CriticalBoostLv)
                {
                    case 1:
                        modifier = 1.30;
                        break;
                    case 2:
                        modifier = 1.35;
                        break;
                    case >= 3:
                        modifier = 1.40;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    newDamage.PhysicsCriticalModifier = modifier;
                }
            }

            // 火属性攻撃強化
            if (equip.FireAttackLv > 0)
            {
                int add = 0;
                double modifier = 1.00;
                switch (equip.FireAttackLv)
                {
                    case 1:
                        add = 2;
                        break;
                    case 2:
                        add = 3;
                        break;
                    case 3:
                        add = 4;
                        modifier = 1.05;
                        break;
                    case 4:
                        add = 4;
                        modifier = 1.10;
                        break;
                    case >= 5:
                        add = 4;
                        modifier = 1.20;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Fire)
                    {
                        newDamage.AdditionalElement += add;
                        newDamage.ElementModifier1 *= modifier;
                    }
                }
            }

            // 水属性攻撃強化
            if (equip.WaterAttackLv > 0)
            {
                int add = 0;
                double modifier = 1.00;
                switch (equip.WaterAttackLv)
                {
                    case 1:
                        add = 2;
                        break;
                    case 2:
                        add = 3;
                        break;
                    case 3:
                        add = 4;
                        modifier = 1.05;
                        break;
                    case 4:
                        add = 4;
                        modifier = 1.10;
                        break;
                    case >= 5:
                        add = 4;
                        modifier = 1.20;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Water)
                    {
                        newDamage.AdditionalElement += add;
                        newDamage.ElementModifier1 *= modifier;
                    }
                }
            }

            // 雷属性攻撃強化
            if (equip.ThunderAttackLv > 0)
            {
                int add = 0;
                double modifier = 1.00;
                switch (equip.ThunderAttackLv)
                {
                    case 1:
                        add = 2;
                        break;
                    case 2:
                        add = 3;
                        break;
                    case 3:
                        add = 4;
                        modifier = 1.05;
                        break;
                    case 4:
                        add = 4;
                        modifier = 1.10;
                        break;
                    case >= 5:
                        add = 4;
                        modifier = 1.20;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Thunder)
                    {
                        newDamage.AdditionalElement += add;
                        newDamage.ElementModifier1 *= modifier;
                    }
                }
            }

            // 氷属性攻撃強化
            if (equip.IceAttackLv > 0)
            {
                int add = 0;
                double modifier = 1.00;
                switch (equip.IceAttackLv)
                {
                    case 1:
                        add = 2;
                        break;
                    case 2:
                        add = 3;
                        break;
                    case 3:
                        add = 4;
                        modifier = 1.05;
                        break;
                    case 4:
                        add = 4;
                        modifier = 1.10;
                        break;
                    case >= 5:
                        add = 4;
                        modifier = 1.20;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Ice)
                    {
                        newDamage.AdditionalElement += add;
                        newDamage.ElementModifier1 *= modifier;
                    }
                }
            }

            // 龍属性攻撃強化
            if (equip.DragonAttackLv > 0)
            {
                int add = 0;
                double modifier = 1.00;
                switch (equip.DragonAttackLv)
                {
                    case 1:
                        add = 2;
                        break;
                    case 2:
                        add = 3;
                        break;
                    case 3:
                        add = 4;
                        modifier = 1.05;
                        break;
                    case 4:
                        add = 4;
                        modifier = 1.10;
                        break;
                    case >= 5:
                        add = 4;
                        modifier = 1.20;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Dragon)
                    {
                        newDamage.AdditionalElement += add;
                        newDamage.ElementModifier1 *= modifier;
                    }
                }
            }

            // 風雷合一
            if (equip.StormsoulLv > 0)
            {
                double modifier = 1.00;
                switch (equip.StormsoulLv)
                {
                    case 1:
                        modifier = 1.05;
                        break;
                    case 2:
                        modifier = 1.10;
                        break;
                    case >= 3:
                        modifier = 1.15;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Thunder || newDamage.ElementKind == Element.Dragon)
                    {
                        newDamage.ElementModifier1 *= modifier;
                    }
                }
            }

            // 会心撃【属性】
            if (equip.CriticalElementLv > 0)
            {
                double modifier = 1.00;
                switch (equip.CriticalElementLv)
                {
                    case 1:
                        modifier = 1.05;
                        break;
                    case 2:
                        modifier = 1.10;
                        break;
                    case >= 3:
                        modifier = 1.15;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    newDamage.ElementCriticalModifier *= modifier;
                }
            }

            // 炎鱗の恩恵
            if (equip.TeostraBlessingLv > 0)
            {
                double modifier = 1.00;
                switch (equip.TeostraBlessingLv)
                {
                    case 1:
                        modifier = 1.05;
                        break;
                    case >= 2:
                        modifier = 1.10;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Fire)
                    {
                        newDamage.ElementModifier1 *= modifier;
                    }
                }
            }

            // 鋼殻の恩恵
            if (equip.KushalaBlessingLv > 0)
            {
                double modifier = 1.00;
                switch (equip.KushalaBlessingLv)
                {
                    case 1:
                        modifier = 1.05;
                        break;
                    case >= 2:
                        modifier = 1.10;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Water || newDamage.ElementKind == Element.Ice)
                    {
                        newDamage.ElementModifier1 *= modifier;
                    }
                }
            }

            // 心眼
            if (equip.MindsEyeLv == 1)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    if (oldDamage.PhysicsSharpnessModifier * oldDamage.PhysicsPhysiology < 45)
                    {
                        DamagePattern normalDamage = oldDamage.Clone();
                        normalDamage.Probability = oldDamage.Probability / 2.0;
                        newDamages.Add(normalDamage);

                        DamagePattern mindsDamage = oldDamage.Clone();
                        mindsDamage.Probability = oldDamage.Probability / 2.0;
                        mindsDamage.PhysicsOtherModifier *= 1.10;
                        newDamages.Add(mindsDamage);
                    }
                    else
                    {
                        newDamages.Add(oldDamage);
                    }
                }
            }
            else if (equip.MindsEyeLv > 1)
            {
                double modifier = 1.0;
                switch (equip.MindsEyeLv)
                {
                    case 2:
                        modifier = 1.15;
                        break;
                    case >= 3:
                        modifier = 1.30;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    if (newDamage.PhysicsSharpnessModifier * newDamage.PhysicsPhysiology < 45)
                    {
                        newDamage.PhysicsOtherModifier *= modifier;
                    }
                }
            }

            // 気炎の旋律
            if (Style.SongofRagingFlameProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double songofRagingFlameProb = Style.SongofRagingFlameProb / 100.0;
                    // 通常
                    if (songofRagingFlameProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - songofRagingFlameProb);
                        newDamages.Add(newDamage);
                    }
                    // 気炎の旋律
                    if (songofRagingFlameProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * songofRagingFlameProb;
                        newDamage.AttackModifier2 *= 1.2;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 攻撃旋律
            if (Style.SongofAttackProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double songofAttackProb = Style.SongofAttackProb / 100.0;
                    // 通常
                    if (songofAttackProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - songofAttackProb);
                        newDamages.Add(newDamage);
                    }
                    // 攻撃旋律
                    if (songofAttackProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * songofAttackProb;
                        newDamage.AttackModifier2 *= 1.1;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 属性旋律
            if (Style.SongofElementProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double songofElementProb = Style.SongofElementProb / 100.0;
                    // 通常
                    if (songofElementProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - songofElementProb);
                        newDamages.Add(newDamage);
                    }
                    // 属性旋律
                    if (songofElementProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * songofElementProb;
                        newDamage.ElementModifier2 *= 1.1;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 力の護符
            if (Style.PowercharmProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double powercharmProb = Style.PowercharmProb / 100.0;
                    // 通常
                    if (powercharmProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - powercharmProb);
                        newDamages.Add(newDamage);
                    }
                    // 力の護符
                    if (powercharmProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * powercharmProb;
                        newDamage.AdditionalAttack1 += 6;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 力の爪
            if (Style.PowertalonProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double powertalonProb = Style.PowertalonProb / 100.0;
                    // 通常
                    if (powertalonProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - powertalonProb);
                        newDamages.Add(newDamage);
                    }
                    // 力の爪
                    if (powertalonProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * powertalonProb;
                        newDamage.AdditionalAttack1 += 9;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 鬼人薬&鬼人薬G
            if (Style.DemondrugProb > 0 || Style.DemondrugGProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double demondrugGProb = Style.DemondrugGProb / 100.0;
                    double demondrugProb = Math.Max(Style.DemondrugProb / 100.0 - demondrugGProb, 0.0);
                    // 通常
                    if (demondrugProb + demondrugGProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - demondrugProb - demondrugGProb);
                        newDamages.Add(newDamage);
                    }
                    // 鬼人薬
                    if (demondrugProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * demondrugProb;
                        newDamage.AdditionalAttack1 += 5;
                        newDamages.Add(newDamage);
                    }
                    // 鬼人薬G
                    if (demondrugGProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * demondrugGProb;
                        newDamage.AdditionalAttack1 += 7;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 怪力の種・ビルドアップ・ホムラチョウ
            if (Style.MightSeedProb > 0 ||
                Style.BuildUpProb > 0 ||
                Style.ButterflameProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double mightSeedProb = Style.MightSeedProb / 100.0;
                    double butterflameProb = Style.ButterflameProb * (1 - mightSeedProb) / 100.0;
                    double buildUpProb = Style.BuildUpProb * (1 - mightSeedProb - butterflameProb) / 100.0;
                    // 通常
                    if (mightSeedProb + butterflameProb + buildUpProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - mightSeedProb - butterflameProb - buildUpProb);
                        newDamages.Add(newDamage);
                    }
                    // 怪力の種
                    if (mightSeedProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * mightSeedProb;
                        newDamage.AdditionalAttack1 += 10;
                        newDamages.Add(newDamage);
                    }
                    // ホムラチョウ
                    if (butterflameProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * butterflameProb;
                        newDamage.AdditionalAttack1 += 25;
                        newDamages.Add(newDamage);
                    }
                    // ビルドアップ
                    if (buildUpProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * buildUpProb;
                        newDamage.AdditionalAttack1 += 15;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 鬼人の粉塵
            if (Style.DemonPowderProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double demonPowderProb = Style.DemonPowderProb / 100.0;
                    // 通常
                    if (demonPowderProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - demonPowderProb);
                        newDamages.Add(newDamage);
                    }
                    // 鬼人の粉塵
                    if (demonPowderProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * demonPowderProb;
                        newDamage.AdditionalAttack1 += 10;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 鬼人弾
            if (Style.DemonAmmoProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double demonAmmoProb = Style.DemonAmmoProb / 100.0;
                    // 通常
                    if (demonAmmoProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - demonAmmoProb);
                        newDamages.Add(newDamage);
                    }
                    // 鬼人弾
                    if (demonAmmoProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * demonAmmoProb;
                        newDamage.AdditionalAttack1 += 10;
                        newDamage.PhysicsSharpnessModifier *= 1.1;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 短期催眠術
            if (Style.FelyneBoosterProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double felyneBoosterProb = Style.FelyneBoosterProb / 100.0;
                    // 通常
                    if (felyneBoosterProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - felyneBoosterProb);
                        newDamages.Add(newDamage);
                    }
                    // 短期催眠術
                    if (felyneBoosterProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * felyneBoosterProb;
                        newDamage.AdditionalAttack1 += 9;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 強化太鼓
            if (Style.PowerDrumProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double powerDrumProb = Style.PowerDrumProb / 100.0;
                    // 通常
                    if (powerDrumProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - powerDrumProb);
                        newDamages.Add(newDamage);
                    }
                    // 強化太鼓
                    if (powerDrumProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * powerDrumProb;
                        newDamage.AttackModifier1 *= 1.05;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 強化納刀
            if (Style.PowerSheatheProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double powerSheatheProb = Style.PowerSheatheProb / 100.0;
                    // 通常
                    if (powerSheatheProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - powerSheatheProb);
                        newDamages.Add(newDamage);
                    }
                    // 強化納刀
                    if (powerSheatheProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * powerSheatheProb;
                        newDamage.AttackModifier1 *= 1.10;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 紅ヒトダマドリ
            if (Style.Spiribirds > 0)
            {
                foreach (var newDamage in newDamages)
                {
                    newDamage.AdditionalAttack1 += Style.Spiribirds;
                }
            }

            // 怨虎竜の魂
            if (Style.MagnamaloSoulProb > 0 && HasRanpage(equip, "怨虎竜の魂"))
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double magnamaloSoulProb = Style.MagnamaloSoulProb / 100.0;
                    // 通常
                    if (magnamaloSoulProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - magnamaloSoulProb);
                        newDamages.Add(newDamage);
                    }
                    // 怨虎竜の魂
                    if (magnamaloSoulProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * magnamaloSoulProb;
                        newDamage.AdditionalAttack1 += 12;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 火やられ特効
            if (Style.FireBlightProb > 0 && HasRanpage(equip, "火やられ特効"))
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double fireBlightProb = Style.FireBlightProb / 100.0;
                    // 通常
                    if (fireBlightProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - fireBlightProb);
                        newDamages.Add(newDamage);
                    }
                    // 火やられ特効
                    if (fireBlightProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * fireBlightProb;
                        newDamage.PhysicsOtherModifier *= 1.1;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 雷やられ特効
            if (Style.ThunderBlightProb > 0 && HasRanpage(equip, "雷やられ特効"))
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double thunderBlightProb = Style.ThunderBlightProb / 100.0;
                    // 通常
                    if (thunderBlightProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - thunderBlightProb);
                        newDamages.Add(newDamage);
                    }
                    // 雷やられ特効
                    if (thunderBlightProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * thunderBlightProb;
                        newDamage.PhysicsOtherModifier *= 1.1;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 氷やられ特効
            if (Style.IceBlightProb > 0 && HasRanpage(equip, "氷やられ特効"))
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double iceBlightProb = Style.IceBlightProb / 100.0;
                    // 通常
                    if (iceBlightProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - iceBlightProb);
                        newDamages.Add(newDamage);
                    }
                    // 氷やられ特効
                    if (iceBlightProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * iceBlightProb;
                        newDamage.PhysicsOtherModifier *= 1.1;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 霞龍の魂
            if (Style.ChameleosSoulProb > 0 && HasRanpage(equip, "霞龍の魂"))
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double chameleosSoulProb = Style.ChameleosSoulProb / 100.0;
                    // 通常
                    if (chameleosSoulProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - chameleosSoulProb);
                        newDamages.Add(newDamage);
                    }
                    // 霞龍の魂
                    if (chameleosSoulProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * chameleosSoulProb;
                        newDamage.AdditionalAttack1 += 15;
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 攻撃錬成
            if (equip.AttackAugLv > 0)
            {
                int level = equip.AttackAugLv;
                int add = 0;
                switch (level)
                {
                    case 1:
                        add = 5;
                        break;
                    case 2:
                        add = 10;
                        break;
                    case 3:
                        add = 15;
                        break;
                    case 4:
                        add = 20;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    newDamage.WeaponAttack += add;
                }
            }

            // 攻撃力強化
            if (GetRanpageLv(equip, "攻撃力強化") > 0)
            {
                int level = GetRanpageLv(equip, "攻撃力強化");
                int add = 0;
                switch (level)
                {
                    case 1:
                        add = 4;
                        break;
                    case 2:
                        add = 6;
                        break;
                    case 3:
                        add = 8;
                        break;
                    case 4:
                        add = 10;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    newDamage.WeaponAttack += add;
                }
            }

            // 弱点特効【属性】
            if (HasRanpage(equip, "弱点特効【属性】"))
            {
                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementPhysiology >= 25)
                    {
                        newDamage.ElementOtherModifier *= 1.30;
                    }
                }
            }

            // 属性錬成
            if (equip.ElementAugLv > 0)
            {
                int level = equip.ElementAugLv;
                int add = 0;
                switch (level)
                {
                    case 1:
                        add = 5;
                        break;
                    case 2:
                        add = 10;
                        break;
                    case 3:
                        add = 15;
                        break;
                    case 4:
                        add = 20;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind != Element.None)
                    {
                        newDamage.ElementValue += add;
                    }
                }
            }

            // 属性強化【火】
            if (GetRanpageLv(equip, "属性強化【火】") > 0)
            {
                int level = GetRanpageLv(equip, "属性強化【火】");
                int add = 0;
                switch (level)
                {
                    case 1:
                        add = 4;
                        break;
                    case 2:
                        add = 6;
                        break;
                    case 3:
                        add = 8;
                        break;
                    case 4:
                        add = 10;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Fire)
                    {
                        newDamage.ElementValue += add;
                    }
                }
            }

            // 属性強化【水】
            if (GetRanpageLv(equip, "属性強化【水】") > 0)
            {
                int level = GetRanpageLv(equip, "属性強化【水】");
                int add = 0;
                switch (level)
                {
                    case 1:
                        add = 4;
                        break;
                    case 2:
                        add = 6;
                        break;
                    case 3:
                        add = 8;
                        break;
                    case 4:
                        add = 10;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Water)
                    {
                        newDamage.ElementValue += add;
                    }
                }
            }

            // 属性強化【雷】
            if (GetRanpageLv(equip, "属性強化【雷】") > 0)
            {
                int level = GetRanpageLv(equip, "属性強化【雷】");
                int add = 0;
                switch (level)
                {
                    case 1:
                        add = 4;
                        break;
                    case 2:
                        add = 6;
                        break;
                    case 3:
                        add = 8;
                        break;
                    case 4:
                        add = 10;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Thunder)
                    {
                        newDamage.ElementValue += add;
                    }
                }
            }

            // 属性強化【氷】
            if (GetRanpageLv(equip, "属性強化【氷】") > 0)
            {
                int level = GetRanpageLv(equip, "属性強化【氷】");
                int add = 0;
                switch (level)
                {
                    case 1:
                        add = 4;
                        break;
                    case 2:
                        add = 6;
                        break;
                    case 3:
                        add = 8;
                        break;
                    case 4:
                        add = 10;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Ice)
                    {
                        newDamage.ElementValue += add;
                    }
                }
            }

            // 属性強化【龍】
            if (GetRanpageLv(equip, "属性強化【龍】") > 0)
            {
                int level = GetRanpageLv(equip, "属性強化【龍】");
                int add = 0;
                switch (level)
                {
                    case 1:
                        add = 4;
                        break;
                    case 2:
                        add = 6;
                        break;
                    case 3:
                        add = 8;
                        break;
                    case 4:
                        add = 10;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.Dragon)
                    {
                        newDamage.ElementValue += add;
                    }
                }
            }

            // 属性値強化
            if (GetRanpageLv(equip, "属性値強化") > 0)
            {
                int level = GetRanpageLv(equip, "属性値強化");
                int add = 0;
                switch (level)
                {
                    case 1:
                        add = 5;
                        break;
                    case 2:
                        add = 7;
                        break;
                    case 3:
                        add = 10;
                        break;
                    default:
                        break;
                }
                foreach (var newDamage in newDamages)
                {
                    newDamage.ElementValue += add;
                }
            }

            // 属性値激化
            if (HasRanpage(equip, "属性値激化"))
            {
                foreach (var newDamage in newDamages)
                {
                    newDamage.ElementValue += 10;
                    newDamage.WeaponAttack -= 15;
                }
            }

            // 無属性攻撃強化
            if (HasRanpage(equip, "無属性攻撃強化"))
            {
                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementKind == Element.None)
                    {
                        newDamage.WeaponAttack += 10;
                    }
                }
            }

            // 痛恨の一撃
            if (HasRanpage(equip, "痛恨の一撃"))
            {
                foreach (var newDamage in newDamages)
                {
                    newDamage.BrutalStrike = true;
                }
            }

            // 業鎧【修羅】
            if (equip.MailOfHellfireLv > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double swapRedProb = Style.SwapRedProb / 100.0;
                    // 蒼の書
                    if (swapRedProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - swapRedProb);
                        switch (equip.MailOfHellfireLv)
                        {
                            case 1:
                                newDamage.ElementModifier1 *= 1.05;
                                break;
                            case 2:
                                newDamage.ElementModifier1 *= 1.10;
                                break;
                            case >= 3:
                                newDamage.ElementModifier1 *= 1.20;
                                break;
                            default:
                                break;
                        }
                        newDamages.Add(newDamage);
                    }
                    // 朱の書
                    if (swapRedProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * swapRedProb;
                        switch (equip.MailOfHellfireLv)
                        {
                            case 1:
                                newDamage.AdditionalAttack1 += 15;
                                break;
                            case 2:
                                newDamage.AdditionalAttack1 += 25;
                                break;
                            case >= 3:
                                newDamage.AdditionalAttack1 += 35;
                                break;
                            default:
                                break;
                        }
                        newDamages.Add(newDamage);
                    }
                }
            }

            // TODO: 本来は業鎧と独立してはいけない
            // 伏魔響命
            if (equip.DerelictionLv > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double swapRedProb = Style.SwapRedProb / 100.0;
                    double dereliction3Prob = Style.Dereliction3Prob / 100.0;
                    double dereliction2Prob = Math.Max(Style.Dereliction2Prob / 100.0 - dereliction3Prob, 0.0);
                    double dereliction1Prob = 1.0 - dereliction2Prob - dereliction3Prob;

                    // 蒼の書
                    if (swapRedProb < 1)
                    {
                        // キュリア1匹
                        if (dereliction1Prob > 0)
                        {
                            DamagePattern newDamage = oldDamage.Clone();
                            newDamage.Probability = oldDamage.Probability * (1 - swapRedProb) * dereliction1Prob;
                            switch (equip.DerelictionLv)
                            {
                                case 1:
                                    newDamage.AdditionalAttack1 += 15;
                                    break;
                                case 2:
                                    newDamage.AdditionalAttack1 += 20;
                                    break;
                                case >= 3:
                                    newDamage.AdditionalAttack1 += 25;
                                    break;
                                default:
                                    break;
                            }
                            newDamages.Add(newDamage);
                        }
                        // キュリア2匹
                        if (dereliction2Prob > 0)
                        {
                            DamagePattern newDamage = oldDamage.Clone();
                            newDamage.Probability = oldDamage.Probability * (1 - swapRedProb) * dereliction2Prob;
                            switch (equip.DerelictionLv)
                            {
                                case 1:
                                    newDamage.AdditionalAttack1 += 20;
                                    break;
                                case 2:
                                    newDamage.AdditionalAttack1 += 25;
                                    break;
                                case >= 3:
                                    newDamage.AdditionalAttack1 += 30;
                                    break;
                                default:
                                    break;
                            }
                            newDamages.Add(newDamage);
                        }
                        // キュリア3匹
                        if (dereliction3Prob > 0)
                        {
                            DamagePattern newDamage = oldDamage.Clone();
                            newDamage.Probability = oldDamage.Probability * (1 - swapRedProb) * dereliction3Prob;
                            switch (equip.DerelictionLv)
                            {
                                case 1:
                                    newDamage.AdditionalAttack1 += 25;
                                    break;
                                case 2:
                                    newDamage.AdditionalAttack1 += 30;
                                    break;
                                case >= 3:
                                    newDamage.AdditionalAttack1 += 35;
                                    break;
                                default:
                                    break;
                            }
                            newDamages.Add(newDamage);
                        }
                    }
                    // 朱の書
                    if (swapRedProb > 0)
                    {
                        // キュリア1匹
                        if (dereliction1Prob > 0)
                        {
                            DamagePattern newDamage = oldDamage.Clone();
                            newDamage.Probability = oldDamage.Probability * swapRedProb * dereliction1Prob;
                            switch (equip.DerelictionLv)
                            {
                                case 1:
                                    newDamage.AdditionalElement += 5;
                                    break;
                                case 2:
                                    newDamage.AdditionalAttack1 += 7;
                                    break;
                                case >= 3:
                                    newDamage.AdditionalAttack1 += 10;
                                    break;
                                default:
                                    break;
                            }
                            newDamages.Add(newDamage);
                        }
                        // キュリア2匹
                        if (dereliction2Prob > 0)
                        {
                            DamagePattern newDamage = oldDamage.Clone();
                            newDamage.Probability = oldDamage.Probability * swapRedProb * dereliction2Prob;
                            switch (equip.DerelictionLv)
                            {
                                case 1:
                                    newDamage.AdditionalElement += 8;
                                    break;
                                case 2:
                                    newDamage.AdditionalAttack1 += 12;
                                    break;
                                case >= 3:
                                    newDamage.AdditionalAttack1 += 15;
                                    break;
                                default:
                                    break;
                            }
                            newDamages.Add(newDamage);
                        }
                        // キュリア3匹
                        if (dereliction3Prob > 0)
                        {
                            DamagePattern newDamage = oldDamage.Clone();
                            newDamage.Probability = oldDamage.Probability * swapRedProb * dereliction3Prob;
                            switch (equip.DerelictionLv)
                            {
                                case 1:
                                    newDamage.AdditionalElement += 12;
                                    break;
                                case 2:
                                    newDamage.AdditionalAttack1 += 15;
                                    break;
                                case >= 3:
                                    newDamage.AdditionalAttack1 += 20;
                                    break;
                                default:
                                    break;
                            }
                            newDamages.Add(newDamage);
                        }
                    }
                }
            }

            // 巧撃
            if (equip.AdrenalineRushLv > 0 && Style.AdrenalineRushProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double adrenalineRushProb = Style.AdrenalineRushProb / 100.0;
                    // 通常
                    if (adrenalineRushProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - adrenalineRushProb);
                        newDamages.Add(newDamage);
                    }
                    // 巧撃
                    if (adrenalineRushProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * adrenalineRushProb;
                        switch (equip.AdrenalineRushLv)
                        {
                            case 1:
                                newDamage.AdditionalAttack1 += 10;
                                break;
                            case 2:
                                newDamage.AdditionalAttack1 += 15;
                                break;
                            case >= 3:
                                newDamage.AdditionalAttack1 += 30;
                                break;
                            default:
                                break;
                        }
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 災禍転福
            if (equip.CoalescenceLv > 0 && Style.CoalescenceProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double coalescenceProb = Style.CoalescenceProb / 100.0;
                    // 通常
                    if (coalescenceProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - coalescenceProb);
                        newDamages.Add(newDamage);
                    }
                    // 災禍転福
                    if (coalescenceProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * coalescenceProb;
                        switch (equip.CoalescenceLv)
                        {
                            case 1:
                                newDamage.AdditionalAttack1 += 12;
                                newDamage.AdditionalElement += 2;
                                break;
                            case 2:
                                newDamage.AdditionalAttack1 += 15;
                                newDamage.AdditionalElement += 3;
                                break;
                            case >= 3:
                                newDamage.AdditionalAttack1 += 18;
                                newDamage.AdditionalElement += 4;
                                break;
                            default:
                                break;
                        }
                        newDamages.Add(newDamage);
                    }
                }
            }

            // 弱点特効【属性】
            if (equip.ElementExploitLv > 0)
            {
                double modifier = 1;
                switch (equip.ElementExploitLv)
                {
                    case 1:
                        modifier = 1.1;
                        break;
                    case 2:
                        modifier = 1.125;
                        break;
                    case >= 3:
                        modifier = 1.15;
                        break;
                    default:
                        break;
                }

                foreach (var newDamage in newDamages)
                {
                    if (newDamage.ElementPhysiology >= 20)
                    {
                        newDamage.ElementOtherModifier *= modifier;
                    }
                }
            }

            // 闇討ち
            if (equip.SneakAttackLv > 0 && Style.SneakAttackProb > 0)
            {
                oldDamages = newDamages;
                newDamages = new();
                foreach (var oldDamage in oldDamages)
                {
                    double sneakAttackProb = Style.SneakAttackProb / 100.0;
                    // 通常
                    if (sneakAttackProb < 1)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * (1 - sneakAttackProb);
                        newDamages.Add(newDamage);
                    }
                    // 闇討ち
                    if (sneakAttackProb > 0)
                    {
                        DamagePattern newDamage = oldDamage.Clone();
                        newDamage.Probability = oldDamage.Probability * sneakAttackProb;
                        switch (equip.SneakAttackLv)
                        {
                            case 1:
                                newDamage.PhysicsOtherModifier *= 1.05;
                                break;
                            case 2:
                                newDamage.PhysicsOtherModifier *= 1.1;
                                break;
                            case >= 3:
                                newDamage.PhysicsOtherModifier *= 1.2;
                                break;
                            default:
                                break;
                        }
                        newDamages.Add(newDamage);
                    }
                }
            }


            return newDamages;
        }




        private bool HasRanpage(Equipment equip, string skillName)
        {
            if (equip.RampageSkill1 == skillName ||
                equip.RampageSkill2 == skillName ||
                equip.RampageSkill3 == skillName)
            {
                return true;
            }
            return false;
        }

        private int GetRanpageLv(Equipment equip, string skillName)
        {
            List<string> levelStrs = new() { "Ⅰ", "Ⅱ", "Ⅲ", "Ⅳ" };
            for (int i = 0; i < levelStrs.Count; i++)
            {
                if (HasRanpage(equip, skillName + levelStrs[i]))
                {
                    return i + 1;
                }
            }
            return 0;
        }


        private static int CompareFullDamage(CalcResult x, CalcResult y)
        {
            if (x.FullDamage < y.FullDamage)
            {
                return 1;
            }
            else if (x.FullDamage > y.FullDamage)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
