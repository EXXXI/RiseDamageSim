using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseDamageSim.Model
{
    internal class Sharpness
    {
        public double Red { get; set; }
        public double Orange { get; set; }
        public double Yellow { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
        public double White { get; set; }
        public double Purple { get; set; }
        public double Max { get; set; }

        public void SetSharpness(List<double> sharpList)
        {
            if (sharpList.Count > 0)
            {
                Red = sharpList[0];
            }
            if (sharpList.Count > 1)
            {
                Orange = sharpList[1];
            }
            if (sharpList.Count > 2)
            {
                Yellow = sharpList[2];
            }
            if (sharpList.Count > 3)
            {
                Green = sharpList[3];
            }
            if (sharpList.Count > 4)
            {
                Blue = sharpList[4];
            }
            if (sharpList.Count > 5)
            {
                White = sharpList[5];
            }
            if (sharpList.Count > 6)
            {
                Purple = sharpList[6];
            }
        }

        public Sharpness GetUseSharpness(double useCount, double handicraftAdditional, double augAdditional)
        {
            List<double> full = new() { Red, Orange, Yellow, Green, Blue, White, Purple };
            double max = Max + handicraftAdditional;
            double sum1 = 0.0;
            List<double> maxed = new();
            foreach (var sharp in full)
            {
                if (sum1 >= max)
                {
                    maxed.Add(0.0);
                }
                else if(sum1 + sharp >= max)
                {
                    maxed.Add(max - sum1 + augAdditional);
                    sum1 = max;
                }
                else
                {
                    maxed.Add(sharp);
                    sum1 += sharp;
                }
            }

            // 一旦処理の都合で反転
            maxed.Reverse();

            List<double> use = new();
            double sum2 = 0.0;
            foreach (var sharp in maxed)
            {
                if (sum2 >= useCount)
                {
                    use.Add(0.0);
                }
                else if (sum2 + sharp >= useCount)
                {
                    use.Add(useCount - sum2);
                    sum2 = useCount;
                }
                else
                {
                    use.Add(sharp);
                    sum2 += sharp;
                }
            }

            // 反転を戻す
            use.Reverse();

            if (sum2 < useCount)
            {
                use[0] += useCount - sum2;
            }

            Sharpness useSharpness = new();
            useSharpness.SetSharpness(use);

            return useSharpness;
        }
    }
}
