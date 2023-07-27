namespace TopologyCardRegistrar
{
    partial class Form1
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
            btnClickThis = new Button();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            holeCountLabel = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            SaveCardButton = new Button();
            saveFileDialog1 = new SaveFileDialog();
            outputSvgPathTextBox = new TextBox();
            outputSvgButton = new Button();
            outputHoleCountPathBox = new TextBox();
            outputHoleCountbutton = new Button();
            splitContainer1 = new SplitContainer();
            prevButton = new Button();
            nextButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // btnClickThis
            // 
            btnClickThis.Location = new Point(12, 872);
            btnClickThis.Name = "btnClickThis";
            btnClickThis.Size = new Size(521, 23);
            btnClickThis.TabIndex = 0;
            btnClickThis.Text = "画像を読み込む";
            btnClickThis.UseVisualStyleBackColor = true;
            btnClickThis.Click += LoadSvgButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(714, 596);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(1040, 748);
            panel1.TabIndex = 2;
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 903);
            label1.Name = "label1";
            label1.Size = new Size(356, 32);
            label1.TabIndex = 4;
            label1.Text = "入力された画像の要素ごとの穴の数";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(3, 3);
            label2.Name = "label2";
            label2.Size = new Size(329, 32);
            label2.TabIndex = 5;
            label2.Text = "入力された画像の保存先フォルダ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(3, 2);
            label3.Name = "label3";
            label3.Size = new Size(242, 32);
            label3.TabIndex = 6;
            label3.Text = "穴の数の出力先ファイル";
            // 
            // SaveCardButton
            // 
            SaveCardButton.Enabled = false;
            SaveCardButton.Location = new Point(536, 872);
            SaveCardButton.Name = "SaveCardButton";
            SaveCardButton.Size = new Size(516, 23);
            SaveCardButton.TabIndex = 7;
            SaveCardButton.Text = "カードを保存する";
            SaveCardButton.UseVisualStyleBackColor = true;
            SaveCardButton.Click += SaveCardButton_Click;
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
            outputSvgButton.Click += OutputSvgButton_Click;
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
            outputHoleCountbutton.Click += OutputHoleCountbutton_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(12, 795);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(outputSvgPathTextBox);
            splitContainer1.Panel1.Controls.Add(outputSvgButton);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(label3);
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
            prevButton.Click += prevButton_Click;
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
            nextButton.Click += nextButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1065, 980);
            Controls.Add(nextButton);
            Controls.Add(prevButton);
            Controls.Add(splitContainer1);
            Controls.Add(SaveCardButton);
            Controls.Add(label1);
            Controls.Add(holeCountLabel);
            Controls.Add(panel1);
            Controls.Add(btnClickThis);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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

        private Button btnClickThis;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Label holeCountLabel;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button SaveCardButton;
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