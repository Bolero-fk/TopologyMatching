namespace TopologyCardRegister
{
    public partial class MainForm : Form
    {
        string[] m_svgFilePaths;
        int m_nowPage;
        int[] m_holeCount;

        const int DISPLAY_IMAGE_HEIGHT_IN_PIXELS = 1024;
        const int DISPLAY_IMAGE_WIDTH_IN_PIXELS = 1024;
        static readonly Color DISPLAY_IMAGE_BACKGROUND_COLOR = Color.White;

        public MainForm()
        {
            m_svgFilePaths = Array.Empty<string>();
            m_nowPage = 0;
            m_holeCount = Array.Empty<int>();

            InitializeComponent();
            holeCountLabel.Text = string.Empty;
        }

        /// <summary>
        /// svgLoadボタンが押された際の挙動を定義します
        /// </summary>
        void OnClickLoadSvgButton(object sender, EventArgs e)
        {
            m_svgFilePaths = RequestSvgFilePaths();

            if (m_svgFilePaths.Length != 0)
            {
                m_nowPage = 0;
                DisplaySvg(m_svgFilePaths[m_nowPage]);
                TryTogglePaginationButton();
            }
            TryEnableSaveCardButton();
        }

        /// <summary>
        /// 入力された画像のパスを読み込み画面に表示します
        /// </summary>
        void DisplaySvg(string svgFilePath)
        {
            Bitmap bitmap = LoadSvg(svgFilePath);

            svgDisplayBox.Size = bitmap.Size;
            svgDisplayBox.Image = bitmap;

            DisplayHoleCount(bitmap);
        }

        /// <summary>
        /// 入力された画像のholeCountを画面に表示します
        /// </summary>
        void DisplayHoleCount(Bitmap bitmap)
        {
            m_holeCount = TopologyStatusCalculator.CalculateHoleCount(bitmap).ToArray();
            holeCountLabel.Text = string.Join(',', m_holeCount.Select(num => num.ToString()));
        }

        /// <summary>
        /// svg画像のファイルパスの入力をユーザーにリクエストします。
        /// </summary>
        string[] RequestSvgFilePaths()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
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
        Bitmap LoadSvg(string filePath)
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
        void OnClickOutputSvgButton(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    outputSvgPathTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
            TryEnableSaveCardButton();
        }

        /// <summary>
        /// OutputHoleCountボタンを押した際の挙動を定義します
        /// </summary>
        void OnClickOutputHoleCountbutton(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "json files (*.json)|*.json";
                saveFileDialog.OverwritePrompt = false;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    outputHoleCountPathBox.Text = saveFileDialog.FileName;
                }
            }
            TryEnableSaveCardButton();
        }

        /// <summary>
        /// saveCarボタンを押した際の挙動を定義します
        /// </summary>
        void OnClickSaveCardButton(object sender, EventArgs e)
        {
            string svgFileName = Path.GetFileName(m_svgFilePaths[m_nowPage]);
            string jsonPath = outputHoleCountPathBox.Text;
            string svgFolderPath = outputSvgPathTextBox.Text;

            // 画像を保存する
            File.Copy(m_svgFilePaths[m_nowPage], Path.Combine(svgFolderPath, svgFileName), true);

            // jsonを保存する
            JsonSaver.SaveJson(jsonPath, svgFileName, m_holeCount);
        }

        /// <summary>
        /// 条件を満たしている場合にsaveCardボタンをアクティブにします。
        /// </summary>
        void TryEnableSaveCardButton()
        {
            if (CanSaveCard())
            {
                saveCardButton.Enabled = true;
            }
        }

        /// <summary>
        /// 保存に必要な情報が入力されているかどうかを判定します。
        /// </summary>
        bool CanSaveCard()
        {
            if (outputSvgPathTextBox.Text == string.Empty)
            {
                return false;
            }
            if (outputHoleCountPathBox.Text == string.Empty)
            {
                return false;
            }
            if (holeCountLabel.Text == string.Empty)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ページを切り替えるボタンの状態を切り替える必要がある場合は切り替えます。
        /// </summary>
        void TryTogglePaginationButton()
        {
            prevButton.Enabled = m_nowPage > 0;
            nextButton.Enabled = m_nowPage < m_svgFilePaths.Length - 1;
        }

        /// <summary>
        /// prevButtonを押した際の挙動を定義します
        /// </summary>
        void OnClickPrevButton(object sender, EventArgs e)
        {
            m_nowPage--;
            DisplaySvg(m_svgFilePaths[m_nowPage]);
            TryTogglePaginationButton();
        }

        /// <summary>
        /// nextButtonを押した際の挙動を定義します
        /// </summary>
        void OnClickNextButton(object sender, EventArgs e)
        {
            m_nowPage++;
            DisplaySvg(m_svgFilePaths[m_nowPage]);
            TryTogglePaginationButton();
        }
    }
}