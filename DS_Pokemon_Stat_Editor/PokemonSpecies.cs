using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Pokemon_Sinjoh_Editor.PokemonSpecies;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

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
        private ushort effortYield;
        public ushort Item1;
        public ushort Item2;
        public byte GenderRatio;
        public byte NumEggCyles;
        public byte BaseHappiness;
        public XPGroups XPGroup;
        public EggGroups EggGroup1;
        public EggGroups EggGroup2;
        public byte Ability1;
        public byte Ability2;
        public byte SafariRunChance;
        public byte PokedexColor;
        private BitArray learnableTMsAndHMs; //whether a pokemon can learn each TM and HM, ordered from TM01 to HM08

        private const int TOTAL_TM_HM_BYTES = 13;
        private const int TM_HM_TOTAL = 100;
        public const int TM_TOTAL = 92;
        public const int HM_TOTAL = 8;
        public const byte GENDER_RATIO_MALE_ONLY = 0;
        public const byte GENDER_RATIO_FEMALE_ONLY = 254;
        public const byte GENDER_RATIO_GENDERLESS = 255;

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

            effortYield = pokemonSpeciesReader.ReadUInt16();

            Item1 = pokemonSpeciesReader.ReadUInt16();
            Item2 = pokemonSpeciesReader.ReadUInt16();

            GenderRatio = pokemonSpeciesReader.ReadByte();
            NumEggCyles = pokemonSpeciesReader.ReadByte();
            BaseHappiness = pokemonSpeciesReader.ReadByte();
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

            pokemonSpeciesWriter.Write(effortYield);

            pokemonSpeciesWriter.Write(Item1);
            pokemonSpeciesWriter.Write(Item2);

            pokemonSpeciesWriter.Write(GenderRatio);
            pokemonSpeciesWriter.Write(NumEggCyles);
            pokemonSpeciesWriter.Write(BaseHappiness);
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

            pokemonSpeciesWriter.Dispose();
            return pokemonSpeciesBinary;
        }

        public int HPEVYield
        {
            get { return effortYield & HP_EV_MASK; }
            set { effortYield = (ushort)((value & HP_EV_MASK) & (effortYield | HP_EV_MASK)); }
        }

        public int AttackEVYield
        {
            get { return (effortYield & ATTACK_EV_MASK) >> ATTACK_EV_BIT_SHIFT; }
            set { effortYield = (ushort)((value & ATTACK_EV_MASK) & (effortYield | ATTACK_EV_MASK)); }
        }

        public int DefenseEVYield
        {
            get { return (effortYield & DEFENSE_EV_MASK) >> DEFENSE_EV_BIT_SHIFT; }
            set { effortYield = (ushort)((value & DEFENSE_EV_MASK) & (effortYield | DEFENSE_EV_MASK)); }
        }

        public int SpecialAttackEVYield
        {
            get { return (effortYield & SPECIAL_ATTACK_EV_MASK) >> SPECIAL_ATTACK_EV_BIT_SHIFT; }
            set { effortYield = (ushort)((value & SPECIAL_ATTACK_EV_MASK) & (effortYield | SPECIAL_ATTACK_EV_MASK)); }
        }

        public int SpecialDefenseEVYield
        {
            get { return (effortYield & SPECIAL_DEFENSE_EV_MASK) >> SPECIAL_DEFENSE_EV_BIT_SHIFT; }
            set { effortYield = (ushort)((value & SPECIAL_DEFENSE_EV_MASK) & (effortYield | SPECIAL_DEFENSE_EV_MASK)); }
        }

        public int SpeedEVYield
        {
            get { return (effortYield & SPEED_EV_MASK) >> SPEED_EV_BIT_SHIFT; }
            set { effortYield = (ushort)((value & SPEED_EV_MASK) & (effortYield | SPEED_EV_MASK)); }
        }

        /*
        public int[] GetLearnableTMsAndHMs()
        {
            int[] LearnableTMAndHMIndexes = new int[TM_HM_TOTAL];

            for (int i = 0; i < TM_HM_TOTAL; i++)
            {
                if (learnableTMsAndHMs[i])
                {
                    LearnableTMAndHMIndexes.Add(i);
                }
            }

            return LearnableTMAndHMIndexes;
        }
        */

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

        public void SetLearnableTMsAndHMs(int[] TMAndHMIndexes)
        {
            learnableTMsAndHMs.SetAll(false);

            foreach (int TMOrHMIndex in TMAndHMIndexes)
                learnableTMsAndHMs.Set(TMOrHMIndex, true);
        }

        public void setAllTMsAndHMs(bool TMsAndHMsLearnable) => learnableTMsAndHMs.SetAll(TMsAndHMsLearnable);
    }
}
