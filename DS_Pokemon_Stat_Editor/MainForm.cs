using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS_Pokemon_Stat_Editor
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();

            mainTabControl.Enabled = false;

            moveEffectNumericNoArrows.Maximum = DS_Pokemon_Stat_Editor.Move.NUM_EFFECTS;
        }

        private void LoadMoveData()
        {
            mainTabControl.Enabled = true;

            movesComboBox.Items.AddRange(RomFile.GetMoveNames());
            moveTypeComboBox.Items.AddRange(RomFile.GetTypeNames());
            moveTargetComboBox.Items.AddRange(RomFile.GetMoveTargets());
            moveContestConditionComboBox.Items.AddRange(RomFile.GetMoveContestConditions());
            moveContestEffectComboBox.Items.AddRange(RomFile.GetMoveContestEffect());

            moveCategoryComboBox.Items.Add("PHYSICAL");
            moveCategoryComboBox.Items.Add("SPECIAL");
            moveCategoryComboBox.Items.Add("STATUS");

            DisplayMoveValues(0);

            movesComboBox.SelectedIndex = 1; //makes pound the initially selected move
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
            moveTargetComboBox.SelectedIndex = DS_Pokemon_Stat_Editor.Move.TargetEnumToIndexValue(RomFile.MoveList[moveIndex].Target);
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

        private void IncludeGameVersionInText(string romName)
        {
            Text = "Pokemon Stat Editor - " + romName;
        }

        private void MarkUnsavedChanges()
        {
            if (!Text.Contains("*"))
                Text += '*';
            RomFile.AreUnsavedChanges = true;
        }

        private void speciesGenderlessRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void openRomFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog filePicker = new OpenFileDialog())
            {
                filePicker.Filter = "NDS files (*.nds)|*.nds";
                filePicker.RestoreDirectory = true;

                if (filePicker.ShowDialog() == DialogResult.OK)
                {
                    RomFile.LoadNewRom(filePicker.FileName);

                    if (!RomFile.IsValidGameVersion())
                        MessageBox.Show("File selected is not a valid DS pokemon rom. It will not be loaded.");
                    else if (!RomFile.IsSupportedGameVersion())
                        MessageBox.Show("Pokemon Black/White and Black2/White2 roms are not supported due to significant differences in data structures from Gen 4.");
                    else
                    {
                        IncludeGameVersionInText(RomFile.GetGameVersion());
                        LoadMoveData();
                    }
                }                
            }
        }

        private void movesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayMoveValues(movesComboBox.SelectedIndex);
        }




        private void moveTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].Type = (byte)moveTypeComboBox.SelectedIndex;
            MarkUnsavedChanges();
        }

        private void moveCategoryComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].Category = (Move.Categories)moveCategoryComboBox.SelectedIndex;
            MarkUnsavedChanges();
        }

        private void moveTargetComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].Target = DS_Pokemon_Stat_Editor.Move.IndexValueToTargetEnum(moveTargetComboBox.SelectedIndex);
            MarkUnsavedChanges();
        }

        private void moveContestEffectComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].ContestEffect = (byte)moveContestEffectComboBox.SelectedIndex;
            MarkUnsavedChanges();
        }

        private void moveContestConditionComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].ContestCondition = (Move.ContestConditions)moveContestConditionComboBox.SelectedIndex;
            MarkUnsavedChanges();
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RomFile.IsValidGameVersion() && RomFile.AreUnsavedChanges)
            {
                try
                {
                    RomFile.Write();
                    Text = Text.Remove(Text.Length - 1);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                
            }
        }

        private void moveCategoryComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((Move.Categories)moveCategoryComboBox.SelectedIndex == DS_Pokemon_Stat_Editor.Move.Categories.STATUS)
            {
                movePowerNumericNoArrows.Enabled = false;
                moveKingsRockCheckBox.Enabled = false;
                moveSnatchCheckBox.Enabled = true;
                moveMagicCoatCheckBox.Enabled = true;
            }
            else
            {
                movePowerNumericNoArrows.Enabled = true;
                moveKingsRockCheckBox.Enabled = true;
                moveSnatchCheckBox.Enabled = false;
                moveMagicCoatCheckBox.Enabled = false;
            }
        }
    }
}
