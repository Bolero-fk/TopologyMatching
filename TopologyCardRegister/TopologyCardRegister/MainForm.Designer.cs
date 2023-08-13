namespace TopologyCardRegister
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            loadSvgButton = new Button();
            svgDisplayBox = new PictureBox();
            panel = new Panel();
            holeCountLabel = new Label();
            holeCountDescription = new Label();
            outputSvgLabel = new Label();
            outputHoleCountLabel = new Label();
            saveCardButton = new Button();
            saveFileDialog1 = new SaveFileDialog();
            outputSvgPathTextBox = new TextBox();
            outputSvgButton = new Button();
            outputHoleCountPathBox = new TextBox();
            outputHoleCountbutton = new Button();
            splitContainer1 = new SplitContainer();
            prevButton = new Button();
            nextButton = new Button();
            ((System.ComponentModel.ISupportInitialize)svgDisplayBox).BeginInit();
            panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // loadSvgButton
            // 
            loadSvgButton.Location = new Point(12, 872);
            loadSvgButton.Name = "loadSvgButton";
            loadSvgButton.Size = new Size(521, 23);
            loadSvgButton.TabIndex = 0;
            loadSvgButton.Text = "画像を読み込む";
            loadSvgButton.UseVisualStyleBackColor = true;
            loadSvgButton.Click += OnClickLoadSvgButton;
            // 
            // svgDisplayBox
            // 
            svgDisplayBox.Location = new Point(0, 0);
            svgDisplayBox.Name = "svgDisplayBox";
            svgDisplayBox.Size = new Size(714, 596);
            svgDisplayBox.SizeMode = PictureBoxSizeMode.AutoSize;
            svgDisplayBox.TabIndex = 1;
            svgDisplayBox.TabStop = false;
            // 
            // panel
            // 
            panel.AutoScroll = true;
            panel.Controls.Add(svgDisplayBox);
            panel.Location = new Point(12, 12);
            panel.Name = "panel";
            panel.Size = new Size(1040, 748);
            panel.TabIndex = 2;
            // 
            // holeCountLabel
            // 
            holeCountLabel.AutoSize = true;
            holeCountLabel.Font = new Font("Yu Gothic UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point);
            holeCountLabel.Location = new Point(35, 927);
            holeCountLabel.Name = "holeCountLabel";
            holeCountLabel.Size = new Size(208, 50);
            holeCountLabel.TabIndex = 3;
            holeCountLabel.Text = "Hole Count";
            // 
            // holeCountDescription
            // 
            holeCountDescription.AutoSize = true;
            holeCountDescription.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            holeCountDescription.Location = new Point(12, 903);
            holeCountDescription.Name = "holeCountDescription";
            holeCountDescription.Size = new Size(356, 32);
            holeCountDescription.TabIndex = 4;
            holeCountDescription.Text = "入力された画像の要素ごとの穴の数";
            // 
            // outputSvgLabel
            // 
            outputSvgLabel.AutoSize = true;
            outputSvgLabel.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            outputSvgLabel.Location = new Point(3, 3);
            outputSvgLabel.Name = "outputSvgLabel";
            outputSvgLabel.Size = new Size(329, 32);
            outputSvgLabel.TabIndex = 5;
            outputSvgLabel.Text = "入力された画像の保存先フォルダ";
            // 
            // outputHoleCountLabel
            // 
            outputHoleCountLabel.AutoSize = true;
            outputHoleCountLabel.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            outputHoleCountLabel.Location = new Point(3, 2);
            outputHoleCountLabel.Name = "outputHoleCountLabel";
            outputHoleCountLabel.Size = new Size(242, 32);
            outputHoleCountLabel.TabIndex = 6;
            outputHoleCountLabel.Text = "穴の数の出力先ファイル";
            // 
            // saveCardButton
            // 
            saveCardButton.Enabled = false;
            saveCardButton.Location = new Point(536, 872);
            saveCardButton.Name = "saveCardButton";
            saveCardButton.Size = new Size(516, 23);
            saveCardButton.TabIndex = 7;
            saveCardButton.Text = "カードを保存する";
            saveCardButton.UseVisualStyleBackColor = true;
            saveCardButton.Click += OnClickSaveCardButton;
            // 
            // outputSvgPathTextBox
            // 
            outputSvgPathTextBox.Enabled = false;
            outputSvgPathTextBox.Location = new Point(3, 38);
            outputSvgPathTextBox.Name = "outputSvgPathTextBox";
            outputSvgPathTextBox.RightToLeft = RightToLeft.Yes;
            outputSvgPathTextBox.Size = new Size(433, 23);
            outputSvgPathTextBox.TabIndex = 8;
            outputSvgPathTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // outputSvgButton
            // 
            outputSvgButton.Location = new Point(442, 38);
            outputSvgButton.Name = "outputSvgButton";
            outputSvgButton.Size = new Size(75, 23);
            outputSvgButton.TabIndex = 10;
            outputSvgButton.Text = "参照";
            outputSvgButton.UseVisualStyleBackColor = true;
            outputSvgButton.Click += OnClickOutputSvgButton;
            // 
            // outputHoleCountPathBox
            // 
            outputHoleCountPathBox.Enabled = false;
            outputHoleCountPathBox.Location = new Point(3, 39);
            outputHoleCountPathBox.Name = "outputHoleCountPathBox";
            outputHoleCountPathBox.RightToLeft = RightToLeft.Yes;
            outputHoleCountPathBox.Size = new Size(429, 23);
            outputHoleCountPathBox.TabIndex = 11;
            outputHoleCountPathBox.TextAlign = HorizontalAlignment.Right;
            // 
            // outputHoleCountbutton
            // 
            outputHoleCountbutton.Location = new Point(438, 38);
            outputHoleCountbutton.Name = "outputHoleCountbutton";
            outputHoleCountbutton.Size = new Size(75, 23);
            outputHoleCountbutton.TabIndex = 12;
            outputHoleCountbutton.Text = "参照";
            outputHoleCountbutton.UseVisualStyleBackColor = true;
            outputHoleCountbutton.Click += OnClickOutputHoleCountbutton;
            // 
            // splitContainer1
            // 
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(12, 795);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(outputSvgLabel);
            splitContainer1.Panel1.Controls.Add(outputSvgPathTextBox);
            splitContainer1.Panel1.Controls.Add(outputSvgButton);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(outputHoleCountLabel);
            splitContainer1.Panel2.Controls.Add(outputHoleCountbutton);
            splitContainer1.Panel2.Controls.Add(outputHoleCountPathBox);
            splitContainer1.Size = new Size(1040, 71);
            splitContainer1.SplitterDistance = 520;
            splitContainer1.TabIndex = 13;
            // 
            // prevButton
            // 
            prevButton.Enabled = false;
            prevButton.Location = new Point(12, 766);
            prevButton.Name = "prevButton";
            prevButton.Size = new Size(75, 23);
            prevButton.TabIndex = 14;
            prevButton.Text = "前へ";
            prevButton.UseVisualStyleBackColor = true;
            prevButton.Click += OnClickPrevButton;
            // 
            // nextButton
            // 
            nextButton.Enabled = false;
            nextButton.Location = new Point(978, 766);
            nextButton.Name = "nextButton";
            nextButton.Size = new Size(75, 23);
            nextButton.TabIndex = 15;
            nextButton.Text = "次へ";
            nextButton.UseVisualStyleBackColor = true;
            nextButton.Click += OnClickNextButton;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1065, 980);
            Controls.Add(nextButton);
            Controls.Add(prevButton);
            Controls.Add(splitContainer1);
            Controls.Add(saveCardButton);
            Controls.Add(holeCountDescription);
            Controls.Add(holeCountLabel);
            Controls.Add(panel);
            Controls.Add(loadSvgButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)svgDisplayBox).EndInit();
            panel.ResumeLayout(false);
            panel.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button loadSvgButton;
        private PictureBox svgDisplayBox;
        private Panel panel;
        private Label holeCountLabel;
        private Label holeCountDescription;
        private Label outputSvgLabel;
        private Label outputHoleCountLabel;
        private Button saveCardButton;
        private SaveFileDialog saveFileDialog1;
        private TextBox outputSvgPathTextBox;
        private Button outputSvgButton;
        private TextBox outputHoleCountPathBox;
        private Button outputHoleCountbutton;
        private SplitContainer splitContainer1;
        private Button prevButton;
        private Button nextButton;
    }
}