using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Pokemon_Sinjoh_Editor
{
    public class Move
    {
        public ushort Effect;
        public byte EffectChance;
        public byte Power;
        public byte PowerPoints;
        public byte Type;
        public byte Accuracy;
        public sbyte Priority;
        public byte ContestEffect;
        public Categories Category;
        public ContestConditions ContestCondition;
        public Targets Target;
        private Flags flags;

        //these are parallel arrays
        private static string[] EffectDescriptions;
        private static bool[] DoEffectsCalcDamage;

        public const int NUM_EFFECTS = 276;
      

        public static string[] ContestEffectDescriptions { get; private set; } = new string[] 
        {
            "N/A",
            "2 appeal, perform first next turn",
            "2 appeal, perform last next turn",
            "Unused",
            "2 appeal, +2 if voltage went up",
            "3 appeal",
            "1 appeal, +3 for unique judge",
            "2 appeal, repeatable",
            "Voltage = performance",
            "15 appeal if all same judge",
            "2 appeal, lower all judge voltages",
            "Double score next turn",
            "Steal voltage of previous pokemon",
            "2 appeal, no voltage gain this turn",
            "2 appeal, random order next turn",
            "2 appeal, 2x score final performance",
            "Raise score if voltage low",
            "2 appeal, +2 if first",
            "2 appeal, +2 if last",
            "2 appeal, no voltage loss this turn",
            "1 appeal, +3 if voltage raise 2x in a row",
            "Higher score the later in turn",
            "2 appeal, +3 if voltage maxed",
            "1 appeal, +3 if lowest score"
        };

        public static bool[] DoesEffectCalcDamage { get; private set; } = new bool[277];

        public static int TargetEnumToIndexValue(Targets targetVal)
        {
            //just restorting to using a switch expression because I've spent 3 hours trying to figure out an equation
            return targetVal switch
            {
                Targets.FOE_OR_ALLY => 0,
                Targets.OTHER => 1,
                Targets.RANDOM => 2,
                Targets.ALL_FOES => 3,
                Targets.ALL_OTHERS => 4,
                Targets.SELF => 5,
                Targets.SELF_AND_ALLY => 6,
                Targets.ALL => 7,
                Targets.FOE_SIDE => 8,
                Targets.ALLY => 9,
                Targets.SELF_OR_ALLY => 10,
                Targets.ANY_FOE => 11,
                _ => -1
            };
        }

        public static Targets IndexValueToTargetEnum(int index)
        {
            //have to check for 0 because Math.pow doesn't support negative exponents
            if (index == 0)
                return 0;
            else
                return (Targets)((int)Math.Pow(2, index - 1));
        }

        #region enums
        public enum Categories
        {
            PHYSICAL, SPECIAL, STATUS
        }

        public enum ContestConditions
        {
            COOL, BEAUTIFUL, CUTE, SMART, TOUGH
        }

        public enum Targets
        {
            FOE_OR_ALLY = 0,
            OTHER = 1,
            RANDOM = 2,
            ALL_FOES = 4,
            ALL_OTHERS = 8,
            SELF = 16,
            SELF_AND_ALLY = 32,
            ALL = 64,
            FOE_SIDE = 128,
            ALLY = 256,
            SELF_OR_ALLY = 512,
            ANY_FOE = 1024,
        }

        public enum Flags
        {
            NONE = 0,
            CONTACT = 1,
            PROTECT = 2,
            MAGIC_COAT = 4,
            SNATCH = 8,
            MIRROR_MOVE = 16,
            KINGS_ROCK = 32,
            KEEP_HP_BAR_VISIBLE = 64,
            HIDE_POKEMON_SHADOWS = 128
        }

        #endregion

        public bool ContactFlag
        {
            get => flags.HasFlag(Flags.CONTACT);
            set
            {
                if (value)
                    flags |= Flags.CONTACT;
                else
                {
                    flags &= ~Flags.CONTACT;
                }
            }
        }

        public bool ProtectFlag
        {
            get => flags.HasFlag(Flags.PROTECT);
            set
            {
                if (value)
                    flags |= Flags.PROTECT;
                else
                {
                    flags &= ~Flags.PROTECT;
                }
            }
        }

        public bool MagicCoatFlag
        {
            get => flags.HasFlag(Flags.MAGIC_COAT);
            set
            {
                if (value)
                    flags |= Flags.MAGIC_COAT;
                else
                {
                    flags &= ~Flags.MAGIC_COAT;
                }
            }
        }

        public bool SnatchFlag
        {
            get => flags.HasFlag(Flags.SNATCH);
            set
            {
                if (value)
                    flags |= Flags.SNATCH;
                else
                {
                    flags &= ~Flags.SNATCH;
                }
            }
        }

        public bool MirrorMoveFlag
        {
            get => flags.HasFlag(Flags.MIRROR_MOVE);
            set
            {
                if (value)
                    flags |= Flags.MIRROR_MOVE;
                else
                {
                    flags &= ~Flags.MIRROR_MOVE;
                }
            }
        }

        public bool KingsRockFlag
        {
            get => flags.HasFlag(Flags.KINGS_ROCK);
            set
            {
                if (value)
                    flags |= Flags.KINGS_ROCK;
                else
                {
                    flags &= ~Flags.KINGS_ROCK;
                }
            }
        }

        public bool KeepHPBarVisibleFlag
        {
            get => flags.HasFlag(Flags.KEEP_HP_BAR_VISIBLE);
            set
            {
                if (value)
                    flags |= Flags.KEEP_HP_BAR_VISIBLE;
                else
                {
                    flags &= ~Flags.KEEP_HP_BAR_VISIBLE;
                }
            }
        }

        public bool HidePokemonShadowsFlag
        {
            get => flags.HasFlag(Flags.HIDE_POKEMON_SHADOWS);
            set
            {
                if (value)
                    flags |= Flags.HIDE_POKEMON_SHADOWS;
                else
                {
                    flags &= ~Flags.HIDE_POKEMON_SHADOWS;
                }
            }
        }

        public Move()
        {
            Effect = 0;
            EffectChance = 0;
            Power = 0;
            PowerPoints = 0;
            Accuracy = 0;
            Type = 0;
            Priority = 0;
            ContestEffect = 0;
            Category = 0;
            ContestCondition = 0;
            Target = 0;
            flags = 0;
        }

        public static string[] GetEffectDescriptions()
        {
            if (EffectDescriptions.Count() == 0)
            {
                EffectDescriptions = new string[NUM_EFFECTS];

                EffectDescriptions[0] = "Only Damage";
                EffectDescriptions[1] = "Apply sleep to foe";
                EffectDescriptions[2] = "Apply poison to foe";
                EffectDescriptions[3] = "Heal user 50% of damage";
                EffectDescriptions[4] = "Apply burn to foe";
                EffectDescriptions[5] = "Apply freeze to foe";
                EffectDescriptions[6] = "Apply paralysis to foe";
                EffectDescriptions[7] = "Faint user, half foe's defense for this move's damage";
                EffectDescriptions[8] = "Heal user 50% of damage if foe is asleep, fail otherwise";
                EffectDescriptions[9] = "Use the foe's last used move";
                EffectDescriptions[10] = "Raise user's attack by 1 stage";
                EffectDescriptions[11] = "Raise user's defense by 1 stage";
                EffectDescriptions[12] = "";
                EffectDescriptions[13] = "Raise user's special attack by 1 stage";
                EffectDescriptions[14] = "";
                EffectDescriptions[15] = "";
                EffectDescriptions[16] = "Raise user's evasion by 1 stage";
                EffectDescriptions[17] = "Never misses";
                EffectDescriptions[18] = "Lower foe's attack by 1 stage";
                EffectDescriptions[19] = "Lower foe's defense by 1 stage";
                EffectDescriptions[20] = "Lower foe's speed by 1 stage";
                EffectDescriptions[21] = "";
                EffectDescriptions[22] = "";
                EffectDescriptions[23] = "Lower foe's accuracy by 1 stage";
                EffectDescriptions[24] = "Lower foe's evasion by 1 stage";


                return EffectDescriptions;
            }
            else
                return EffectDescriptions;
        }

        public static bool[] GetDoEffectCalcDamage()
        {
            if (DoEffectsCalcDamage.Count() == 0)
            {
                DoEffectsCalcDamage = new bool[NUM_EFFECTS];

                DoEffectsCalcDamage[0] = true;
                DoEffectsCalcDamage[1] = false;
                DoEffectsCalcDamage[2] = true;
                DoEffectsCalcDamage[3] = true;
                DoEffectsCalcDamage[4] = true;
                DoEffectsCalcDamage[5] = true;
                DoEffectsCalcDamage[6] = true;
                DoEffectsCalcDamage[7] = true;
                DoEffectsCalcDamage[8] = true;
                DoEffectsCalcDamage[9] = false;
                DoEffectsCalcDamage[10] = false;

                return DoEffectsCalcDamage;
            }
            else
                return DoEffectsCalcDamage;
        }

                public Move(MemoryStream move)
                {
                    var moveBinaryReader = new BinaryReader(move, Encoding.UTF8, true);
                    Effect = moveBinaryReader.ReadUInt16();
                    Category = (Categories)moveBinaryReader.ReadByte();
                    Power = moveBinaryReader.ReadByte();
                    Type = moveBinaryReader.ReadByte();
                    Accuracy = moveBinaryReader.ReadByte();
                    PowerPoints = moveBinaryReader.ReadByte();
                    EffectChance = moveBinaryReader.ReadByte();
                    Target = (Targets)moveBinaryReader.ReadUInt16();
                    Priority = moveBinaryReader.ReadSByte();
                    flags = (Flags)moveBinaryReader.ReadByte();
                    ContestEffect = moveBinaryReader.ReadByte();
                    ContestCondition = (ContestConditions)moveBinaryReader.ReadByte();
                }

        public MemoryStream GetBinary()
        {
            var moveBinary = new MemoryStream();
            using (var moveBinaryWriter = new BinaryWriter(moveBinary, Encoding.UTF8, true))
            {
                moveBinaryWriter.Write(Effect);
                moveBinaryWriter.Write((byte)Category);
                moveBinaryWriter.Write(Power);
                moveBinaryWriter.Write(Type);
                moveBinaryWriter.Write(Accuracy);
                moveBinaryWriter.Write(PowerPoints);
                moveBinaryWriter.Write(EffectChance);
                moveBinaryWriter.Write((ushort)Target);
                moveBinaryWriter.Write(Priority);
                moveBinaryWriter.Write((byte)flags);
                moveBinaryWriter.Write(ContestEffect);
                moveBinaryWriter.Write((byte)ContestCondition);
                moveBinaryWriter.Write((ushort)0x0000);
            }
            return moveBinary;
        }


        public bool Equals(Move comparingMove)
        {
            if (comparingMove.Effect != Effect)
                return false;

            if (comparingMove.EffectChance != Effect)
                return false;

            if (comparingMove.Power != Power)
                return false;

            if (comparingMove.Accuracy != Accuracy)
                return false;

            if (comparingMove.Type != Type)
                return false;

            if (comparingMove.Priority != Priority)
                return false;

            if (!string.Equals(comparingMove.Category, Category.ToString()))
                return false;

            if (!string.Equals(comparingMove.Target, Target.ToString()))
                return false;

            if (!string.Equals(comparingMove.ContestCondition, ContestCondition.ToString()))
                return false;

            if (comparingMove.ContestEffect != ContestEffect)
                return false;

            if (comparingMove.ContactFlag != ContactFlag)
                return false;

            if (comparingMove.ProtectFlag != ProtectFlag)
                return false;

            if (comparingMove.MirrorMoveFlag != MirrorMoveFlag)
                return false;

            if (comparingMove.SnatchFlag != SnatchFlag)
                return false;

            if (comparingMove.MagicCoatFlag != MagicCoatFlag)
                return false;

            if (comparingMove.KingsRockFlag != KingsRockFlag)
                return false;

            if (comparingMove.KeepHPBarVisibleFlag != KeepHPBarVisibleFlag)
                return false;

            if (comparingMove.HidePokemonShadowsFlag != HidePokemonShadowsFlag)
                return false;

            return true;
        }


        override public string ToString()
        {
            return "Power=" + Power + Environment.NewLine
                + "PP=" + PowerPoints + Environment.NewLine
                + "Accuracy=" + Accuracy + Environment.NewLine
                + "Type=" + Type + Environment.NewLine
                + "Category=" + Category + Environment.NewLine
                + "Effect=" + Effect + Environment.NewLine
                + "Effect chance=" + EffectChance + Environment.NewLine
                + "Priority=" + Priority + Environment.NewLine
                + "Target=" + Target + Environment.NewLine
                + "Contest condition=" + ContestCondition + Environment.NewLine
                + "Contest Effect=" + ContestEffect + Environment.NewLine
                + "Makes contact=" + ContactFlag + Environment.NewLine
                + "Affected by protect=" + ProtectFlag + Environment.NewLine
                + "Affected by magic coat=" + MagicCoatFlag + Environment.NewLine
                + "Affected by snatch=" + SnatchFlag + Environment.NewLine
                + "Affected by mirror coat=" + MirrorMoveFlag + Environment.NewLine
                + "Affected by king's rock=" + KingsRockFlag + Environment.NewLine
                + "Keep HP bar visible=" + KeepHPBarVisibleFlag + Environment.NewLine
                + "Hide pokemon shadows=" + HidePokemonShadowsFlag + Environment.NewLine;
        }


    }
}
