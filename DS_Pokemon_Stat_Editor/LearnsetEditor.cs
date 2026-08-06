using Pokemon_Sinjoh_Editor.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Pokemon_Sinjoh_Editor
{
    partial class MainForm
    {
        List<ComboBox> levelUpMovesComboBoxList = new List<ComboBox>();
        List<NumericNoArrows> levelUpMoveNumericNoArrowsList = new List<NumericNoArrows>();
        List<ComboBox> learnsetEggMovesComboBoxes = new List<ComboBox>();
        List<int> learnsetPreviousEggMoves = new List<int>(PokemonSpecies.MAX_EGG_MOVES);
        bool learnsetControlsCanRecieveUserInput;

        private void setupLearnsetText()
        {
            learnsetPokemonComboBox.Items.Clear();

            if (levelUpMovesComboBoxList.Count == 0)
            {
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove1ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove2ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove3ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove4ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove5ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove6ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove7ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove8ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove9ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove10ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove11ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove12ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove13ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove14ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove15ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove16ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove17ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove18ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove19ComboBox);
                levelUpMovesComboBoxList.Add(learnsetLevelUpMove20ComboBox);

            }

            if (levelUpMoveNumericNoArrowsList.Count == 0)
            {
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel1NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel2NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel3NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel4NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel5NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel6NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel7NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel8NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel9NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel10NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel11NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel12NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel13NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel14NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel15NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel16NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel17NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel18NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel19NumericNoArrows);
                levelUpMoveNumericNoArrowsList.Add(learnsetLevelUpLevel20NumericNoArrows);
            }

            if (learnsetEggMovesComboBoxes.Count == 0)
            {
                learnsetEggMovesComboBoxes.Add(learnsetEggMove1ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove2ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove3ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove4ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove5ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove6ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove7ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove8ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove9ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove10ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove11ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove12ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove13ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove14ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove15ComboBox);
                learnsetEggMovesComboBoxes.Add(learnsetEggMove16ComboBox);
            }

            if (learnsetPreviousEggMoves.Count == 0)
            {
                for (int i = 0; i < PokemonSpecies.MAX_EGG_MOVES; i++)
                    learnsetPreviousEggMoves.Add(0);
            }

            foreach (ComboBox levelUpMoveComboBox in levelUpMovesComboBoxList)
            {
                levelUpMoveComboBox.Items.Clear();
                levelUpMoveComboBox.Items.AddRange(RomFile.MoveNames.ToArray());
            }

            foreach (ComboBox eggMoveComboBox in learnsetEggMovesComboBoxes)
            {
                eggMoveComboBox.Items.Clear();
                eggMoveComboBox.Items.AddRange(RomFile.MoveNames.ToArray());
            }

            learnsetMoveTutorCheckedListBox.Items.Clear();
            learnsetTMCheckedListBox.Items.Clear();
            learnsetHMCheckedListBox.Items.Clear();

            learnsetPokemonComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNamesNoAltForms());
            learnsetTMCheckedListBox.Items.AddRange(TextArchive.GetTMNames());
            learnsetHMCheckedListBox.Items.AddRange(TextArchive.GetHMNames());

            if (RomFile.gameFamily == RomFile.GameFamilies.HGSS)
                learnsetMoveTutorCheckedListBox.Items.AddRange(MoveTutorTable.GetTutorMoveNamesHGSS());
            else if (RomFile.gameFamily == RomFile.GameFamilies.PL)
            {
                learnsetMoveTutorCheckedListBox.BeginUpdate();

                foreach (TutorMoveEntryPL tutorMove in RomFile.moveTutorPoolPL)
                    learnsetMoveTutorCheckedListBox.Items.Add(RomFile.MoveNames[tutorMove.MoveID - Pokemon_Sinjoh_Editor.Move.START_INDEX]);

                learnsetMoveTutorCheckedListBox.EndUpdate();
            }


        }

        private void UpdateDisplayedLearnsetValues()
        {
            if (RomFile.gameFamily == RomFile.GameFamilies.DP)
            {
                learnsetMoveTutorCheckedListBox.Visible = false;
                learnsetMoveTutorLabel.Visible = false;
            }
            else
            {
                learnsetMoveTutorCheckedListBox.Visible = true;
                learnsetMoveTutorLabel.Visible = true;
            }

            //event handlers have to removed when checkedlistboxes are updated, otherwise they'll fire when the user isn't interacting with them
            learnsetTMCheckedListBox.ItemCheck -= speciesTMCheckedListBox_ItemCheck;
            learnsetHMCheckedListBox.ItemCheck -= speciesHMCheckedListBox_ItemCheck;
            learnsetMoveTutorCheckedListBox.ItemCheck -= learnsetMoveTutorCheckedListBox_ItemCheck;
            learnsetPokemonComboBox.SelectedIndex = 0;
            learnsetMoveTutorCheckedListBox.ItemCheck += learnsetMoveTutorCheckedListBox_ItemCheck;
            learnsetTMCheckedListBox.ItemCheck += new ItemCheckEventHandler(speciesTMCheckedListBox_ItemCheck);
            learnsetHMCheckedListBox.ItemCheck += new ItemCheckEventHandler(speciesHMCheckedListBox_ItemCheck);
        }

        private void displayLearnsetValues(int pokemonIndex)
        {
            List<int> learnableTutorMoves;

            learnsetControlsCanRecieveUserInput = false;

            int numMoves = RomFile.LevelUpMovesList[pokemonIndex].GetNumMoves();

            for (int i = 0; i < numMoves; i++)
            {
                levelUpMovesComboBoxList[i].SelectedIndex = RomFile.LevelUpMovesList[pokemonIndex].GetMoveID(i) - Pokemon_Sinjoh_Editor.Move.START_INDEX;
                levelUpMoveNumericNoArrowsList[i].Value = RomFile.LevelUpMovesList[pokemonIndex].LevelsLearned[i];
                levelUpMovesComboBoxList[i].Visible = true;
                levelUpMoveNumericNoArrowsList[i].Visible = true;
            }

            for (int i = numMoves; i < levelUpMovesComboBoxList.Count; i++)
            {
                levelUpMovesComboBoxList[i].Visible = false;
                levelUpMoveNumericNoArrowsList[i].Visible = false;
            }

            int numEggMoves = RomFile.PokemonSpeciesList[pokemonIndex].EggMoves.Count;

            for (int i = 0; i < numEggMoves; i++)
            {
                learnsetEggMovesComboBoxes[i].Visible = true;
                learnsetEggMovesComboBoxes[i].SelectedIndex = RomFile.PokemonSpeciesList[pokemonIndex].EggMoves[i] - Pokemon_Sinjoh_Editor.Move.START_INDEX;
            }

            for (int i = numEggMoves; i < learnsetEggMovesComboBoxes.Count; i++)
                learnsetEggMovesComboBoxes[i].Visible = false;

            for (int i = 0; i < numEggMoves; i++)
                learnsetPreviousEggMoves[i] = learnsetEggMovesComboBoxes[i].SelectedIndex;


            List<int> learnableTMs;
            learnableTMs = RomFile.PokemonSpeciesList[pokemonIndex].GetLearnableTMs();

            for (int i = 0; i < learnsetTMCheckedListBox.Items.Count; i++)
                learnsetTMCheckedListBox.SetItemChecked(i, false);

            foreach (int tmIndex in learnableTMs)
                learnsetTMCheckedListBox.SetItemChecked(tmIndex, true);


            List<int> learnableHMs;
            learnableHMs = RomFile.PokemonSpeciesList[pokemonIndex].GetLearnableHMs();

            for (int i = 0; i < learnsetHMCheckedListBox.Items.Count; i++)
                learnsetHMCheckedListBox.SetItemChecked(i, false);

            foreach (int hmIndex in learnableHMs)
                learnsetHMCheckedListBox.SetItemChecked(hmIndex, true);


            if (RomFile.gameFamily != RomFile.GameFamilies.DP)
            {
                //need to account for the fact that the move tutor table has no entries for EGG or BAD EGG
                if (pokemonIndex >= PokemonSpecies.EGG_SPECIES_INDEX)
                {
                    pokemonIndex -= PokemonSpecies.NUM_EGG_ENTRIES;
                }

                learnableTutorMoves = RomFile.MoveTutorTableList[pokemonIndex].GetLearnableMoves(RomFile.gameFamily);

                for (int i = 0; i < learnsetMoveTutorCheckedListBox.Items.Count; i++)
                    learnsetMoveTutorCheckedListBox.SetItemChecked(i, false);

                foreach (int moveIndex in learnableTutorMoves)
                    learnsetMoveTutorCheckedListBox.SetItemChecked(moveIndex, true);
            }

            learnsetControlsCanRecieveUserInput = true;

        }

        private void learnsetPokemonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int speciesIndex = learnsetPokemonComboBox.SelectedIndex;
            string[] altFormNames = TextArchive.GetAltFormsNames(speciesIndex + PokemonSpecies.START_INDEX);

            learnsetAltFormsComboBox.Items.Clear();

            if (altFormNames.Length > 0)
            {
                learnsetAltFormsComboBox.Items.AddRange(altFormNames);
                learnsetAltFormsComboBox.Enabled = true;
                learnsetAltFormsComboBox.SelectedIndex = 0;
                speciesIndex = GetSpeciesAltFormCorrectedIndex(speciesIndex, learnsetAltFormsComboBox.SelectedIndex);
            }
            else
            {
                learnsetAltFormsComboBox.Items.Add(EN_SPECIES_NO_ALT_FORMS_TEXT);
                learnsetAltFormsComboBox.SelectedIndex = 0;
                learnsetAltFormsComboBox.Enabled = false;
            }

            displayLearnsetValues(speciesIndex);
        }

        private void learnsetAltFormsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int speciesIndex = GetSpeciesAltFormCorrectedIndex(learnsetPokemonComboBox.SelectedIndex, learnsetAltFormsComboBox.SelectedIndex);

            displayLearnsetValues(speciesIndex);
        }

        private bool learnsetIsDuplicateEggMove(int moveID)
        {
            int chosenMoveCount = 0;

            for (int i = 0; i < RomFile.PokemonSpeciesList[learnsetPokemonComboBox.SelectedIndex].EggMoves.Count; i++)
            {
                if (learnsetEggMovesComboBoxes[i].SelectedIndex == moveID)
                    chosenMoveCount++;
            }

            return chosenMoveCount > 1;
        }

        private void learnsetUpdateLearnedMoveID(int levelUpMoveIndex, int moveID)
        {
            int speciesIndex = GetSpeciesAltFormCorrectedIndex(learnsetPokemonComboBox.SelectedIndex, learnsetAltFormsComboBox.SelectedIndex);

            moveID += Pokemon_Sinjoh_Editor.Move.START_INDEX;

            if (RomFile.LevelUpMovesList[speciesIndex].GetMoveID(levelUpMoveIndex) != moveID)
            {
                RomFile.LevelUpMovesList[speciesIndex].SetMoveID(levelUpMoveIndex, moveID);
                MarkUnsavedChanges(SaveSubFile.LEVELUPMOVES);
            }
        }

        private void learnsetUpdateLearnedMoveLevel(int levelUpMoveIndex, int level)
        {
            int speciesIndex = GetSpeciesAltFormCorrectedIndex(learnsetPokemonComboBox.SelectedIndex, learnsetAltFormsComboBox.SelectedIndex);

            if (RomFile.LevelUpMovesList[speciesIndex].LevelsLearned[levelUpMoveIndex] != level)
            {
                RomFile.LevelUpMovesList[speciesIndex].LevelsLearned[levelUpMoveIndex] = (ushort)level;
                MarkUnsavedChanges(SaveSubFile.LEVELUPMOVES);
            }
        }

        private void learnsetUpdateEggMove(int eggMoveIndex, int moveID)
        {
            int speciesIndex = GetSpeciesAltFormCorrectedIndex(learnsetPokemonComboBox.SelectedIndex, learnsetAltFormsComboBox.SelectedIndex);

            if (learnsetIsDuplicateEggMove(moveID))
            {
                learnsetEggMovesComboBoxes[eggMoveIndex].SelectedIndex = learnsetPreviousEggMoves[eggMoveIndex];
            }
            else
            {
                moveID += PokemonSpecies.START_INDEX;

                if (RomFile.PokemonSpeciesList[speciesIndex].EggMoves[eggMoveIndex] != moveID)
                {
                    learnsetPreviousEggMoves[eggMoveIndex] = moveID - PokemonSpecies.START_INDEX;
                    RomFile.PokemonSpeciesList[speciesIndex].EggMoves[eggMoveIndex] = (ushort)moveID;
                    MarkUnsavedChanges(SaveSubFile.EGGMOVES);
                }
            }  
        }


        private void learnsetLevelUpMove1ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(0, learnsetLevelUpMove1ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove2ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(1, learnsetLevelUpMove2ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove3ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(2, learnsetLevelUpMove3ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove4ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(3, learnsetLevelUpMove4ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove5ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(4, learnsetLevelUpMove5ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove6ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(5, learnsetLevelUpMove6ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove7ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(6, learnsetLevelUpMove7ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove8ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(7, learnsetLevelUpMove8ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove9ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(8, learnsetLevelUpMove9ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove10ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(9, learnsetLevelUpMove10ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove11ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(10, learnsetLevelUpMove11ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove12ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(11, learnsetLevelUpMove12ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove13ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(12, learnsetLevelUpMove13ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove14ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(13, learnsetLevelUpMove14ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove15ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(14, learnsetLevelUpMove15ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove16ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(15, learnsetLevelUpMove16ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove17ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(16, learnsetLevelUpMove17ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove18ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(17, learnsetLevelUpMove18ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove19ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(18, learnsetLevelUpMove19ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpMove20ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveID(19, learnsetLevelUpMove20ComboBox.SelectedIndex);
        }

        private void learnsetLevelUpLevel1NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel1NumericNoArrows.Value > learnsetLevelUpLevel2NumericNoArrows.Value && learnsetLevelUpLevel2NumericNoArrows.Visible)
                learnsetLevelUpLevel1NumericNoArrows.Value = learnsetLevelUpLevel2NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(0, (int)learnsetLevelUpLevel1NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel2NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel2NumericNoArrows.Value > learnsetLevelUpLevel3NumericNoArrows.Value && learnsetLevelUpLevel3NumericNoArrows.Visible)
                learnsetLevelUpLevel2NumericNoArrows.Value = learnsetLevelUpLevel3NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(1, (int)learnsetLevelUpLevel2NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel3NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel3NumericNoArrows.Value > learnsetLevelUpLevel4NumericNoArrows.Value && learnsetLevelUpLevel4NumericNoArrows.Visible)
                learnsetLevelUpLevel3NumericNoArrows.Value = learnsetLevelUpLevel4NumericNoArrows.Value;
            else if (learnsetLevelUpLevel3NumericNoArrows.Value < learnsetLevelUpLevel2NumericNoArrows.Value)
                learnsetLevelUpLevel3NumericNoArrows.Value = learnsetLevelUpLevel2NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(2, (int)learnsetLevelUpLevel3NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel4NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel4NumericNoArrows.Value > learnsetLevelUpLevel5NumericNoArrows.Value && learnsetLevelUpLevel5NumericNoArrows.Visible)
                learnsetLevelUpLevel4NumericNoArrows.Value = learnsetLevelUpLevel5NumericNoArrows.Value;
            else if (learnsetLevelUpLevel4NumericNoArrows.Value < learnsetLevelUpLevel3NumericNoArrows.Value)
                learnsetLevelUpLevel4NumericNoArrows.Value = learnsetLevelUpLevel3NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(3, (int)learnsetLevelUpLevel4NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel5NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel5NumericNoArrows.Value > learnsetLevelUpLevel6NumericNoArrows.Value && learnsetLevelUpLevel6NumericNoArrows.Visible)
                learnsetLevelUpLevel5NumericNoArrows.Value = learnsetLevelUpLevel6NumericNoArrows.Value;
            else if (learnsetLevelUpLevel5NumericNoArrows.Value < learnsetLevelUpLevel4NumericNoArrows.Value)
                learnsetLevelUpLevel5NumericNoArrows.Value = learnsetLevelUpLevel4NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(4, (int)learnsetLevelUpLevel5NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel6NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel6NumericNoArrows.Value > learnsetLevelUpLevel7NumericNoArrows.Value && learnsetLevelUpLevel7NumericNoArrows.Visible)
                learnsetLevelUpLevel6NumericNoArrows.Value = learnsetLevelUpLevel7NumericNoArrows.Value;
            else if (learnsetLevelUpLevel6NumericNoArrows.Value < learnsetLevelUpLevel5NumericNoArrows.Value)
                learnsetLevelUpLevel6NumericNoArrows.Value = learnsetLevelUpLevel5NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(5, (int)learnsetLevelUpLevel6NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel7NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel7NumericNoArrows.Value > learnsetLevelUpLevel8NumericNoArrows.Value && learnsetLevelUpLevel8NumericNoArrows.Visible)
                learnsetLevelUpLevel7NumericNoArrows.Value = learnsetLevelUpLevel8NumericNoArrows.Value;
            else if (learnsetLevelUpLevel7NumericNoArrows.Value < learnsetLevelUpLevel6NumericNoArrows.Value)
                learnsetLevelUpLevel7NumericNoArrows.Value = learnsetLevelUpLevel6NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(6, (int)learnsetLevelUpLevel7NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel8NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel8NumericNoArrows.Value > learnsetLevelUpLevel9NumericNoArrows.Value && learnsetLevelUpLevel9NumericNoArrows.Visible)
                learnsetLevelUpLevel8NumericNoArrows.Value = learnsetLevelUpLevel9NumericNoArrows.Value;
            else if (learnsetLevelUpLevel8NumericNoArrows.Value < learnsetLevelUpLevel7NumericNoArrows.Value)
                learnsetLevelUpLevel8NumericNoArrows.Value = learnsetLevelUpLevel7NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(7, (int)learnsetLevelUpLevel8NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel9NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel9NumericNoArrows.Value > learnsetLevelUpLevel10NumericNoArrows.Value && learnsetLevelUpLevel10NumericNoArrows.Visible)
                learnsetLevelUpLevel9NumericNoArrows.Value = learnsetLevelUpLevel10NumericNoArrows.Value;
            else if (learnsetLevelUpLevel9NumericNoArrows.Value < learnsetLevelUpLevel8NumericNoArrows.Value)
                learnsetLevelUpLevel9NumericNoArrows.Value = learnsetLevelUpLevel8NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(8, (int)learnsetLevelUpLevel9NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel10NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel10NumericNoArrows.Value > learnsetLevelUpLevel11NumericNoArrows.Value && learnsetLevelUpLevel11NumericNoArrows.Visible)
                learnsetLevelUpLevel10NumericNoArrows.Value = learnsetLevelUpLevel11NumericNoArrows.Value;
            else if (learnsetLevelUpLevel10NumericNoArrows.Value < learnsetLevelUpLevel9NumericNoArrows.Value)
                learnsetLevelUpLevel10NumericNoArrows.Value = learnsetLevelUpLevel9NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(9, (int)learnsetLevelUpLevel10NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel11NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel11NumericNoArrows.Value > learnsetLevelUpLevel12NumericNoArrows.Value && learnsetLevelUpLevel12NumericNoArrows.Visible)
                learnsetLevelUpLevel11NumericNoArrows.Value = learnsetLevelUpLevel12NumericNoArrows.Value;
            else if (learnsetLevelUpLevel11NumericNoArrows.Value < learnsetLevelUpLevel10NumericNoArrows.Value)
                learnsetLevelUpLevel11NumericNoArrows.Value = learnsetLevelUpLevel10NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(10, (int)learnsetLevelUpLevel11NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel12NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel12NumericNoArrows.Value > learnsetLevelUpLevel13NumericNoArrows.Value && learnsetLevelUpLevel13NumericNoArrows.Visible)
                learnsetLevelUpLevel12NumericNoArrows.Value = learnsetLevelUpLevel13NumericNoArrows.Value;
            else if (learnsetLevelUpLevel12NumericNoArrows.Value < learnsetLevelUpLevel11NumericNoArrows.Value)
                learnsetLevelUpLevel12NumericNoArrows.Value = learnsetLevelUpLevel11NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(11, (int)learnsetLevelUpLevel12NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel13NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel13NumericNoArrows.Value > learnsetLevelUpLevel14NumericNoArrows.Value && learnsetLevelUpLevel14NumericNoArrows.Visible)
                learnsetLevelUpLevel13NumericNoArrows.Value = learnsetLevelUpLevel14NumericNoArrows.Value;
            else if (learnsetLevelUpLevel13NumericNoArrows.Value < learnsetLevelUpLevel12NumericNoArrows.Value)
                learnsetLevelUpLevel13NumericNoArrows.Value = learnsetLevelUpLevel12NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(12, (int)learnsetLevelUpLevel13NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel14NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel14NumericNoArrows.Value > learnsetLevelUpLevel15NumericNoArrows.Value && learnsetLevelUpLevel15NumericNoArrows.Visible)
                learnsetLevelUpLevel14NumericNoArrows.Value = learnsetLevelUpLevel15NumericNoArrows.Value;
            else if (learnsetLevelUpLevel14NumericNoArrows.Value < learnsetLevelUpLevel13NumericNoArrows.Value)
                learnsetLevelUpLevel14NumericNoArrows.Value = learnsetLevelUpLevel13NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(13, (int)learnsetLevelUpLevel14NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel15NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel15NumericNoArrows.Value > learnsetLevelUpLevel16NumericNoArrows.Value && learnsetLevelUpLevel16NumericNoArrows.Visible)
                learnsetLevelUpLevel15NumericNoArrows.Value = learnsetLevelUpLevel16NumericNoArrows.Value;
            else if (learnsetLevelUpLevel15NumericNoArrows.Value < learnsetLevelUpLevel14NumericNoArrows.Value)
                learnsetLevelUpLevel15NumericNoArrows.Value = learnsetLevelUpLevel14NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(14, (int)learnsetLevelUpLevel15NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel16NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel16NumericNoArrows.Value > learnsetLevelUpLevel17NumericNoArrows.Value && learnsetLevelUpLevel17NumericNoArrows.Visible)
                learnsetLevelUpLevel16NumericNoArrows.Value = learnsetLevelUpLevel17NumericNoArrows.Value;
            else if (learnsetLevelUpLevel16NumericNoArrows.Value < learnsetLevelUpLevel15NumericNoArrows.Value)
                learnsetLevelUpLevel16NumericNoArrows.Value = learnsetLevelUpLevel15NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(15, (int)learnsetLevelUpLevel16NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel17NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel17NumericNoArrows.Value > learnsetLevelUpLevel18NumericNoArrows.Value && learnsetLevelUpLevel18NumericNoArrows.Visible)
                learnsetLevelUpLevel17NumericNoArrows.Value = learnsetLevelUpLevel18NumericNoArrows.Value;
            else if (learnsetLevelUpLevel17NumericNoArrows.Value < learnsetLevelUpLevel16NumericNoArrows.Value)
                learnsetLevelUpLevel17NumericNoArrows.Value = learnsetLevelUpLevel16NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(16, (int)learnsetLevelUpLevel17NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel18NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel18NumericNoArrows.Value > learnsetLevelUpLevel19NumericNoArrows.Value && learnsetLevelUpLevel19NumericNoArrows.Visible)
                learnsetLevelUpLevel18NumericNoArrows.Value = learnsetLevelUpLevel19NumericNoArrows.Value;
            else if (learnsetLevelUpLevel18NumericNoArrows.Value < learnsetLevelUpLevel17NumericNoArrows.Value)
                learnsetLevelUpLevel18NumericNoArrows.Value = learnsetLevelUpLevel17NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(17, (int)learnsetLevelUpLevel18NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel19NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel19NumericNoArrows.Value > learnsetLevelUpLevel20NumericNoArrows.Value && learnsetLevelUpLevel20NumericNoArrows.Visible)
                learnsetLevelUpLevel19NumericNoArrows.Value = learnsetLevelUpLevel20NumericNoArrows.Value;
            else if (learnsetLevelUpLevel19NumericNoArrows.Value < learnsetLevelUpLevel18NumericNoArrows.Value)
                learnsetLevelUpLevel19NumericNoArrows.Value = learnsetLevelUpLevel18NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(18, (int)learnsetLevelUpLevel19NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel20NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel20NumericNoArrows.Value < learnsetLevelUpLevel19NumericNoArrows.Value)
                learnsetLevelUpLevel20NumericNoArrows.Value = learnsetLevelUpLevel19NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(19, (int)learnsetLevelUpLevel20NumericNoArrows.Value);

        }

        private void learnsetMoveTutorCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int speciesIndex = GetSpeciesAltFormCorrectedIndex(learnsetPokemonComboBox.SelectedIndex, learnsetAltFormsComboBox.SelectedIndex);

            if (speciesIndex >= PokemonSpecies.EGG_SPECIES_INDEX)
                speciesIndex -= PokemonSpecies.NUM_EGG_ENTRIES;

            if (learnsetControlsCanRecieveUserInput)
            {
                RomFile.MoveTutorTableList[speciesIndex].SetLearnableMove(e.Index, e.NewValue.HasFlag(CheckState.Checked));
                MarkUnsavedChanges(SaveSubFile.TUTORLEARNSET);
            }
        }

        private void speciesTMCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int speciesIndex = GetSpeciesAltFormCorrectedIndex(learnsetPokemonComboBox.SelectedIndex, learnsetAltFormsComboBox.SelectedIndex);

            if (learnsetControlsCanRecieveUserInput)
            {
                RomFile.PokemonSpeciesList[speciesIndex].SetLearnableTM(e.Index, e.NewValue.HasFlag(CheckState.Checked));
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesHMCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int speciesIndex = GetSpeciesAltFormCorrectedIndex(learnsetPokemonComboBox.SelectedIndex, learnsetAltFormsComboBox.SelectedIndex);

            if (learnsetControlsCanRecieveUserInput)
            {
                RomFile.PokemonSpeciesList[speciesIndex].SetLearnableHM(e.Index, e.NewValue.HasFlag(CheckState.Checked));
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }


        private void learnsetEggMove1ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(0, learnsetEggMove1ComboBox.SelectedIndex);
        }

        private void learnsetEggMove2ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(1, learnsetEggMove2ComboBox.SelectedIndex);
        }

        private void learnsetEggMove3ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(2, learnsetEggMove3ComboBox.SelectedIndex);
        }

        private void learnsetEggMove4ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(3, learnsetEggMove4ComboBox.SelectedIndex);
        }

        private void learnsetEggMove5ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(4, learnsetEggMove5ComboBox.SelectedIndex);
        }

        private void learnsetEggMove6ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(5, learnsetEggMove6ComboBox.SelectedIndex);
        }

        private void learnsetEggMove7ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(6, learnsetEggMove7ComboBox.SelectedIndex);
        }

        private void learnsetEggMove8ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(7, learnsetEggMove8ComboBox.SelectedIndex);
        }

        private void learnsetEggMove9ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(8, learnsetEggMove9ComboBox.SelectedIndex);
        }

        private void learnsetEggMove10ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(9, learnsetEggMove10ComboBox.SelectedIndex);
        }

        private void learnsetEggMove11ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(10, learnsetEggMove11ComboBox.SelectedIndex);
        }

        private void learnsetEggMove12ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(11, learnsetEggMove12ComboBox.SelectedIndex);
        }

        private void learnsetEggMove13ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(12, learnsetEggMove13ComboBox.SelectedIndex);
        }

        private void learnsetEggMove14ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(13, learnsetEggMove14ComboBox.SelectedIndex);
        }

        private void learnsetEggMove15ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(14, learnsetEggMove15ComboBox.SelectedIndex);
        }

        private void learnsetEggMove16ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            learnsetUpdateEggMove(15, learnsetEggMove16ComboBox.SelectedIndex);
        }
    }
}
