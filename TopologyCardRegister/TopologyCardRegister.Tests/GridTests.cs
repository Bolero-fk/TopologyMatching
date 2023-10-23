namespace TopologyCardRegister.Tests
{
    using Xunit;

    public class GridTests
    {
        [Fact]
        public void ConstructorTest()
        {
            var grid = new Grid<int>(2, 3);

            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    Assert.Equal(0, grid[i, j]);
                }
            }
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(100, -100)]
        public void ConstructorTestNegativeDimensionsError(int h, int w)
        {
            Assert.Throws<OverflowException>(() => new Grid<int>(h, w));
        }

        [Fact]
        public void PosGetAndSetUsingPosTest()
        {
            var grid = new Grid<int>(2, 2);
            var pos = new Pos(1, 1);

            grid[pos] = 1;

            Assert.Equal(1, grid[pos]);
        }

        [Theory]
        [InlineData(10, 10, 9, 10)]
        [InlineData(10, 10, -1, 0)]
        public void PosGetAndSetUsingPosTestOutOfRangeError(int gridHeight, int gridWidth, int h, int w)
        {
            var grid = new Grid<int>(gridHeight, gridWidth);
            var pos = new Pos(h, w);

            Assert.Throws<IndexOutOfRangeException>(() => { var temp = grid[new Pos(h, w)]; });
            Assert.Throws<IndexOutOfRangeException>(() => grid[new Pos(h, w)] = 1);
        }


        [Fact]
        public void PosGetAndSetUsingCoordinatesTest()
        {
            var grid = new Grid<int>(2, 2);

            grid[1, 1] = 42;

            Assert.Equal(42, grid[1, 1]);
        }

        [Theory]
        [InlineData(10, 10, 9, 10)]
        [InlineData(10, 10, -1, 0)]
        public void PosGetAndSetUsingCoordinatesTestOutOfRangeError(int gridHeight, int gridWidth, int h, int w)
        {
            var grid = new Grid<int>(gridHeight, gridWidth);
            var pos = new Pos(h, w);

            Assert.Throws<IndexOutOfRangeException>(() => { var temp = grid[h, w]; });
            Assert.Throws<IndexOutOfRangeException>(() => grid[h, w] = 1);
        }


        [Theory]
        [InlineData(2, 2, 0, 0, true)]
        [InlineData(2, 2, 1, 1, true)]
        [InlineData(2, 2, 2, 2, false)]
        [InlineData(2, 2, -1, 0, false)]
        [InlineData(2, 2, 0, -1, false)]
        public void IsInUsingPosTest(int h, int w, int x, int y, bool expected)
        {
            var grid = new Grid<int>(h, w);
            var pos = new Pos(x, y);

            var result = grid.IsIn(pos);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2, 2, 0, 0, true)]
        [InlineData(2, 2, 1, 1, true)]
        [InlineData(2, 2, 2, 2, false)]
        [InlineData(2, 2, -1, 0, false)]
        [InlineData(2, 2, 0, -1, false)]
        public void IsInUsingCoordinatesTest(int h, int w, int x, int y, bool expected)
        {
            var grid = new Grid<int>(h, w);

            var result = grid.IsIn(x, y);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2, 3, 6)]
        public void ForTest(int h, int w, int expected)
        {
            var grid = new Grid<int>(h, w);
            var cellCount = 0;

            grid.For((h, w) => cellCount++);

            Assert.Equal(expected, cellCount);
        }
    }
}
