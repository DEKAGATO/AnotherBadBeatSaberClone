﻿using System.Collections.Generic;

namespace AudioAnalysis
{
    /**
     * Configs in this namespace contain information about a spectrum that was analyzed.
     **/
    namespace SpectrumConfigs
    {
        /// <summary>
        /// Config that contains band configs with analyzed spectrum data.Summarizes those band configs
        /// and for example provides easier access to the peaks that have been found in the different bands.
        /// </summary>
        public class AnalyzedSpectrumConfig
        {
            public float[] Spectrum { get; set; }
            public float Time { get; set; }
            public bool HasPeak { get; set; }
            public List<int> PeakBands { get; set; } = new List<int>();
            public List<BeatConfig> BandBeatData { get; set; } = new List<BeatConfig>();
        }


        /// <summary>
        /// Config that contains data obtained from a specific spectrum that is analyzed in a specific band.
        /// </summary>
        public class BeatConfig
        {
            public float SpectralFlux { get; set; }
            public float Threshold { get; set; }
            public float PrunedSpectralFlux { get; set; }
            public float Band { get; set; }
            public float PeakBPM { get; set; }
            public bool IsPeak { get; set; }
        }

    }

}
