using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pokemon_Sinjoh_Editor
{
    public class Item
    {
        public ushort Price;
        public byte HoldEffect;
        public byte HoldEffectParameter;
        public byte PluckEffect;
        public byte FlingEffect;
        public byte FlingPower;
        public byte NaturalGiftPower;
        public byte NaturalGiftType;
        public bool PreventToss;
        public bool Selectable;
        public FieldPockets FieldPocket;
        public BattlePockets BattlePocket;
        public byte FieldUseFunction;
        public byte BattleUseFunction;
        public byte PartyUse;

        public const int NATURAL_GIFT_UNUSEABLE = 31;
        private const int NATURAL_GIFT_TYPE_BIT_MASK = 0b_0001_1111;
        private const int PREVENT_TOSS_BIT_MASK = 0b_0010_0000;
        private const int SELECTABLE_BIT_MASK = 0b_0100_0000;
        private const int FIELD_POCKET_BIT_MASK = 0b_0000_0111_1000_0000;
        private const int BATTLE_POCKET_BIT_MASK = 0b_1111_1000_0000_0000;

        private const int PREVENT_TOSS_BIT_SHIFT = 5;
        private const int SELECTABLE_BIT_SHIFT = 6;
        private const int FIELD_POCKET_BIT_SHIFT = 7;
        private const int BATTLE_POCKET_BIT_SHIFT = 11;

        public enum FieldPockets
        {
            ITEMS,
            MEDICINE,
            POKEBALLS,
            TMS_and_HMS,
            BERRIES,
            MAIL,
            BATTLE_ITEMS,
            KEY_ITEMS
        }

        public enum BattlePockets
        {
            NONE = 0,
            POKEBALLS = 1,
            HP_PP_RESTORE = 4,
            STATUS_RESTORE = 8,
            BATTLE_ITEMS = 16
        }

        public Item(MemoryStream item) 
        {
            var itemBinaryReader = new BinaryReader(item, Encoding.UTF8, true);
            Price = itemBinaryReader.ReadUInt16();
            HoldEffect = itemBinaryReader.ReadByte();
            HoldEffectParameter = itemBinaryReader.ReadByte();
            PluckEffect = itemBinaryReader.ReadByte();
            FlingEffect = itemBinaryReader.ReadByte();
            FlingPower = itemBinaryReader.ReadByte();
            NaturalGiftPower = itemBinaryReader.ReadByte();

            ushort bitField = itemBinaryReader.ReadUInt16();

            NaturalGiftType = (byte)(bitField & NATURAL_GIFT_TYPE_BIT_MASK);
            PreventToss = ((bitField & PREVENT_TOSS_BIT_MASK) >> PREVENT_TOSS_BIT_SHIFT) > 0 ? true : false;
            Selectable = ((bitField & SELECTABLE_BIT_MASK) >> SELECTABLE_BIT_SHIFT) > 0 ? true : false;
            FieldPocket = (FieldPockets)((bitField & FIELD_POCKET_BIT_MASK) >> FIELD_POCKET_BIT_SHIFT);
            BattlePocket = (BattlePockets)((bitField & BATTLE_POCKET_BIT_MASK) >> BATTLE_POCKET_BIT_SHIFT);

            FieldUseFunction = itemBinaryReader.ReadByte();
            BattleUseFunction = itemBinaryReader.ReadByte();
            PartyUse = itemBinaryReader.ReadByte();

            itemBinaryReader.Dispose();
        }

        public MemoryStream GetBinary()
        {
            var itemBinary = new MemoryStream();
            using (var itemBinaryWriter = new BinaryWriter(itemBinary, Encoding.UTF8, true))
            {
                itemBinaryWriter.Write(Price);
                itemBinaryWriter.Write(HoldEffect);
                itemBinaryWriter.Write(HoldEffectParameter);
                itemBinaryWriter.Write(PluckEffect);
                itemBinaryWriter.Write(FlingEffect);
                itemBinaryWriter.Write(FlingPower);
                itemBinaryWriter.Write(NaturalGiftPower);
                itemBinaryWriter.Write((ushort)NaturalGiftType);
                itemBinaryWriter.Write((ushort)(PreventToss ? 1 : 0));
                itemBinaryWriter.Write((ushort)(Selectable ? 1 : 0));

                itemBinaryWriter.Write((ushort)FieldPocket);
                itemBinaryWriter.Write((ushort)BattlePocket);

                itemBinaryWriter.Write(FieldUseFunction);
                itemBinaryWriter.Write(BattleUseFunction);
                itemBinaryWriter.Write(PartyUse);

            }

            return itemBinary;
        }

        public bool CanNaturalGiftUse() => NaturalGiftType >= NATURAL_GIFT_UNUSEABLE ? false : true;
        public void SetCanNaturalGiftUse(bool useable)
        {
            if (useable)
                NaturalGiftType = NATURAL_GIFT_UNUSEABLE;
            else
                NaturalGiftType = 0;
        }

    }
}
