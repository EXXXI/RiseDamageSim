using Prism.Mvvm;
using Reactive.Bindings;
using RiseDamageSim.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.ViewModels.Controls
{
    internal class EquipmentRowViewModel : BindableBase
    {
        public ReactivePropertySlim<string> Name { get; set; } = new();

        private Equipment Original { get; set; }

        public ReactiveCommand ShowEquipmentCommand { get; } = new ReactiveCommand();

        public ReactiveCommand DeleteEquipmentCommand { get; } = new ReactiveCommand();

        public EquipmentRowViewModel(Equipment equip)
        {
            ShowEquipmentCommand.Subscribe(_ => ShowEquipment());
            DeleteEquipmentCommand.Subscribe(_ => DeleteEquipment());
            Original = equip;
            Name.Value = equip.Name;
        }

        private void ShowEquipment()
        {
            MainViewModel.Instance.SetEquipment(Original);
        }

        private void DeleteEquipment()
        {
            MainViewModel.Instance.DeleteEquipment(Original);
        }

        public Equipment GetOriginal()
        {
            return Original;
        }
    }
}
