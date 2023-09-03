using System.IO;


namespace DS_Pokemon_Stat_Editor
{
    internal class FAT //File Allocation Table
    {
        private uint[] startOffsets;
        private uint[] endOffsets;
        private uint offsetInContainingFile;
        private uint numFiles;

        private const int ENTRY_LENGTH = 8;

        public FAT(uint offsetInContainingFile)
        {
            this.offsetInContainingFile = offsetInContainingFile;
        }

        public void SetNumFilesForNarc(uint numFiles)
        {
            this.numFiles = numFiles;
        }

        public void SetTotalLengthForRom(uint length)
        {
            numFiles = length / ENTRY_LENGTH;
        }

        public void Read(BinaryReader FATReader)
        {
            startOffsets = new uint[numFiles];
            endOffsets = new uint[numFiles];

            FATReader.BaseStream.Position = offsetInContainingFile;

            for (int i = 0; i < numFiles; i++)
            {
                startOffsets[i] = FATReader.ReadUInt32();
                endOffsets[i] = FATReader.ReadUInt32();
            }
        }

        public void Write(BinaryWriter FATWriter)
        {
            FATWriter.BaseStream.Position = offsetInContainingFile;
            for (int i = 0; i < numFiles; i++)
            {
                FATWriter.Write(startOffsets[i]);
                FATWriter.Write(endOffsets[i]);
            }
        }

        public uint GetStartOffset(int fileID) => startOffsets[fileID];
        public uint GetEndOffset(int fileID) => endOffsets[fileID];
    }
}
