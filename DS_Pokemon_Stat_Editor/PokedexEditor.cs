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
                pokedexHTMetersNumericNoArrows.Visible = false;
                pokedexHTMetersLabel.Visible = false;
                pokedexHTFTNumericNoArrows.Visible = true;
                pokedexHTINNumericNoArrows.Visible = true;
                pokedexHTFTLabel.Visible = true;
                pokedexHTINLabel.Visible = true;
                pokedexWTUnitLabel.Text = "lbs.";

                pokedexHTFTLabel.Location = new Point(pokedexHTFTLabel.Location.X, pokedexHTMetersLabel.Location.Y);
                pokedexHTINLabel.Location = new Point(pokedexHTINLabel.Location.X, pokedexHTMetersLabel.Location.Y);
                pokedexHTFTNumericNoArrows.Location = new Point(pokedexHTFTNumericNoArrows.Location.X, pokedexHTMetersLabel.Location.Y);
                pokedexHTINNumericNoArrows.Location = new Point(pokedexHTINNumericNoArrows.Location.X, pokedexHTMetersLabel.Location.Y);
            }
            else
            {
                pokedexHTMetersNumericNoArrows.Visible = true;
                pokedexHTMetersLabel.Visible = true;
                pokedexHTFTNumericNoArrows.Visible = false;
                pokedexHTINNumericNoArrows.Visible = false;
                pokedexHTFTLabel.Visible = false;
                pokedexHTINLabel.Visible = false;
                pokedexWTUnitLabel.Text = "kg";
            }

            pokedexNumNationalNumericUpDown.Maximum = RomFile.GetNumPokemon();
        }

        private void UpdateDisplayedPokedexValues()
        {
            //we need the event handler to only be set after all text is loaded into the controls
            this.pokedexNameComboBox.SelectedValueChanged += new System.EventHandler(this.pokedexNameComboBox_SelectedValueChanged);

            pokedexNameComboBox.SelectedIndex = 0; //makes pound the initially selected move
        }

        private void DisplayPokedexValues(int pokemonIndex)
        {
            pokedexCategoryTextBox.Text = RomFile.GetPokedexCategory(pokemonIndex);

            pokedexNumChangedByCode = true;
            pokedexNumNationalNumericUpDown.Value = pokemonIndex + 1; //add 1 because pokedex num is not a 0th based index
            pokedexNumChangedByCode = false;

            pokdexEntryRichTextBox.Text = RomFile.GetPokedexDescription(pokemonIndex);
            
            if (RomFile.Language == Languages.ENGLISH)
            {
                pokedexWTNumericNoArrows.Value = (decimal)RomFile.WeightList[pokemonIndex].GetPounds();
                pokedexHTFTNumericNoArrows.Value = RomFile.HeightList[pokemonIndex].GetFeet();
                pokedexHTINNumericNoArrows.Value = RomFile.HeightList[pokemonIndex].GetInches();
            }
            else
            {
                pokedexWTNumericNoArrows.Value = (decimal)RomFile.WeightList[pokemonIndex].GetKilograms();
                pokedexHTMetersNumericNoArrows.Value = (decimal)RomFile.HeightList[pokemonIndex].GetMeters();
            }
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
                MarkUnsavedChanges();
            }
            else if (RomFile.WeightList[pokedexNameComboBox.SelectedIndex].GetKilograms() != (double)pokedexWTNumericNoArrows.Value)
            {
                RomFile.WeightList[pokedexNameComboBox.SelectedIndex].SetKilograms((double)pokedexWTNumericNoArrows.Value);
                MarkUnsavedChanges();
            }
        }

        private void pokedexHTMetersNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.HeightList[pokedexNameComboBox.SelectedIndex].GetMeters() != (double)pokedexHTMetersNumericNoArrows.Value)
            {
                RomFile.HeightList[pokedexNameComboBox.SelectedIndex].SetMeters((double)pokedexHTMetersNumericNoArrows.Value);
                MarkUnsavedChanges();
            }
        }

        private void pokedexHTFTNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.HeightList[pokedexNameComboBox.SelectedIndex].GetFeet() != (int)pokedexHTFTNumericNoArrows.Value)
            {
                RomFile.HeightList[pokedexNameComboBox.SelectedIndex].SetImperial((int)pokedexHTFTNumericNoArrows.Value, (int)pokedexHTINNumericNoArrows.Value);
                MarkUnsavedChanges();
            }
        }

        private void pokedexHTINNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.HeightList[pokedexNameComboBox.SelectedIndex].GetInches() != (int)pokedexHTINNumericNoArrows.Value)
            {
                RomFile.HeightList[pokedexNameComboBox.SelectedIndex].SetImperial((int)pokedexHTFTNumericNoArrows.Value, (int)pokedexHTINNumericNoArrows.Value);
                MarkUnsavedChanges();
            }
        }
    }
}
