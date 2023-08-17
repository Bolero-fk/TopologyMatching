using Newtonsoft.Json;

public class JsonSaver
{
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

        topologyCards.Add(new TopologyCard
        {
            ImageName = imageName,
            HoleCount = holeCounts,
        });

        File.WriteAllText(jsonPath, JsonConvert.SerializeObject(topologyCards));
    }

    static List<TopologyCard> LoadTopologyCardJson(string jsonPath)
    {
        try
        {
            List<TopologyCard>? readData = JsonConvert.DeserializeObject<List<TopologyCard>>(File.ReadAllText(jsonPath));

            if (readData != null)
            {
                return readData;
            }
        }
        catch (Newtonsoft.Json.JsonSerializationException)
        {
            return new List<TopologyCard>();
        }

        return new List<TopologyCard>();
    }
}
