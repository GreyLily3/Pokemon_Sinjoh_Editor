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
        public byte HP { get; set; }
        public byte Attack { get; set; }
        public byte Defense { get; set; }
        public byte Speed { get; set; }
        public byte SpecialAttack { get; set; }
        public byte SpecialDefense { get; set; }
        public byte Type1 { get; set; }
        public byte Type2 { get; set; }
        public byte CatchRate { get; set; }
        public byte BaseXP { get; set; }
        public byte EffortYield1 { get; set; }
        public byte EffortYield2 { get; set; }
        public ushort Item1 { get; set; }
        public ushort Item2 { get; set; }
        public byte GenderRatio { get; set; }
        public byte HatchStepsMultiplier { get; set; }
        public byte BaseHappiness { get; set; }
        public byte XPGroup { get; set; }
        public EggGroups EggGroup1 { get; set; }
        public EggGroups EggGroup2 { get; set; }
        public byte Ability1 { get; set; }
        public byte Ability2 { get; set; }
        public byte SafariRunChance { get; set; }
        public byte PokedexColor { get; set; }
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


        public PokemonSpecies(MemoryStream pokeData)
        {
            var pokeDataReader = new BinaryReader(pokeData);

            HP = pokeDataReader.ReadByte();
            Attack = pokeDataReader.ReadByte();
            Defense = pokeDataReader.ReadByte();
            Speed = pokeDataReader.ReadByte();
            SpecialAttack = pokeDataReader.ReadByte();
            SpecialDefense = pokeDataReader.ReadByte();

            Type1 = pokeDataReader.ReadByte();
            Type2 = pokeDataReader.ReadByte();

            CatchRate = pokeDataReader.ReadByte();
            BaseXP = pokeDataReader.ReadByte();

            EffortYield1 = pokeDataReader.ReadByte();
            EffortYield2 = pokeDataReader.ReadByte();

            Item1 = pokeDataReader.ReadUInt16();
            Item2 = pokeDataReader.ReadUInt16();

            GenderRatio = pokeDataReader.ReadByte();
            HatchStepsMultiplier = pokeDataReader.ReadByte();
            BaseHappiness = pokeDataReader.ReadByte();
            XPGroup = pokeDataReader.ReadByte();

            EggGroup1 = (EggGroups)pokeDataReader.ReadByte();
            EggGroup2 = (EggGroups)pokeDataReader.ReadByte();

            Ability1 = pokeDataReader.ReadByte();
            Ability2 = pokeDataReader.ReadByte();

            SafariRunChance = pokeDataReader.ReadByte();
            PokedexColor = pokeDataReader.ReadByte();

            pokeDataReader.BaseStream.Position += 2; //skip over 2 bytes of padding

            learnableTMsAndHMs = new BitArray(pokeDataReader.ReadBytes(TOTAL_TM_HM_BYTES));

            pokeDataReader.Dispose();
        }

        public MemoryStream GetBinaryData()
        {
            var binaryPokeData = new MemoryStream();
            var pokeDataWriter = new BinaryWriter(binaryPokeData);

            byte[] learnableTMsHMsBuffer = new byte[TOTAL_TM_HM_BYTES];
            learnableTMsAndHMs.CopyTo(learnableTMsHMsBuffer, 0);

            pokeDataWriter.Write(HP);
            pokeDataWriter.Write(Attack);
            pokeDataWriter.Write(Defense);
            pokeDataWriter.Write(Speed);
            pokeDataWriter.Write(SpecialAttack);
            pokeDataWriter.Write(SpecialDefense);

            pokeDataWriter.Write(Type1);
            pokeDataWriter.Write(Type2);

            pokeDataWriter.Write(CatchRate);
            pokeDataWriter.Write(BaseXP);

            pokeDataWriter.Write(EffortYield1);
            pokeDataWriter.Write(EffortYield2);

            pokeDataWriter.Write(Item1);
            pokeDataWriter.Write(Item2);

            pokeDataWriter.Write(GenderRatio);
            pokeDataWriter.Write(HatchStepsMultiplier);
            pokeDataWriter.Write(BaseHappiness);
            pokeDataWriter.Write(XPGroup);

            pokeDataWriter.Write((byte)EggGroup1);
            pokeDataWriter.Write((byte)EggGroup2);

            pokeDataWriter.Write(Ability1);
            pokeDataWriter.Write(Ability2);

            pokeDataWriter.Write(SafariRunChance);
            pokeDataWriter.Write(PokedexColor);

            pokeDataWriter.Write((ushort)0b_0000);

            pokeDataWriter.Write(learnableTMsHMsBuffer);

            //write 3 bytes of padding
            pokeDataWriter.Write((ushort)0b_0000);
            pokeDataWriter.Write((byte)0b_00);

            pokeDataWriter.Dispose();
            return binaryPokeData;
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
