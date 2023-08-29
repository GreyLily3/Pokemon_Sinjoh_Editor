using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DS_Pokemon_Stat_Editor.PokemonSpecies;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace DS_Pokemon_Stat_Editor
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
        public byte EffortYield1;
        public byte EffortYield2;
        public ushort Item1;
        public ushort Item2;
        public byte GenderRatio;
        public byte HatchStepsMultiplier;
        public byte BaseHappiness;
        public byte XPGroup;
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

        public enum EggGroups
        {
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

            EffortYield1 = pokemonSpeciesReader.ReadByte();
            EffortYield2 = pokemonSpeciesReader.ReadByte();

            Item1 = pokemonSpeciesReader.ReadUInt16();
            Item2 = pokemonSpeciesReader.ReadUInt16();

            GenderRatio = pokemonSpeciesReader.ReadByte();
            HatchStepsMultiplier = pokemonSpeciesReader.ReadByte();
            BaseHappiness = pokemonSpeciesReader.ReadByte();
            XPGroup = pokemonSpeciesReader.ReadByte();

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

            pokemonSpeciesWriter.Write(EffortYield1);
            pokemonSpeciesWriter.Write(EffortYield2);

            pokemonSpeciesWriter.Write(Item1);
            pokemonSpeciesWriter.Write(Item2);

            pokemonSpeciesWriter.Write(GenderRatio);
            pokemonSpeciesWriter.Write(HatchStepsMultiplier);
            pokemonSpeciesWriter.Write(BaseHappiness);
            pokemonSpeciesWriter.Write(XPGroup);

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

        public List<int> getLearnableTMsAndHMs()
        {
            List<int> LearnableTMAndHMIndexes = new List<int>();

            for (int i = 0; i < TM_HM_TOTAL; i++)
            {
                if (learnableTMsAndHMs[i])
                {
                    LearnableTMAndHMIndexes.Add(i);
                }
            }

            return LearnableTMAndHMIndexes;
        }

        public void setLearnableTMsAndHMs(int[] TMAndHMIndexes)
        {
            learnableTMsAndHMs.SetAll(false);

            foreach (int TMOrHMIndex in TMAndHMIndexes)
                learnableTMsAndHMs.Set(TMOrHMIndex, true);
        }

        public void setAllTMsAndHMs(bool TMsAndHMsLearnable) => learnableTMsAndHMs.SetAll(TMsAndHMsLearnable);
    }
}
