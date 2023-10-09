namespace TopologyCardRegister
{
    public partial class MainForm : Form
    {
        private string[] svgFilePaths;
        private int nowPage;
        private int[] holeCount;
        private const int DISPLAY_IMAGE_HEIGHT_IN_PIXELS = 1024;
        private const int DISPLAY_IMAGE_WIDTH_IN_PIXELS = 1024;
        private static readonly Color DISPLAY_IMAGE_BACKGROUND_COLOR = Color.White;

        public MainForm()
        {
            this.svgFilePaths = Array.Empty<string>();
            this.nowPage = 0;
            this.holeCount = Array.Empty<int>();

            this.InitializeComponent();
            this.holeCountLabel.Text = string.Empty;
        }

        /// <summary>
        /// svgLoadボタンが押された際の挙動を定義します
        /// </summary>
        private void OnClickLoadSvgButton(object sender, EventArgs e)
        {
            this.svgFilePaths = RequestSvgFilePaths();

            if (this.svgFilePaths.Length != 0)
            {
                this.nowPage = 0;
                this.DisplaySvg(this.svgFilePaths[this.nowPage]);
                this.TryTogglePaginationButton();
            }
            this.TryEnableSaveCardButton();
        }

        /// <summary>
        /// 入力された画像のパスを読み込み画面に表示します
        /// </summary>
        private void DisplaySvg(string svgFilePath)
        {
            var bitmap = LoadSvg(svgFilePath);

            this.svgDisplayBox.Size = bitmap.Size;
            this.svgDisplayBox.Image = bitmap;

            this.DisplayHoleCount(bitmap);
        }

        /// <summary>
        /// 入力された画像のholeCountを画面に表示します
        /// </summary>
        private void DisplayHoleCount(Bitmap bitmap)
        {
            this.holeCount = TopologyStatusCalculator.CalculateHoleCount(bitmap).ToArray();
            this.holeCountLabel.Text = string.Join(',', this.holeCount.Select(num => num));
        }

        /// <summary>
        /// svg画像のファイルパスの入力をユーザーにリクエストします。
        /// </summary>
        private static string[] RequestSvgFilePaths()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "svg files (*.svg)|*.svg";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileNames;
                }
            }

            return Array.Empty<string>();
        }

        /// <summary>
        /// svg画像を読み込みます
        /// </summary>
        private static Bitmap LoadSvg(string filePath)
        {
            var svgDocument = Svg.SvgDocument.Open(filePath);
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
        /// outputSvgボタンを押した際の挙動を定義します
        /// </summary>
        private void OnClickOutputSvgButton(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    this.outputSvgPathTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
            this.TryEnableSaveCardButton();
        }

        /// <summary>
        /// OutputHoleCountボタンを押した際の挙動を定義します
        /// </summary>
        private void OnClickOutputHoleCountbutton(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "json files (*.json)|*.json";
                saveFileDialog.OverwritePrompt = false;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.outputHoleCountPathBox.Text = saveFileDialog.FileName;
                }
            }
            this.TryEnableSaveCardButton();
        }

        /// <summary>
        /// saveCarボタンを押した際の挙動を定義します
        /// </summary>
        private void OnClickSaveCardButton(object sender, EventArgs e)
        {
            var svgFileName = Path.GetFileName(this.svgFilePaths[this.nowPage]);
            var jsonPath = this.outputHoleCountPathBox.Text;
            var svgFolderPath = this.outputSvgPathTextBox.Text;

            // 画像を保存する
            File.Copy(this.svgFilePaths[this.nowPage], Path.Combine(svgFolderPath, svgFileName), true);

            // jsonを保存する
            JsonSaver.SaveJson(jsonPath, svgFileName, this.holeCount);
        }

        /// <summary>
        /// 条件を満たしている場合にsaveCardボタンをアクティブにします。
        /// </summary>
        private void TryEnableSaveCardButton()
        {
            if (this.CanSaveCard())
            {
                this.saveCardButton.Enabled = true;
            }
        }

        /// <summary>
        /// 保存に必要な情報が入力されているかどうかを判定します。
        /// </summary>
        private bool CanSaveCard()
        {
            if (this.outputSvgPathTextBox.Text == string.Empty)
            {
                return false;
            }
            if (this.outputHoleCountPathBox.Text == string.Empty)
            {
                return false;
            }
            if (this.holeCountLabel.Text == string.Empty)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ページを切り替えるボタンの状態を切り替える必要がある場合は切り替えます。
        /// </summary>
        private void TryTogglePaginationButton()
        {
            this.prevButton.Enabled = this.nowPage > 0;
            this.nextButton.Enabled = this.nowPage < this.svgFilePaths.Length - 1;
        }

        /// <summary>
        /// prevButtonを押した際の挙動を定義します
        /// </summary>
        private void OnClickPrevButton(object sender, EventArgs e)
        {
            this.nowPage--;
            this.DisplaySvg(this.svgFilePaths[this.nowPage]);
            this.TryTogglePaginationButton();
        }

        /// <summary>
        /// nextButtonを押した際の挙動を定義します
        /// </summary>
        private void OnClickNextButton(object sender, EventArgs e)
        {
            this.nowPage++;
            this.DisplaySvg(this.svgFilePaths[this.nowPage]);
            this.TryTogglePaginationButton();
        }
    }
}