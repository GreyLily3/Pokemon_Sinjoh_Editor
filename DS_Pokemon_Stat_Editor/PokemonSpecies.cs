using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;


namespace Pokemon_Sinjoh_Editor
{
    public class PokemonSpecies
    {
        public byte HP;
        public byte Attack;
        public byte Defense;
        public byte Speed;
        public byte SpecialAttack;
        public byte SpecialDefense;
        public byte Type1;
        public byte Type2;
        public byte CatchRate;
        public byte BaseXP;
        private byte hpEVYield;
        private byte attackEVYield;
        private byte defenseEVYield;
        private byte specialAttackEVYield;
        private byte specialDefenseEVYield;
        private byte speedEVYield;
        public ushort Item1;
        public ushort Item2;
        private byte genderRatio;
        public byte NumEggCyles;
        public byte BaseFriendship;
        public XPGroups XPGroup;
        public EggGroups EggGroup1;
        public EggGroups EggGroup2;
        public byte Ability1;
        public byte Ability2;
        public byte SafariRunChance;
        public byte PokedexColor;
        private BitArray learnableTMsAndHMs; //whether a pokemon can learn each TM and HM, ordered from TM01 to HM08

        private const int TOTAL_TM_HM_BYTES = 13;
        public const int TM_TOTAL = 92;
        public const int HM_TOTAL = 8;
        public const byte GENDER_RATIO_MALE_ONLY = 0;
        public const byte GENDER_RATIO_FEMALE_ONLY = 254;
        public const byte GENDER_RATIO_GENDERLESS = 255;
        public const byte GENDER_RATIO_50_PERCENT = 127;
        public const byte MAX_EV_YIELD = 3;

        private const ushort HP_EV_MASK = 0b_0000_0000_0000_0011;
        private const ushort ATTACK_EV_MASK = 0b_0000_0000_0000_1100;
        private const ushort DEFENSE_EV_MASK = 0b_0000_0000_0011_0000;
        private const ushort SPEED_EV_MASK = 0b_0000_0000_1100_0000;
        private const ushort SPECIAL_ATTACK_EV_MASK = 0b_0000_0011_0000_0000;
        private const ushort SPECIAL_DEFENSE_EV_MASK = 0b_0000_1100_0000_0000;

        private const int ATTACK_EV_BIT_SHIFT = 2;
        private const int DEFENSE_EV_BIT_SHIFT = 4;
        private const int SPEED_EV_BIT_SHIFT = 6;
        private const int SPECIAL_ATTACK_EV_BIT_SHIFT = 8;
        private const int SPECIAL_DEFENSE_EV_BIT_SHIFT = 10;

        public const int HGSS_NUM_ALT_FORMS = 12;
        public const int PL_NUM_ALT_FORMS = 12;
        public const int DP_NUM_ALT_FORMS = 5;
        public const int EGG_SPECIES_INDEX = 493; //1 less than actual game value because we remove the 0th placeholder entry
        public const int BAD_EGG_SPECIES_INDEX = 494; //1 less than actual game value because we remove the 0th placeholder entry

        public enum EggGroups
        {
            NULL,
            MONSTER,
            WATER1,
            BUG,
            FLYING,
            FIELD,
            FAIRY,
            GRASS,
            HUMAN_LIKE,
            WATER3,
            MINERAL,
            AMORPHOUS,
            WATER2,
            DITTO,
            DRAGON,
            NO_EGG_GROUP
        }

        public enum XPGroups
        {
            MEDIUM_FAST,
            ERRATIC,
            FLUCTUATING,
            MEDIUM_SLOW,
            FAST,
            SLOW,
            UNUSED1,
            UNUSED2
        }


        public PokemonSpecies(MemoryStream pokemonSpecies)
        {
            var pokemonSpeciesReader = new BinaryReader(pokemonSpecies);

            HP = pokemonSpeciesReader.ReadByte();
            Attack = pokemonSpeciesReader.ReadByte();
            Defense = pokemonSpeciesReader.ReadByte();
            Speed = pokemonSpeciesReader.ReadByte();
            SpecialAttack = pokemonSpeciesReader.ReadByte();
            SpecialDefense = pokemonSpeciesReader.ReadByte();

            Type1 = pokemonSpeciesReader.ReadByte();
            Type2 = pokemonSpeciesReader.ReadByte();

            CatchRate = pokemonSpeciesReader.ReadByte();
            BaseXP = pokemonSpeciesReader.ReadByte();

            ushort evYields = pokemonSpeciesReader.ReadUInt16();

            hpEVYield = (byte)(evYields & HP_EV_MASK);
            attackEVYield = (byte)((evYields & ATTACK_EV_MASK) >> ATTACK_EV_BIT_SHIFT);
            defenseEVYield = (byte)((evYields & DEFENSE_EV_MASK) >> DEFENSE_EV_BIT_SHIFT);
            speedEVYield = (byte)((evYields & SPEED_EV_MASK) >> SPEED_EV_BIT_SHIFT);
            specialAttackEVYield = (byte)((evYields & SPECIAL_ATTACK_EV_MASK) >> SPECIAL_ATTACK_EV_BIT_SHIFT);
            specialDefenseEVYield = (byte)((evYields & SPECIAL_DEFENSE_EV_MASK) >> SPECIAL_DEFENSE_EV_BIT_SHIFT);

            Item1 = pokemonSpeciesReader.ReadUInt16();
            Item2 = pokemonSpeciesReader.ReadUInt16();

            genderRatio = pokemonSpeciesReader.ReadByte();
            NumEggCyles = pokemonSpeciesReader.ReadByte();
            BaseFriendship = pokemonSpeciesReader.ReadByte();
            XPGroup = (XPGroups)pokemonSpeciesReader.ReadByte();

            EggGroup1 = (EggGroups)pokemonSpeciesReader.ReadByte();
            EggGroup2 = (EggGroups)pokemonSpeciesReader.ReadByte();

            Ability1 = pokemonSpeciesReader.ReadByte();
            Ability2 = pokemonSpeciesReader.ReadByte();

            SafariRunChance = pokemonSpeciesReader.ReadByte();
            PokedexColor = pokemonSpeciesReader.ReadByte();

            pokemonSpeciesReader.BaseStream.Position += 2; //skip over 2 bytes of padding

            learnableTMsAndHMs = new BitArray(pokemonSpeciesReader.ReadBytes(TOTAL_TM_HM_BYTES));

            pokemonSpeciesReader.Dispose();
        }

        public MemoryStream GetBinary()
        {
            var pokemonSpeciesBinary = new MemoryStream();
            var pokemonSpeciesWriter = new BinaryWriter(pokemonSpeciesBinary);

            byte[] learnableTMsHMsBuffer = new byte[TOTAL_TM_HM_BYTES];
            learnableTMsAndHMs.CopyTo(learnableTMsHMsBuffer, 0);

            pokemonSpeciesWriter.Write(HP);
            pokemonSpeciesWriter.Write(Attack);
            pokemonSpeciesWriter.Write(Defense);
            pokemonSpeciesWriter.Write(Speed);
            pokemonSpeciesWriter.Write(SpecialAttack);
            pokemonSpeciesWriter.Write(SpecialDefense);

            pokemonSpeciesWriter.Write(Type1);
            pokemonSpeciesWriter.Write(Type2);

            pokemonSpeciesWriter.Write(CatchRate);
            pokemonSpeciesWriter.Write(BaseXP);

            ushort evYields;
            evYields = hpEVYield;
            evYields |= (ushort)(attackEVYield << ATTACK_EV_BIT_SHIFT);
            evYields |= (ushort)(defenseEVYield << DEFENSE_EV_BIT_SHIFT);
            evYields |= (ushort)(speedEVYield << SPEED_EV_BIT_SHIFT);
            evYields |= (ushort)(specialAttackEVYield << SPECIAL_ATTACK_EV_BIT_SHIFT);
            evYields |= (ushort)(specialDefenseEVYield << SPECIAL_DEFENSE_EV_BIT_SHIFT);

            pokemonSpeciesWriter.Write(evYields);

            pokemonSpeciesWriter.Write(Item1);
            pokemonSpeciesWriter.Write(Item2);

            pokemonSpeciesWriter.Write(genderRatio);
            pokemonSpeciesWriter.Write(NumEggCyles);
            pokemonSpeciesWriter.Write(BaseFriendship);
            pokemonSpeciesWriter.Write((byte)XPGroup);

            pokemonSpeciesWriter.Write((byte)EggGroup1);
            pokemonSpeciesWriter.Write((byte)EggGroup2);

            pokemonSpeciesWriter.Write(Ability1);
            pokemonSpeciesWriter.Write(Ability2);

            pokemonSpeciesWriter.Write(SafariRunChance);
            pokemonSpeciesWriter.Write(PokedexColor);

            pokemonSpeciesWriter.Write((ushort)0b_0000);

            pokemonSpeciesWriter.Write(learnableTMsHMsBuffer);

            //write 3 bytes of padding
            pokemonSpeciesWriter.Write((ushort)0b_0000);
            pokemonSpeciesWriter.Write((byte)0b_00);

            return pokemonSpeciesBinary;
        }

        public void SetGenderlessGenderRatio() => genderRatio = GENDER_RATIO_GENDERLESS;
        public void SetFemaleOnlyGenderRatio() => genderRatio = GENDER_RATIO_FEMALE_ONLY;
        public void SetMaleOnlyGenderRatio() => genderRatio = GENDER_RATIO_MALE_ONLY;
        public bool GetIsGenderless() => genderRatio == GENDER_RATIO_GENDERLESS;
        public bool GetIsFemaleOnly() => genderRatio == GENDER_RATIO_FEMALE_ONLY;
        public bool GetIsMaleOnly() => genderRatio == GENDER_RATIO_MALE_ONLY;
        public static byte Get50PercentGenderRatio() => GENDER_RATIO_50_PERCENT;

        public int GenderRatio { get => genderRatio; set { genderRatio = (byte)(value < GENDER_RATIO_FEMALE_ONLY ? value : GENDER_RATIO_FEMALE_ONLY - 1); } }

        public byte HPEVYield
        {
            get { return hpEVYield; }
            set { hpEVYield = value <= MAX_EV_YIELD ? value : MAX_EV_YIELD; }
        }

        public byte AttackEVYield
        {
            get { return attackEVYield; }
            set { attackEVYield = value <= MAX_EV_YIELD ? value : MAX_EV_YIELD; }
        }

        public byte DefenseEVYield
        {
            get { return defenseEVYield; }
            set { defenseEVYield = value <= MAX_EV_YIELD ? value : MAX_EV_YIELD; }
        }
        public byte SpecialAttackEVYield
        {
            get { return specialAttackEVYield; }
            set { specialAttackEVYield = value <= MAX_EV_YIELD ? value : MAX_EV_YIELD; }
        }

        public byte SpecialDefenseEVYield
        {
            get { return specialDefenseEVYield; }
            set { specialDefenseEVYield = value <= MAX_EV_YIELD ? value : MAX_EV_YIELD; }
        }

        public byte SpeedEVYield
        {
            get { return speedEVYield; }
            set { speedEVYield = value <= MAX_EV_YIELD ? value : MAX_EV_YIELD; }
        }

        public List<int> GetLearnableHMs()
        {
            List<int> learnableHMs = new List<int>(HM_TOTAL);

            for (int i = 0; i < HM_TOTAL; i++)
            {
                if (learnableTMsAndHMs[TM_TOTAL + i])
                    learnableHMs.Add(i);
            }

            return learnableHMs;
        }

        public List<int> GetLearnableTMs()
        {
            List<int> learnableTMs = new List<int>(TM_TOTAL);

            for (int i = 0; i < TM_TOTAL; i++)
            {
                if (learnableTMsAndHMs[i])
                    learnableTMs.Add(i);
            }

            return learnableTMs;
        }

        public void SetLearnableTM(int tmIndex, bool isLearnable)
        {
            learnableTMsAndHMs[tmIndex] = isLearnable;
        }

        public void SetLearnableHM(int hmIndex, bool isLearnable)
        {
            learnableTMsAndHMs[hmIndex + TM_TOTAL] = isLearnable;
        }

        public void setAllTMsAndHMs(bool TMsAndHMsLearnable) => learnableTMsAndHMs.SetAll(TMsAndHMsLearnable);
    }
}
