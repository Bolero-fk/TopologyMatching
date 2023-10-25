namespace TopologyCardRegister
{
    public class MonochromeCell
    {
        private const int UNASSIGNED_SEGMENT_ID = -1;

        public int SegmentId { get; set; }
        public CellColor Color { get; set; }

        public MonochromeCell()
        {
            this.SegmentId = UNASSIGNED_SEGMENT_ID;
            this.Color = CellColor.NONE;
        }

        public enum CellColor
        {
            WHITE,
            BLACK,
            NONE
        }

        /// <summary>
        /// セルに設定されている色を反転します。
        /// 設定されている色がNONEになっている場合はなにも実行されません
        /// </summary>
        public void InvertColor()
        {
            if (this.Color == CellColor.NONE)
            {
                return;
            }

            this.Color = this.Color == CellColor.WHITE ? CellColor.BLACK : CellColor.WHITE;
        }

        public bool IsSegmentIdAssigned()
        {
            return this.SegmentId != UNASSIGNED_SEGMENT_ID;
        }
    }
}
