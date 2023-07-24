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

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string svgFilePath = GetSvgFilePath();

            if(svgFilePath != string.Empty)
                DisplaySvg(svgFilePath);
        }

        string GetSvgFilePath()
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string? currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                openFileDialog.InitialDirectory = currentPath;
                openFileDialog.Filter = "svg files (*.svg)|*.svg";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }

            return filePath;
        }

        void DisplaySvg(string _filePath)
        {
            var svgDocument = Svg.SvgDocument.Open(_filePath);
            svgDocument.Width = 1024;
            svgDocument.Height = 1024;
            var bitmap = svgDocument.Draw();
            pictureBox1.Size = bitmap.Size;
            pictureBox1.Image = bitmap;
        }
    }
}