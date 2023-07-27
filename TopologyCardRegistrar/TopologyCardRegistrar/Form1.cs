using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace TopologyCardRegistrar
{
    public partial class Form1 : Form
    {
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
                Bitmap bitmap = DisplaySvg(svgFilePath);
                TopologyStatusCalculator statusCalculator = new TopologyStatusCalculator();
                string holeCount = string.Join(',', statusCalculator.CalculateToPologyStatus(bitmap).Select(num => num.ToString())); ;
                holeCountLabel.Text = holeCount;
                Debug.WriteLine(holeCount);
            }
            ChangeSaveCardButton();
        }

        string GetSvgFilePath()
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "svg files (*.svg)|*.svg";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }

            return filePath;
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