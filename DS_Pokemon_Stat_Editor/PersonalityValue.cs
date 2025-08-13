using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon_Sinjoh_Editor
{
    public class PersonalityValue
    {
        public uint PV;

        public PersonalityValue(uint pv) 
        {
            this.PV = pv;
        }

        public Gender GetGender(byte genderRatio)
        {
            if (genderRatio == 255)
                return Gender.UNKNOWN;
            else
                return (PV & 0b_1111_1111) > genderRatio ? Gender.MALE : Gender.FEMALE;
        }

        public bool HasSecondAbility()
        {
            return (PV & 0b_0001) == 0;
        }

        public uint GetNature()
        {
            return PV % 25;
        }
    }
}
