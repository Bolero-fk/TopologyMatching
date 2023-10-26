namespace TopologyCardRegister
{
    /// <summary>
    /// 二値化されたセル情報を表すクラスです。
    /// </summary>
    public class MonochromeCell
    {
        /// <summary> セグメントIDが未割り当ての場合の値 </summary>
        private const int UNASSIGNED_SEGMENT_ID = -1;

        /// <summary> セルが属するセグメントのID </summary>
        public int SegmentId { get; set; }

        /// <summary> セルの色（白、黒、または未設定） </summary>
        public CellColor Color { get; set; }

        /// <summary>
        /// MonochromeCellクラスの新しいインスタンスを初期化します。
        /// </summary>
        public MonochromeCell()
        {
            this.SegmentId = UNASSIGNED_SEGMENT_ID;
            this.Color = CellColor.NONE;
        }

        /// <summary>
        /// セルの色を表す列挙型
        /// </summary>
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

        /// <summary>
        /// セルにセグメントIDが割り当てられているかどうかを確認します。
        /// </summary>
        /// <returns>セグメントIDが割り当てられていればtrue、そうでなければfalse</returns>

        public bool IsSegmentIdAssigned()
        {
            return this.SegmentId != UNASSIGNED_SEGMENT_ID;
        }

        /// <summary>
        /// セルの色が黒かどうかを確認します。
        /// </summary>
        /// <returns>セルの色が黒の場合はtrue、そうでなければfalse</returns>
        public bool IsBlack()
        {
            return this.Color == CellColor.BLACK;
        }

        /// <summary>
        /// セルの色が白かどうかを確認します。
        /// </summary>
        /// <returns>セルの色が白の場合はtrue、そうでなければfalse</returns>
        public bool IsWhite()
        {
            return this.Color == CellColor.WHITE;
        }
    }
}
