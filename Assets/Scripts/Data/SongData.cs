using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongData
{
    public string songName;

    public float songLength;

    public SongDifficulty songDifficulty;

    public int maxCombo;

    public int maxScore;

    // Player data
    public int playerHighCombo;

    public int playerHighScore;

    public int playTimes;

    public SongData()
    {
        songName = "";
        songLength = 0f;
        songDifficulty = SongDifficulty.Easy;
        maxCombo = 0;
        maxScore = 0;
        playerHighCombo = 0;
        playerHighScore = 0;
        playTimes = 0;
    }

}
