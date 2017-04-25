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

        TextBox[,] varBox;
        TextBox[,] coefficientBox;
        TextBox[] answerBox;
        Panel EquationPanel;
        char[] varList;
        float[,] equationMatrice;
        byte[] lineVarsCount;

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
            Size coefficientBoxSize = new Size(25, 10);
            Size varBoxSize = new Size(20, 10);
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

            List<char> extraVarList = new List<char> { 'a', 'b', 'c', 'w', 't', 'n', 'm' };

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
            solveButton.Location = new Point((EquationPanel.Width - solveButton.Width) / 2 - 35, answerBox[answerBox.Length - 1].Location.Y + 50);
            solveButton.Click += solveButton_Click;
            solveButton.Text = "Solve!";
            solveButton.Font = new Font(solveButton.Text, (float)(15));

            EquationPanel.Controls.Add(solveButton);
        }

        static void showMatrice(float[,] matrice, string title)
        {
            string matriceString = title + Environment.NewLine;

            for (byte i = 0; i < matrice.GetLength(1); i++)
            {
                for (byte j = 0; j < matrice.GetLength(0); j++)
                {
                    matriceString += matrice[j, i].ToString() + " ";
                }
                matriceString += Environment.NewLine;
            }

            MessageBox.Show(matriceString);
        }

        public byte numVarsPerLine(byte lineToCount, float[,] matrice)
        {
            byte numVars = 0;
            for (byte i = 0; i < matrice.GetLength(0) - 1; i++)
            {
                if (matrice[i, lineToCount] != 0)
                {
                    numVars++;
                }
            }
            return numVars;
        }

        static void swapRows(float[,] matrice, byte row1, byte row2)
        {
            float[,] tempMatrice = (float[,])(matrice.Clone());

            for (byte i = 0; i < matrice.GetLength(0); i++)
            {
                matrice[i, row1] = tempMatrice[i, row2];
                matrice[i, row2] = tempMatrice[i, row1];
            }
        }

        static void scaleRow(float[,] matrice, byte row, float scalar)
        {
            float[,] tempMatrice = (float[,])(matrice.Clone());

            for (byte i = 0; i < matrice.GetLength(0); i++)
            {
                matrice[i, row] = tempMatrice[i, row] * scalar;                
            }
        }

        static void nullifyErrors(float[,] matrice)
        {
            for (byte i = 0; i < matrice.GetLength(0); i++)
            {
                for (byte j = 0; j < matrice.GetLength(1); j++)
                {
                    if (Math.Abs(matrice[i, j]) < 0.0001)
                    {
                        matrice[i, j] = 0;
                    }
                }
            }
        }

        static void addRows(float[,] matrice, byte rowAddedTo, byte rowAdded)
        {
            for (byte i = 0; i < matrice.GetLength(0); i++)
            {
                matrice[i, rowAddedTo] += matrice[i, rowAdded];
            }
        }

        public void arrangeMatrice()
        {
            lineVarsCount = new byte[equationMatrice.GetLength(1)];

            for (byte i = 0; i < equationMatrice.GetLength(1); i++)
            {
                lineVarsCount[i] = numVarsPerLine(i, equationMatrice);
            }

            //bubble sort
            for (byte amountSorted = 0; amountSorted < equationMatrice.GetLength(1); amountSorted++)
            {
                for (byte sorter = 0; sorter < equationMatrice.GetLength(1) - amountSorted - 1; sorter++)
                {
                    if (lineVarsCount[sorter] > lineVarsCount[sorter + 1])
                    {
                        swapRows(equationMatrice, sorter, (byte)(sorter + 1));
                        byte tempVar = lineVarsCount[sorter];
                        lineVarsCount[sorter] = lineVarsCount[sorter + 1];
                        lineVarsCount[sorter + 1] = tempVar;
                    }
                }
            }
        }

        public void eliminateVar(float[,] matrice, byte varElim, byte startRow)
        {
            byte row = startRow;

            for (; row < matrice.GetLength(1) - 1; row++)
            {
                for (byte i = 0; i < matrice.GetLength(0) - 1 && i < varElim + 1; i++)
                {
                    if (matrice[i, row] != 0 && i != varElim)
                    {
                        break;
                    }
                    else if (i == varElim)
                    {
                        for (byte j = (byte)(row + 1); j < matrice.GetLength(1); j++)
                        {
                            if (matrice[i, j] != 0)
                            {
                                scaleRow(matrice, row, -matrice[i, j] / matrice[varElim, row]);
                                addRows(matrice, j, row);
                            }
                        }
                        break;
                    }
                }
            }

            nullifyErrors(matrice);
        }

        static bool solvable(float[,] matrice, byte[] lineVarsCount)
        {
            byte isSolvable = 0;
            byte equationsLost = 0;
            for (byte i = 0; i < matrice.GetLength(1); i++)
            {
                if (lineVarsCount[i] == 0)
                {
                    if (matrice[matrice.GetLength(0) - 1, i] == 0)
                    {
                        equationsLost++;
                        if (matrice.GetLength(1) - equationsLost < matrice.GetLength(0) - 1)
                        {
                            isSolvable = 1;
                        }
                    }
                    else
                    {
                        isSolvable = 2;
                        break;
                    }
                }
            }

            if (isSolvable != 0)
            {
                if (isSolvable == 1)
                {
                    MessageBox.Show("Sorry, there is no unique solution to this set of equations.");
                }
                else if (isSolvable == 2)
                {
                    MessageBox.Show("Sorry, there is no solution to this set of equations.");
                }
                return false;
            }
            return true;
        }

        void solveButton_Click(object sender, EventArgs e)
        {
            byte varNum = (byte)(varBox.GetLength(0));
            byte eqNum = (byte)(varBox.GetLength(1));
            float[] solutionMatrice = new float[varList.Length];
            for (byte i = 0; i < solutionMatrice.Length - 1; i++, solutionMatrice[i] = float.MaxValue) { }
            equationMatrice = new float[varNum + 1, eqNum];

            for (byte i = 0; i < eqNum; i++)
            {
                for (byte j = 0; j < varNum; j++)
                {
                    if (!float.TryParse(varBox[j, i].Text, out equationMatrice[j, i]))
                    {
                        MessageBox.Show("Please enter numbers as coefficients, not text!");
                        return;
                    }
                }
                if (!float.TryParse(answerBox[i].Text, out equationMatrice[varNum, i]))
                {
                    MessageBox.Show("Please enter numbers as answers, not text!");
                    return;
                }
            }

            arrangeMatrice();

            showMatrice(equationMatrice, "Matrice Arranged");

            //make matrice echelon form
            for (byte row = 0; row < eqNum; row++)
            {
                for (byte i = 0; i < varNum; i++)
                {
                    if (equationMatrice[i, row] != 0)
                    {
                        eliminateVar(equationMatrice, i, row);
                        showMatrice(equationMatrice, (i+1).ToString()+" variables eliminated");
                        for (byte j = row; j < eqNum; j++)
                        {
                            lineVarsCount[j] = numVarsPerLine(j, equationMatrice);
                        }
                    }
                }
            }

            //check if there's a solution, unqie or at all
            if (!solvable(equationMatrice, lineVarsCount))
            {
                return;
            }

            //back-substitue and show answers
            for (sbyte row = (sbyte)(eqNum - 1); row > -1; row--)
            {
                for (byte col = 0; col < varNum; col++)
                {
                    if (equationMatrice[col, row] != 0)
                    {
                        scaleRow(equationMatrice, (byte)(row), 1 / equationMatrice[col, row]);

                        solutionMatrice[col] = equationMatrice[varNum, row];
                        for (byte i = 0; i < row; i++)
                        {
                            equationMatrice[varNum, i] -= solutionMatrice[col] * equationMatrice[col, i];
                            equationMatrice[col, i] = 0;
                        }

                        showMatrice(equationMatrice, (eqNum-row).ToString() + " variables solved");

                        break;
                    }
                }
            }

            string answers = "Solutions:"+ Environment.NewLine;

            for (byte i = 0; i < varNum; i++)
            {
                answers += varList[i] + " = " + solutionMatrice[i].ToString();
                answers += Environment.NewLine;
            }
            MessageBox.Show(answers);
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