using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DankMoon {

    /// <summary>
    /// A Riff File.
    /// </summary>
    public class Riff {

        /// <summary>
        /// Magic.
        /// </summary>
        public string Magic = "RIFF";

        /// <summary>
        /// Filesize.
        /// </summary>
        public int Size => Chunks.Select(x => x.Size).Sum() + 4 + 8 * Chunks.Count;

        /// <summary>
        /// File type.
        /// </summary>
        public string Type = "WAVE";

        /// <summary>
        /// Chunks.
        /// </summary>
        public List<RiffChunk> Chunks = new List<RiffChunk>();

        /// <summary>
        /// Read a RIFF from a file.
        /// </summary>
        /// <param name="file">File to read the RIFF from.</param>
        public Riff(byte[] file) {

            //Set up stream.
            MemoryStream src = new MemoryStream(file);
            BinaryDataReader br = new BinaryDataReader(src);

            //Read file.
            Magic = new string(br.ReadChars(4));
            int size = br.ReadInt32() - 4;
            Type = new string(br.ReadChars(4));
            Chunks = new List<RiffChunk>();
            while (br.Position < file.Length) {
                string magic = new string(br.ReadChars(4));
                br.Position -= 4;
                ReadChunk(magic, br);
            }

            //Dispose stream.
            try { br.Dispose(); } catch { }
            try { src.Dispose(); } catch { }

        }

        /// <summary>
        /// Create a new RIFF.
        /// </summary>
        /// <param name="br">The reader.</param>
        public Riff(BinaryDataReader br) {

            //Read file.
            Magic = new string(br.ReadChars(4));
            int size = br.ReadInt32() - 4;
            Type = new string(br.ReadChars(4));
            Chunks = new List<RiffChunk>();
            bool freePass = true;
            while (freePass) {
                if (Chunks.Count > 0) {
                    if (Chunks[Chunks.Count - 1].Magic.Equals("data")) {
                        freePass = false;
                    }
                }
                string magic = new string(br.ReadChars(4));
                br.Position -= 4;
                ReadChunk(magic, br);
            }

        }

        /// <summary>
        /// Read a chunk.
        /// </summary>
        /// <param name="magic">Chunk magic.</param>
        /// <param name="br">The reader.</param>
        public virtual void ReadChunk(string magic, BinaryDataReader br) {
            Chunks.Add(new RiffChunk(br));
        }

        /// <summary>
        /// Convert the RIFF to a file.
        /// </summary>
        /// <returns>A file.</returns>
        public byte[] ToFile() {

            //New writer.
            MemoryStream o = new MemoryStream();
            BinaryDataWriter bw = new BinaryDataWriter(o);

            //New writer.
            MemoryStream o2 = new MemoryStream();
            BinaryDataWriter w = new BinaryDataWriter(o2);

            //Write dummy data.
            foreach (var r in Chunks) {
                r.Write(w);
            }

            //Write data.
            bw.Write(Magic.ToCharArray());
            bw.Write(Size);
            bw.Write(Type.ToCharArray());
            foreach (var r in Chunks) {
                r.Write(bw);
            }

            //To return.
            byte[] ret = o.ToArray();

            //Dispose of data.
            try { bw.Dispose(); } catch { }
            try { o.Dispose(); } catch { }
            try { w.Dispose(); } catch { }
            try { o2.Dispose(); } catch { }

            //Return file.
            return ret;

        }

    }

}
