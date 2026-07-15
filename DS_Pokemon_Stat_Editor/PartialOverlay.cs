using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pokemon_Sinjoh_Editor
{
    public class PartialOverlay
    {
        public readonly int OverlayIndex;
        private uint startOffset;
        private uint length;
        private Byte[] content;
        private readonly bool isOffsetOverlayRelative;

        public PartialOverlay(BinaryReader partialOverlayReader, int overlayIndex, uint startRomOffset, uint length) 
        {
            OverlayIndex = overlayIndex;
            this.startOffset = startRomOffset;
            this.length = length;
            isOffsetOverlayRelative = false;
            partialOverlayReader.BaseStream.Position = startRomOffset;

            content = partialOverlayReader.ReadBytes((int)length);
        }

        public PartialOverlay(Overlay parentOverlay, int overlayIndex, uint startRelativeOffset, uint length)
        {
            OverlayIndex = overlayIndex;
            this.startOffset = startRelativeOffset;
            this.length = length;
            isOffsetOverlayRelative = true;

            content = parentOverlay.GetOverlaySubset(startRelativeOffset, length);
        }

        public List<MemoryStream> SplitIntoMemStreams(uint bytesPerStream)
        {
            var splitMemoryStreams = new List<MemoryStream>();

            for (int i = 0; i < content.Length; i += (int)bytesPerStream)
                splitMemoryStreams.Add(new MemoryStream(content, i, (int)bytesPerStream));

            return splitMemoryStreams;
        }
    }
}
