using Pokemon_Sinjoh_Editor.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Pokemon_Sinjoh_Editor
{
    partial class MainForm
    {
        private bool pokedexNumChangedByCode = false;

        private void setupPokedexText()
        {
            pokedexNameComboBox.Items.Clear();
            pokedexNameComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNamesNoAltForms());

            if (RomFile.Language == Languages.ENGLISH)
            {
                pokedexWTUnitLabel.Text = "lbs.";
            }
            else
            {
                pokedexWTUnitLabel.Text = "kg";
            }

            pokedexNumNationalNumericUpDown.Maximum = RomFile.GetNumPokemon();
            pokedexHTDecimetersNumericUpDown.Maximum = uint.MaxValue;
            pokedexHTMetersNumericNoArrows.Maximum = uint.MaxValue;
            pokedexHTFTNumericNoArrows.Maximum = uint.MaxValue;
        }

        private void UpdateDisplayedPokedexValues()
        {
            //we need the event handler to only be set after all text is loaded into the controls
            this.pokedexNameComboBox.SelectedValueChanged += new System.EventHandler(this.pokedexNameComboBox_SelectedValueChanged);

            pokedexNameComboBox.SelectedIndex = 0; //makes bulbasaur's entry the first entry selected
        }

        private void DisplayPokedexValues(int pokemonIndex)
        {
            pokedexCategoryTextBox.Text = RomFile.GetPokedexCategory(pokemonIndex);

            pokedexNumChangedByCode = true;
            pokedexNumNationalNumericUpDown.Value = pokemonIndex + 1; //add 1 because pokedex num is not a 0th based index
            pokedexNumChangedByCode = false;

            pokdexEntryRichTextBox.Text = RomFile.GetPokedexDescription(pokemonIndex);

            pokedexHTDecimetersNumericUpDown.Value = RomFile.HeightList[pokemonIndex].decimeters;
            pokedexHTFTNumericNoArrows.Value = RomFile.HeightList[pokemonIndex].GetFeet();
            pokedexHTINNumericNoArrows.Value = RomFile.HeightList[pokemonIndex].GetInches();
            pokedexHTMetersNumericNoArrows.Value = (decimal)RomFile.HeightList[pokemonIndex].GetMeters();

            if (RomFile.Language == Languages.ENGLISH)
                pokedexWTNumericNoArrows.Value = (decimal)RomFile.WeightList[pokemonIndex].GetPounds();
            else
                pokedexWTNumericNoArrows.Value = (decimal)RomFile.WeightList[pokemonIndex].GetKilograms();

        }

        private void pokedexNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            DisplayPokedexValues(pokedexNameComboBox.SelectedIndex);
        }

        private void pokedexNumNationalNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!pokedexNumChangedByCode)
                pokedexNameComboBox.SelectedIndex = (int)pokedexNumNationalNumericUpDown.Value - 1;
        }

        private void pokedexWTNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.Language == Languages.ENGLISH && RomFile.WeightList[pokedexNameComboBox.SelectedIndex].GetPounds() != (double)pokedexWTNumericNoArrows.Value)
            {
                RomFile.WeightList[pokedexNameComboBox.SelectedIndex].SetImperial((double)pokedexWTNumericNoArrows.Value);
                MarkUnsavedChanges(SaveSubFile.POKEDEX);
                //make sure the weight displayed matches the rounding used in game
                pokedexWTNumericNoArrows.Value = (decimal)RomFile.WeightList[pokedexNameComboBox.SelectedIndex].GetPounds(); 
            }
            else if (RomFile.Language != Languages.ENGLISH && RomFile.WeightList[pokedexNameComboBox.SelectedIndex].GetKilograms() != (double)pokedexWTNumericNoArrows.Value)
            {
                RomFile.WeightList[pokedexNameComboBox.SelectedIndex].SetKilograms((double)pokedexWTNumericNoArrows.Value);
                MarkUnsavedChanges(SaveSubFile.POKEDEX);
            }
        }

        private void pokedexHTMetersNumericNoArrows_Validated(object sender, EventArgs e)
        {

        }

        private void pokedexHTFTNumericNoArrows_Validated(object sender, EventArgs e)
        {
 
        }

        private void pokedexHTINNumericNoArrows_Validated(object sender, EventArgs e)
        {

        }

        private void pokedexHTDecimetersNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (RomFile.HeightList[pokedexNameComboBox.SelectedIndex].decimeters != (uint)pokedexHTDecimetersNumericUpDown.Value)
            {
                RomFile.HeightList[pokedexNameComboBox.SelectedIndex].decimeters = (uint)pokedexHTDecimetersNumericUpDown.Value;
                MarkUnsavedChanges(SaveSubFile.POKEDEX);

                pokedexHTINNumericNoArrows.Value = RomFile.HeightList[pokedexNameComboBox.SelectedIndex].GetInches();
                pokedexHTFTNumericNoArrows.Value = RomFile.HeightList[pokedexNameComboBox.SelectedIndex].GetFeet();
                pokedexHTMetersNumericNoArrows.Value = (decimal)RomFile.HeightList[pokedexNameComboBox.SelectedIndex].GetMeters();
            }
        }
    }
}
