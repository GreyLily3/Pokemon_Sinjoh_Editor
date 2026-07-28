using Pokemon_Sinjoh_Editor.Enums;
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

            speciesComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNames());
            speciesType1ComboBox.Items.AddRange(RomFile.TypeNames.ToArray());
            speciesType2ComboBox.Items.AddRange(RomFile.TypeNames.ToArray());
            speciesAbility1ComboBox.Items.AddRange(RomFile.AbilityNames.ToArray());
            speciesAbility2ComboBox.Items.AddRange(RomFile.AbilityNames.ToArray());
            speciesHeldItem1ComboBox.Items.AddRange(RomFile.ItemNames.ToArray());
            speciesHeldItem2ComboBox.Items.AddRange(RomFile.ItemNames.ToArray());
            speciesEggGroup1ComboBox.Items.AddRange(TextArchive.GetEggGroupNames());
            speciesEggGroup2ComboBox.Items.AddRange(TextArchive.GetEggGroupNames());
            speciesXPGroupComboBox.Items.AddRange(TextArchive.GetXPGroupNames());
        }

        private void UpdateDisplayedSpeciesValues()
        {
            speciesComboBox.SelectedIndex = 0;
            displaySpeciesValues(0);
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

            speciesControlsCanRecieveUserInput = true;
        }

        private void speciesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            displaySpeciesValues(speciesComboBox.SelectedIndex);

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
                learnsetTMCheckedListBox.Enabled = true;
                learnsetHMCheckedListBox.Enabled = true;
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
                learnsetTMCheckedListBox.Enabled = false;
                learnsetHMCheckedListBox.Enabled = false;
            }
        }

        private void speciesMaleOnlyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (speciesMaleOnlyRadioButton.Checked)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SetMaleOnlyGenderRatio();
                speciesGenderRatioNumericNoArrows.Enabled = false;
                
                if (speciesControlsCanRecieveUserInput)
                    MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesFemaleOnlyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (speciesFemaleOnlyRadioButton.Checked)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SetFemaleOnlyGenderRatio();
                speciesGenderRatioNumericNoArrows.Enabled = false;

                if (speciesControlsCanRecieveUserInput)
                    MarkUnsavedChanges(SaveSubFile.SPECIES);
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
                    MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesGenderlessRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (speciesGenderlessRadioButton.Checked)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SetGenderlessGenderRatio();
                speciesGenderRatioNumericNoArrows.Enabled = false;

                if (speciesControlsCanRecieveUserInput)
                    MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }


        private void speciesHPNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].HP != speciesHPNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].HP = (byte)speciesHPNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesAttackNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Attack != speciesAttackNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Attack = (byte)speciesAttackNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesDefenseNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Defense != speciesDefenseNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Defense = (byte)speciesDefenseNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesSpecialAttackNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialAttack != speciesSpecialAttackNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialAttack = (byte)speciesSpecialAttackNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesSpecialDefenseNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialDefense != speciesSpecialDefenseNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialDefense = (byte)speciesSpecialDefenseNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesSpeedNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Speed != speciesSpeedNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Speed = (byte)speciesSpeedNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesHPEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].HPEVYield != speciesHPEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].HPEVYield = (byte)speciesHPEVNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesAttackEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].AttackEVYield != speciesAttackEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].AttackEVYield = (byte)speciesAttackEVNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesDefenseEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].DefenseEVYield != speciesDefenseEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].DefenseEVYield = (byte)speciesDefenseEVNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesSpecialAttackEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialAttackEVYield != speciesSpecialAttackEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialAttackEVYield = (byte)speciesSpecialAttackEVNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesSpecialDefenseEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialDefenseEVYield != speciesSpecialDefenseEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpecialDefenseEVYield = (byte)speciesSpecialDefenseEVNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesSpeedEVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpeedEVYield != speciesSpeedEVNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SpeedEVYield = (byte)speciesSpeedEVNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesBaseXPYieldNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].BaseXP != speciesBaseXPYieldNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].BaseXP = (byte)speciesBaseXPYieldNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesEggCyclesNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].NumEggCyles != speciesEggCyclesNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].NumEggCyles = (byte)speciesEggCyclesNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesCatchRateNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].CatchRate != speciesCatchRateNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].CatchRate = (byte)speciesCatchRateNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesHappinessNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].BaseFriendship != speciesBaseFriendshipNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].BaseFriendship = (byte)speciesBaseFriendshipNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesSafariRunChanceNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SafariRunChance != speciesSafariRunChanceNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].SafariRunChance = (byte)speciesSafariRunChanceNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesGenderRatioNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].GenderRatio != speciesGenderRatioNumericNoArrows.Value)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].GenderRatio = (byte)speciesGenderRatioNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesType1ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Type1 != speciesType1ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Type1 = (byte)speciesType1ComboBox.SelectedIndex;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesType2ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Type2 != speciesType2ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Type2 = (byte)speciesType2ComboBox.SelectedIndex;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesAbility1ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Ability1 != speciesAbility1ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Ability1 = (byte)speciesAbility1ComboBox.SelectedIndex;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesAbility2ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Ability2 != speciesAbility2ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Ability2 = (byte)speciesAbility2ComboBox.SelectedIndex;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesXPGroupComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].XPGroup != (PokemonSpecies.XPGroups)speciesXPGroupComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].XPGroup = (PokemonSpecies.XPGroups)speciesXPGroupComboBox.SelectedIndex;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesHeldItem1ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Item1 != speciesHeldItem1ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Item1 = (ushort)speciesHeldItem1ComboBox.SelectedIndex;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesHeldItem2ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Item2 != speciesHeldItem2ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].Item2 = (ushort)speciesHeldItem2ComboBox.SelectedIndex;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesEggGroup1ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].EggGroup1 != (PokemonSpecies.EggGroups)speciesEggGroup1ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].EggGroup1 = (PokemonSpecies.EggGroups)speciesEggGroup1ComboBox.SelectedIndex;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }

        private void speciesEggGroup2ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].EggGroup2 != (PokemonSpecies.EggGroups)speciesEggGroup2ComboBox.SelectedIndex)
            {
                RomFile.PokemonSpeciesList[speciesComboBox.SelectedIndex].EggGroup2 = (PokemonSpecies.EggGroups)speciesEggGroup2ComboBox.SelectedIndex;
                MarkUnsavedChanges(SaveSubFile.SPECIES);
            }
        }
    }
}
