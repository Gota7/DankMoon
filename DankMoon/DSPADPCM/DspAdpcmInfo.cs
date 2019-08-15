using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DankMoon {

    /// <summary>
    /// DSP-ADPCM info.
    /// </summary>
    public class DspAdpcmInfo {

        /// <summary>
        /// 1 - [8][2] array of coefficients.
        /// </summary>
        public Int16[][] coefs;

        /// <summary>
        /// 2 - Predictor scale.
        /// </summary>
        public UInt16 pred_scale;

        /// <summary>
        /// 3 - History 1.
        /// </summary>
        public Int16 yn1;

        /// <summary>
        /// 4 - History 2.
        /// </summary>
        public Int16 yn2;

        /// <summary>
        /// 5 - Loop predictor scale.
        /// </summary>
        public UInt16 loop_pred_scale;

        /// <summary>
        /// 6 - Loop history 1.
        /// </summary>
        public Int16 loop_yn1;

        /// <summary>
        /// 7 - Loop history 2.
        /// </summary>
        public Int16 loop_yn2;

    }

    /// <summary>
    /// Dsp adpcm constants.
    /// </summary>
    public static class DspAdpcmConstants {

        public const int BYTES_PER_FRAME = 8;
        public const int SAMPLES_PER_FRAME = 14;
        public const int NIBBLES_PER_FRAME = 16;

    }

}
