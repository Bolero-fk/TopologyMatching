using System;
using Newtonsoft.Json;

public class JsonSaver
{
    public JsonSaver()
    {
    }

    class TopologyCard
    {
        public string ImageName { get; set; }
        public int[] HoleCount { get; set; }
    }


    static public void SaveJson(string jsonPath, string imageName, int[] holeCounts)
    {
        List<TopologyCard> topologyCards = new List<TopologyCard>();
        if (File.Exists(jsonPath))
        {
            topologyCards = LoadTopologyCardJson(jsonPath);
        }

        topologyCards.RemoveAll(x => x.ImageName == imageName);

        var topologyCard = new TopologyCard
        {
            ImageName = imageName,
            HoleCount = holeCounts,
        };
        topologyCards.Add(topologyCard);

        string jsonOutput = JsonConvert.SerializeObject(topologyCards);

        File.WriteAllText(jsonPath, jsonOutput);
    }

    static List<TopologyCard> LoadTopologyCardJson(string jsonPath)
    {
        try
        {
            List<TopologyCard>? readData = JsonConvert.DeserializeObject<List<TopologyCard>>(File.ReadAllText(jsonPath));

            if (readData != null)
                return readData;
        }
        catch (Newtonsoft.Json.JsonSerializationException)
        {
            return new List<TopologyCard>();
        }

        return new List<TopologyCard>();
    }
}
