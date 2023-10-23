namespace TopologyCardRegister.Tests
{
    using Xunit;
    using System.IO;
    using Newtonsoft.Json;

    public class JsonSaverTests : IDisposable
    {
        private readonly string testDir;
        private readonly string testJsonPath;

        public JsonSaverTests()
        {
            this.testDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(this.testDir);
            this.testJsonPath = Path.Combine(this.testDir, "test.json");
        }

        [Fact]
        public void SaveJsonTest()
        {
            var imageName = "test_image.png";
            var holeCount = new int[] { 1, 2, 3 };

            JsonSaver.SaveJson(this.testJsonPath, imageName, holeCount);

            Assert.True(File.Exists(this.testJsonPath));

            var savedDataJson = File.ReadAllText(this.testJsonPath);
            var savedData = JsonConvert.DeserializeObject<List<object>>(savedDataJson);

            Assert.NotNull(savedData);
            Assert.Single(savedData);

            var topologyCard = (Newtonsoft.Json.Linq.JObject)savedData[0];

            var imageNameFromCard = topologyCard["ImageName"];
            var holeCountFromCard = topologyCard["HoleCount"];

            Assert.NotNull(imageNameFromCard);
            Assert.NotNull(holeCountFromCard);

            Assert.Equal(imageName, imageNameFromCard.ToObject<string>());
            Assert.Equal(holeCount, holeCountFromCard.ToObject<int[]>());
        }

        [Fact]
        public void SaveJsonTestInvalidPathError()
        {
            Assert.Throws<ArgumentException>(() => JsonSaver.SaveJson(string.Empty, "image1", new int[] { 1, 2, 3 }));
        }

        [Fact]
        public void OverwriteSaveJsonTest()
        {
            var imageName = "test_image.svg";
            var initialHoleCount = new int[] { 1, 2, 3 };
            JsonSaver.SaveJson(this.testJsonPath, imageName, initialHoleCount);

            var updatedHoleCount = new int[] { 4, 5, 6 };
            JsonSaver.SaveJson(this.testJsonPath, imageName, updatedHoleCount);

            var savedDataJson = File.ReadAllText(this.testJsonPath);
            var savedData = JsonConvert.DeserializeObject<List<object>>(savedDataJson);

            Assert.NotNull(savedData);
            Assert.Single(savedData);

            var topologyCard = (Newtonsoft.Json.Linq.JObject)savedData[0];

            var holeCountFromCard = topologyCard["HoleCount"];
            Assert.NotNull(holeCountFromCard);
            Assert.Equal(updatedHoleCount, holeCountFromCard.ToObject<int[]>());
        }

        [Fact]
        public void OverwriteSaveToInvalidJsonTest()
        {
            var invalidSampleJson = /*lang=json*/ @"{'user': 'Alice', 'age': 29}";
            File.WriteAllText(this.testJsonPath, invalidSampleJson);

            var imageName = "test_image.svg";
            var holeCount = new int[] { 1, 2, 3 };
            JsonSaver.SaveJson(this.testJsonPath, imageName, holeCount);

            var savedDataJson = File.ReadAllText(this.testJsonPath);
            var savedData = JsonConvert.DeserializeObject<List<object>>(savedDataJson);

            Assert.NotNull(savedData);
            Assert.Single(savedData);

            var topologyCard = (Newtonsoft.Json.Linq.JObject)savedData[0];

            var holeCountFromCard = topologyCard["HoleCount"];
            Assert.NotNull(holeCountFromCard);
            Assert.Equal(holeCount, holeCountFromCard.ToObject<int[]>());
        }

        [Fact]
        public void OverwriteSaveToEmptyJsonTest()
        {
            File.WriteAllText(this.testJsonPath, "");

            var imageName = "test_image.svg";
            var holeCount = new int[] { 1, 2, 3 };
            JsonSaver.SaveJson(this.testJsonPath, imageName, holeCount);

            var savedDataJson = File.ReadAllText(this.testJsonPath);
            var savedData = JsonConvert.DeserializeObject<List<object>>(savedDataJson);

            Assert.NotNull(savedData);
            Assert.Single(savedData);

            var topologyCard = (Newtonsoft.Json.Linq.JObject)savedData[0];

            var holeCountFromCard = topologyCard["HoleCount"];
            Assert.NotNull(holeCountFromCard);
            Assert.Equal(holeCount, holeCountFromCard.ToObject<int[]>());
        }

        [Fact]
        public void SaveTwoJsonTest()
        {
            var imageNames = new string[] { "test_image1.svg", "test_image2.svg" };
            var holeCounts = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 } };

            for (var i = 0; i < imageNames.Length; i++)
            {
                JsonSaver.SaveJson(this.testJsonPath, imageNames[i], holeCounts[i]);
            }

            var savedDataJson = File.ReadAllText(this.testJsonPath);
            var savedData = JsonConvert.DeserializeObject<List<object>>(savedDataJson);

            Assert.NotNull(savedData);
            Assert.Equal(2, savedData.Count);

            for (var i = 0; i < 2; i++)
            {
                var topologyCard = (Newtonsoft.Json.Linq.JObject)savedData[i];

                var holeCountFromCard = topologyCard["HoleCount"];

                Assert.NotNull(holeCountFromCard);
                Assert.Equal(holeCounts[i], holeCountFromCard.ToObject<int[]>());
            }
        }

        public void Dispose()
        {
            if (Directory.Exists(this.testDir))
            {
                Directory.Delete(this.testDir, true);
            }
            GC.SuppressFinalize(this);
        }
    }
}
