namespace TopologyCardRegister.Tests
{
    using Xunit;
    using System.IO;
    using Newtonsoft.Json.Linq;

    public class TopologyCardTests
    {
        private readonly string outputSvgFolderPath;
        private readonly string testDirectory;
        private readonly string testJsonPath;

        private static readonly string PROJECT_DIRECTORY = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
        private static readonly string TESTCASE_DIRECTORY = PROJECT_DIRECTORY + "/TopologyCardTestCase";

        public TopologyCardTests()
        {
            this.testDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(this.testDirectory);

            this.testJsonPath = Path.Combine(this.testDirectory, "test.json");

            this.outputSvgFolderPath = Path.Combine(this.testDirectory, "svgOutput");
            Directory.CreateDirectory(this.outputSvgFolderPath);
        }

        [Fact]
        public void DefaultConstructorTest()
        {
            var card = new TopologyCard();

            Assert.Equal(string.Empty, card.SvgFilePath);
            Assert.NotNull(card.SvgImage);
            Assert.Empty(card.HoleCounts);
        }

        [Theory]
        [InlineData("TestCase1.in")]
        public void ParameterizedConstructorTest(string svgFileName)
        {
            var svgFilePath = Path.Combine(TESTCASE_DIRECTORY, svgFileName);
            var card = new TopologyCard(svgFilePath);

            Assert.Equal(svgFilePath, card.SvgFilePath);
            Assert.NotNull(card.SvgImage);
        }

        [Theory]
        [InlineData("TestCase1.in")]
        public void SaveCardTest(string inputFileName)
        {
            var inputFilePath = Path.Combine(TESTCASE_DIRECTORY, inputFileName);
            var card = new TopologyCard(inputFilePath);

            var expectedSvgPath = Path.Combine(this.outputSvgFolderPath, Path.GetFileName(inputFileName));

            if (File.Exists(expectedSvgPath))
            {
                File.Delete(expectedSvgPath);
            }

            if (File.Exists(this.testJsonPath))
            {
                File.Delete(this.testJsonPath);
            }

            card.Save(this.outputSvgFolderPath, this.testJsonPath);

            Assert.True(File.Exists(expectedSvgPath));
            Assert.True(File.Exists(this.testJsonPath));
        }

        [Theory]
        [InlineData("TestCase1.in", "TestCase1.out")] //「@」一つの穴を持つ図形が一つ
        [InlineData("TestCase2.in", "TestCase2.out")] //「ᕯ」穴なしの図形が二つ
        [InlineData("TestCase3.in", "TestCase3.out")] //「3」穴なしの図形が一つ
        public void VerifySavedJsonContent(string inputFileName, string expectResultFileName)
        {
            if (File.Exists(this.testJsonPath))
            {
                File.Delete(this.testJsonPath);
            }

            var inputFilePath = Path.Combine(TESTCASE_DIRECTORY, inputFileName);
            var expectResultFilePath = Path.Combine(TESTCASE_DIRECTORY, expectResultFileName);

            var card = new TopologyCard(inputFilePath);
            card.Save(this.outputSvgFolderPath, this.testJsonPath);

            var actualJsonContent = File.ReadAllText(this.testJsonPath);
            var expectedJsonContent = File.ReadAllText(expectResultFilePath);

            var actualJObject = JArray.Parse(actualJsonContent);
            var expectedJObject = JArray.Parse(expectedJsonContent);

            Assert.True(JToken.DeepEquals(actualJObject, expectedJObject));
        }
    }
}
