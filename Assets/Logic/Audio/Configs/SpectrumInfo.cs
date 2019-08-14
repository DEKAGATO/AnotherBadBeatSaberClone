﻿using System.Collections.Generic;

namespace AudioSpectrumInfo {
    public class AnalyzedSpectrumData
    {
        public float[] spectrum;
        public float time;
        public bool hasPeak;
        public List<int> peakBands = new List<int>();
        public List<BeatInfo> beatData = new List<BeatInfo>();
    }

    public class BeatInfo
    {
        public float spectralFlux;
        public float threshold;
        public float prunedSpectralFlux;
        public float band;
        public float peakBPM;
        public bool isPeak;
    }

}