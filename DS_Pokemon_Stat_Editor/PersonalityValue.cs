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

        public Gender GetGender(int genderRatio)
        {
            if (genderRatio == 255)
                return Gender.UNKNOWN;
            else
                return (PV & 0b_1111_1111) > genderRatio ? Gender.MALE : Gender.FEMALE;
        }

        public bool GetHasSecondAbility()
        {
            return (PV & 0b_0001) == 1;
        }

        public Nature GetNature()
        {
            return (Nature)(PV % 25);
        }
    }
}
