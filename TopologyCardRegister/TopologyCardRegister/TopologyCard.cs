namespace TopologyCardRegister
{
    /// <summary>
    /// トポロジーカードを管理し、SVG画像と関連データの読み書きをサポートするクラス。
    /// </summary>
    public class TopologyCard
    {
        /// <summary>
        /// SVGファイルパスが設定されていない場合にスローされる例外。
        /// </summary>
        public class NotSetSvgFilePathException : Exception
        {
            public NotSetSvgFilePathException(string message) : base(message)
            {
            }
        }

        /// <summary> 表示する画像の縦幅。 </summary>
        private const int DISPLAY_IMAGE_HEIGHT_IN_PIXELS = 1024;

        /// <summary> 表示する画像の横幅。 </summary>
        private const int DISPLAY_IMAGE_WIDTH_IN_PIXELS = 1024;

        /// <summary> 表示する画像の背景色。 </summary>
        private static readonly Color DISPLAY_IMAGE_BACKGROUND_COLOR = Color.White;

        /// <summary> 入力した画像のファイルパス。 </summary>
        public string SvgFilePath { get; }

        /// <summary> 入力した画像のbitmapデータ。 </summary>
        public Bitmap SvgImage { get; }

        /// <summary> 入力した画像の図形の穴の数。 </summary>
        public int[] HoleCounts { get; }

        /// <summary>
        /// デフォルトコンストラクター。各プロパティを初期状態に設定します。
        /// </summary>
        public TopologyCard()
        {
            this.SvgFilePath = string.Empty;
            this.SvgImage = new Bitmap(DISPLAY_IMAGE_WIDTH_IN_PIXELS, DISPLAY_IMAGE_HEIGHT_IN_PIXELS);
            this.HoleCounts = Array.Empty<int>();
        }

        /// <summary>
        /// コンストラクター。指定されたSVGファイルパスから画像を読み込み、図形の穴の数を計算します。
        /// </summary>
        /// <param name="svgFilePath">SVGファイルのパス</param>
        public TopologyCard(string svgFilePath)
        {
            this.SvgFilePath = svgFilePath;
            this.SvgImage = this.LoadSvg();
            this.HoleCounts = TopologyStatusCalculator.Execute(this.SvgImage).ToArray();
        }

        /// <summary>
        /// 指定されたSVGファイルパスからSVGドキュメントを読み込み、背景色を設定してBitmapとして返します。
        /// </summary>
        /// <returns>読み込んだSVG画像のBitmapデータ。</returns>
        private Bitmap LoadSvg()
        {
            var svgDocument = Svg.SvgDocument.Open(this.SvgFilePath);
            svgDocument.Children.Insert(0, new Svg.SvgRectangle
            {
                Width = new Svg.SvgUnit(svgDocument.Width.Type, svgDocument.Width.Value),
                Height = new Svg.SvgUnit(svgDocument.Height.Type, svgDocument.Height.Value),
                Fill = new Svg.SvgColourServer(DISPLAY_IMAGE_BACKGROUND_COLOR)
            });

            svgDocument.Height = DISPLAY_IMAGE_HEIGHT_IN_PIXELS;
            svgDocument.Width = DISPLAY_IMAGE_WIDTH_IN_PIXELS;

            return svgDocument.Draw();
        }

        /// <summary>
        /// このオブジェクトのSVG画像とJSONデータを指定されたパスに保存します。
        /// </summary>
        /// <param name="svgFolderPath">SVG画像を保存するフォルダのパス。</param>
        /// <param name="jsonPath">JSONデータを保存するファイルのパス。</param>
        public void Save(string svgFolderPath, string jsonPath)
        {
            if (this.SvgFilePath == string.Empty)
            {
                throw new NotSetSvgFilePathException("SvgFilePath has not been set.");
            }

            var svgFileName = Path.GetFileName(this.SvgFilePath);

            // 画像を保存する
            File.Copy(this.SvgFilePath, Path.Combine(svgFolderPath, svgFileName), true);

            // jsonを保存する
            JsonSaver.SaveJson(jsonPath, svgFileName, this.HoleCounts);
        }
    }
}
