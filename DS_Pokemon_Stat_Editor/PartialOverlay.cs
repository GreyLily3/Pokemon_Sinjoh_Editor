using System;
using System.Collections.Generic;
using System.IO;

namespace Pokemon_Sinjoh_Editor
{
    public class PartialOverlay
    {
        public readonly int OverlayIndex;
        public Byte[] Content;
        private uint startOffset;
        private uint length;
        private readonly bool isOffsetOverlayRelative;

        public PartialOverlay(BinaryReader partialOverlayReader, int overlayIndex, uint startRomOffset, uint length) 
        {
            OverlayIndex = overlayIndex;
            this.startOffset = startRomOffset;
            this.length = length;
            isOffsetOverlayRelative = false;
            partialOverlayReader.BaseStream.Position = startRomOffset;

            Content = partialOverlayReader.ReadBytes((int)length);
        }

        public PartialOverlay(Overlay parentOverlay, int overlayIndex, uint startRelativeOffset, uint length)
        {
            OverlayIndex = overlayIndex;
            this.startOffset = startRelativeOffset;
            this.length = length;
            isOffsetOverlayRelative = true;

            Content = parentOverlay.GetSubsetBytes(startRelativeOffset, length);
        }

        public List<MemoryStream> SplitIntoMemStreams(uint bytesPerStream)
        {
            var splitMemoryStreams = new List<MemoryStream>();

            for (int i = 0; i < Content.Length; i += (int)bytesPerStream)
                splitMemoryStreams.Add(new MemoryStream(Content, i, (int)bytesPerStream));

            return splitMemoryStreams;
        }

        public void WriteUncompressed(BinaryWriter bw)
        {
            bw.BaseStream.Position = startOffset;
            bw.Write(Content);
        }
    }
}
