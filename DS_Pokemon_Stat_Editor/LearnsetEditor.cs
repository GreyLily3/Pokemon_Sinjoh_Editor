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
        bool learnsetControlsCanRecieveUserInput;

        private void setupLearnsetText()
        {
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

            learnsetLevelUpMove1ComboBox.Items.Clear();
            learnsetLevelUpMove2ComboBox.Items.Clear();
            learnsetLevelUpMove3ComboBox.Items.Clear();
            learnsetLevelUpMove4ComboBox.Items.Clear();
            learnsetLevelUpMove5ComboBox.Items.Clear();
            learnsetLevelUpMove6ComboBox.Items.Clear();
            learnsetLevelUpMove7ComboBox.Items.Clear();
            learnsetLevelUpMove8ComboBox.Items.Clear();
            learnsetLevelUpMove9ComboBox.Items.Clear();
            learnsetLevelUpMove10ComboBox.Items.Clear();
            learnsetLevelUpMove11ComboBox.Items.Clear();
            learnsetLevelUpMove12ComboBox.Items.Clear();
            learnsetLevelUpMove13ComboBox.Items.Clear();
            learnsetLevelUpMove14ComboBox.Items.Clear();
            learnsetLevelUpMove15ComboBox.Items.Clear();
            learnsetLevelUpMove16ComboBox.Items.Clear();
            learnsetLevelUpMove17ComboBox.Items.Clear();
            learnsetLevelUpMove18ComboBox.Items.Clear();
            learnsetLevelUpMove19ComboBox.Items.Clear();
            learnsetLevelUpMove20ComboBox.Items.Clear();

            learnsetMoveTutorCheckedListBox.Items.Clear();

            learnsetLevelUpMove1ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove2ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove3ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove4ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove5ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove6ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove7ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove8ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove9ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove10ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove11ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove12ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove13ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove14ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove15ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove16ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove17ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove18ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove19ComboBox.Items.AddRange(RomFile.GetMoveNames());
            learnsetLevelUpMove20ComboBox.Items.AddRange(RomFile.GetMoveNames());

            learnsetPokemonComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNamesNoEggs());

            if (RomFile.gameFamily == RomFile.GameFamilies.HGSS)
                learnsetMoveTutorCheckedListBox.Items.AddRange(MoveTutorTable.GetTutorMoveNamesHGSS());
            else if (RomFile.gameFamily == RomFile.GameFamilies.PL)
            {
                learnsetMoveTutorCheckedListBox.BeginUpdate();

                foreach (TutorMoveEntryPL tutorMove in RomFile.moveTutorPoolPL)
                    learnsetMoveTutorCheckedListBox.Items.Add(RomFile.MoveNames[tutorMove.MoveID - Pokemon_Sinjoh_Editor.Move.STARTING_INDEX]);

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


            //remove event handler for checkedListBox when updated, otherwise the event will fire while the user isn't interacting with the control
            learnsetMoveTutorCheckedListBox.ItemCheck -= learnsetMoveTutorCheckedListBox_ItemCheck;
            learnsetPokemonComboBox.SelectedIndex = 0;
            learnsetMoveTutorCheckedListBox.ItemCheck += learnsetMoveTutorCheckedListBox_ItemCheck;


        }

        private void displayLearnsetValues(int pokemonIndex)
        {
            List<int> learnableTutorMoves;

            //need to skip over egg entries in species list but only for level-up moves
            if (pokemonIndex >= PokemonSpecies.EGG_SPECIES_INDEX)
            {
                pokemonIndex += PokemonSpecies.NUM_EGG_ENTRIES;
            }

            int numMoves = RomFile.LevelUpMovesList[pokemonIndex].GetNumMoves();

            learnsetControlsCanRecieveUserInput = false;

            for (int i = 0; i < numMoves; i++)
            {
                levelUpMovesComboBoxList[i].SelectedIndex = RomFile.LevelUpMovesList[pokemonIndex].GetMoveID(i) - Pokemon_Sinjoh_Editor.Move.STARTING_INDEX;
                levelUpMoveNumericNoArrowsList[i].Value = RomFile.LevelUpMovesList[pokemonIndex].LevelsLearned[i];
                levelUpMovesComboBoxList[i].Visible = true;
                levelUpMoveNumericNoArrowsList[i].Visible = true;
            }

            for (int i = numMoves; i < levelUpMovesComboBoxList.Count; i++)
            {
                levelUpMovesComboBoxList[i].Visible = false;
                levelUpMoveNumericNoArrowsList[i].Visible = false;
            }

            if (pokemonIndex >= PokemonSpecies.EGG_SPECIES_INDEX)
            {
                pokemonIndex -= PokemonSpecies.NUM_EGG_ENTRIES;
            }

            if (RomFile.gameFamily != RomFile.GameFamilies.DP)
            {
               
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
            displayLearnsetValues(learnsetPokemonComboBox.SelectedIndex);
        }

        private void learnsetUpdateLearnedMoveID(int levelUpMoveIndex, int moveID)
        {
            moveID += Pokemon_Sinjoh_Editor.Move.STARTING_INDEX;

            if (RomFile.LevelUpMovesList[learnsetPokemonComboBox.SelectedIndex].GetMoveID(levelUpMoveIndex) != moveID)
            {
                RomFile.LevelUpMovesList[learnsetPokemonComboBox.SelectedIndex].SetMoveID(levelUpMoveIndex, moveID);
                MarkUnsavedChanges(SaveSubFile.LEVELUPMOVES);
            }
        }

        private void learnsetUpdateLearnedMoveLevel(int levelUpMoveIndex, int level)
        {
            if (RomFile.LevelUpMovesList[learnsetPokemonComboBox.SelectedIndex].LevelsLearned[levelUpMoveIndex] != level)
            {
                RomFile.LevelUpMovesList[learnsetPokemonComboBox.SelectedIndex].LevelsLearned[levelUpMoveIndex] = (ushort)level;
                MarkUnsavedChanges(SaveSubFile.LEVELUPMOVES);
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

            learnsetUpdateLearnedMoveLevel(2, (int)learnsetLevelUpLevel3NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel4NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel4NumericNoArrows.Value > learnsetLevelUpLevel5NumericNoArrows.Value && learnsetLevelUpLevel5NumericNoArrows.Visible)
                learnsetLevelUpLevel4NumericNoArrows.Value = learnsetLevelUpLevel5NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(3, (int)learnsetLevelUpLevel4NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel5NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel5NumericNoArrows.Value > learnsetLevelUpLevel6NumericNoArrows.Value && learnsetLevelUpLevel6NumericNoArrows.Visible)
                learnsetLevelUpLevel5NumericNoArrows.Value = learnsetLevelUpLevel6NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(4, (int)learnsetLevelUpLevel5NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel6NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel6NumericNoArrows.Value > learnsetLevelUpLevel7NumericNoArrows.Value && learnsetLevelUpLevel7NumericNoArrows.Visible)
                learnsetLevelUpLevel6NumericNoArrows.Value = learnsetLevelUpLevel7NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(5, (int)learnsetLevelUpLevel6NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel7NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel7NumericNoArrows.Value > learnsetLevelUpLevel8NumericNoArrows.Value && learnsetLevelUpLevel8NumericNoArrows.Visible)
                learnsetLevelUpLevel7NumericNoArrows.Value = learnsetLevelUpLevel8NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(6, (int)learnsetLevelUpLevel7NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel8NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel8NumericNoArrows.Value > learnsetLevelUpLevel9NumericNoArrows.Value && learnsetLevelUpLevel9NumericNoArrows.Visible)
                learnsetLevelUpLevel8NumericNoArrows.Value = learnsetLevelUpLevel9NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(7, (int)learnsetLevelUpLevel8NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel9NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel9NumericNoArrows.Value > learnsetLevelUpLevel10NumericNoArrows.Value && learnsetLevelUpLevel10NumericNoArrows.Visible)
                learnsetLevelUpLevel9NumericNoArrows.Value = learnsetLevelUpLevel10NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(8, (int)learnsetLevelUpLevel9NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel10NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel10NumericNoArrows.Value > learnsetLevelUpLevel11NumericNoArrows.Value && learnsetLevelUpLevel11NumericNoArrows.Visible)
                learnsetLevelUpLevel10NumericNoArrows.Value = learnsetLevelUpLevel11NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(9, (int)learnsetLevelUpLevel10NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel11NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel11NumericNoArrows.Value > learnsetLevelUpLevel12NumericNoArrows.Value && learnsetLevelUpLevel12NumericNoArrows.Visible)
                learnsetLevelUpLevel11NumericNoArrows.Value = learnsetLevelUpLevel12NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(10, (int)learnsetLevelUpLevel11NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel12NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel12NumericNoArrows.Value > learnsetLevelUpLevel13NumericNoArrows.Value && learnsetLevelUpLevel13NumericNoArrows.Visible)
                learnsetLevelUpLevel12NumericNoArrows.Value = learnsetLevelUpLevel13NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(11, (int)learnsetLevelUpLevel12NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel13NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel13NumericNoArrows.Value > learnsetLevelUpLevel14NumericNoArrows.Value && learnsetLevelUpLevel14NumericNoArrows.Visible)
                learnsetLevelUpLevel13NumericNoArrows.Value = learnsetLevelUpLevel14NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(12, (int)learnsetLevelUpLevel13NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel14NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel14NumericNoArrows.Value > learnsetLevelUpLevel15NumericNoArrows.Value && learnsetLevelUpLevel15NumericNoArrows.Visible)
                learnsetLevelUpLevel14NumericNoArrows.Value = learnsetLevelUpLevel15NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(13, (int)learnsetLevelUpLevel14NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel15NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel15NumericNoArrows.Value > learnsetLevelUpLevel16NumericNoArrows.Value && learnsetLevelUpLevel16NumericNoArrows.Visible)
                learnsetLevelUpLevel15NumericNoArrows.Value = learnsetLevelUpLevel16NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(14, (int)learnsetLevelUpLevel15NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel16NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel16NumericNoArrows.Value > learnsetLevelUpLevel17NumericNoArrows.Value && learnsetLevelUpLevel17NumericNoArrows.Visible)
                learnsetLevelUpLevel16NumericNoArrows.Value = learnsetLevelUpLevel17NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(15, (int)learnsetLevelUpLevel16NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel17NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel17NumericNoArrows.Value > learnsetLevelUpLevel18NumericNoArrows.Value && learnsetLevelUpLevel18NumericNoArrows.Visible)
                learnsetLevelUpLevel17NumericNoArrows.Value = learnsetLevelUpLevel18NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(16, (int)learnsetLevelUpLevel17NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel18NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel18NumericNoArrows.Value > learnsetLevelUpLevel19NumericNoArrows.Value && learnsetLevelUpLevel19NumericNoArrows.Visible)
                learnsetLevelUpLevel18NumericNoArrows.Value = learnsetLevelUpLevel19NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(17, (int)learnsetLevelUpLevel18NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel19NumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (learnsetLevelUpLevel19NumericNoArrows.Value > learnsetLevelUpLevel20NumericNoArrows.Value && learnsetLevelUpLevel20NumericNoArrows.Visible)
                learnsetLevelUpLevel19NumericNoArrows.Value = learnsetLevelUpLevel20NumericNoArrows.Value;

            learnsetUpdateLearnedMoveLevel(18, (int)learnsetLevelUpLevel19NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel20NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(19, (int)learnsetLevelUpLevel20NumericNoArrows.Value);
        }

        private void learnsetMoveTutorCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (learnsetControlsCanRecieveUserInput)
            {
                RomFile.MoveTutorTableList[learnsetPokemonComboBox.SelectedIndex].SetLearnableMove(e.Index, e.NewValue.HasFlag(CheckState.Checked));
                MarkUnsavedChanges(SaveSubFile.TUTORLEARNSET);
            }
        }
    }
}
