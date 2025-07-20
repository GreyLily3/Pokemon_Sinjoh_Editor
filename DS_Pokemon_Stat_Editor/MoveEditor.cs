using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Sinjoh_Editor
{
    partial class MainForm
    {
        private void LoadMoveData()
        {
            if (movesComboBox.Items.Count > 0)
                movesComboBox.Items.Clear();

            if (moveTypeComboBox.Items.Count > 0)
                moveTypeComboBox.Items.Clear();

            if (moveTargetComboBox.Items.Count > 0)
                moveTargetComboBox.Items.Clear();

            if (moveContestConditionComboBox.Items.Count > 0)
                moveContestConditionComboBox.Items.Clear();

            if (moveContestEffectComboBox.Items.Count > 0)
                moveContestEffectComboBox.Items.Clear();

            if (moveCategoryComboBox.Items.Count > 0)
                moveCategoryComboBox.Items.Clear();

            movesComboBox.Items.AddRange(RomFile.GetMoveNames());
            moveTypeComboBox.Items.AddRange(RomFile.GetTypeNames());
            moveTargetComboBox.Items.AddRange(RomFile.GetMoveTargets());
            moveContestConditionComboBox.Items.AddRange(RomFile.GetMoveContestConditions());
            moveContestEffectComboBox.Items.AddRange(RomFile.GetMoveContestEffect());
            moveCategoryComboBox.Items.AddRange(RomFile.GetMoveCategories());


            //we need the event handler to only be set after all text is loaded into the controls
            this.moveCategoryComboBox.SelectedValueChanged += new System.EventHandler(this.moveCategoryComboBox_SelectedValueChanged);

            movesComboBox.SelectedIndex = 0; //makes pound the initially selected move
            DisplayMoveValues(0); //needed to make sure the values displayed are updated when changing to a new rom without closing if Pound is selected as the move

        }

        private void DisplayMoveValues(int moveIndex)
        {
            movePowerNumericNoArrows.Value = RomFile.MoveList[moveIndex].Power;
            moveAccuracyNumericNoArrows.Value = RomFile.MoveList[moveIndex].Accuracy;
            movePPNumericNoArrows.Value = RomFile.MoveList[moveIndex].PowerPoints;
            moveTypeComboBox.SelectedIndex = RomFile.MoveList[moveIndex].Type;
            moveCategoryComboBox.SelectedIndex = (int)RomFile.MoveList[moveIndex].Category;
            moveEffectNumericNoArrows.Value = RomFile.MoveList[moveIndex].Effect;
            moveEffectChanceNumericNoArrows.Value = RomFile.MoveList[moveIndex].EffectChance;
            movePriorityNumericNoArrows.Value = RomFile.MoveList[moveIndex].Priority;
            moveTargetComboBox.SelectedIndex = Pokemon_Sinjoh_Editor.Move.TargetEnumToIndexValue(RomFile.MoveList[moveIndex].Target);
            moveContestEffectComboBox.SelectedIndex = RomFile.MoveList[moveIndex].ContestEffect;
            moveContestConditionComboBox.SelectedIndex = (int)RomFile.MoveList[moveIndex].ContestCondition;

            moveContactCheckBox.Checked = RomFile.MoveList[moveIndex].ContactFlag;
            moveProtectCheckBox.Checked = RomFile.MoveList[moveIndex].ProtectFlag;
            moveMagicCoatCheckBox.Checked = RomFile.MoveList[moveIndex].MagicCoatFlag;
            moveSnatchCheckBox.Checked = RomFile.MoveList[moveIndex].SnatchFlag;
            moveMirrorMoveCheckBox.Checked = RomFile.MoveList[moveIndex].MirrorMoveFlag;
            moveKingsRockCheckBox.Checked = RomFile.MoveList[moveIndex].KingsRockFlag;
            moveHPBarCheckBox.Checked = RomFile.MoveList[moveIndex].KeepHPBarVisibleFlag;
            moveShadowCheckBox.Checked = RomFile.MoveList[moveIndex].HidePokemonShadowsFlag;
        }

        private void movesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayMoveValues(movesComboBox.SelectedIndex);
        }




        private void moveTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.MoveList[movesComboBox.SelectedIndex].Type != (byte)moveTypeComboBox.SelectedIndex)
            {
                RomFile.MoveList[movesComboBox.SelectedIndex].Type = (byte)moveTypeComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void moveCategoryComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.MoveList[movesComboBox.SelectedIndex].Category != (Move.Categories)moveCategoryComboBox.SelectedIndex)
            {
                RomFile.MoveList[movesComboBox.SelectedIndex].Category = (Move.Categories)moveCategoryComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void moveTargetComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.MoveList[movesComboBox.SelectedIndex].Target != Pokemon_Sinjoh_Editor.Move.IndexValueToTargetEnum(moveTargetComboBox.SelectedIndex))
            {
                RomFile.MoveList[movesComboBox.SelectedIndex].Target = Pokemon_Sinjoh_Editor.Move.IndexValueToTargetEnum(moveTargetComboBox.SelectedIndex);
                MarkUnsavedChanges();
            }
        }

        private void moveContestEffectComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.MoveList[movesComboBox.SelectedIndex].ContestEffect != (byte)moveContestEffectComboBox.SelectedIndex)
            {
                RomFile.MoveList[movesComboBox.SelectedIndex].ContestEffect = (byte)moveContestEffectComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void moveContestConditionComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.MoveList[movesComboBox.SelectedIndex].ContestCondition != (Move.ContestConditions)moveContestConditionComboBox.SelectedIndex)
            {
                RomFile.MoveList[movesComboBox.SelectedIndex].ContestCondition = (Move.ContestConditions)moveContestConditionComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void movePowerNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.MoveList[movesComboBox.SelectedIndex].Power != (byte)movePowerNumericNoArrows.Value)
            {
                RomFile.MoveList[movesComboBox.SelectedIndex].Power = (byte)movePowerNumericNoArrows.Value;
                MarkUnsavedChanges();
            }

        }

        private void moveAccuracyNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.MoveList[movesComboBox.SelectedIndex].Accuracy != (byte)moveAccuracyNumericNoArrows.Value)
            {
                RomFile.MoveList[movesComboBox.SelectedIndex].Accuracy = (byte)moveAccuracyNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void movePPNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (movePPNumericNoArrows.Value % MOVE_PP_MULTIPLE != 0)
            {
                //if PP is not a multiple of 5 then make it the next lowest multiple of 5
                movePPNumericNoArrows.Value -= movePPNumericNoArrows.Value % MOVE_PP_MULTIPLE;
            }

            if (RomFile.MoveList[movesComboBox.SelectedIndex].PowerPoints != (byte)movePPNumericNoArrows.Value)
            {
                RomFile.MoveList[movesComboBox.SelectedIndex].PowerPoints = (byte)movePPNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void moveEffectNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.MoveList[movesComboBox.SelectedIndex].Effect != (byte)moveEffectNumericNoArrows.Value)
            {
                RomFile.MoveList[movesComboBox.SelectedIndex].Effect = (byte)moveEffectNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void moveEffectChanceNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.MoveList[movesComboBox.SelectedIndex].EffectChance != (byte)moveEffectChanceNumericNoArrows.Value)
            {
                RomFile.MoveList[movesComboBox.SelectedIndex].EffectChance = (byte)moveEffectChanceNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void movePriorityNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.MoveList[movesComboBox.SelectedIndex].Priority != (sbyte)movePriorityNumericNoArrows.Value)
            {
                RomFile.MoveList[movesComboBox.SelectedIndex].Priority = (sbyte)movePriorityNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void moveContactCheckBox_Click(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].ContactFlag = moveContactCheckBox.Checked;
            MarkUnsavedChanges();
        }

        private void moveProtectCheckBox_Click(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].ProtectFlag = moveProtectCheckBox.Checked;
            MarkUnsavedChanges();
        }

        private void moveMagicCoatCheckBox_Click(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].MagicCoatFlag = moveMagicCoatCheckBox.Checked;
            MarkUnsavedChanges();
        }

        private void moveSnatchCheckBox_Click(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].SnatchFlag = moveSnatchCheckBox.Checked;
            MarkUnsavedChanges();
        }

        private void moveMirrorMoveCheckBox_Click(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].MirrorMoveFlag = moveMirrorMoveCheckBox.Checked;
            MarkUnsavedChanges();
        }

        private void moveKingsRockCheckBox_Click(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].KingsRockFlag = moveKingsRockCheckBox.Checked;
            MarkUnsavedChanges();
        }

        private void moveHPBarCheckBox_Click(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].KeepHPBarVisibleFlag = moveHPBarCheckBox.Checked;
            MarkUnsavedChanges();
        }

        private void moveShadowCheckBox_Click(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].HidePokemonShadowsFlag = moveShadowCheckBox.Checked;
            MarkUnsavedChanges();
        }

        private void moveCategoryComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((Move.Categories)moveCategoryComboBox.SelectedIndex == Pokemon_Sinjoh_Editor.Move.Categories.STATUS)
            {
                movePowerNumericNoArrows.Enabled = false;
                movePowerNumericNoArrows.Value = 0;
                RomFile.MoveList[movesComboBox.SelectedIndex].Power = 0;

                moveKingsRockCheckBox.Enabled = false;
                moveKingsRockCheckBox.Checked = false;
                RomFile.MoveList[movesComboBox.SelectedIndex].KingsRockFlag = false;

                moveSnatchCheckBox.Enabled = true;
                moveMagicCoatCheckBox.Enabled = true;
            }
            else
            {
                movePowerNumericNoArrows.Enabled = true;
                moveKingsRockCheckBox.Enabled = true;

                moveSnatchCheckBox.Enabled = false;
                moveSnatchCheckBox.Checked = false;
                RomFile.MoveList[movesComboBox.SelectedIndex].SnatchFlag = false;

                moveMagicCoatCheckBox.Enabled = false;
                moveMagicCoatCheckBox.Checked = false;
                RomFile.MoveList[movesComboBox.SelectedIndex].MagicCoatFlag = false;
            }
        }
    }
}
