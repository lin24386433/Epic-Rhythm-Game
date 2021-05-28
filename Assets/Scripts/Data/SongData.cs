using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum songDifficulty
{
    easy,
    normal,
    hard,
    expert,
    master
}

[System.Serializable]
public class SongData 
{
    public string songName;

    public float songLength;

    public songDifficulty songDifficulty = songDifficulty.easy;

    public int maxCombo;

    public int maxScore;

    // Player data
    public int playerHighCombo;

    public int playerHighScore;

    public int playTimes;

}
