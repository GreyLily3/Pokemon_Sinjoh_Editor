using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Sinjoh_Editor
{
    public class NPCTrade
    {
        public ushort OfferedPokemon;
        public ushort WantedPokemon;
        public byte HPIV;
        public byte AttackIV;
        public byte DefenseIV;
        public byte SpeedIV;
        public byte SpecialAttckIV;
        public byte SpecialDefenseIV;
        public byte Ability;
        public ushort OriginalTrainerID;
        public ushort HeldItem;
        public Languages LanguageOfOrigin;
        public WantedGender Gender;
        public PersonalityValue PersonalityValue;
        public byte Sheen;
        public byte Cool;
        public byte Beauty;
        public byte Cute;
        public byte Smart;
        public byte Tough;

        public enum Languages
        {
            JAPANESE = 1,
            ENGLISH,
            FRENCH,
            ITALIAN,
            GERMAN,
            UNKNOWN,
            SPANISH,
            KOREAN
        }

        public enum WantedGender
        {
            ANY,
            FEMALE,
            MALE
        }

        public NPCTrade(MemoryStream npcTrade)
        {
            var tradeBinaryReader = new BinaryReader(npcTrade, Encoding.UTF8, true);
            OfferedPokemon = (ushort)tradeBinaryReader.ReadUInt32();

            HPIV = (byte)tradeBinaryReader.ReadUInt32();
            AttackIV = (byte)tradeBinaryReader.ReadUInt32();
            DefenseIV = (byte)tradeBinaryReader.ReadUInt32();
            SpeedIV = (byte)tradeBinaryReader.ReadUInt32();
            SpecialAttckIV = (byte)tradeBinaryReader.ReadUInt32();
            SpecialDefenseIV = (byte)tradeBinaryReader.ReadUInt32();

            Ability = (byte)tradeBinaryReader.ReadUInt32();
            OriginalTrainerID = (ushort)tradeBinaryReader.ReadUInt32();

            Cool = (byte)tradeBinaryReader.ReadUInt32();
            Beauty = (byte)tradeBinaryReader.ReadUInt32();
            Cute = (byte)tradeBinaryReader.ReadUInt32();
            Smart = (byte)tradeBinaryReader.ReadUInt32();
            Tough = (byte)tradeBinaryReader.ReadUInt32();

            this.PersonalityValue = new PersonalityValue(tradeBinaryReader.ReadUInt32());
            HeldItem = (ushort)tradeBinaryReader.ReadUInt32();
            Gender = (WantedGender)tradeBinaryReader.ReadUInt32();
            Sheen = (byte)tradeBinaryReader.ReadUInt32();
            LanguageOfOrigin = (Languages)tradeBinaryReader.ReadUInt32();
            WantedPokemon = (ushort)tradeBinaryReader.ReadUInt32();
        }

        public MemoryStream GetBinary()
        {
            var tradeBinary = new MemoryStream();
            using (var tradeBinaryWriter = new BinaryWriter(tradeBinary, Encoding.UTF8, true))
            {
                tradeBinaryWriter.Write((uint)OfferedPokemon);

                tradeBinaryWriter.Write((uint)HPIV);
                tradeBinaryWriter.Write((uint)AttackIV);
                tradeBinaryWriter.Write((uint)DefenseIV);
                tradeBinaryWriter.Write((uint)SpeedIV);
                tradeBinaryWriter.Write((uint)SpecialAttckIV);
                tradeBinaryWriter.Write((uint)SpecialDefenseIV);

                tradeBinaryWriter.Write((uint)Ability);
                tradeBinaryWriter.Write((uint)OriginalTrainerID);

                tradeBinaryWriter.Write((uint)Cool);
                tradeBinaryWriter.Write((uint)Beauty);
                tradeBinaryWriter.Write((uint)Cute);
                tradeBinaryWriter.Write((uint)Smart);
                tradeBinaryWriter.Write((uint)Tough);

                tradeBinaryWriter.Write(PersonalityValue.PV);
                tradeBinaryWriter.Write((uint)HeldItem);
                tradeBinaryWriter.Write((uint)Gender);
                tradeBinaryWriter.Write((uint)Sheen);
                tradeBinaryWriter.Write((uint)LanguageOfOrigin);
                tradeBinaryWriter.Write((uint)WantedPokemon);
                tradeBinaryWriter.Write((uint)0); //filler
            }

            return tradeBinary;
        }

        
    }
}
