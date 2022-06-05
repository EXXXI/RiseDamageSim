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
    internal class PhysiologyMotionTabSubViewModel : BindableBase
    {
        #region 肉質
        public ReactivePropertySlim<List<Monster>> Monsters { get; set; } = new();
        public ReactivePropertySlim<Monster> SelectedMonster { get; set; } = new();
        public ReactivePropertySlim<List<PhysiologyRowViewModel>> PhysiologyVMs { get; set; } = new();
        public ReactivePropertySlim<double> RageModifier { get; set; } = new(1.0);
        public ReactivePropertySlim<bool> IsAerial { get; set; } = new(false);
        public ReactivePropertySlim<bool> IsAquatic { get; set; } = new(false);
        public ReactivePropertySlim<bool> IsWyvern { get; set; } = new(false);
        public ReactivePropertySlim<bool> IsSmall { get; set; } = new(false);

        #endregion

        #region モーション
        public ReactivePropertySlim<List<MotionRowViewModel>> MotionVMs { get; set; } = new();

        #endregion

        public PhysiologyMotionTabSubViewModel()
        {

            Monsters.Value = Masters.Monsters;
            SelectedMonster.Value = Monsters.Value[0];

            SelectedMonster.Subscribe(_ => ShowPhysiology());
            ShowMotion();
        }

        private void ShowPhysiology()
        {
            List<PhysiologyRowViewModel> vms = new();
            foreach (var phisiology in SelectedMonster.Value.Physiologies)
            {
                vms.Add(new PhysiologyRowViewModel(phisiology));
            }
            PhysiologyVMs.Value = vms;
        }

        private void ShowMotion()
        {
            List<MotionRowViewModel> vms = new();
            foreach (var motion in Masters.GreatSwordMotion)
            {
                vms.Add(new MotionRowViewModel(motion));
            }
            MotionVMs.Value = vms;
        }

        public List<Physiology> GetPhisiologies()
        {
            List<Physiology> physiologies = new();
            foreach (var vm in PhysiologyVMs.Value)
            {
                var physiology = vm.GetOriginal();
                physiology.RageModifier = RageModifier.Value;
                physiology.IsAerial = IsAerial.Value;
                physiology.IsAquatic = IsAquatic.Value;
                physiology.IsWyvern = IsWyvern.Value;
                physiology.IsSmall = IsSmall.Value;
                physiologies.Add(vm.GetOriginal());
            }
            return physiologies;
        }

        public List<Motion> GetMotions()
        {
            List<Motion> motions = new();
            foreach (var vm in MotionVMs.Value)
            {
                motions.Add(vm.GetOriginal());
            }
            return motions;
        }
    }
}
