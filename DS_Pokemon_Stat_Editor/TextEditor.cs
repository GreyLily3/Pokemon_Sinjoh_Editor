using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Pokemon_Sinjoh_Editor
{
    partial class MainForm
    {
        private void setupTextEditor()
        {
            textInGameListView.Columns.Add("Text", -2, HorizontalAlignment.Left);
            textInGameListView.LabelEdit = true;

            TextBankIndexNumericUpDown.Maximum = RomFile.GetNumTextbanks();

        }

        public void UpdateDisplayedTextValues()
        {
            DisplayTextValues();
        }

        private void DisplayTextValues()
        {
            List<string> currentTextBank = RomFile.gameText.TextBanks[(int)TextBankIndexNumericUpDown.Value];

            textInGameListView.Clear();
            textInGameListView.Columns.Add("Text", -2, HorizontalAlignment.Left);

            textInGameListView.BeginUpdate();
            foreach (string gameTextString in currentTextBank)
            {
                textInGameListView.Items.Add(gameTextString);
            }
            textInGameListView.EndUpdate();
        }

        private void TextBankIndexNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            DisplayTextValues();
        }

        private void textBankComboBox_SelectedValueChanged(object sender, EventArgs e)
        {

        }
    }
}
