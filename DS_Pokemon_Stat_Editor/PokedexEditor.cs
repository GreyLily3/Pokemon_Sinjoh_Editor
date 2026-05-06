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
            
        }

        private void pokedexNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            DisplayPokedexValues(pokedexNameComboBox.SelectedIndex);
        }
    }
}
