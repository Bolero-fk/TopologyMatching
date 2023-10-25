namespace TopologyCardRegister.Tests
{
    using System.Drawing;

    public class BitmapToMonochromeGridConverterTests
    {
        [Theory]
        [InlineData(10, 10)]
        [InlineData(5, 10)]
        [InlineData(10, 5)]
        public void GridConverterTest(int width, int height)
        {
            var brightnessThreshold = 0.5f;

            var bitmap = InitializeNettingBitmap(width, height);

            var grid = BitmapToMonochromeGridConverter.Execute(bitmap, 0, brightnessThreshold);

            grid.For((h, w) =>
            {
                if ((h + w) % 2 == 0)
                {
                    Assert.Equal(MonochromeCell.CellColor.BLACK, grid[h, w].Color);
                }
                else
                {
                    Assert.Equal(MonochromeCell.CellColor.WHITE, grid[h, w].Color);
                }
            });

        }

        [Theory]
        [InlineData(10, 10, 10)]
        [InlineData(5, 10, 10)]
        [InlineData(10, 5, 10)]
        public void GridPaddingTest(int width, int height, int paddingSize)
        {
            var brightnessThreshold = 0.5f;

            var bitmap = InitializeAllBlackBitmap(width, height);

            var grid = BitmapToMonochromeGridConverter.Execute(bitmap, paddingSize, brightnessThreshold);

            grid.For((h, w) =>
            {
                var isPadding = h < paddingSize || height + paddingSize - 1 < h || w < paddingSize || width + paddingSize - 1 < w;

                if (isPadding)
                {
                    Assert.Equal(MonochromeCell.CellColor.WHITE, grid[h, w].Color);
                }
                else
                {
                    Assert.Equal(MonochromeCell.CellColor.BLACK, grid[h, w].Color);
                }
            });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        public void NegaticePaddingTestError(int paddingSize)
        {
            var bitmap = new Bitmap(3, 3);

            Assert.Throws<ArgumentOutOfRangeException>(() => BitmapToMonochromeGridConverter.Execute(bitmap, paddingSize, 0.5f));
        }

        [Theory]
        [InlineData(1.1f)]
        [InlineData(-0.1f)]
        public void InvalidBrightnessThresholdTestError(float threshold)
        {
            var bitmap = new Bitmap(3, 3);

            Assert.Throws<ArgumentOutOfRangeException>(() => BitmapToMonochromeGridConverter.Execute(bitmap, 1, threshold));
        }

        private static Bitmap InitializeAllBlackBitmap(int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    bitmap.SetPixel(x, y, Color.Black);
                }
            }

            return bitmap;
        }

        private static Bitmap InitializeNettingBitmap(int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    bitmap.SetPixel(x, y, (x + y) % 2 == 0 ? Color.Black : Color.White);
                }
            }

            return bitmap;
        }
    }
}
