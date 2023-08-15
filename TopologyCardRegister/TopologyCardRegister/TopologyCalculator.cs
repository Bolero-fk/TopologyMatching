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

            public Cell this[int h, int w]
            {
                get => this.m_cells[h, w];
                set => this.m_cells[h, w] = value;
            }

            public bool IsIn(int h, int w)
            {
                return 0 <= h && h <= m_height - 1 && 0 <= w && w <= m_width - 1;
            }
        }

        /// <summary>
        /// 入力された図形から各連結成分の穴の数を数えて昇順にして返します。
        /// </summary>
        /// <param name="_bitmap"></param>
        /// <returns></returns>
        public List<int> CalculateToPologyStatus(Bitmap _bitmap)
        {
            // 入力された画像を二値化します。
            Grid binary = ConvertToBinary(_bitmap);

            for (int i = 0; i < binary.m_height; i++)
                for (int j = 0; j < binary.m_width; j++)
                {
                    if (IsNoise(i, j, binary))
                    {
                        if (binary[i, j].m_color == Grid.Cell.CellColor.BLACK)
                            binary[i, j].m_color = Grid.Cell.CellColor.WHITE;
                        else if (binary[i, j].m_color == Grid.Cell.CellColor.WHITE)
                            binary[i, j].m_color = Grid.Cell.CellColor.BLACK;
                    }
                }

            // 割り振られていないマスが見つかったらそのマスと同じ色で連結している部分にidを割り振る
            int topologyCount = 0;
            for (int i = 0; i < binary.m_height; i++)
                for (int j = 0; j < binary.m_width; j++)
                {
                    if (binary[i, j].m_segmentId != -1)
                        continue;

                    Dfs(i, j, topologyCount, ref binary);
                    topologyCount++;
                }

            Debug.WriteLine(topologyCount);

            // 各黒成分の隣にある白成分の数を数える
            var nextIds = CalculateNextIds(binary);

            List<int> topologyStatus = new List<int>();
            foreach (HashSet<int> nextId in nextIds.Values)
                topologyStatus.Add(nextId.Count - 1);

            // 穴の数を昇順になるように並び変える
            topologyStatus.Sort();

            return topologyStatus;
        }

        bool IsNoise(int x, int y, Grid binary)
        {
            // 黒色は8方向、白色は4方向に移動させる。
            int[] black_dir_x = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] black_dir_y = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] white_dir_x = new int[] { 0, 0, -1, 1 };
            int[] white_dir_y = new int[] { -1, 1, 0, 0 };

            int[] dir_x = binary[x, y].m_color == Grid.Cell.CellColor.BLACK ? black_dir_x : white_dir_x;
            int[] dir_y = binary[x, y].m_color == Grid.Cell.CellColor.BLACK ? black_dir_y : white_dir_y;

            int sameColorCount = 0;

            for (int d = 0; d < dir_x.Length; d++)
            {
                int next_x = x + dir_x[d];
                int next_y = y + dir_y[d];

                // 隣接マスが図形外の場合は飛ばす
                if (!binary.IsIn(next_x, next_y))
                    continue;

                if (binary[x, y].m_color == binary[next_x, next_y].m_color)
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
        void Dfs(int startX, int startY, int id, ref Grid binary)
        {
            // 黒色は8方向、白色は4方向に移動させる。
            int[] black_dir_x = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] black_dir_y = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] white_dir_x = new int[] { 0, 0, -1, 1 };
            int[] white_dir_y = new int[] { -1, 1, 0, 0 };

            Queue<(int, int)> queue = new Queue<(int, int)>();
            queue.Enqueue((startX, startY));

            int[] dir_x = binary[startX, startY].m_color == Grid.Cell.CellColor.BLACK ? black_dir_x : white_dir_x;
            int[] dir_y = binary[startX, startY].m_color == Grid.Cell.CellColor.BLACK ? black_dir_y : white_dir_y;

            binary[startX, startY].m_segmentId = id;

            while (queue.Count > 0)
            {
                (int now_x, int now_y) = queue.Dequeue();

                for (int d = 0; d < dir_x.Length; d++)
                {
                    int next_x = now_x + dir_x[d];
                    int next_y = now_y + dir_y[d];

                    // 隣接マスが図形外の場合は飛ばす
                    if (!binary.IsIn(next_x, next_y))
                        continue;

                    // 隣接マスが既にidを振られている場合は飛ばす
                    if (binary[next_x, next_y].m_segmentId != -1)
                        continue;

                    // 隣接マスの色が異なる場合は飛ばす
                    if (binary[next_x, next_y].m_color != binary[now_x, now_y].m_color)
                        continue;

                    binary[next_x, next_y].m_segmentId = id;
                    queue.Enqueue((next_x, next_y));
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
            int[] black_dir_x = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] black_dir_y = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int x = 0; x < binary.m_height; x++)
                for (int y = 0; y < binary.m_width; y++)
                {
                    //　白色は飛ばす
                    if (binary[x, y].m_color == Grid.Cell.CellColor.WHITE)
                        continue;

                    for (int d = 0; d < black_dir_x.Length; d++)
                    {
                        int next_x = x + black_dir_x[d];
                        int next_y = y + black_dir_y[d];

                        // 隣接マスが図形外の場合は飛ばす
                        if (!binary.IsIn(next_x, next_y))
                            continue;

                        // 隣接マスが黒色の場合は飛ばす
                        if (binary[next_x, next_y].m_color == Grid.Cell.CellColor.BLACK)
                            continue;

                        // nextIdsの初期化
                        if (!nextIds.ContainsKey(binary[x, y].m_segmentId))
                            nextIds.Add(binary[x, y].m_segmentId, new HashSet<int>());

                        // 白色のidを追加する
                        nextIds[binary[x, y].m_segmentId].Add(binary[next_x, next_y].m_segmentId);
                    }

                }

            return nextIds;
        }
    }
}