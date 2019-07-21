﻿using UnityEngine;
using PSpectrumData;
using System.Collections;

public class MSpectrumPlotter : MonoBehaviour
{
    public const string SHOW_PRUNED = "SHOW_PRUNED";
    public const string SHOW_PEAKS = "SHOW_PEAKS";

    private const int DISPLAY_WINDOW_SIZE = 300;

    private FastList<SpectrumInfo> _spectrumDataList;
    private FastList<Transform> _plotPoints;
    private bool _isReady = false;
    private float _lastTime;
    private int _spectrumIndex = 0;
    private int _bands;
    private string _type;

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

        for (int i = 0; i < DISPLAY_WINDOW_SIZE; i++) // This does not work, why not?
        {
            for (int j = 0; j < _bands; j++)
            {
                _setPointHeight(_plotPoints[i].Find("Peak" + j.ToString()), -1000);
            }
        }
    }

    void FixedUpdate()
    {
        if (_isReady && _hasRemainingSamples())
        {
            float newTime = Time.time;
            Debug.Log(newTime - _lastTime);
            _lastTime = newTime;

            _updatePlot();
        } else
        {
            Debug.Log("not enough samples or no samples left!");
        }
    }

    public void setDataAndStart(FastList<SpectrumInfo> spectrumDataList, string type)
    {
        _spectrumDataList = spectrumDataList;
        _type = type;
        _bands = _spectrumDataList[0].bandData.Count;
        _lastTime = Time.time;
        _isReady = true;
    }

    private bool _hasRemainingSamples()
    {
        return _spectrumIndex < _spectrumDataList.Count;
    }

    private void _updatePlot()
    {
        if (_spectrumIndex == 0)
        {
            Transform audioAnalyzer = GameObject.Find("AudioAnalyzer").transform;
            AudioSource audio = audioAnalyzer.GetComponent<AudioSource>();
            audio.Play();
        }
        switch (_type)
        {
            case SHOW_PEAKS:
                _showPeaks();
                break;

            case SHOW_PRUNED:
                _showPruned();
                break;
        }
        _spectrumIndex++;
    }

    private void _showPeaks()
    {
        for (int pointIndex = 0; pointIndex < DISPLAY_WINDOW_SIZE; pointIndex++)
        {
            SpectrumInfo info = _getInfo(pointIndex);

            for (int j = 0; j < _bands; j++)
            {
                SpectrumBandData bandData = info.bandData[j];

                Transform peak = _plotPoints[pointIndex].Find("Peak" + j.ToString());
                peak.gameObject.SetActive(false); // TODO why does this not work?

                Transform thresh = _plotPoints[pointIndex].Find("Thresh" + j.ToString());

                Color peakColor = bandData.isPeak ? Color.red : Color.white;
                //float peakHeight = bandData.isPeak ? bandData.spectralFlux : -100.0f;
                float peakHeight = bandData.isPeak ? 1.0f : -100.0f;

                _setPointHeight(peak, peakHeight);
                peak.gameObject.SetActive(bandData.isPeak);
                (peak.GetComponent<Renderer>() as Renderer).material.color = peakColor;

                _setPointHeight(thresh, bandData.threshold);
            }
        }
    }

    private void _showPruned()
    {
        for (int pointIndex = 0; pointIndex < DISPLAY_WINDOW_SIZE; pointIndex++)
        {
            SpectrumInfo info = _getInfo(pointIndex);

            for (int j = 0; j < _bands; j++)
            {
                SpectrumBandData bandData = info.bandData[j];
                bool isZero = bandData.prunedSpectralFlux == 0;

                Transform pruned = _plotPoints[pointIndex].Find("Pruned" + j.ToString());
                float currentHeight = (pruned.GetComponent<Renderer>() as Renderer).bounds.size.y;
                float newHeight = isZero ? 0.0005f : bandData.prunedSpectralFlux;
                Vector3 rescale = pruned.localScale;
                rescale.y = newHeight * rescale.y / currentHeight;
                pruned.localScale = rescale;
            }
        }
    }

    private void _setPointHeight(Transform point, float height)
    {
        float displayMultiplier = 0.06f * 2;

        point.localPosition = new Vector3(point.localPosition.x, height * displayMultiplier, point.localPosition.z);
    }

    private bool _areSomePointsOOB()
    {
        return _spectrumIndex > (_spectrumDataList.Count - 1 - DISPLAY_WINDOW_SIZE);
    }

    private SpectrumInfo _getInfo(int pointIndex)
    {
        int pointDataIndex = _spectrumIndex + pointIndex;
        bool isOutOfBounds = pointDataIndex > _spectrumDataList.Count - 1;
        SpectrumInfo info = isOutOfBounds ? _getEmptySpectrumInfo() : _spectrumDataList[pointDataIndex];
        return info;
    }

    private SpectrumInfo _getEmptySpectrumInfo()
    {
        SpectrumInfo emptyInfo = new SpectrumInfo();
        emptyInfo.hasPeak = false;

        for (int i = 0; i < _bands; i++)
        {
            SpectrumBandData bandData = new SpectrumBandData();
            bandData.band = i;
            bandData.isPeak = false;
            bandData.spectralFlux = 0.0f;
            bandData.prunedSpectralFlux = 0.0f;
            bandData.threshold = 0.0f;
            emptyInfo.bandData.Add(bandData);
        }
        return emptyInfo;
    }

}