namespace TopologyCardRegister
{
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    public class BitmapToMonochromeGridConverter
    {
        // 画素の白黒を判定する際の輝度の閾値 (0に近いほど黒、1に近いほど白となる)
        private const float BRIGHTNESS_THREHOLD = 0.5f;

        /// <summary> 黒画素が画像の端にあると正しく穴の判定ができないので入力画像の余白を設定する </summary>
        private const int INPUT_IMAGE_PADDING_SIZE = 1;

        /// <summary>
        /// 入力されたbitmapデータを二値化したグラフに変換します
        /// </summary>
        public static Grid<MonochromeCell> ConvertToBinaryGrid(Bitmap bitmap)
        {
            var bitmapWithPadding = AddPadding(bitmap, INPUT_IMAGE_PADDING_SIZE);
            var width = bitmapWithPadding.Width;
            var height = bitmapWithPadding.Height;

            // bitmapの各ピクセルを取得する処理が遅いので配列に各ピクセルのRGBAを転写してそれを処理に使う
            var pixelValues = CopyBitmap(bitmapWithPadding);

            var grid = new Grid<MonochromeCell>(height, width);

            for (var i = 0; i < pixelValues.Length; i += 4)
            {
                var w = i / 4 % width;
                var h = i / 4 / width;

                var pixelColor = Color.FromArgb(pixelValues[i + 3], pixelValues[i + 2], pixelValues[i + 1], pixelValues[i]);
                var brightness = pixelColor.GetBrightness();

                // 閾値に基づいてピクセルを白または黒に分類します
                grid[h, w].Color = brightness < BRIGHTNESS_THREHOLD ? MonochromeCell.CellColor.BLACK : MonochromeCell.CellColor.WHITE;
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

    }
}
