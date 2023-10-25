namespace TopologyCardRegister
{
    /// <summary>
    /// 入力されたbitmapの穴の数を計算するクラスです。
    /// </summary>
    public class TopologyStatusCalculator
    {
        /// <summary> 入力された画像の白黒を判別する際の輝度の閾値 </summary>
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
        /// 入力された図形の各連結成分の穴の数を数えて昇順にして返します。
        /// </summary>
        /// <param name="bitmap">入力される図形を表すビットマップ。</param>
        /// <returns>各連結成分の穴の数のリストを昇順で返します。</returns>
        public static List<int> Execute(Bitmap bitmap)
        {
            CheckBitmapNotNull(bitmap);

            var grid = BitmapToMonochromeGridConverter.Execute(bitmap, INPUT_IMAGE_PADDING_SIZE, BRIGHTNESS_THRESHOLD);

            ChangeNoiseCellColor(grid);

            AssignSegmentIdToGridCell(grid);

            var holeCount = CalculateHoleCounts(grid);

            return holeCount;
        }

        /// <summary>
        /// 指定されたグリッドに基づいて各黒色成分の隣接する白色成分の数から穴の数を計算し、その結果を昇順に並べたリストとして返します。
        /// </summary>
        /// <param name="grid">MonochromeCellを要素とするグリッド。</param>
        /// <returns>各黒色成分に隣接する白色成分の数から算出した穴の数のリスト。このリストは昇順にソートされています。</returns>

        private static List<int> CalculateHoleCounts(Grid<MonochromeCell> grid)
        {
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
        /// 与えられたBitmapオブジェクトがnullでないことを確認します。nullの場合、ArgumentNullExceptionをスローします。
        /// </summary>
        /// <param name="bitmap">確認対象のBitmapオブジェクト。</param>
        /// <exception cref="ArgumentNullException">Bitmapオブジェクトがnullの場合にスローされます。</exception>

        private static void CheckBitmapNotNull(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap), "Bitmap object should not be null.");
            }
        }

        /// <summary>
        /// グリッド内の各セルを調査して、隣接するセルの色が全て異なるセル（ノイズとして認識されるセル）を見つけ、その色を反転します。
        /// 例えば、ノイズとして認識されるセルは以下の図の#のようなものです。
        /// ...
        /// .#.
        /// ...
        /// </summary>
        /// <param name="grid">MonochromeCellのグリッド。このグリッド内のセルがノイズであるかどうかを判定し、必要に応じて色を反転させます。</param>

        private static void ChangeNoiseCellColor(Grid<MonochromeCell> grid)
        {
            foreach (var noiseCell in FindNoiseCell(grid))
            {
                noiseCell.InvertColor();
            }
        }

        /// <summary>
        /// グリッド内のノイズとして認識されるセルを探索し、それらのセルのリストを返します。
        /// ノイズセルは、その周囲のすべてのセルが異なる色であるセルとして定義されます。
        /// </summary>
        /// <param name="grid">MonochromeCellのグリッド。このグリッド内のセルがノイズであるかどうかを判定します。</param>
        /// <returns>ノイズとして認識されるセルのリストを返します。</returns>

        private static List<MonochromeCell> FindNoiseCell(Grid<MonochromeCell> grid)
        {
            var result = new List<MonochromeCell>();

            grid.For((h, w) =>
            {
                var checkPos = new Pos(h, w);
                if (IsNoise(checkPos, grid))
                {
                    result.Add(grid[checkPos]);
                }
            });

            return result;
        }

        /// <summary>
        /// 指定された位置のセルがノイズであるかを判定します。
        /// ノイズセルは、その周囲のすべてのセルが異なる色であるセルとして定義されます。
        /// </summary>
        /// <param name="pos">ノイズであるかどうかを判定したいセルの位置。</param>
        /// <param name="grid">MonochromeCellのグリッド。このグリッド内のセルがノイズであるかどうかを判定します。</param>
        /// <returns>指定された位置のセルがノイズであればtrue、それ以外の場合はfalseを返します。</returns>

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
        /// グリッド内のすべてのセルにセグメントIDを割り当てます。
        /// 未割り当てのセルが見つかった場合、そのセルと同じ色で連結しているすべてのセルに新しいセグメントIDを割り当てます。
        /// </summary>
        /// <param name="grid">MonochromeCellのグリッド。このグリッド内のセルにセグメントIDを割り当てます。</param>
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
        /// 指定された位置のセルと連結しているすべてのセルに、指定されたIDを割り当てます。
        /// </summary>
        /// <param name="startPos">セグメントの基準となる位置を指定するセル。</param>
        /// <param name="id">連結しているセグメントに割り当てるID。</param>
        /// <param name="grid">MonochromeCellのグリッド。このグリッド内のセルにセグメントIDを割り当てます。</param>
        private static void AssignSegmentIdToSameSegmentCell(Pos startPos, int id, Grid<MonochromeCell> grid)
        {
            foreach (var pos in FindEqualSegmentPos(startPos, grid))
            {
                grid[pos].SegmentId = id;
            }
        }

        /// <summary>
        /// 指定された位置のセルと同じ色を持つ、連結しているすべてのセルの位置を探索し、HashSetとして返します。
        /// </summary>
        /// <param name="pos">探索を開始するセルの位置。</param>
        /// <param name="grid">MonochromeCellのグリッド。</param>
        /// <returns>指定された位置のセルと同じ色を持ち、連結しているすべてのセルの位置のHashSet。</returns>

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
        /// 黒色成分と隣接する白色成分のセグメントIdを計算します。
        /// </summary>
        /// <param name="grid">MonochromeCellのグリッド。</param>
        /// <returns>
        /// Dictionaryのキーは黒色成分のセグメントId、値はそのキーのセグメントに隣接する白色セグメントのIdのHashSet。
        /// </returns>
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

        /// <summary>
        /// グリッド内のすべての黒色セルの位置をリストとして返します。
        /// </summary>
        /// <param name="grid">MonochromeCellのグリッド。</param>
        /// <returns>グリッド内の黒色セルの位置を含むリスト。</returns>
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

        /// <summary>
        /// グリッド内のすべての黒色セグメントのIDをリストとして返します。
        /// </summary>
        /// <param name="grid">MonochromeCellのグリッド。</param>
        /// <returns>グリッド内の黒色セグメントのIDを含むリスト。</returns>

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

        /// <summary>
        /// 指定された位置から次に移動可能な位置を検索します。
        /// </summary>
        /// <param name="pos">現在の位置</param>
        /// <param name="grid">MonochromeCellのグリッド。</param>
        /// <returns>指定された位置から次に移動可能な位置のリスト。</returns>
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
