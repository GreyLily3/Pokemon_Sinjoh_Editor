//type of file used to store data within .nds format. NARC stands for Nitro ARChive
//
// code adapted from https://github.com/AdAstra-LD/DS-Pokemon-Rom-Editor/blob/main/DS_Map/Narc.cs

using System;
using System.Collections.Generic;
using System.IO;

namespace Pokemon_Sinjoh_Editor
{
    internal class NarcFile
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

        private FAT fat;
        private long narcFileOffset;
        uint numElements;
        uint FimgOffset;


        public List<MemoryStream> Elements { get; set; } = new List<MemoryStream>();

        public NarcFile(long narcFileOffset)
        {
            this.narcFileOffset = narcFileOffset;
        }

        public void Read(BinaryReader reader)
        {
            uint FNTBOffset;

            reader.BaseStream.Position = narcFileOffset;
            if (reader.ReadUInt32() != NARC_FILE_MAGIC_NUM)
            {
                throw new Exception("Error! Narc sub-file expected at offset:" + narcFileOffset + "\nThe rom file's allocation table may be corrupted.\n");
            }

            reader.BaseStream.Position = narcFileOffset + FAT_NUM_ELEMENTS_OFFSET;
            numElements = reader.ReadUInt32();

            FNTBOffset = numElements * FAT_ELEMENT_LENGTH + FAT_OFFSET + FAT_HEADER_LENGTH;
            reader.BaseStream.Position = narcFileOffset + FNTBOffset + FILE_NAME_TABLE_SIGNATURE_LENGTH;
            FimgOffset = reader.ReadUInt32() + FNTBOffset;

            fat = new FAT(narcFileOffset + FAT_OFFSET + FAT_HEADER_LENGTH);
            fat.SetNumFilesForNarc(numElements);
            fat.Read(reader);

            readElements(reader);

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
                while (curOffset % 4 != 0)
                { // Force offsets to be a multiple of 4
                    bw.Write((Byte)0xFF); curOffset++;
                }
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
    }
}
