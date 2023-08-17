using Newtonsoft.Json;

public class JsonSaver
{
    class TopologyCard
    {
        public string ImageName { get; set; }
        public int[] HoleCount { get; set; }
    }


    /// <summary>
    /// 入力されたholeCountと画像名をjsonに保存します
    /// </summary>
    static public void SaveJson(string jsonPath, string imageName, int[] holeCounts)
    {
        List<TopologyCard> topologyCards = new List<TopologyCard>();
        if (File.Exists(jsonPath))
        {
            topologyCards = LoadTopologyCardJson(jsonPath);
        }

        // 読み込んだファイルに既に同名の画像が存在する場合、holeCountを上書きする
        topologyCards.RemoveAll(x => x.ImageName == imageName);

        topologyCards.Add(new TopologyCard
        {
            ImageName = imageName,
            HoleCount = holeCounts,
        });

        File.WriteAllText(jsonPath, JsonConvert.SerializeObject(topologyCards));
    }

    /// <summary>
    /// 指定されたjsonが既に存在する場合は内容を読み込みます。
    /// </summary>
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
