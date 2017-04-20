namespace MatrixLinearEquationsSolver
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
            this.EquationNumTextBox = new System.Windows.Forms.TextBox();
            this.VarNumTextBox = new System.Windows.Forms.TextBox();
            this.NumEquationBox = new System.Windows.Forms.NumericUpDown();
            this.NumVarBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NumEquationBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVarBox)).BeginInit();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(13, 13);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(99, 23);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Create Equations";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // EquationNumTextBox
            // 
            this.EquationNumTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.EquationNumTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EquationNumTextBox.Enabled = false;
            this.EquationNumTextBox.Location = new System.Drawing.Point(119, 13);
            this.EquationNumTextBox.Name = "EquationNumTextBox";
            this.EquationNumTextBox.Size = new System.Drawing.Size(102, 13);
            this.EquationNumTextBox.TabIndex = 1;
            this.EquationNumTextBox.Text = "Number of Equations:";
            // 
            // VarNumTextBox
            // 
            this.VarNumTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.VarNumTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.VarNumTextBox.Enabled = false;
            this.VarNumTextBox.Location = new System.Drawing.Point(119, 42);
            this.VarNumTextBox.Name = "VarNumTextBox";
            this.VarNumTextBox.Size = new System.Drawing.Size(102, 13);
            this.VarNumTextBox.TabIndex = 2;
            this.VarNumTextBox.Text = "Number of Variables:";
            // 
            // NumEquationBox
            // 
            this.NumEquationBox.Location = new System.Drawing.Point(228, 13);
            this.NumEquationBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumEquationBox.Name = "NumEquationBox";
            this.NumEquationBox.Size = new System.Drawing.Size(44, 20);
            this.NumEquationBox.TabIndex = 3;
            this.NumEquationBox.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // NumVarBox
            // 
            this.NumVarBox.Location = new System.Drawing.Point(227, 40);
            this.NumVarBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumVarBox.Name = "NumVarBox";
            this.NumVarBox.Size = new System.Drawing.Size(44, 20);
            this.NumVarBox.TabIndex = 4;
            this.NumVarBox.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.NumVarBox);
            this.Controls.Add(this.NumEquationBox);
            this.Controls.Add(this.VarNumTextBox);
            this.Controls.Add(this.EquationNumTextBox);
            this.Controls.Add(this.StartButton);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.NumEquationBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVarBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox EquationNumTextBox;
        private System.Windows.Forms.TextBox VarNumTextBox;
        private System.Windows.Forms.NumericUpDown NumEquationBox;
        private System.Windows.Forms.NumericUpDown NumVarBox;
    }
}

