using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
		public static List<string> TradedPokemonAndTrainerNicknames { get; private set; }

		private static FAT fat;

		private static uint FATOffset;
        private static uint FATLength;

		#region Constants

		private const int ROM_NAME_LENGTH = 0xA;
		private const int FAT_POINTER_OFFSET = 0x48;

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
			TradedPokemonAndTrainerNicknames = gameText.TextBanks[getTradePokemonNicknamesAndTrainerNamesTextBankID()];

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

            try
			{
				movesNarc.Write(romWriter);
				pokemonSpeciesNarc.Write(romWriter);
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
			return new string[] { 
				"TM01 Focus Punch",
				"TM02 Dragon Claw",
				"TM03 Water Pulse",
				"TM04 Calm Mind",
				"TM05 Roar",
				"TM06 Toxic",
				"TM07 Hail",
				"TM08 Bulk Up",
				"TM09 Bullet Seed",
				"TM10 Hidden Power",
				"TM11 Sunny Day",
				"TM12 Taunt",
				"TM13 Ice Beam",
				"TM14 Blizzard",
				"TM15 Hyper Beam",
				"TM16 Light Screen",
				"TM17 Protect",
				"TM18 Rain Dance",
				"TM19 Giga Drain",
				"TM20 Safeguard",
				"TM21 Frustration",
				"TM22 SolarBeam",
				"TM23 Iron Tail",
				"TM24 Thunderbolt",
				"TM25 Thunder",
				"TM26 Earthquake",
				"TM27 Return",
				"TM28 Dig",
				"TM29 Psychic",
				"TM30 Shadow Ball",
				"TM31 Brick Break",
				"TM32 Double Team",
				"TM33 Reflect",
				"TM34 Shock Wave",
				"TM35 Flamethrower",
				"TM36 Sludge Bomb",
				"TM37 Sandstorm",
				"TM38 Fire Blast",
				"TM39 Rock Tomb",
				"TM40 Aerial Ace",
				"TM41 Torment",
				"TM42 Facade",
				"TM43 Secret Power",
				"TM44 Rest",
				"TM45 Attract",
				"TM46 Thief",
				"TM47 Steel Wing",
				"TM48 Skill Swap",
				"TM49 Snatch",
				"TM50 Overheat",
				"TM51 Roost",
				"TM52 Focus Blast",
				"TM53 Energy Ball",
				"TM54 False Swipe",
				"TM55 Brine",
				"TM56 Fling",
				"TM57 Charge Beam",
				"TM58 Endure",
				"TM59 Dragon Pulse",
				"TM60 Drain Punch",
				"TM61 Will-O-Wisp",
				"TM62 Silver Wind",
				"TM63 Embargo",
				"TM64 Explosion",
				"TM65 Shadow Claw",
				"TM66 Payback",
				"TM67 Recycle",
				"TM68 Giga Impact",
				"TM69 Rock Polish",
				"TM70 Flash",
				"TM71 Stone Edge",
				"TM72 Avalanche",
				"TM73 Thunder Wave",
				"TM74 Gyro Ball",
				"TM75 Swords Dance",
				"TM76 Stealth Rock",
				"TM77 Psych Up",
				"TM78 Captivate",
				"TM79 Dark Pulse",
				"TM80 Rock Slide",
				"TM81 X-Scissor",
				"TM82 Sleep Talk",
				"TM83 Natural Gift",
				"TM84 Poison Jab",
				"TM85 Dream Eater",
				"TM86 Grass Knot",
				"TM87 Swagger",
				"TM88 Pluck",
				"TM89 U-turn",
				"TM90 Substitute",
				"TM91 Flash Cannon",
				"TM92 Trick Room"
			};
		}

		//will replace this static list later if there's a good way to get/set what moves are set as HMs (this is stored in ARM9 binary)
        public static string[] GetHMNames()
        {
			return new string[] { "HM01 CUT", "HM02 FLY", "HM03 SURF", "HM04 STRENGTH", "HM05 WHIRLPOOL", "HM06 ROCK SMASH", "HM07 WATERFALL", "HM08 ROCK CLIMB" };
        }

        public static string GetGameVersion() => GameVersion.ToString();
		public static string[] GetMoveNames() => MoveNames.ToArray();
		public static string[] GetPokemonSpeciesNames()
		{
			string[] speciesNames = new string[PokemonSpeciesList.Count];

			for (int i = 0; i < PokemonNames.Count; i++)
				speciesNames[i] = PokemonNames[i];

            speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 1] = "DEOXYS (Attack Form)";
            speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 2] = "DEOXYS (Defense Form)";
            speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 3] = "DEOXYS (Speed Form)";
            speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 4] = "WORMADAM (Sandy Form)";
            speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 5] = "WORMADAM (Trash Form)";

            if (gameFamily == GameFamilies.HGSS || gameFamily == GameFamilies.PL)
			{
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 6] = "GIRATINA (Origin Form)";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 7] = "SHAYMIN (Sky Form)";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 8] = "ROTOM (Heat Form)";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 9] = "ROTOM (Wash Form)";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 10] = "ROTOM (Frost Form)";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 11] = "ROTOM (Fan Form)";
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + 12] = "ROTOM (Mow Form)";
            }

			return speciesNames;
        }
		public static string[] GetItemNames() => ItemNames.ToArray();
		public static string[] GetTypeNames() => TypeNames.ToArray();
		public static string[] GetAbilityNames() => AbilityNames.ToArray();
        public static string[] GetMoveCategories() => Enum.GetNames(typeof(Move.Categories));
        public static string[] GetMoveContestConditions() => Enum.GetNames(typeof(Move.ContestConditions));
		public static string[] GetMoveContestEffect() => Move.ContestEffectDescriptions;
        public static string[] GetMoveTargets() => Enum.GetNames(typeof(Move.Targets));
		public static string[] GetEggGroupNames() => Enum.GetNames(typeof(PokemonSpecies.EggGroups));
		public static string[] GetXPGroupNames() => Enum.GetNames(typeof(PokemonSpecies.XPGroups));
		public static string[] GetLanguageNames() => Enum.GetNames(typeof(NPCTrade.Languages));
		public static string[] GetWantedGenderNames() => Enum.GetNames(typeof(NPCTrade.WantedGender));
		public static string[] GetTradePokemonNickNames() => TradedPokemonAndTrainerNicknames.ToArray();
    }
}
