using System;

namespace Pokemon_Sinjoh_Editor
{
    public class Weight
    {
        private uint hectograms;

        public Weight(uint weight) 
        {
            hectograms = weight;
        }

        public double GetPounds() => Math.Round(hectograms * 0.2204622622, 1);
        public double GetKilograms() => hectograms / 10D;
    }
}
