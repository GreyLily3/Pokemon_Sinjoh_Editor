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

        private const int UINT_NUM_BYTES = 4;

        private const int MOVE_TUTOR_TABLE_BIN_HGSS = 0x1E6;
        private const int JP_KR_MOVE_TUTOR_TABLE_BIN_HGSS = 0x1E4;

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

			movesNarc = new NarcFile(NarcFile.GetMovesNarcOffset(fat), romFileReader);
			pokemonSpeciesNarc = new NarcFile(NarcFile.GetSpeciesNarcOffset(fat, GameVersion), romFileReader);
            pokedexNarc = new NarcFile(NarcFile.GetPokedexNarcOffset(fat), romFileReader);
            npcTradesNarc = new NarcFile(NarcFile.GetNPCTradesNarcOffset(fat), romFileReader);
            itemsNarc = new NarcFile(NarcFile.GetItemsNarcOffset(fat), romFileReader);
            levelUpMovesNarc = new NarcFile(NarcFile.GetLearnsetNarcOffset(fat), romFileReader);
            gameTextNarc = new NarcFile(NarcFile.GetTextNarcOffset(fat), romFileReader);

            readOverlays(romFileReader);

            if (gameFamily == GameFamilies.HGSS)
                eggMovesHGSSNarc = new NarcFile(NarcFile.GetEggMoveNarcOffset(fat), romFileReader);

            gameText = new TextArchive(gameTextNarc, Language == Languages.KOREAN);

            SetupTextLists();

            MoveList.Clear();
            PokemonSpeciesList.Clear();
            NPCTradesList.Clear();
            ItemList.Clear();
            LevelUpMovesList.Clear();
            HeightList.Clear();
            WeightList.Clear();
            MoveTutorTableList.Clear();
            moveTutorPoolPL.Clear();

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

                for (int i = UINT_NUM_BYTES; i < pokedexNarc.Elements[NarcFile.POKEDEX_HEIGHT_ELEMENT_INDEX].Length; i += UINT_NUM_BYTES)
                {
                    HeightList.Add(new Height(pokedexHeightReader.ReadUInt32()));
                }
            }

            //weight is stored in the 1st element of the narc
            using (BinaryReader pokedexWeightReader = new BinaryReader(pokedexNarc.Elements[1]))
            {
                pokedexWeightReader.ReadUInt32();

                for (int i = UINT_NUM_BYTES; i < pokedexNarc.Elements[NarcFile.POKEDEX_WEIGHT_ELEMENT_INDEX].Length; i += UINT_NUM_BYTES)
                {
                    WeightList.Add(new Weight(pokedexWeightReader.ReadUInt32()));
                }
            }


        }

        private static void readOverlays(BinaryReader romFileReader)
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

                    pokedexNarc.Elements[NarcFile.POKEDEX_HEIGHT_ELEMENT_INDEX] = (MemoryStream)heightWriter.BaseStream;
                };

                using (BinaryWriter weightWriter = new BinaryWriter(new MemoryStream(HeightList.Count * UINT_NUM_BYTES)))
                {
                    for (int i = 0; i < WeightList.Count; i++)
                        weightWriter.Write(WeightList[i].hectograms);

                    pokedexNarc.Elements[NarcFile.POKEDEX_WEIGHT_ELEMENT_INDEX] = (MemoryStream)weightWriter.BaseStream;
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
                speciesNames[PokemonSpecies.BAD_EGG_SPECIES_INDEX + PokemonSpecies.START_INDEX + i] = altFormNames[i];

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
                altFormNames = new string[PokemonSpecies.NUM_ALT_FORMS_DP];
            else
                altFormNames = new string[PokemonSpecies.NUM_ALT_FORMS_PL_HGSS];

            altFormNames[0] = PokemonNames[PokemonSpecies.DEOXYS_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetDeoxysAttackFormNameIndex()] + ")";
            altFormNames[1] = PokemonNames[PokemonSpecies.DEOXYS_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetDeoxysDefenseFormNameIndex()] + ")";
            altFormNames[2] = PokemonNames[PokemonSpecies.DEOXYS_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetDeoxysSpeedFormNameIndex()] + ")";
            altFormNames[3] = PokemonNames[PokemonSpecies.WORMADAM_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetWormadamSandyFormNameIndex()] + ")";
            altFormNames[4] = PokemonNames[PokemonSpecies.WORMADAM_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetWormadamTrashFormNameIndex()] + ")";

            if (gameFamily != GameFamilies.DP)
            {
                altFormNames[5] = PokemonNames[PokemonSpecies.GIRATINA_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetGiratinaOriginFormNameIndex()] + ")";
                altFormNames[6] = PokemonNames[PokemonSpecies.SHAYMIN_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetShayminSkyFormNameIndex()] + ")";
                altFormNames[7] = PokemonNames[PokemonSpecies.ROTOM_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetRotomHeatFormNameIndex()] + ")";
                altFormNames[8] = PokemonNames[PokemonSpecies.ROTOM_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetRotomWashFormNameIndex()] + ")";
                altFormNames[9] = PokemonNames[PokemonSpecies.ROTOM_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetRotomFrostFormNameIndex()] + ")";
                altFormNames[10] = PokemonNames[PokemonSpecies.ROTOM_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetRotomFanFormNameIndex()] + ")";
                altFormNames[11] = PokemonNames[PokemonSpecies.ROTOM_INDEX - PokemonSpecies.START_INDEX] + " (" + PokedexText[TextArchive.GetRotomMowFormNameIndex()] + ")";
            }

            return altFormNames;
        }

        public static string[] GetItemNamesWithoutUnknown()
        {
            List<string> itemNames = new List<string>(itemsNarc.Elements.Count);

            int firstUnknownItemIndex;
            int lastUnknownItemIndex;

            if (gameFamily == GameFamilies.DP)
                firstUnknownItemIndex = Item.UNKNOWN_BLOCK_FIRST_INDEX_DP;
            else
                firstUnknownItemIndex = Item.UNKNOWN_BLOCK_FIRST_INDEX_PLHGSS;

            for (int i = 0; i < firstUnknownItemIndex; i++)
                itemNames.Add(ItemNames[i]);

            if (gameFamily == GameFamilies.HGSS)
            {
                lastUnknownItemIndex = Item.UNKNOWN_LAST_INDEX_HGSS;

                for (int i = Item.UNKNOWN_BLOCK_LAST_INDEX + 1; i < Item.UNKNOWN_LAST_INDEX_HGSS; i++)
                    itemNames.Add(ItemNames[i]);
            }
            else
                lastUnknownItemIndex = Item.UNKNOWN_BLOCK_LAST_INDEX;

                for (int i = lastUnknownItemIndex + 1; i < ItemNames.Count; i++)
                    itemNames.Add(ItemNames[i]);

            return itemNames.ToArray();
        }


        
    }
}
