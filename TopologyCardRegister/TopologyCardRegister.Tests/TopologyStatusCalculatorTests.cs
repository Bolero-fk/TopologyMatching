namespace TopologyCardRegister.Tests
{
    using System.Collections.Generic;
    using System.Drawing;
    using TopologyCardRegister;

    public class TopologyStatusCalculatorTests
    {
        private static readonly string ProjectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
        private static readonly string TestcaseDirectory = ProjectDirectory + "/TopologyStatusCalculatorTestCase";

        [Fact]
        public void InputNullBitmapError()
        {
            Bitmap nullBitmap = null;

            Assert.Throws<ArgumentNullException>(() => TopologyStatusCalculator.Execute(nullBitmap));
        }

        [Theory]
        [InlineData("/TestCase1.in", "/TestCase1.out")] // 全て白色
        [InlineData("/TestCase2.in", "/TestCase2.out")] // 全て黒色
        [InlineData("/TestCase3.in", "/TestCase3.out")] // 白い1ドットがノイズとして判定される
        [InlineData("/TestCase4.in", "/TestCase4.out")] // 黒い1ドットがノイズとして判定される
        [InlineData("/TestCase5.in", "/TestCase5.out")] // 一つの図形に穴一つ
        [InlineData("/TestCase6.in", "/TestCase6.out")] // 一つの図形に穴二つ
        [InlineData("/TestCase7.in", "/TestCase7.out")] // 図形の外に図形
        [InlineData("/TestCase8.in", "/TestCase8.out")] // 図形の中に図形
        public void HandmadeSampleTest(string inputPath, string expectResultPath)
        {
            var inputLines = File.ReadAllLines(TestcaseDirectory + inputPath);
            var bitmap = CreateBitmapFromStrings(inputLines);

            var result = TopologyStatusCalculator.Execute(bitmap);

            var expectResult = ConvertStringToList(File.ReadAllText(TestcaseDirectory + expectResultPath));

            Assert.NotNull(result);
            Assert.Equal(expectResult, result);
        }

        private static Bitmap CreateBitmapFromStrings(string[] lines)
        {
            var width = lines[0].Length;
            var height = lines.Length;

            var bitmap = new Bitmap(width, height);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var currentChar = lines[y][x];
                    if (currentChar == '.')
                    {
                        bitmap.SetPixel(x, y, Color.White);
                    }
                    else if (currentChar == '#')
                    {
                        bitmap.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return bitmap;
        }

        private static List<int> ConvertStringToList(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<int>();
            }

            return new List<int>(Array.ConvertAll(input.Split(' '), int.Parse));
        }
    }
}
