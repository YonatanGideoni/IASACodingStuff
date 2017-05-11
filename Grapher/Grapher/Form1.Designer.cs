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
            this.FunctionText = new System.Windows.Forms.TextBox();
            this.ParseButton = new System.Windows.Forms.Button();
            this.MinXVal = new System.Windows.Forms.NumericUpDown();
            this.MaxXVal = new System.Windows.Forms.NumericUpDown();
            this.MinXBox = new System.Windows.Forms.TextBox();
            this.MaxXBox = new System.Windows.Forms.TextBox();
            this.graphPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.MinXVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxXVal)).BeginInit();
            this.SuspendLayout();
            // 
            // FunctionText
            // 
            this.FunctionText.Location = new System.Drawing.Point(12, 41);
            this.FunctionText.Name = "FunctionText";
            this.FunctionText.Size = new System.Drawing.Size(100, 20);
            this.FunctionText.TabIndex = 1;
            // 
            // ParseButton
            // 
            this.ParseButton.Location = new System.Drawing.Point(12, 12);
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
            this.MinXVal.Location = new System.Drawing.Point(185, 40);
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
            5,
            0,
            0,
            -2147483648});
            // 
            // MaxXVal
            // 
            this.MaxXVal.DecimalPlaces = 3;
            this.MaxXVal.Location = new System.Drawing.Point(185, 14);
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
            5,
            0,
            0,
            0});
            // 
            // MinXBox
            // 
            this.MinXBox.BackColor = System.Drawing.SystemColors.Control;
            this.MinXBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MinXBox.Location = new System.Drawing.Point(119, 42);
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
            this.MaxXBox.Location = new System.Drawing.Point(119, 17);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 722);
            this.Controls.Add(this.graphPanel);
            this.Controls.Add(this.MaxXBox);
            this.Controls.Add(this.MinXBox);
            this.Controls.Add(this.MaxXVal);
            this.Controls.Add(this.MinXVal);
            this.Controls.Add(this.ParseButton);
            this.Controls.Add(this.FunctionText);
            this.Name = "Form1";
            this.Text = "Form1";
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
    }
}

