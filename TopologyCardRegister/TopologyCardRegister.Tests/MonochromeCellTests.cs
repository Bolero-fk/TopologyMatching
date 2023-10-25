namespace TopologyCardRegister.Tests
{
    public class MonochromeCellTests
    {
        [Fact]
        public void DefaultConstructorTest()
        {
            var cell = new MonochromeCell();

            Assert.Equal(MonochromeCell.CellColor.NONE, cell.Color);
            Assert.False(cell.IsSegmentIdAssigned());
        }

        [Fact]
        public void InvertColorWhiteToBlackTest()
        {
            var cell = new MonochromeCell { Color = MonochromeCell.CellColor.WHITE };

            cell.InvertColor();

            Assert.Equal(MonochromeCell.CellColor.BLACK, cell.Color);
        }

        [Fact]
        public void InvertColorBlackToWhiteTest()
        {
            var cell = new MonochromeCell { Color = MonochromeCell.CellColor.BLACK };

            cell.InvertColor();

            Assert.Equal(MonochromeCell.CellColor.WHITE, cell.Color);
        }

        [Fact]
        public void InvertColorRemainsNoneTest()
        {
            var cell = new MonochromeCell { Color = MonochromeCell.CellColor.NONE };

            cell.InvertColor();

            Assert.Equal(MonochromeCell.CellColor.NONE, cell.Color);
        }

        [Fact]
        public void IsSegmentIdAssignedTest()
        {
            var cell = new MonochromeCell { SegmentId = 1 };

            Assert.True(cell.IsSegmentIdAssigned());
        }

        [Fact]
        public void IsSegmentIdAssignedWhenNotAssignedTest()
        {
            var cell = new MonochromeCell();

            Assert.False(cell.IsSegmentIdAssigned());
        }

        [Fact]
        public void IsBlackTrueTest()
        {
            var cell = new MonochromeCell { Color = MonochromeCell.CellColor.BLACK };

            Assert.True(cell.IsBlack());
        }

        [Fact]
        public void IsBlackFalseTest()
        {
            var cell = new MonochromeCell { Color = MonochromeCell.CellColor.WHITE };

            Assert.False(cell.IsBlack());
        }

        [Fact]
        public void IsWhiteTrueTest()
        {
            var cell = new MonochromeCell { Color = MonochromeCell.CellColor.WHITE };

            Assert.True(cell.IsWhite());
        }

        [Fact]
        public void IsWhiteFalseTest()
        {
            var cell = new MonochromeCell { Color = MonochromeCell.CellColor.BLACK };

            Assert.False(cell.IsWhite());
        }
    }
}
