using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo 
{
    public static string songName = DataFunctions.GetAllSongDataNameInFile()[0];

    public static int indexOfAllSongs;

    public static Sprite songImg;

    public static int gameScore = 146000;

    public static int gameCombo = 100;

    public static int perfectCount = 100;

    public static int goodCount = 10;

    public static int badCount = 10;

    public static int missCount = 10;

}
