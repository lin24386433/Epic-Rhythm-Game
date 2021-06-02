using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreType
{
    Perfect = 500,
    Good = 350,
    Bad = 200,
    Miss = 0
}

public enum SongDifficulty
{
    Easy,
    Normal,
    Hard,
    Expert,
    Master
}

public enum Rank
{
    S,  // 96 - 100%
    A,  // 90 -  95%
    B,  // 80 -  89%
    C,  // 70 -  79%
    F   //  0 -  69%
}
