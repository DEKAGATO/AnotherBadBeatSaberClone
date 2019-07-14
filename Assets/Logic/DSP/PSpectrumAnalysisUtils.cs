﻿using UnityEngine;

public static class PSpectrumAnalysisUtils
{
    public static bool shouldBeExtraPeak(float totalAverageFlux, float currentAverageFlux)
    {
        return currentAverageFlux > totalAverageFlux * 1.5;
    }

    public static bool sampleIsCloser(float newVal, float oldVal, float trueVal)
    {
        return Mathf.Abs(trueVal - newVal) < Mathf.Abs(trueVal - oldVal);
    }

}
