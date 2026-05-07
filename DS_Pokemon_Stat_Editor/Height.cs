using System;

namespace Pokemon_Sinjoh_Editor
{
    public class Height
    {
        private uint decimeters;

        public Height(uint height)
        {
            decimeters = height;
        }

        public int GetInches() => (int)Math.Round(decimeters * 3.937007874) % 12;
        public int GetFeet() => (int)Math.Round(decimeters * 3.937007874, 1) / 12;
        public double GetMeters() => decimeters / 10D;

    }
}
