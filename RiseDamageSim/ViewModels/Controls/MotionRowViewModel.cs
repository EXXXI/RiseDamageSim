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
    internal class MotionRowViewModel : BindableBase
    {
        public ReactivePropertySlim<double> Frequency { get; set; } = new(0);
        public ReactivePropertySlim<double> PunishingDrawProbability { get; set; } = new(0);
        public ReactivePropertySlim<double> CriticalDrawProbability { get; set; } = new(0);
        public ReactivePropertySlim<string> Name { get; set; } = new();
        public ReactivePropertySlim<int> MotionValue { get; set; } = new();
        public ReactivePropertySlim<double> SharpnessModifier { get; set; } = new();
        public ReactivePropertySlim<string> PhysicsElementName { get; set; } = new();
        public ReactivePropertySlim<double> ElementModifier { get; set; } = new();
        public ReactivePropertySlim<string> IsSilkbind { get; set; } = new();
        private Motion Original { get; set; }

        public MotionRowViewModel(Motion motion)
        {
            Name.Value = motion.Name;
            MotionValue.Value = motion.MotionValue;
            SharpnessModifier.Value = motion.SharpnessModifier;
            PhysicsElementName.Value = motion.PhysicsElement.Str();
            ElementModifier.Value = motion.ElementModifier;
            IsSilkbind.Value = motion.IsSilkbind ? "蟲技" : string.Empty;

            Original = motion;
        }

        public Motion GetOriginal()
        {
            Original.Frequency = Frequency.Value;
            Original.PunishingDrawProbability = PunishingDrawProbability.Value;
            Original.CriticalDrawProbability = CriticalDrawProbability.Value;
            return Original;
        }
    }
}
