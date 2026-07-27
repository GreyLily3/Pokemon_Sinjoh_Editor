using Pokemon_Sinjoh_Editor.Enums;
using System;


namespace Pokemon_Sinjoh_Editor
{
    partial class MainForm
    {
        private void setupTradeText()
        {
            tradeWantedPokemonComboBox.Items.Clear();
            tradeOfferedPokemonComboBox.Items.Clear();
            tradeHeldItemComboBox.Items.Clear();
            tradeTrainerComboBox.Items.Clear();
            tradeLanguageComboBox.Items.Clear();
            tradeNewPVAbilityComboBox.Items.Clear();
            tradeNewPVNatureComboBox.Items.Clear();
            tradeNewPVGenderComboBox.Items.Clear();

            tradeWantedPokemonComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNamesNoAltForms());
            tradeOfferedPokemonComboBox.Items.AddRange(RomFile.GetPokemonSpeciesNamesNoAltForms());

            tradeHeldItemComboBox.Items.AddRange(RomFile.ItemNames.ToArray());
            tradeLanguageComboBox.Items.AddRange(TextArchive.GetLanguageNames());

            tradeTrainerComboBox.Items.AddRange(RomFile.TradePokemonTrainerNames.ToArray());

            tradeNewPVNatureComboBox.Items.AddRange(RomFile.NatureNames.ToArray());
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

            if (RomFile.gameFamily == RomFile.GameFamilies.HGSS && tradeTrainerComboBox.SelectedIndex >= NPCTrade.TRADE_JASMINE_INDEX && tradeTrainerComboBox.SelectedIndex <= NPCTrade.TRADE_WEBSTER_INDEX)
            {
                tradeWantedPokemonComboBox.Visible = false;

                if (tradeTrainerComboBox.SelectedIndex == NPCTrade.TRADE_JASMINE_INDEX)
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
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradePVNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.PV != tradePVNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.PV = (uint)tradePVNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeCoolNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Cool != tradeCoolNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Cool = (byte)tradeCoolNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeBeautyNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Beauty != tradeBeautyNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Beauty = (byte)tradeBeautyNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeCuteNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Cute != tradeCuteNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Cute = (byte)tradeCuteNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeSmartNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Smart != tradeSmartNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Smart = (byte)tradeSmartNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeToughNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Tough != tradeToughNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Tough = (byte)tradeToughNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeSheenNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Sheen != tradeSheenNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].Sheen = (byte)tradeSheenNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeHPIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].HPIV != tradeHPIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].HPIV = (byte)tradeHPIVsNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeAttackIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].AttackIV != tradeAttackIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].AttackIV = (byte)tradeAttackIVsNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeDefenseIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].DefenseIV != tradeDefenseIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].DefenseIV = (byte)tradeDefenseIVsNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeSpeedIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpeedIV != tradeSpeedIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpeedIV = (byte)tradeSpeedIVsNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeSpecialAttackIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpecialAttckIV != tradeSpecialAttackIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpecialAttckIV = (byte)tradeSpecialAttackIVsNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeSpecialDefenseIVsNumericNoArrows_Validated(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpecialDefenseIV != tradeSpecialAttackIVsNumericNoArrows.Value)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].SpecialDefenseIV = (byte)tradeSpecialAttackIVsNumericNoArrows.Value;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeWantedPokemonComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].WantedPokemon != tradeWantedPokemonComboBox.SelectedIndex + 1)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].WantedPokemon = (ushort)(tradeWantedPokemonComboBox.SelectedIndex + 1);
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeOfferedPokemonComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].OfferedPokemon != tradeOfferedPokemonComboBox.SelectedIndex + 1)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].OfferedPokemon = (ushort)(tradeOfferedPokemonComboBox.SelectedIndex + 1);
                MarkUnsavedChanges(SaveSubFile.TRADES);
                updatePVDerivedFields();
            }
        }

        private void tradeOfferedPokemonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void tradeRandomPVButton_Click(object sender, EventArgs e)
        {
            PokemonSpecies species = RomFile.PokemonSpeciesList[tradeOfferedPokemonComboBox.SelectedIndex];
            NPCTrade trade = RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex];
            bool onlyOneGender;
            Gender gender;
            Nature nature = (Nature)tradeNewPVNatureComboBox.SelectedIndex;
            bool hasSecondAbility;
            bool hasTwoAbilities;

            if (tradeNewPVGenderComboBox.Items.Count > 1)
            {
                onlyOneGender = false;
                gender = (Gender)tradeNewPVGenderComboBox.SelectedIndex;
            }
            else
            {
                onlyOneGender = true;
                gender = Gender.MALE; //placeholder value
            }

            if (tradeNewPVAbilityComboBox.SelectedIndex == 1)
                hasSecondAbility = true;
            else
                hasSecondAbility = false;

            if (tradeNewPVAbilityComboBox.Items.Count > 1)
                hasTwoAbilities = true;
            else
                hasTwoAbilities = false;

            if (!tradeNewPVAbilityCheckBox.Checked && !tradeNewPVGenderCheckBox.Checked && !tradeNewPVNatureCheckBox.Checked && !onlyOneGender && hasTwoAbilities)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.SetTraits(species, gender, nature, hasSecondAbility);
            }
            else if ((tradeNewPVAbilityCheckBox.Checked || !hasTwoAbilities) && !tradeNewPVGenderCheckBox.Checked && !tradeNewPVNatureCheckBox.Checked && !onlyOneGender)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.SetTraits(species, gender, nature);
            }
            else if (!tradeNewPVAbilityCheckBox.Checked && hasTwoAbilities && !tradeNewPVGenderCheckBox.Checked && !onlyOneGender && tradeNewPVNatureCheckBox.Checked)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.SetTraits(species, gender, hasSecondAbility);
            }
            else if (!tradeNewPVAbilityCheckBox.Checked && hasTwoAbilities && (tradeNewPVGenderCheckBox.Checked || onlyOneGender) && !tradeNewPVNatureCheckBox.Checked)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.SetTraits(nature, hasSecondAbility);
            }
            else if ((tradeNewPVAbilityCheckBox.Checked || !hasTwoAbilities) && (tradeNewPVGenderCheckBox.Checked || onlyOneGender) && !tradeNewPVNatureCheckBox.Checked)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.SetTraits(nature);
            }
            else if ((tradeNewPVAbilityCheckBox.Checked || !hasTwoAbilities) && !tradeNewPVGenderCheckBox.Checked && !onlyOneGender && tradeNewPVNatureCheckBox.Checked)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.SetTraits(species, gender);
            }
            else if (!tradeNewPVAbilityCheckBox.Checked && hasTwoAbilities && (tradeNewPVGenderCheckBox.Checked || onlyOneGender) && tradeNewPVNatureCheckBox.Checked)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.SetTraits(hasSecondAbility);
            }
            else if ((tradeNewPVAbilityCheckBox.Checked || hasTwoAbilities) && (tradeNewPVGenderCheckBox.Checked || onlyOneGender) && tradeNewPVNatureCheckBox.Checked)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue = new PersonalityValue();
            }

                tradePVNumericNoArrows.Value = RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue.PV;
        }

        private void tradeLanguageComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].LanguageOfOrigin != (Languages)tradeLanguageComboBox.SelectedIndex)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].LanguageOfOrigin = (Languages)tradeLanguageComboBox.SelectedIndex;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradeHeldItemComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].HeldItem != tradeHeldItemComboBox.SelectedIndex)
            {
                RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].HeldItem = (ushort)tradeHeldItemComboBox.SelectedIndex;
                MarkUnsavedChanges(SaveSubFile.TRADES);
            }
        }

        private void tradePVNumericNoArrows_ValueChanged(object sender, EventArgs e)
        {
            updatePVDerivedFields();
        }

        public void updatePVDerivedFields()
        {
            PersonalityValue pv = RomFile.NPCTradesList[tradeTrainerComboBox.SelectedIndex].PersonalityValue;

            tradeGenderTextBox.Text = TextArchive.GetGenderName(pv.GetGender(RomFile.PokemonSpeciesList[tradeOfferedPokemonComboBox.SelectedIndex].GenderRatio));
            tradeNatureTextBox.Text = RomFile.NatureNames[(int)pv.GetNature()];
            tradeAbilityTextBox.Text = RomFile.GetAbilityName(tradeOfferedPokemonComboBox.SelectedIndex, pv);


            tradeNewPVAbilityComboBox.Items.Clear();
            tradeNewPVAbilityComboBox.Items.Add(RomFile.GetAbility1Name(tradeOfferedPokemonComboBox.SelectedIndex));

            if (RomFile.PokemonSpeciesList[tradeOfferedPokemonComboBox.SelectedIndex].GetHasSecondAbility())
            {
                tradeNewPVAbilityComboBox.Items.Add(RomFile.GetAbility2Name(tradeOfferedPokemonComboBox.SelectedIndex));
                tradeNewPVAbilityCheckBox.Enabled = true;
                tradeNewPVAbilityComboBox.SelectedIndex = pv.GetHasSecondAbility() ? PersonalityValue.ABILITY2 - 1 : PersonalityValue.ABILITY1 - 1;
            }
            else
            {
                tradeNewPVAbilityCheckBox.Enabled = false;
                tradeNewPVAbilityComboBox.SelectedIndex = 0;
            }

            tradeNewPVGenderComboBox.Items.Clear();
            if (RomFile.PokemonSpeciesList[tradeOfferedPokemonComboBox.SelectedIndex].GetIsMaleOnly())
            {
                tradeNewPVGenderComboBox.Items.Add(Gender.MALE.ToString());
                tradeNewPVGenderCheckBox.Enabled = false;
                tradeNewPVGenderComboBox.SelectedIndex = 0;
            }
            else if (RomFile.PokemonSpeciesList[tradeOfferedPokemonComboBox.SelectedIndex].GetIsFemaleOnly())
            {
                tradeNewPVGenderComboBox.Items.Add(Gender.FEMALE.ToString());
                tradeNewPVGenderCheckBox.Enabled = false;
                tradeNewPVGenderComboBox.SelectedIndex = 0;
            }
            else if (RomFile.PokemonSpeciesList[tradeOfferedPokemonComboBox.SelectedIndex].GetIsGenderless())
            {
                tradeNewPVGenderComboBox.Items.Add(Gender.UNKNOWN.ToString());
                tradeNewPVGenderCheckBox.Enabled = false;
                tradeNewPVGenderComboBox.SelectedIndex = 0;
            }
            else
            {
                tradeNewPVGenderComboBox.BeginUpdate();
                tradeNewPVGenderComboBox.Items.Add(Gender.MALE.ToString());
                tradeNewPVGenderComboBox.Items.Add(Gender.FEMALE.ToString());
                tradeNewPVGenderComboBox.EndUpdate();

                tradeNewPVGenderCheckBox.Enabled = true;
                tradeNewPVGenderComboBox.SelectedIndex = (int)pv.GetGender(RomFile.PokemonSpeciesList[tradeOfferedPokemonComboBox.SelectedIndex].GenderRatio);
            }

            tradeNewPVNatureComboBox.SelectedIndex = (int)pv.GetNature();
        }

    }
}
