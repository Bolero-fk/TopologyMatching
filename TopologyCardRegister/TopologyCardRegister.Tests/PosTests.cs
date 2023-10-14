namespace TopologyCardRegister.Tests
{
    public class PosTests
    {
        [Fact]
        public void DefaultConstructorTest()
        {
            var pos = new Pos();

            Assert.Equal(0, pos.X);
            Assert.Equal(0, pos.Y);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(-3, 4)]
        [InlineData(0, 0)]
        public void ParameterizedConstructorTest(int x, int y)
        {
            var pos = new Pos(x, y);

            Assert.Equal(x, pos.X);
            Assert.Equal(y, pos.Y);
        }

        [Theory]
        [InlineData(1, 2, 3, 4, 4, 6)]
        [InlineData(-1, -2, 3, 4, 2, 2)]
        [InlineData(0, 0, 0, 0, 0, 0)]
        public void AddTest(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            var pos1 = new Pos(x1, y1);
            var pos2 = new Pos(x2, y2);
            var pos3 = pos1 + pos2;

            Assert.Equal(pos3.X, x3);
            Assert.Equal(pos3.Y, y3);
        }

        [Theory]
        [InlineData(1, 2, 3, 4, -2, -2)]
        [InlineData(-1, -2, 3, 4, -4, -6)]
        [InlineData(0, 0, 0, 0, 0, 0)]
        public void SubTest(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            var pos1 = new Pos(x1, y1);
            var pos2 = new Pos(x2, y2);
            var pos3 = pos1 - pos2;

            Assert.Equal(pos3.X, x3);
            Assert.Equal(pos3.Y, y3);
        }

        [Fact]
        public void StaticDirectionsTest()
        {
            Assert.Equal(0, Pos.UP.X);
            Assert.Equal(1, Pos.UP.Y);

            Assert.Equal(1, Pos.RIGHT.X);
            Assert.Equal(0, Pos.RIGHT.Y);

            Assert.Equal(0, Pos.DOWN.X);
            Assert.Equal(-1, Pos.DOWN.Y);

            Assert.Equal(-1, Pos.LEFT.X);
            Assert.Equal(0, Pos.LEFT.Y);
        }
    }
}
