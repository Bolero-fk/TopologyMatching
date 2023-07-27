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
            button1 = new Button();
            openFileDialog1 = new OpenFileDialog();
            openFileDialog2 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            textBox1 = new TextBox();
            button3 = new Button();
            textBox2 = new TextBox();
            button4 = new Button();
            splitContainer1 = new SplitContainer();
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
            btnClickThis.Location = new Point(12, 869);
            btnClickThis.Name = "btnClickThis";
            btnClickThis.Size = new Size(521, 23);
            btnClickThis.TabIndex = 0;
            btnClickThis.Text = "画像を読み込む";
            btnClickThis.UseVisualStyleBackColor = true;
            btnClickThis.Click += button1_Click;
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
            holeCountLabel.Location = new Point(42, 927);
            holeCountLabel.Name = "holeCountLabel";
            holeCountLabel.Size = new Size(208, 50);
            holeCountLabel.TabIndex = 3;
            holeCountLabel.Text = "Hole Count";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 895);
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
            label3.Location = new Point(3, 3);
            label3.Name = "label3";
            label3.Size = new Size(242, 32);
            label3.TabIndex = 6;
            label3.Text = "穴の数の出力先ファイル";
            // 
            // button1
            // 
            button1.Location = new Point(536, 869);
            button1.Name = "button1";
            button1.Size = new Size(516, 23);
            button1.TabIndex = 7;
            button1.Text = "カードを保存する";
            button1.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            openFileDialog2.FileName = "openFileDialog2";
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new Point(3, 38);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(433, 23);
            textBox1.TabIndex = 8;
            // 
            // button3
            // 
            button3.Location = new Point(442, 38);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 10;
            button3.Text = "参照";
            button3.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            textBox2.Enabled = false;
            textBox2.Location = new Point(3, 39);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(429, 23);
            textBox2.TabIndex = 11;
            // 
            // button4
            // 
            button4.Location = new Point(438, 38);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 12;
            button4.Text = "参照";
            button4.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new Point(12, 781);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(textBox1);
            splitContainer1.Panel1.Controls.Add(button3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(button4);
            splitContainer1.Panel2.Controls.Add(textBox2);
            splitContainer1.Size = new Size(1040, 82);
            splitContainer1.SplitterDistance = 520;
            splitContainer1.TabIndex = 13;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1065, 991);
            Controls.Add(splitContainer1);
            Controls.Add(button1);
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
        private Button button1;
        private OpenFileDialog openFileDialog1;
        private OpenFileDialog openFileDialog2;
        private SaveFileDialog saveFileDialog1;
        private TextBox textBox1;
        private Button button3;
        private TextBox textBox2;
        private Button button4;
        private SplitContainer splitContainer1;
    }
}