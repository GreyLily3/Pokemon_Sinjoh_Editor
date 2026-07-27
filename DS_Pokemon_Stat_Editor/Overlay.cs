using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Pokemon_Sinjoh_Editor
{
    public class Overlay
    {
        public MemoryStream Content;
        private uint fileOffset;

        public const int MOVE_TUTOR_INDEX_PL = 5;
        public const int EGG_MOVE_INDEX_DPPL = 5;
        public const int NUM_OVERLAYS_DP = 87;
        public const int NUM_OVERLAYS_PL = 122;
        public const int NUM_OVERLAYS_JP_HGSS = 128;
        public const int NUM_OVERLAYS_NONJP_HGSS = 129;
        private const int FIRST_BIT_MASK = 1;
        private const int FIRST_BIT_ON = 1;
        public const int MOVE_TUTOR_LEARNSET_OFFSET_PL = 0x3012C;
        public const int MOVE_TUTOR_POOL_OFFSET_PL = 0x2FF64;

        private const uint EN_EGG_MOVES_OFFSET_PL = 0x29222;
        private const uint JP_EGG_MOVES_OFFSET_PL = 0x29012;
        private const uint KR_EGG_MOVES_OFFSET_PL = 0x2921A;
        private const uint FR_IT_EGG_MOVES_OFFSET_PL = 0x2922A;
        private const uint DE_EGG_MOVES_OFFSET_PL = 0x2923E;
        private const uint ES_EGG_MOVES_OFFSET_PL = 0x2922A;

        private const uint EN_EGG_MOVES_OFFSET_DP = 0x20668;
        private const uint JP_EGG_MOVES_OFFSET_DP = 0x21654;
        private const uint KR_EGG_MOVES_OFFSET_DP = 0x2061c;
        private const uint EURO_EGG_MOVES_OFFSET_DP = 0x20620;

        public const uint EGG_MOVES_LENGTH = 0xEEC;

        /// <summary>
        /// Reads an overlay into the "Content" memorystream
        /// </summary>
        public Overlay(FAT fat, int overlayNum, BinaryReader overlayReader)
        {
            fileOffset = fat.GetStartOffset(overlayNum);
            overlayReader.BaseStream.Position = fat.GetStartOffset(overlayNum);

            Content = new MemoryStream(overlayReader.ReadBytes((int)fat.GetFileSize(overlayNum)));
        }

        public Byte[] GetSubsetBytes(uint startPosition, uint length)
        {
            var subsetBytes = new Byte[length];
            Content.Position = startPosition;
            Content.Read(subsetBytes, 0, (int)length);
            return subsetBytes;
        }

        public MemoryStream GetSubsetStream(uint startPosition, uint length)
        {
            var subsetBytes = new Byte[length];
            Content.Position = startPosition;
            Content.Read(subsetBytes, 0, (int)length);
            return new MemoryStream(subsetBytes);
        }

        public void ReplaceSubset(MemoryStream subsetStream, uint startPosition)
        {
            Content.Position = startPosition;
            Content.Write(subsetStream.ToArray(), 0, (int)subsetStream.Length);
        }

        public void Write(BinaryWriter romWriter)
        {
            romWriter.BaseStream.Position = fileOffset;

            romWriter.Write(Content.ToArray());
        }

        public bool IsCompressed()
        {
            if (RomFile.gameFamily == RomFile.GameFamilies.PL)
                return false;
            else
            {
                using (BinaryReader compressionReader = new BinaryReader(Content, Encoding.UTF8, true))
                {
                    return (compressionReader.PeekChar() & FIRST_BIT_MASK) == FIRST_BIT_ON;
                }
            }
        }

        public static uint GetEggMovesOffset(RomFile.GameFamilies gameFamily, Languages gameLanguage)
        {
            switch (gameFamily)
            {
                case RomFile.GameFamilies.PL:
                    switch (gameLanguage)
                    {
                        case Languages.ENGLISH:
                            return EN_EGG_MOVES_OFFSET_PL;
                        case Languages.JAPANESE:
                            return JP_EGG_MOVES_OFFSET_PL;
                        case Languages.KOREAN:
                            return KR_EGG_MOVES_OFFSET_PL;
                        case Languages.GERMAN:
                            return DE_EGG_MOVES_OFFSET_PL;
                        case Languages.SPANISH:
                            return ES_EGG_MOVES_OFFSET_PL;
                        default:
                            return FR_IT_EGG_MOVES_OFFSET_PL;
                    }
                case RomFile.GameFamilies.DP:
                    switch (gameLanguage)
                    {
                        case Languages.ENGLISH:
                            return EN_EGG_MOVES_OFFSET_DP;
                        case Languages.JAPANESE:
                            return JP_EGG_MOVES_OFFSET_DP;
                        case Languages.KOREAN:
                            return KR_EGG_MOVES_OFFSET_DP;
                        default:
                            return EURO_EGG_MOVES_OFFSET_DP;
                    }
                default:
                    return uint.MaxValue; //placeholder, HGSS does not have egg moves in an overlay
            }
        }
    }
}
