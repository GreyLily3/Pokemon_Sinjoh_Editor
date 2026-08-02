using Pokemon_Sinjoh_Editor.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
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
        private const string EN_SPECIES_NO_ALT_FORMS_TEXT = "No alt forms";

        public MainForm()
        {
            InitializeComponent();

            this.KeyPreview = true;

            mainTabControl.Enabled = false;

            //hide sub-editors that aren't fully implemented yet
            mainTabControl.TabPages.Remove(itemsTabPage);
            mainTabControl.TabPages.Remove(pokedexTabPage);
            mainTabControl.TabPages.Remove(textTabPage);

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

            tradeAbilityTooltip.AutomaticDelay = 500;
            tradeGenderTooltip.AutomaticDelay = 500;
            tradeNatureTooltip.AutomaticDelay = 500;

            INIManager.LoadINI();
            setLanguage();
        }

        private void setLanguage()
        {
            switch (INIManager.Language)
            {
                case Languages.JAPANESE:
                    日本語ToolStripMenuItem.Checked = true;
                    ChangeLanguage("ja-JP");
                    break;
                case Languages.ENGLISH:
                    englishToolStripMenuItem.Checked = true;
                    ChangeLanguage("en");
                    break;
                case Languages.FRENCH:
                    françaisToolStripMenuItem.Checked = true;
                    ChangeLanguage("fr");
                    break;
                case Languages.SPANISH:
                    españolToolStripMenuItem.Checked = true;
                    ChangeLanguage("es");
                    break;
                case Languages.ITALIAN:
                    italianoToolStripMenuItem.Checked = true;
                    ChangeLanguage("it");
                    break;
                case Languages.GERMAN:
                    ChangeLanguage("de");
                    deutschToolStripMenuItem.Checked = true;
                    break;
                case Languages.KOREAN:
                    ChangeLanguage("ko-KR");
                    한국어ToolStripMenuItem.Checked = true;
                    break;

            }
        }

        private void IncludeGameVersionInText(string romName)
        {
            Text = "Pokemon Sinjoh Editor - " + romName;
        }

        private void MarkUnsavedChanges(SaveSubFile subFile)
        {
            if (!Text.Contains("*"))
                Text += '*';

            RomFile.AreUnsavedChanges = true;

            switch (subFile)
            {
                case SaveSubFile.MOVES:
                    RomFile.UnsavedChangesMoves = true;
                    break;
                case SaveSubFile.SPECIES:
                    RomFile.UnsavedChangesSpecies = true;
                    break;
                case SaveSubFile.TRADES:
                    RomFile.UnsavedChangesTrades = true;
                    break;
                case SaveSubFile.ITEMS:
                    RomFile.UnsavedChangesItems = true;
                    break;
                case SaveSubFile.POKEDEX:
                    RomFile.UnsavedChangesPokedex = true;
                    break;
                case SaveSubFile.LEVELUPMOVES:
                    RomFile.UnsavedChangesLevelUpMoves = true;
                    break;
                case SaveSubFile.TUTORLEARNSET:
                    RomFile.UnsavedChangesMoveTutorMoves = true;
                    break;
                case SaveSubFile.EGGMOVES:
                    RomFile.UnsavedChangesEggMoves = true;
                    break;
            }
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
                        setupMoveText();
                        setupSpeciesText();
                        setupTradeText();
                        setupItemText();
                        setupPokedexText();
                        setupTextEditor();
                        setupLearnsetText();
                        UpdateDisplayedMoveValues();
                        UpdateDisplayedSpeciesValues();
                        UpdateDisplayedTradeValues();
                        updateDisplayedItemValues();
                        UpdateDisplayedPokedexValues();
                        UpdateDisplayedTextValues();
                        UpdateDisplayedLearnsetValues();
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

        private void tradeAbilityTextBox_MouseHover(object sender, EventArgs e)
        {
            tradeAbilityTooltip.Show("If the personality value is odd, the pokemon will have it's first ability. If it's even, the pokemon will have it's second ability (if it has one)", tradeAbilityTextBox);
        }

        private void tradeGenderTextBox_MouseHover(object sender, EventArgs e)
        {
            tradeGenderTooltip.Show("If the personality value % 256 is greater than the pokemon's gender ratio it will be male, unless the pokemon is gender unknown", tradeGenderTextBox);
        }

        private void tradeNatureTextBox_MouseHover(object sender, EventArgs e)
        {
            tradeNatureTooltip.Show("The pokemon's nature is personality value % 25, with 0 = Hardy, and 24 = Quirky", tradeNatureTextBox);
        }

        private void switchComboBoxesTextLanguage()
        {
            int moveSelected;
            int speciesSelected;
            int tradeSelected;

            //fixes all combo boxes and the gender textbox in the trade editor not updating their text properly when switching languages
            if (RomFile.IsValidGameVersion())
            {
                moveSelected = movesComboBox.SelectedIndex;
                speciesSelected = speciesComboBox.SelectedIndex;
                tradeSelected = tradeTrainerComboBox.SelectedIndex;

                setupMoveText();
                setupSpeciesText();
                setupTradeText();

                movesComboBox.SelectedIndex = moveSelected;
                speciesComboBox.SelectedIndex = speciesSelected;
                tradeTrainerComboBox.SelectedIndex = tradeSelected;

                updatePVDerivedFields();
            }
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (INIManager.Language != Languages.ENGLISH)
            {
                englishToolStripMenuItem.Checked = true;
                ChangeLanguage("en");
                INIManager.Language = Languages.ENGLISH;
                INIManager.SaveINI();

                españolToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;

                switchComboBoxesTextLanguage();
            }
        }

        private void españolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (INIManager.Language != Languages.SPANISH)
            {
                españolToolStripMenuItem.Checked = true;
                ChangeLanguage("es");
                INIManager.Language = Languages.SPANISH;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;

                switchComboBoxesTextLanguage();
            }
        }

        private void françaisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (INIManager.Language != Languages.FRENCH)
            {
                françaisToolStripMenuItem.Checked = true;
                ChangeLanguage("fr");
                INIManager.Language = Languages.FRENCH;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                españolToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;

                switchComboBoxesTextLanguage();
            }
        }

        private void deutschToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (INIManager.Language != Languages.GERMAN)
            {
                deutschToolStripMenuItem.Checked = true;
                ChangeLanguage("de");
                INIManager.Language = Languages.GERMAN;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                españolToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;

                switchComboBoxesTextLanguage();
            }
        }

        private void italianoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (INIManager.Language != Languages.ITALIAN)
            {
                italianoToolStripMenuItem.Checked = true;
                ChangeLanguage("it");
                INIManager.Language = Languages.ITALIAN;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                españolToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;

                switchComboBoxesTextLanguage();
            }
        }

        private void 日本語ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (INIManager.Language != Languages.JAPANESE)
            {
                日本語ToolStripMenuItem.Checked = true;
                ChangeLanguage("ja-JP");
                INIManager.Language = Languages.JAPANESE;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                españolToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;

                switchComboBoxesTextLanguage();
            }
        }

        private void 한국어ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (INIManager.Language != Languages.KOREAN)
            {
                한국어ToolStripMenuItem.Checked = true;
                ChangeLanguage("ko-KR");
                INIManager.Language = Languages.KOREAN;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                españolToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;

                switchComboBoxesTextLanguage();
            }
        }

        private void ChangeLanguage(string lang)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            localizeForm(this);

        }

        private void localizeForm(Form frm)
        {
            var manager = new ComponentResourceManager(frm.GetType());
            manager.ApplyResources(frm, "$this");
            applyResources(manager, frm.Controls);
        }

        private void applyResources(ComponentResourceManager manager, Control.ControlCollection ctls)
        {
            foreach (Control ctl in ctls)
            {
                manager.ApplyResources(ctl, ctl.Name);
                applyResources(manager, ctl.Controls);
            }

            //above loop doesn't apply to toolstripmenuitems, so we have to do it manually
            manager.ApplyResources(fileToolStripMenuItem, fileToolStripMenuItem.Name);
            manager.ApplyResources(openRomFileToolStripMenuItem, openRomFileToolStripMenuItem.Name);
            manager.ApplyResources(saveToolStripMenuItem, saveToolStripMenuItem.Name);
            manager.ApplyResources(quitToolStripMenuItem, quitToolStripMenuItem.Name);

            manager.ApplyResources(optionsToolStripMenuItem, optionsToolStripMenuItem.Name);
            manager.ApplyResources(languageToolStripMenuItem, languageToolStripMenuItem.Name);

        }

    }
}
