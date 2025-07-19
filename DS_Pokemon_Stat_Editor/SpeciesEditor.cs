using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Pokemon_Sinjoh_Editor
{
    partial class MainForm
    {
        

        private void loadSpeciesData()
        {
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
            speciesTMCheckedListBox.Items.AddRange(RomFile.GetTMNames());
            speciesHMCheckedListBox.Items.AddRange(RomFile.GetHMNames());

            speciesComboBox.SelectedIndex = 0;
            displaySpeciesValues(0);
        }

        private void displaySpeciesValues(int pokemonIndex)
        {
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
            speciesHappinessNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].BaseHappiness;
            speciesBaseXPYieldNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].BaseXP;
            speciesSafariRunChanceNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].SafariRunChance;
            speciesEggCyclesNumericNoArrows.Value = RomFile.PokemonSpeciesList[pokemonIndex].NumEggCyles;

            if (RomFile.PokemonSpeciesList[pokemonIndex].GenderRatio == PokemonSpecies.GENDER_RATIO_MALE_ONLY)
            {
                speciesMaleOnlyRadioButton.Checked = true;
            }
            else if (RomFile.PokemonSpeciesList[pokemonIndex].GenderRatio == PokemonSpecies.GENDER_RATIO_FEMALE_ONLY)
            {
                speciesFemaleOnlyRadioButton.Checked = true;
            }
            else if (RomFile.PokemonSpeciesList[pokemonIndex].GenderRatio == PokemonSpecies.GENDER_RATIO_GENDERLESS)
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

        }

        private void speciesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            displaySpeciesValues(speciesComboBox.SelectedIndex);
        }

        private void speciesMaleOnlyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (speciesMaleOnlyRadioButton.Checked)
            {
                speciesFemaleOnlyRadioButton.Checked = false;
                speciesGenderlessRadioButton.Checked = false;
                speciesMaleAndFemaleRadioButton.Checked = false;
                speciesGenderRatioNumericNoArrows.Enabled = false;
            }
        }

        private void speciesFemaleOnlyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (speciesFemaleOnlyRadioButton.Checked)
            {
                speciesMaleOnlyRadioButton.Checked = false;
                speciesGenderlessRadioButton.Checked = false;
                speciesMaleAndFemaleRadioButton.Checked = false;
                speciesGenderRatioNumericNoArrows.Enabled = false;
            }
        }

        private void speciesMaleAndFemaleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (speciesMaleAndFemaleRadioButton.Checked)
            {
                speciesMaleOnlyRadioButton.Checked = false;
                speciesGenderlessRadioButton.Checked = false;
                speciesFemaleOnlyRadioButton.Checked = false;
                speciesGenderRatioNumericNoArrows.Enabled = true;
            }
        }

        private void speciesGenderlessRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (speciesGenderlessRadioButton.Checked)
            {
                speciesMaleOnlyRadioButton.Checked = false;
                speciesFemaleOnlyRadioButton.Checked = false;
                speciesMaleAndFemaleRadioButton.Checked = false;
                speciesGenderRatioNumericNoArrows.Enabled = false;
            }
        }


    }
}
