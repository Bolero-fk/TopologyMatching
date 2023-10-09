namespace TopologyCardRegister
{
    /// <summary>
    /// svg画像をbitmapのグリッドに変換したデータを管理するクラスです
    /// </summary>
    public class Grid
    {
        public class Cell
        {
            public int SegmentId { get; set; }
            public CellColor Color { get; set; }

            public Cell()
            {
                this.SegmentId = -1;
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
        }

        private readonly int height;
        private readonly int width;
        private readonly Cell[,] cells;

        public Grid(int height, int width)
        {
            this.height = height;
            this.width = width;
            this.cells = new Cell[height, width];
            for (var h = 0; h < height; h++)
            {
                for (var w = 0; w < width; w++)
                {
                    this.cells[h, w] = new Cell();
                }
            }
        }

        public Cell this[Pos pos]
        {
            get => this.cells[pos.X, pos.Y];
            set => this.cells[pos.X, pos.Y] = value;
        }

        public Cell this[int h, int w]
        {
            get => this.cells[h, w];
            set => this.cells[h, w] = value;
        }

        /// <summary>
        /// 入力されたposがグリッド内かどうかを判定します
        /// </summary>
        public bool IsIn(Pos pos)
        {
            return this.IsIn(pos.X, pos.Y);
        }

        /// <summary>
        /// 入力された座標(h, w)がグリッド内かどうかを判定します
        /// </summary>
        public bool IsIn(int h, int w)
        {
            return 0 <= h && h <= this.height - 1 && 0 <= w && w <= this.width - 1;
        }

        /// <summary>
        /// グリッド内の全セルに対して入力されたfunctionを適用します
        /// </summary>
        public void For(Action<int, int> function)
        {
            for (var h = 0; h < this.height; h++)
            {
                for (var w = 0; w < this.width; w++)
                {
                    function(h, w);
                }
            }
        }
    }
}