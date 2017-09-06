namespace Grapher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.FunctionText = new System.Windows.Forms.TextBox();
            this.ParseButton = new System.Windows.Forms.Button();
            this.MinXVal = new System.Windows.Forms.NumericUpDown();
            this.MaxXVal = new System.Windows.Forms.NumericUpDown();
            this.MinXBox = new System.Windows.Forms.TextBox();
            this.MaxXBox = new System.Windows.Forms.TextBox();
            this.graphPanel = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.MaxYVal = new System.Windows.Forms.NumericUpDown();
            this.MinYVal = new System.Windows.Forms.NumericUpDown();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.AngleBar = new System.Windows.Forms.HScrollBar();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.ResolutionBox = new System.Windows.Forms.NumericUpDown();
            this.ContourBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.MinXVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxXVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxYVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinYVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResolutionBox)).BeginInit();
            this.SuspendLayout();
            // 
            // FunctionText
            // 
            this.FunctionText.Location = new System.Drawing.Point(61, 43);
            this.FunctionText.Name = "FunctionText";
            this.FunctionText.Size = new System.Drawing.Size(100, 20);
            this.FunctionText.TabIndex = 1;
            // 
            // ParseButton
            // 
            this.ParseButton.Location = new System.Drawing.Point(70, 10);
            this.ParseButton.Name = "ParseButton";
            this.ParseButton.Size = new System.Drawing.Size(75, 23);
            this.ParseButton.TabIndex = 2;
            this.ParseButton.Text = "Parse";
            this.ParseButton.UseVisualStyleBackColor = true;
            this.ParseButton.Click += new System.EventHandler(this.ParseButton_Click);
            // 
            // MinXVal
            // 
            this.MinXVal.DecimalPlaces = 2;
            this.MinXVal.Location = new System.Drawing.Point(527, 44);
            this.MinXVal.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.MinXVal.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.MinXVal.Name = "MinXVal";
            this.MinXVal.Size = new System.Drawing.Size(48, 20);
            this.MinXVal.TabIndex = 3;
            this.MinXVal.Value = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            // 
            // MaxXVal
            // 
            this.MaxXVal.DecimalPlaces = 2;
            this.MaxXVal.Location = new System.Drawing.Point(527, 18);
            this.MaxXVal.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.MaxXVal.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.MaxXVal.Name = "MaxXVal";
            this.MaxXVal.Size = new System.Drawing.Size(48, 20);
            this.MaxXVal.TabIndex = 4;
            this.MaxXVal.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // MinXBox
            // 
            this.MinXBox.BackColor = System.Drawing.SystemColors.Control;
            this.MinXBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MinXBox.Location = new System.Drawing.Point(461, 46);
            this.MinXBox.MaxLength = 5;
            this.MinXBox.Name = "MinXBox";
            this.MinXBox.ReadOnly = true;
            this.MinXBox.Size = new System.Drawing.Size(60, 13);
            this.MinXBox.TabIndex = 5;
            this.MinXBox.Text = "Minimum X:";
            // 
            // MaxXBox
            // 
            this.MaxXBox.BackColor = System.Drawing.SystemColors.Control;
            this.MaxXBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MaxXBox.Location = new System.Drawing.Point(461, 21);
            this.MaxXBox.MaxLength = 5;
            this.MaxXBox.Name = "MaxXBox";
            this.MaxXBox.ReadOnly = true;
            this.MaxXBox.Size = new System.Drawing.Size(60, 13);
            this.MaxXBox.TabIndex = 6;
            this.MaxXBox.Text = "Maximum X:";
            // 
            // graphPanel
            // 
            this.graphPanel.BackColor = System.Drawing.Color.White;
            this.graphPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.graphPanel.Location = new System.Drawing.Point(13, 68);
            this.graphPanel.Name = "graphPanel";
            this.graphPanel.Size = new System.Drawing.Size(675, 642);
            this.graphPanel.TabIndex = 7;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(17, 44);
            this.textBox1.MaxLength = 5;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(38, 17);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "f(x,y)=";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(581, 21);
            this.textBox2.MaxLength = 5;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(60, 13);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = "Maximum Y:";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Control;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(581, 46);
            this.textBox3.MaxLength = 5;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(60, 13);
            this.textBox3.TabIndex = 11;
            this.textBox3.Text = "Minimum Y:";
            // 
            // MaxYVal
            // 
            this.MaxYVal.DecimalPlaces = 2;
            this.MaxYVal.Location = new System.Drawing.Point(647, 18);
            this.MaxYVal.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.MaxYVal.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.MaxYVal.Name = "MaxYVal";
            this.MaxYVal.Size = new System.Drawing.Size(50, 20);
            this.MaxYVal.TabIndex = 10;
            this.MaxYVal.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // MinYVal
            // 
            this.MinYVal.DecimalPlaces = 2;
            this.MinYVal.Location = new System.Drawing.Point(647, 44);
            this.MinYVal.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.MinYVal.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.MinYVal.Name = "MinYVal";
            this.MinYVal.Size = new System.Drawing.Size(50, 20);
            this.MinYVal.TabIndex = 9;
            this.MinYVal.Value = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.Control;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(294, 21);
            this.textBox4.MaxLength = 5;
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(69, 13);
            this.textBox4.TabIndex = 14;
            this.textBox4.Text = "Viewing Angle:";
            // 
            // AngleBar
            // 
            this.AngleBar.Location = new System.Drawing.Point(270, 44);
            this.AngleBar.Maximum = 90;
            this.AngleBar.Name = "AngleBar";
            this.AngleBar.Size = new System.Drawing.Size(108, 17);
            this.AngleBar.TabIndex = 15;
            this.AngleBar.Value = 45;
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.SystemColors.Control;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Location = new System.Drawing.Point(162, 19);
            this.textBox5.MaxLength = 5;
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(60, 13);
            this.textBox5.TabIndex = 16;
            this.textBox5.Text = "Resolution:";
            // 
            // ResolutionBox
            // 
            this.ResolutionBox.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ResolutionBox.Location = new System.Drawing.Point(218, 17);
            this.ResolutionBox.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ResolutionBox.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ResolutionBox.Name = "ResolutionBox";
            this.ResolutionBox.Size = new System.Drawing.Size(50, 20);
            this.ResolutionBox.TabIndex = 17;
            this.ResolutionBox.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // ContourBox
            // 
            this.ContourBox.AutoSize = true;
            this.ContourBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ContourBox.Location = new System.Drawing.Point(181, 47);
            this.ContourBox.Name = "ContourBox";
            this.ContourBox.Size = new System.Drawing.Size(63, 17);
            this.ContourBox.TabIndex = 18;
            this.ContourBox.Text = "Contour";
            this.ContourBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 722);
            this.Controls.Add(this.ContourBox);
            this.Controls.Add(this.ResolutionBox);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.AngleBar);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.MaxYVal);
            this.Controls.Add(this.MinYVal);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.graphPanel);
            this.Controls.Add(this.MaxXBox);
            this.Controls.Add(this.MinXBox);
            this.Controls.Add(this.MaxXVal);
            this.Controls.Add(this.MinXVal);
            this.Controls.Add(this.ParseButton);
            this.Controls.Add(this.FunctionText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Grapher-By JRG";
            ((System.ComponentModel.ISupportInitialize)(this.MinXVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxXVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxYVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinYVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResolutionBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FunctionText;
        private System.Windows.Forms.Button ParseButton;
        private System.Windows.Forms.NumericUpDown MinXVal;
        private System.Windows.Forms.NumericUpDown MaxXVal;
        private System.Windows.Forms.TextBox MinXBox;
        private System.Windows.Forms.TextBox MaxXBox;
        private System.Windows.Forms.Panel graphPanel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.NumericUpDown MaxYVal;
        private System.Windows.Forms.NumericUpDown MinYVal;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.HScrollBar AngleBar;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.NumericUpDown ResolutionBox;
        private System.Windows.Forms.CheckBox ContourBox;
    }
}

