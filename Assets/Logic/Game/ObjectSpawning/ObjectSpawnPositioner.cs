﻿using System.Collections.Generic;
using BeatMappingConfigs;

public static class ObjectSpawnPositioner
{
    private static Dictionary<int, float> _horizontalMapping;
    private static Dictionary<int, float> _verticalMapping;

    public static float getHorizontalPosition(int lineIndex)
    {
        if (_horizontalMapping == null)
        {
            _setupMappings();
        }
        return _horizontalMapping[lineIndex];
    }

    public static float getVerticalPosition(int lineLayer)
    {
        if (_verticalMapping == null)
        {
            _setupMappings();
        }
        return _verticalMapping[lineLayer];
    }

    private static void _setupMappings()
    {
        _horizontalMapping = new Dictionary<int, float>();
        _verticalMapping = new Dictionary<int, float>();

        _verticalMapping[NoteConfig.LINE_LAYER_0] = 0.0f;
        _verticalMapping[NoteConfig.LINE_LAYER_1] = 0.4f;
        _verticalMapping[NoteConfig.LINE_LAYER_2] = 0.8f;
        _verticalMapping[NoteConfig.LINE_LAYER_3] = 1.2f;

        _horizontalMapping[NoteConfig.LINE_INDEX_0] = -0.6f;
        _horizontalMapping[NoteConfig.LINE_INDEX_1] = -0.2f;
        _horizontalMapping[NoteConfig.LINE_INDEX_2] = 0.2f;
        _horizontalMapping[NoteConfig.LINE_INDEX_3] = 0.6f;
    }
}