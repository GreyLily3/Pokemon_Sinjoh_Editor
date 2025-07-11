using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DS_Pokemon_Stat_Editor
{
	public static class RomFile
	{
		private static NarcFile movesNarc;
		private static NarcFile pokemonSpeciesNarc;
		private static NarcFile gameTextNarc;
		private static TextArchive gameText;
		private static GameVersions gameVersion;
        private static GameFamilies gameFamily;

		public static List<Move> MoveList = new List<Move>();
        private static List<PokemonSpecies> PokemonSpeciesList = new List<PokemonSpecies>();

		public static List<string> MoveNames { get; private set; }
		public static List<string> PokemonNames { get; private set; }
		public static List<string> TypeNames { get; private set; }
		public static List<string> AbilityNames { get; private set; }
		public static List<string> ItemNames { get; private set; }

		private static FAT fat;

		private static uint FATOffset;
        private static uint FATLength;

		#region Constants

		private const int ROM_NAME_LENGTH = 0xA;
		private const int FAT_POINTER_OFFSET = 0x48;

		const int MOVES_TEXT_BANK_DP = 589;
		const int MOVES_TEXT_BANK_PL = 648;
		const int MOVES_TEXT_BANK_HGSS = 751;

		const int POKEMON_NAMES_TEXT_BANK_DP = 362;
		const int POKEMON_NAMES_TEXT_BANK_PL = 412;
		const int POKEMON_NAMES_TEXT_BANK_HGSS = 237;

		const int TYPES_TEXT_BANK_DP = 565;
		const int TYPES_TEXT_BANK_PL = 624;
		const int TYPES_TEXT_BANK_HGSS = 735;

		const int ABILITY_NAMES_TEXT_BANK_DP = 553;
		const int ABILITY_NAMES_TEXT_BANK_PL = 611;
		const int ABILITY_NAMES_TEXT_BANK_HGSS = 721;

		const int ITEM_NAMES_TEXT_BANK_DP = 344;
		const int ITEM_NAMES_TEXT_BANK_PL = 392;
		const int ITEM_NAMES_TEXT_BANK_HGSS = 222;

		private const int MOVES_NARC_ID_DP = 0x158;
		private const int MOVES_NARC_ID_PL = 0x1BE;
		private const int MOVES_NARC_ID_HGSS = 0x8C;

		private const int POKEMON_SPECIES_NARC_ID_DIAMOND = 0x146;
		private const int POKEMON_SPECIES_NARC_ID_PEARL = 0x148;
		private const int POKEMON_SPECIES_NARC_ID_PL = 0x1A5;
		private const int POKEMON_SPECIES_NARC_ID_HGSS = 0x83;

		private const int TEXT_NARC_ID_DP = 0x13D;
		private const int TEXT_NARC_ID_PL = 0x194;
		private const int TEXT_NARC_ID_HGSS = 0x9C;

		#endregion

		public static void LoadNewRom(string romFilePath)
		{
			BinaryReader romReader = new BinaryReader(new FileStream(romFilePath, FileMode.Open));

            tryReadGameVersion(romReader);
            gameFamily = getGameFamily(gameVersion);

			read(romReader);
        }

		private enum GameVersions
		{
			DIAMOND,
			PEARL,
			PLATINUM,
			HEARTGOLD,
			SOULSILVER,
			BLACK,
			WHITE,
			BLACK2,
			WHITE2,
			NULL
		}

		private enum GameFamilies
		{
			DP,
			PL,
			HGSS,
			BW,
			B2W2,
			NULL
		}


        

        private static void tryReadGameVersion(BinaryReader romFileReader)
		{
			try
			{
                gameVersion = getGameVersionFromRomName(new string(romFileReader.ReadChars(ROM_NAME_LENGTH)).Replace("\0", ""));
            }
			catch (Exception)
			{
				gameVersion = GameVersions.NULL;
			}
		}

        private static GameFamilies getGameFamily(GameVersions gameVersion)
        {
            switch (gameVersion)
            {
                case GameVersions.HEARTGOLD:
                case GameVersions.SOULSILVER:
                    return GameFamilies.HGSS;
                case GameVersions.DIAMOND:
                case GameVersions.PEARL:
                    return GameFamilies.DP;
                case GameVersions.PLATINUM:
                    return GameFamilies.PL;
                case GameVersions.BLACK:
                case GameVersions.WHITE:
                    return GameFamilies.BW;
                case GameVersions.BLACK2:
                case GameVersions.WHITE2:
                    return GameFamilies.B2W2;
                default:
                    return GameFamilies.NULL;
            }
        }

        private static GameVersions getGameVersionFromRomName(string romName)
        {
            return romName switch
            {
                "POKEMON HG" => GameVersions.HEARTGOLD,
                "POKEMON SS" => GameVersions.SOULSILVER,
                "POKEMON D" => GameVersions.DIAMOND,
                "POKEMON P" => GameVersions.PEARL,
                "POKEMON PL" => GameVersions.PLATINUM,
                "POKEMON B" => GameVersions.BLACK,
                "POKEMON W" => GameVersions.WHITE,
                "POKEMON B2" => GameVersions.BLACK2,
                "POKEMON W2" => GameVersions.WHITE2,
                _ => GameVersions.NULL
            };
        }

        private static void read(BinaryReader romFileReader)
		{
			readHeader(romFileReader);

			fat = new FAT(FATOffset);
			fat.SetTotalLengthForRom(FATLength);
            fat.Read(romFileReader);

			movesNarc = new NarcFile(getMovesNarcOffset());
			movesNarc.Read(romFileReader);

			pokemonSpeciesNarc = new NarcFile(getSpeciesNarcOffset());
			pokemonSpeciesNarc.Read(romFileReader);

			gameTextNarc = new NarcFile(getTextNarcOffset());
			gameTextNarc.Read(romFileReader);
			gameText = new TextArchive(gameTextNarc);

			MoveNames = gameText.TextBanks[getMoveNameTextBankID()];
			PokemonNames = gameText.TextBanks[getPokemonNamesTextBankID()];
			TypeNames = gameText.TextBanks[getTypeNamesTextBankID()];
			ItemNames = gameText.TextBanks[getItemNamesTextBankID()];
			AbilityNames = gameText.TextBanks[getAbilityNamesTextBankID()];

			foreach (MemoryStream move in movesNarc.Elements)
				MoveList.Add(new Move(move));

			foreach (MemoryStream pokemonSpecies in pokemonSpeciesNarc.Elements)
				PokemonSpeciesList.Add(new PokemonSpecies(pokemonSpecies));
					
		}

		private static void readHeader(BinaryReader romFileReader)
		{
            try
			{
                romFileReader.BaseStream.Position = FAT_POINTER_OFFSET;
                FATOffset = romFileReader.ReadUInt32();
                FATLength = romFileReader.ReadUInt32();
            }
			catch (EndOfStreamException e)
			{
				throw new EndOfStreamException("End of file reached while reading rom header. The file may be corrupted.\n" + e.Message);
			}
			catch (IOException e)
			{
				throw new IOException("An I/O error occured while reading rom header. Make sure another program does not have this file loaded for editing.\n" + e.Message);
			}
			catch (Exception e)
			{
				throw new Exception("An exception occured while reading rom header.\n" + e.Message);
			}
			
		}

		private static uint getMovesNarcOffset()
		{
			return gameFamily switch
			{
				GameFamilies.DP => fat.GetStartOffset(MOVES_NARC_ID_DP),
				GameFamilies.PL => fat.GetStartOffset(MOVES_NARC_ID_PL),
				GameFamilies.HGSS => fat.GetStartOffset(MOVES_NARC_ID_HGSS),
				_ => 0
			};
		}

		private static uint getSpeciesNarcOffset()
		{
			return gameVersion switch
			{
				GameVersions.DIAMOND => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_DIAMOND),
				GameVersions.PEARL => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_PEARL),
				GameVersions.PLATINUM => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_PL),
				GameVersions.HEARTGOLD => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_HGSS),
                GameVersions.SOULSILVER => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_HGSS),
                _ => 0
			};
		}

		private static uint getTextNarcOffset()
		{
			return gameFamily switch
			{
				GameFamilies.DP => fat.GetStartOffset(TEXT_NARC_ID_DP),
				GameFamilies.PL => fat.GetStartOffset(TEXT_NARC_ID_PL),
				GameFamilies.HGSS => fat.GetStartOffset(TEXT_NARC_ID_HGSS),
				_ => 0
			};
		}

		private static int getMoveNameTextBankID()
		{
			return gameFamily switch
			{
				GameFamilies.DP => MOVES_TEXT_BANK_DP,
				GameFamilies.PL => MOVES_TEXT_BANK_PL,
				GameFamilies.HGSS => MOVES_TEXT_BANK_HGSS,
				_ => -1
			};
		}

		private static int getTypeNamesTextBankID()
		{
			return gameFamily switch
			{
				GameFamilies.DP => TYPES_TEXT_BANK_DP,
				GameFamilies.PL => TYPES_TEXT_BANK_PL,
				GameFamilies.HGSS => TYPES_TEXT_BANK_HGSS,
				_ => -1
			};
		}

		private static int getPokemonNamesTextBankID()
		{
			return gameFamily switch
			{
				GameFamilies.DP => POKEMON_NAMES_TEXT_BANK_DP,
				GameFamilies.PL => POKEMON_NAMES_TEXT_BANK_PL,
				GameFamilies.HGSS => POKEMON_NAMES_TEXT_BANK_HGSS,
				_ => -1
			};
		}

		private static int getItemNamesTextBankID()
		{
			return gameFamily switch
			{
				GameFamilies.DP => ITEM_NAMES_TEXT_BANK_DP,
				GameFamilies.PL => ITEM_NAMES_TEXT_BANK_PL,
                GameFamilies.HGSS => ITEM_NAMES_TEXT_BANK_HGSS,
                _ => -1
			};
		}

		private static int getAbilityNamesTextBankID()
		{
			return gameFamily switch
			{
				GameFamilies.DP => ABILITY_NAMES_TEXT_BANK_DP,
				GameFamilies.PL => ABILITY_NAMES_TEXT_BANK_PL,
				GameFamilies.HGSS => ABILITY_NAMES_TEXT_BANK_HGSS,
				_ => -1
			};
		}
		
		public static void Save(BinaryWriter writer)
		{
			for (int i = 0; i < MoveList.Count; i++)
				movesNarc.Elements[i] = MoveList[i].GetBinary();

			for (int i = 0; i < PokemonSpeciesList.Count; i++)
				pokemonSpeciesNarc.Elements[i] = PokemonSpeciesList[i].GetBinary();

			movesNarc.Write(writer);
			pokemonSpeciesNarc.Write(writer);
		}

        public static bool IsValidGameVersion()
        {
            if (gameFamily == GameFamilies.NULL)
                return false;
            else
                return true;
        }

        public static bool IsSupportedGameVersion()
        {
            if (gameFamily == GameFamilies.BW || gameFamily == GameFamilies.B2W2)
                return false;
            else
                return true;
        }

        public static Move GetMove(int moveIndex) => MoveList[moveIndex];

        public static void UpdateMove(int moveIndex, Move updatedMove)
        {
            MoveList[moveIndex] = updatedMove;
        }

        public static string GetGameVersion() => gameVersion.ToString();
		public static string[] GetMoveNames() => MoveNames.ToArray();
		public static string[] GetPokemonSpeciesNames() => PokemonNames.ToArray();
		public static string[] GetItemNames() => ItemNames.ToArray();
		public static string[] GetTypeNames() => TypeNames.ToArray();
		public static string[] GetAbilityNames() => AbilityNames.ToArray();
        public static string[] GetMoveCategories() => Enum.GetNames(typeof(Move.Categories));
        public static string[] GetMoveContestConditions() => Enum.GetNames(typeof(Move.ContestConditions));
		public static string[] GetMoveContestEffect() => Move.ContestEffectDescriptions;
        public static string[] GetMoveTargets() => Enum.GetNames(typeof(Move.Targets));

    }
}
