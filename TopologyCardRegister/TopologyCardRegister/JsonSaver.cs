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
            string existingJson = File.ReadAllText(jsonPath);
            List<TopologyCard>? readData = JsonConvert.DeserializeObject<List<TopologyCard>>(existingJson);

            if (readData != null)
                topologyCards = readData;
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
}
