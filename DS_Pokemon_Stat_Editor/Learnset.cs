using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pokemon_Sinjoh_Editor
{
    public class Learnset
    {
        private const ushort DELIMITER = 0XFFFF;
        private const ushort PADDING_END = 0x0;
        private const ushort MOVE_ID_BITMASK = 0b_0001_1111_1111;
        private const ushort LEVEL_BITMASK = 0b_1111_1110_0000_0000;
        private const int LEVEL_BITSHIFT = 9;
        private const int MAX_MOVE_ID = 511;
        private const int MAX_MOVE_LEVEL = 100;


        private List<ushort> moveIDs;
        public List<ushort> LevelsLearned;

        public int GetMoveID(int learnedMoveIndex) => moveIDs[learnedMoveIndex];
        public void SetMoveID(int learnedMoveIndex, int moveID)
        {
            moveIDs[learnedMoveIndex] = moveID <= MAX_MOVE_ID ? (ushort)moveID : (ushort)MAX_MOVE_ID;
        }

        public int GetNumMoves() => moveIDs.Count;

        public Learnset(MemoryStream learnedMovesStream)
        {
            ushort moveIDAndLevel;

            moveIDs = new List<ushort>();
            LevelsLearned = new List<ushort>();

            using (var learnedMovesReader = new BinaryReader(learnedMovesStream, Encoding.UTF8, true))
            {
                moveIDAndLevel = learnedMovesReader.ReadUInt16();

                while (moveIDAndLevel != DELIMITER)
                {
                    moveIDs.Add((ushort)(moveIDAndLevel & MOVE_ID_BITMASK));
                    LevelsLearned.Add((ushort)((moveIDAndLevel & LEVEL_BITMASK) >> LEVEL_BITSHIFT));
                    moveIDAndLevel = learnedMovesReader.ReadUInt16();
                }
            }

        }

        public MemoryStream GetBinary()
        {
            ushort moveIDAndLevel;
            MemoryStream learnsetStream = new MemoryStream();

            using (var learnsetWriter = new BinaryWriter(learnsetStream, Encoding.UTF8, true))
            {
                for (int i = 0; i < moveIDs.Count; i++)
                {
                    moveIDAndLevel = (ushort)(moveIDs[i] | (LevelsLearned[i] << LEVEL_BITSHIFT));
                    learnsetWriter.Write(moveIDAndLevel);
                }

                learnsetWriter.Write(DELIMITER);

                
                //number of bytes for each entry must be divisible by 4
                if (learnsetStream.Length % 4 != 0)
                    learnsetWriter.Write(PADDING_END);
            }

            return learnsetStream;
        }


    }
}
