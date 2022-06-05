using Prism.Mvvm;
using Reactive.Bindings;
using RiseDamageSim.Domain;
using RiseDamageSim.Model;
using RiseDamageSim.ViewModels.Controls;
using RiseDamageSim.ViewModels.SubViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.ViewModels
{
    internal class MainViewModel : BindableBase
    {
        public static MainViewModel Instance;

        public ReactivePropertySlim<EquipmentTabSubViewModel> EquipmentTabSubVM { get; set; } = new(new EquipmentTabSubViewModel());
        public ReactivePropertySlim<PhysiologyMotionTabSubViewModel> PhysiologyMotionTabSubVM { get; set; } = new(new PhysiologyMotionTabSubViewModel());
        public ReactivePropertySlim<BattleStyleTabSubViewModel> BattleStyleTabSubVM { get; set; } = new(new BattleStyleTabSubViewModel());

        public ReactivePropertySlim<List<CalcResult>> CalcResults { get; set; } = new();

        public ReactiveCommand CalcCommand { get; set; } = new();

        public MainViewModel()
        {
            Instance = this;

            CalcCommand.Subscribe(_ => Calc());
        }

        private void Calc()
        {
            var equipments = EquipmentTabSubVM.Value.GetEquipments();
            var physiologies = PhysiologyMotionTabSubVM.Value.GetPhisiologies();
            var motions = PhysiologyMotionTabSubVM.Value.GetMotions();
            var style = BattleStyleTabSubVM.Value.GetStyle();
            
            Simulator simulator = new(equipments, physiologies, motions, style);

            CalcResults.Value = simulator.CalcDamageAll();
        }

        internal void DeleteEquipment(Equipment original)
        {
            EquipmentTabSubVM.Value.DeleteEquipment(original);
        }

        internal void SetEquipment(Equipment original)
        {
            EquipmentTabSubVM.Value.SetEquipment(original);
        }




    }
}
