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
            this.StartButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BrdSizeBox = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.BlackCheckerBox = new System.Windows.Forms.TextBox();
            this.WhiteCheckerBox = new System.Windows.Forms.TextBox();
            this.TurnBox = new System.Windows.Forms.TextBox();
            this.CompButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.BrdSizeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(13, 13);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "New Game";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
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
            this.textBox1.BackColor = System.Drawing.Color.ForestGreen;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(126, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(63, 13);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "Board Size:";
            // 
            // BlackCheckerBox
            // 
            this.BlackCheckerBox.BackColor = System.Drawing.Color.Black;
            this.BlackCheckerBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BlackCheckerBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.BlackCheckerBox.ForeColor = System.Drawing.Color.White;
            this.BlackCheckerBox.Location = new System.Drawing.Point(254, 7);
            this.BlackCheckerBox.Name = "BlackCheckerBox";
            this.BlackCheckerBox.ReadOnly = true;
            this.BlackCheckerBox.Size = new System.Drawing.Size(100, 13);
            this.BlackCheckerBox.TabIndex = 4;
            this.BlackCheckerBox.Visible = false;
            // 
            // WhiteCheckerBox
            // 
            this.WhiteCheckerBox.BackColor = System.Drawing.Color.White;
            this.WhiteCheckerBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.WhiteCheckerBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.WhiteCheckerBox.ForeColor = System.Drawing.Color.Black;
            this.WhiteCheckerBox.Location = new System.Drawing.Point(254, 27);
            this.WhiteCheckerBox.Name = "WhiteCheckerBox";
            this.WhiteCheckerBox.ReadOnly = true;
            this.WhiteCheckerBox.Size = new System.Drawing.Size(100, 13);
            this.WhiteCheckerBox.TabIndex = 5;
            this.WhiteCheckerBox.Visible = false;
            // 
            // TurnBox
            // 
            this.TurnBox.BackColor = System.Drawing.Color.ForestGreen;
            this.TurnBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TurnBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.TurnBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TurnBox.Location = new System.Drawing.Point(148, 39);
            this.TurnBox.Name = "TurnBox";
            this.TurnBox.ReadOnly = true;
            this.TurnBox.Size = new System.Drawing.Size(100, 19);
            this.TurnBox.TabIndex = 6;
            this.TurnBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CompButton
            // 
            this.CompButton.Enabled = false;
            this.CompButton.Location = new System.Drawing.Point(13, 39);
            this.CompButton.Name = "CompButton";
            this.CompButton.Size = new System.Drawing.Size(114, 23);
            this.CompButton.TabIndex = 7;
            this.CompButton.Text = "Turn Computer ON?";
            this.CompButton.UseVisualStyleBackColor = true;
            this.CompButton.Visible = false;
            this.CompButton.Click += new System.EventHandler(this.CompButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.ForestGreen;
            this.ClientSize = new System.Drawing.Size(479, 347);
            this.Controls.Add(this.CompButton);
            this.Controls.Add(this.TurnBox);
            this.Controls.Add(this.WhiteCheckerBox);
            this.Controls.Add(this.BlackCheckerBox);
            this.Controls.Add(this.BrdSizeBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.StartButton);
            this.Name = "Form1";
            this.Text = "Checkers";
            ((System.ComponentModel.ISupportInitialize)(this.BrdSizeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown BrdSizeBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox WhiteCheckerBox;
        private System.Windows.Forms.TextBox TurnBox;
        private System.Windows.Forms.Button CompButton;
        private System.Windows.Forms.TextBox BlackCheckerBox;
    }
}

