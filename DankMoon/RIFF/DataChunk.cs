using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace DankMoon {

    /// <summary>
    /// Data chunk.
    /// </summary>
    public class DataChunk : RiffChunk {

        /// <summary>
        /// Format chunk.
        /// </summary>
        public FormatChunk Fmt;

        /// <summary>
        /// Data.
        /// </summary>
        public short[][] Pcm16;

        /// <summary>
        /// Data.
        /// </summary>
        public byte[][] DspApcm;

        /// <summary>
        /// New data chunk.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="fmt">Format chunk.</param>
        public DataChunk(BinaryDataReader br, FormatChunk fmt) : base() {
            Fmt = fmt;
            Read(br);
        }

        /// <summary>
        /// Read the data.
        /// </summary>
        /// <param name="br">The reader.</param>
        public override void ReadData(BinaryDataReader br) {

            //DSP-ADPCM.
            if (Fmt.WaveFormat == WaveFormatType.DSPADPCM) {

                //Get number of frames.
                int numFrames = Size / Fmt.NumChannels / 8;
                List<byte>[] d = new List<byte>[Fmt.NumChannels];
                for (int i = 0; i < d.Length; i++) {
                    d[i] = new List<byte>();
                }
                for (int i = 0; i < numFrames; i++) {
                    for (int j = 0; j < Fmt.NumChannels; j++) {
                        d[j].AddRange(br.ReadBytes(8));
                    }
                }

                //Finish.
                DspApcm = new byte[d.Length][];
                for (int i = 0; i < d.Length; i++) {
                    DspApcm[i] = d[i].ToArray();
                }

            }

        }

        /// <summary>
        /// Write data.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public override void WriteData(BinaryDataWriter bw) {

            //PCM16.
            if (Fmt.WaveFormat == WaveFormatType.PCM) {

                //Write data.
                int numSamples = Pcm16[0].Length;
                for (int i = 0; i < numSamples; i++) {
                    for (int j = 0; j < Pcm16.Length; j++) {
                        bw.Write(Pcm16[j][i]);
                    }
                }

            }

        }

    }

}
