using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pokemon_Sinjoh_Editor
{
	public static class RomFile
	{
        private static Dictionary<int, Overlay> Overlays = new Dictionary<int, Overlay>();
		private static NarcFile movesNarc;
		private static NarcFile pokemonSpeciesNarc;
		private static NarcFile npcTradesNarc;
		private static NarcFile gameTextNarc;
        private static NarcFile itemsNarc;
        private static NarcFile pokedexNarc;
        private static NarcFile levelUpMovesNarc;
        private static NarcFile eggMovesHGSSNarc;
        private static PartialOverlay moveTutorLearnsetPL;
        private static PartialOverlay moveTutorPoolPartOverlayPL;
        
		public static TextArchive gameText;
        public static Languages Language;
		private static GameVersions GameVersion;
        public static GameFamilies gameFamily = GameFamilies.NULL;
		private static string romPath;
		public static bool AreUnsavedChanges = false;
        public static bool UnsavedChangesMoves = false;
        public static bool UnsavedChangesSpecies = false;
        public static bool UnsavedChangesTrades = false;
        public static bool UnsavedChangesItems = false;
        public static bool UnsavedChangesPokedex = false;
        public static bool UnsavedChangesLevelUpMoves = false;
        public static bool UnsavedChangesMoveTutorMoves = false;
        public static bool UnsavedChangesEggMoves = false;

        public static List<Move> MoveList = new List<Move>();
        public static List<PokemonSpecies> PokemonSpeciesList = new List<PokemonSpecies>();
		public static List<NPCTrade> NPCTradesList = new List<NPCTrade>();
        public static List<Item> ItemList = new List<Item>();
        public static List<Height> HeightList = new List<Height>();
        public static List<Weight> WeightList = new List<Weight>();
        public static List<Learnset> LevelUpMovesList = new List<Learnset>();
        public static List<MoveTutorTable> MoveTutorTableList = new List<MoveTutorTable>();
        public static List<TutorMoveEntryPL> moveTutorPoolPL = new List<TutorMoveEntryPL>();

        public static List<string> MoveNames { get; private set; }
        public static List<string> MoveDescriptions { get; private set; }
        public static List<string> PokedexCategoryNames { get; private set; }
        public static List<string> PokedexDescriptions { get; private set; }
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

        private const int POKEDEX_NARC_HEIGHT_INDEX = 0;
        private const int POKEDEX_NARC_WEIGHT_INDEX = 1;

        private const int UINT_NUM_BYTES = 4;

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
		private const int MOVES_NARC_ID_PL = 0x1BD;
		private const int MOVES_NARC_ID_HGSS = 0x8C;
        private const int JAP_MOVES_NARC_ID_DP = 0x159;
        private const int JAP_MOVES_NARC_ID_PL = 0x1C3;
        private const int JAP_MOVES_NARC_ID_HGSS = 0x8B;
        private const int KOR_MOVES_NARC_ID_PL = 0x1A3;
        private const int KOR_MOVES_NARC_ID_DP = 0x144;

        private const int POKEMON_SPECIES_NARC_ID_DIAMOND = 0x146;
		private const int POKEMON_SPECIES_NARC_ID_PEARL = 0x148;
		private const int POKEMON_SPECIES_NARC_ID_PL = 0x1A5;
		private const int POKEMON_SPECIES_NARC_ID_HGSS = 0x83;
        private const int JAP_POKEMON_SPECIES_NARC_ID_D = 0x147;
        private const int JAP_POKEMON_SPECIES_NARC_ID_P = 0x149;
        private const int JAP_POKEMON_SPECIES_NARC_ID_PL = 0x1AB;
        private const int JAP_POKEMON_SPECIES_NARC_ID_HGSS = 0x82;
        private const int KOR_POKEMON_SPECIES_NARC_ID_PL = 0x18B;
        private const int KOR_POKEMON_SPECIES_NARC_ID_D = 0x132;
        private const int KOR_POKEMON_SPECIES_NARC_ID_P = 0x134;

        private const int POKEDEX_NARC_ID_DP = 0x5A; //same for japanese & korean
        private const int POKEDEX_NARC_ID_PL = 0x82; //same for japanese & korean, there is a seemingly identical narc at 0x81, but this one has gira in the name so it's likely the one platinum uses
        private const int POKEDEX_NARC_ID_HGSS = 0x157; //same for korean
        private const int JAP_POKEDEX_NARC_ID_HGSS = 0x156;

        private const int NPC_TRADES_NARC_ID_DP = 0x10E;
        private const int NPC_TRADES_NARC_ID_PL = 0x150;
        private const int NPC_TRADES_NARC_ID_HGSS = 0xF1;
        private const int JAP_NPC_TRADES_NARC_ID_PL = 0x151;
        private const int JAP_NPC_TRADES_NARC_ID_HGSS = 0xF0;
        private const int KOR_NPC_TRADES_NARC_ID_PL = 0x1BB;
        private const int KOR_NPC_TRADES_NARC_ID_DP = 0x152;

        private const int TEXT_NARC_ID_DP = 0x13D;
		private const int TEXT_NARC_ID_PL = 0x194;
		private const int TEXT_NARC_ID_HGSS = 0x9C;
        private const int JAP_TEXT_NARC_ID_DP = 0x13E;
        private const int JAP_TEXT_NARC_ID_PL = 0x19A;
        private const int JAP_TEXT_NARC_ID_HGSS = 0x9B;
        private const int KOR_TEXT_NARC_ID_DP = 0x129;
        private const int KOR_TEXT_NARC_ID_PL = 0x17A;

        private const int ITEMS_NARC_ID_DP = 0x13A;
        private const int ITEMS_NARC_ID_PL = 0x192;
        private const int ITEMS_NARC_ID_HGSS = 0x92;
        private const int JAP_ITEMS_NARC_ID_DP = 0x13B;
        private const int JAP_ITEMS_NARC_ID_PL = 0x198;
        private const int JAP_ITEMS_NARC_ID_HGSS = 0x91;
        private const int KOR_ITEMS_NARC_ID_DP = 0x126;
        private const int KOR_ITEMS_NARC_ID_PL = 0x178;
        private const int KOR_ITEMS_NARC_ID_HGSS = 0x92;

        private const int LEVEL_UP_MOVES_NARC_ID_DP = 0x148;
        private const int LEVEL_UP_MOVES_NARC_ID_PL = 0x1A7;
        private const int LEVEL_UP_MOVES_NARC_ID_HGSS = 0xA2;
        private const int JAP_LEVEL_UP_MOVES_NARC_ID_DP = 0x149;
        private const int JAP_LEVEL_UP_MOVES_NARC_ID_PL = 0x1AD;
        private const int JAP_LEVEL_UP_MOVES_NARC_ID_HGSS = 0xA1;
        private const int KOR_LEVEL_UP_MOVES_NARC_ID_DP = 0x134;
        private const int KOR_LEVEL_UP_MOVES_NARC_ID_PL = 0x18D;

        private const int EGG_MOVES_NARC_ID_HGSS = 0x166;
        private const int JAP_EGG_MOVES_NARC_ID_HGSS = 0x165;

        private const int MOVE_TUTOR_TABLE_BIN_HGSS = 0x1E6;
        private const int JP_KR_MOVE_TUTOR_TABLE_BIN_HGSS = 0x1E4;

        private const int SPECIES_START_INDEX = 1;
        private const int MOVE_START_INDEX = 1;

        private const int SPECIES_WORMADAM_INDEX = 413;
        private const int SPECIES_DEOXYS_INDEX = 386;
        private const int SPECIES_GIRATINA_INDEX = 487;
        private const int SPECIES_SHAYMIN_INDEX = 492;
        private const int SPECIES_ROTOM_INDEX = 479;

        public const int TRADE_JASMINE_INDEX = 5;
        public const int TRADE_WEBSTER_INDEX = 7;

        public const int UNKNOWN_ITEM_BLOCK_FIRST_INDEX_PLHGSS = 113;
        public const int UNKNOWN_ITEM_BLOCK_FIRST_INDEX_DP = 112;
        public const int UNKNOWN_ITEM_BLOCK_LAST_INDEX = 134;
        public const int UNKNOWN_ITEM_LAST_INDEX_HGSS = 428;

        public const int POKEMON_NAME_MAX_LENGTH = 10;
        public const int MOVE_NAME_MAX_LENGTH = 12;

        private const int NUM_ALT_FORMS_DP = 5;
        private const int NUM_ALT_FORMS_PL_HGSS = 12;


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

			movesNarc = new NarcFile(getMovesNarcOffset(), romFileReader);
			pokemonSpeciesNarc = new NarcFile(getSpeciesNarcOffset(), romFileReader);
            pokedexNarc = new NarcFile(getPokedexNarcOffset(), romFileReader);
            npcTradesNarc = new NarcFile(getNPCTradesNarcOffset(), romFileReader);
            itemsNarc = new NarcFile(getItemsNarcOffset(), romFileReader);
            levelUpMovesNarc = new NarcFile(getLearnsetNarcOffset(), romFileReader);
            gameTextNarc = new NarcFile(getTextNarcOffset(), romFileReader);

            setupOverlays(romFileReader);

            if (gameFamily == GameFamilies.HGSS)
                eggMovesHGSSNarc = new NarcFile(getEggMoveNarcOffset(), romFileReader);

            gameText = new TextArchive(gameTextNarc, Language == Languages.KOREAN);

            SetupTextLists();

            MoveList.Clear();
            PokemonSpeciesList.Clear();
            NPCTradesList.Clear();
            ItemList.Clear();
            LevelUpMovesList.Clear();

            //skip the first move because it's a placeholder
            for (int i = 1; i < movesNarc.Elements.Count; i++)
				MoveList.Add(new Move(movesNarc.Elements[i]));

			//skip the first pokemon because it's a placeholder
            for (int i = 1; i < pokemonSpeciesNarc.Elements.Count; i++)
                PokemonSpeciesList.Add(new PokemonSpecies(pokemonSpeciesNarc.Elements[i]));

            for (int i = 0; i < npcTradesNarc.Elements.Count; i++)
				NPCTradesList.Add(new NPCTrade(npcTradesNarc.Elements[i]));

			for (int i = 0; i < itemsNarc.Elements.Count; i++)
                ItemList.Add(new Item(itemsNarc.Elements[i]));

            for (int i = 1; i < levelUpMovesNarc.Elements.Count; i++)
                LevelUpMovesList.Add(new Learnset(levelUpMovesNarc.Elements[i]));

            if (gameFamily != GameFamilies.HGSS)
                setEggMoves(Overlays[Overlay.EGG_MOVE_INDEX_DPPL].GetSubsetStream(Overlay.GetEggMovesOffset(gameFamily, Language), Overlay.EGG_MOVES_LENGTH));

            if (gameFamily == GameFamilies.HGSS)
            {
                uint moveTutorTableOffset = fat.GetStartOffset(getMoveTutorTableBinOffset());
                var moveTutorMemStreams = readBinaryTableFile(romFileReader, moveTutorTableOffset, MoveTutorTable.BYTES_PER_SPECIES_HGSS, PokemonSpeciesList.Count);

                foreach (MemoryStream memStream in moveTutorMemStreams)
                    MoveTutorTableList.Add(new MoveTutorTable(memStream));

                setEggMoves(eggMovesHGSSNarc.Elements[0]);
            }
            else if (gameFamily == GameFamilies.PL)
            {
                uint numMoveTutorLearnsetEntries = (uint)(PokemonSpeciesList.Count - PokemonSpecies.NUM_EGG_ENTRIES);
                uint moveTutorPoolOffset = fat.GetStartOffset(Overlay.MOVE_TUTOR_INDEX_PL) + Overlay.MOVE_TUTOR_POOL_OFFSET_PL;
                uint moveTutorLearnsetOffset = fat.GetStartOffset(Overlay.MOVE_TUTOR_INDEX_PL) + Overlay.MOVE_TUTOR_LEARNSET_OFFSET_PL;
                uint moveTutorLearnsetLength = numMoveTutorLearnsetEntries * MoveTutorTable.BYTES_PER_SPECIES_PL;
                uint moveTutorPoolLength = MoveTutorTable.NUM_MOVES_PL * TutorMoveEntryPL.NUM_BYTES_PER;

                moveTutorLearnsetPL = new PartialOverlay(romFileReader, Overlay.MOVE_TUTOR_INDEX_PL, moveTutorLearnsetOffset, moveTutorLearnsetLength);
                moveTutorPoolPartOverlayPL = new PartialOverlay(romFileReader, Overlay.MOVE_TUTOR_INDEX_PL, moveTutorPoolOffset, moveTutorPoolLength);

                List<MemoryStream> moveTutorLearnsetMemStreams = moveTutorLearnsetPL.SplitIntoMemStreams(MoveTutorTable.BYTES_PER_SPECIES_PL);
                List<MemoryStream> moveTutorPoolMemoryStream = moveTutorPoolPartOverlayPL.SplitIntoMemStreams(TutorMoveEntryPL.NUM_BYTES_PER);

                foreach (MemoryStream moveTutorLearnsetMemStream in moveTutorLearnsetMemStreams)
                    MoveTutorTableList.Add(new MoveTutorTable(moveTutorLearnsetMemStream));

                foreach (MemoryStream moveTutorPoolEntry in moveTutorPoolMemoryStream)
                    moveTutorPoolPL.Add(new TutorMoveEntryPL(moveTutorPoolEntry));
            }

            //height is stored in the 0th element of the narc
            using (BinaryReader pokedexHeightReader = new BinaryReader(pokedexNarc.Elements[0]))
            {
                pokedexHeightReader.ReadUInt32();

                for (int i = UINT_NUM_BYTES; i < pokedexNarc.Elements[POKEDEX_NARC_HEIGHT_INDEX].Length; i += UINT_NUM_BYTES)
                {
                    HeightList.Add(new Height(pokedexHeightReader.ReadUInt32()));
                }
            }

            //weight is stored in the 1st element of the narc
            using (BinaryReader pokedexWeightReader = new BinaryReader(pokedexNarc.Elements[1]))
            {
                pokedexWeightReader.ReadUInt32();

                for (int i = UINT_NUM_BYTES; i < pokedexNarc.Elements[POKEDEX_NARC_WEIGHT_INDEX].Length; i += UINT_NUM_BYTES)
                {
                    WeightList.Add(new Weight(pokedexWeightReader.ReadUInt32()));
                }
            }


        }

        private static void setupOverlays(BinaryReader romFileReader)
        {
            if (gameFamily != GameFamilies.HGSS)
            {
                Overlays.Add(5, new Overlay(fat, 5, romFileReader));
            }
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

        private static void SetupTextLists()
        {
            int textBankIndex;
            List<string> tradesTextBank;

            if (gameText.TryGetTextBankIndex(gameFamily, Language, TextBankName.MOVES, out textBankIndex))
                MoveNames = setupTextListWithPlaceholder(textBankIndex);

            if (gameText.TryGetTextBankIndex(gameFamily, Language, TextBankName.MOVEDESCRIPTIONS, out textBankIndex))
                MoveDescriptions = setupTextListWithPlaceholder(textBankIndex);

            if (gameText.TryGetTextBankIndex(gameFamily, Language, TextBankName.POKEMON, out textBankIndex))
                PokemonNames = setupTextListWithPlaceholder(textBankIndex);

            if (gameText.TryGetTextBankIndex(gameFamily, Language, TextBankName.POKEDEXCATEGORIES, out textBankIndex))
                PokedexCategoryNames = setupTextListWithPlaceholder(textBankIndex);

            if (gameText.TryGetTextBankIndex(gameFamily, Language, TextBankName.POKEDEXDESCRIPTIONS, out textBankIndex))
                PokedexDescriptions = setupTextListWithPlaceholder(textBankIndex);

            if (gameText.TryGetTextBankIndex(gameFamily, Language, TextBankName.TYPES, out textBankIndex))
                TypeNames = gameText.TextBanks[textBankIndex];

            if (gameText.TryGetTextBankIndex(gameFamily, Language, TextBankName.ITEMS, out textBankIndex))
                ItemNames = gameText.TextBanks[textBankIndex];

            if (gameText.TryGetTextBankIndex(gameFamily, Language, TextBankName.ABILITIES, out textBankIndex))
                AbilityNames = gameText.TextBanks[textBankIndex];

            if (gameText.TryGetTextBankIndex(gameFamily, Language, TextBankName.POKEDEXMISC, out textBankIndex))
                PokedexText = gameText.TextBanks[textBankIndex];

            if (gameText.TryGetTextBankIndex(gameFamily, Language, TextBankName.NATURES, out textBankIndex))
                NatureNames = gameText.TextBanks[textBankIndex];

            TradePokemonNicknames.Clear();
            TradePokemonTrainerNames.Clear();

            if (gameText.TryGetTextBankIndex(gameFamily, Language, TextBankName.TRADES, out textBankIndex))
            {
                tradesTextBank = gameText.TextBanks[textBankIndex];
                for (int i = 0; i < npcTradesNarc.Elements.Count; i++)
                    TradePokemonNicknames.Add(tradesTextBank[i]);

                //multiply count by 2 because there is a trainer name for each pokemon nickname at the start of the text bank
                for (int i = npcTradesNarc.Elements.Count; i < npcTradesNarc.Elements.Count * 2; i++)
                    TradePokemonTrainerNames.Add(tradesTextBank[i]);
            }
                
        }

        private static List<string> setupTextListWithPlaceholder(int textBankIndex)
        {
            List<string> textList;

            textList = gameText.TextBanks[textBankIndex];
            textList.RemoveAt(0);
            return textList;
        }

		private static uint getMovesNarcOffset()
		{
            if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(JAP_MOVES_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(JAP_MOVES_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(JAP_MOVES_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(KOR_MOVES_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(KOR_MOVES_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(MOVES_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(MOVES_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(MOVES_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(MOVES_NARC_ID_HGSS),
                    _ => 0
                };
            }
               
		}

		private static uint getSpeciesNarcOffset()
		{
            if (Language == Languages.JAPANESE)
            {
                return GameVersion switch
                {
                    GameVersions.DIAMOND => fat.GetStartOffset(JAP_POKEMON_SPECIES_NARC_ID_D),
                    GameVersions.PEARL => fat.GetStartOffset(JAP_POKEMON_SPECIES_NARC_ID_P),
                    GameVersions.PLATINUM => fat.GetStartOffset(JAP_POKEMON_SPECIES_NARC_ID_PL),
                    GameVersions.HEARTGOLD => fat.GetStartOffset(JAP_POKEMON_SPECIES_NARC_ID_HGSS),
                    GameVersions.SOULSILVER => fat.GetStartOffset(JAP_POKEMON_SPECIES_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return GameVersion switch
                {
                    GameVersions.DIAMOND => fat.GetStartOffset(KOR_POKEMON_SPECIES_NARC_ID_D),
                    GameVersions.PEARL => fat.GetStartOffset(KOR_POKEMON_SPECIES_NARC_ID_P),
                    GameVersions.PLATINUM => fat.GetStartOffset(KOR_POKEMON_SPECIES_NARC_ID_PL),
                    GameVersions.HEARTGOLD => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_HGSS),
                    GameVersions.SOULSILVER => fat.GetStartOffset(POKEMON_SPECIES_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else
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
            
		}

        private static uint getPokedexNarcOffset()
        {
            if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(POKEDEX_NARC_ID_DP), //same as all other langauges
                    GameFamilies.PL => fat.GetStartOffset(POKEDEX_NARC_ID_PL), //same as all other langauges
                    GameFamilies.HGSS => fat.GetStartOffset(JAP_POKEDEX_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(POKEDEX_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(POKEDEX_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(POKEDEX_NARC_ID_HGSS),
                    _ => 0
                };
            }
        }

		private static uint getNPCTradesNarcOffset()
		{
            if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(NPC_TRADES_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(JAP_NPC_TRADES_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(JAP_NPC_TRADES_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(KOR_NPC_TRADES_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(KOR_NPC_TRADES_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(NPC_TRADES_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(NPC_TRADES_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(NPC_TRADES_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(NPC_TRADES_NARC_ID_HGSS),
                    _ => 0
                };
            }
        }

        private static uint getItemsNarcOffset()
        { 
            if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(JAP_ITEMS_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(JAP_ITEMS_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(JAP_ITEMS_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(KOR_ITEMS_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(KOR_ITEMS_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(KOR_ITEMS_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(ITEMS_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(ITEMS_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(ITEMS_NARC_ID_HGSS),
                    _ => 0
                };
            }
        }

        private static uint getLearnsetNarcOffset()
        {
            if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(JAP_LEVEL_UP_MOVES_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(JAP_LEVEL_UP_MOVES_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(JAP_LEVEL_UP_MOVES_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(KOR_LEVEL_UP_MOVES_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(KOR_LEVEL_UP_MOVES_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(LEVEL_UP_MOVES_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(LEVEL_UP_MOVES_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(LEVEL_UP_MOVES_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(LEVEL_UP_MOVES_NARC_ID_HGSS),
                    _ => 0
                };
            }
        }

        private static uint getTextNarcOffset()
		{
			if (Language == Languages.JAPANESE)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(JAP_TEXT_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(JAP_TEXT_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(JAP_TEXT_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else if (Language == Languages.KOREAN)
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(KOR_TEXT_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(KOR_TEXT_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(TEXT_NARC_ID_HGSS),
                    _ => 0
                };
            }
            else
            {
                return gameFamily switch
                {
                    GameFamilies.DP => fat.GetStartOffset(TEXT_NARC_ID_DP),
                    GameFamilies.PL => fat.GetStartOffset(TEXT_NARC_ID_PL),
                    GameFamilies.HGSS => fat.GetStartOffset(TEXT_NARC_ID_HGSS),
                    _ => 0
                };
            }
                
		}

        private static uint getEggMoveNarcOffset()
        {
            if (Language == Languages.JAPANESE)
                return fat.GetStartOffset(JAP_EGG_MOVES_NARC_ID_HGSS);
            else
                return fat.GetStartOffset(EGG_MOVES_NARC_ID_HGSS);
        }

        private static int getMoveTutorTableBinOffset()
        {
            if (gameFamily == GameFamilies.HGSS)
            {
                switch (Language)
                {
                    case Languages.JAPANESE:
                    case Languages.KOREAN:
                        return JP_KR_MOVE_TUTOR_TABLE_BIN_HGSS;
                    default:
                        return MOVE_TUTOR_TABLE_BIN_HGSS;
                }
            }
            else
            {
                return 0; //placeholder value
            }
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

        public static int GetNumPokemon() => WeightList.Count;

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

        private static void setEggMoves(MemoryStream eggMoveStream)
        {
            int pokemonIndex = 0;
            using (var eggMoveStreamReader = new BinaryReader(eggMoveStream))
            {
                ushort buffer = eggMoveStreamReader.ReadUInt16();

                while (buffer != PokemonSpecies.EGG_MOVE_TABLE_TERMINATOR)
                {
                    if (buffer > PokemonSpecies.EGG_MOVE_TABLE_POKEMON_INDEX_INDICATOR)
                        pokemonIndex = buffer - PokemonSpecies.EGG_MOVE_TABLE_POKEMON_INDEX_INDICATOR - PokemonSpecies.START_INDEX;
                    else
                        PokemonSpeciesList[pokemonIndex].EggMoves.Add(buffer);

                    buffer = eggMoveStreamReader.ReadUInt16();
                }
            }
        }

        private static MemoryStream getEggMovesStream()
        {
            MemoryStream eggMoveStream = new MemoryStream();
            var eggMoveWriter = new BinaryWriter(eggMoveStream);

            for (int i = 0; i < PokemonSpeciesList.Count; i++)
            {
                if (PokemonSpeciesList[i].GetHasEggMoves())
                {
                    eggMoveWriter.Write((ushort)(i + PokemonSpecies.START_INDEX + PokemonSpecies.EGG_MOVE_TABLE_POKEMON_INDEX_INDICATOR));

                    for (int j = 0; j < PokemonSpeciesList[i].EggMoves.Count; j++)
                        eggMoveWriter.Write(PokemonSpeciesList[i].EggMoves[j]);
                }
            }

            eggMoveWriter.Write(PokemonSpecies.EGG_MOVE_TABLE_TERMINATOR);

            return eggMoveStream;
        }

        private static List<MemoryStream> readBinaryTableFile(BinaryReader binaryReader, uint subFileOffset, int rowNumBytes, int numRows)
        {
            List<MemoryStream> binaryStreams = new List<MemoryStream>();
            BinaryWriter binaryStreamWriter;
            binaryReader.BaseStream.Position = subFileOffset;

            for (int i = 0; i < numRows; i++)
            {
                binaryStreams.Add(new MemoryStream(rowNumBytes));
                using (binaryStreamWriter = new BinaryWriter(binaryStreams[i],Encoding.UTF8, true))
                    binaryStreamWriter.Write(binaryReader.ReadBytes(rowNumBytes));
            }

            return binaryStreams;
        }

        private static void WriteBinaryTableFile(BinaryWriter binaryWriter, List<Byte[]> binaryBytes, uint subFileOffset)
        {
            binaryWriter.BaseStream.Position = subFileOffset;

            foreach (Byte[] bytes in binaryBytes)
                binaryWriter.Write(bytes);
        }

		public static void Write()
		{
			FileStream romFileStream = new FileStream(romPath, FileMode.Open);
            BinaryWriter romWriter = new BinaryWriter(romFileStream, Encoding.UTF8, true);
            uint moveTutorTableOffset;

            if (UnsavedChangesMoves)
            {
                for (int i = 0; i < MoveList.Count; i++)
                    movesNarc.Elements[i + 1] = MoveList[i].GetBinary(); //skip the first move in movesNarc because it's a placeholder
            }
			
            if (UnsavedChangesSpecies)
            {
                for (int i = 0; i < PokemonSpeciesList.Count; i++)
                    pokemonSpeciesNarc.Elements[i + 1] = PokemonSpeciesList[i].GetBinary(); //skip the first pokemon in pokemonSpeciesNarc because it's a placeholder
            }

			if (UnsavedChangesTrades)
            {
                for (int i = 0; i < NPCTradesList.Count; i++)
                    npcTradesNarc.Elements[i] = NPCTradesList[i].GetBinary();
            }

            if (UnsavedChangesLevelUpMoves)
            {
                for (int i = 0; i < LevelUpMovesList.Count; i++)
                    levelUpMovesNarc.Elements[i + 1] = LevelUpMovesList[i].GetBinary();
            }

            if (UnsavedChangesMoveTutorMoves)
            {
                if (gameFamily == GameFamilies.HGSS)
                {
                    var moveTutorLearnsetBytesList = new List<Byte[]>();
                    moveTutorTableOffset = fat.GetStartOffset(getMoveTutorTableBinOffset());

                    for (int i = 0; i < MoveTutorTableList.Count; i++)
                        moveTutorLearnsetBytesList.Add(MoveTutorTableList[i].GetBinaryByteArray());

                    try
                    {
                        WriteBinaryTableFile(romWriter, moveTutorLearnsetBytesList, moveTutorTableOffset);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("An exception occured while attempting to save move tutor data.\n" + e.Message);
                    }
                    
                }
                else if (gameFamily == GameFamilies.PL)
                {
                    Byte[] wholeMoveTutorLearnset = new byte[MoveTutorTableList.Count * MoveTutorTable.BYTES_PER_SPECIES_PL];

                    for (int i = 0; i < MoveTutorTableList.Count; i++)
                        MoveTutorTableList[i].GetBinaryByteArray().CopyTo(wholeMoveTutorLearnset, i * MoveTutorTable.BYTES_PER_SPECIES_PL);

                    moveTutorLearnsetPL.Content = wholeMoveTutorLearnset;

                    try
                    {
                        moveTutorLearnsetPL.WriteUncompressed(romWriter);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("An exception occured while attempting to save move tutor data.\n" + e.Message);
                    }
                }
            }

			if (UnsavedChangesPokedex)
            {
                using (BinaryWriter heightWriter = new BinaryWriter(new MemoryStream(HeightList.Count * UINT_NUM_BYTES)))
                {
                    for (int i = 0; i < HeightList.Count; i++)
                        heightWriter.Write(HeightList[i].decimeters);

                    pokedexNarc.Elements[POKEDEX_NARC_HEIGHT_INDEX] = (MemoryStream)heightWriter.BaseStream;
                };

                using (BinaryWriter weightWriter = new BinaryWriter(new MemoryStream(HeightList.Count * UINT_NUM_BYTES)))
                {
                    for (int i = 0; i < WeightList.Count; i++)
                        weightWriter.Write(WeightList[i].hectograms);

                    pokedexNarc.Elements[POKEDEX_NARC_WEIGHT_INDEX] = (MemoryStream)weightWriter.BaseStream;
                };
            }

            if (UnsavedChangesEggMoves)
            {
                if (gameFamily == GameFamilies.HGSS)
                    eggMovesHGSSNarc.Elements[0] = getEggMovesStream();
                else
                {
                    Overlays[Overlay.EGG_MOVE_INDEX_DPPL].ReplaceSubset(getEggMovesStream(), Overlay.GetEggMovesOffset(gameFamily, Language));
                }
            }

            try
			{
                if (UnsavedChangesMoves)
                {
                    movesNarc.Write(romWriter);
                    UnsavedChangesMoves = false;
                }

                if (UnsavedChangesSpecies)
                {
                    pokemonSpeciesNarc.Write(romWriter);
                    UnsavedChangesSpecies = false;
                } 

                if (UnsavedChangesTrades)
                {
                    npcTradesNarc.Write(romWriter);
                    UnsavedChangesTrades = false;
                }
                    
                if (UnsavedChangesPokedex)
                {
                    pokedexNarc.Write(romWriter);
                    UnsavedChangesPokedex = false;
                }

                if (UnsavedChangesLevelUpMoves)
                {
                    levelUpMovesNarc.Write(romWriter);
                    UnsavedChangesLevelUpMoves = false;
                }

                if (UnsavedChangesEggMoves)
                {
                    if (gameFamily == GameFamilies.HGSS)
                        eggMovesHGSSNarc.Write(romWriter);
                    else
                        Overlays[Overlay.EGG_MOVE_INDEX_DPPL].Write(romWriter);

                    UnsavedChangesEggMoves = false;
                }

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
                Languages.KOREAN => "기술머신",
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
                Languages.KOREAN => "비전머신",
                _ => "HM0"
            };

            for (int i = 0; i < 8; i++)
                HMNames[i] = hmString + (i + 1) + " " + MoveNames[HMIndices[i] - MOVE_START_INDEX];

            return HMNames;
        }

        public static string GetAbilityName(int speciesIndex, PersonalityValue pv)
        {
            int genderRatio = PokemonSpeciesList[speciesIndex].GenderRatio;

            if (PokemonSpeciesList[speciesIndex].GetHasSecondAbility() && pv.GetHasSecondAbility())
                return AbilityNames[PokemonSpeciesList[speciesIndex].Ability2];
            else
                return AbilityNames[PokemonSpeciesList[speciesIndex].Ability1];

        }

        public static string GetAbility1Name(int speciesIndex) => AbilityNames[PokemonSpeciesList[speciesIndex].Ability1];
        public static string GetAbility2Name(int speciesIndex) => AbilityNames[PokemonSpeciesList[speciesIndex].Ability2];

        public static string GetGameVersion() => GameVersion.ToString();
		public static string[] GetMoveNames() => MoveNames.ToArray();

        public static int GetNumTextbanks() => gameText.TextBanks.Length - 1;

        public static string GetMoveDescription(int moveIndex) => MoveDescriptions[moveIndex].Replace("\\n", " ");
        public static string GetPokedexDescription(int pokemonIndex) => PokedexDescriptions[pokemonIndex].Replace("\\n", " ");
        public static string[] GetPokemonSpeciesNames()
		{
			string[] speciesNames = new string[PokemonSpeciesList.Count];

			for (int i = 0; i < PokemonNames.Count; i++)
				speciesNames[i] = PokemonNames[i];

            string[] altFormNames = GetSpeciesAltFormNames();

            for (int i = 0; i < altFormNames.Length; i++)
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + SPECIES_START_INDEX + i] = altFormNames[i];

            return speciesNames;
        }

        public static string[] GetPokemonSpeciesNamesNoAltForms()
        {
            string[] speciesNames = new string[PokemonSpecies.EGG_SPECIES_INDEX];

            for (int i = 0; i < (PokemonSpecies.EGG_SPECIES_INDEX); i++)
                speciesNames[i] = PokemonNames[i];

            return speciesNames;
        }

        public static string[] GetPokemonSpeciesNamesNoEggs()
        {
            string[] speciesNames = new string[PokemonSpeciesList.Count - PokemonSpecies.NUM_EGG_ENTRIES];

            for (int i = 0; i < PokemonNames.Count; i++)
            {
                if (i != PokemonSpecies.EGG_SPECIES_INDEX && i != PokemonSpecies.BAD_EGG_SPECIES_INDEX)
                    speciesNames[i] = PokemonNames[i];
            }

            string[] altFormNames = GetSpeciesAltFormNames();

            for (int i = 0; i < altFormNames.Length; i++)
                speciesNames[PokemonSpecies.EGG_SPECIES_INDEX + i] = altFormNames[i];

            return speciesNames;
        }

        private static string[] GetSpeciesAltFormNames()
        {
            string[] altFormNames;

            if (gameFamily == GameFamilies.DP)
                altFormNames = new string[NUM_ALT_FORMS_DP];
            else
                altFormNames = new string[NUM_ALT_FORMS_PL_HGSS];

            altFormNames[0] = PokemonNames[SPECIES_DEOXYS_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getDeoxysAttackFormNameIndex()] + ")";
            altFormNames[1] = PokemonNames[SPECIES_DEOXYS_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getDeoxysDefenseFormNameIndex()] + ")";
            altFormNames[2] = PokemonNames[SPECIES_DEOXYS_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getDeoxysSpeedFormNameIndex()] + ")";
            altFormNames[3] = PokemonNames[SPECIES_WORMADAM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getWormadamSandyFormNameIndex()] + ")";
            altFormNames[4] = PokemonNames[SPECIES_WORMADAM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getWormadamTrashFormNameIndex()] + ")";

            if (gameFamily != GameFamilies.DP)
            {
                altFormNames[5] = PokemonNames[SPECIES_GIRATINA_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getGiratinaOriginFormNameIndex()] + ")";
                altFormNames[6] = PokemonNames[SPECIES_SHAYMIN_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getShayminSkyFormNameIndex()] + ")";
                altFormNames[7] = PokemonNames[SPECIES_ROTOM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getRotomHeatFormNameIndex()] + ")";
                altFormNames[8] = PokemonNames[SPECIES_ROTOM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getRotomWashFormNameIndex()] + ")";
                altFormNames[9] = PokemonNames[SPECIES_ROTOM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getRotomFrostFormNameIndex()] + ")";
                altFormNames[10] = PokemonNames[SPECIES_ROTOM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getRotomFanFormNameIndex()] + ")";
                altFormNames[11] = PokemonNames[SPECIES_ROTOM_INDEX - SPECIES_START_INDEX] + " (" + PokedexText[getRotomMowFormNameIndex()] + ")";
            }

            return altFormNames;
        }

        public static string GetPokedexCategory(int pokemonIndex) => PokedexCategoryNames[pokemonIndex];
        public static string[] GetItemNames() => ItemNames.ToArray();
        public static string[] GetItemNamesWithoutUnknown()
        {
            List<string> itemNames = new List<string>(itemsNarc.Elements.Count);

            int firstUnknownItemIndex;
            int lastUnknownItemIndex;

            if (gameFamily == GameFamilies.DP)
                firstUnknownItemIndex = UNKNOWN_ITEM_BLOCK_FIRST_INDEX_DP;
            else
                firstUnknownItemIndex = UNKNOWN_ITEM_BLOCK_FIRST_INDEX_PLHGSS;

            for (int i = 0; i < firstUnknownItemIndex; i++)
                itemNames.Add(ItemNames[i]);

            if (gameFamily == GameFamilies.HGSS)
            {
                lastUnknownItemIndex = UNKNOWN_ITEM_LAST_INDEX_HGSS;

                for (int i = UNKNOWN_ITEM_BLOCK_LAST_INDEX + 1; i < UNKNOWN_ITEM_LAST_INDEX_HGSS; i++)
                    itemNames.Add(ItemNames[i]);
            }
            else
                lastUnknownItemIndex = UNKNOWN_ITEM_BLOCK_LAST_INDEX;

                for (int i = lastUnknownItemIndex + 1; i < ItemNames.Count; i++)
                    itemNames.Add(ItemNames[i]);

            return itemNames.ToArray();
        }


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
            switch (INIManager.Language)
            {
                case Languages.ENGLISH:
                    string[] targetNames = Enum.GetNames(typeof(Move.Targets));

                    for (int i = 0; i < targetNames.Length; i++)
                        targetNames[i] = targetNames[i].Replace('_', ' ');
                    return targetNames;
                case Languages.FRENCH:
                    return new string[] { "Normale", "1 OTHER", "1 adv. au hasard", "Adv. proches", "PKMN proches", "Soi", "Côté allié", "Tous côtés", "Côté adv.", "1 allié", "Soi ou 1 allié", "ANY FOE" };
                case Languages.SPANISH:
                    return new string[] { "Normal", "1 OTHER", "1 rival aleatorio", "Rivales cercanos", "Pokémon cercanos", "Usuario", "Aliados de combate", "Todos", "Rivales de combate", "1 aliado", "Usuario o 1 aliado", "ANY FOE" };
                case Languages.GERMAN:
                    return new string[] { "Normal", "1 OTHER", "1 beliebiger Gegner", "Mehrere Gegner", "PKMN im Umkreis", "Anwender", "Eigene Seite", "Beide Seiten", "Gegnerseite", "1 Mitstreiter", "Anwender oder 1 Mitsreiter", "ANY FOE" };
                case Languages.ITALIAN:
                    return new string[] { "Normale", "1 OTHER", "Un nemico a caso", "Più nemici", "Più alleati", "Se stesso", "Tuo campo", "Entrambi i campi", "Campo nemico", "Un alleato", "Se stesso o un alleato", "ANY FOE" };
                case Languages.JAPANESE:
                    return new string[] { "通常", "相手複数", "相手ランダ", "相手複数", "相手", "自分", "味方場", "味方複数", "相手場", "味方１匹", "自分か味方１匹", "相手１匹" };
                case Languages.KOREAN:
                    return new string[] { "통상", "1 OTHER", "상대랜덤1마리", "상대복수", "상대·같은편복수", "자신", "같은편장소", "상대·같으편장소", "상대장소", "같은편1마리", "자신또는같은편1마리", "ANY FOE" };
                default:
                    return new string[0];
            }
        }

        public static string[] GetEggGroupNames() 
        {
            switch (INIManager.Language)
            {
                case Languages.ENGLISH:
                    string[] eggGroupNames = Enum.GetNames(typeof(PokemonSpecies.EggGroups));

                    for (int i = 0; i < eggGroupNames.Length; i++)
                        eggGroupNames[i] = eggGroupNames[i].Replace('_', ' ');

                    return eggGroupNames;
                case Languages.FRENCH:
                    return new string[] { "Monstreux", "Aquatique", "Insectoïde", "Aérien", "Terrestre", "Féerique", "Végétal", "Humanoïde", "Aquatique 3", "Minéral", "Amorphe", "Aquatique 2", "Métamorph", "Draconique", "Inconnu" };
                case Languages.SPANISH:
                    return new string[] { "Monstruo", "Agua 1", "Bicho", "Volador", "Campo", "Hada", "Planta", "Humanoide", "Agua 3", "Mineral", "Amorfo", "Agua 2", "Ditto", "Dragón", "Desconocido" };
                case Languages.GERMAN:
                    return new string[] { "Monster", "Wasser 1", "Käfer", "Flug", "Feld", "Fee", "Pflanze", "Humanotyp", "Wasser 3", "Mineral", "Amorph", "Wasser 2", "Ditto", "Drache", "Unbekannt" };
                case Languages.ITALIAN:
                    return new string[] { "Mostro", "Acqua 1", " Coleottero", "Volante", "Campo", "Magico", "Erba", "Umanoide", "Acqua 3", "Minerale", "Amorfo", "Acqua 2", "Ditto", "Drago", "Sconosciuto" };
                case Languages.JAPANESE:
                    return new string[] { "怪獣"    , "水中 1"   , "虫グ"        , "飛行"   , "陸上"      , "妖精"    , "植物", "人型", "水中 3", "鉱物", "不定形", "水中 2", "メタモン", "ドラゴ", "タマゴ未発見" };
                case Languages.KOREAN:
                    return new string[] { "괴수", "수중 1", "벌레", "비행", "육상", "요정", "식물", "인간형", "수중 3", "광물", "부정형", "수중 2", "메타몽", "드래곤", "알미발견" };
                default:
                    return new string[0];
            }
        }

        public static string[] GetXPGroupNames()
        {
            switch (INIManager.Language)
            {
                case Languages.ENGLISH:
                    string[] xpGroupNames = Enum.GetNames(typeof(PokemonSpecies.XPGroups));

                    for (int i = 0; i < xpGroupNames.Length; i++)
                        xpGroupNames[i] = xpGroupNames[i].Replace('_', ' ');
                    return xpGroupNames;

                case Languages.FRENCH:
                    return new string[] { "Moyenne", "Erratique", "Fluctuante", "Parabolique", "Rapide", "Lente", "???", "???" };
                case Languages.SPANISH:
                    return new string[] { "Medio", "Errático", "Fluctuante", "Parabólico", "Rápido", "Lento", "???", "???" };
                case Languages.GERMAN:
                    return new string[] { "Mittel-Schnell", "Erratic", "Fluctuating", "Mittel-Langsam", "Schnell", "Langsam", "???", "???" };
                case Languages.ITALIAN:
                    return new string[] { "Medio-veloce", "Irregolare", "Fluttuante", "Medio-lenta", "Veloce", "Lenta", "???", "???" };
                case Languages.JAPANESE:
                    return new string[] { "100万タイプ", "60万タイプ", "164万タイプ", "105万タイプ", "80万タイプ", "125万タイプ", "???", "???" };
                case Languages.KOREAN: 
                    return new string[] { "MEDIUM_FAST", "ERRATIC", "FLUCTUATING", "MEDIUM_SLOW", "FAST", "SLOW", "UNUSED1", "UNUSED2" };
                default:
                    return new string[0];
            }
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
                Languages.KOREAN => new string[] { "일본어", "영어", "프랑스어", "이탈리아어", "독일어", "???", "스페인어", "한국어" },
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
                        return "수컷";
                    else if (gender == Gender.FEMALE)
                        return "암컷";
                    else
                        return "성별 불명";
                default:
                    return gender.ToString();
            }
        }

		public static string[] GetTradePokemonNickNames() => TradePokemonNicknames.ToArray();
		public static string[] GetTradePokemonTrainerNames() => TradePokemonTrainerNames.ToArray();
        public static string[] GetFieldPocketNames() => Enum.GetNames(typeof(Item.FieldPockets));
        public static string[] GetBattlePocketNames() => Enum.GetNames(typeof(Item.BattlePockets));
    }
}
