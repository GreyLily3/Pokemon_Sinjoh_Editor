using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pokemon_Sinjoh_Editor
{
    public class MoveTutorTable
    {
        private BitArray learnableMoves;

        public const int NUM_MOVES_PL = 38;
        public const int NUM_MOVES_HGSS = 52;
        public const int BYTES_PER_SPECIES_PL = 5;
        public const int BYTES_PER_SPECIES_HGSS = 8;
        public const int DEOXYS_ALT_FORMS_OFFSET = 2; //the 3 deoxys alt forms all share the same table row, so we need to offset every alt form after
        public readonly static int[] MoveIDsDPPL = new int[] { 291,
            189,
            210,
            196,
            205,
            9,
            7,
            276,
            8,
            442,
            401,
            466,
            380,
            173,
            180,
            314,
            270,
            283,
            200,
            246,
            235,
            324,
            428,
            410,
            414,
            441,
            239,
            402,
            334,
            393,
            387,
            340,
            271,
            257,
            282,
            389,
            129,
            253 };

        public readonly static int[] MoveIDsHGSS = new int[] { 291,
            189,
            210,
            196,
            205,
            9,
            7,
            276,
            8,
            442,
            401,
            466,
            380,
            173,
            180,
            314,
            270,
            283,
            200,
            246,
            235,
            324,
            428,
            410,
            414,
            441,
            239,
            402,
            334,
            393,
            387,
            340,
            271,
            257,
            282,
            389,
            129,
            253,
            162,
            220,
            81,
            366,
            356,
            388,
            277,
            272,
            215,
            67,
            143,
            335,
            450,
            29};

        public MoveTutorTable(MemoryStream moveTutorStream)
        {
            using (var moveTutorReader = new BinaryReader(moveTutorStream, Encoding.UTF8, true))
            {
                moveTutorReader.BaseStream.Position = 0;
                learnableMoves = new BitArray(moveTutorReader.ReadBytes((int)moveTutorStream.Length));
            }
        }

        public List<int> GetLearnableMoves(RomFile.GameFamilies gameFamily)
        {
            List<int> learnableMoveIDs = new List<int>(NUM_MOVES_HGSS);
            int numMoves;

            if (gameFamily == RomFile.GameFamilies.HGSS)
                numMoves = NUM_MOVES_HGSS;
            else
                numMoves = NUM_MOVES_PL;

                for (int i = 0; i < numMoves; i++)
                {
                    if (learnableMoves[i])
                        learnableMoveIDs.Add(i);
                }

            return learnableMoveIDs;
        }

        public void SetLearnableMove(int moveID, bool isLearnable)
        {
            learnableMoves[moveID] = isLearnable;
        }

        public static string[] GetTutorMoveNamesPL()
        {
            string[] moveNameList = new string[NUM_MOVES_PL];

            for (int i = 0; i < NUM_MOVES_PL; i++)
                moveNameList[i] = RomFile.MoveNames[MoveIDsDPPL[i] - Move.STARTING_INDEX];

            return moveNameList;
        }

        public static string[] GetTutorMoveNamesHGSS()
        {
            string[] moveNameList = new string[NUM_MOVES_HGSS];

            for (int i = 0; i < NUM_MOVES_HGSS; i++)
                moveNameList[i] = RomFile.MoveNames[MoveIDsHGSS[i] - Move.STARTING_INDEX];

            return moveNameList;
        }

    }
}
