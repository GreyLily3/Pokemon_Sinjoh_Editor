using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pokemon_Sinjoh_Editor
{
	public static class RomFile
	{
		private static NarcFile movesNarc;
		private static NarcFile pokemonSpeciesNarc;
		private static NarcFile npcTradesNarc;
		private static NarcFile gameTextNarc;
		private static TextArchive gameText;
        public static Languages Language;
		private static GameVersions GameVersion;
        public static GameFamilies gameFamily = GameFamilies.NULL;
		private static string romPath;
		public static bool AreUnsavedChanges = false;

		public static List<Move> MoveList = new List<Move>();
        public static List<PokemonSpecies> PokemonSpeciesList = new List<PokemonSpecies>();
		public static List<NPCTrade> NPCTradesList = new List<NPCTrade>();

		public static List<string> MoveNames { get; private set; }
		public static List<string> PokemonNames { get; private set; }
		public static List<string> TypeNames { get; private set; }
		public static List<string> AbilityNames { get; private set; }
		public static List<string> ItemNames { get; private set; }
		public static List<string> TradePokemonNicknames { get; private set; } = new List<string>();
        public static List<string> TradePokemonTrainerNames { get; private set; } = new List<string>();
		public static List<string> PokedexText {  get; private set; } = new List<string>();
        public static List<string> NatureNames { get; private set; } = new List<string>();

        private static FAT fat;

		private static uint FATOffset;
        private static uint FATLength;

		#region Constants

		private const int ROM_NAME_LENGTH = 0xA;
		private const int FAT_POINTER_OFFSET = 0x48;
        private const int LANGUAGE_GAME_CODE_OFFSET = 0xF;

        private const int MOVES_TEXT_BANK_DP = 589;
        private const int MOVES_TEXT_BANK_PL = 648;
        private const int MOVES_TEXT_BANK_HGSS = 751;

        private const int POKEMON_NAMES_TEXT_BANK_DP = 362;
        private const int POKEMON_NAMES_TEXT_BANK_PL = 412;
        private const int POKEMON_NAMES_TEXT_BANK_HGSS = 237;

        private const int TYPES_TEXT_BANK_DP = 565;
        private const int TYPES_TEXT_BANK_PL = 624;
        private const int TYPES_TEXT_BANK_HGSS = 735;

        private const int ABILITY_NAMES_TEXT_BANK_DP = 553;
        private const int ABILITY_NAMES_TEXT_BANK_PL = 611;
        private const int ABILITY_NAMES_TEXT_BANK_HGSS = 721;

        private const int ITEM_NAMES_TEXT_BANK_DP = 344;
        private const int ITEM_NAMES_TEXT_BANK_PL = 392;
        private const int ITEM_NAMES_TEXT_BANK_HGSS = 222;

        private const int TRADE_POKEMON_NICKNAME_AND_TRAINER_TEXT_BANK_DP = 326;
        private const int TRADE_POKEMON_NICKNAME_AND_TRAINER_TEXT_BANK_PL = 370;
        private const int TRADE_POKEMON_NICKNAME_AND_TRAINER_TEXT_BANK_HGSS = 200;

        private const int POKEDEX_TEXT_BANK_DP = 614;
        private const int POKEDEX_TEXT_BANK_PL = 697;
        private const int POKEDEX_TEXT_BANK_HGSS = 802;

        private const int NATURE_TEXT_BANK_DP = 190;
        private const int NATURE_TEXT_BANK_PL = 202;
        private const int NATURE_TEXT_BANK_HGSS = 34;

        private const int DEOXYS_ATTACK_FORM_NAME_INDEX_DP = 111;
        private const int DEOXYS_ATTACK_FORM_NAME_INDEX_PL = 112;
        private const int DEOXYS_ATTACK_FORM_NAME_INDEX_HGSS = 146;

        private const int DEOXYS_DEFENSE_FORM_NAME_INDEX_DP = 112;
        private const int DEOXYS_DEFENSE_FORM_NAME_INDEX_PL = 113;
        private const int DEOXYS_DEFENSE_FORM_NAME_INDEX_HGSS = 147;

        private const int DEOXYS_SPEED_FORM_NAME_INDEX_DP = 113;
        private const int DEOXYS_SPEED_FORM_NAME_INDEX_PL = 114;
        private const int DEOXYS_SPEED_FORM_NAME_INDEX_HGSS = 148;

        private const int SHAYMIN_SKY_FORM_NAME_INDEX_PL = 116;
        private const int SHAYMIN_SKY_FORM_NAME_INDEX_HGSS = 150;

        private const int GIRATINA_ORIGIN_FORM_NAME_INDEX_PL = 118;
        private const int GIRATINA_ORIGIN_FORM_NAME_INDEX_HGSS = 152;

        private const int ROTOM_HEAT_FORM_NAME_INDEX_PL = 120;
        private const int ROTOM_HEAT_FORM_NAME_INDEX_HGSS = 154;

        private const int ROTOM_WASH_FORM_NAME_INDEX_PL = 121;
        private const int ROTOM_WASH_FORM_NAME_INDEX_HGSS = 155;

        private const int ROTOM_FROST_FORM_NAME_INDEX_PL = 122;
        private const int ROTOM_FROST_FORM_NAME_INDEX_HGSS = 156;

        private const int ROTOM_FAN_FORM_NAME_INDEX_PL = 123;
        private const int ROTOM_FAN_FORM_NAME_INDEX_HGSS = 157;

        private const int ROTOM_MOW_FORM_NAME_INDEX_PL = 124;
        private const int ROTOM_MOW_FORM_NAME_INDEX_HGSS = 158;

        private const int WORMADAM_SANDY_FORM_NAME_INDEX_DPPL = 18;
        private const int WORMADAM_SANDY_FORM_NAME_INDEX_HGSS = 119;

        private const int WORMADAM_TRASH_FORM_NAME_INDEX_DPPL = 19;
        private const int WORMADAM_TRASH_FORM_NAME_INDEX_HGSS = 120;

        private const int MOVES_NARC_ID_DP = 0x158;
		private const int MOVES_NARC_ID_PL = 0x1BE;
		private const int MOVES_NARC_ID_HGSS = 0x8C;

		private const int POKEMON_SPECIES_NARC_ID_DIAMOND = 0x146;
		private const int POKEMON_SPECIES_NARC_ID_PEARL = 0x148;
		private const int POKEMON_SPECIES_NARC_ID_PL = 0x1A5;
		private const int POKEMON_SPECIES_NARC_ID_HGSS = 0x83;

        private const int NPC_TRADES_ID_DP = 0x10E;
        private const int NPC_TRADES_ID_PL = 0x150;
        private const int NPC_TRADES_NARC_ID_HGSS = 0xF1;

        private const int TEXT_NARC_ID_DP = 0x13D;
		private const int TEXT_NARC_ID_PL = 0x194;
		private const int TEXT_NARC_ID_HGSS = 0x9C;

		private const int SPECIES_START_INDEX = 1;
        private const int MOVE_START_INDEX = 1;

        private const int SPECIES_WORMADAM_INDEX = 413;
        private const int SPECIES_DEOXYS_INDEX = 386;
        private const int SPECIES_GIRATINA_INDEX = 487;
        private const int SPECIES_SHAYMIN_INDEX = 492;
        private const int SPECIES_ROTOM_INDEX = 479;

        public const int TRADE_JASMINE_INDEX = 5;
        public const int TRADE_WEBSTER_INDEX = 7;

        public const int POKEMON_NAME_MAX_LENGTH = 10;
        public const int MOVE_NAME_MAX_LENGTH = 12;

        #endregion

        public static void LoadNewRom(string romFilePath)
		{
			romPath = romFilePath;
			FileStream romFileStream = new FileStream(romPath, FileMode.Open); //we need to store the filestream seperately to dispose of it later
            BinaryReader romReader = new BinaryReader(romFileStream, Encoding.UTF8, true); //leave streams open so that it doesn't close the narcfile memorystreams

            tryReadGameVersion(romReader);
            gameFamily = getGameFamily(GameVersion);

			if (!IsValidGameVersion() || !IsSupportedGameVersion())
				return;

			read(romReader);
			romFileStream.Dispose();
			romReader.Dispose();
        }

		public enum GameVersions
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

		public enum GameFamilies
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
                GameVersion = getGameVersionFromRomName(new string(romFileReader.ReadChars(ROM_NAME_LENGTH)).Replace("\0", ""));
            }
			catch (Exception)
			{
				GameVersion = GameVersions.NULL;
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

			npcTradesNarc = new NarcFile(getNPCTradesNarcOffset());
			npcTradesNarc.Read(romFileReader);

            gameTextNarc = new NarcFile(getTextNarcOffset());
			gameTextNarc.Read(romFileReader);
			gameText = new TextArchive(gameTextNarc);

            MoveNames = gameText.TextBanks[getMoveNameTextBankID()];
			MoveNames.RemoveAt(0); //remove the first entry because it's a placeholder

			PokemonNames = gameText.TextBanks[getPokemonNamesTextBankID()];
			PokemonNames.RemoveAt(0); //remove the first entry because it's a placeholder

			TypeNames = gameText.TextBanks[getTypeNamesTextBankID()];
			ItemNames = gameText.TextBanks[getItemNamesTextBankID()];
			AbilityNames = gameText.TextBanks[getAbilityNamesTextBankID()];
			PokedexText = gameText.TextBanks[getPokedexTextBankID()];
            NatureNames = gameText.TextBanks[getNatureTextBankID()];

            TradePokemonNicknames.Clear();
            TradePokemonTrainerNames.Clear();

            for (int i = 0; i < npcTradesNarc.Elements.Count; i++)
                TradePokemonNicknames.Add(gameText.TextBanks[getTradePokemonNicknamesAndTrainerNamesTextBankID()][i]);

            //multiply count by 2 because there is a trainer name for pokemon nickname
            for (int i = npcTradesNarc.Elements.Count; i < npcTradesNarc.Elements.Count * 2; i++)
                TradePokemonTrainerNames.Add(gameText.TextBanks[getTradePokemonNicknamesAndTrainerNamesTextBankID()][i]);


            MoveList.Clear();
			PokemonSpeciesList.Clear();
            NPCTradesList.Clear();

            //skip the first move because it's a placeholder
            for (int i = 1; i < movesNarc.Elements.Count; i++)
				MoveList.Add(new Move(movesNarc.Elements[i]));

			//skip the first pokemon because it's a placeholder
            for (int i = 1; i < pokemonSpeciesNarc.Elements.Count; i++)
                PokemonSpeciesList.Add(new PokemonSpecies(pokemonSpeciesNarc.Elements[i]));

            for (int i = 0; i < npcTradesNarc.Elements.Count; i++)
				NPCTradesList.Add(new NPCTrade(npcTradesNarc.Elements[i]));

			
        }

		private static void readHeader(BinaryReader romFileReader)
		{
            try
			{
                romFileReader.BaseStream.Position = LANGUAGE_GAME_CODE_OFFSET;

                switch (romFileReader.ReadChar())
                {
                    case 'E':
                        Language = Languages.ENGLISH;
                        break;
                    case 'J':
                        Language = Languages.JAPANESE;
                        break;
                    case 'K':
                        Language = Languages.KOREAN;
                        break;
                    case 'S':
                        Language = Languages.SPANISH;
                        break;
                    case 'D':
                        Language = Languages.GERMAN;
                        break;
                    case 'F':
                        Language = Languages.FRENCH;
                        break;
                    case 'I':
                        Language = Languages.ITALIAN;
                        break;
                    default:
                        Language = Languages.UNKNOWN;
                        break;
                }

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
			return GameVersion switch
			{
				GameVersions.DIAMOND => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_DIAMOND),
				GameVersions.PEARL => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_PEARL),
				GameVersions.PLATINUM => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_PL),
				GameVersions.HEARTGOLD => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_HGSS),
                GameVersions.SOULSILVER => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_HGSS),
                _ => 0
			};
		}

		private static uint getNPCTradesNarcOffset()
		{
            return gameFamily switch
            {
                GameFamilies.DP => fat.GetStartOffset(NPC_TRADES_ID_DP),
                GameFamilies.PL => fat.GetStartOffset(NPC_TRADES_ID_PL),
                GameFamilies.HGSS => fat.GetStartOffset(NPC_TRADES_NARC_ID_HGSS),
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

		private static int getTradePokemonNicknamesAndTrainerNamesTextBankID()
		{
            return gameFamily switch
            {
                GameFamilies.DP => TRADE_POKEMON_NICKNAME_AND_TRAINER_TEXT_BANK_DP,
                GameFamilies.PL => TRADE_POKEMON_NICKNAME_AND_TRAINER_TEXT_BANK_PL,
                GameFamilies.HGSS => TRADE_POKEMON_NICKNAME_AND_TRAINER_TEXT_BANK_HGSS,
                _ => -1
            };
        }

		private static int getPokedexTextBankID()
		{
            return gameFamily switch
            {
                GameFamilies.DP => POKEDEX_TEXT_BANK_DP,
                GameFamilies.PL => POKEDEX_TEXT_BANK_PL,
                GameFamilies.HGSS => POKEDEX_TEXT_BANK_HGSS,
                _ => -1
            };
        }

		private static int getDeoxysAttackFormNameIndex()
		{
            return gameFamily switch
            {
                GameFamilies.DP => DEOXYS_ATTACK_FORM_NAME_INDEX_DP,
                GameFamilies.PL => DEOXYS_ATTACK_FORM_NAME_INDEX_PL,
                GameFamilies.HGSS => DEOXYS_ATTACK_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getDeoxysDefenseFormNameIndex()
        {
            return gameFamily switch
            {
                GameFamilies.DP => DEOXYS_DEFENSE_FORM_NAME_INDEX_DP,
                GameFamilies.PL => DEOXYS_DEFENSE_FORM_NAME_INDEX_PL,
                GameFamilies.HGSS => DEOXYS_DEFENSE_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getDeoxysSpeedFormNameIndex()
        {
            return gameFamily switch
            {
                GameFamilies.DP => DEOXYS_SPEED_FORM_NAME_INDEX_DP,
                GameFamilies.PL => DEOXYS_SPEED_FORM_NAME_INDEX_PL,
                GameFamilies.HGSS => DEOXYS_SPEED_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getWormadamSandyFormNameIndex()
        {
            return gameFamily switch
            {
                GameFamilies.DP => WORMADAM_SANDY_FORM_NAME_INDEX_DPPL,
                GameFamilies.PL => WORMADAM_SANDY_FORM_NAME_INDEX_DPPL,
                GameFamilies.HGSS => WORMADAM_SANDY_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getWormadamTrashFormNameIndex()
        {
            return gameFamily switch
            {
                GameFamilies.DP => WORMADAM_TRASH_FORM_NAME_INDEX_DPPL,
                GameFamilies.PL => WORMADAM_TRASH_FORM_NAME_INDEX_DPPL,
                GameFamilies.HGSS => WORMADAM_TRASH_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getGiratinaOriginFormNameIndex()
        {
            return gameFamily switch
            {
                GameFamilies.PL => GIRATINA_ORIGIN_FORM_NAME_INDEX_PL,
                GameFamilies.HGSS => GIRATINA_ORIGIN_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getShayminSkyFormNameIndex()
        {
            return gameFamily switch
            {
                GameFamilies.PL => SHAYMIN_SKY_FORM_NAME_INDEX_PL,
                GameFamilies.HGSS => SHAYMIN_SKY_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getRotomHeatFormNameIndex()
        {
            return gameFamily switch
            {
                GameFamilies.PL => ROTOM_HEAT_FORM_NAME_INDEX_PL,
                GameFamilies.HGSS => ROTOM_HEAT_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getRotomWashFormNameIndex()
        {
            return gameFamily switch
            {
                GameFamilies.PL => ROTOM_WASH_FORM_NAME_INDEX_PL,
                GameFamilies.HGSS => ROTOM_WASH_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getRotomFrostFormNameIndex()
        {
            return gameFamily switch
            {
                GameFamilies.PL => ROTOM_FROST_FORM_NAME_INDEX_PL,
                GameFamilies.HGSS => ROTOM_FROST_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getRotomFanFormNameIndex()
        {
            return gameFamily switch
            {
                GameFamilies.PL => ROTOM_FAN_FORM_NAME_INDEX_PL,
                GameFamilies.HGSS => ROTOM_FAN_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getRotomMowFormNameIndex()
        {
            return gameFamily switch
            {
                GameFamilies.PL => ROTOM_MOW_FORM_NAME_INDEX_PL,
                GameFamilies.HGSS => ROTOM_MOW_FORM_NAME_INDEX_HGSS,
                _ => -1
            };
        }

        private static int getNatureTextBankID()
        {
            return gameFamily switch
            {
                GameFamilies.DP => NATURE_TEXT_BANK_DP,
                GameFamilies.PL => NATURE_TEXT_BANK_PL,
                GameFamilies.HGSS => NATURE_TEXT_BANK_HGSS,
                _ => 0
            };
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

		public static void Write()
		{
			FileStream romFileStream = new FileStream(romPath, FileMode.Open);
            BinaryWriter romWriter = new BinaryWriter(romFileStream, Encoding.UTF8, true);

			for (int i = 0; i < MoveList.Count; i++)
				movesNarc.Elements[i + 1] = MoveList[i].GetBinary(); //skip the first move in movesNarc because it's a placeholder

			for (int i = 0; i < PokemonSpeciesList.Count; i++)
				pokemonSpeciesNarc.Elements[i + 1] = PokemonSpeciesList[i].GetBinary(); //skip the first pokemon in pokemonSpeciesNarc because it's a placeholder

			for (int i = 0; i < NPCTradesList.Count; i++)
                npcTradesNarc.Elements[i] = NPCTradesList[i].GetBinary();

            try
			{
				movesNarc.Write(romWriter);
				pokemonSpeciesNarc.Write(romWriter);
                npcTradesNarc.Write(romWriter);
                AreUnsavedChanges = false;
            }
            catch (EndOfStreamException e)
            {
                throw new EndOfStreamException("End of file reached while attempting to save data. The file may be corrupted.\n" + e.Message);
            }
            catch (IOException e)
            {
                throw new IOException("An I/O error occured while attempting to save data. Make sure another program does not have this file loaded for editing.\n" + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception("An exception occured while attempting to save data.\n" + e.Message);
            }
			finally
			{
				romFileStream.Dispose();
            }
        }

        //will replace this static list later if there's a good way to get/set what moves are set as TMs (this is stored in ARM9 binary)
        public static string[] GetTMNames()
		{
            string tmString;
            string[] TMNames = new string[92];
            int[] TMIndices = { 264, 337, 352, 347, 46, 92, 258, 339, 331, 237, 241, 269, 58, 59, 63, 113, 182, 240, 202, 219, 218, 76, 231, 85, 87, 89, 216,
            91, 94, 247, 280, 104, 115, 351, 53, 188, 201, 126, 317, 332, 259, 263, 290, 156, 213, 168, 211, 285, 289, 315, 355, 411, 412, 206, 362, 374, 
            451, 203, 406, 409, 261, 318, 373, 153, 421, 371, 278, 416, 397, 148, 444, 419, 86, 360, 14, 446, 244, 445, 399, 157, 404, 214,
            363, 398, 138, 447, 207, 365, 369, 164, 430, 433};

            tmString = INIManager.Language switch
            {
                Languages.ENGLISH => "TM",
                Languages.FRENCH => "CT",
                Languages.SPANISH => "MT",
                Languages.GERMAN => "TM",
                Languages.ITALIAN => "MT",
                Languages.JAPANESE => "わざマシン",
                Languages.KOREAN => "TM",
                _ => "TM"
            };

            for (int i = 0; i < 92; i++)
                TMNames[i] = tmString + (i + 1).ToString("D2") + " " + MoveNames[TMIndices[i] - MOVE_START_INDEX];

            return TMNames;
		}

		//will replace static indices later if there's a good way to get/set what moves are HMs (this is stored in ARM9 binary)
        public static string[] GetHMNames()
        {
            string hmString;
            string[] HMNames = new string[8];
            int[] HMIndices = { 15, 19, 57, 70, 250, 249, 127, 431 };

            if (gameFamily != GameFamilies.HGSS)
                HMIndices[5] = 432; //replace whirlpool with defog for HM05

            hmString = INIManager.Language switch
            {
                Languages.ENGLISH => "HM0",
                Languages.FRENCH => "CS0",
                Languages.SPANISH => "MO0",
                Languages.GERMAN => "VM",
                Languages.ITALIAN => "MN",
                Languages.JAPANESE => "ひでんマシン0",
                Languages.KOREAN => "TM",
                _ => "HM0"
            };

            for (int i = 0; i < 8; i++)
                HMNames[i] = hmString + (i + 1) + " " + MoveNames[HMIndices[i] - MOVE_START_INDEX];

            return HMNames;
        }

        public static string GetAbilityName(int speciesIndex, PersonalityValue pv)
        {
            bool pokemonHasTwoAbilities;
            int genderRatio = PokemonSpeciesList[speciesIndex].GenderRatio;

            if (PokemonSpeciesList[speciesIndex].Ability1 == PokemonSpeciesList[speciesIndex].Ability2 || PokemonSpeciesList[speciesIndex].Ability2 == 0)
                pokemonHasTwoAbilities = false;
            else
                pokemonHasTwoAbilities = true;

            if (pokemonHasTwoAbilities && pv.GetHasSecondAbility())
                return AbilityNames[PokemonSpeciesList[speciesIndex].Ability2];
            else
                return AbilityNames[PokemonSpeciesList[speciesIndex].Ability1];

        }

        public static string GetGameVersion() => GameVersion.ToString();
		public static string[] GetMoveNames() => MoveNames.ToArray();
		public static string[] GetPokemonSpeciesNames()
		{
			string[] speciesNames = new string[PokemonSpeciesList.Count];

			for (int i = 0; i < PokemonNames.Count; i++)
				speciesNames[i] = PokemonNames[i];

            speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 1] = speciesNames[SPECIES_DEOXYS_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getDeoxysAttackFormNameIndex()] + ")";
            speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 2] = speciesNames[SPECIES_DEOXYS_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getDeoxysDefenseFormNameIndex()] + ")";
            speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 3] = speciesNames[SPECIES_DEOXYS_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getDeoxysSpeedFormNameIndex()] + ")";
            speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 4] = speciesNames[SPECIES_WORMADAM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getWormadamSandyFormNameIndex()] + ")";
            speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 5] = speciesNames[SPECIES_WORMADAM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getWormadamTrashFormNameIndex()] + ")";

            if (gameFamily == GameFamilies.HGSS || gameFamily == GameFamilies.PL)
			{
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 6] = speciesNames[SPECIES_GIRATINA_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getGiratinaOriginFormNameIndex()] + ")";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 7] = speciesNames[SPECIES_SHAYMIN_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getShayminSkyFormNameIndex()] + ")";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 8] = speciesNames[SPECIES_ROTOM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getRotomHeatFormNameIndex()] + ")";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 9] = speciesNames[SPECIES_ROTOM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getRotomWashFormNameIndex()] + ")";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 10] = speciesNames[SPECIES_ROTOM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getRotomFrostFormNameIndex()] + ")";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 11] = speciesNames[SPECIES_ROTOM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getRotomFanFormNameIndex()] + ")";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 12] = speciesNames[SPECIES_ROTOM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getRotomMowFormNameIndex()] + ")";
            }

			return speciesNames;
        }
		public static string[] GetItemNames() => ItemNames.ToArray();
		public static string[] GetTypeNames() => TypeNames.ToArray();
		public static string[] GetAbilityNames() => AbilityNames.ToArray();
        public static string[] GetMoveCategories()
        {
            return INIManager.Language switch
            {
                Languages.ENGLISH => Enum.GetNames(typeof(Move.Categories)),
                Languages.FRENCH => new string[]{ "Physique", "Spéciale", "Statut" },
                Languages.SPANISH => new string[] { "Físico", "Especial", "Estado" },
                Languages.GERMAN => new string[] { "Physische", "Spezial", "Status" },
                Languages.ITALIAN => new string[] { "Fisica", "Speciale", "Stato" },
                Languages.JAPANESE => new string[] { "物理", "特殊", "変化" },
                Languages.KOREAN => new string[] { "물리", "특수", "변화" },
                _ => Enum.GetNames(typeof(Move.Categories))
            };
            
        }

        public static string[] GetMoveContestConditions()
        {
            if (INIManager.Language == Languages.ENGLISH)
                return Enum.GetNames(typeof(Move.ContestConditions));
            else if (INIManager.Language == Languages.FRENCH)
                return new string[] { "Sang-froid", "Beauté", "Grâce", "Intelligence", "Robustesse" };
            else if (INIManager.Language == Languages.SPANISH)
                return new string[] { "Carisma", "Belleza", "Dulzura", "Ingenio", "Dureza" };
            else if (INIManager.Language == Languages.GERMAN)
                return new string[] { "Coole", "Schönheit", "Anmut", "Klugheit", "Stärke" };
            else if (INIManager.Language == Languages.ITALIAN)
                return new string[] { "Classe", "Bellezza", "Grazia", "Acume", "Grinta" };
            else if (INIManager.Language == Languages.JAPANESE)
                return new string[] { "かっこよさ", "うつくしさ", "かわいさ", "かしこさ", "たくましさ" };
            else if (INIManager.Language == Languages.KOREAN)
                return new string[] { "근사함", "아름다움", "귀여움", "슬기로움", "강인함" };
            else
                return Enum.GetNames(typeof(Move.ContestConditions));
        }
		public static string[] GetMoveContestEffect() => Move.ContestEffectDescriptions;

        public static string[] GetMoveTargets()
        {
            string[] targetNames = Enum.GetNames(typeof(Move.Targets));

            for (int i = 0; i < targetNames.Length; i++)
                targetNames[i] = targetNames[i].Replace('_', ' ');

            return targetNames;
        }

        public static string[] GetEggGroupNames() 
        {
            string[] eggGroupNames;

            if (INIManager.Language == Languages.ENGLISH)
            {
                eggGroupNames = Enum.GetNames(typeof(PokemonSpecies.EggGroups));

                for (int i = 0; i < eggGroupNames.Length; i++)
                    eggGroupNames[i] = eggGroupNames[i].Replace('_', ' ');
            }
            else
            {
                eggGroupNames = INIManager.Language switch
                {
                    Languages.FRENCH => new string[] { "Monstreux", "Aquatique", "Insectoïde" , "Aérien" , "Terrestre", "Féerique", "Végétal", "Humanoïde", "Aquatique 3", "Minéral", "Amorphe", "Aquatique 2", "Métamorph", "Draconique", "Inconnu" },
                    Languages.SPANISH => new string[] { "Monstruo", "Agua 1"   , "Bicho"      , "Volador", "Campo"    , "Hada"    , "Planta", "Humanoide", "Agua 3", "Mineral", "Amorfo", "Agua 2", "Ditto", "Dragón", "Desconocido" },
                    Languages.GERMAN => new string[] { "Monster"  , "Wasser 1" , "Käfer"      , "Flug"   , "Feld"     , "Fee"     , "Pflanze", "Humanotyp", "Wasser 3", "Mineral", "Amorph", "Wasser 2", "Ditto", "Drache", "Unbekannt" },
                    Languages.ITALIAN => new string[] { "Mostro"  , "Acqua 1"  , " Coleottero", "Volante", "Campo"    , "Magico"  , "Erba", "Umanoide", "Acqua 3", "Minerale", "Amorfo", "Acqua 2", "Ditto", "Drago", "Sconosciuto" },
                    Languages.JAPANESE => new string[] { "怪獣"    , "水中 1"   , "虫グ"        , "飛行"   , "陸上"      , "妖精"    , "植物", "人型", "水中 3", "鉱物", "不定形", "水中 2", "メタモン", "ドラゴ", "タマゴ未発見" },
                    Languages.KOREAN => new string[] { "괴수"      , "수중 1"   , "벌레"        , "비행"   , "육상"      , "요정"    , "식물", "인간형", "수중 3", "광물", "부정형", "수중 2", "메타몽", "드래곤", "알미발견" },
                };
            }

            return eggGroupNames;
        }

        public static string[] GetXPGroupNames()
        {
            string[] xpGroupNames;

            if (INIManager.Language == Languages.ENGLISH)
            {
                xpGroupNames = Enum.GetNames(typeof(PokemonSpecies.XPGroups));

                for (int i = 0; i < xpGroupNames.Length; i++)
                    xpGroupNames[i] = xpGroupNames[i].Replace('_', ' ');
            }
            else
            {
                xpGroupNames = INIManager.Language switch
                {
                    Languages.FRENCH => new string[] { "Moyenne", "Erratique", "Fluctuante", "Parabolique", "Rapide", "Lente", "???", "???"},
                    Languages.SPANISH => new string[] { "Medio", "Errático", "Fluctuante", "Parabólico", "Rápido", "Lento", "???", "???"},
                    Languages.GERMAN => new string[] { "Mittel-Schnell", "Erratic", "Fluctuating", "Mittel-Langsam", "Schnell", "Langsam", "???", "???"},
                    Languages.ITALIAN => new string[] { "Medio-veloce", "Irregolare", "Fluttuante", "Medio-lenta", "Veloce", "Lenta", "???", "???"},
                    Languages.JAPANESE => new string[] { "100万タイプ", "60万タイプ", "164万タイプ", "105万タイプ", "80万タイプ", "125万タイプ", "???", "???" },
                    Languages.KOREAN => new string[] { "괴수", "수중 1", "벌레", "비행", "육상", "요정", "식물", "인간형"},
                };
            }


            return xpGroupNames;
        }

		public static string[] GetLanguageNames()
        {
            return INIManager.Language switch
            {
                Languages.ENGLISH => Enum.GetNames(typeof(Languages)),
                Languages.FRENCH => new string[] { "Japonais", "Anglais", "Français", "Italien", "Allemand", "???", "Espagnol", "Coréen" },
                Languages.SPANISH => new string[] { "Japonés", "Inglés", "Francés", "Italiano", "Alemán", "???", "Español", "Coreano" },
                Languages.GERMAN => new string[] { "Japanisch", "Englisch", "Französisch", "Italienisch", "Deutsch", "???", "Spanisch", "Koreanisch" },
                Languages.ITALIAN => new string[] { "Giapponese", "Inglese", "Francese", "Italiano", "Tedesco", "???", "Spagnolo", "Coreano" },
                Languages.JAPANESE => new string[] { "日本語", "英語", "フランス語", "イタリア語", "ドイツ語", "???", "スペイン語", "ハングル語" },
                Languages.KOREAN => new string[] { "물리", "특수", "변화" },
                _ => Enum.GetNames(typeof(Languages))
            };

            
        }

		public static string[] GetWantedGenderNames() => Enum.GetNames(typeof(NPCTrade.WantedGender));

        public static string GetGenderName(Gender gender)
        {
            switch (INIManager.Language)
            {
                case Languages.ENGLISH:
                    return gender.ToString();
                case Languages.FRENCH:
                    if (gender == Gender.MALE)
                        return "Mâle";
                    else if (gender == Gender.FEMALE)
                        return "Femelle";
                    else
                        return "Inconnu";
                case Languages.SPANISH:
                    if (gender == Gender.MALE)
                        return "Macho";
                    else if (gender == Gender.FEMALE)
                        return "Hembra";
                    else
                        return "Sin sexo";
                case Languages.GERMAN:
                    if (gender == Gender.MALE)
                        return "Männlich";
                    else if (gender == Gender.FEMALE)
                        return "Weiblich";
                    else
                        return "Unbekannt";
                case Languages.ITALIAN:
                    if (gender == Gender.MALE)
                        return "Maschio";
                    else if (gender == Gender.FEMALE)
                        return "Femmina";
                    else
                        return "Unbekannt";
                case Languages.JAPANESE:
                    if (gender == Gender.MALE)
                        return "オス";
                    else if (gender == Gender.FEMALE)
                        return "メス";
                    else
                        return "性別不明";
                case Languages.KOREAN:
                    if (gender == Gender.MALE)
                        return "オス";
                    else if (gender == Gender.FEMALE)
                        return "メス";
                    else
                        return "性別不明";
                default:
                    return gender.ToString();
            }
        }

		public static string[] GetTradePokemonNickNames() => TradePokemonNicknames.ToArray();
		public static string[] GetTradePokemonTrainerNames() => TradePokemonTrainerNames.ToArray();
    }
}
