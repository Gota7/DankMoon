using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace DankMoon {

    /// <summary>
    /// Wave file.
    /// </summary>
    public class Wave : Riff {

        /// <summary>
        /// Create a new wave from a file.
        /// </summary>
        /// <param name="file">The file to create a wave from.</param>
        public Wave(byte[] file) : base(file) {}

        /// <summary>
        /// Create a new wave from a reader.
        /// </summary>
        /// <param name="br">The reader.</param>
        public Wave(BinaryDataReader br) : base(br) {}
        
        /// <summary>
        /// Format chunk.
        /// </summary>
        private FormatChunk fmt;

        /// <summary>
        /// Read a chunk.
        /// </summary>
        /// <param name="magic">Chunk magic.</param>
        /// <param name="br">The reader.</param>
        public override void ReadChunk(string magic, BinaryDataReader br) {
            if (magic.Equals("fmt ")) {
                fmt = new FormatChunk(br);
                Chunks.Add(fmt);
            } else if (magic.Equals("data")) {
                Chunks.Add(new DataChunk(br, fmt));
            } else {
                new RiffChunk(br);
            }
        }

    }

}
