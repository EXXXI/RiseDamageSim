using Prism.Mvvm;
using Reactive.Bindings;
using RiseDamageSim.Model;
using RiseDamageSim.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.ViewModels.SubViews
{
    internal class EquipmentTabSubViewModel : BindableBase
    {
        public ReactivePropertySlim<string> Name { get; set; } = new();
        public string Id { get; set; } = string.Empty;
        public ReactivePropertySlim<List<Weapon>> Weapons { get; set; } = new();
        public ReactivePropertySlim<Weapon> SelectedWeapon { get; set; } = new();
        public ReactivePropertySlim<List<string>> RampageSkills { get; set; } = new();
        public ReactivePropertySlim<string> SelectedRampageSkill1 { get; set; } = new();
        public ReactivePropertySlim<string> SelectedRampageSkill2 { get; set; } = new();
        public ReactivePropertySlim<string> SelectedRampageSkill3 { get; set; } = new();
        public ReactivePropertySlim<List<int>> SkillLvs { get; set; } = new();
        public ReactivePropertySlim<List<EquipmentRowViewModel>> EquipmentVMs { get; set; } = new();
        public ReactivePropertySlim<bool> ShowAllRampage { get; set; } = new(true);
        public ReactivePropertySlim<List<CompareSet>> CompareSets { get; set; } = new();
        public ReactivePropertySlim<CompareSet> SelectedSet { get; set; } = new();
        public ReactivePropertySlim<string> SetName { get; set; } = new();

        #region スキルレベル
        /// <summary>
        /// 攻撃錬成
        /// </summary>
        public ReactivePropertySlim<int> AttackAugLv { get; set; } = new(0);

        /// <summary>
        /// 斬れ味錬成
        /// </summary>
        public ReactivePropertySlim<int> SharpAugLv { get; set; } = new(0);

        /// <summary>
        /// 会心錬成
        /// </summary>
        public ReactivePropertySlim<int> CritAugLv { get; set; } = new(0);

        /// <summary>
        /// 属性錬成
        /// </summary>
        public ReactivePropertySlim<int> ElementAugLv { get; set; } = new(0);

        /// <summary>
        /// 匠
        /// </summary>
        public ReactivePropertySlim<int> HandicraftLv { get; set; } = new(0);

        /// <summary>
        /// 業物
        /// </summary>
        public ReactivePropertySlim<int> RazorSharpLv { get; set; } = new(0);

        /// <summary>
        /// 達人芸
        /// </summary>
        public ReactivePropertySlim<int> MastersTouchLv { get; set; } = new(0);

        /// <summary>
        /// 攻撃
        /// </summary>
        public ReactivePropertySlim<int> AttackBoostLv { get; set; } = new(0);

        /// <summary>
        /// フルチャージ
        /// </summary>
        public ReactivePropertySlim<int> PeakPerformanceLv { get; set; } = new(0);

        /// <summary>
        /// 抜刀術【力】
        /// </summary>
        public ReactivePropertySlim<int> PunishingDrawLv { get; set; } = new(0);

        /// <summary>
        /// 攻めの守勢
        /// </summary>
        public ReactivePropertySlim<int> OffensiveGuardLv { get; set; } = new(0);

        /// <summary>
        /// 龍気活性
        /// </summary>
        public ReactivePropertySlim<int> DragonheartLv { get; set; } = new(0);

        /// <summary>
        /// 逆襲
        /// </summary>
        public ReactivePropertySlim<int> CounterstrikeLv { get; set; } = new(0);

        /// <summary>
        /// 逆恨み
        /// </summary>
        public ReactivePropertySlim<int> ResentmentLv { get; set; } = new(0);

        /// <summary>
        /// 死中に活
        /// </summary>
        public ReactivePropertySlim<int> ResuscitateLv { get; set; } = new(0);

        /// <summary>
        /// 不屈
        /// </summary>
        public ReactivePropertySlim<int> FortifyLv { get; set; } = new(0);

        /// <summary>
        /// 鈍器使い
        /// </summary>
        public ReactivePropertySlim<int> BludgeonerLv { get; set; } = new(0);

        /// <summary>
        /// 火事場力
        /// </summary>
        public ReactivePropertySlim<int> HeroicsLv { get; set; } = new(0);

        /// <summary>
        /// 壁面移動
        /// </summary>
        public ReactivePropertySlim<int> WallRunnerLv { get; set; } = new(0);

        /// <summary>
        /// 見切り
        /// </summary>
        public ReactivePropertySlim<int> CriticalEyeLv { get; set; } = new(0);

        /// <summary>
        /// 弱点特効
        /// </summary>
        public ReactivePropertySlim<int> WeaknessExploitLv { get; set; } = new(0);

        /// <summary>
        /// 超会心
        /// </summary>
        public ReactivePropertySlim<int> CriticalBoostLv { get; set; } = new(0);

        /// <summary>
        /// 抜刀術【技】
        /// </summary>
        public ReactivePropertySlim<int> CriticalDrawLv { get; set; } = new(0);

        /// <summary>
        /// 渾身
        /// </summary>
        public ReactivePropertySlim<int> MaximumMightLv { get; set; } = new(0);

        /// <summary>
        /// 力の開放
        /// </summary>
        public ReactivePropertySlim<int> LatentPowerLv { get; set; } = new(0);

        /// <summary>
        /// 滑走強化
        /// </summary>
        public ReactivePropertySlim<int> AffinitySlidingLv { get; set; } = new(0);

        /// <summary>
        /// 挑戦者
        /// </summary>
        public ReactivePropertySlim<int> AgitatorLv { get; set; } = new(0);

        /// <summary>
        /// 火属性攻撃強化
        /// </summary>
        public ReactivePropertySlim<int> FireAttackLv { get; set; } = new(0);

        /// <summary>
        /// 水属性攻撃強化
        /// </summary>
        public ReactivePropertySlim<int> WaterAttackLv { get; set; } = new(0);

        /// <summary>
        /// 雷属性攻撃強化
        /// </summary>
        public ReactivePropertySlim<int> ThunderAttackLv { get; set; } = new(0);

        /// <summary>
        /// 氷属性攻撃強化
        /// </summary>
        public ReactivePropertySlim<int> IceAttackLv { get; set; } = new(0);

        /// <summary>
        /// 龍属性攻撃強化
        /// </summary>
        public ReactivePropertySlim<int> DragonAttackLv { get; set; } = new(0);

        /// <summary>
        /// 風雷合一
        /// </summary>
        public ReactivePropertySlim<int> StormsoulLv { get; set; } = new(0);

        /// <summary>
        /// 会心撃【属性】
        /// </summary>
        public ReactivePropertySlim<int> CriticalElementLv { get; set; } = new(0);

        /// <summary>
        /// 炎鱗の恩恵
        /// </summary>
        public ReactivePropertySlim<int> TeostraBlessingLv { get; set; } = new(0);

        /// <summary>
        /// 鋼殻の恩恵
        /// </summary>
        public ReactivePropertySlim<int> KushalaBlessingLv { get; set; } = new(0);

        /// <summary>
        /// 心眼
        /// </summary>
        public ReactivePropertySlim<int> MindsEyeLv { get; set; } = new(0);

        /// <summary>
        /// しまき装備数(雷神龍の魂)
        /// </summary>
        public ReactivePropertySlim<int> IbushiLv { get; set; } = new(0);

        /// <summary>
        /// 狂竜症【蝕】
        /// </summary>
        public ReactivePropertySlim<int> BloodlustLv { get; set; } = new(0);

        /// <summary>
        /// 研磨術【鋭】
        /// </summary>
        public ReactivePropertySlim<int> GrinderLv { get; set; } = new(0);

        /// <summary>
        /// 業鎧【修羅】
        /// </summary>
        public ReactivePropertySlim<int> MailOfHellfireLv { get; set; } = new(0);

        /// <summary>
        /// 攻勢
        /// </summary>
        public ReactivePropertySlim<int> ForayLv { get; set; } = new(0);

        /// <summary>
        /// 巧撃
        /// </summary>
        public ReactivePropertySlim<int> AdrenalineRushLv { get; set; } = new(0);

        /// <summary>
        /// 災禍転福
        /// </summary>
        public ReactivePropertySlim<int> CoalescenceLv { get; set; } = new(0);

        /// <summary>
        /// 弱点特効【属性】
        /// </summary>
        public ReactivePropertySlim<int> ElementExploitLv { get; set; } = new(0);

        /// <summary>
        /// チャージマスター
        /// </summary>
        public ReactivePropertySlim<int> ChargeMasterLv { get; set; } = new(0);

        /// <summary>
        /// 伏魔響命
        /// </summary>
        public ReactivePropertySlim<int> DerelictionLv { get; set; } = new(0);

        /// <summary>
        /// 闇討ち
        /// </summary>
        public ReactivePropertySlim<int> SneakAttackLv { get; set; } = new(0);

        /// <summary>
        /// 連撃
        /// </summary>
        public ReactivePropertySlim<int> ChainCritLv { get; set; } = new(0);

        /// <summary>
        /// 蓄積時攻撃強化
        /// </summary>
        public ReactivePropertySlim<int> BuildupBoostLv { get; set; } = new(0);
        #endregion

        public ReactiveCommand AddEquipmentCommand { get; } = new ReactiveCommand();
        public ReactiveCommand UpdateEquipmentCommand { get; } = new ReactiveCommand();
        public ReactiveCommand AddNewSetCommand { get; } = new ReactiveCommand();
        public ReactiveCommand UpdateSetNameCommand { get; } = new ReactiveCommand();
        public ReactiveCommand DeleteSetCommand { get; } = new ReactiveCommand();


        public EquipmentTabSubViewModel()
        {
            Weapons.Value = Masters.GreatSwords;
            SelectedWeapon.Value = Weapons.Value[0];
            SkillLvs.Value = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            CompareSets.Value = Masters.CompareSets;
            SelectedSet.Value = CompareSets.Value[0];

            ShowAllRampage.Subscribe(_ => SetRampageSkills());
            SelectedWeapon.Subscribe(_ => SetRampageSkills());
            AddEquipmentCommand.Subscribe(_ => SaveEquipment(false));
            UpdateEquipmentCommand.Subscribe(_ => SaveEquipment(true));
            SelectedSet.Subscribe(_ => ShowEquipments());
            AddNewSetCommand.Subscribe(_ => SaveSet(false));
            UpdateSetNameCommand.Subscribe(_ => SaveSet(true));
            DeleteSetCommand.Subscribe(_ => DeleteSet());
        }

        private void SetRampageSkills()
        {
            string none = "なし";
            List<string> rampages = new();
            rampages.Add(none);
            if (ShowAllRampage.Value)
            {
                foreach (var rampage in Masters.RampageSkills)
                {
                    rampages.Add(rampage);
                }
            }
            else
            {
                foreach (var rampage in SelectedWeapon.Value.RampageSkills)
                {
                    rampages.Add(rampage);
                }
            }
            RampageSkills.Value = rampages;
            SelectedRampageSkill1.Value = none;
            SelectedRampageSkill2.Value = none;
            SelectedRampageSkill3.Value = none;
        }

        private void SaveEquipment(bool isUpdate)
        {
            Equipment? equip = null;
            if (isUpdate)
            {
                equip = Masters.GetEquipment(Id);
            }
            if (equip == null)
            {
                equip = new Equipment
                {
                    Id = Guid.NewGuid().ToString()
                };
                Id = equip.Id;
                Masters.AddEquipment(SelectedSet.Value, equip);
            }
            equip.Name = Name.Value;
            equip.Weapon = SelectedWeapon.Value;
            equip.RampageSkill1 = SelectedRampageSkill1.Value;
            equip.RampageSkill2 = SelectedRampageSkill2.Value;
            equip.RampageSkill3 = SelectedRampageSkill3.Value;
            equip.AttackAugLv = AttackAugLv.Value;
            equip.SharpAugLv = SharpAugLv.Value;
            equip.CritAugLv = CritAugLv.Value;
            equip.ElementAugLv = ElementAugLv.Value;
            equip.HandicraftLv = HandicraftLv.Value;
            equip.RazorSharpLv = RazorSharpLv.Value;
            equip.MastersTouchLv = MastersTouchLv.Value;
            equip.AttackBoostLv = AttackBoostLv.Value;
            equip.PeakPerformanceLv = PeakPerformanceLv.Value;
            equip.PunishingDrawLv = PunishingDrawLv.Value;
            equip.OffensiveGuardLv = OffensiveGuardLv.Value;
            equip.DragonheartLv = DragonheartLv.Value;
            equip.CounterstrikeLv = CounterstrikeLv.Value;
            equip.ResentmentLv = ResentmentLv.Value;
            equip.ResuscitateLv = ResuscitateLv.Value;
            equip.BludgeonerLv = BludgeonerLv.Value;
            equip.FortifyLv = FortifyLv.Value;
            equip.HeroicsLv = HeroicsLv.Value;
            equip.WallRunnerLv = WallRunnerLv.Value;
            equip.CriticalEyeLv = CriticalEyeLv.Value;
            equip.WeaknessExploitLv = WeaknessExploitLv.Value;
            equip.CriticalBoostLv = CriticalBoostLv.Value;
            equip.CriticalDrawLv = CriticalDrawLv.Value;
            equip.MaximumMightLv = MaximumMightLv.Value;
            equip.LatentPowerLv = LatentPowerLv.Value;
            equip.AffinitySlidingLv = AffinitySlidingLv.Value;
            equip.AgitatorLv = AgitatorLv.Value;
            equip.FireAttackLv = FireAttackLv.Value;
            equip.WaterAttackLv = WaterAttackLv.Value;
            equip.ThunderAttackLv = ThunderAttackLv.Value;
            equip.IceAttackLv = IceAttackLv.Value;
            equip.DragonAttackLv = DragonAttackLv.Value;
            equip.StormsoulLv = StormsoulLv.Value;
            equip.CriticalElementLv = CriticalElementLv.Value;
            equip.TeostraBlessingLv = TeostraBlessingLv.Value;
            equip.KushalaBlessingLv = KushalaBlessingLv.Value;
            equip.MindsEyeLv = MindsEyeLv.Value;
            equip.IbushiLv = IbushiLv.Value;
            equip.BloodlustLv = BloodlustLv.Value;
            equip.GrinderLv = GrinderLv.Value;
            equip.MailOfHellfireLv = MailOfHellfireLv.Value;
            equip.ForayLv = ForayLv.Value;
            equip.AdrenalineRushLv = AdrenalineRushLv.Value;
            equip.CoalescenceLv = CoalescenceLv.Value;
            equip.ElementExploitLv = ElementExploitLv.Value;
            equip.ChargeMasterLv = ChargeMasterLv.Value;
            equip.DerelictionLv = DerelictionLv.Value;
            equip.SneakAttackLv = SneakAttackLv.Value;
            equip.ChainCritLv = ChainCritLv.Value;
            equip.BuildupBoostLv = BuildupBoostLv.Value;

            Masters.SaveCompareSets();
            ShowEquipments();

        }

        internal void SetEquipment(Equipment equip)
        {
            Name.Value = equip.Name;
            Id = equip.Id;
            SelectedWeapon.Value = equip.Weapon;
            SelectedRampageSkill1.Value = equip.RampageSkill1;
            SelectedRampageSkill2.Value = equip.RampageSkill2;
            SelectedRampageSkill3.Value = equip.RampageSkill3;
            AttackAugLv.Value = equip.AttackAugLv;
            SharpAugLv.Value = equip.SharpAugLv;
            CritAugLv.Value = equip.CritAugLv;
            ElementAugLv.Value = equip.ElementAugLv;
            HandicraftLv.Value = equip.HandicraftLv;
            RazorSharpLv.Value = equip.RazorSharpLv;
            MastersTouchLv.Value = equip.MastersTouchLv;
            AttackBoostLv.Value = equip.AttackBoostLv;
            PeakPerformanceLv.Value = equip.PeakPerformanceLv;
            PunishingDrawLv.Value = equip.PunishingDrawLv;
            OffensiveGuardLv.Value = equip.OffensiveGuardLv;
            DragonheartLv.Value = equip.DragonheartLv;
            CounterstrikeLv.Value = equip.CounterstrikeLv;
            ResentmentLv.Value = equip.ResentmentLv;
            ResuscitateLv.Value = equip.ResuscitateLv;
            BludgeonerLv.Value = equip.BludgeonerLv;
            FortifyLv.Value = equip.FortifyLv;
            HeroicsLv.Value = equip.HeroicsLv;
            WallRunnerLv.Value = equip.WallRunnerLv;
            CriticalEyeLv.Value = equip.CriticalEyeLv;
            WeaknessExploitLv.Value = equip.WeaknessExploitLv;
            CriticalBoostLv.Value = equip.CriticalBoostLv;
            CriticalDrawLv.Value = equip.CriticalDrawLv;
            MaximumMightLv.Value = equip.MaximumMightLv;
            LatentPowerLv.Value = equip.LatentPowerLv;
            AffinitySlidingLv.Value = equip.AffinitySlidingLv;
            AgitatorLv.Value = equip.AgitatorLv;
            FireAttackLv.Value = equip.FireAttackLv;
            WaterAttackLv.Value = equip.WaterAttackLv;
            ThunderAttackLv.Value = equip.ThunderAttackLv;
            IceAttackLv.Value = equip.IceAttackLv;
            DragonAttackLv.Value = equip.DragonAttackLv;
            StormsoulLv.Value = equip.StormsoulLv;
            CriticalElementLv.Value = equip.CriticalElementLv;
            TeostraBlessingLv.Value = equip.TeostraBlessingLv;
            KushalaBlessingLv.Value = equip.KushalaBlessingLv;
            MindsEyeLv.Value = equip.MindsEyeLv;
            IbushiLv.Value = equip.IbushiLv;
            BloodlustLv.Value = equip.BloodlustLv;
            GrinderLv.Value = equip.GrinderLv;
            MailOfHellfireLv.Value = equip.MailOfHellfireLv;
            ForayLv.Value = equip.ForayLv;
            AdrenalineRushLv.Value = equip.AdrenalineRushLv;
            CoalescenceLv.Value = equip.CoalescenceLv;
            ElementExploitLv.Value = equip.ElementExploitLv;
            ChargeMasterLv.Value = equip.ChargeMasterLv;
            DerelictionLv.Value = equip.DerelictionLv;
            SneakAttackLv.Value = equip.SneakAttackLv;
            ChainCritLv.Value = equip.ChainCritLv;
            BuildupBoostLv.Value = equip.BuildupBoostLv;
        }

        internal void DeleteEquipment(Equipment original)
        {
            Masters.DeleteEquipment(SelectedSet.Value, original);
            ShowEquipments();
        }

        private void SaveSet(bool isUpdate)
        {
            CompareSet? set = null;
            if (isUpdate)
            {
                set = Masters.GetSet(SelectedSet.Value);
            }
            if (set == null)
            {
                set = new CompareSet();
                Masters.AddSet(set);
            }
            set.Name = SetName.Value;

            Masters.SaveCompareSets();
            List<CompareSet> list = new();
            foreach (CompareSet masset in Masters.CompareSets)
            {
                list.Add(masset);
            }
            CompareSets.Value = list;
            SelectedSet.Value = set;
            ShowEquipments();
        }



        private void DeleteSet()
        {
            bool isDeleted = Masters.DeleteSet(SelectedSet.Value);

            if (!isDeleted)
            {
                return;
            }

            Masters.SaveCompareSets();
            List<CompareSet> list = new();
            foreach (CompareSet masset in Masters.CompareSets)
            {
                list.Add(masset);
            }
            CompareSets.Value = list;
            SelectedSet.Value = CompareSets.Value[0];
            ShowEquipments();
        }

        private void ShowEquipments()
        {
            if (SelectedSet.Value == null)
            {
                SelectedSet.Value = CompareSets.Value[0];
            }
            SetName.Value = SelectedSet.Value.Name;
            List<EquipmentRowViewModel> vms = new();
            foreach (var equip in SelectedSet.Value.Equipments)
            {
                vms.Add(new EquipmentRowViewModel(equip));
            }
            EquipmentVMs.Value = vms;
        }

        public List<Equipment> GetEquipments()
        {
            List<Equipment> equipments = new();    
            foreach (var vm in EquipmentVMs.Value)
            {
                equipments.Add(vm.GetOriginal());
            }
            return equipments;
        }
    }
}
