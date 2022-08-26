using Prism.Mvvm;
using Reactive.Bindings;
using RiseDamageSim.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.ViewModels.SubViews
{
    internal class BattleStyleTabSubViewModel : BindableBase
    {
        public ReactivePropertySlim<double> SongofRagingFlameProb { get; set; } = new();
        public ReactivePropertySlim<double> SongofAttackProb { get; set; } = new();
        public ReactivePropertySlim<double> SongofCriticalProb { get; set; } = new();
        public ReactivePropertySlim<double> SongofElementProb { get; set; } = new();
        public ReactivePropertySlim<double> PowercharmProb { get; set; } = new(100);
        public ReactivePropertySlim<double> PowertalonProb { get; set; } = new(100);
        public ReactivePropertySlim<double> DemondrugProb { get; set; } = new();
        public ReactivePropertySlim<double> DemondrugGProb { get; set; } = new();
        public ReactivePropertySlim<double> MightSeedProb { get; set; } = new(50);
        public ReactivePropertySlim<double> BuildUpProb { get; set; } = new();
        public ReactivePropertySlim<double> ButterflameProb { get; set; } = new();
        public ReactivePropertySlim<double> DemonPowderProb { get; set; } = new(50);
        public ReactivePropertySlim<double> DemonAmmoProb { get; set; } = new();
        public ReactivePropertySlim<double> FelyneBoosterProb { get; set; } = new();
        public ReactivePropertySlim<double> CutterflyProb { get; set; } = new();
        public ReactivePropertySlim<double> LampsquidProb { get; set; } = new();
        public ReactivePropertySlim<double> RousingRoarProb { get; set; } = new();
        public ReactivePropertySlim<double> PowerDrumProb { get; set; } = new();
        public ReactivePropertySlim<double> PowerSheatheProb { get; set; } = new(75);
        public ReactivePropertySlim<double> PeakPerformanceProb { get; set; } = new();
        public ReactivePropertySlim<double> DragonheartProb { get; set; } = new(80);
        public ReactivePropertySlim<double> CounterstrikeProb { get; set; } = new(30);
        public ReactivePropertySlim<double> ResentmentProb { get; set; } = new(30);
        public ReactivePropertySlim<double> ResuscitateProb { get; set; } = new(80);
        public ReactivePropertySlim<double> FortifyProb { get; set; } = new();
        public ReactivePropertySlim<double> Fortify2Prob { get; set; } = new();
        public ReactivePropertySlim<double> HeroicsProb { get; set; } = new();
        public ReactivePropertySlim<double> DangoAdrenalineProb { get; set; } = new();
        public ReactivePropertySlim<double> MaximumMightProb { get; set; } = new();
        public ReactivePropertySlim<double> LatentPowerProb { get; set; } = new();
        public ReactivePropertySlim<double> AffinitySlidingProb { get; set; } = new();
        public ReactivePropertySlim<double> MagnamaloSoulProb { get; set; } = new();
        public ReactivePropertySlim<double> ChameleosSoulProb { get; set; } = new();
        public ReactivePropertySlim<double> KushalaDaoraSoulProb { get; set; } = new();
        public ReactivePropertySlim<double> KushalaDaoraSoul5Prob { get; set; } = new();
        public ReactivePropertySlim<double> WallRunnerProb { get; set; } = new();
        public ReactivePropertySlim<double> OffensiveGuardProb { get; set; } = new();
        public ReactivePropertySlim<double> RageProb { get; set; } = new(75);
        public ReactivePropertySlim<double> FireBlightProb { get; set; } = new();
        public ReactivePropertySlim<double> WaterBlightProb { get; set; } = new();
        public ReactivePropertySlim<double> ThunderBlightProb { get; set; } = new();
        public ReactivePropertySlim<double> IceBlightProb { get; set; } = new();
        public ReactivePropertySlim<double> AttackCount { get; set; } = new(30);
        public ReactivePropertySlim<double> Spiribirds { get; set; } = new();
        public ReactivePropertySlim<double> FrenzyInfectProb { get; set; } = new();
        public ReactivePropertySlim<double> FrenzyTreatProb { get; set; } = new();
        public ReactivePropertySlim<double> GrinderProb { get; set; } = new();
        public ReactivePropertySlim<double> PoisonProb { get; set; } = new();
        public ReactivePropertySlim<double> ParalysisProb { get; set; } = new();
        public ReactivePropertySlim<double> AdrenalineRushProb { get; set; } = new();
        public ReactivePropertySlim<double> CoalescenceProb { get; set; } = new();
        public ReactivePropertySlim<double> Dereliction2Prob { get; set; } = new(85);
        public ReactivePropertySlim<double> Dereliction3Prob { get; set; } = new(70);
        public ReactivePropertySlim<double> SneakAttackProb { get; set; } = new();
        public ReactivePropertySlim<double> SwapRedProb { get; set; } = new();

        public BattleStyle GetStyle()
        {
            BattleStyle style = new();

            style.SongofRagingFlameProb = SongofRagingFlameProb.Value;
            style.SongofAttackProb = SongofAttackProb.Value;
            style.SongofCriticalProb = SongofCriticalProb.Value;
            style.SongofElementProb = SongofElementProb.Value;
            style.PowercharmProb = PowercharmProb.Value;
            style.PowertalonProb = PowertalonProb.Value;
            style.DemondrugProb = DemondrugProb.Value;
            style.DemondrugGProb = DemondrugGProb.Value;
            style.MightSeedProb = MightSeedProb.Value;
            style.BuildUpProb = BuildUpProb.Value;
            style.ButterflameProb = ButterflameProb.Value;
            style.DemonPowderProb = DemonPowderProb.Value;
            style.DemonAmmoProb = DemonAmmoProb.Value;
            style.FelyneBoosterProb = FelyneBoosterProb.Value;
            style.CutterflyProb = CutterflyProb.Value;
            style.LampsquidProb = LampsquidProb.Value;
            style.RousingRoarProb = RousingRoarProb.Value;
            style.PowerDrumProb = PowerDrumProb.Value;
            style.PowerSheatheProb = PowerSheatheProb.Value;
            style.PeakPerformanceProb = PeakPerformanceProb.Value;
            style.DragonheartProb = DragonheartProb.Value;
            style.CounterstrikeProb = CounterstrikeProb.Value;
            style.ResentmentProb = ResentmentProb.Value;
            style.ResuscitateProb = ResuscitateProb.Value;
            style.FortifyProb = FortifyProb.Value;
            style.Fortify2Prob = Fortify2Prob.Value;
            style.HeroicsProb = HeroicsProb.Value;
            style.DangoAdrenalineProb = DangoAdrenalineProb.Value;
            style.MaximumMightProb = MaximumMightProb.Value;
            style.LatentPowerProb = LatentPowerProb.Value;
            style.AffinitySlidingProb = AffinitySlidingProb.Value;
            style.MagnamaloSoulProb = MagnamaloSoulProb.Value;
            style.ChameleosSoulProb = ChameleosSoulProb.Value;
            style.KushalaDaoraSoulProb = KushalaDaoraSoulProb.Value;
            style.KushalaDaoraSoul5Prob = KushalaDaoraSoul5Prob.Value;
            style.WallRunnerProb = WallRunnerProb.Value;
            style.OffensiveGuardProb = OffensiveGuardProb.Value;
            style.RageProb = RageProb.Value;
            style.FireBlightProb = FireBlightProb.Value;
            style.WaterBlightProb = WaterBlightProb.Value;
            style.ThunderBlightProb = ThunderBlightProb.Value;
            style.IceBlightProb = IceBlightProb.Value;
            style.AttackCount = AttackCount.Value;
            style.Spiribirds = Spiribirds.Value;
            style.FrenzyInfectProb = FrenzyInfectProb.Value;
            style.FrenzyTreatProb = FrenzyTreatProb.Value;
            style.GrinderProb = GrinderProb.Value;
            style.PoisonProb = PoisonProb.Value;
            style.ParalysisProb = ParalysisProb.Value;
            style.AdrenalineRushProb = AdrenalineRushProb.Value;
            style.CoalescenceProb = CoalescenceProb.Value;
            style.Dereliction2Prob = Dereliction2Prob.Value;
            style.Dereliction3Prob = Dereliction3Prob.Value;
            style.SneakAttackProb = SneakAttackProb.Value;
            style.SwapRedProb = SwapRedProb.Value;

            return style;
        }
    }


}
