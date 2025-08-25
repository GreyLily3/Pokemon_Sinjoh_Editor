using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pokemon_Sinjoh_Editor
{
	public partial class MainForm : Form
	{
		private const int MOVE_PP_MULTIPLE = 5;
		private const string SPECIES_TYPE_TOOLTIP_TEXT = "For monotype pokemon set type 1 & 2 to the same type";
		private const string SPECIES_ABILITY_TOOLTIP_TEXT = "For pokemon with only one ability you can either set ability 1 & 2 to the same ability or set ability 2 to '---'";
		private const string SPECIES_EGG_GROUP_TOOLTIP_TEXT = "Pokemon can only breed with other pokemon that share an egg group. For pokemon with a single egg group set egg groups 1 & 2 to be the same";
		private const string SPECIES_WILD_HELD_ITEM_TOOLTIP_TEXT = "The item the pokemon has a chance of holding when encountered in the wild";
		private const string SPECIES_EV_YIELD_TOOLTIP_TEXT = "What effort values the pokemon will give when defeated. Each pokemon can have a total of 510 EVs across all stats";

		private Color darkModeBackColor = Color.FromArgb(64, 64, 64);
        private Color darkModeTextColor = Color.FromArgb(255, 255, 255);
		private Color lightModeBackColor = SystemColors.Window;
        private Color lightModeTextColor = SystemColors.WindowText;

        public MainForm()
		{
			InitializeComponent();

			this.KeyPreview = true;

			mainTabControl.Enabled = false;

			moveEffectNumericNoArrows.Maximum = Pokemon_Sinjoh_Editor.Move.NUM_EFFECTS;

			movePowerTooltip.SetToolTip(movePowerNumericNoArrows, "The base damage of the move. Whether or not this field is used for an attack is determined by the move effect");            
			moveAccuracyTooltip.SetToolTip(moveAccuracyNumericNoArrows, "The chance of a move working on an enemy/enemies. Moves that target the User or User and Allies ignore this field");
			movePowerPointsTooltip.SetToolTip(movePPNumericNoArrows, "The base Power Points for a move. Can only be a mutiple of 5, otherwise it breaks PP ups and Max PPs");
			moveEffectTooltip.SetToolTip(moveEffectNumericNoArrows, "What the move actually does, including whether or not it calculates damage based on its power");
			moveEffectChanceTooltip.SetToolTip(moveEffectChanceNumericNoArrows, "The chance of the move applying a secondary effect based on the set effect, ie. a status condition, stat change or flinch when using a damaging move");
			movePriorityTooltip.SetToolTip(movePriorityNumericNoArrows, "The order the move will be used in compared to the opponent's move. NOTE: This field is only checked if certain effects are assigned");
			moveTargetTooltip.SetToolTip(moveTargetComboBox, "What pokemon in battle the move is used on. NOTE: Moves may cause effects to pokemon other than the target, ex. Swagger confusing the user while targetting another pokemon");
			
			moveContactTooltip.SetToolTip(moveContactCheckBox, "If the move counts as making contact for certain abilities and held items, ex. static, rough skin, the poison barb");
			moveProtectTooltip.SetToolTip(moveProtectCheckBox, "If the move is negated when its target is using protect");
			moveMagicCoatTooltip.SetToolTip(moveMagicCoatCheckBox, "If the move can be reflected back onto the user by a target using magic coat");
			moveSnatchTooltip.SetToolTip(moveSnatchCheckBox, "If the move can be stolen by another pokemon using snatch");
			moveMirrorMoveTooltip.SetToolTip(moveMirrorMoveCheckBox, "If the move can be copied by another pokemon using mirror move on the user");
			moveMirrorMoveTooltip.SetToolTip(moveKingsRockCheckBox, "If the flinch chance from holding the king's rock will applied when this move is used");
			moveHPBarTooltip.SetToolTip(moveHPBarCheckBox, "If both pokemons' HP bars are shown when the move's animation is playing");
			moveShadowTooltip.SetToolTip(moveShadowCheckBox, "If both pokemons' shadows are hidden when the move's animation is playing");

            speciesTypeTooltip.SetToolTip(speciesType1ComboBox, SPECIES_TYPE_TOOLTIP_TEXT);
            speciesTypeTooltip.SetToolTip(speciesType2ComboBox, SPECIES_TYPE_TOOLTIP_TEXT);
            speciesAbilityTooltip.SetToolTip(speciesAbility1ComboBox, SPECIES_ABILITY_TOOLTIP_TEXT);
            speciesAbilityTooltip.SetToolTip(speciesAbility2ComboBox, SPECIES_ABILITY_TOOLTIP_TEXT);
            speciesXPGroupTooltip.SetToolTip(speciesXPGroupComboBox, "Determines how much EXP a pokemon needs for each level. Different EXP groups require different amounts of EXP to reach lvl.100");
            speciesBaseXPTooltip.SetToolTip(speciesBaseXPYieldNumericNoArrows, "The amount of experience the pokemon will reward when defeated before taking into account modifiers to the amount like the Exp. share or lucky egg");
            speciesWildHeldItemTooltip.SetToolTip(speciesHeldItem1ComboBox, SPECIES_WILD_HELD_ITEM_TOOLTIP_TEXT);
            speciesWildHeldItemTooltip.SetToolTip(speciesHeldItem2ComboBox, SPECIES_WILD_HELD_ITEM_TOOLTIP_TEXT);
            speciesEggGroupTooltip.SetToolTip(speciesEggGroup1ComboBox, SPECIES_EGG_GROUP_TOOLTIP_TEXT);
            speciesEggGroupTooltip.SetToolTip(speciesEggGroup2ComboBox, SPECIES_EGG_GROUP_TOOLTIP_TEXT);
            speciesEggCyclesTooltip.SetToolTip(speciesEggCyclesNumericNoArrows, "Add 1 then multiply by 255 to get the number of steps needed to hatch this pokemon from an egg");
            speciesCatchRateTooltip.SetToolTip(speciesCatchRateNumericNoArrows, "Determines how difficult the pokemon is to catch, the higher it is the more likely catching will be. Ex. a pokemon with 255 catch will have a 33.3% chance to be caught with a pokeball at full health");
            speciesBaseFriendshipTooltip.SetToolTip(speciesBaseFriendshipNumericNoArrows, "How much friendship the pokemon will start with when caught or recieved from trading. Pokemon with friendship evolutions will evovle at 220");
            speciesSafariRunChanceTooltip.SetToolTip(speciesSafariRunChanceNumericNoArrows, "The base chance out of 254 a pokemon will run every turn when encountered in the safari zone/Great Marsh");
            speciesEVYieldTooltip.SetToolTip(speciesEVOnDefeatGroupBox, SPECIES_EV_YIELD_TOOLTIP_TEXT);

			tradeAbilityTooltip.AutomaticDelay = 500;
			tradeGenderTooltip.AutomaticDelay = 500;
			tradeNatureTooltip.AutomaticDelay = 500;

			INIManager.LoadINI();
			setLanguage();
        }

		private void setLanguage()
		{
			switch (INIManager.Language)
			{
				case Languages.JAPANESE:
					日本語ToolStripMenuItem.Checked = true;
                    setTextToJapanese();
                    break;
                case Languages.ENGLISH:
                    englishToolStripMenuItem.Checked = true;
                    setTextToEnglish();
                    break;
                case Languages.FRENCH:
                    englishToolStripMenuItem.Checked = true;
                    setTextToFrench();
                    break;
                case Languages.SPANISH:
                    españolToolStripMenuItem.Checked = true;
                    setTextToSpanish();
                    break;
                case Languages.ITALIAN:
                    italianoToolStripMenuItem.Checked = true;
                    setTextToItalian();
                    break;
                case Languages.GERMAN:
                    setTextToGerman();
                    deutschToolStripMenuItem.Checked = true;
                    break;
                case Languages.KOREAN:
                    setTextToKorean();
                    한국어ToolStripMenuItem.Checked = true;
                    break;

            }
		}

		private void setTextToEnglish()
		{
            fileToolStripMenuItem.Text = "File";
            optionsToolStripMenuItem.Text = "Options";
            languageToolStripMenuItem.Text = "Language";
            openRomFileToolStripMenuItem.Text = "Open Rom File";
            saveToolStripMenuItem.Text = "Save";
            quitToolStripMenuItem.Text = "Exit";

            movesTabPage.Text = "Moves";
            speciesTabPage.Text = "Pokémon";
            npcTradeTabPage.Text = "NPC Trades";

            moveSelectedLabel.Text = "Move:";
            movePowerLabel.Text = "Power:";
            moveAccuracyLabel.Text = "Accuracy:";
            movePPLabel.Text = "PP:";
            moveTypeLabel.Text = "Type:";
            moveCategoryLabel.Text = "Category:";
            moveEffectLabel.Text = "Effect:";
            moveEffectChanceLabel.Text = "Effect Chance:";
            movePriorityLabel.Text = "Priority:";
            moveTargetLabel.Text = "Target:";
			moveContestEffectLabel.Text = "Contest Effect:";
            moveContestConditionLabel.Text = "Contest Condition:";

            moveContactCheckBox.Text = "Makes Contact";
            moveProtectCheckBox.Text = "Affected by Protect";
            moveMagicCoatCheckBox.Text = "Affected by Magic Coat";
            moveSnatchCheckBox.Text = "Affected by Snatch";
            moveMirrorMoveCheckBox.Text = "Affected by Mirror Move";
            moveKingsRockCheckBox.Text = "Affected by King's Rock";
			moveHPBarCheckBox.Text = "Keep HP Bar";
			moveShadowCheckBox.Text = "Hide Shadow";

			speciesHPLabel.Text = "HP:";
            speciesAttackLabel.Text = "Attack:";
            speciesDefenseLabel.Text = "Defense:";
            speciesSpecialAttackLabel.Text = "Special Attack:";
            speciesSpecialDefenseLabel.Text = "Special Defense:";
            speciesSpeedLabel.Text = "Speed:";

			speciesHPEVlabel.Text = "HP:";
            speciesAttackEVLabel.Text = "Attack:";
            speciesDefenseEVLabel.Text = "Defense:";
            speciesSpecialAttackEVLabel.Text = "Special Attack:";
            speciesSpecialDefenseEVLabel.Text = "Special Defense:";
            speciesSpeedEVlabel.Text = "Speed:";

			speciesType1Label.Text = "Type 1:";
            speciesType2Label.Text = "Type 2:";
            speciesAbility1Label.Text = "Ability 1:";
            speciesAbility2Label.Text = "Ability 2:";
			speciesXPGroupLabel.Text = "Experience Group:";
            speciesBaseXPLabel.Text = "Base EXP Yield:";
            speciesHeldItem1Label.Text = "Wild Held Item 1:\r\n(50% chance)\r\n";
            speciesHeldItem2Label.Text = "Wild Held Item 2:\r\n(5% chance)\r\n";
            speciesEggGroup1Label.Text = "Egg Group 1:";
            speciesEggGroup2Label.Text = "Egg Group 2:";
            speciesEggCyclesLabel.Text = "Egg Cycles:";
			speciesMaleOnlyRadioButton.Text = "Male Only";
            speciesFemaleOnlyRadioButton.Text = "Female only";
            speciesGenderlessRadioButton.Text = "Genderless";
            speciesMaleAndFemaleRadioButton.Text = "Male && Female";
            speciesGenderMaleToFemaleLabel.Text = "Gender Ratio:\r\n(Male to Female)";
            speciesCatchRateLabel.Text = "Catch Rate:";
            speciesBaseHappinessLabel.Text = "Base Friendship:";
            speciesSafariRunChanceLabel.Text = "Run Chance:\r\n(Safari Zone)";
            speciesLearnableTMsLabel.Text = "Learnable TMs:";
            speciesLearnableHMsLabel.Text = "Learnable HMs:";

			speciesBaseStatsGroupBox.Text = "Base Stats";
            speciesEVOnDefeatGroupBox.Text = "Effort Values Awarded";
            speciesTypesGroupBox.Text = "Types";
            speciesAbilitiesGroupBox.Text = "Abilities";
            speciesXPGroupBox.Text = "EXP";
            speciesHeldItemsGroupBox.Text = "Held Items in the Wild";
            speciesEggGroupsGroupBox.Text = "Egg";
            speciesGenderGroupBox.Text = "Gender";
            speciesMiscGroupBox.Text = "Misc";

            tradeWantedPokemonLabel.Text = "Wanted Pokemon";
            tradeOfferedPokemonLabel.Text = "Offered Pokemon";
            tradeNicknameLabel.Text = "Offered Pokemon Nickname";
            tradeOriginalTrainerIDLabel.Text = "Original Trainer ID";

            tradeHPIVsLabel.Text = "HP";
            tradeAttackIVsLabel.Text = "Attack";
            tradeDefenseIVsLabel.Text = "Defense";
            tradeSpecialAttackIVsLabel.Text = "Special Attack";
            tradeSpecialDefenseIVsLabel.Text = "Special Defense";
            tradeSpeedIVsLabel.Text = "Speed";

            tradeCoolLabel.Text = "Cool";
            tradeBeautyLabel.Text = "Beauty";
            tradeCuteLabel.Text = "Cute";
            tradeSmartLabel.Text = "Smart";
            tradeToughLabel.Text = "Tough";
            tradeSheenLabel.Text = "Sheen";

            tradeHeldItemLabel.Text = "Held Item";
            tradeLanguageLabel.Text = "Language of Origin";
            tradePVLabel.Text = "Personality Value";
            tradeAbilityLabel.Text = "Ability";
            tradeGenderLabel.Text = "Gender";
            tradeNatureLabel.Text = "Nature";

            tradeIVsGroupBox.Text = "IVs";
            tradeContestStatsGroupBox.Text = "Contest Stats";
            tradePVDerivedGroupBox.Text = "Derived from PV";
        }

        private void setTextToSpanish()
        {
            fileToolStripMenuItem.Text = "Archivo";
            optionsToolStripMenuItem.Text = "Configuración";
            languageToolStripMenuItem.Text = "Idioma";
            openRomFileToolStripMenuItem.Text = "Abrir";
            saveToolStripMenuItem.Text = "Guardar";
            quitToolStripMenuItem.Text = "Salir";

            movesTabPage.Text = "Movimientos";
            speciesTabPage.Text = "Pokémon";
            npcTradeTabPage.Text = "Intercambios";

            moveSelectedLabel.Text = "movimiento:";
            movePowerLabel.Text = "Potencia:";
            moveAccuracyLabel.Text = "Precisión:";
            movePPLabel.Text = "PP:";
            moveTypeLabel.Text = "Tipo:";
            moveCategoryLabel.Text = "Clase:";
            moveEffectLabel.Text = "Efecto:";
            moveEffectChanceLabel.Text = "Efecto secundario %:";
            movePriorityLabel.Text = "Prioridad:";
            moveTargetLabel.Text = "Blanco:";
            moveContestEffectLabel.Text = "Contest Effect:";
            moveContestConditionLabel.Text = "Cualidad de Concurso:";

            moveContactCheckBox.Text = "Contacto";
            moveProtectCheckBox.Text = "Afectado por Protección";
            moveMagicCoatCheckBox.Text = "Afectado por Capa Mágica";
            moveSnatchCheckBox.Text = "Afectado por Robo";
            moveMirrorMoveCheckBox.Text = "Afectado por Espejo";
            moveKingsRockCheckBox.Text = "Afectado por Roca del rey";
            moveHPBarCheckBox.Text = "Keep HP Bar";
            moveShadowCheckBox.Text = "Hide Shadow";

            speciesHPLabel.Text = "PS:";
            speciesAttackLabel.Text = "Ataque:";
            speciesDefenseLabel.Text = "Defensa:";
            speciesSpecialAttackLabel.Text = "Ataque Especial:";
            speciesSpecialDefenseLabel.Text = "Defensa Especial:";
            speciesSpeedLabel.Text = "Velocidad:";

            speciesHPEVlabel.Text = "PS:";
            speciesAttackEVLabel.Text = "Ataque:";
            speciesDefenseEVLabel.Text = "Defensa:";
            speciesSpecialAttackEVLabel.Text = "Ataque Especial:";
            speciesSpecialDefenseEVLabel.Text = "Defensa Especial:";
            speciesSpeedEVlabel.Text = "Velocidad:";

            speciesType1Label.Text = "Tipo 1:";
            speciesType2Label.Text = "Tipo 2:";
            speciesAbility1Label.Text = "Habilidad 1:";
            speciesAbility2Label.Text = "Habilidad 2:";
            speciesXPGroupLabel.Text = "Tipo de Crecimiento:";
            speciesBaseXPLabel.Text = "Base EXP Yield:";
            speciesHeldItem1Label.Text = "Objetos Equipados 1:\r\n(50%)\r\n";
            speciesHeldItem2Label.Text = "Objetos Equipados 2:\r\n(5%)\r\n";
            speciesEggGroup1Label.Text = "Grupo de huevo 1:";
            speciesEggGroup2Label.Text = "Grupo de huevo 2:";
            speciesEggCyclesLabel.Text = "Ciclos de Huevos:";
            speciesMaleOnlyRadioButton.Text = "Macho";
            speciesFemaleOnlyRadioButton.Text = "Hembra";
            speciesGenderlessRadioButton.Text = "Sin Sexo";
            speciesMaleAndFemaleRadioButton.Text = "Masculino con Femenino";
            speciesGenderMaleToFemaleLabel.Text = "Proporción de Sexos:\r\n(Masculino / Femenino)";
            speciesCatchRateLabel.Text = "Ratio de captura:";
            speciesBaseHappinessLabel.Text = "Amistad base:";
            speciesSafariRunChanceLabel.Text = "Run Chance:\r\n(Zona Safari)";
            speciesLearnableTMsLabel.Text = "MT:";
            speciesLearnableHMsLabel.Text = "MO:";

            speciesBaseStatsGroupBox.Text = "Características";
            speciesEVOnDefeatGroupBox.Text = "Effort Values Awarded";
            speciesTypesGroupBox.Text = "Tipos";
            speciesAbilitiesGroupBox.Text = "Habilidades";
            speciesXPGroupBox.Text = "Experiencia";
            speciesHeldItemsGroupBox.Text = "Objetos equipados en estado salvaje";
            speciesEggGroupsGroupBox.Text = "Huevo";
            speciesGenderGroupBox.Text = "Sexo";
            speciesMiscGroupBox.Text = "Misc";

            tradeWantedPokemonLabel.Text = "Pokémon Entregado";
            tradeOfferedPokemonLabel.Text = "Pokémon Recibido";
            tradeNicknameLabel.Text = "Mote";
            tradeOriginalTrainerIDLabel.Text = "ID de Entrenador";

            tradeHPIVsLabel.Text = "PS";
            tradeAttackIVsLabel.Text = "Ataque";
            tradeDefenseIVsLabel.Text = "Defensa";
            tradeSpecialAttackIVsLabel.Text = "Ataque Especial";
            tradeSpecialDefenseIVsLabel.Text = "Defensa Especial";
            tradeSpeedIVsLabel.Text = "Velocidad";

            tradeCoolLabel.Text = "Carisma";
            tradeBeautyLabel.Text = "Belleza";
            tradeCuteLabel.Text = "Dulzura";
            tradeSmartLabel.Text = "Ingenio";
            tradeToughLabel.Text = "Dureza";
            tradeSheenLabel.Text = "Brillo";

            tradeHeldItemLabel.Text = "Objeto Equipado";
            tradeLanguageLabel.Text = "Language of Origin";
            tradePVLabel.Text = "Valor de personalidad";
            tradeAbilityLabel.Text = "Habilidad";
            tradeGenderLabel.Text = "Sexo";
            tradeNatureLabel.Text = "Naturaleza";

            tradeIVsGroupBox.Text = "Genética";
            tradeContestStatsGroupBox.Text = "Contest Stats";
            tradePVDerivedGroupBox.Text = "Derived from PV";
        }

        private void setTextToFrench()
        {
            fileToolStripMenuItem.Text = "Fichier";
            optionsToolStripMenuItem.Text = "Paramètres";
            languageToolStripMenuItem.Text = "Langue";
            openRomFileToolStripMenuItem.Text = "Ouvrir";
            saveToolStripMenuItem.Text = "Enregistrer";
            quitToolStripMenuItem.Text = "Quitter";

            movesTabPage.Text = "Capacités";
            speciesTabPage.Text = "Pokémon";
            npcTradeTabPage.Text = "Échange Interne";

            moveSelectedLabel.Text = "Capacité:";
            movePowerLabel.Text = "Puissance:";
            moveAccuracyLabel.Text = "Précision:";
            movePPLabel.Text = "PP:";
            moveTypeLabel.Text = "Type:";
            moveCategoryLabel.Text = "Catégorie:";
            moveEffectLabel.Text = "Effet:";
            moveEffectChanceLabel.Text = "Effect Chance:";
            movePriorityLabel.Text = "Priorité:";
            moveTargetLabel.Text = "Cible:";
            moveContestEffectLabel.Text = "Contest Effect:";
            moveContestConditionLabel.Text = "Condition:";

            moveContactCheckBox.Text = "Contact";
            moveProtectCheckBox.Text = "Affecté par Abri";
            moveMagicCoatCheckBox.Text = "Affecté par Reflet Magik";
            moveSnatchCheckBox.Text = "Affecté par Snatch";
            moveMirrorMoveCheckBox.Text = "Affecté par Mimique";
            moveKingsRockCheckBox.Text = "Affecté par Roche Royale";
            moveHPBarCheckBox.Text = "Keep HP Bar";
            moveShadowCheckBox.Text = "Hide Shadow";

            speciesHPLabel.Text = "PV:";
            speciesAttackLabel.Text = "Attaque:";
            speciesDefenseLabel.Text = "Défense:";
            speciesSpecialAttackLabel.Text = "Attaque Spéciale:";
            speciesSpecialDefenseLabel.Text = "Défense Spéciale:";
            speciesSpeedLabel.Text = "Vitesse:";

            speciesHPEVlabel.Text = "PV:";
            speciesAttackEVLabel.Text = "Attaque:";
            speciesDefenseEVLabel.Text = "Défense:";
            speciesSpecialAttackEVLabel.Text = "Attaque Spéciale:";
            speciesSpecialDefenseEVLabel.Text = "Défense Spéciale:";
            speciesSpeedEVlabel.Text = "Vitesse:";

            speciesType1Label.Text = "Type 1:";
            speciesType2Label.Text = "Type 2:";
            speciesAbility1Label.Text = "Talent 1:";
            speciesAbility2Label.Text = "Talent 2:";
            speciesXPGroupLabel.Text = "Courbe d'expérience:";
            speciesBaseXPLabel.Text = "Points exp:";
            speciesHeldItem1Label.Text = "Objet Tenu 1:\r\n(50%)\r\n";
            speciesHeldItem2Label.Text = "Objet Tenu 2:\r\n(5%)\r\n";
            speciesEggGroup1Label.Text = "Groupes d'Œuf 1:";
            speciesEggGroup2Label.Text = "Groupes d'Œuf 2:";
            speciesEggCyclesLabel.Text = "Éclosion:";
            speciesMaleOnlyRadioButton.Text = "Mâle";
            speciesFemaleOnlyRadioButton.Text = "Femelle ";
            speciesGenderlessRadioButton.Text = "Genderless";
            speciesMaleAndFemaleRadioButton.Text = "Mâle ou Femelle";
            speciesGenderMaleToFemaleLabel.Text = "Mâle / Femelle";
            speciesCatchRateLabel.Text = "Taux de Capture:";
            speciesBaseHappinessLabel.Text = "Bonheur:";
            speciesSafariRunChanceLabel.Text = "Run Chance:\r\n(Parc Safari)";
            speciesLearnableTMsLabel.Text = "CT:";
            speciesLearnableHMsLabel.Text = "CS:";

            speciesBaseStatsGroupBox.Text = "Statistique de Base";
            speciesEVOnDefeatGroupBox.Text = "Points effort";
            speciesTypesGroupBox.Text = "Types";
            speciesAbilitiesGroupBox.Text = "Talents";
            speciesXPGroupBox.Text = "Expérience";
            speciesHeldItemsGroupBox.Text = "Objet Tenu";
            speciesEggGroupsGroupBox.Text = "œuf";
            speciesGenderGroupBox.Text = "Sexe";
            speciesMiscGroupBox.Text = "Misc";

            tradeWantedPokemonLabel.Text = "Pokémon Demandé";
            tradeOfferedPokemonLabel.Text = "Pokémon Obtenu";
            tradeNicknameLabel.Text = "Surnom";
            tradeOriginalTrainerIDLabel.Text = "N° ID";

            tradeHPIVsLabel.Text = "PV";
            tradeAttackIVsLabel.Text = "Attaque";
            tradeDefenseIVsLabel.Text = "Défense";
            tradeSpecialAttackIVsLabel.Text = "Attaque Spéciale";
            tradeSpecialDefenseIVsLabel.Text = "Défense Spéciale";
            tradeSpeedIVsLabel.Text = "Vitesse";

            tradeCoolLabel.Text = "Sang-froid";
            tradeBeautyLabel.Text = "Beauté";
            tradeCuteLabel.Text = "Grâce";
            tradeSmartLabel.Text = "Intelligence";
            tradeToughLabel.Text = "Robustesse";
            tradeSheenLabel.Text = "Lustre";

            tradeHeldItemLabel.Text = "Objet Tenu";
            tradeLanguageLabel.Text = "Langue";
            tradePVLabel.Text = "Valeur Interne Personnelle";
            tradeAbilityLabel.Text = "Talent";
            tradeGenderLabel.Text = "Sexe";
            tradeNatureLabel.Text = "Nature";

            tradeIVsGroupBox.Text = "IVs";
            tradeContestStatsGroupBox.Text = "Contest Stats";
            tradePVDerivedGroupBox.Text = "Derived from PV";
        }

        private void setTextToGerman()
        {
            fileToolStripMenuItem.Text = "Datei";
            optionsToolStripMenuItem.Text = "Einstellung";
            languageToolStripMenuItem.Text = "Sprache";
            openRomFileToolStripMenuItem.Text = "öffnen";
            saveToolStripMenuItem.Text = "Speichern";
            quitToolStripMenuItem.Text = "Beenden";

            movesTabPage.Text = "Attacke";
            speciesTabPage.Text = "Pokémon";
            npcTradeTabPage.Text = "Tausch";

            moveSelectedLabel.Text = "Attacke:";
            movePowerLabel.Text = "Stärke:";
            moveAccuracyLabel.Text = "Genauigkeit:";
            movePPLabel.Text = "AP:";
            moveTypeLabel.Text = "Typ:";
            moveCategoryLabel.Text = "Kategorie:";
            moveEffectLabel.Text = "Effekt:";
            moveEffectChanceLabel.Text = "Effect Chance:";
            movePriorityLabel.Text = "Priorität:";
            moveTargetLabel.Text = "Zielerfassung:";
            moveContestEffectLabel.Text = "Contest Effect:";
            moveContestConditionLabel.Text = "Eigenschaft:";

            moveContactCheckBox.Text = "Kontakt";
            moveProtectCheckBox.Text = "Schutzschild";
            moveMagicCoatCheckBox.Text = "Magiemantel";
            moveSnatchCheckBox.Text = "Übernahme";
            moveMirrorMoveCheckBox.Text = "Spiegeltrick";
            moveKingsRockCheckBox.Text = "King-Stein";
            moveHPBarCheckBox.Text = "Keep HP Bar";
            moveShadowCheckBox.Text = "Hide Shadow";

            speciesHPLabel.Text = "KP:";
            speciesAttackLabel.Text = "Angriff:";
            speciesDefenseLabel.Text = "Verteidigung:";
            speciesSpecialAttackLabel.Text = "Spezial-Angriff:";
            speciesSpecialDefenseLabel.Text = "Spezial-Verteidigung:";
            speciesSpeedLabel.Text = "Initiative:";

            speciesHPEVlabel.Text = "KP:";
            speciesAttackEVLabel.Text = "Angriff:";
            speciesDefenseEVLabel.Text = "Verteidigung:";
            speciesSpecialAttackEVLabel.Text = "Spezial-Angriff:";
            speciesSpecialDefenseEVLabel.Text = "Spezial-Verteidigung:";
            speciesSpeedEVlabel.Text = "Initiative:";

            speciesType1Label.Text = "Typ 1:";
            speciesType2Label.Text = "Typ 2:";
            speciesAbility1Label.Text = "Fähigkeit 1:";
            speciesAbility2Label.Text = "Fähigkeit 2:";
            speciesXPGroupLabel.Text = "EP-Kategorie:";
            speciesBaseXPLabel.Text = "Basis-EP:";
            speciesHeldItem1Label.Text = "Getragene Items 1:\r\n(50%)\r\n";
            speciesHeldItem2Label.Text = "Getragene Items 2:\r\n(5%)\r\n";
            speciesEggGroup1Label.Text = "Ei-Gruppen 1:";
            speciesEggGroup2Label.Text = "Ei-Gruppen 2:";
            speciesEggCyclesLabel.Text = "Ei-Zyklen:";
            speciesMaleOnlyRadioButton.Text = "Männlich";
            speciesFemaleOnlyRadioButton.Text = "Weiblich";
            speciesGenderlessRadioButton.Text = "Genderless";
            speciesMaleAndFemaleRadioButton.Text = "Männlich && Weiblich";
            speciesGenderMaleToFemaleLabel.Text = "Männlich / Weiblich";
            speciesCatchRateLabel.Text = "Fangrate:";
            speciesBaseHappinessLabel.Text = "Start-Freundschaft:";
            speciesSafariRunChanceLabel.Text = "Run Chance:\r\n(Safari-Zone)";
            speciesLearnableTMsLabel.Text = "TM:";
            speciesLearnableHMsLabel.Text = "VM:";

            speciesBaseStatsGroupBox.Text = "Statuswerte";
            speciesEVOnDefeatGroupBox.Text = "Basispunkte";
            speciesTypesGroupBox.Text = "Typ";
            speciesAbilitiesGroupBox.Text = "Fähigkeiten";
            speciesXPGroupBox.Text = "EP";
            speciesHeldItemsGroupBox.Text = "Getragene Items";
            speciesEggGroupsGroupBox.Text = "Ei";
            speciesGenderGroupBox.Text = "Geschlecht";
            speciesMiscGroupBox.Text = "Misc";

            tradeWantedPokemonLabel.Text = "Spieler-Pokémon";
            tradeOfferedPokemonLabel.Text = "Tausch-Pokémon";
            tradeNicknameLabel.Text = "Spitzname";
            tradeOriginalTrainerIDLabel.Text = "ID-Nummer";

            tradeHPIVsLabel.Text = "KP";
            tradeAttackIVsLabel.Text = "Angriff";
            tradeDefenseIVsLabel.Text = "Verteidigung";
            tradeSpecialAttackIVsLabel.Text = "Spezial-Angriff";
            tradeSpecialDefenseIVsLabel.Text = "Spezial-Verteidigung";
            tradeSpeedIVsLabel.Text = "Initiative";

            tradeCoolLabel.Text = "Coole";
            tradeBeautyLabel.Text = "Schönheit";
            tradeCuteLabel.Text = "Anmut";
            tradeSmartLabel.Text = "Klugheit";
            tradeToughLabel.Text = "Stärke";
            tradeSheenLabel.Text = "Glanz";

            tradeHeldItemLabel.Text = "Getragenes Item";
            tradeLanguageLabel.Text = "Sprache";
            tradePVLabel.Text = "Personality Value";
            tradeAbilityLabel.Text = "Fähigkeit";
            tradeGenderLabel.Text = "Geschlecht";
            tradeNatureLabel.Text = "Wesen";

            tradeIVsGroupBox.Text = "IS";
            tradeContestStatsGroupBox.Text = "Contest Stats";
            tradePVDerivedGroupBox.Text = "Derived from PV";
        }

        private void setTextToItalian()
        {
            fileToolStripMenuItem.Text = "File";
            optionsToolStripMenuItem.Text = "Configurazione";
            languageToolStripMenuItem.Text = "Linguaggio";
            openRomFileToolStripMenuItem.Text = "Apri";
            saveToolStripMenuItem.Text = "Salva";
            quitToolStripMenuItem.Text = "Esci";

            movesTabPage.Text = "Mossa";
            speciesTabPage.Text = "Pokémon";
            npcTradeTabPage.Text = "Scambi in-game";

            moveSelectedLabel.Text = "Mossa:";
            movePowerLabel.Text = "Potenza:";
            moveAccuracyLabel.Text = "Precisione:";
            movePPLabel.Text = "PP:";
            moveTypeLabel.Text = "Tipo:";
            moveCategoryLabel.Text = "Categoria:";
            moveEffectLabel.Text = "Effetto:";
            moveEffectChanceLabel.Text = "Effetti in lotta:";
            movePriorityLabel.Text = "Priorità:";
            moveTargetLabel.Text = "Raggio:";
            moveContestEffectLabel.Text = "Contest Effect:";
            moveContestConditionLabel.Text = "Virtù:";

            moveContactCheckBox.Text = "Contatto";
            moveProtectCheckBox.Text = "Bloccata da Protezione";
            moveMagicCoatCheckBox.Text = "Riflessa da Magivelo";
            moveSnatchCheckBox.Text = "Può essere rubata da Scippo";
            moveMirrorMoveCheckBox.Text = "Può essere copiata da Speculmossa";
            moveKingsRockCheckBox.Text = "Attiva Roccia di re";
            moveHPBarCheckBox.Text = "Keep HP Bar";
            moveShadowCheckBox.Text = "Hide Shadow";

            speciesHPLabel.Text = "PS:";
            speciesAttackLabel.Text = "Attacco:";
            speciesDefenseLabel.Text = "Difesa:";
            speciesSpecialAttackLabel.Text = "Attacco Speciale:";
            speciesSpecialDefenseLabel.Text = "Difesa Speciale:";
            speciesSpeedLabel.Text = "Velocità:";

            speciesHPEVlabel.Text = "PS:";
            speciesAttackEVLabel.Text = "Attacco:";
            speciesDefenseEVLabel.Text = "Difesa:";
            speciesSpecialAttackEVLabel.Text = "Attacco Speciale:";
            speciesSpecialDefenseEVLabel.Text = "Difesa Speciale:";
            speciesSpeedEVlabel.Text = "Velocità:";

            speciesType1Label.Text = "Tipo 1:";
            speciesType2Label.Text = "Tipo 2:";
            speciesAbility1Label.Text = "Abilità 1:";
            speciesAbility2Label.Text = "Abilità 2:";
            speciesXPGroupLabel.Text = "Tasso di allevamento:";
            speciesBaseXPLabel.Text = "Esperienza base ceduta:";
            speciesHeldItem1Label.Text = "Strumenti tenuti 1:\r\n(50%)\r\n";
            speciesHeldItem2Label.Text = "Strumenti tenuti 2:\r\n(5%)\r\n";
            speciesEggGroup1Label.Text = "Gruppo Uova 1:";
            speciesEggGroup2Label.Text = "Gruppo Uova 2:";
            speciesEggCyclesLabel.Text = "Cicli Uovo:";
            speciesMaleOnlyRadioButton.Text = "Maschio";
            speciesFemaleOnlyRadioButton.Text = "Femmina";
            speciesGenderlessRadioButton.Text = "Genere Sconosciuto";
            speciesMaleAndFemaleRadioButton.Text = "Maschio e Femmina";
            speciesGenderMaleToFemaleLabel.Text = "Maschio / Femmina";
            speciesCatchRateLabel.Text = "Tasso di cattura:";
            speciesBaseHappinessLabel.Text = "Affetto di base:";
            speciesSafariRunChanceLabel.Text = "Run Chance:\r\n(Zona Safari)";
            speciesLearnableTMsLabel.Text = "MT:";
            speciesLearnableHMsLabel.Text = "MN:";

            speciesBaseStatsGroupBox.Text = "Statistiche";
            speciesEVOnDefeatGroupBox.Text = "Punti base ceduti";
            speciesTypesGroupBox.Text = "Tipo";
            speciesAbilitiesGroupBox.Text = "Abilità";
            speciesXPGroupBox.Text = "PE";
            speciesHeldItemsGroupBox.Text = "Strumenti tenuti";
            speciesEggGroupsGroupBox.Text = "Uovo";
            speciesGenderGroupBox.Text = "Sesso";
            speciesMiscGroupBox.Text = "Misc";

            tradeWantedPokemonLabel.Text = "Pokémon da Offrire";
            tradeOfferedPokemonLabel.Text = "Pokémon Ricevuto";
            tradeNicknameLabel.Text = "Soprannome";
            tradeOriginalTrainerIDLabel.Text = "Numero ID Allenatore";

            tradeHPIVsLabel.Text = "PS";
            tradeAttackIVsLabel.Text = "Attacco";
            tradeDefenseIVsLabel.Text = "Difesa";
            tradeSpecialAttackIVsLabel.Text = "Attacco Speciale";
            tradeSpecialDefenseIVsLabel.Text = "Difesa Speciale";
            tradeSpeedIVsLabel.Text = "Velocità";

            tradeCoolLabel.Text = "Classe";
            tradeBeautyLabel.Text = "Bellezza";
            tradeCuteLabel.Text = "Grazia";
            tradeSmartLabel.Text = "Acume";
            tradeToughLabel.Text = "Grinta";
            tradeSheenLabel.Text = "Lustro";

            tradeHeldItemLabel.Text = "Strumento";
            tradeLanguageLabel.Text = "Lingua";
            tradePVLabel.Text = "Personalità";
            tradeAbilityLabel.Text = "Abilità";
            tradeGenderLabel.Text = "Sesso";
            tradeNatureLabel.Text = "Natura";

            tradeIVsGroupBox.Text = "IV";
            tradeContestStatsGroupBox.Text = "Contest Stats";
            tradePVDerivedGroupBox.Text = "Derived from PV";
        }

        private void setTextToJapanese()
		{
            fileToolStripMenuItem.Text = "ファイル";
            optionsToolStripMenuItem.Text = "設定";
            languageToolStripMenuItem.Text = "言語";
            openRomFileToolStripMenuItem.Text = "開く";
            saveToolStripMenuItem.Text = "セーブ";
            quitToolStripMenuItem.Text = "終了";

            movesTabPage.Text = "わざ";
            speciesTabPage.Text = "ポケモン";
            npcTradeTabPage.Text = "交換";

            moveSelectedLabel.Text = "わざ";
			movePowerLabel.Text = "威力";
			moveAccuracyLabel.Text = "命中率";
			movePPLabel.Text = "PP";
			moveTypeLabel.Text = "タイプ";
			moveCategoryLabel.Text = "分類";
			moveEffectLabel.Text = "効果";
			moveEffectChanceLabel.Text = "確率";
            movePriorityLabel.Text = "優先度";
			moveTargetLabel.Text = "範囲";
			moveContestConditionLabel.Text = "コンディション";

			moveContactCheckBox.Text = "接触";
			moveProtectCheckBox.Text = "まもる";
			moveMagicCoatCheckBox.Text = "マジックコート";
			moveSnatchCheckBox.Text = "よこどり";
			moveMirrorMoveCheckBox.Text = "オウムがえし";
            moveKingsRockCheckBox.Text = "おうじゃのしるし";

            speciesHPLabel.Text = "HP";
            speciesAttackLabel.Text = "攻撃能力";
            speciesDefenseLabel.Text = "防御能力";
            speciesSpecialAttackLabel.Text = "特攻能力";
            speciesSpecialDefenseLabel.Text = "特防能力";
            speciesSpeedLabel.Text = "素早さ能力";

            speciesHPEVlabel.Text = "HP";
            speciesAttackEVLabel.Text = "攻撃能力";
            speciesDefenseEVLabel.Text = "防御能力";
            speciesSpecialAttackEVLabel.Text = "特攻能力";
            speciesSpecialDefenseEVLabel.Text = "特防能力";
            speciesSpeedEVlabel.Text = "素早さ能力";

            speciesType1Label.Text = "タイプ 1";
            speciesType2Label.Text = "タイプ 2";
            speciesAbility1Label.Text = "特性 1";
            speciesAbility2Label.Text = "特性 2";
            speciesXPGroupLabel.Text = "経験値タイプ";
            speciesBaseXPLabel.Text = "基礎経験値";
            speciesHeldItem1Label.Text = "持ち物 1:\r\n(50%)\r\n";
            speciesHeldItem2Label.Text = "持ち物 2:\r\n(5%)\r\n";
            speciesEggGroup1Label.Text = "タマゴグループ 1";
            speciesEggGroup2Label.Text = "タマゴグループ 2";
            speciesEggCyclesLabel.Text = "タマゴのサイクル";
            speciesMaleOnlyRadioButton.Text = "オス";
            speciesFemaleOnlyRadioButton.Text = "メス";
            speciesGenderlessRadioButton.Text = "性別不明";
            speciesMaleAndFemaleRadioButton.Text = "オスかメス";
            speciesGenderMaleToFemaleLabel.Text = "男女比:\r\n(オス / メス)";
            speciesCatchRateLabel.Text = "捕捉率";
            speciesBaseHappinessLabel.Text = "なつき";
            speciesSafariRunChanceLabel.Text = "逃げる %:\r\n(サファリゾーン)";
            speciesLearnableTMsLabel.Text = "わざマシン";
            speciesLearnableHMsLabel.Text = "ひでんマシン";

            speciesBaseStatsGroupBox.Text = "種族値";
            speciesEVOnDefeatGroupBox.Text = "獲得努力値";
            speciesTypesGroupBox.Text = "タイプ";
            speciesAbilitiesGroupBox.Text = "特性";
            speciesXPGroupBox.Text = "経験";
            speciesHeldItemsGroupBox.Text = "持ち物";
            speciesEggGroupsGroupBox.Text = "たまご";
            speciesGenderGroupBox.Text = "性別";
            speciesMiscGroupBox.Text = "Misc";

            tradeWantedPokemonLabel.Text = "出すポケモン";
            tradeOfferedPokemonLabel.Text = "貰うポケモン";
            tradeNicknameLabel.Text = "ニックネーム";
            tradeOriginalTrainerIDLabel.Text = "IDNo.";

            tradeHPIVsLabel.Text = "HP";
            tradeAttackIVsLabel.Text = "攻撃能力";
            tradeDefenseIVsLabel.Text = "防御能力";
            tradeSpecialAttackIVsLabel.Text = "特攻能力";
            tradeSpecialDefenseIVsLabel.Text = "特防能力";
            tradeSpeedIVsLabel.Text = "素早さ能力";

            tradeCoolLabel.Text = "かっこよさ";
            tradeBeautyLabel.Text = "うつくしさ";
            tradeCuteLabel.Text = "かわいさ";
            tradeSmartLabel.Text = " かしこさ";
            tradeToughLabel.Text = "たくましさ ";
            tradeSheenLabel.Text = "けづや";

            tradeHeldItemLabel.Text = "持ち物";
            tradeLanguageLabel.Text = "言語";
            tradePVLabel.Text = "性格値";
            tradeAbilityLabel.Text = "特性";
            tradeGenderLabel.Text = "性別";
            tradeNatureLabel.Text = "性格";

            tradeIVsGroupBox.Text = "個体値";
            tradeContestStatsGroupBox.Text = "コンテスト";
            tradePVDerivedGroupBox.Text = "Derived from PV";
        }

        private void setTextToKorean()
        {
            fileToolStripMenuItem.Text = "파일";
            optionsToolStripMenuItem.Text = "설정";
            languageToolStripMenuItem.Text = "언어";
            openRomFileToolStripMenuItem.Text = "열기";
            saveToolStripMenuItem.Text = "저장";
            quitToolStripMenuItem.Text = "종료";

            movesTabPage.Text = "기술";
            speciesTabPage.Text = "포켓몬";
            npcTradeTabPage.Text = "교환";

            moveSelectedLabel.Text = "기술:";
            movePowerLabel.Text = "위력:";
            moveAccuracyLabel.Text = "명중:";
            movePPLabel.Text = "PP:";
            moveTypeLabel.Text = "타입:";
            moveCategoryLabel.Text = "분류:";
            moveEffectLabel.Text = "효과:";
            moveEffectChanceLabel.Text = "Effect Chance:";
            movePriorityLabel.Text = "선제기술:";
            moveTargetLabel.Text = "범위:";
            moveContestEffectLabel.Text = "Contest Effect:";
            moveContestConditionLabel.Text = "컨디션:";

            moveContactCheckBox.Text = "접촉";
            moveProtectCheckBox.Text = "방어";
            moveMagicCoatCheckBox.Text = "매직코트";
            moveSnatchCheckBox.Text = "가로챔";
            moveMirrorMoveCheckBox.Text = "따라하기";
            moveKingsRockCheckBox.Text = "왕의징표석";
            moveHPBarCheckBox.Text = "Keep HP Bar";
            moveShadowCheckBox.Text = "Hide Shadow";

            speciesHPLabel.Text = "HP:";
            speciesAttackLabel.Text = "공격:";
            speciesDefenseLabel.Text = "방어:";
            speciesSpecialAttackLabel.Text = "특수공격:";
            speciesSpecialDefenseLabel.Text = "특수방어:";
            speciesSpeedLabel.Text = "스피드:";

            speciesHPEVlabel.Text = "HP:";
            speciesAttackEVLabel.Text = "공격:";
            speciesDefenseEVLabel.Text = "방어:";
            speciesSpecialAttackEVLabel.Text = "특수공격:";
            speciesSpecialDefenseEVLabel.Text = "특수방어:";
            speciesSpeedEVlabel.Text = "스피드:";

            speciesType1Label.Text = "타입 1:";
            speciesType2Label.Text = "타입 2:";
            speciesAbility1Label.Text = "특성 1:";
            speciesAbility2Label.Text = "특성 2:";
            speciesXPGroupLabel.Text = "Experience Group:";
            speciesBaseXPLabel.Text = "Base EXP Yield:";
            speciesHeldItem1Label.Text = "지닌물건 1:\r\n(50%)\r\n";
            speciesHeldItem2Label.Text = "지닌물건 2:\r\n(5%)\r\n";
            speciesEggGroup1Label.Text = "알그룹 1:";
            speciesEggGroup2Label.Text = "알그룹 2:";
            speciesEggCyclesLabel.Text = "Egg Cycles:";
            speciesMaleOnlyRadioButton.Text = "수컷";
            speciesFemaleOnlyRadioButton.Text = "암컷";
            speciesGenderlessRadioButton.Text = "성별 불명";
            speciesMaleAndFemaleRadioButton.Text = "수컷 && 암컷";
            speciesGenderMaleToFemaleLabel.Text = "수컷 / 암컷)";
            speciesCatchRateLabel.Text = "포획률:";
            speciesBaseHappinessLabel.Text = "친밀도:";
            speciesSafariRunChanceLabel.Text = "Run Chance:\r\n(사파리존)";
            speciesLearnableTMsLabel.Text = "기술머신:";
            speciesLearnableHMsLabel.Text = "비전머신:";

            speciesBaseStatsGroupBox.Text = "종족값";
            speciesEVOnDefeatGroupBox.Text = "기초포인트";
            speciesTypesGroupBox.Text = "타입";
            speciesAbilitiesGroupBox.Text = "특성";
            speciesXPGroupBox.Text = "경험";
            speciesHeldItemsGroupBox.Text = "Held Items in the Wild";
            speciesEggGroupsGroupBox.Text = "알";
            speciesGenderGroupBox.Text = "성별";
            speciesMiscGroupBox.Text = "Misc";

            tradeWantedPokemonLabel.Text = "Wanted Pokemon";
            tradeOfferedPokemonLabel.Text = "Offered Pokemon";
            tradeNicknameLabel.Text = "Offered Pokemon Nickname";
            tradeOriginalTrainerIDLabel.Text = "IDNo.";

            tradeHPIVsLabel.Text = "HP";
            tradeAttackIVsLabel.Text = "공격";
            tradeDefenseIVsLabel.Text = "방어";
            tradeSpecialAttackIVsLabel.Text = "특수공격";
            tradeSpecialDefenseIVsLabel.Text = "특수방어";
            tradeSpeedIVsLabel.Text = "스피드";

            tradeCoolLabel.Text = "근사함";
            tradeBeautyLabel.Text = "아름다움";
            tradeCuteLabel.Text = "귀여움";
            tradeSmartLabel.Text = "슬기로움";
            tradeToughLabel.Text = "강인함";
            tradeSheenLabel.Text = "털색";

            tradeHeldItemLabel.Text = "지닌물건";
            tradeLanguageLabel.Text = "언어";
            tradePVLabel.Text = "Personality Value";
            tradeAbilityLabel.Text = "특성";
            tradeGenderLabel.Text = "성별";
            tradeNatureLabel.Text = "성격";

            tradeIVsGroupBox.Text = "개체값";
            tradeContestStatsGroupBox.Text = "Contest Stats";
            tradePVDerivedGroupBox.Text = "Derived from PV";
        }

        private void IncludeGameVersionInText(string romName)
		{
			Text = "Pokemon Sinjoh Editor - " + romName;
		}

		private void MarkUnsavedChanges()
		{
			if (!Text.Contains("*"))
				Text += '*';
			RomFile.AreUnsavedChanges = true;
		}

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
			save();
        }

        private void openRomFileToolStripMenuItem_Click(object sender, EventArgs e)
		{	
			DialogResult saveChanges;
			if (RomFile.IsValidGameVersion() && RomFile.AreUnsavedChanges)
			{
				if (RomFile.AreUnsavedChanges)
				{
					saveChanges = MessageBox.Show("There are unsaved changes to the selected ROM, do you want to save them before opening a new ROM?", "Save Changes before opening new ROM?", MessageBoxButtons.YesNoCancel);

					if (saveChanges == DialogResult.Yes)
						save();
					else if (saveChanges == DialogResult.Cancel)
						return;
				}
			}

			using (OpenFileDialog filePicker = new OpenFileDialog())
			{
				filePicker.Filter = "NDS files (*.nds)|*.nds";
				filePicker.RestoreDirectory = true;

				if (filePicker.ShowDialog() == DialogResult.OK)
				{
					RomFile.LoadNewRom(filePicker.FileName);

					if (!RomFile.IsValidGameVersion())
						MessageBox.Show("File selected is not a valid DS pokemon rom. It will not be loaded.");
					else if (!RomFile.IsSupportedGameVersion())
						MessageBox.Show("Pokemon Black/White and Black2/White2 roms are not supported due to significant differences in data structures from Gen 4.");
					else
					{
						IncludeGameVersionInText(RomFile.GetGameVersion());
                        mainTabControl.Enabled = true;
                        LoadMoveData();
						loadSpeciesData();
						LoadTradeControlText();
						UpdateDisplayedTradeValues();
                    }
				}                
			}

			if (RomFile.gameFamily == RomFile.GameFamilies.HGSS)
			{
				moveContestConditionComboBox.Enabled = false;
				moveContestEffectComboBox.Enabled = false;
				moveContestConditionTooltip.SetToolTip(moveContestConditionComboBox, "Unused in HeartGold and SoulSilver");
				moveContestEffectTooltip.SetToolTip(moveContestEffectComboBox, "Unused in HeartGold and SoulSilver");
			}
			else
			{
				moveContestConditionComboBox.Enabled = true;
				moveContestEffectComboBox.Enabled = true;
				moveContestConditionTooltip.SetToolTip(moveContestConditionComboBox, "Determines what contest type the move will score best in when used");
				moveContestEffectTooltip.SetToolTip(moveContestEffectComboBox, "What a move will do when used in contests");
			}
		}

		private void save()
		{
			if (RomFile.IsValidGameVersion() && RomFile.AreUnsavedChanges)
			{
				try
				{
					RomFile.Write();
					Text = Text.Remove(Text.Length - 1); //remove the * indicating unsaved changes
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
				}
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult saveChanges;
			if (RomFile.AreUnsavedChanges)
			{
				saveChanges = MessageBox.Show("There are unsaved changes to the selected ROM, do you want to save them before closing?", "Save Changes before Closing?", MessageBoxButtons.YesNoCancel);

				if (saveChanges == DialogResult.Yes)
					save();
				else if (saveChanges == DialogResult.Cancel)
					e.Cancel = true;
			}
		}

        private void MainForm_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
			if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
			{
                this.ActiveControl = null;

                save();
            }
        }

        private void tradeAbilityTextBox_MouseHover(object sender, EventArgs e)
        {
			tradeAbilityTooltip.Show("If the personality value is odd, the pokemon will have it's first ability. If it's even, the pokemon will have it's second ability (if it has one)", tradeAbilityTextBox);
        }

        private void tradeGenderTextBox_MouseHover(object sender, EventArgs e)
        {
			tradeGenderTooltip.Show("If the personality value % 256 is greater than the pokemon's gender ratio it will be male, unless the pokemon is gender unknown", tradeGenderTextBox);
        }

        private void tradeNatureTextBox_MouseHover(object sender, EventArgs e)
        {
            tradeNatureTooltip.Show("The pokemon's nature is personality value % 25, with 0 = Hardy, and 24 = Quirky", tradeNatureTextBox);
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (INIManager.Language != Languages.ENGLISH)
            {
				englishToolStripMenuItem.Checked = true;
				setTextToEnglish();
                INIManager.Language = Languages.ENGLISH;
                INIManager.SaveINI();

                españolToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;
            }
        }

		private void españolToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (INIManager.Language != Languages.SPANISH)
			{
                españolToolStripMenuItem.Checked = true;
                setTextToSpanish();
                INIManager.Language = Languages.SPANISH;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;
            }
		}

        private void françaisToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (INIManager.Language != Languages.FRENCH)
			{
                françaisToolStripMenuItem.Checked = true;
                setTextToFrench();
                INIManager.Language = Languages.FRENCH;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                españolToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;
            }
        }

		private void deutschToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (INIManager.Language != Languages.GERMAN)
			{
                deutschToolStripMenuItem.Checked = true;
                setTextToGerman();
                INIManager.Language = Languages.GERMAN;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                españolToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;
            }
		}

		private void italianoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (INIManager.Language != Languages.ITALIAN)
			{
                italianoToolStripMenuItem.Checked = true;
                setTextToItalian();
                INIManager.Language = Languages.ITALIAN;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                españolToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;
            }
		}

        private void 日本語ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (INIManager.Language != Languages.JAPANESE)
            {
                日本語ToolStripMenuItem.Checked = true;
				setTextToJapanese();
                INIManager.Language = Languages.JAPANESE;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                españolToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;
                한국어ToolStripMenuItem.Checked = false;
            }
        }

        private void 한국어ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (INIManager.Language != Languages.KOREAN)
            {
                한국어ToolStripMenuItem.Checked = true;
                setTextToKorean();
                INIManager.Language = Languages.KOREAN;
                INIManager.SaveINI();

                englishToolStripMenuItem.Checked = false;
                日本語ToolStripMenuItem.Checked = false;
                españolToolStripMenuItem.Checked = false;
                françaisToolStripMenuItem.Checked = false;
                deutschToolStripMenuItem.Checked = false;
                italianoToolStripMenuItem.Checked = false;
            }
        }

        
    }
}
