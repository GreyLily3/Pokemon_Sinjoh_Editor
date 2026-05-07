using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Sinjoh_Editor
{
    partial class MainForm
    {
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
            pokedexNumTextBox.Text = (pokemonIndex + 1).ToString();
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
    }
}
