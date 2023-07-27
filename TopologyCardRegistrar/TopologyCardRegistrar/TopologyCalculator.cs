using System;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TopologyCardRegistrar
{
    public class TopologyStatusCalculator
    {
        public TopologyStatusCalculator()
        {
        }

        /// <summary>
        /// 入力された図形から各連結成分の穴の数を数えて昇順にして返します。
        /// </summary>
        /// <param name="_bitmap"></param>
        /// <returns></returns>
        public List<int> CalculateToPologyStatus(Bitmap _bitmap)
        {
            // 入力された画像を二値化します。
            bool[,] binary = ConvertToBinary(_bitmap);

            // 各白黒成分にidを割り振る
            int[,] topologyId = new int[binary.GetLength(0), binary.GetLength(1)];

            // 初期化
            for (int i = 0; i < binary.GetLength(0); i++)
                for (int j = 0; j < binary.GetLength(1); j++)
                {
                    topologyId[i, j] = -1;
                }

            // 割り振られていないマスが見つかったらそのマスと同じ色で連結している部分にidを割り振る
            int topologyCount = 0;
            for (int i = 0; i < binary.GetLength(0); i++)
                for (int j = 0; j < binary.GetLength(1); j++)
                {
                    if (topologyId[i, j] != -1)
                        continue;

                    Dfs(i, j, topologyCount, ref binary, ref topologyId);
                    topologyCount++;
                }

            // 各黒成分の隣にある白成分の数を数える
            var nextIds = CalculateNextIds(binary, topologyId);

            List<int> topologyStatus = new List<int>();
            foreach (HashSet<int> nextId in nextIds.Values)
                topologyStatus.Add(nextId.Count - 1);

            // 穴の数を昇順になるように並び変える
            topologyStatus.Sort();

            return topologyStatus;
        }

        /// <summary>
        /// [startX, startY]と連結している成分にidを割り振ります。
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="id"></param>
        /// <param name="binary"></param>
        /// <param name="topologyId"></param>
        void Dfs(int startX, int startY, int id, ref bool[,] binary, ref int[,] topologyId)
        {
            // 黒色は8方向、白色は4方向に移動させる。
            int[] black_dir_x = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] black_dir_y = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] white_dir_x = new int[] { 0, 0, -1, 1 };
            int[] white_dir_y = new int[] { -1, 1, 0, 0 };

            Queue<(int, int)> queue = new Queue<(int, int)>();
            queue.Enqueue((startX, startY));

            int[] dir_x = binary[startX, startY] ? black_dir_x : white_dir_x;
            int[] dir_y = binary[startX, startY] ? black_dir_y : white_dir_y;

            topologyId[startX, startY] = id;

            while (queue.Count > 0)
            {
                (int now_x, int now_y) = queue.Dequeue();

                for (int d = 0; d < dir_x.Length; d++)
                {
                    int next_x = now_x + dir_x[d];
                    int next_y = now_y + dir_y[d];

                    // 隣接マスが図形外の場合は飛ばす
                    if (next_x < 0 || next_x > binary.GetLength(0) - 1 || next_y < 0 || next_y > binary.GetLength(1) - 1)
                        continue;

                    // 隣接マスが既にidを振られている場合は飛ばす
                    if (topologyId[next_x, next_y] != -1)
                        continue;

                    // 隣接マスの色が異なる場合は飛ばす
                    if (binary[next_x, next_y] != binary[now_x, now_y])
                        continue;

                    topologyId[next_x, next_y] = id;
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
        public bool[,] ConvertToBinary(Bitmap bitmap, float threshold = 0.5f)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            // RGBAの各ピクセルは4バイト
            byte[] pixelValues = new byte[width * height * 4];
            Marshal.Copy(data.Scan0, pixelValues, 0, pixelValues.Length);

            bitmap.UnlockBits(data);

            bool[,] result = new bool[height, width];

            for (int i = 0; i < pixelValues.Length; i += 4)
            {
                int x = (i / 4) % width;
                int y = (i / 4) / width;

                Color pixelColor = Color.FromArgb(pixelValues[i + 3], pixelValues[i + 2], pixelValues[i + 1], pixelValues[i]);
                float brightness = pixelColor.GetBrightness();

                // 閾値に基づいてピクセルを白または黒に分類します。
                result[x, y] = brightness < threshold; // 白:false, 黒:true
            }

            return result;
        }

        /// <summary>
        /// 各黒色成分の隣にある白成分を返します。
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="topologyId"></param>
        /// <returns></returns>
        private Dictionary<int, HashSet<int>> CalculateNextIds(bool[,] binary, int[,] topologyId)
        {
            Dictionary<int, HashSet<int>> nextIds = new Dictionary<int, HashSet<int>>();
            int[] black_dir_x = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] black_dir_y = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int x = 0; x < binary.GetLength(0); x++)
                for (int y = 0; y < binary.GetLength(1); y++)
                {
                    //　白色は飛ばす
                    if (!binary[x, y])
                        continue;

                    for (int d = 0; d < black_dir_x.Length; d++)
                    {
                        int next_x = x + black_dir_x[d];
                        int next_y = y + black_dir_y[d];

                        // 隣接マスが図形外の場合は飛ばす
                        if (next_x < 0 || next_x > binary.GetLength(0) - 1 || next_y < 0 || next_y > binary.GetLength(1) - 1)
                            continue;

                        // 隣接マスが黒色の場合は飛ばす
                        if (binary[next_x, next_y])
                            continue;

                        // nextIdsの初期化
                        if (!nextIds.ContainsKey(topologyId[x, y]))
                            nextIds.Add(topologyId[x, y], new HashSet<int>());

                        // 白色のidを追加する
                        nextIds[topologyId[x, y]].Add(topologyId[next_x, next_y]);
                    }

                }

            return nextIds;
        }
    }
}