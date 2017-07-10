namespace ElectricGrid
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
            this.InitGridButton = new System.Windows.Forms.Button();
            this.GridSizeNum = new System.Windows.Forms.NumericUpDown();
            this.GridSizeText = new System.Windows.Forms.TextBox();
            this.inVoltageText = new System.Windows.Forms.TextBox();
            this.inVoltageBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.GridSizeNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inVoltageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // InitGridButton
            // 
            this.InitGridButton.Location = new System.Drawing.Point(13, 13);
            this.InitGridButton.Name = "InitGridButton";
            this.InitGridButton.Size = new System.Drawing.Size(75, 23);
            this.InitGridButton.TabIndex = 0;
            this.InitGridButton.Text = "Create Grid";
            this.InitGridButton.UseVisualStyleBackColor = true;
            this.InitGridButton.Click += new System.EventHandler(this.InitGridButton_Click);
            // 
            // GridSizeNum
            // 
            this.GridSizeNum.Location = new System.Drawing.Point(186, 13);
            this.GridSizeNum.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.GridSizeNum.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.GridSizeNum.Name = "GridSizeNum";
            this.GridSizeNum.Size = new System.Drawing.Size(37, 20);
            this.GridSizeNum.TabIndex = 1;
            this.GridSizeNum.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // GridSizeText
            // 
            this.GridSizeText.BackColor = System.Drawing.SystemColors.Control;
            this.GridSizeText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GridSizeText.Location = new System.Drawing.Point(131, 15);
            this.GridSizeText.Name = "GridSizeText";
            this.GridSizeText.Size = new System.Drawing.Size(49, 13);
            this.GridSizeText.TabIndex = 2;
            this.GridSizeText.Text = "Grid Size:";
            // 
            // inVoltageText
            // 
            this.inVoltageText.BackColor = System.Drawing.SystemColors.Control;
            this.inVoltageText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.inVoltageText.Location = new System.Drawing.Point(107, 48);
            this.inVoltageText.Name = "inVoltageText";
            this.inVoltageText.Size = new System.Drawing.Size(73, 13);
            this.inVoltageText.TabIndex = 4;
            this.inVoltageText.Text = "Input Voltage:";
            // 
            // inVoltageBox
            // 
            this.inVoltageBox.Location = new System.Drawing.Point(186, 46);
            this.inVoltageBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.inVoltageBox.Name = "inVoltageBox";
            this.inVoltageBox.Size = new System.Drawing.Size(37, 20);
            this.inVoltageBox.TabIndex = 3;
            this.inVoltageBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.inVoltageText);
            this.Controls.Add(this.inVoltageBox);
            this.Controls.Add(this.GridSizeText);
            this.Controls.Add(this.GridSizeNum);
            this.Controls.Add(this.InitGridButton);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Electric Grid";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.GridSizeNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inVoltageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button InitGridButton;
        private System.Windows.Forms.NumericUpDown GridSizeNum;
        private System.Windows.Forms.TextBox GridSizeText;
        private System.Windows.Forms.TextBox inVoltageText;
        private System.Windows.Forms.NumericUpDown inVoltageBox;
    }
}

