using System;

namespace Pokemon_Sinjoh_Editor
{
    public class PersonalityValue
    {
        public uint PV;

        private const int ANY_NATURE = -1;
        private const int ANY_GENDER = -1;
        private const int ANY_ABILITY = -1;
        public const int ABILITY1 = 1;
        public const int ABILITY2 = 2;
        private const int NUM_NATURES = 25;
        private const int GENDER_RATIO_BIT_MASK = 0b_1111_1111;
        private const int ABILITY_BIT_MASK = 0b_0001;

        public PersonalityValue(uint pv)
        {
            this.PV = pv;
        }

        public PersonalityValue()
        {
            this.PV = GenerateRandom();
        }

        private uint GenerateRandom() => (uint)new Random().Next(-int.MaxValue, int.MaxValue);

        private void GenerateWithTraits(PokemonSpecies species, int desiredGender, int desiredNature, int desiredAbility)
        {
            Random RandomGenerator = new Random();
            bool correctGender = false;
            bool correctAbility = false;
            int unomdifiedNature;

            do
            {
                PV = GenerateRandom();

                unomdifiedNature = (int)GetNature();
                if (desiredNature != ANY_NATURE)
                {
                    this.PV += (uint)(desiredNature - unomdifiedNature);
                }

                if (desiredGender == ANY_GENDER || (int)GetGender(species.GenderRatio) == desiredGender)
                    correctGender = true;
                else
                {
                    correctGender = false;
                    continue;
                }

                if (desiredAbility == ANY_ABILITY || (this.GetHasSecondAbility() && desiredAbility == ABILITY2) || (!GetHasSecondAbility() && desiredAbility == ABILITY1))
                    correctAbility = true;
                else
                    correctAbility = false;
            } while (!(correctGender && correctAbility));
        }

        public void SetTraits(PokemonSpecies species, Gender gender, Nature nature, bool hasSecondAbility)
        {
            GenerateWithTraits(species, (int)gender, (int)nature, hasSecondAbility ? ABILITY2 : ABILITY1);
        }

        public void SetTraits(PokemonSpecies species, Gender gender, Nature nature)
        {
            GenerateWithTraits(species, (int)gender, (int)nature, ANY_ABILITY);
        }

        public void SetTraits(Nature nature, bool hasSecondAbility)
        {
            GenerateWithTraits(new PokemonSpecies(), ANY_GENDER, (int)nature, hasSecondAbility ? ABILITY2 : ABILITY1);
        }

        public void SetTraits(PokemonSpecies species, Gender gender, bool hasSecondAbility)
        {
            GenerateWithTraits(species, (int)gender, ANY_NATURE, hasSecondAbility ? ABILITY2 : ABILITY1);
        }

        public void SetTraits(Nature nature)
        {
            GenerateWithTraits(new PokemonSpecies(), ANY_GENDER, (int)nature, ANY_ABILITY);
        }

        public void SetTraits(PokemonSpecies species, Gender gender)
        {
            GenerateWithTraits(species, (int)gender, ANY_NATURE, ANY_ABILITY);
        }

        public void SetTraits(bool hasSecondAbility)
        {
            GenerateWithTraits(new PokemonSpecies(), ANY_GENDER, ANY_NATURE, hasSecondAbility ? ABILITY2 : ABILITY1);
        }

        public Gender GetGender(int genderRatio)
        {
            if (genderRatio == PokemonSpecies.GENDER_RATIO_GENDERLESS)
                return Gender.UNKNOWN;
            else
                return (PV & GENDER_RATIO_BIT_MASK) > genderRatio ? Gender.MALE : Gender.FEMALE;
        }

        public bool GetHasSecondAbility()
        {
            return (PV & ABILITY_BIT_MASK) == 1;
        }

        public Nature GetNature()
        {
            return (Nature)(PV % NUM_NATURES);
        }
    }
}
