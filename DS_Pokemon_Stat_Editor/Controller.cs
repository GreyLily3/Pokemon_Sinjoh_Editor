using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS_Pokemon_Stat_Editor
{
    internal static class Controller
    {
        static public MainForm View;
        static private RomFile currentRom;
        static private string currentRomFilePath;

        public static void InitializeView()
        {
            View = new MainForm();
        }

        public static void LoadRom(string romFilePath)
        {
            currentRomFilePath = romFilePath;
            using BinaryReader romReader = new BinaryReader(new FileStream(currentRomFilePath, FileMode.Open));
            currentRom = new RomFile(romReader);

            if (!currentRom.IsValidGameVersion())
                MessageBox.Show("File selected is not a valid DS pokemon rom. It will not be loaded.");
            else if (!currentRom.IsSupportedGameVersion())
                MessageBox.Show("Pokemon Black/White and Black2/White2 roms are not supported due to significant differences in data structures from Gen 4.");
            else
            {
                currentRom.Read(romReader);
                View.IncludeGameVersionInText(currentRom.getGameVersion());
            }

        }

        public static Move GetMove(int moveIndex) => currentRom.GetMove(moveIndex);
        public static void UpdateMove(int moveIndex, Move UpdatedMove) => currentRom.UpdateMove(moveIndex, UpdatedMove);

        public static string[] GetTypeNames() => currentRom.TypeNames.ToArray();

        public static string[] GetMoveNames() => currentRom.MoveNames.ToArray();

        public static string[] GetPokemonSpeciesNames() => currentRom.PokemonNames.ToArray();

        public static string[] GetItemNames() => currentRom.ItemNames.ToArray();

        public static string[] GetAbilityNames() => currentRom.AbilityNames.ToArray();

        public static string[] GetMoveCategories() => Enum.GetNames(typeof(Move.Categories));
        public static string[] GetMoveContestConditions() => Enum.GetNames(typeof(Move.ContestConditions));
        public static string[] GetMoveTargets() => Enum.GetNames(typeof(Move.Targets));
    }
}
