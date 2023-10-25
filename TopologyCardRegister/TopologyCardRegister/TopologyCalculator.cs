namespace TopologyCardRegister
{
    public class TopologyStatusCalculator
    {
        private const float BRIGHTNESS_THRESHOLD = 0.5f;

        /// <summary> 黒画素が画像の端にあると正しく穴の判定ができないので入力画像の余白を設定する </summary>
        private const int INPUT_IMAGE_PADDING_SIZE = 1;

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

        /// <summary>
        /// 入力された図形の各連結成分の穴の数を数えて昇順にして返します
        /// </summary>
        public static List<int> CalculateHoleCount(Bitmap bitmap)
        {
            CheckBitmapNotNull(bitmap);

            // 入力された画像を二値化したグラフに変換します
            var grid = BitmapToMonochromeGridConverter.Execute(bitmap, INPUT_IMAGE_PADDING_SIZE, BRIGHTNESS_THRESHOLD);

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

        /// <summary>
        /// 隣接するセルの色が全て別の色になっているようなセルはノイズとみなして色を変えます
        /// 以下の図の#はノイズと認識されます
        /// ...
        /// .#.
        /// ...
        /// </summary>
        private static void ChangeNoiseCellColor(Grid<MonochromeCell> grid)
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
        private static bool IsNoise(Pos pos, Grid<MonochromeCell> grid)
        {
            foreach (var nextPos in FindNextPos(pos, grid))
            {
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
        private static void AssignSegmentIdToGridCell(Grid<MonochromeCell> grid)
        {
            var segmentCount = 0;

            grid.For((h, w) =>
            {
                var checkPos = new Pos(h, w);

                // 割り振られていないマスが見つかったらそのマスと同じ色で連結している部分にidを割り振る
                if (grid[checkPos].IsSegmentIdAssigned())
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
        private static void AssignSegmentIdToSameSegmentCell(Pos startPos, int id, Grid<MonochromeCell> grid)
        {
            foreach (var pos in FindEqualSegmentPos(startPos, grid))
            {
                grid[pos].SegmentId = id;
            }
        }

        private static HashSet<Pos> FindEqualSegmentPos(Pos pos, Grid<MonochromeCell> grid)
        {
            var result = new HashSet<Pos>();

            var segmentPos = new Queue<Pos>();
            segmentPos.Enqueue(pos);

            // bfsによりposと同色の領域を全て探索する
            while (segmentPos.Count > 0)
            {
                var nowPos = segmentPos.Dequeue();

                foreach (var nextPos in FindNextPos(nowPos, grid))
                {
                    var isPassed = result.Contains(nextPos);
                    var isNotSameColor = grid[nextPos].Color != grid[nowPos].Color;

                    if (isPassed || isNotSameColor)
                    {
                        continue;
                    }

                    result.Add(nextPos);
                    segmentPos.Enqueue(nextPos);
                }
            }

            return result;
        }

        /// <summary>
        /// 各黒色成分の隣にある白成分を返します
        /// </summary>
        /// <returns>result[黒色成分のセグメントId]: キーに使われているセグメントに隣接する白色セグメントのId</returns>
        private static Dictionary<int, HashSet<int>> CalculateWhiteSegmentIdNextToBlackSegment(Grid<MonochromeCell> grid)
        {
            var whiteSegmentIds = FindBlackSegmentIds(grid).ToDictionary(blackSegmentId => blackSegmentId, _ => new HashSet<int>());

            foreach (var blackPosition in FindBlackPositions(grid))
            {
                var blackSegmentId = grid[blackPosition].SegmentId;

                foreach (var nextPos in FindNextPos(blackPosition, grid))
                {
                    // 黒色に隣接する白色マスを探すために、そうでない場合は飛ばす
                    if (grid[nextPos].IsBlack())
                    {
                        continue;
                    }

                    whiteSegmentIds[blackSegmentId].Add(/* 白色セグメントのid */ grid[nextPos].SegmentId);
                }
            }

            return whiteSegmentIds;
        }

        private static List<Pos> FindBlackPositions(Grid<MonochromeCell> grid)
        {
            var blackPositions = new List<Pos>();
            grid.For((h, w) =>
            {
                var pos = new Pos(h, w);

                if (grid[pos].IsBlack())
                {
                    blackPositions.Add(pos);
                }
            });

            return blackPositions;
        }

        private static List<int> FindBlackSegmentIds(Grid<MonochromeCell> grid)
        {
            var result = new HashSet<int>();

            grid.For((h, w) =>
            {
                if (grid[h, w].IsBlack())
                {
                    result.Add(grid[h, w].SegmentId);
                }
            });

            return result.ToList();
        }

        private static List<Pos> FindNextPos(Pos pos, Grid<MonochromeCell> grid)
        {
            var result = new List<Pos>();

            var nextDirections = grid[pos].IsBlack() ? BLACK_NEXT_DIRECTIONS : WHITE_NEXT_DIRECTIONS;
            foreach (var nextDirection in nextDirections)
            {
                var nextPos = pos + nextDirection;
                if (!grid.IsIn(nextPos))
                {
                    continue;
                }
                result.Add(nextPos);
            }

            return result;
        }
    }
}
