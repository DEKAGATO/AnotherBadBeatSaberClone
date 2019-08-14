﻿using UnityEngine;
using UnityEditor;

public class JsonHighscoreStringBuilder
{
    private FastList<HighscoreData> _highscoreData;
    private string _json;

    public void setData(FastList<HighscoreData> highscoreData)
    {
        _highscoreData = highscoreData;
    }

    public string getJsonString()
    {
        _json = "{\"_highscores\": [";
        string score = "\"_score\":";
        string rank = "\"_rank\":";

        HighscoreData highscore;
        bool isLast;
        string temp;

        for (int i = 0; i < _highscoreData.Count; i++)
        {
            highscore = _highscoreData[i];
            isLast = i >= _highscoreData.Count - 1;

            temp = score + highscore.score.ToString() + ",";
            temp += rank + highscore.rank.ToString();
            temp = _addBrackets(temp, isLast);
            _json += temp;
        }
        _json += "]}";
        return _json;
    }

    private string _addBrackets(string json, bool isLast = false)
    {
        json = "{" + json + "}";
        return isLast ? json : json + ",";
    }
}