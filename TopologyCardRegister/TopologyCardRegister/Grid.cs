namespace TopologyCardRegister
{
    /// <summary>
    /// グリッドデータを管理するクラスです。
    /// </summary>
    /// <typeparam name="TCell">セルの型です。新しいインスタンスを作成できる必要があります。</typeparam>
    public class Grid<TCell> where TCell : new()
    {
        /// <summary> グリッドの高さを表します。 </summary>
        private readonly int height;
        /// <summary> グリッドの幅を表します。 </summary>
        private readonly int width;
        /// <summary> グリッド内のセルデータを保持する二次元配列。 </summary>
        private readonly TCell[,] cells;

        /// <summary>
        /// 指定した高さと幅で新しいグリッドを初期化します。
        /// セルはデフォルトで新しいインスタンスが割り当てられます。
        /// </summary>
        /// <param name="height">グリッドの高さ。</param>
        /// <param name="width">グリッドの幅。</param>
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

        /// <summary>
        /// Pos構造体を使用して、グリッドの指定された位置のセルを取得または設定します。
        /// </summary>
        /// <param name="pos">セルの位置を示すPos構造体。</param>
        /// <returns>指定された位置のセル。</returns>
        public TCell this[Pos pos]
        {
            get => this.cells[pos.X, pos.Y];
            set => this.cells[pos.X, pos.Y] = value;
        }

        /// <summary>
        /// グリッドの指定された位置のセルを取得または設定します。
        /// </summary>
        /// <param name="h">セルの行番号 (0-indexed)。</param>
        /// <param name="w">セルの列番号 (0-indexed)。</param>
        /// <returns>指定された位置のセル。</returns>
        public TCell this[int h, int w]
        {
            get => this.cells[h, w];
            set => this.cells[h, w] = value;
        }

        /// <summary>
        /// 入力されたposがグリッド内かどうかを判定します。
        /// </summary>
        /// <param name="pos">判定対象の位置を示すPos構造体。</param>
        /// <returns>指定されたposがgrid内かどうか。</returns>
        public bool IsIn(Pos pos)
        {
            return this.IsIn(pos.X, pos.Y);
        }

        /// <summary>
        /// 入力された座標(h, w)がグリッド内かどうかを判定します。
        /// </summary>
        /// <param name="h">判定したい位置の行番号 (0-indexed)。</param>
        /// <param name="w">判定したい位置の列番号 (0-indexed)。</param>
        /// <returns>指定された座標がgrid内かどうか。</returns>
        public bool IsIn(int h, int w)
        {
            return 0 <= h && h <= this.height - 1 && 0 <= w && w <= this.width - 1;
        }

        /// <summary>
        /// グリッド内の全セルに対して入力されたfunctionを適用します。
        /// </summary>
        /// <param name="function">各セルに適用したいアクション。
        /// 関数の第一引数は行番号、第二引数は列番号です。</param>
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
