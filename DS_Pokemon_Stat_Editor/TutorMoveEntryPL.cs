using System.IO;

namespace Pokemon_Sinjoh_Editor
{
    public class TutorMoveEntryPL
    {
        public ushort MoveID;
        public byte RedShards;
        public byte BlueShards;
        public byte YellowShards;
        public byte GreenShards;
        public TutorsType Tutor;

        public const int NUM_BYTES_PER = 12;

        public enum TutorsType
        {
            R212,
            SURVIVAL,
            SNOWPOINT
        }

        public TutorMoveEntryPL(MemoryStream tutorEntryStream)
        {
            using (BinaryReader tutorReader = new BinaryReader(tutorEntryStream))
            {
                MoveID = tutorReader.ReadUInt16();
                RedShards = tutorReader.ReadByte(); 
                BlueShards = tutorReader.ReadByte(); 
                YellowShards = tutorReader.ReadByte(); 
                GreenShards = tutorReader.ReadByte();
                Tutor = (TutorsType)tutorReader.ReadByte();
            }
        }

        public MemoryStream GetBinary()
        {
            MemoryStream tutorEntryStream = new MemoryStream();

            using (BinaryWriter tutorWriter = new BinaryWriter(tutorEntryStream))
            {
                tutorWriter.Write(MoveID);
                tutorWriter.Write(RedShards);
                tutorWriter.Write(BlueShards);
                tutorWriter.Write(YellowShards);
                tutorWriter.Write(GreenShards);
                tutorWriter.Write((byte)Tutor);
            }

            return tutorEntryStream;
        }
    }
}
