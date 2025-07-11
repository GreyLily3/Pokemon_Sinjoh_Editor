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

        private void SaveChangesToCurrentMove(int moveIndex)
        {
            Move updatedMove = new Move
            {
                Power = (byte)movePowerNumericNoArrows.Value,
                Accuracy = (byte)moveAccuracyNumericNoArrows.Value,
                PowerPoints = (byte)movePPNumericNoArrows.Value,
                Type = (byte)moveTypeComboBox.SelectedIndex,
                Category = (Move.Categories)moveCategoryComboBox.SelectedIndex,
                Effect = (ushort)moveEffectNumericNoArrows.Value,
                EffectChance = (byte)moveEffectChanceNumericNoArrows.Value,
                Priority = (sbyte)movePriorityNumericNoArrows.Value,
                Target = (Move.Targets)moveTargetComboBox.SelectedIndex,
                ContestEffect = (byte)moveContestEffectComboBox.SelectedIndex,
                ContestCondition = (Move.ContestConditions)moveContestConditionComboBox.SelectedIndex,

                ContactFlag = moveContactCheckBox.Checked,
                ProtectFlag = moveProtectCheckBox.Checked,
                MagicCoatFlag = moveMagicCoatCheckBox.Checked,
                SnatchFlag = moveSnatchCheckBox.Checked,
                MirrorMoveFlag = moveMirrorMoveCheckBox.Checked,
                KingsRockFlag = moveKingsRockCheckBox.Checked,
                KeepHPBarVisibleFlag = moveHPBarCheckBox.Checked,
                HidePokemonShadowsFlag = moveShadowCheckBox.Checked
            };

            RomFile.UpdateMove(moveIndex, updatedMove);
        }

        private void IncludeGameVersionInText(string romName)
        {
            Text = "Pokemon Stat Editor - " + romName;
        }

        private void MarkUnsavedChanges()
        {
            if (!Text.Contains("*"))
                Text += '*';
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
                }

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

        private void moveTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void moveCategoryComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void moveTargetComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void moveContestEffectComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void moveContestConditionComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void movePowerNumericNoArrows_Validated(object sender, EventArgs e)
        {
            RomFile.MoveList[movesComboBox.SelectedIndex].Power = (byte)movePowerNumericNoArrows.Value;
        }

        private void movesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayMoveValues(movesComboBox.SelectedIndex);
        }
    }
}
