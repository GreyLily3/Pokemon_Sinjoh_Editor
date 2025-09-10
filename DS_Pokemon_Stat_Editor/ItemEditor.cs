using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon_Sinjoh_Editor
{
    partial class MainForm
    {
        private void setupItemText()
        {
            itemSelectedComboBox.Items.Clear();
            itemNaturalGiftTypeComboBox.Items.Clear();
            itemSelectedComboBox.Items.AddRange(RomFile.GetItemNamesWithoutUnknown());
            itemNaturalGiftTypeComboBox.Items.AddRange(RomFile.GetTypeNames());
            itemFieldPocketComboBox.Items.AddRange(RomFile.GetFieldPocketNames());
            itemBattlePocketComboBox.Items.AddRange(RomFile.GetBattlePocketNames());
        }

        private void updateDisplayedItemValues()
        {
            itemSelectedComboBox.SelectedIndex = 0;
            displayItemValues(0);
        }

        private void displayItemValues(int selectedItem)
        {
            bool canUseNaturalGift;

            itemPriceNumericNoArrows.Value = RomFile.ItemList[selectedItem].Price;
            itemHoldEffectNumericNoArrows.Value = RomFile.ItemList[selectedItem].HoldEffect;
            itemHoldParameterNumericNoArrows.Value = RomFile.ItemList[selectedItem].HoldEffectParameter;
            itemPluckEffectNumericNoArrows.Value = RomFile.ItemList[selectedItem].PluckEffect;
            itemFlingPowerNumericNoArrows.Value = RomFile.ItemList[selectedItem].FlingPower;
            itemFlingEffectNumericNoArrows.Value = RomFile.ItemList[selectedItem].FlingEffect;

            canUseNaturalGift = RomFile.ItemList[selectedItem].CanNaturalGiftUse();
            itemNaturalGiftUseableCheckBox.Checked = canUseNaturalGift;

            if (canUseNaturalGift)
            {
                itemNaturalGiftTypeComboBox.Enabled = true;
                itemNaturalGiftTypeComboBox.SelectedIndex = RomFile.ItemList[selectedItem].NaturalGiftType;
            }
            else
                itemNaturalGiftTypeComboBox.Enabled = false;

            itemFieldPocketComboBox.SelectedIndex = (int)RomFile.ItemList[selectedItem].FieldPocket;
            itemBattlePocketComboBox.SelectedIndex = (int)RomFile.ItemList[selectedItem].BattlePocket;

            itemNaturalGiftPowerNumericNoArrows.Value = RomFile.ItemList[selectedItem].NaturalGiftPower;
            itemPreventTossCheckBox.Checked = RomFile.ItemList[selectedItem].PreventToss;


            
        }
        private void itemSelectedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayItemValues(itemSelectedComboBox.SelectedIndex);
        }
    }
}
