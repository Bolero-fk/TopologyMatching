using System;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TopologyCardRegister
{
    public class TopologyStatusCalculator
    {
        public TopologyStatusCalculator()
        {
        }

        public class Grid
        {
            public class Cell
            {
                public int m_segmentId;
                public CellColor m_color;

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
            }

            public int m_height;
            public int m_width;

            public Cell[,] m_cells;

            Grid() { }

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

            public bool IsIn(Pos pos)
            {
                return IsIn(pos.m_x, pos.m_y);
            }

            public bool IsIn(int h, int w)
            {
                return 0 <= h && h <= m_height - 1 && 0 <= w && w <= m_width - 1;
            }

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

        public class Pos
        {
            public int m_x;
            public int m_y;

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
        }

        static readonly Pos UP = new Pos(0, 1);
        static readonly Pos UP_RIGHT = new Pos(1, 1);
        static readonly Pos RIGHT = new Pos(1, 0);
        static readonly Pos DOWN_RIGHT = new Pos(1, -1);
        static readonly Pos DOWN = new Pos(0, -1);
        static readonly Pos DOWN_LEFT = new Pos(-1, -1);
        static readonly Pos LEFT = new Pos(-1, 0);
        static readonly Pos UP_LEFT = new Pos(-1, 1);

        static readonly Pos[] BLACK_ADJACENT_DIRECTIONS = new Pos[] { UP, UP_RIGHT, RIGHT, DOWN_RIGHT, DOWN, DOWN_LEFT, LEFT, UP_LEFT };
        static readonly Pos[] WHITE_ADJACENT_DIRECTIONS = new Pos[] { UP, RIGHT, DOWN, LEFT };

        /// <summary>
        /// 入力された図形から各連結成分の穴の数を数えて昇順にして返します。
        /// </summary>
        /// <param name="_bitmap"></param>
        /// <returns></returns>
        public List<int> CalculateToPologyStatus(Bitmap _bitmap)
        {
            // 入力された画像を二値化します。
            Grid binary = ConvertToBinary(_bitmap);

            ChangeNoiseCellColor(binary);

            // 割り振られていないマスが見つかったらそのマスと同じ色で連結している部分にidを割り振る
            int topologyCount = 0;

            binary.For((h, w) =>
            {
                Pos checkPos = new Pos(h, w);
                if (binary[checkPos].m_segmentId != -1)
                    return;

                Dfs(checkPos, topologyCount, ref binary);
                topologyCount++;
            });

            // 各黒成分の隣にある白成分の数を数える
            var nextIds = CalculateNextIds(binary);

            List<int> topologyStatus = new List<int>();
            foreach (HashSet<int> nextId in nextIds.Values)
                topologyStatus.Add(nextId.Count - 1);

            // 穴の数を昇順になるように並び変える
            topologyStatus.Sort();

            return topologyStatus;
        }

        void ChangeNoiseCellColor(Grid binary)
        {
            binary.For((h, w) =>
            {
                Pos checkPos = new Pos(h, w);
                if (IsNoise(checkPos, binary))
                {
                    if (binary[checkPos].m_color == Grid.Cell.CellColor.BLACK)
                        binary[checkPos].m_color = Grid.Cell.CellColor.WHITE;
                    else if (binary[checkPos].m_color == Grid.Cell.CellColor.WHITE)
                        binary[checkPos].m_color = Grid.Cell.CellColor.BLACK;
                }
            });
        }

        bool IsNoise(Pos pos, Grid binary)
        {
            Pos[] adjacentDirections = binary[pos].m_color == Grid.Cell.CellColor.BLACK ? BLACK_ADJACENT_DIRECTIONS : WHITE_ADJACENT_DIRECTIONS;

            int sameColorCount = 0;

            foreach (Pos adjacentDirection in adjacentDirections)
            {
                Pos nextPos = pos + adjacentDirection;

                // 隣接マスが図形外の場合は飛ばす
                if (!binary.IsIn(nextPos))
                    continue;

                if (binary[pos].m_color == binary[nextPos].m_color)
                    sameColorCount++;
            }

            return sameColorCount == 0;
        }

        /// <summary>
        /// [startX, startY]と連結している成分にidを割り振ります。
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="id"></param>
        /// <param name="binary"></param>
        /// <param name="topologyId"></param>
        void Dfs(Pos startPos, int id, ref Grid binary)
        {

            Queue<Pos> queue = new Queue<Pos>();
            queue.Enqueue(startPos);

            Pos[] adjacentDirections = binary[startPos].m_color == Grid.Cell.CellColor.BLACK ? BLACK_ADJACENT_DIRECTIONS : WHITE_ADJACENT_DIRECTIONS;

            binary[startPos].m_segmentId = id;

            while (queue.Count > 0)
            {
                Pos nowPos = queue.Dequeue();

                foreach (Pos adjacentDirection in adjacentDirections)
                {
                    Pos nextPos = nowPos + adjacentDirection;

                    // 隣接マスが図形外の場合は飛ばす
                    if (!binary.IsIn(nextPos))
                        continue;

                    // 隣接マスが既にidを振られている場合は飛ばす
                    if (binary[nextPos].m_segmentId != -1)
                        continue;

                    // 隣接マスの色が異なる場合は飛ばす
                    if (binary[nextPos].m_color != binary[nowPos].m_color)
                        continue;

                    binary[nextPos].m_segmentId = id;
                    queue.Enqueue(nextPos);
                }
            }
        }

        /// <summary>
        /// 入力されたbitmapデータを二値化して返します。
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public Grid ConvertToBinary(Bitmap bitmap, float threshold = 0.5f)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            // RGBAの各ピクセルは4バイト
            byte[] pixelValues = new byte[width * height * 4];
            Marshal.Copy(data.Scan0, pixelValues, 0, pixelValues.Length);

            bitmap.UnlockBits(data);

            Grid result = new Grid(height, width);

            for (int i = 0; i < pixelValues.Length; i += 4)
            {
                int x = (i / 4) % width;
                int y = (i / 4) / width;

                Color pixelColor = Color.FromArgb(pixelValues[i + 3], pixelValues[i + 2], pixelValues[i + 1], pixelValues[i]);
                float brightness = pixelColor.GetBrightness();

                // 閾値に基づいてピクセルを白または黒に分類します。
                if (brightness < threshold)
                    result[x, y].m_color = Grid.Cell.CellColor.BLACK;
                else
                    result[x, y].m_color = Grid.Cell.CellColor.WHITE;
            }

            return result;
        }

        /// <summary>
        /// 各黒色成分の隣にある白成分を返します。
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="topologyId"></param>
        /// <returns></returns>
        private Dictionary<int, HashSet<int>> CalculateNextIds(Grid binary)
        {
            Dictionary<int, HashSet<int>> nextIds = new Dictionary<int, HashSet<int>>();
            Pos[] adjacentDirections = BLACK_ADJACENT_DIRECTIONS;

            binary.For((h, w) =>
            {
                Pos pos = new Pos(h, w);

                //　白色は飛ばす
                if (binary[pos].m_color == Grid.Cell.CellColor.WHITE)
                    return;

                foreach (Pos adjacentDirection in adjacentDirections)
                {
                    Pos nextPos = pos + adjacentDirection;

                    // 隣接マスが図形外の場合は飛ばす
                    if (!binary.IsIn(nextPos))
                        continue;

                    // 隣接マスが黒色の場合は飛ばす
                    if (binary[nextPos].m_color == Grid.Cell.CellColor.BLACK)
                        continue;

                    // nextIdsの初期化
                    if (!nextIds.ContainsKey(binary[pos].m_segmentId))
                        nextIds.Add(binary[pos].m_segmentId, new HashSet<int>());

                    // 白色のidを追加する
                    nextIds[binary[pos].m_segmentId].Add(binary[nextPos].m_segmentId);
                }
            });

            return nextIds;
        }
    }
}