using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatrixLinearEquationsSolver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            byte numEquations = (byte)(NumEquationBox.Value);
            byte numVars = (byte)(NumVarBox.Value);

            if (numVars > numEquations)
            {
                MessageBox.Show("There can't be more variables than equations.");
            }
            else
            {
                NumEquationBox.Enabled = false;
                NumVarBox.Enabled = false;
                StartButton.Enabled = false;

                EquationNumTextBox.Visible = false;
            }
        }
    }
}
