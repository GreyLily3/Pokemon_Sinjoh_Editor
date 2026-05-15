using System;

namespace Pokemon_Sinjoh_Editor
{
    public class Height
    {
        public uint decimeters;
        private const double INCHES_PER_DECIMETERS = 3.937007874;
        private const double METERS_PER_DECIMETER = 0.1;

        public Height(uint height)
        {
            decimeters = height;
        }

        public int GetInches() => (int)Math.Round(decimeters * INCHES_PER_DECIMETERS) % 12;
        public int GetFeet() => (int)Math.Round(decimeters * INCHES_PER_DECIMETERS) / 12;
        public double GetMeters() => decimeters * METERS_PER_DECIMETER;
        public void SetImperial(int feet, int inches)
        {
            decimeters = (uint)Math.Round((feet * 12 + inches) / INCHES_PER_DECIMETERS);
        }
        public void SetMeters(double meters)
        {
            decimeters = (uint)(meters / METERS_PER_DECIMETER);
        }
    }
}
