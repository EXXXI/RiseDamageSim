using RiseDamageSim.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RiseDamageSim.Model
{
    internal class Equipment
    {
        public string Name { get; set; }
        public string Id { get; set; }

        [JsonIgnore]
        public Weapon Weapon { get; set; }
        public string WeaponName
        {
            get { return Weapon.Name; }
            set { Weapon = Masters.GetWeapon(value); }
        }
        public string RampageSkill1 { get; set; }
        public string RampageSkill2 { get; set; }
        public string RampageSkill3 { get; set; }

        /// <summary>
        /// 攻撃錬成
        /// </summary>
        public int AttackAugLv { get; set; }

        /// <summary>
        /// 斬れ味錬成
        /// </summary>
        public int SharpAugLv { get; set; }

        /// <summary>
        /// 会心錬成
        /// </summary>
        public int CritAugLv { get; set; }

        /// <summary>
        /// 属性錬成
        /// </summary>
        public int ElementAugLv { get; set; }

        /// <summary>
        /// 匠
        /// </summary>
        public int HandicraftLv { get; set; }

        /// <summary>
        /// 業物
        /// </summary>
        public int RazorSharpLv { get; set; }

        /// <summary>
        /// 達人芸
        /// </summary>
        public int MastersTouchLv { get; set; }

        /// <summary>
        /// 攻撃
        /// </summary>
        public int AttackBoostLv { get; set; }

        /// <summary>
        /// フルチャージ
        /// </summary>
        public int PeakPerformanceLv { get; set; }

        /// <summary>
        /// 抜刀術【力】
        /// </summary>
        public int PunishingDrawLv { get; set; }

        /// <summary>
        /// 攻めの守勢
        /// </summary>
        public int OffensiveGuardLv { get; set; }

        /// <summary>
        /// 龍気活性
        /// </summary>
        public int DragonheartLv { get; set; }

        /// <summary>
        /// 逆襲
        /// </summary>
        public int CounterstrikeLv { get; set; }

        /// <summary>
        /// 逆恨み
        /// </summary>
        public int ResentmentLv { get; set; }

        /// <summary>
        /// 死中に活
        /// </summary>
        public int ResuscitateLv { get; set; }

        /// <summary>
        /// 鈍器使い
        /// </summary>
        public int BludgeonerLv { get; set; }

        /// <summary>
        /// 不屈
        /// </summary>
        public int FortifyLv { get; set; }

        /// <summary>
        /// 火事場力
        /// </summary>
        public int HeroicsLv { get; set; }

        /// <summary>
        /// 壁面移動
        /// </summary>
        public int WallRunnerLv { get; set; }

        /// <summary>
        /// 見切り
        /// </summary>
        public int CriticalEyeLv { get; set; }

        /// <summary>
        /// 弱点特効
        /// </summary>
        public int WeaknessExploitLv { get; set; }

        /// <summary>
        /// 超会心
        /// </summary>
        public int CriticalBoostLv { get; set; }

        /// <summary>
        /// 抜刀術【技】
        /// </summary>
        public int CriticalDrawLv { get; set; }

        /// <summary>
        /// 渾身
        /// </summary>
        public int MaximumMightLv { get; set; }

        /// <summary>
        /// 力の開放
        /// </summary>
        public int LatentPowerLv { get; set; }

        /// <summary>
        /// 滑走強化
        /// </summary>
        public int AffinitySlidingLv { get; set; }

        /// <summary>
        /// 挑戦者
        /// </summary>
        public int AgitatorLv { get; set; }

        /// <summary>
        /// 火属性攻撃強化
        /// </summary>
        public int FireAttackLv { get; set; }

        /// <summary>
        /// 水属性攻撃強化
        /// </summary>
        public int WaterAttackLv { get; set; }

        /// <summary>
        /// 雷属性攻撃強化
        /// </summary>
        public int ThunderAttackLv { get; set; }

        /// <summary>
        /// 氷属性攻撃強化
        /// </summary>
        public int IceAttackLv { get; set; }

        /// <summary>
        /// 龍属性攻撃強化
        /// </summary>
        public int DragonAttackLv { get; set; }

        /// <summary>
        /// 風雷合一
        /// </summary>
        public int StormsoulLv { get; set; }

        /// <summary>
        /// 会心撃【属性】
        /// </summary>
        public int CriticalElementLv { get; set; }

        /// <summary>
        /// 炎鱗の恩恵
        /// </summary>
        public int TeostraBlessingLv { get; set; }

        /// <summary>
        /// 鋼殻の恩恵
        /// </summary>
        public int KushalaBlessingLv { get; set; }

        /// <summary>
        /// 心眼
        /// </summary>
        public int MindsEyeLv { get; set; }

        /// <summary>
        /// しまき装備数(雷神龍の魂)
        /// </summary>
        public int IbushiLv { get; set; }

        /// <summary>
        /// 狂竜症【蝕】
        /// </summary>
        public int BloodlustLv { get; set; }

        /// <summary>
        /// 研磨術【鋭】
        /// </summary>
        public int GrinderLv { get; set; }

        /// <summary>
        /// 業鎧【修羅】
        /// </summary>
        public int MailOfHellfireLv { get; set; }

        /// <summary>
        /// 攻勢
        /// </summary>
        public int ForayLv { get; set; }

        /// <summary>
        /// 巧撃
        /// </summary>
        public int AdrenalineRushLv { get; set; }

        /// <summary>
        /// 災禍転福
        /// </summary>
        public int CoalescenceLv { get; set; }

        /// <summary>
        /// 弱点特効【属性】
        /// </summary>
        public int ElementExploitLv { get; set; }

        /// <summary>
        /// チャージマスター
        /// </summary>
        public int ChargeMasterLv { get; set; }

        /// <summary>
        /// 伏魔響命
        /// </summary>
        public int DerelictionLv { get; set; }

        /// <summary>
        /// 闇討ち
        /// </summary>
        public int SneakAttackLv { get; set; }

        /// <summary>
        /// 連撃
        /// </summary>
        public int ChainCritLv { get; set; }

        


    }
}
