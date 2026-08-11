//type of file used to store data within .nds format. NARC stands for Nitro ARChive
//
// code adapted from https://github.com/AdAstra-LD/DS-Pokemon-Rom-Editor/blob/main/DS_Map/Narc.cs

using System;
using System.Collections.Generic;
using System.IO;
using static Pokemon_Sinjoh_Editor.RomFile;

namespace Pokemon_Sinjoh_Editor
{
    public class NarcFile
    {
        private const uint NARC_FILE_MAGIC_NUM = 0x4352414E; //"NARC" in ascii/unicode
        private const uint NARC_FILE_SIGNATURE_END = 0x0100FFFE;
        private const uint FAT_SIGNATURE = 0x46415442;
        private const uint FILE_NAME_TABLE_SIGNATURE = 0x464E5442;
        private const uint FILE_IMAGE_SIGNATURE = 0x46494D47;
        private const ushort NARC_FILE_HEADER_SIZE_BYTES = 16;
        private const ushort NARC_FILE_HEADER_NUM_SECTIONS = 3;
        private const uint FAT_OFFSET = 0x10;
        private const uint FAT_NUM_ELEMENTS_OFFSET = 0x18;
        private const uint FAT_HEADER_LENGTH = 0xc;
        private const uint FAT_ELEMENT_LENGTH = 0x8;
        private const uint FILE_IMAGE_HEADER_LENGTH = 0x8;   
        private const uint FILE_NAME_TABLE_SIGNATURE_LENGTH = 0x4;

        private const int MOVES_INDEX_DP = 0x158;
        private const int MOVES_INDEX_PL = 0x1BD;
        private const int MOVES_INDEX_HGSS = 0x8C;
        private const int JP_MOVES_INDEX_DP = 0x159;
        private const int JP_MOVES_INDEX_PL = 0x1C3;
        private const int JP_MOVES_INDEX_HGSS = 0x8B;
        private const int KR_MOVES_INDEX_PL = 0x1A3;
        private const int KR_MOVES_INDEX_DP = 0x144;

        private const int POKEMON_SPECIES_INDEX_DIAMOND = 0x146;
        private const int POKEMON_SPECIES_INDEX_PEARL = 0x148;
        private const int POKEMON_SPECIES_INDEX_PL = 0x1A5;
        private const int POKEMON_SPECIES_INDEX_HGSS = 0x83;
        private const int JP_POKEMON_SPECIES_INDEX_D = 0x147;
        private const int JP_POKEMON_SPECIES_INDEX_P = 0x149;
        private const int JP_POKEMON_SPECIES_INDEX_PL = 0x1AB;
        private const int JP_POKEMON_SPECIES_INDEX_HGSS = 0x82;
        private const int KR_POKEMON_SPECIES_INDEX_PL = 0x18B;
        private const int KR_POKEMON_SPECIES_INDEX_D = 0x132;
        private const int KR_POKEMON_SPECIES_INDEX_P = 0x134;

        private const int POKEDEX_INDEX_DP = 0x5A; //same for japanese & korean
        private const int POKEDEX_INDEX_PL = 0x82; //same for japanese & korean, there is a seemingly identical narc at 0x81, but this one has gira in the name so it's likely the one platinum uses
        private const int POKEDEX_INDEX_HGSS = 0x157; //same for korean
        private const int JP_POKEDEX_INDEX_HGSS = 0x156;

        private const int NPC_TRADES_INDEX_DP = 0x10E;
        private const int NPC_TRADES_INDEX_PL = 0x150;
        private const int NPC_TRADES_INDEX_HGSS = 0xF1;
        private const int JP_NPC_TRADES_INDEX_PL = 0x151;
        private const int JP_NPC_TRADES_INDEX_HGSS = 0xF0;
        private const int KR_NPC_TRADES_INDEX_PL = 0x1BB;
        private const int KR_NPC_TRADES_INDEX_DP = 0x152;

        private const int TEXT_INDEX_DP = 0x13D;
        private const int TEXT_INDEX_PL = 0x194;
        private const int TEXT_INDEX_HGSS = 0x9C;
        private const int JP_TEXT_INDEX_DP = 0x13E;
        private const int JP_TEXT_INDEX_PL = 0x19A;
        private const int JP_TEXT_INDEX_HGSS = 0x9B;
        private const int KR_TEXT_INDEX_DP = 0x129;
        private const int KR_TEXT_INDEX_PL = 0x17A;

        private const int ITEMS_INDEX_DP = 0x13A;
        private const int ITEMS_INDEX_PL = 0x192;
        private const int ITEMS_INDEX_HGSS = 0x92;
        private const int JP_ITEMS_INDEX_DP = 0x13B;
        private const int JP_ITEMS_INDEX_PL = 0x198;
        private const int JP_ITEMS_INDEX_HGSS = 0x91;
        private const int KR_ITEMS_INDEX_DP = 0x126;
        private const int KR_ITEMS_INDEX_PL = 0x178;
        private const int KR_ITEMS_INDEX_HGSS = 0x92;

        private const int LEVEL_UP_MOVES_INDEX_DP = 0x148;
        private const int LEVEL_UP_MOVES_INDEX_PL = 0x1A7;
        private const int LEVEL_UP_MOVES_INDEX_HGSS = 0xA2;
        private const int JP_LEVEL_UP_MOVES_INDEX_DP = 0x149;
        private const int JP_LEVEL_UP_MOVES_INDEX_PL = 0x1AD;
        private const int JP_LEVEL_UP_MOVES_INDEX_HGSS = 0xA1;
        private const int KR_LEVEL_UP_MOVES_INDEX_DP = 0x134;
        private const int KR_LEVEL_UP_MOVES_INDEX_PL = 0x18D;

        private const int EGG_MOVES_INDEX_HGSS = 0x166;
        private const int JP_EGG_MOVES_INDEX_HGSS = 0x165;

        public const int POKEDEX_HEIGHT_ELEMENT_INDEX = 0;
        public const int POKEDEX_WEIGHT_ELEMENT_INDEX = 1;

        private FAT fat;
        private long narcFileOffset;
        uint numElements;
        uint FimgOffset;


        public List<MemoryStream> Elements { get; set; } = new List<MemoryStream>();

        public NarcFile(long narcFileOffset, BinaryReader narcReader)
        {
            this.narcFileOffset = narcFileOffset;

            uint FNTBOffset;

            narcReader.BaseStream.Position = narcFileOffset;
            if (narcReader.ReadUInt32() != NARC_FILE_MAGIC_NUM)
            {
                throw new Exception("Error! Narc sub-file expected at offset:" + narcFileOffset + "\nThe rom file's allocation table may be corrupted.\n");
            }

            narcReader.BaseStream.Position = narcFileOffset + FAT_NUM_ELEMENTS_OFFSET;
            numElements = narcReader.ReadUInt32();

            FNTBOffset = numElements * FAT_ELEMENT_LENGTH + FAT_OFFSET + FAT_HEADER_LENGTH;
            narcReader.BaseStream.Position = narcFileOffset + FNTBOffset + FILE_NAME_TABLE_SIGNATURE_LENGTH;
            FimgOffset = narcReader.ReadUInt32() + FNTBOffset;

            fat = new FAT(narcFileOffset + FAT_OFFSET + FAT_HEADER_LENGTH);
            fat.SetNumFilesForNarc(numElements);
            fat.Read(narcReader);

            readElements(narcReader);
        }


        private void readElements(BinaryReader reader)
        {
            byte[] buffer;
            for (int i = 0; i < numElements; i++)
            {
                reader.BaseStream.Position = FimgOffset + fat.GetStartOffset(i) + FILE_IMAGE_HEADER_LENGTH + narcFileOffset;
                buffer = new byte[fat.GetEndOffset(i) - fat.GetStartOffset(i)];
                reader.Read(buffer, 0, (int)(fat.GetEndOffset(i) - fat.GetStartOffset(i)));
                Elements.Add(new MemoryStream(buffer));
            }
        }

        public void UpdateElement(MemoryStream newElement, int index)
        {
            Elements[index] = newElement;
        }

        public void Write(BinaryWriter bw)
        {
            uint fileSizeOffset, fimgSizeOffset, curOffset;
            byte[] buffer;
            bw.BaseStream.Position = narcFileOffset;

            // Write NARC Section
            bw.Write(NARC_FILE_MAGIC_NUM);
            bw.Write(NARC_FILE_SIGNATURE_END);
            fileSizeOffset = (uint)bw.BaseStream.Position;
            bw.Write((UInt32)0x0);
            bw.Write(NARC_FILE_HEADER_SIZE_BYTES);
            bw.Write(NARC_FILE_HEADER_NUM_SECTIONS);
            // Write FATB Section
            bw.Write(FAT_SIGNATURE);
            bw.Write((UInt32)(FAT_HEADER_LENGTH + Elements.Count * FAT_ELEMENT_LENGTH));
            bw.Write((UInt32)Elements.Count);
            curOffset = 0;
            for (int i = 0; i < Elements.Count; i++)
            {
                while (curOffset % 4 != 0)
                {
                    curOffset++;     // Force offsets to be a multiple of 4
                }

                bw.Write(curOffset);
                curOffset += (uint)Elements[i].Length;
                bw.Write(curOffset);
            }
            // Write FNTB Section (No names, sorry =( )
            bw.Write(FILE_NAME_TABLE_SIGNATURE);
            bw.Write(0x10);             //FNTB Size
            bw.Write(0x4);              //the offset of the first name directory
            bw.Write(0x10000);          //filler data describing a file at position 0 with 1 directory in the archive
            // Write FIMG Section
            bw.Write(FILE_IMAGE_SIGNATURE);
            fimgSizeOffset = (uint)bw.BaseStream.Position;
            bw.Write((UInt32)0x0);
            curOffset = 0;

            for (int i = 0; i < Elements.Count; i++)
            {   
                // Data writing
                buffer = new byte[Elements[i].Length];
                Elements[i].Seek(0, SeekOrigin.Begin);
                Elements[i].Read(buffer, 0, (int)Elements[i].Length);
                bw.Write(buffer, 0, (int)Elements[i].Length);
                curOffset += (uint)Elements[i].Length;
            }
            // Writes sizes
            int fileSize = (int)(bw.BaseStream.Position - narcFileOffset);
            bw.Seek((int)fileSizeOffset, SeekOrigin.Begin);         // File size
            bw.Write((UInt32)fileSize);
            bw.Seek((int)fimgSizeOffset, SeekOrigin.Begin);         // seeks back to FIMG size
            bw.Write((UInt32)curOffset + FILE_IMAGE_HEADER_LENGTH);
        }

        public static uint GetMovesNarcOffset(FAT fat)
        {
            if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(JP_MOVES_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(JP_MOVES_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(JP_MOVES_INDEX_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(KR_MOVES_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(KR_MOVES_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(MOVES_INDEX_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(MOVES_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(MOVES_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(MOVES_INDEX_HGSS),
                    _ => 0
                };
            }

        }

        public static uint GetSpeciesNarcOffset(FAT fat, RomFile.GameVersions gameVersion)
        {
            if (Language == Languages.JAPANESE)
            {
                return gameVersion switch
                {
                    GameVersions.DIAMOND => fat.GetStartOffset(JP_POKEMON_SPECIES_INDEX_D),
                    GameVersions.PEARL => fat.GetStartOffset(JP_POKEMON_SPECIES_INDEX_P),
                    GameVersions.PLATINUM => fat.GetStartOffset(JP_POKEMON_SPECIES_INDEX_PL),
                    GameVersions.HEARTGOLD => fat.GetStartOffset(JP_POKEMON_SPECIES_INDEX_HGSS),
                    GameVersions.SOULSILVER => fat.GetStartOffset(JP_POKEMON_SPECIES_INDEX_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return gameVersion switch
                {
                    GameVersions.DIAMOND => fat.GetStartOffset(KR_POKEMON_SPECIES_INDEX_D),
                    GameVersions.PEARL => fat.GetStartOffset(KR_POKEMON_SPECIES_INDEX_P),
                    GameVersions.PLATINUM => fat.GetStartOffset(KR_POKEMON_SPECIES_INDEX_PL),
                    GameVersions.HEARTGOLD => fat.GetStartOffset(POKEMON_SPECIES_INDEX_HGSS),
                    GameVersions.SOULSILVER => fat.GetStartOffset(POKEMON_SPECIES_INDEX_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameVersion switch
                {
                    GameVersions.DIAMOND => fat.GetStartOffset(POKEMON_SPECIES_INDEX_DIAMOND),
                    GameVersions.PEARL => fat.GetStartOffset(POKEMON_SPECIES_INDEX_PEARL),
                    GameVersions.PLATINUM => fat.GetStartOffset(POKEMON_SPECIES_INDEX_PL),
                    GameVersions.HEARTGOLD => fat.GetStartOffset(POKEMON_SPECIES_INDEX_HGSS),
                    GameVersions.SOULSILVER => fat.GetStartOffset(POKEMON_SPECIES_INDEX_HGSS),
                    _ => 0
                };
            }

        }

        public static uint GetPokedexNarcOffset(FAT fat)
        {
            if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(POKEDEX_INDEX_DP), //same as all other langauges
                    GameFamilies.PL => fat.GetStartOffset(POKEDEX_INDEX_PL), //same as all other langauges
                    GameFamilies.HGSS => fat.GetStartOffset(JP_POKEDEX_INDEX_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(POKEDEX_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(POKEDEX_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(POKEDEX_INDEX_HGSS),
                    _ => 0
                };
            }
        }

        public static uint GetNPCTradesNarcOffset(FAT fat)
        {
            if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(NPC_TRADES_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(JP_NPC_TRADES_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(JP_NPC_TRADES_INDEX_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(KR_NPC_TRADES_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(KR_NPC_TRADES_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(NPC_TRADES_INDEX_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(NPC_TRADES_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(NPC_TRADES_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(NPC_TRADES_INDEX_HGSS),
                    _ => 0
                };
            }
        }

        public static uint GetItemsNarcOffset(FAT fat)
        {
            if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(JP_ITEMS_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(JP_ITEMS_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(JP_ITEMS_INDEX_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(KR_ITEMS_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(KR_ITEMS_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(KR_ITEMS_INDEX_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(ITEMS_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(ITEMS_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(ITEMS_INDEX_HGSS),
                    _ => 0
                };
            }
        }

        public static uint GetLearnsetNarcOffset(FAT fat)
        {
            if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(JP_LEVEL_UP_MOVES_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(JP_LEVEL_UP_MOVES_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(JP_LEVEL_UP_MOVES_INDEX_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(KR_LEVEL_UP_MOVES_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(KR_LEVEL_UP_MOVES_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(LEVEL_UP_MOVES_INDEX_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(LEVEL_UP_MOVES_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(LEVEL_UP_MOVES_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(LEVEL_UP_MOVES_INDEX_HGSS),
                    _ => 0
                };
            }
        }

        public static uint GetTextNarcOffset(FAT fat)
        {
            if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(JP_TEXT_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(JP_TEXT_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(JP_TEXT_INDEX_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(KR_TEXT_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(KR_TEXT_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(TEXT_INDEX_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(TEXT_INDEX_DP),
                    GameFamilies.PL => fat.GetStartOffset(TEXT_INDEX_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(TEXT_INDEX_HGSS),
                    _ => 0
                };
            }

        }

        public static uint GetEggMoveNarcOffset(FAT fat)
        {
            if (Language == Languages.JAPANESE)
                return fat.GetStartOffset(JP_EGG_MOVES_INDEX_HGSS);
            else
                return fat.GetStartOffset(EGG_MOVES_INDEX_HGSS);
        }
    }
}
