namespace TopologyCardRegister
{
    public class TopologyCard
    {
        private const int DISPLAY_IMAGE_HEIGHT_IN_PIXELS = 1024;
        private const int DISPLAY_IMAGE_WIDTH_IN_PIXELS = 1024;
        private static readonly Color DISPLAY_IMAGE_BACKGROUND_COLOR = Color.White;

        public string SvgFilePath { get; }
        public Bitmap SvgImage { get; }
        public int[] HoleCounts { get; }

        public TopologyCard()
        {
            this.SvgFilePath = string.Empty;
            this.SvgImage = new Bitmap(DISPLAY_IMAGE_WIDTH_IN_PIXELS, DISPLAY_IMAGE_HEIGHT_IN_PIXELS);
            this.HoleCounts = Array.Empty<int>();
        }

        public TopologyCard(string svgFilePath)
        {
            this.SvgFilePath = svgFilePath;
            this.SvgImage = this.LoadSvg();
            this.HoleCounts = TopologyStatusCalculator.CalculateHoleCount(this.SvgImage).ToArray();
        }

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

        public void Save(string svgFolderPath, string jsonPath)
        {
            var svgFileName = Path.GetFileName(this.SvgFilePath);

            // 画像を保存する
            File.Copy(this.SvgFilePath, Path.Combine(svgFolderPath, svgFileName), true);

            // jsonを保存する
            JsonSaver.SaveJson(jsonPath, svgFileName, this.HoleCounts);
        }
    }
}
