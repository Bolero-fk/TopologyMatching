namespace TopologyCardRegister
{
    using Newtonsoft.Json;

    public class JsonSaver
    {
        /// <summary>
        /// Json出力用のトポロジカードの情報を保持するクラス。
        /// </summary>
        private class TopologyCard
        {
            /// <summary> json先に保存する画像の名前。 </summary>
            public string ImageName { get; set; }
            /// <summary> 保存する画像のホールの数。 </summary>
            public int[] HoleCount { get; set; }

            /// <summary>
            /// Json出力用のトポロジカードの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="imageName">画像の名前。</param>
            /// <param name="holeCount">ホールの数。</param>
            public TopologyCard(string imageName, int[] holeCount)
            {
                this.ImageName = imageName;
                this.HoleCount = holeCount;
            }
        }

        /// <summary>
        /// 入力されたholeCountと画像名をjsonに保存します。
        /// 既存の同名の画像が存在する場合、holeCountを上書きします。
        /// </summary>
        /// <param name="jsonPath">保存先のjsonファイルのパス。</param>
        /// <param name="imageName">保存する画像の名前。</param>
        /// <param name="holeCount">保存するホールの数。</param>
        public static void SaveJson(string jsonPath, string imageName, int[] holeCount)
        {
            var topologyCards = new List<TopologyCard>();
            if (File.Exists(jsonPath))
            {
                topologyCards = LoadTopologyCardJson(jsonPath);
            }

            // 読み込んだファイルに既に同名の画像が存在する場合、holeCountを上書きする
            topologyCards.RemoveAll(x => x.ImageName == imageName);

            topologyCards.Add(new TopologyCard(imageName, holeCount));

            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(topologyCards));
        }

        /// <summary>
        /// 指定されたJSONファイルのパスからトポロジーカードのリストをロードします。
        /// ファイルが存在し、正しくデシリアライズできる場合はその内容を返します。
        /// それ以外の場合は空のリストを返します。
        /// </summary>
        /// <param name="jsonPath">トポロジーカードのリストを含むJSONファイルのパス。</param>
        /// <returns>JSONファイルからロードしたトポロジーカードのリスト。ファイルが存在しない、または読み込みに失敗した場合は空のリスト。</returns>
        private static List<TopologyCard> LoadTopologyCardJson(string jsonPath)
        {
            try
            {
                var readData = JsonConvert.DeserializeObject<List<TopologyCard>>(File.ReadAllText(jsonPath));

                if (readData != null)
                {
                    return readData;
                }
            }
            catch (JsonSerializationException)
            {
                return new List<TopologyCard>();
            }

            return new List<TopologyCard>();
        }
    }
}
