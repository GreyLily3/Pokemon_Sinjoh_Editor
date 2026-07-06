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

        private const int MOVES_STARTING_INDEX = 1;

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

            learnsetPokemonComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNames());


        }

        private void UpdateDisplayedLearnsetValues()
        {
            learnsetPokemonComboBox.SelectedIndex = 0;
        }

        private void displayLearnsetValues(int pokemonIndex)
        {
            int numMoves = RomFile.LevelUpMovesList[pokemonIndex].GetNumMoves();

            for (int i = 0; i < numMoves; i++)
            {
                levelUpMovesComboBoxList[i].SelectedIndex = RomFile.LevelUpMovesList[pokemonIndex].GetMoveID(i) - MOVES_STARTING_INDEX;
                levelUpMoveNumericNoArrowsList[i].Value = RomFile.LevelUpMovesList[pokemonIndex].LevelsLearned[i];
                levelUpMovesComboBoxList[i].Visible = true;
                levelUpMoveNumericNoArrowsList[i].Visible = true;
            }

            for (int i = numMoves; i < levelUpMovesComboBoxList.Count; i++)
            {
                levelUpMovesComboBoxList[i].Visible = false;
                levelUpMoveNumericNoArrowsList[i].Visible = false;
            }
        }

        private void learnsetPokemonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayLearnsetValues(learnsetPokemonComboBox.SelectedIndex);
        }

        private void learnsetUpdateLearnedMoveID(int levelUpMoveIndex, int moveID)
        {
            moveID += MOVES_STARTING_INDEX;

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
            learnsetUpdateLearnedMoveLevel(0, (int)learnsetLevelUpLevel1NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel2NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(1, (int)learnsetLevelUpLevel2NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel3NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(2, (int)learnsetLevelUpLevel3NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel4NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(3, (int)learnsetLevelUpLevel4NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel5NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(4, (int)learnsetLevelUpLevel5NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel6NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(5, (int)learnsetLevelUpLevel6NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel7NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(6, (int)learnsetLevelUpLevel7NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel8NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(7, (int)learnsetLevelUpLevel8NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel9NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(8, (int)learnsetLevelUpLevel9NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel10NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(9, (int)learnsetLevelUpLevel10NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel11NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(10, (int)learnsetLevelUpLevel11NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel12NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(11, (int)learnsetLevelUpLevel12NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel13NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(12, (int)learnsetLevelUpLevel13NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel14NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(13, (int)learnsetLevelUpLevel14NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel15NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(14, (int)learnsetLevelUpLevel15NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel16NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(15, (int)learnsetLevelUpLevel16NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel17NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(16, (int)learnsetLevelUpLevel17NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel18NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(17, (int)learnsetLevelUpLevel18NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel19NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(18, (int)learnsetLevelUpLevel19NumericNoArrows.Value);
        }

        private void learnsetLevelUpLevel20NumericNoArrows_Validated(object sender, EventArgs e)
        {
            learnsetUpdateLearnedMoveLevel(19, (int)learnsetLevelUpLevel20NumericNoArrows.Value);
        }

        private void learnsetMoveTutorCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }
    }
}
