using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DS_Pokemon_Stat_Editor
{
	public class RomFile
	{
		private NarcFile movesNarc;
		private NarcFile pokemonSpeciesNarc;
		private NarcFile gameTextNarc;
		private TextArchive gameText;
		private GameVersions gameVersion;
		private readonly GameFamilies gameFamily;

		private List<Move> MoveList = new List<Move>();
        private List<PokemonSpecies> PokemonSpeciesList = new List<PokemonSpecies>();

		public List<string> MoveNames { get; private set; }
		public List<string> PokemonNames { get; private set; }
		public List<string> TypeNames { get; private set; }
		public List<string> AbilityNames { get; private set; }
		public List<string> ItemNames { get; private set; }

		private FAT fat;

		private uint FATOffset;
		private uint FATLength;

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
		const int TYPES_TEXT_BANK_PL = 642;
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

		private const int POKEMON_SPECIES_NARC_ID_DP = 0x146;
		private const int POKEMON_SPECIES_NARC_ID_PL = 0x1A5;
		private const int POKEMON_SPECIES_NARC_ID_HGSS = 0x83;

		private const int TEXT_NARC_ID_DP = 0x13D;
		private const int TEXT_NARC_ID_PL = 0x194;
		private const int TEXT_NARC_ID_HGSS = 0x9C;

		#endregion



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

		public RomFile(BinaryReader romFileReader)
		{
			tryReadGameVersion(romFileReader);
            gameFamily = getGameFamily(gameVersion);
        }

        private GameFamilies getGameFamily(GameVersions gameVersion)
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

        private void tryReadGameVersion(BinaryReader romFileReader)
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

        private GameVersions getGameVersionFromRomName(string romName)
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

        public void Read(BinaryReader romFileReader)
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

		private void readHeader(BinaryReader romFileReader)
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

		private uint getMovesNarcOffset()
		{
			return gameFamily switch
			{
				GameFamilies.DP => fat.GetStartOffset(MOVES_NARC_ID_DP),
				GameFamilies.PL => fat.GetStartOffset(MOVES_NARC_ID_PL),
				GameFamilies.HGSS => fat.GetStartOffset(MOVES_NARC_ID_HGSS),
				_ => 0
			};
		}

		private uint getSpeciesNarcOffset()
		{
			return gameFamily switch
			{
				GameFamilies.DP => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_DP),
				GameFamilies.PL => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_PL),
				GameFamilies.HGSS => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_HGSS),
				_ => 0
			};
		}

		private uint getTextNarcOffset()
		{
			return gameFamily switch
			{
				GameFamilies.DP => fat.GetStartOffset(TEXT_NARC_ID_DP),
				GameFamilies.PL => fat.GetStartOffset(TEXT_NARC_ID_PL),
				GameFamilies.HGSS => fat.GetStartOffset(TEXT_NARC_ID_HGSS),
				_ => 0
			};
		}

		private int getMoveNameTextBankID()
		{
			return gameFamily switch
			{
				GameFamilies.DP => MOVES_TEXT_BANK_DP,
				GameFamilies.PL => MOVES_TEXT_BANK_PL,
				GameFamilies.HGSS => MOVES_TEXT_BANK_HGSS,
				_ => -1
			};
		}

		private int getTypeNamesTextBankID()
		{
			return gameFamily switch
			{
				GameFamilies.DP => TYPES_TEXT_BANK_DP,
				GameFamilies.PL => TYPES_TEXT_BANK_PL,
				GameFamilies.HGSS => TYPES_TEXT_BANK_HGSS,
				_ => -1
			};
		}

		private int getPokemonNamesTextBankID()
		{
			return gameFamily switch
			{
				GameFamilies.DP => POKEMON_NAMES_TEXT_BANK_DP,
				GameFamilies.PL => POKEMON_NAMES_TEXT_BANK_PL,
				GameFamilies.HGSS => POKEMON_NAMES_TEXT_BANK_HGSS,
				_ => -1
			};
		}

		private int getItemNamesTextBankID()
		{
			return gameFamily switch
			{
				GameFamilies.DP => ITEM_NAMES_TEXT_BANK_DP,
				GameFamilies.PL => ITEM_NAMES_TEXT_BANK_PL,
                GameFamilies.HGSS => ITEM_NAMES_TEXT_BANK_HGSS,
                _ => -1
			};
		}

		private int getAbilityNamesTextBankID()
		{
			return gameFamily switch
			{
				GameFamilies.DP => ABILITY_NAMES_TEXT_BANK_DP,
				GameFamilies.PL => ABILITY_NAMES_TEXT_BANK_PL,
				GameFamilies.HGSS => ABILITY_NAMES_TEXT_BANK_HGSS,
				_ => -1
			};
		}
		
		public void Save(BinaryWriter writer)
		{
			for (int i = 0; i < MoveList.Count; i++)
				movesNarc.Elements[i] = MoveList[i].GetBinary();

			for (int i = 0; i < PokemonSpeciesList.Count; i++)
				pokemonSpeciesNarc.Elements[i] = PokemonSpeciesList[i].GetBinary();

			movesNarc.Write(writer);
			pokemonSpeciesNarc.Write(writer);
		}

        public bool IsValidGameVersion()
        {
            if (gameFamily == GameFamilies.NULL)
                return false;
            else
                return true;
        }

        public bool IsSupportedGameVersion()
        {
            if (gameFamily == GameFamilies.BW || gameFamily == GameFamilies.B2W2)
                return false;
            else
                return true;
        }

        public Move GetMove(int moveIndex) => MoveList[moveIndex];

        public void UpdateMove(int moveIndex, Move updatedMove)
        {
            MoveList[moveIndex] = updatedMove;
        }

        public string getGameVersion() => gameVersion.ToString();
    }
}
