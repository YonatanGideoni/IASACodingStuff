namespace Project_Checkers
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RestartButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BrdSizeBox = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.WhiteCheckerBox = new System.Windows.Forms.TextBox();
            this.BlackCheckerBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.BrdSizeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // RestartButton
            // 
            this.RestartButton.Location = new System.Drawing.Point(13, 13);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(75, 23);
            this.RestartButton.TabIndex = 0;
            this.RestartButton.Text = "New Game";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.RestartButton_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(13, 76);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(43, 36);
            this.panel1.TabIndex = 1;
            // 
            // BrdSizeBox
            // 
            this.BrdSizeBox.Location = new System.Drawing.Point(183, 13);
            this.BrdSizeBox.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.BrdSizeBox.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.BrdSizeBox.Name = "BrdSizeBox";
            this.BrdSizeBox.Size = new System.Drawing.Size(36, 20);
            this.BrdSizeBox.TabIndex = 2;
            this.BrdSizeBox.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(126, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(63, 13);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "Board Size:";
            // 
            // WhiteCheckerBox
            // 
            this.WhiteCheckerBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.WhiteCheckerBox.Cursor = System.Windows.Forms.Cursors.No;
            this.WhiteCheckerBox.Location = new System.Drawing.Point(258, 13);
            this.WhiteCheckerBox.Name = "WhiteCheckerBox";
            this.WhiteCheckerBox.ReadOnly = true;
            this.WhiteCheckerBox.Size = new System.Drawing.Size(100, 13);
            this.WhiteCheckerBox.TabIndex = 4;
            this.WhiteCheckerBox.Text = "White Checkers:";
            // 
            // BlackCheckerBox
            // 
            this.BlackCheckerBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BlackCheckerBox.Cursor = System.Windows.Forms.Cursors.No;
            this.BlackCheckerBox.Location = new System.Drawing.Point(258, 32);
            this.BlackCheckerBox.Name = "BlackCheckerBox";
            this.BlackCheckerBox.ReadOnly = true;
            this.BlackCheckerBox.Size = new System.Drawing.Size(100, 13);
            this.BlackCheckerBox.TabIndex = 5;
            this.BlackCheckerBox.Text = "Black Checkers:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 347);
            this.Controls.Add(this.BlackCheckerBox);
            this.Controls.Add(this.WhiteCheckerBox);
            this.Controls.Add(this.BrdSizeBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.RestartButton);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.BrdSizeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown BrdSizeBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox WhiteCheckerBox;
        private System.Windows.Forms.TextBox BlackCheckerBox;
    }
}

