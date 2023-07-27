using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace TopologyCardRegistrar
{
    public partial class Form1 : Form
    {
        string m_imgFilePath = string.Empty;
        int[] m_holeCounts = new int[0];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            holeCountLabel.Text = string.Empty;
        }

        private void LoadSvgButton_Click(object sender, EventArgs e)
        {
            string svgFilePath = GetSvgFilePath();

            if (svgFilePath != string.Empty)
            {
                m_imgFilePath = svgFilePath;
                Bitmap bitmap = DisplaySvg(svgFilePath);
                TopologyStatusCalculator statusCalculator = new TopologyStatusCalculator();
                m_holeCounts = statusCalculator.CalculateToPologyStatus(bitmap).ToArray();
                string holeCount = string.Join(',', m_holeCounts.Select(num => num.ToString())); ;
                holeCountLabel.Text = holeCount;
            }
            ChangeSaveCardButton();
        }

        string GetSvgFilePath()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "svg files (*.svg)|*.svg";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    return openFileDialog.FileName;
                }
            }

            return string.Empty;
        }

        Bitmap DisplaySvg(string _filePath)
        {
            var svgDocument = Svg.SvgDocument.Open(_filePath);
            svgDocument.Children.Insert(0, new Svg.SvgRectangle
            {
                Width = new Svg.SvgUnit(svgDocument.Width.Type, svgDocument.Width.Value),
                Height = new Svg.SvgUnit(svgDocument.Height.Type, svgDocument.Height.Value),
                Fill = new Svg.SvgColourServer(Color.White)
            });

            svgDocument.Width = 1024;
            svgDocument.Height = 1024;
            var bitmap = svgDocument.Draw();
            pictureBox1.Size = bitmap.Size;
            pictureBox1.Image = bitmap;

            return bitmap;
        }

        private void OutputSvgButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    outputSvgPathTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
            ChangeSaveCardButton();
        }

        private void OutputHoleCountbutton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                string? currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                saveFileDialog.Filter = "json files (*.json)|*.json";
                saveFileDialog.OverwritePrompt = false;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    outputHoleCountPathBox.Text = saveFileDialog.FileName;
                }
            }
            ChangeSaveCardButton();
        }

        private void SaveCardButton_Click(object sender, EventArgs e)
        {
            string imgFileName = Path.GetFileName(m_imgFilePath);
            string jsonPath = outputHoleCountPathBox.Text;
            string imgFolderPath = outputSvgPathTextBox.Text;

            // ‰æ‘œ‚ð•Û‘¶‚·‚é
            File.Copy(m_imgFilePath, Path.Combine(imgFolderPath, imgFileName), true);

            // json‚ð•Û‘¶‚·‚é
            JsonSaver.SaveJson(jsonPath, imgFileName, m_holeCounts);
        }

        private void ChangeSaveCardButton()
        {
            if (outputSvgPathTextBox.Text == string.Empty)
                return;
            if (outputHoleCountPathBox.Text == string.Empty)
                return;
            if (holeCountLabel.Text == string.Empty)
                return;
            SaveCardButton.Enabled = true;
        }
    }
}