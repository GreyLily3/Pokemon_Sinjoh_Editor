using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Sinjoh_Editor
{
    partial class MainForm
    {
        private void LoadTradeControlText()
        {
            tradeWantedPokemonComboBox.Items.Clear();
            tradeOfferedPokemonComboBox.Items.Clear();
            tradeHeldItemComboBox.Items.Clear();
            tradeAbilityComboBox.Items.Clear();
            tradeTrainerComboBox.Items.Clear();
            tradeLanguageComboBox.Items.Clear();
            tradeGenderComboBox.Items.Clear();

            tradeWantedPokemonComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNames());
            tradeOfferedPokemonComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNames());

            tradeHeldItemComboBox.Items.AddRange(RomFile.GetItemNames());
            tradeAbilityComboBox.Items.AddRange(RomFile.GetAbilityNames());
            tradeLanguageComboBox.Items.AddRange(RomFile.GetLanguageNames());
            tradeGenderComboBox.Items.AddRange(RomFile.GetWantedGenderNames());


            for (int i = RomFile.NPCTradesList.Count; i < RomFile.NPCTradesList.Count * 2; i++)
                tradeTrainerComboBox.Items.Add(RomFile.TradedPokemonAndTrainerNicknames[i]);
        }

        private void UpdateDisplayedTradeValues()
        {
            tradeTrainerComboBox.SelectedIndex = 0;
            DisplayTradeValues(0);
        }

        private void DisplayTradeValues(int tradeIndex)
        {
            //subtract 1 from the pokemon's index because the names in the combo boxes start at 0
            tradeWantedPokemonComboBox.SelectedIndex = RomFile.NPCTradesList[tradeIndex].WantedPokemon - 1;
            tradeOfferedPokemonComboBox.SelectedIndex = RomFile.NPCTradesList[tradeIndex].OfferedPokemon - 1;

            tradeHeldItemComboBox.SelectedIndex = RomFile.NPCTradesList[tradeIndex].HeldItem;
            tradeAbilityComboBox.SelectedIndex = RomFile.NPCTradesList[tradeIndex].Ability;

            tradeHPIVsNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].HPIV;
            tradeAttackIVsNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].AttackIV;
            tradeDefenseIVsNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].DefenseIV;
            tradeSpeedIVsNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].SpeedIV;
            tradeSpecialAttackIVsNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].SpecialAttckIV;
            tradeSpecialDefenseIVsNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].SpecialDefenseIV;

            tradeCoolNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].Cool;
            tradeBeautyNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].Beauty;
            tradeCuteNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].Cute;
            tradeSmartNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].Smart;
            tradeToughNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].Tough;

            tradePVNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].PersonalityValue;
            tradeOriginalTrainerIDNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].OriginalTrainerID;
            tradeSheenNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].Sheen;

            tradeLanguageComboBox.SelectedIndex = (int)RomFile.NPCTradesList[tradeIndex].LanguageOfOrigin - 1;
            tradeGenderComboBox.SelectedIndex = (int)RomFile.NPCTradesList[tradeIndex].Gender;

            tradeNicknameTextBox.Text = RomFile.TradedPokemonAndTrainerNicknames[tradeIndex];
        }

        private void tradeTrainerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayTradeValues(tradeTrainerComboBox.SelectedIndex);
        }
    }
}
