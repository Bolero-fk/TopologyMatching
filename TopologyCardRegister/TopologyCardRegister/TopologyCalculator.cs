namespace TopologyCardRegister
{
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    public class TopologyStatusCalculator
    {
        /// <summary>
        /// svg画像をbitmapのグリッドに変換したデータを管理するクラスです
        /// </summary>
        private class Grid
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

        /// <summary>
        /// グリッドの座標を管理するクラスです
        /// </summary>
        public class Pos
        {
            public int X { get; }
            public int Y { get; }

            public Pos()
            {
                this.X = 0;
                this.Y = 0;
            }

            public Pos(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static Pos operator +(Pos a, Pos b)
            {
                return new Pos(a.X + b.X, a.Y + b.Y);
            }

            public static Pos operator -(Pos a, Pos b)
            {
                return new Pos(a.X - b.X, a.Y - b.Y);
            }

            public static readonly Pos UP = new Pos(0, 1);
            public static readonly Pos RIGHT = new Pos(1, 0);
            public static readonly Pos DOWN = new Pos(0, -1);
            public static readonly Pos LEFT = new Pos(-1, 0);
        }

        /*
         * 以下の図の「.」を白、「#」を黒としたときに、黒のパーツの数が1、その穴の数が1となるように
         * 黒の隣接判定は8方向、白の隣接判定は4方向にする
         * .......
         * ..###..
         * .#...#.
         * ..#.#..
         * .#...#.
         * ..###..
         * .......
         */
        private static readonly Pos[] BLACK_NEXT_DIRECTIONS = new Pos[] { Pos.UP, Pos.UP + Pos.RIGHT, Pos.RIGHT, Pos.DOWN + Pos.RIGHT, Pos.DOWN, Pos.DOWN + Pos.LEFT, Pos.LEFT, Pos.UP + Pos.LEFT };
        private static readonly Pos[] WHITE_NEXT_DIRECTIONS = new Pos[] { Pos.UP, Pos.RIGHT, Pos.DOWN, Pos.LEFT };

        // 画素の白黒を判定する際の輝度の閾値 (0に近いほど黒、1に近いほど白となる)
        private const float BRIGHTNESS_THREHOLD = 0.5f;

        /// <summary>
        /// 入力された図形の各連結成分の穴の数を数えて昇順にして返します
        /// </summary>
        public static List<int> CalculateHoleCount(Bitmap bitmap)
        {
            // 入力された画像を二値化したグラフに変換します
            var grid = ConvertToBinaryGrid(bitmap);

            ChangeNoiseCellColor(grid);

            AssignSegmentIdToGridCell(grid);

            // 各黒成分の隣にある白成分の数を数える
            // whiteSegmentIds[黒色成分のセグメントId]: キーに使われているセグメントに隣接する白色セグメントのId
            var whiteSegmentIds = CalculateWhiteSegmentIdNextToBlackSegment(grid);

            // 黒色成分の数が連結成分の数に、それに隣接する白の数-1が穴の数になる
            var holeCount = new List<int>();
            foreach (var whiteSegmentId in whiteSegmentIds.Values)
            {
                holeCount.Add(whiteSegmentId.Count - 1);
            }

            // 穴の数を昇順になるように並び変える
            holeCount.Sort();

            return holeCount;
        }

        /// <summary>
        /// 隣接するセルの色が全て別の色になっているようなセルはノイズとみなして色を変えます
        /// 以下の図の#はノイズと認識されます
        /// ...
        /// .#.
        /// ...
        /// </summary>
        private static void ChangeNoiseCellColor(Grid grid)
        {
            grid.For((h, w) =>
            {
                var checkPos = new Pos(h, w);
                if (IsNoise(checkPos, grid))
                {
                    grid[checkPos].InvertColor();
                }
            });
        }

        /// <summary>
        /// posの位置にあるセルがノイズかどうかを判定します
        /// </summary>
        private static bool IsNoise(Pos pos, Grid grid)
        {
            var nextDirections = grid[pos].Color == Grid.Cell.CellColor.BLACK ? BLACK_NEXT_DIRECTIONS : WHITE_NEXT_DIRECTIONS;

            foreach (var nextDirection in nextDirections)
            {
                var nextPos = pos + nextDirection;

                // 隣接マスが図形外の場合は飛ばす
                if (!grid.IsIn(nextPos))
                {
                    continue;
                }

                // 周囲のセルが同色ならノイズでない
                if (grid[pos].Color == grid[nextPos].Color)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// セグメントIdをグリッドのセルに割り当てます
        /// </summary>
        private static void AssignSegmentIdToGridCell(Grid grid)
        {
            var segmentCount = 0;

            grid.For((h, w) =>
            {
                var checkPos = new Pos(h, w);

                // 割り振られていないマスが見つかったらそのマスと同じ色で連結している部分にidを割り振る
                if (grid[checkPos].SegmentId != -1)
                {
                    return;
                }

                AssignSegmentIdToSameSegmentCell(checkPos, segmentCount, grid);
                segmentCount++;
            });
        }

        /// <summary>
        /// startPosの位置にあるセルと連結しているセグメントにidを割り当てます
        /// </summary>
        private static void AssignSegmentIdToSameSegmentCell(Pos startPos, int id, Grid grid)
        {
            var segmentPos = new Queue<Pos>();
            segmentPos.Enqueue(startPos);

            var nextDirections = grid[startPos].Color == Grid.Cell.CellColor.BLACK ? BLACK_NEXT_DIRECTIONS : WHITE_NEXT_DIRECTIONS;

            grid[startPos].SegmentId = id;

            // bfsによりstartPosと同色の領域を全て探索する
            while (segmentPos.Count > 0)
            {
                var nowPos = segmentPos.Dequeue();

                foreach (var nextDirection in nextDirections)
                {
                    var nextPos = nowPos + nextDirection;

                    // 隣接マスが図形外の場合は飛ばす
                    if (!grid.IsIn(nextPos))
                    {
                        continue;
                    }

                    // 隣接マスが既にidを振られている場合は飛ばす
                    if (grid[nextPos].SegmentId != -1)
                    {
                        continue;
                    }

                    // 同色の色を探すために隣接マスの色が異なる場合は飛ばす
                    if (grid[nextPos].Color != grid[nowPos].Color)
                    {
                        continue;
                    }

                    grid[nextPos].SegmentId = id;
                    segmentPos.Enqueue(nextPos);
                }
            }
        }

        /// <summary>
        /// 入力されたbitmapデータを二値化したグラフに変換します
        /// </summary>
        private static Grid ConvertToBinaryGrid(Bitmap bitmap)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;

            // bitmapの各ピクセルを取得する処理が遅いので配列に各ピクセルのRGBAを転写してそれを処理に使う
            var pixelValues = CopyBitmap(bitmap);

            var grid = new Grid(height, width);

            for (var i = 0; i < pixelValues.Length; i += 4)
            {
                var x = i / 4 % width;
                var y = i / 4 / width;

                var pixelColor = Color.FromArgb(pixelValues[i + 3], pixelValues[i + 2], pixelValues[i + 1], pixelValues[i]);
                var brightness = pixelColor.GetBrightness();

                // 閾値に基づいてピクセルを白または黒に分類します
                grid[x, y].Color = brightness < BRIGHTNESS_THREHOLD ? Grid.Cell.CellColor.BLACK : Grid.Cell.CellColor.WHITE;
            }

            return grid;
        }

        /// <summary>
        /// bitmapデータをbyte配列をコピーしたものを返します
        /// </summary>
        private static byte[] CopyBitmap(Bitmap bitmap)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;

            var data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            // RGBAの各ピクセルは4バイト
            var pixelValues = new byte[width * height * 4];
            Marshal.Copy(data.Scan0, pixelValues, 0, pixelValues.Length);

            bitmap.UnlockBits(data);

            return pixelValues;
        }

        /// <summary>
        /// 各黒色成分の隣にある白成分を返します
        /// </summary>
        /// <returns>result[黒色成分のセグメントId]: キーに使われているセグメントに隣接する白色セグメントのId</returns>
        private static Dictionary<int, HashSet<int>> CalculateWhiteSegmentIdNextToBlackSegment(Grid grid)
        {
            var whiteSegmentIds = new Dictionary<int, HashSet<int>>();
            var nextDirections = BLACK_NEXT_DIRECTIONS;

            grid.For((h, w) =>
            {
                var pos = new Pos(h, w);

                //　posが黒色の座標を探す
                if (grid[pos].Color == Grid.Cell.CellColor.WHITE)
                {
                    return;
                }

                var blackSegmentId = grid[pos].SegmentId;
                foreach (var nextDirection in nextDirections)
                {
                    var nextPos = pos + nextDirection;

                    // 隣接マスが図形外の場合は飛ばす
                    if (!grid.IsIn(nextPos))
                    {
                        continue;
                    }

                    // 黒色に隣接する白色マスを探すために、そうでない場合は飛ばす
                    if (grid[nextPos].Color == Grid.Cell.CellColor.BLACK)
                    {
                        continue;
                    }

                    // whiteSegmentIdsがblackSegmentIdをキーとして持たない場合はキーを追加する
                    if (!whiteSegmentIds.ContainsKey(blackSegmentId))
                    {
                        whiteSegmentIds.Add(blackSegmentId, new HashSet<int>());
                    }

                    whiteSegmentIds[blackSegmentId].Add(/* 白色セグメントのid */ grid[nextPos].SegmentId);
                }
            });

            return whiteSegmentIds;
        }
    }
}
