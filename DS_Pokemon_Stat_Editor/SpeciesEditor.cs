using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Pokemon_Sinjoh_Editor
{
    partial class MainForm
    {
        private bool speciesControlsCanRecieveUserInput;

        private void setupSpeciesText()
        {
            speciesComboBox.Items.Clear();
            speciesType1ComboBox.Items.Clear();
            speciesType2ComboBox.Items.Clear();
            speciesAbility1ComboBox.Items.Clear();
            speciesAbility2ComboBox.Items.Clear();
            speciesHeldItem1ComboBox.Items.Clear();
            speciesHeldItem2ComboBox.Items.Clear();
            speciesEggGroup1ComboBox.Items.Clear();
            speciesEggGroup2ComboBox.Items.Clear();
            speciesXPGroupComboBox.Items.Clear();
            speciesTMCheckedListBox.Items.Clear();
            speciesHMCheckedListBox.Items.Clear();

            speciesTMCheckedListBox.Items.AddRange(RomFile.GetTMNames());
            speciesHMCheckedListBox.Items.AddRange(RomFile.GetHMNames());

            speciesComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNames());
            speciesType1ComboBox.Items.AddRange(RomFile.GetTypeNames());
            speciesType2ComboBox.Items.AddRange(RomFile.GetTypeNames());
            speciesAbility1ComboBox.Items.AddRange(RomFile.GetAbilityNames());
            speciesAbility2ComboBox.Items.AddRange(RomFile.GetAbilityNames());
            speciesHeldItem1ComboBox.Items.AddRange(RomFile.GetItemNames());
            speciesHeldItem2ComboBox.Items.AddRange(RomFile.GetItemNames());
            speciesEggGroup1ComboBox.Items.AddRange(RomFile.GetEggGroupNames());
            speciesEggGroup2ComboBox.Items.AddRange(RomFile.GetEggGroupNames());
            speciesXPGroupComboBox.Items.AddRange(RomFile.GetXPGroupNames());
        }

        private void UpdateDisplayedSpeciesValues()
        {
            //event handlers have to removed when checkedlistboxes are updated, otherwise they'll fire when the user isn't interacting with the checkedlistboxes
            this.speciesTMCheckedListBox.ItemCheck -= this.speciesTMCheckedListBox_ItemCheck;
            this.speciesHMCheckedListBox.ItemCheck -= this.speciesHMCheckedListBox_ItemCheck;

            speciesComboBox.SelectedIndex = 0;
            displaySpeciesValues(0);

            this.speciesTMCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.speciesTMCheckedListBox_ItemCheck);
            this.speciesHMCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.speciesHMCheckedListBox_ItemCheck);
        }


        private void displaySpeciesValues(int pokemonIndex)
        {
            speciesControlsCanRecieveUserInput = false;

            speciesHPNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].HP;
            speciesAttackNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].Attack;
            speciesDefenseNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].Defense;
            speciesSpecialAttackNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].SpecialAttack;
            speciesSpecialDefenseNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].SpecialDefense;
            speciesSpeedNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].Speed;

            speciesType1ComboBox.SelectedIndex = RomFile.PokemonSpeciesList[pokemonIndex].Type1;
            speciesType2ComboBox.SelectedIndex = RomFile.PokemonSpeciesList[pokemonIndex].Type2;
            speciesHeldItem1ComboBox.SelectedIndex = RomFile.PokemonSpeciesList[pokemonIndex].Item1;
            speciesHeldItem2ComboBox.SelectedIndex = RomFile.PokemonSpeciesList[pokemonIndex].Item2;
            speciesEggGroup1ComboBox.SelectedIndex = (int)RomFile.PokemonSpeciesList[pokemonIndex].EggGroup1;
            speciesEggGroup2ComboBox.SelectedIndex = (int)RomFile.PokemonSpeciesList[pokemonIndex].EggGroup2;
            speciesAbility1ComboBox.SelectedIndex = RomFile.PokemonSpeciesList[pokemonIndex].Ability1;
            speciesAbility2ComboBox.SelectedIndex = RomFile.PokemonSpeciesList[pokemonIndex].Ability2;
            speciesXPGroupComboBox.SelectedIndex = (int)RomFile.PokemonSpeciesList[pokemonIndex].XPGroup;

            speciesCatchRateNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].CatchRate;
            speciesBaseFriendshipNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].BaseFriendship;
            speciesBaseXPYieldNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].BaseXP;
            speciesSafariRunChanceNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].SafariRunChance;
            speciesEggCyclesNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].NumEggCyles;

            speciesHPEVNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].HPEVYield;
            speciesAttackEVNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].AttackEVYield;
            speciesDefenseEVNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].DefenseEVYield;
            speciesSpecialAttackEVNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].SpecialAttackEVYield;
            speciesSpecialDefenseEVNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].SpecialDefenseEVYield;
            speciesSpeedEVNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].SpeedEVYield;


            if (RomFile.PokemonSpeciesList[pokemonIndex].GetIsMaleOnly())
            {
                speciesMaleOnlyRadioButton.Checked = true;
            }
            else if (RomFile.PokemonSpeciesList[pokemonIndex].GetIsFemaleOnly())
            {
                speciesFemaleOnlyRadioButton.Checked = true;
            }
            else if (RomFile.PokemonSpeciesList[pokemonIndex].GetIsGenderless())
            {
                speciesGenderlessRadioButton.Checked = true;
            }
            else
            {
                speciesMaleAndFemaleRadioButton.Checked = true;
                speciesGenderRatioNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].GenderRatio;
            }

            List<int> learnableTMs;
            learnableTMs = RomFile.PokemonSpeciesList[pokemonIndex].GetLearnableTMs();

            for(int i = 0; i < speciesTMCheckedListBox.Items.Count; i++)
                speciesTMCheckedListBox.SetItemChecked(i, false);

            foreach (int tmIndex in learnableTMs)
                speciesTMCheckedListBox.SetItemChecked(tmIndex, true);


            List<int> learnableHMs;
            learnableHMs = RomFile.PokemonSpeciesList[pokemonIndex].GetLearnableHMs();

            for (int i = 0; i < speciesHMCheckedListBox.Items.Count; i++)
                speciesHMCheckedListBox.SetItemChecked(i, false);

            foreach (int hmIndex in learnableHMs)
                speciesHMCheckedListBox.SetItemChecked(hmIndex, true);

            speciesControlsCanRecieveUserInput = true;
        }

        private void speciesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //event handlers have to removed when checkedlistboxes are updated, otherwise they'll fire when the user isn't interacting with the checkedlistboxes
            this.speciesTMCheckedListBox.ItemCheck -= speciesTMCheckedListBox_ItemCheck;
            this.speciesHMCheckedListBox.ItemCheck -= speciesHMCheckedListBox_ItemCheck;
            displaySpeciesValues(speciesComboBox.SelectedIndex);
            this.speciesTMCheckedListBox.ItemCheck += new ItemCheckEventHandler(this.speciesTMCheckedListBox_ItemCheck);
            this.speciesHMCheckedListBox.ItemCheck += new ItemCheckEventHandler(this.speciesHMCheckedListBox_ItemCheck);

            if (speciesComboBox.SelectedIndex < PokemonSpecies.EGG_SPECIES_INDEX || speciesComboBox.SelectedIndex > PokemonSpecies.BAD_EGG_SPECIES_INDEX)
            {
                speciesBaseStatsGroupBox.Enabled = true;
                speciesEVOnDefeatGroupBox.Enabled = true;
                speciesTypesGroupBox.Enabled = true;
                speciesAbilitiesGroupBox.Enabled = true;
                speciesXPGroupBox.Enabled = true;
                speciesHeldItemsGroupBox.Enabled = true;
                speciesEggGroupsGroupBox.Enabled = true;
                speciesGenderGroupBox.Enabled = true;
                speciesMiscGroupBox.Enabled = true;
                speciesTMCheckedListBox.Enabled = true;
                speciesHMCheckedListBox.Enabled = true;
            }
            else
            {
                speciesBaseStatsGroupBox.Enabled = false;
                speciesEVOnDefeatGroupBox.Enabled = false;
                speciesTypesGroupBox.Enabled = false;
                speciesAbilitiesGroupBox.Enabled = false;
                speciesXPGroupBox.Enabled = false;
                speciesHeldItemsGroupBox.Enabled = false;
                speciesEggGroupsGroupBox.Enabled = false;
                speciesGenderGroupBox.Enabled = false;
                speciesMiscGroupBox.Enabled = false;
                speciesTMCheckedListBox.Enabled = false;
                speciesHMCheckedListBox.Enabled = false;
            }
        }

        private void speciesMaleOnlyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (speciesMaleOnlyRadioButton.Checked)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SetMaleOnlyGenderRatio();
                speciesGenderRatioNumericNoArrows.Enabled = false;
                
                if (speciesControlsCanRecieveUserInput)
                    MarkUnsavedChanges();
            }
        }

        private void speciesFemaleOnlyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (speciesFemaleOnlyRadioButton.Checked)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SetFemaleOnlyGenderRatio();
                speciesGenderRatioNumericNoArrows.Enabled = false;

                if (speciesControlsCanRecieveUserInput)
                    MarkUnsavedChanges();
            }
        }

        private void speciesMaleAndFemaleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (speciesMaleAndFemaleRadioButton.Checked)
            {
                speciesGenderRatioNumericNoArrows.Enabled = true;

                int genderRatio = RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].GenderRatio;
                if (genderRatio < speciesGenderRatioNumericNoArrows.Minimum || genderRatio > speciesGenderRatioNumericNoArrows.Maximum)
                {
                    speciesGenderRatioNumericNoArrows.Value = PokemonSpecies.Get50PercentGenderRatio();
                    RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].GenderRatio = (int)speciesGenderRatioNumericNoArrows.Value;
                }

                if (speciesControlsCanRecieveUserInput)
                    MarkUnsavedChanges();
            }
        }

        private void speciesGenderlessRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (speciesGenderlessRadioButton.Checked)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SetGenderlessGenderRatio();
                speciesGenderRatioNumericNoArrows.Enabled = false;

                if (speciesControlsCanRecieveUserInput)
                    MarkUnsavedChanges();
            }
        }


        private void speciesHPNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].HP != speciesHPNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].HP = (byte)speciesHPNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesAttackNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Attack != speciesAttackNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Attack = (byte)speciesAttackNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesDefenseNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Defense != speciesDefenseNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Defense = (byte)speciesDefenseNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesSpecialAttackNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialAttack != speciesSpecialAttackNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialAttack = (byte)speciesSpecialAttackNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesSpecialDefenseNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialDefense != speciesSpecialDefenseNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialDefense = (byte)speciesSpecialDefenseNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesSpeedNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Speed != speciesSpeedNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Speed = (byte)speciesSpeedNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesHPEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].HPEVYield != speciesHPEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].HPEVYield = (byte)speciesHPEVNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesAttackEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].AttackEVYield != speciesAttackEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].AttackEVYield = (byte)speciesAttackEVNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesDefenseEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].DefenseEVYield != speciesDefenseEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].DefenseEVYield = (byte)speciesDefenseEVNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesSpecialAttackEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialAttackEVYield != speciesSpecialAttackEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialAttackEVYield = (byte)speciesSpecialAttackEVNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesSpecialDefenseEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialDefenseEVYield != speciesSpecialDefenseEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialDefenseEVYield = (byte)speciesSpecialDefenseEVNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesSpeedEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpeedEVYield != speciesSpeedEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpeedEVYield = (byte)speciesSpeedEVNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesBaseXPYieldNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].BaseXP != speciesBaseXPYieldNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].BaseXP = (byte)speciesBaseXPYieldNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesEggCyclesNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].NumEggCyles != speciesEggCyclesNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].NumEggCyles = (byte)speciesEggCyclesNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesCatchRateNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].CatchRate != speciesCatchRateNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].CatchRate = (byte)speciesCatchRateNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesHappinessNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].BaseFriendship != speciesBaseFriendshipNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].BaseFriendship = (byte)speciesBaseFriendshipNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesSafariRunChanceNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SafariRunChance != speciesSafariRunChanceNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SafariRunChance = (byte)speciesSafariRunChanceNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesGenderRatioNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].GenderRatio != speciesGenderRatioNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].GenderRatio = (byte)speciesGenderRatioNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void speciesType1ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Type1 != speciesType1ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Type1 = (byte)speciesType1ComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void speciesType2ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Type2 != speciesType2ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Type2 = (byte)speciesType2ComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void speciesAbility1ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Ability1 != speciesAbility1ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Ability1 = (byte)speciesAbility1ComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void speciesAbility2ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Ability2 != speciesAbility2ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Ability2 = (byte)speciesAbility2ComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void speciesXPGroupComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].XPGroup != (PokemonSpecies.XPGroups)speciesXPGroupComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].XPGroup = (PokemonSpecies.XPGroups)speciesXPGroupComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void speciesHeldItem1ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Item1 != speciesHeldItem1ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Item1 = (ushort)speciesHeldItem1ComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void speciesHeldItem2ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Item2 != speciesHeldItem2ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Item2 = (ushort)speciesHeldItem2ComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void speciesEggGroup1ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].EggGroup1 != (PokemonSpecies.EggGroups)speciesEggGroup1ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].EggGroup1 = (PokemonSpecies.EggGroups)speciesEggGroup1ComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void speciesEggGroup2ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].EggGroup2 != (PokemonSpecies.EggGroups)speciesEggGroup2ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].EggGroup2 = (PokemonSpecies.EggGroups)speciesEggGroup2ComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void speciesTMCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (speciesControlsCanRecieveUserInput)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SetLearnableTM(e.Index, e.NewValue.HasFlag(CheckState.Checked));
                MarkUnsavedChanges();
            }
        }

        private void speciesHMCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (speciesControlsCanRecieveUserInput)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SetLearnableHM(e.Index, e.NewValue.HasFlag(CheckState.Checked));
                MarkUnsavedChanges();
            }
        }
    }
}
