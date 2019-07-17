﻿using UnityEngine;
using PSpectrumData;

public class MSpectrumPlotter : MonoBehaviour
{
    public static int DISPLAY_WINDOW_SIZE = 300;

    private int _currentPlotIndex = 0;
    private FastList<SpectrumInfo> _spectrumDataList;
    private FastList<Transform> _plotPoints;
    private bool _isReady = false;
    private float _time = 0;
    private int _bands;

    void Start()
    {
        _plotPoints = new FastList<Transform>();
        float localWidth = transform.Find("Point/BasePoint").localScale.x;

        // -n/2...0...n/2
        for (int i = 0; i < DISPLAY_WINDOW_SIZE; i++)
        {
            //Instantiate points
            GameObject point = Instantiate(Resources.Load("Point"), transform) as GameObject;
            Transform pointTransform = point.transform;
            Transform originalPointTransform = transform.Find("Point");
            // Applying original materials to all new sub points.
            foreach (Transform child in originalPointTransform)
            {
                string name = child.name;
                Renderer originalRenderer = originalPointTransform.Find(name).GetComponent<Renderer>();
                Renderer newPointRenderer = pointTransform.Find(name).GetComponent<Renderer>();
                newPointRenderer.material = originalRenderer.material;
            }

            // Set position
            float pointX = (DISPLAY_WINDOW_SIZE / 2) * -1 * localWidth + i * localWidth;
            pointTransform.localPosition = new Vector3(pointX, pointTransform.localPosition.y, pointTransform.localPosition.z);
            pointTransform.localPosition = new Vector3(pointX, pointTransform.localPosition.y, pointTransform.localPosition.z);

            _plotPoints.Add(pointTransform);
        }
    }

    void FixedUpdate()
    {
        if (_isReady && _hasRemainingSamples())
        {
            _time += 0.02f;  // 50 ms fixed update
            _updatePlot();
        }
    }

    public void setSpectrumData(FastList<SpectrumInfo> spectrumDataList)
    {
        _spectrumDataList = spectrumDataList;
        _bands = _spectrumDataList[0].bandData.Count;
        _isReady = true;
    }

    private bool _hasRemainingSamples()
    {
        return _currentPlotIndex <= _spectrumDataList.Count;
    }

    private void _updatePlot()
    {
        if (_currentPlotIndex < 0 || _currentPlotIndex >= _spectrumDataList.Count)
        {
            return;
        }
        if (_spectrumDataList[_currentPlotIndex].time > _time)
        {
            return;
        }
        if (_plotPoints.Count < DISPLAY_WINDOW_SIZE - 1)
            return;

        int numPlotted = 0;
        int windowStart = 0;
        int windowEnd = 0;

        if (_currentPlotIndex > 0)
        {
            windowStart = Mathf.Max(0, _currentPlotIndex - DISPLAY_WINDOW_SIZE / 2);
            windowEnd = Mathf.Min(_currentPlotIndex + DISPLAY_WINDOW_SIZE / 2, _spectrumDataList.Count - 1);
        }
        else
        {
            windowStart = Mathf.Max(0, _spectrumDataList.Count - DISPLAY_WINDOW_SIZE - 1);
            windowEnd = Mathf.Min(windowStart + DISPLAY_WINDOW_SIZE, _spectrumDataList.Count);
        }

        for (int i = windowStart; i < windowEnd; i++)
        {
            int plotIndex = numPlotted;
            numPlotted++;

            Transform fluxPoint = _plotPoints[plotIndex].Find("FluxPoint");
            //Transform threshPoint = _plotPoints[plotIndex].Find("ThreshPoint");

            FastList<Transform> peakPoints = new FastList<Transform>();
            FastList<Transform> threshPoints = new FastList<Transform>();
            for (int z = 0; z < _bands; z++)
            {
                peakPoints.Add(_plotPoints[plotIndex].Find("Peak" + z.ToString()));
                peakPoints[z].gameObject.SetActive(false);
                threshPoints.Add(_plotPoints[plotIndex].Find("Thresh" + z.ToString()));
            }

            SpectrumInfo info = _spectrumDataList[i];
            /*if (!info.hasPeak)
            {
                float fluxSum = 0;
                float threshSum = 0;
                foreach (SpectrumBandData data in info.bandData)
                {
                    fluxSum += data.spectralFlux;
                    threshSum += data.threshold;
                }
                _setPointHeight(fluxPoint, fluxSum);
                //_setPointHeight(threshPoint, threshSum);
            }*/

            for (int j = 0; j < info.bandData.Count; j++)
            {
                SpectrumBandData bandData = info.bandData[j];
                if (bandData.isPeak)
                {
                    _setPointHeight(peakPoints[j], bandData.spectralFlux);
                    peakPoints[j].gameObject.SetActive(true);
                }
                _setPointHeight(threshPoints[j], bandData.threshold);
            }


            

            //Transform extraPeakPoint = _plotPoints[plotIndex].Find("ExtraPeakPoint");


            /*if (_spectrumDataList[i].isExtraPeak)
            {
                _setPointHeight(extraPeakPoint, _spectrumDataList[i].spectralFlux);
                _setPointHeight(peakPoint, 0f);
                _setPointHeight(fluxPoint, 0f);
            }
            else if (_spectrumDataList[i].isPeak)
            {
                _setPointHeight(peakPoint, _spectrumDataList[i].spectralFlux);
                _setPointHeight(extraPeakPoint, -1000f);
                _setPointHeight(fluxPoint, 0f);
            }
            else
            {
                _setPointHeight(fluxPoint, _spectrumDataList[i].spectralFlux);
                _setPointHeight(extraPeakPoint, 0f);
                _setPointHeight(peakPoint, -1000f);
            }
            _setPointHeight(threshPoint, _spectrumDataList[i].threshold);*/
        }

        _currentPlotIndex += 1;

    }

    private void _setPointHeight(Transform point, float height)
    {
        float displayMultiplier = 0.06f + 2f;

        point.localPosition = new Vector3(point.localPosition.x, height * displayMultiplier, point.localPosition.z);
    }

}
