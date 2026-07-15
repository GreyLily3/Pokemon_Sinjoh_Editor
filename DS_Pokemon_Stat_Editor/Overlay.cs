using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pokemon_Sinjoh_Editor
{
    public class Overlay
    {
        public MemoryStream Content;

        public const int MOVE_TUTOR_OVERLAY_INDEX_PL = 5;
        public const int NUM_OVERLAYS_DP = 87;
        public const int NUM_OVERLAYS_PL = 122;
        public const int NUM_OVERLAYS_JP_HGSS = 128;
        public const int NUM_OVERLAYS_NONJP_HGSS = 129;
        private const int FIRST_BIT_MASK = 1;
        private const int FIRST_BIT_ON = 1;
        public const int MOVE_TUTOR_LEARNSET_OFFSET_PL = 0x3012C;
        public const int MOVE_TUTOR_POOL_OFFSET_PL = 0x2FF64;

        /// <summary>
        /// Reads an overlay into the "Content" memorystream
        /// </summary>
        public Overlay(FAT fat, int overlayNum, BinaryReader overlayReader)
        {
            overlayReader.BaseStream.Position = fat.GetStartOffset(overlayNum);

            Content = new MemoryStream(overlayReader.ReadBytes((int)fat.GetFileSize(overlayNum)));
        }

        //public MemoryStream GetOverlaySubset(int startPosition, int length)
        //{
        //    using (var overlayReader = new BinaryReader(Content, Encoding.UTF8, true))
        //    {
        //        overlayReader.BaseStream.Position = startPosition;
        //        return new MemoryStream(overlayReader.ReadBytes(length));
        //    }
        //}

        public Byte[] GetOverlaySubset(uint startPosition, uint length)
        {
            var subsetBytes = new Byte[length];
            Content.Read(subsetBytes, (int)startPosition, (int)length);
            return subsetBytes;
        }

        private bool IsCompressed()
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
    }
}
