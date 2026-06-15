using System;

namespace Pokemon_Sinjoh_Editor
{
    public class Weight
    {
        public uint hectograms { private set; get; }
        private const double POUNDS_PER_HECTOGRAM = 0.2204622622;
        private const double KILOGRAMS_PER_HECTOGRAM = 0.1;

        public Weight(uint weight) 
        {
            hectograms = weight;
        }

        public double GetPounds() => Math.Round(hectograms * POUNDS_PER_HECTOGRAM, 1);
        public double GetKilograms() => hectograms * KILOGRAMS_PER_HECTOGRAM;
        public void SetImperial(double pounds)
        {
            hectograms = (uint)Math.Round(pounds / POUNDS_PER_HECTOGRAM);
        }
        public void SetKilograms(double kilograms)
        {
            hectograms = (uint)(kilograms / KILOGRAMS_PER_HECTOGRAM);
        }
    }
}
