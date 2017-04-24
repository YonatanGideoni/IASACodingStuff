﻿using System;
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

        TextBox[,] varBox;
        TextBox[,] coefficientBox;
        TextBox[] answerBox;
        Panel EquationPanel;
        char[] varList;
        float[,] equationMatrice;

        public void hideStartGUI()
        {
            NumEquationBox.Enabled = false;
            NumVarBox.Enabled = false;
            StartButton.Enabled = false;

            VarNumTextBox.Visible = false;
            EquationNumTextBox.Visible = false;
            NumEquationBox.Visible = false;
            NumVarBox.Visible = false;
            StartButton.Visible = false;
        }

        public void insertEquationsToPanel(Panel eqPanel, List<char> varList, byte numEq, byte numVars)
        {
            Size varBoxSize = new Size(25, 10);
            Size coefficientBoxSize = new Size(20, 10);
            Size answerBoxSize = new Size(35, 10);

            varBox = new TextBox[numVars, numEq];
            coefficientBox = new TextBox[numVars, numEq];
            answerBox = new TextBox[numEq];

            for (byte eqIndex = 0; eqIndex < numEq; eqIndex++)
            {
                for (byte varIndex = 0; varIndex < numVars; varIndex++)
                {
                    varBox[varIndex, eqIndex] = new TextBox();
                    varBox[varIndex, eqIndex].Size = varBoxSize;
                    varBox[varIndex, eqIndex].Location = new Point(20 + varIndex * 50, 50 + eqIndex * 30);
                    varBox[varIndex, eqIndex].Text = "0";
                    varBox[varIndex, eqIndex].MaxLength = 3;

                    eqPanel.Controls.Add(varBox[varIndex, eqIndex]);

                    coefficientBox[varIndex, eqIndex] = new TextBox();
                    coefficientBox[varIndex, eqIndex].ReadOnly = true;
                    coefficientBox[varIndex, eqIndex].BackColor = Control.DefaultBackColor;
                    coefficientBox[varIndex, eqIndex].BorderStyle = System.Windows.Forms.BorderStyle.None;
                    coefficientBox[varIndex, eqIndex].Location = new Point(varBox[varIndex, eqIndex].Width + varBox[varIndex, eqIndex].Location.X + 3,
                                                                                                             varBox[varIndex, eqIndex].Location.Y);
                    coefficientBox[varIndex, eqIndex].Size = coefficientBoxSize;
                    coefficientBox[varIndex, eqIndex].Text += varList[varIndex];
                    coefficientBox[varIndex, eqIndex].Font = new Font(coefficientBox[varIndex, eqIndex].ToString(), (float)(10));
                    coefficientBox[varIndex, eqIndex].TextAlign = HorizontalAlignment.Left;
                    if (varIndex < numVars - 1)
                    {
                        coefficientBox[varIndex, eqIndex].Text += " +";
                    }
                    else
                    {
                        coefficientBox[varIndex, eqIndex].Text += " =";
                    }

                    eqPanel.Controls.Add(coefficientBox[varIndex, eqIndex]);
                }
                answerBox[eqIndex] = new TextBox();
                answerBox[eqIndex].Size = answerBoxSize;
                answerBox[eqIndex].Location = new Point(coefficientBox[numVars - 1, eqIndex].Location.X + coefficientBox[numVars - 1, eqIndex].Width,
                                                        coefficientBox[numVars - 1, eqIndex].Location.Y);
                answerBox[eqIndex].Text = "0";
                answerBox[eqIndex].MaxLength = 5;

                eqPanel.Controls.Add(answerBox[eqIndex]);
            }
        }

        public char[] createEquations(Panel eqPanel, byte numEq, byte numVars)
        {
            List<char> varList = new List<char>() { 'x', 'y', 'z' };

            List<char> extraVarList = new List<char> { 'a', 'b', 'c','w', 't', 'n', 'm' };

            while (varList.Count < numVars)
            {
                varList.Add(extraVarList[0]);
                extraVarList.RemoveAt(0);
            }

            insertEquationsToPanel(eqPanel, varList, numEq, numVars);

            eqPanel.Size = new Size(70 + numVars * 50, 80 + numEq * 30);

            return (varList.ToArray());
        }

        public void addEquationPanel(byte numEq, byte numVars)
        {
            EquationPanel = new Panel();
            EquationPanel.Location = new Point(0, 0);

            varList = createEquations(EquationPanel, numEq, numVars);

            this.Controls.Add(EquationPanel);
            this.Size = new Size(100 + EquationPanel.Width, 100 + EquationPanel.Height);
            EquationPanel.Size = this.Size;
        }

        public void addSolveButton()
        {
            Button solveButton = new Button();
            solveButton.Size = new Size(100, 50);
            solveButton.Location = new Point((EquationPanel.Width - solveButton.Width) / 2 - 35, answerBox[answerBox.Length - 1].Location.Y+50);
            solveButton.Click += solveButton_Click;
            solveButton.Text = "Solve!";
            solveButton.Font = new Font(solveButton.Text, (float)(15));          

            EquationPanel.Controls.Add(solveButton);
        }

        void solveButton_Click(object sender, EventArgs e)
        {
            byte varNum = (byte)(varBox.GetLength(0)+1);
            byte eqNum = (byte)(varBox.GetLength(1)+1);
            equationMatrice = new float[varNum + 1, eqNum];


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
                hideStartGUI();

                addEquationPanel(numEquations, numVars);

                addSolveButton();
            }
        }
    }
}