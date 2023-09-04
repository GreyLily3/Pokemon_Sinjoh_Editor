using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        }

        public void DisplayMoveValues(int moveIndex)
        {
            Move currentMove = Controller.GetMove(moveIndex);

            movePowerNumericNoArrows.Value = currentMove.Power;
            moveAccuracyNumericNoArrows.Value = currentMove.Accuracy;
            movePPNumericNoArrows.Value = currentMove.PowerPoints;
            moveTypeComboBox.SelectedIndex = currentMove.Type;
            moveCategoryComboBox.SelectedIndex = (int)currentMove.Category;
            moveEffectComboBox.SelectedIndex = currentMove.Effect;
            moveEffectChanceNumericNoArrows.Value = currentMove.EffectChance;
            movePriorityNumericNoArrows.Value = currentMove.Priority;
            moveTargetComboBox.SelectedIndex = (int)currentMove.Target;
            moveContestEffectComboBox.SelectedIndex = currentMove.ContestEffect;
            moveContestConditionComboBox.SelectedIndex = (int)currentMove.ContestCondition;

            moveContactCheckBox.Checked = currentMove.ContactFlag;
            moveProtectCheckBox.Checked = currentMove.ProtectFlag;
            moveMagicCoatCheckBox.Checked = currentMove.MagicCoatFlag;
            moveSnatchCheckBox.Checked = currentMove.SnatchFlag;
            moveMirrorMoveCheckBox.Checked = currentMove.MirrorMoveFlag;
            moveKingsRockCheckBox.Checked = currentMove.KingsRockFlag;
            moveHPBarCheckBox.Checked = currentMove.KeepHPBarVisibleFlag;
            moveShadowCheckBox.Checked = currentMove.HidePokemonShadowsFlag;
        }

        public void SaveChangesToCurrentMove(int moveIndex)
        {
            Move updatedMove = new Move
            {
                Power = (byte)movePowerNumericNoArrows.Value,
                Accuracy = (byte)moveAccuracyNumericNoArrows.Value,
                PowerPoints = (byte)movePPNumericNoArrows.Value,
                Type = (byte)moveTypeComboBox.SelectedIndex,
                Category = (Move.Categories)moveCategoryComboBox.SelectedIndex,
                Effect = (ushort)moveEffectComboBox.SelectedIndex,
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

            Controller.UpdateMove(moveIndex, updatedMove);
        }

        public void LoadMoveNames()
        {
            movesComboBox.Items.AddRange(Controller.GetMoveNames());
        }

        public void IncludeGameVersionInText(string romName)
        {
            Text = "Pokemon Stat Editor - " + romName;
        }

        public void MarkUnsavedChanges()
        {
            if (!Text.Contains("*"))
                Text += '*';
        }

        private void speciesGenderlessRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void moveTargetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
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
                    Controller.LoadRom(filePicker.FileName);
                }
            }
        }
    }
}
