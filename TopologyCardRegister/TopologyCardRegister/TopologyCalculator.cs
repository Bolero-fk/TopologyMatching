using System;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TopologyCardRegister
{
    public class TopologyStatusCalculator
    {
        /// <summary>
        /// svg画像をbitmapのグリッドに変換したデータを管理するクラスです
        /// </summary>
        class Grid
        {
            public class Cell
            {
                public int m_segmentId { get; set; }
                public CellColor m_color { get; set; }

                public Cell()
                {
                    m_segmentId = -1;
                    m_color = CellColor.NONE;
                }

                public enum CellColor
                {
                    WHITE,
                    BLACK,
                    NONE
                }

                public void InvertColor()
                {
                    if (m_color == CellColor.WHITE)
                        m_color = CellColor.BLACK;
                    else if (m_color == CellColor.BLACK)
                        m_color = CellColor.WHITE;
                }
            }

            int m_height;
            int m_width;

            Cell[,] m_cells;

            public Grid(int height, int width)
            {
                m_height = height;
                m_width = width;
                m_cells = new Cell[height, width];
                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        m_cells[h, w] = new Cell();
                    }
                }
            }

            public Cell this[Pos pos]
            {
                get => this.m_cells[pos.m_x, pos.m_y];
                set => this.m_cells[pos.m_x, pos.m_y] = value;
            }

            public Cell this[int h, int w]
            {
                get => this.m_cells[h, w];
                set => this.m_cells[h, w] = value;
            }

            /// <summary>
            /// 入力されたposがグリッド内かどうかを判定します
            /// </summary>
            public bool IsIn(Pos pos)
            {
                return IsIn(pos.m_x, pos.m_y);
            }

            /// <summary>
            /// 入力された座標(h, w)がグリッド内かどうかを判定します
            /// </summary>
            public bool IsIn(int h, int w)
            {
                return 0 <= h && h <= m_height - 1 && 0 <= w && w <= m_width - 1;
            }

            /// <summary>
            /// グリッド内の全セルに対して入力されたfunctionを適用します
            /// </summary>
            public void For(Action<int, int> function)
            {
                for (int h = 0; h < m_height; h++)
                {
                    for (int w = 0; w < m_width; w++)
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
            public int m_x { get; }
            public int m_y { get; }

            public Pos()
            {
                m_x = 0;
                m_y = 0;
            }

            public Pos(int x, int y)
            {
                m_x = x;
                m_y = y;
            }

            public static Pos operator +(Pos a, Pos b)
            {
                return new Pos(a.m_x + b.m_x, a.m_y + b.m_y);
            }

            public static Pos operator -(Pos a, Pos b)
            {
                return new Pos(a.m_x - b.m_x, a.m_y - b.m_y);
            }

            public static readonly Pos UP = new Pos(0, 1);
            public static readonly Pos UP_RIGHT = new Pos(1, 1);
            public static readonly Pos RIGHT = new Pos(1, 0);
            public static readonly Pos DOWN_RIGHT = new Pos(1, -1);
            public static readonly Pos DOWN = new Pos(0, -1);
            public static readonly Pos DOWN_LEFT = new Pos(-1, -1);
            public static readonly Pos LEFT = new Pos(-1, 0);
            public static readonly Pos UP_LEFT = new Pos(-1, 1);
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
        static readonly Pos[] BLACK_NEXT_DIRECTIONS = new Pos[] { Pos.UP, Pos.UP_RIGHT, Pos.RIGHT, Pos.DOWN_RIGHT, Pos.DOWN, Pos.DOWN_LEFT, Pos.LEFT, Pos.UP_LEFT };
        static readonly Pos[] WHITE_NEXT_DIRECTIONS = new Pos[] { Pos.UP, Pos.RIGHT, Pos.DOWN, Pos.LEFT };

        /// <summary>
        /// 入力された図形の各連結成分の穴の数を数えて昇順にして返します
        /// </summary>
        public static List<int> CalculateHoleCount(Bitmap _bitmap)
        {
            // 入力された画像を二値化したグラフに変換します
            Grid grid = ConvertToBinaryGrid(_bitmap);

            ChangeNoiseCellColor(grid);

            AssignSegmentIdToGridCell(grid);

            // 各黒成分の隣にある白成分の数を数える
            // whiteSegmentIds[黒色成分のセグメントId]: キーに使われているセグメントに隣接する白色セグメントのId
            var whiteSegmentIds = CalculateWhiteSegmentIdNextToBlackSegment(grid);

            // 黒色成分の数が連結成分の数に、それに隣接する白の数-1が穴の数になる
            List<int> holeCount = new List<int>();
            foreach (HashSet<int> whiteSegmentId in whiteSegmentIds.Values)
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
        static void ChangeNoiseCellColor(Grid grid)
        {
            grid.For((h, w) =>
            {
                Pos checkPos = new Pos(h, w);
                if (IsNoise(checkPos, grid))
                {
                    grid[checkPos].InvertColor();
                }
            });
        }

        /// <summary>
        /// posの位置にあるセルがノイズかどうかを判定します
        /// </summary>
        static bool IsNoise(Pos pos, Grid grid)
        {
            Pos[] nextDirections = grid[pos].m_color == Grid.Cell.CellColor.BLACK ? BLACK_NEXT_DIRECTIONS : WHITE_NEXT_DIRECTIONS;

            foreach (Pos nextDirection in nextDirections)
            {
                Pos nextPos = pos + nextDirection;

                // 隣接マスが図形外の場合は飛ばす
                if (!grid.IsIn(nextPos))
                {
                    continue;
                }

                // 周囲のセルが同色ならノイズでない
                if (grid[pos].m_color == grid[nextPos].m_color)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// セグメントIdをグリッドのセルに割り当てます
        /// </summary>
        static void AssignSegmentIdToGridCell(Grid grid)
        {
            int segmentCount = 0;

            grid.For((h, w) =>
            {
                Pos checkPos = new Pos(h, w);

                // 割り振られていないマスが見つかったらそのマスと同じ色で連結している部分にidを割り振る
                if (grid[checkPos].m_segmentId != -1)
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
        static void AssignSegmentIdToSameSegmentCell(Pos startPos, int id, Grid grid)
        {
            Queue<Pos> segmentPos = new Queue<Pos>();
            segmentPos.Enqueue(startPos);

            Pos[] nextDirections = grid[startPos].m_color == Grid.Cell.CellColor.BLACK ? BLACK_NEXT_DIRECTIONS : WHITE_NEXT_DIRECTIONS;

            grid[startPos].m_segmentId = id;

            while (segmentPos.Count > 0)
            {
                Pos nowPos = segmentPos.Dequeue();

                foreach (Pos nextDirection in nextDirections)
                {
                    Pos nextPos = nowPos + nextDirection;

                    // 隣接マスが図形外の場合は飛ばす
                    if (!grid.IsIn(nextPos))
                    {
                        continue;
                    }

                    // 隣接マスが既にidを振られている場合は飛ばす
                    if (grid[nextPos].m_segmentId != -1)
                    {
                        continue;
                    }

                    // 隣接マスの色が異なる場合は飛ばす
                    if (grid[nextPos].m_color != grid[nowPos].m_color)
                    {
                        continue;
                    }

                    grid[nextPos].m_segmentId = id;
                    segmentPos.Enqueue(nextPos);
                }
            }
        }

        /// <summary>
        /// 入力されたbitmapデータを二値化したグラフに変換します
        /// </summary>
        static Grid ConvertToBinaryGrid(Bitmap bitmap, float threshold = 0.5f)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            // bitmapの各ピクセルを取得する処理が遅いので配列に各ピクセルのRGBAを転写してそれを処理に使う
            byte[] pixelValues = CopyBitmap(bitmap);

            Grid grid = new Grid(height, width);

            for (int i = 0; i < pixelValues.Length; i += 4)
            {
                int x = (i / 4) % width;
                int y = (i / 4) / width;

                Color pixelColor = Color.FromArgb(pixelValues[i + 3], pixelValues[i + 2], pixelValues[i + 1], pixelValues[i]);
                float brightness = pixelColor.GetBrightness();

                // 閾値に基づいてピクセルを白または黒に分類します
                if (brightness < threshold)
                {
                    grid[x, y].m_color = Grid.Cell.CellColor.BLACK;
                }
                else
                {
                    grid[x, y].m_color = Grid.Cell.CellColor.WHITE;
                }
            }

            return grid;
        }

        /// <summary>
        /// bitmapデータをbyte配列をコピーしたものを返します
        /// </summary>
        static byte[] CopyBitmap(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            // RGBAの各ピクセルは4バイト
            byte[] pixelValues = new byte[width * height * 4];
            Marshal.Copy(data.Scan0, pixelValues, 0, pixelValues.Length);

            bitmap.UnlockBits(data);

            return pixelValues;
        }

        /// <summary>
        /// 各黒色成分の隣にある白成分を返します
        /// result[黒色成分のセグメントId] := キーに使われているセグメントに隣接する白色セグメントのId
        /// </summary>
        static Dictionary<int, HashSet<int>> CalculateWhiteSegmentIdNextToBlackSegment(Grid grid)
        {
            Dictionary<int, HashSet<int>> whiteSegmentIds = new Dictionary<int, HashSet<int>>();
            Pos[] nextDirections = BLACK_NEXT_DIRECTIONS;

            grid.For((h, w) =>
            {
                Pos pos = new Pos(h, w);

                //　白色は飛ばす
                if (grid[pos].m_color == Grid.Cell.CellColor.WHITE)
                {
                    return;
                }

                int blackSegmentId = grid[pos].m_segmentId;
                foreach (Pos nextDirection in nextDirections)
                {
                    Pos nextPos = pos + nextDirection;

                    // 隣接マスが図形外の場合は飛ばす
                    if (!grid.IsIn(nextPos))
                    {
                        continue;
                    }

                    // 隣接マスが黒色の場合は飛ばす
                    if (grid[nextPos].m_color == Grid.Cell.CellColor.BLACK)
                    {
                        continue;
                    }

                    // whiteSegmentIdsがblackSegmentIdをキーとして持たない場合はキーを追加する
                    if (!whiteSegmentIds.ContainsKey(blackSegmentId))
                    {
                        whiteSegmentIds.Add(blackSegmentId, new HashSet<int>());
                    }

                    // 白色のidを追加する
                    whiteSegmentIds[blackSegmentId].Add(/* 白色セグメントのid */ grid[nextPos].m_segmentId);
                }
            });

            return whiteSegmentIds;
        }
    }
}