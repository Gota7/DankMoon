using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DankMoon {
    class Program {
        static void Main(string[] args) {

            Console.WriteLine("Dank Moon Sound Extractor - c2019 Gota7");
            if (!Directory.Exists("wwiseaudio")) {
                Console.WriteLine("wwiseaudio folder not found!");
                Console.WriteLine(" Press any key to continue . . .");
                Console.ReadKey();
                Environment.Exit(0);
            }

            foreach (string s in Directory.EnumerateFiles("wwiseaudio")) {

                //Create folders.
                Directory.CreateDirectory("Extracted/" + Path.GetFileNameWithoutExtension(s));

                //New writer.
                BinaryDataReader br = new BinaryDataReader(new FileStream(s, FileMode.Open));

                //File count.
                int fileCount = 0;

                //READ.
                while (br.Position <= br.BaseStream.Length - 4) {

                    //RIFF.
                    try {
                        if (br.ReadUInt32() == 0x46464952) {

                            //Read RIFF.
                            br.Position -= 4;
                            Wave w = new Wave(br);
                            FormatChunk f = w.Chunks.Where(x => x.Magic.Equals("fmt ")).ElementAt(0) as FormatChunk;
                            f.WaveFormat = WaveFormatType.PCM;
                            f.BitsPerSample = 16;

                            //Convert data.
                            try {
                                DataChunk d = w.Chunks.Where(x => x.Magic.Equals("data")).ElementAt(0) as DataChunk;
                                d.Pcm16 = EncoderFactory.DspApcmToPcm16(d.DspApcm, f.DspAdpcmNumSamples, f.ChannelInfo.ToArray());
                                File.WriteAllBytes("Extracted/" + Path.GetFileNameWithoutExtension(s) + "/" + fileCount.ToString("D4") + ".wav", w.ToFile());
                            } catch {
                                Console.WriteLine("Failed for " + s + " (Sound " + fileCount.ToString("D4") + ")");
                            }
                            fileCount++;

                        }

                        //Nothing.
                        else {
                            br.Position -= 3;
                        }
                    } catch { break; }

                }

                //Close.
                br.Dispose();

            }

            //Exit.
            Console.WriteLine(" Press any key to continue . . .");
            Console.ReadKey();

        }
    }
}
