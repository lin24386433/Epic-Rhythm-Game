using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataFunctions
{
    public static List<string> GetAllSongDataNameInFile()
    {
        List<string> strs = new List<string>();

        string path = Path.Combine(Application.dataPath, "SongDatas");

        DirectoryInfo info = new DirectoryInfo(path);

        DirectoryInfo[] folders = info.GetDirectories();

        if (!Directory.Exists(path))
        {
            return null;
        }

        foreach (DirectoryInfo file in folders)
        {
            strs.Add(file.Name);
        }

        return strs;
    }
}
