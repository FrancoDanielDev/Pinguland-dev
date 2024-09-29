using UnityEngine;
using System.IO;

public static class JSONLocal
{
    public static void JSONSerielization<T>(string path, string fileName, T info)
    {
        if (!Directory.Exists(path))    
            Directory.CreateDirectory(path);

        StreamWriter file = new StreamWriter(path + fileName + ".json");

        if (!File.Exists(path + fileName + ".json"))
            file = File.CreateText(path + fileName + ".json");
        else
        {
            string json = JsonUtility.ToJson(info, true);

            file.Write(json);

            file.Close();
        }

    }

    public static T JSONDesSerielization<T>(string path, string fileName, T info)
    {
        if (!File.Exists(path + fileName + ".json")) JSONSerielization<T>(path, fileName, info);

        string json = File.ReadAllText(path + fileName + ".json");

        info = JsonUtility.FromJson<T>(json);

        return info;
    }
}