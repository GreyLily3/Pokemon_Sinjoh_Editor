using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Pokemon_Stat_Editor
{
    internal class RomFile
    {
        NarcFile movesNarc;
        NarcFile pokemonSpeciesNarc;
        TextArchive gameText;
        GameVersion gameVersion;
        GameFamily gameFamily;

        public enum GameVersion
        {
            DIADMOND,
            PEARL,
            PLATINUM,
            HEARTGOLD,
            SOULSILVER
        }

        public enum GameFamily
        {
            DP,
            PL,
            HGSS
        }


    }
}
