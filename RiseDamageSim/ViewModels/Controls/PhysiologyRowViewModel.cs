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
    internal class PhysiologyRowViewModel : BindableBase
    {
        public ReactivePropertySlim<double> Frequency { get; set; } = new(0);
        public ReactivePropertySlim<string> FullName { get; set; } = new(string.Empty);
        public ReactivePropertySlim<int> Sever { get; set; } = new();
        public ReactivePropertySlim<int> Blunt { get; set; } = new();
        public ReactivePropertySlim<int> Shot { get; set; } = new();
        public ReactivePropertySlim<int> Fire { get; set; } = new();
        public ReactivePropertySlim<int> Water { get; set; } = new();
        public ReactivePropertySlim<int> Thunder { get; set; } = new();
        public ReactivePropertySlim<int> Ice { get; set; } = new();
        public ReactivePropertySlim<int> Dragon { get; set; } = new();
        public ReactivePropertySlim<int> Stun { get; set; } = new();
        public ReactivePropertySlim<int> RageSever { get; set; } = new();
        public ReactivePropertySlim<int> RageBlunt { get; set; } = new();
        public ReactivePropertySlim<int> RageShot { get; set; } = new();
        public ReactivePropertySlim<int> RageFire { get; set; } = new();
        public ReactivePropertySlim<int> RageWater { get; set; } = new();
        public ReactivePropertySlim<int> RageThunder { get; set; } = new();
        public ReactivePropertySlim<int> RageIce { get; set; } = new();
        public ReactivePropertySlim<int> RageDragon { get; set; } = new();
        public ReactivePropertySlim<int> RageStun { get; set; } = new();
        private Physiology Original { get; set; }

        public PhysiologyRowViewModel(Physiology phisiology)
        {
            Original = phisiology;

            FullName.Value = $"{phisiology.Name}({phisiology.StateName})";
            Sever.Value = phisiology.Sever;
            Blunt.Value = phisiology.Blunt;
            Shot.Value = phisiology.Shot;
            Fire.Value = phisiology.Fire;
            Water.Value = phisiology.Water;
            Thunder.Value = phisiology.Thunder;
            Ice.Value = phisiology.Ice;
            Dragon.Value = phisiology.Dragon;
            Stun.Value = phisiology.Stun;
            RageSever.Value = phisiology.RageSever;
            RageBlunt.Value = phisiology.RageBlunt;
            RageShot.Value = phisiology.RageShot;
            RageFire.Value = phisiology.RageFire;
            RageWater.Value = phisiology.RageWater;
            RageThunder.Value = phisiology.RageThunder;
            RageIce.Value = phisiology.RageIce;
            RageDragon.Value = phisiology.RageDragon;
            RageStun.Value = phisiology.RageStun;
        }

        public Physiology GetOriginal()
        {
            Original.Frequency = Frequency.Value;
            return Original;
        }
    }
}
