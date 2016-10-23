namespace Reversi
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.GridSizeBox = new System.Windows.Forms.NumericUpDown();
            this.BoardPanel = new System.Windows.Forms.Panel();
            this.TileCountBox = new System.Windows.Forms.TextBox();
            this.SkipButton = new System.Windows.Forms.Button();
            this.TurnTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.GridSizeBox)).BeginInit();
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
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.HideSelection = false;
            this.textBox1.Location = new System.Drawing.Point(127, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(52, 13);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "Grid Size:";
            // 
            // GridSizeBox
            // 
            this.GridSizeBox.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.GridSizeBox.Location = new System.Drawing.Point(176, 13);
            this.GridSizeBox.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.GridSizeBox.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.GridSizeBox.Name = "GridSizeBox";
            this.GridSizeBox.Size = new System.Drawing.Size(32, 20);
            this.GridSizeBox.TabIndex = 2;
            this.GridSizeBox.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // BoardPanel
            // 
            this.BoardPanel.Location = new System.Drawing.Point(13, 74);
            this.BoardPanel.Name = "BoardPanel";
            this.BoardPanel.Size = new System.Drawing.Size(35, 36);
            this.BoardPanel.TabIndex = 4;
            // 
            // TileCountBox
            // 
            this.TileCountBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TileCountBox.Location = new System.Drawing.Point(238, 12);
            this.TileCountBox.Name = "TileCountBox";
            this.TileCountBox.ReadOnly = true;
            this.TileCountBox.Size = new System.Drawing.Size(150, 13);
            this.TileCountBox.TabIndex = 5;
            // 
            // SkipButton
            // 
            this.SkipButton.Location = new System.Drawing.Point(13, 45);
            this.SkipButton.Name = "SkipButton";
            this.SkipButton.Size = new System.Drawing.Size(75, 23);
            this.SkipButton.TabIndex = 6;
            this.SkipButton.Text = "Skip Turn";
            this.SkipButton.UseVisualStyleBackColor = true;
            this.SkipButton.Click += new System.EventHandler(this.SkipButton_Click);
            // 
            // TurnTextBox
            // 
            this.TurnTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TurnTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TurnTextBox.Location = new System.Drawing.Point(127, 45);
            this.TurnTextBox.Name = "TurnTextBox";
            this.TurnTextBox.ReadOnly = true;
            this.TurnTextBox.Size = new System.Drawing.Size(100, 19);
            this.TurnTextBox.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 262);
            this.Controls.Add(this.TurnTextBox);
            this.Controls.Add(this.SkipButton);
            this.Controls.Add(this.TileCountBox);
            this.Controls.Add(this.BoardPanel);
            this.Controls.Add(this.GridSizeBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.RestartButton);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.GridSizeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.NumericUpDown GridSizeBox;
        private System.Windows.Forms.Panel BoardPanel;
        private System.Windows.Forms.TextBox TileCountBox;
        private System.Windows.Forms.Button SkipButton;
        private System.Windows.Forms.TextBox TurnTextBox;
    }
}

