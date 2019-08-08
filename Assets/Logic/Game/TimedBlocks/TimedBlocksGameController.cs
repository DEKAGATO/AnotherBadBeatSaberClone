﻿using UnityEngine;
using PSpectrumInfo;

public class TimedBlocksGameController : MonoBehaviour
{
    private BlockDataControllerPlain _blockDataController;

    // Start is called before the first frame update
    void Start()
    {
        _blockDataController = new BlockDataControllerPlain();
    }

    public void setSpectrumData(FastList<PAnalyzedSpectrumData> spectrumDataList)
    {
        _blockDataController.createBlockData(spectrumDataList);
    }

}