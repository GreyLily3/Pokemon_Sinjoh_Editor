using System;
using System.Windows.Forms;

namespace Pokemon_Sinjoh_Editor
{
	public partial class MainForm : Form
	{
		private const int MOVE_PP_MULTIPLE = 5;

		public MainForm()
		{
			InitializeComponent();

			mainTabControl.Enabled = false;

			moveEffectNumericNoArrows.Maximum = Pokemon_Sinjoh_Editor.Move.NUM_EFFECTS;

			movePowerTooltip.SetToolTip(movePowerNumericNoArrows, "The base damage of the move. Whether or not this field is used for an attack is determined by the move effect");            
			moveAccuracyTooltip.SetToolTip(moveAccuracyNumericNoArrows, "The chance of a move working on an enemy/enemies. Moves that target the User or User and Allies ignore this field");
			movePowerPointsTooltip.SetToolTip(movePPNumericNoArrows, "The base Power Points for a move. Can only be a mutiple of 5, otherwise it breaks PP ups and Max PPs");
			moveEffectTooltip.SetToolTip(moveEffectNumericNoArrows, "What the move actually does, including whether or not it calculates damage based on its power");
			moveEffectChanceTooltip.SetToolTip(moveEffectChanceNumericNoArrows, "The chance of the move applying a secondary effect based on the set effect, ie. a status condition or stat change");
			movePriorityTooltip.SetToolTip(movePriorityNumericNoArrows, "The order the move will be used in compared to the opponent's move. NOTE: This field is only checked if certain effects are assigned");
			moveTargetTooltip.SetToolTip(moveTargetComboBox, "What pokemon in battle the move is used on. NOTE: Moves may cause effects to pokemon other than the target, ex. Swagger confusing the user while targetting another pokemon");
			
			moveContactTooltip.SetToolTip(moveContactCheckBox, "If the move counts as making contact for certain abilities and held items, ex. static, rough skin, the poison barb");
			moveProtectTooltip.SetToolTip(moveProtectCheckBox, "If the move is negated when its target is using protect");
			moveMagicCoatTooltip.SetToolTip(moveMagicCoatCheckBox, "If the move can be reflected back onto the user by a target using magic coat");
			moveSnatchTooltip.SetToolTip(moveSnatchCheckBox, "If the move can be stolen by another pokemon using snatch");
			moveMirrorMoveTooltip.SetToolTip(moveMirrorMoveCheckBox, "If the move can be copied by another pokemon using mirror move on the user");
			moveMirrorMoveTooltip.SetToolTip(moveKingsRockCheckBox, "If the flinch chance from holding the king's rock will applied when this move is used");
			moveHPBarTooltip.SetToolTip(moveHPBarCheckBox, "If both pokemons' HP bars are shown when the move's animation is playing");
			moveShadowTooltip.SetToolTip(moveShadowCheckBox, "If both pokemons' shadows are hidden when the move's animation is playing");
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

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RomFile.IsValidGameVersion() && RomFile.AreUnsavedChanges)
            {
                save();
            }
        }

        private void openRomFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult saveChanges;
			if (RomFile.IsValidGameVersion() && RomFile.AreUnsavedChanges)
			{
				if (RomFile.AreUnsavedChanges)
				{
					saveChanges = MessageBox.Show("There are unsaved changes to the selected ROM, do you want to save them before opening a new ROM?", "Save Changes before opening new ROM?", MessageBoxButtons.YesNoCancel);

					if (saveChanges == DialogResult.Yes)
						save();
					else if (saveChanges == DialogResult.Cancel)
						return;
				}
			}

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
                        mainTabControl.Enabled = true;
                        LoadMoveData();
						loadSpeciesData();

                    }
				}                
			}

			if (RomFile.gameFamily == RomFile.GameFamilies.HGSS)
			{
				moveContestConditionComboBox.Enabled = false;
				moveContestEffectComboBox.Enabled = false;
				moveContestConditionTooltip.SetToolTip(moveContestConditionComboBox, "Unused in HeartGold and SoulSilver");
				moveContestEffectTooltip.SetToolTip(moveContestEffectComboBox, "Unused in HeartGold and SoulSilver");
			}
			else
			{
				moveContestConditionComboBox.Enabled = true;
				moveContestEffectComboBox.Enabled = true;
				moveContestConditionTooltip.SetToolTip(moveContestConditionComboBox, "Determines what contest type the move will score best in when used");
				moveContestEffectTooltip.SetToolTip(moveContestEffectComboBox, "What a move will do when used in contests");
			}
		}

		private void save()
		{
			try
			{
				RomFile.Write();
				Text = Text.Remove(Text.Length - 1); //remove the * indicating unsaved changes
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.ToString());
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult saveChanges;
			if (RomFile.AreUnsavedChanges)
			{
				saveChanges = MessageBox.Show("There are unsaved changes to the selected ROM, do you want to save them before closing?", "Save Changes before Closing?", MessageBoxButtons.YesNoCancel);

				if (saveChanges == DialogResult.Yes)
					save();
				else if (saveChanges == DialogResult.Cancel)
					e.Cancel = true;
			}
		}

        
    }
}
