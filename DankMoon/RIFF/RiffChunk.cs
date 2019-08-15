using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DankMoon {

    /// <summary>
    /// Riff chunk.
    /// </summary>
    public class RiffChunk {

        /// <summary>
        /// Magic.
        /// </summary>
        public string Magic;

        /// <summary>
        /// Size of the chunk.
        /// </summary>
        public int Size => Data.Length;

        /// <summary>
        /// Chunk data.
        /// </summary>
        private byte[] Data = new byte[0];

        /// <summary>
        /// Read the data.
        /// </summary>
        /// <param name="br">The reader.</param>
        public virtual void ReadData(BinaryDataReader br) {}

        /// <summary>
        /// Write the data.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public virtual void WriteData(BinaryDataWriter bw) {}

        /// <summary>
        /// Blank constructor.
        /// </summary>
        public RiffChunk() {}

        /// <summary>
        /// Create a new RIFF chunk.
        /// </summary>
        /// <param name="br">The reader.</param>
        public RiffChunk(BinaryDataReader br) {
            Read(br);
        }

        /// <summary>
        /// Read the chunk.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void Read(BinaryDataReader br) {

            //Read chunk.
            Magic = new string(br.ReadChars(4));
            int size = br.ReadInt32();
            Data = br.ReadBytes(size);

            //Read data.
            MemoryStream src = new MemoryStream(Data);
            BinaryDataReader r = new BinaryDataReader(src);
            ReadData(r);
            try { r.Dispose(); } catch { }
            try { src.Dispose(); } catch { }

        }

        /// <summary>
        /// Write the chunk.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(BinaryDataWriter bw) {

            //Get the data.
            MemoryStream o = new MemoryStream();
            BinaryDataWriter w = new BinaryDataWriter(o);
            WriteData(w);
            Data = o.ToArray();
            try { w.Dispose(); } catch { }
            try { o.Dispose(); } catch { }

            //Write chunk.
            bw.Write(Magic.ToCharArray());
            bw.Write(Size);
            bw.Write(Data);

        }

    }

}
