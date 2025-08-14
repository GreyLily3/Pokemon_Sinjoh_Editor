using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
            tradeTrainerComboBox.Items.Clear();
            tradeLanguageComboBox.Items.Clear();

            tradeWantedPokemonComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNames());
            tradeOfferedPokemonComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNames());

            tradeHeldItemComboBox.Items.AddRange(RomFile.GetItemNames());
            tradeLanguageComboBox.Items.AddRange(RomFile.GetLanguageNames());

            tradeTrainerComboBox.Items.AddRange(RomFile.GetTradePokemonTrainerNames());
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

            tradePVNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].PersonalityValue.PV;
            tradeOriginalTrainerIDNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].OriginalTrainerID;
            tradeSheenNumericNoArrows.Value = RomFile.NPCTradesList[tradeIndex].Sheen;

            tradeLanguageComboBox.SelectedIndex = (int)RomFile.NPCTradesList[tradeIndex].LanguageOfOrigin - 1;

            tradeNicknameTextBox.Text = RomFile.TradePokemonNicknames[tradeIndex];
        }

        private void tradeTrainerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayTradeValues(tradeTrainerComboBox.SelectedIndex);

            if (RomFile.gameFamily == RomFile.GameFamilies.HGSS && tradeTrainerComboBox.SelectedIndex >= RomFile.TRADE_JASMINE_INDEX && tradeTrainerComboBox.SelectedIndex <= RomFile.TRADE_WEBSTER_INDEX)
            {
                tradeWantedPokemonComboBox.Visible = false;

                if (tradeTrainerComboBox.SelectedIndex == RomFile.TRADE_JASMINE_INDEX)
                {
                    tradeWantedPokemonLabel.Visible = true;
                    tradeAnyPokemonWantedComboBox.SelectedIndex = 0;
                    tradeAnyPokemonWantedComboBox.Visible = true;
                    tradeAnyPokemonWantedComboBox.Location = tradeWantedPokemonComboBox.Location;
                }
                else
                {
                    tradeAnyPokemonWantedComboBox.Visible = false;
                    tradeWantedPokemonLabel.Visible = false;
                }
                    
            }
            else
            {
                tradeWantedPokemonComboBox.Visible = true;
                tradeWantedPokemonLabel.Visible = true;
                tradeAnyPokemonWantedComboBox.Visible = false;
            }
        }

        private void tradeOriginalTrainerIDNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].OriginalTrainerID != (ushort)tradeOriginalTrainerIDNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].OriginalTrainerID = (ushort)tradeOriginalTrainerIDNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradePVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.PV != tradePVNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.PV = (uint)tradePVNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeCoolNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Cool != tradeCoolNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Cool = (byte)tradeCoolNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeBeautyNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Beauty != tradeBeautyNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Beauty = (byte)tradeBeautyNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeCuteNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Cute != tradeCuteNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Cute = (byte)tradeCuteNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeSmartNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Smart != tradeSmartNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Smart = (byte)tradeSmartNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeToughNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Tough != tradeToughNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Tough = (byte)tradeToughNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeSheenNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Sheen != tradeSheenNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Sheen = (byte)tradeSheenNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeHPIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].HPIV != tradeHPIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].HPIV = (byte)tradeHPIVsNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeAttackIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].AttackIV != tradeAttackIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].AttackIV = (byte)tradeAttackIVsNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeDefenseIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].DefenseIV != tradeDefenseIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].DefenseIV = (byte)tradeDefenseIVsNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeSpeedIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpeedIV != tradeSpeedIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpeedIV = (byte)tradeSpeedIVsNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeSpecialAttackIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpecialAttckIV != tradeSpecialAttackIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpecialAttckIV = (byte)tradeSpecialAttackIVsNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeSpecialDefenseIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpecialDefenseIV != tradeSpecialAttackIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpecialDefenseIV = (byte)tradeSpecialAttackIVsNumericNoArrows.Value;
                MarkUnsavedChanges();
            }
        }

        private void tradeWantedPokemonComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].WantedPokemon != tradeWantedPokemonComboBox.SelectedIndex + 1)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].WantedPokemon = (ushort)(tradeWantedPokemonComboBox.SelectedIndex + 1);
                MarkUnsavedChanges();
            }
        }

        private void tradeOfferedPokemonComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].OfferedPokemon != tradeOfferedPokemonComboBox.SelectedIndex + 1)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].OfferedPokemon = (ushort)(tradeOfferedPokemonComboBox.SelectedIndex + 1);
                MarkUnsavedChanges();
            }
        }

        private void tradeLanguageComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].LanguageOfOrigin != (Languages)tradeLanguageComboBox.SelectedIndex)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].LanguageOfOrigin = (Languages)tradeLanguageComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void tradeHeldItemComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].HeldItem != tradeHeldItemComboBox.SelectedIndex)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].HeldItem = (ushort)tradeHeldItemComboBox.SelectedIndex;
                MarkUnsavedChanges();
            }
        }

        private void tradePVNumericNoArrows_ValueChanged(object sender, EventArgs e)
        {
            PersonalityValue pv = new PersonalityValue((uint)tradePVNumericNoArrows.Value);

            tradeGenderTextBox.Text = pv.GetGender(RomFile.PokemonSpeciesList[tradeOfferedPokemonComboBox.SelectedIndex].GenderRatio).ToString();
            tradeNatureTextBox.Text = pv.GetNature().ToString();
            tradeAbilityTextBox.Text = RomFile.GetAbilityName(tradeOfferedPokemonComboBox.SelectedIndex, pv);
        }

    }
}
