namespace TopologyCardRegister
{
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    public class TopologyStatusCalculator
    {
        private class Cell
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

        /// <summary> 黒画素が画像の端にあると正しく穴の判定ができないので入力画像の余白を設定する </summary>
        private const int INPUT_IMAGE_PADDING_SIZE = 1;

        /// <summary>
        /// 入力された図形の各連結成分の穴の数を数えて昇順にして返します
        /// </summary>
        public static List<int> CalculateHoleCount(Bitmap bitmap)
        {
            ValidateBitmap(bitmap);

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

        private static void CheckBitmapNotNull(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap), "Bitmap object should not be null.");
            }
        }

        private static void ValidateBitmap(Bitmap bitmap)
        {
            CheckBitmapNotNull(bitmap);
        }

        /// <summary>
        /// 隣接するセルの色が全て別の色になっているようなセルはノイズとみなして色を変えます
        /// 以下の図の#はノイズと認識されます
        /// ...
        /// .#.
        /// ...
        /// </summary>
        private static void ChangeNoiseCellColor(Grid<Cell> grid)
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
        private static bool IsNoise(Pos pos, Grid<Cell> grid)
        {
            var nextDirections = grid[pos].Color == Cell.CellColor.BLACK ? BLACK_NEXT_DIRECTIONS : WHITE_NEXT_DIRECTIONS;

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
        private static void AssignSegmentIdToGridCell(Grid<Cell> grid)
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
        private static void AssignSegmentIdToSameSegmentCell(Pos startPos, int id, Grid<Cell> grid)
        {
            var segmentPos = new Queue<Pos>();
            segmentPos.Enqueue(startPos);

            var nextDirections = grid[startPos].Color == Cell.CellColor.BLACK ? BLACK_NEXT_DIRECTIONS : WHITE_NEXT_DIRECTIONS;

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
        private static Grid<Cell> ConvertToBinaryGrid(Bitmap bitmap)
        {
            var bitmapWithPadding = AddPadding(bitmap, INPUT_IMAGE_PADDING_SIZE);
            var width = bitmapWithPadding.Width;
            var height = bitmapWithPadding.Height;

            // bitmapの各ピクセルを取得する処理が遅いので配列に各ピクセルのRGBAを転写してそれを処理に使う
            var pixelValues = CopyBitmap(bitmapWithPadding);

            var grid = new Grid<Cell>(height, width);

            for (var i = 0; i < pixelValues.Length; i += 4)
            {
                var w = i / 4 % width;
                var h = i / 4 / width;

                var pixelColor = Color.FromArgb(pixelValues[i + 3], pixelValues[i + 2], pixelValues[i + 1], pixelValues[i]);
                var brightness = pixelColor.GetBrightness();

                // 閾値に基づいてピクセルを白または黒に分類します
                grid[h, w].Color = brightness < BRIGHTNESS_THREHOLD ? Cell.CellColor.BLACK : Cell.CellColor.WHITE;
            }

            return grid;
        }

        /// <summary>
        /// 入力されたbitmapの周囲にpaddingSizeの余白を追加します
        /// </summary>
        private static Bitmap AddPadding(Bitmap original, int paddingSize)
        {
            // 新しいBitmapのサイズを計算
            var newWidth = original.Width + (2 * paddingSize);
            var newHeight = original.Height + (2 * paddingSize);

            // 新しいBitmapを作成
            var paddedBitmap = new Bitmap(newWidth, newHeight);

            using (var g = Graphics.FromImage(paddedBitmap))
            {
                // 背景を白色で塗りつぶす
                g.Clear(Color.White);

                // 元のBitmapを新しい位置に描画
                g.DrawImage(original, paddingSize, paddingSize);
            }

            return paddedBitmap;
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
        private static Dictionary<int, HashSet<int>> CalculateWhiteSegmentIdNextToBlackSegment(Grid<Cell> grid)
        {
            var whiteSegmentIds = new Dictionary<int, HashSet<int>>();
            var nextDirections = BLACK_NEXT_DIRECTIONS;

            grid.For((h, w) =>
            {
                var pos = new Pos(h, w);

                //　posが黒色の座標を探す
                if (grid[pos].Color == Cell.CellColor.WHITE)
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
                    if (grid[nextPos].Color == Cell.CellColor.BLACK)
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
