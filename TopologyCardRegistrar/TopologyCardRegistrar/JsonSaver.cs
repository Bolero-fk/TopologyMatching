using System;
using Newtonsoft.Json;

public class JsonSaver
{
    public JsonSaver()
    {
    }

    static public void SaveJson(string jsonPath, string imgName, int[] holeCounts)
    {
        var dataToWrite = new
        {
            ImageName = imgName,
            HoleCount = holeCounts,
        };

        string jsonOutput = JsonConvert.SerializeObject(dataToWrite);

        File.WriteAllText(jsonPath, jsonOutput);
    }
}
