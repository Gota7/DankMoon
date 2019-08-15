using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DankMoon {
    /// <summary>
    /// Handle codec conversions.
    /// </summary>
    public static class EncoderFactory {


        /// <summary>
        /// Convert pcm8 audio to signed pcm8 audio.
        /// </summary>
        /// <param name="pcm8">Pcm8 data.</param>
        /// <returns>Signed pcm8 data.</returns>
        public static sbyte[][] Pcm8ToSignedPcm8(byte[][] pcm8) {

            sbyte[][] newData = new sbyte[pcm8.Length][];

            for (int i = 0; i < newData.Length; i++) {

                //Signed audio.
                List<sbyte> signedAudio = new List<sbyte>();

                //Convert the audio.
                foreach (byte sample in pcm8[i]) {

                    //Signed audio is really just centered at 0 rather than 128.
                    signedAudio.Add((sbyte)(sample - 128));

                }

                newData[i] = signedAudio.ToArray();

            }

            return newData;

        }


        /// <summary>
        /// Convert signed pcm8 audio to pcm8 audio.
        /// </summary>
        /// <param name="sPcm8">Signed pcm8 audio.</param>
        /// <returns>Pcm8 data.</returns>
        public static byte[][] SignedPcm8ToPcm8(sbyte[][] sPcm8) {

            byte[][] newData = new byte[sPcm8.Length][];

            for (int i = 0; i < newData.Length; i++) {

                //Audio.
                List<byte> audio = new List<byte>();

                //Convert the audio.
                foreach (sbyte sample in sPcm8[i]) {

                    //Audio is really just centered at 128 rather than 0.
                    audio.Add((byte)(sample + 128));

                }

                newData[i] = audio.ToArray();

            }

            return newData;

        }


        /// <summary>
        /// Convert unsigned pcm8 audio to pcm16 audio.
        /// </summary>
        /// <param name="pcm8"></param>
        /// <returns></returns>
        public static short[][] Pcm8ToPcm16(byte[][] pcm8) {

            return SignedPcm8ToPcm16(Pcm8ToSignedPcm8(pcm8));

        }


        /// <summary>
        /// Convert signed pcm8 audio to pcm16 audio.
        /// </summary>
        /// <param name="sPcm8"></param>
        /// <returns></returns>
        public static short[][] SignedPcm8ToPcm16(sbyte[][] sPcm8) {

            short[][] pcm16 = new short[sPcm8.Length][];

            //Convert to Pcm16.
            for (int i = 0; i < pcm16.Length; i++) {
                pcm16[i] = new short[sPcm8[i].Length];
                for (int j = 0; j < pcm16[i].Length; j++) {

                    pcm16[i][j] = (short)(sPcm8[i][j] << 8);

                }

            }

            return pcm16;

        }


        /// <summary>
        /// Convert pcm16 audio to signed pcm8.
        /// </summary>
        /// <param name="pcm16"></param>
        /// <returns></returns>
        public static sbyte[][] Pcm16ToSignedPcm8(short[][] pcm16) {

            sbyte[][] arr = new sbyte[pcm16.Length][];
            for (int i = 0; i < arr.Length; i++) {

                arr[i] = new sbyte[pcm16[i].Length];
                for (int j = 0; j < pcm16[i].Length; j++) {

                    arr[i][j] = (sbyte)((pcm16[i][j] >> 8) & 0xFF);

                }

            }
            return arr;

        }


        /// <summary>
        /// Convert pcm16 audio to pcm8.
        /// </summary>
        /// <param name="pcm16"></param>
        /// <returns></returns>
        public static byte[][] Pcm16ToPcm8(short[][] pcm16) {

            sbyte[][] arr = new sbyte[pcm16.Length][];
            for (int i = 0; i < arr.Length; i++) {

                arr[i] = new sbyte[pcm16[i].Length];
                for (int j = 0; j < pcm16[i].Length; j++) {

                    arr[i][j] = (sbyte)((pcm16[i][j] >> 8) & 0xFF);

                }

            }
            return SignedPcm8ToPcm8(arr);

        }


        /// <summary>
        /// Convert dsp adpcm to pcm16.
        /// </summary>
        /// <param name="dspApdcm">The dsp apdcm samples.</param>
        /// <param name="numSamples">Number of samples.</param>
        /// <param name="context">Dsp apdcm context.</param>
        /// <returns></returns>
        public static Int16[][] DspApcmToPcm16(byte[][] dspApdcm, UInt32 numSamples, DspAdpcmInfo[] context) {

            Int16[][] newData = new Int16[dspApdcm.Length][];

            for (int i = 0; i < newData.Length; i++) {

                newData[i] = new Int16[numSamples];
                DspAdpcmDecoder.Decode(dspApdcm[i], ref newData[i], ref context[i], numSamples);

            }

            return newData;

        }

    }

}
