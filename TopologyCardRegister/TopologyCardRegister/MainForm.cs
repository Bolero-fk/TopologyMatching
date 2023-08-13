using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace TopologyCardRegister
{
    public partial class MainForm : Form
    {
        string[] m_imgFilePaths = Array.Empty<string>();
        int m_nowPage = 0;
        int[] m_holeCounts = Array.Empty<int>();

        const int DISPLAY_IMAGE_HEIGHT_IN_PIXELS = 1024;
        const int DISPLAY_IMAGE_WIDTH_IN_PIXELS = 1024;
        static readonly Color DISPLAY_IMAGE_BACKGROUND_COLOR = Color.White;

        public MainForm()
        {
            InitializeComponent();
            holeCountLabel.Text = string.Empty;
        }

        private void OnClickLoadSvgButton(object sender, EventArgs e)
        {
            string[] svgFilePaths = RequestSvgFilePaths();

            if (svgFilePaths.Length != 0)
            {
                m_nowPage = 0;
                m_imgFilePaths = svgFilePaths;
                DisplaySvg(m_imgFilePaths[m_nowPage]);
                TryTogglePaginationButton();
            }
            TryEnableSaveCardButton();
        }

        private void DisplaySvg(string svgFilePath)
        {
            Bitmap bitmap = LoadSvg(svgFilePath);
            TopologyStatusCalculator statusCalculator = new TopologyStatusCalculator();
            m_holeCounts = statusCalculator.CalculateToPologyStatus(bitmap).ToArray();
            string holeCount = string.Join(',', m_holeCounts.Select(num => num.ToString())); ;
            holeCountLabel.Text = holeCount;

            pictureBox1.Size = bitmap.Size;
            pictureBox1.Image = bitmap;
        }

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

            return new string[0];
        }

        Bitmap LoadSvg(string _filePath)
        {
            var svgDocument = Svg.SvgDocument.Open(_filePath);
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

        private void OnClickOutputSvgButton(object sender, EventArgs e)
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

        private void OnClickOutputHoleCountbutton(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                string? currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                saveFileDialog.Filter = "json files (*.json)|*.json";
                saveFileDialog.OverwritePrompt = false;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    outputHoleCountPathBox.Text = saveFileDialog.FileName;
                }
            }
            TryEnableSaveCardButton();
        }

        private void OnClickSaveCardButton(object sender, EventArgs e)
        {
            string imgFileName = Path.GetFileName(m_imgFilePaths[m_nowPage]);
            string jsonPath = outputHoleCountPathBox.Text;
            string imgFolderPath = outputSvgPathTextBox.Text;

            // ‰æ‘œ‚ð•Û‘¶‚·‚é
            File.Copy(m_imgFilePaths[m_nowPage], Path.Combine(imgFolderPath, imgFileName), true);

            // json‚ð•Û‘¶‚·‚é
            JsonSaver.SaveJson(jsonPath, imgFileName, m_holeCounts);
        }

        private void TryEnableSaveCardButton()
        {
            if (CanSaveCard())
                SaveCardButton.Enabled = true;
        }

        bool CanSaveCard()
        {
            if (outputSvgPathTextBox.Text == string.Empty)
                return false;
            if (outputHoleCountPathBox.Text == string.Empty)
                return false;
            if (holeCountLabel.Text == string.Empty)
                return false;

            return true;
        }

        void TryTogglePaginationButton()
        {
            prevButton.Enabled = m_nowPage > 0;
            nextButton.Enabled = m_nowPage < m_imgFilePaths.Length - 1;
        }

        void OnClickPrevButton(object sender, EventArgs e)
        {
            m_nowPage--;
            DisplaySvg(m_imgFilePaths[m_nowPage]);
            TryTogglePaginationButton();
        }

        void OnClickNextButton(object sender, EventArgs e)
        {
            m_nowPage++;
            DisplaySvg(m_imgFilePaths[m_nowPage]);
            TryTogglePaginationButton();
        }
    }
}