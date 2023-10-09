namespace TopologyCardRegister
{
    /// <summary>
    /// svg画像をbitmapのグリッドに変換したデータを管理するクラスです
    /// </summary>
    public class Grid<TCell> where TCell : new()
    {
        private readonly int height;
        private readonly int width;
        private readonly TCell[,] cells;

        public Grid(int height, int width)
        {
            this.height = height;
            this.width = width;
            this.cells = new TCell[height, width];
            for (var h = 0; h < height; h++)
            {
                for (var w = 0; w < width; w++)
                {
                    this.cells[h, w] = new TCell();
                }
            }
        }

        public TCell this[Pos pos]
        {
            get => this.cells[pos.X, pos.Y];
            set => this.cells[pos.X, pos.Y] = value;
        }

        public TCell this[int h, int w]
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