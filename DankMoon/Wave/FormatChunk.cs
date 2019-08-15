using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syroot.BinaryData;

namespace DankMoon {

    /// <summary>
    /// Format chunk.
    /// </summary>
    public class FormatChunk : RiffChunk {

        /// <summary>
        /// Wave format.
        /// </summary>
        public WaveFormatType WaveFormat = WaveFormatType.PCM;

        /// <summary>
        /// Number of channels.
        /// </summary>
        public ushort NumChannels = 2;

        /// <summary>
        /// Sample rate.
        /// </summary>
        public uint SampleRate = 32000;

        /// <summary>
        /// Byte rate.
        /// </summary>
        public uint ByteRate => SampleRate * NumChannels * BitsPerSample / 8;

        /// <summary>
        /// Block align.
        /// </summary>
        public ushort BlockAlign => (ushort)(NumChannels * BitsPerSample / 8);

        /// <summary>
        /// Bits per sample.
        /// </summary>
        public ushort BitsPerSample = 16;

        /// <summary>
        /// Dsp-Adpcm number of samples.
        /// </summary>
        public uint DspAdpcmNumSamples;

        /// <summary>
        /// Dsp-Adpcm info.
        /// </summary>
        public List<DspAdpcmInfo> ChannelInfo = new List<DspAdpcmInfo>();

        /// <summary>
        /// A new format chunk.
        /// </summary>
        /// <param name="br">The reader.</param>
        public FormatChunk(BinaryDataReader br) : base(br) {}

        /// <summary>
        /// Read data.
        /// </summary>
        /// <param name="br">The reader.</param>
        public override void ReadData(BinaryDataReader br) {

            //Read data.
            WaveFormat = (WaveFormatType)br.ReadUInt16();
            NumChannels = br.ReadUInt16();
            SampleRate = br.ReadUInt32();
            br.ReadUInt32(); //Byte rate.
            br.ReadUInt16(); //Block align.
            BitsPerSample = br.ReadUInt16();

            //Extra parameters.
            if (WaveFormat == WaveFormatType.DSPADPCM) {

                //Read extra data.
                br.ReadUInt16(); //Extra data.
                br.ReadUInt16(); //Padding.
                br.ReadUInt16(); //Unknown.
                br.ReadUInt16(); //Padding.
                DspAdpcmNumSamples = br.ReadUInt32();

                //Foreach channel.
                ChannelInfo = new List<DspAdpcmInfo>();
                for (int i = 0; i < NumChannels; i++) {
                    DspAdpcmInfo d = new DspAdpcmInfo();
                    d.coefs = new short[8][];
                    for (int j = 0; j < 8; j++) {
                        d.coefs[j] = br.ReadInt16s(2);
                    }
                    br.ReadUInt16(); //Padding.
                    d.pred_scale = br.ReadUInt16();
                    d.yn1 = br.ReadInt16();
                    d.yn2 = br.ReadInt16();
                    d.loop_pred_scale = br.ReadUInt16();
                    d.loop_yn1 = br.ReadInt16();
                    d.loop_yn2 = br.ReadInt16();
                    ChannelInfo.Add(d);
                }

                //Read the last part.
                br.ReadUInt16(); //Unknown.

            }

        }

        /// <summary>
        /// Write data.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public override void WriteData(BinaryDataWriter bw) {

            //Write data.
            bw.Write((ushort)WaveFormat);
            bw.Write(NumChannels);
            bw.Write(SampleRate);
            bw.Write(ByteRate);
            bw.Write(BlockAlign);
            bw.Write(BitsPerSample);

            //Extra info currently not supported.

        }

    }

}
