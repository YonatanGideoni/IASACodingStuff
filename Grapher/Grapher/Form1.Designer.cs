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
            ((System.ComponentModel.ISupportInitialize)(this.MinXVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxXVal)).BeginInit();
            this.SuspendLayout();
            // 
            // FunctionText
            // 
            this.FunctionText.Location = new System.Drawing.Point(93, 42);
            this.FunctionText.Name = "FunctionText";
            this.FunctionText.Size = new System.Drawing.Size(100, 20);
            this.FunctionText.TabIndex = 1;
            // 
            // ParseButton
            // 
            this.ParseButton.Location = new System.Drawing.Point(93, 9);
            this.ParseButton.Name = "ParseButton";
            this.ParseButton.Size = new System.Drawing.Size(75, 23);
            this.ParseButton.TabIndex = 2;
            this.ParseButton.Text = "Parse";
            this.ParseButton.UseVisualStyleBackColor = true;
            this.ParseButton.Click += new System.EventHandler(this.ParseButton_Click);
            // 
            // MinXVal
            // 
            this.MinXVal.DecimalPlaces = 3;
            this.MinXVal.Location = new System.Drawing.Point(344, 42);
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
            this.MinXVal.Size = new System.Drawing.Size(63, 20);
            this.MinXVal.TabIndex = 3;
            this.MinXVal.Value = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            // 
            // MaxXVal
            // 
            this.MaxXVal.DecimalPlaces = 3;
            this.MaxXVal.Location = new System.Drawing.Point(344, 16);
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
            this.MaxXVal.Size = new System.Drawing.Size(63, 20);
            this.MaxXVal.TabIndex = 4;
            this.MaxXVal.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // MinXBox
            // 
            this.MinXBox.BackColor = System.Drawing.SystemColors.Control;
            this.MinXBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MinXBox.Location = new System.Drawing.Point(278, 44);
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
            this.MaxXBox.Location = new System.Drawing.Point(278, 19);
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
            this.textBox1.Location = new System.Drawing.Point(59, 43);
            this.textBox1.MaxLength = 5;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(28, 17);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "f(x)=";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 722);
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
    }
}

