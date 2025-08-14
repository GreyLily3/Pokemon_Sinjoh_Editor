using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pokemon_Sinjoh_Editor
{
	public partial class MainForm : Form
	{
		private const int MOVE_PP_MULTIPLE = 5;
		private const string SPECIES_TYPE_TOOLTIP_TEXT = "For monotype pokemon set type 1 & 2 to the same type";
		private const string SPECIES_ABILITY_TOOLTIP_TEXT = "For pokemon with only one ability you can either set ability 1 & 2 to the same ability or set ability 2 to '---'";
		private const string SPECIES_EGG_GROUP_TOOLTIP_TEXT = "Pokemon can only breed with other pokemon that share an egg group. For pokemon with a single egg group set egg groups 1 & 2 to be the same";
		private const string SPECIES_WILD_HELD_ITEM_TOOLTIP_TEXT = "The item the pokemon has a chance of holding when encountered in the wild";
		private const string SPECIES_EV_YIELD_TOOLTIP_TEXT = "What effort values the pokemon will give when defeated. Each pokemon can have a total of 510 EVs across all stats";

		private Color darkModeBackColor = Color.FromArgb(64, 64, 64);
        private Color darkModeTextColor = Color.FromArgb(255, 255, 255);
		private Color lightModeBackColor = SystemColors.Window;
        private Color lightModeTextColor = SystemColors.WindowText;

        public MainForm()
		{
			InitializeComponent();

			this.KeyPreview = true;

			mainTabControl.Enabled = false;

			moveEffectNumericNoArrows.Maximum = Pokemon_Sinjoh_Editor.Move.NUM_EFFECTS;

			movePowerTooltip.SetToolTip(movePowerNumericNoArrows, "The base damage of the move. Whether or not this field is used for an attack is determined by the move effect");            
			moveAccuracyTooltip.SetToolTip(moveAccuracyNumericNoArrows, "The chance of a move working on an enemy/enemies. Moves that target the User or User and Allies ignore this field");
			movePowerPointsTooltip.SetToolTip(movePPNumericNoArrows, "The base Power Points for a move. Can only be a mutiple of 5, otherwise it breaks PP ups and Max PPs");
			moveEffectTooltip.SetToolTip(moveEffectNumericNoArrows, "What the move actually does, including whether or not it calculates damage based on its power");
			moveEffectChanceTooltip.SetToolTip(moveEffectChanceNumericNoArrows, "The chance of the move applying a secondary effect based on the set effect, ie. a status condition, stat change or flinch when using a damaging move");
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

            speciesTypeTooltip.SetToolTip(speciesType1ComboBox, SPECIES_TYPE_TOOLTIP_TEXT);
            speciesTypeTooltip.SetToolTip(speciesType2ComboBox, SPECIES_TYPE_TOOLTIP_TEXT);
            speciesAbilityTooltip.SetToolTip(speciesAbility1ComboBox, SPECIES_ABILITY_TOOLTIP_TEXT);
            speciesAbilityTooltip.SetToolTip(speciesAbility2ComboBox, SPECIES_ABILITY_TOOLTIP_TEXT);
            speciesXPGroupTooltip.SetToolTip(speciesXPGroupComboBox, "Determines how much EXP a pokemon needs for each level. Different EXP groups require different amounts of EXP to reach lvl.100");
            speciesBaseXPTooltip.SetToolTip(speciesBaseXPYieldNumericNoArrows, "The amount of experience the pokemon will reward when defeated before taking into account modifiers to the amount like the Exp. share or lucky egg");
            speciesWildHeldItemTooltip.SetToolTip(speciesHeldItem1ComboBox, SPECIES_WILD_HELD_ITEM_TOOLTIP_TEXT);
            speciesWildHeldItemTooltip.SetToolTip(speciesHeldItem2ComboBox, SPECIES_WILD_HELD_ITEM_TOOLTIP_TEXT);
            speciesEggGroupTooltip.SetToolTip(speciesEggGroup1ComboBox, SPECIES_EGG_GROUP_TOOLTIP_TEXT);
            speciesEggGroupTooltip.SetToolTip(speciesEggGroup2ComboBox, SPECIES_EGG_GROUP_TOOLTIP_TEXT);
            speciesEggCyclesTooltip.SetToolTip(speciesEggCyclesNumericNoArrows, "Add 1 then multiply by 255 to get the number of steps needed to hatch this pokemon from an egg");
            speciesCatchRateTooltip.SetToolTip(speciesCatchRateNumericNoArrows, "Determines how difficult the pokemon is to catch, the higher it is the more likely catching will be. Ex. a pokemon with 255 catch will have a 33.3% chance to be caught with a pokeball at full health");
            speciesBaseFriendshipTooltip.SetToolTip(speciesBaseFriendshipNumericNoArrows, "How much friendship the pokemon will start with when caught or recieved from trading. Pokemon with friendship evolutions will evovle at 220");
            speciesSafariRunChanceTooltip.SetToolTip(speciesSafariRunChanceNumericNoArrows, "The base chance out of 254 a pokemon will run every turn when encountered in the safari zone/Great Marsh");
            speciesEVYieldTooltip.SetToolTip(speciesEVOnDefeatGroupBox, SPECIES_EV_YIELD_TOOLTIP_TEXT);

			
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
			save();
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
						LoadTradeControlText();
						UpdateDisplayedTradeValues();
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
			if (RomFile.IsValidGameVersion() && RomFile.AreUnsavedChanges)
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

        private void MainForm_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
			if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
			{
                this.ActiveControl = null;

                save();
            }
                

        }

    }
}
