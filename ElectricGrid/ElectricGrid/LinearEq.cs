using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectricGrid
{
    class LinearEq
    {
        private byte[] lineVarsCount;
        private bool debug = false;

        private void showMatrice(float[,] matrice, string title)
        {
            if (debug)
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
        }

        private byte numVarsPerLine(byte lineToCount, float[,] matrice)
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

        private static void swapRows(float[,] matrice, byte row1, byte row2)
        {
            float[,] tempMatrice = (float[,])(matrice.Clone());

            for (byte i = 0; i < matrice.GetLength(0); i++)
            {
                matrice[i, row1] = tempMatrice[i, row2];
                matrice[i, row2] = tempMatrice[i, row1];
            }
        }

        private void scaleRow(float[,] matrice, byte row, float scalar)
        {
            if (Single.IsNaN(scalar) && debug)
            {
                MessageBox.Show("NaN acquired while scaling rows.");
                showMatrice(matrice, "Matrice which leads to NaN");
            }
            float[,] tempMatrice = (float[,])(matrice.Clone());

            for (byte i = 0; i < matrice.GetLength(0); i++)
            {
                matrice[i, row] = tempMatrice[i, row] * scalar;
            }
        }

        private static void nullifyErrors(float[,] matrice)
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

        private static void addRows(float[,] matrice, byte rowAddedTo, byte rowAdded)
        {
            for (byte i = 0; i < matrice.GetLength(0); i++)
            {
                matrice[i, rowAddedTo] += matrice[i, rowAdded];
            }
        }

        private void arrangeMatrice(float[,] matrice)
        {
            lineVarsCount = new byte[matrice.GetLength(1)];

            //counts number of vars per line
            for (byte i = 0; i < matrice.GetLength(1); i++)
            {
                lineVarsCount[i] = numVarsPerLine(i, matrice);
            }

            //bubble sort
            for (byte amountSorted = 0; amountSorted < matrice.GetLength(1); amountSorted++)
            {
                for (byte sorter = 0; sorter < matrice.GetLength(1) - amountSorted - 1; sorter++)
                {
                    if (lineVarsCount[sorter] < lineVarsCount[sorter + 1])
                    {
                        swapRows(matrice, sorter, (byte)(sorter + 1));
                        byte tempVar = lineVarsCount[sorter];
                        lineVarsCount[sorter] = lineVarsCount[sorter + 1];
                        lineVarsCount[sorter + 1] = tempVar;
                    }
                }
            }
        }

        private void eliminateVar(float[,] matrice, byte varElim, byte startRow)
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
                        if (matrice[varElim, row] != 0)
                        {
                            for (byte j = (byte)(row + 1); j < matrice.GetLength(1); j++)
                            {
                                if (matrice[i, j] != 0)
                                {
                                    scaleRow(matrice, row, -matrice[i, j] / matrice[varElim, row]);
                                    addRows(matrice, j, row);
                                }
                            }
                        }
                        break;
                    }
                }
            }

            nullifyErrors(matrice);
        }

        private static bool solvable(float[,] matrice, byte[] lineVarsCount)
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

        public float[] solveMatrice(float[,] matrice)
        {
            byte varNum = (byte)(matrice.GetLength(0)-1);
            byte eqNum = (byte)(matrice.GetLength(1));
            float[] solutionMatrice = new float[varNum];
            for (byte i = 0; i < solutionMatrice.Length - 1; i++, solutionMatrice[i] = float.MaxValue) { }

            arrangeMatrice(matrice);

            showMatrice(matrice, "Matrice Arranged");

            //make matrice echelon form
            for (byte row = 0; row < eqNum - 1; row++)
            {
                for (byte i = 0; i < varNum; i++)
                {
                    if (matrice[i, row] != 0)
                    {
                        for (byte j = row; j < eqNum; j++)
                        {
                            if (matrice[i, j] != 0)
                            {
                                eliminateVar(matrice, i, row);
                                showMatrice(matrice, (row + 1).ToString() + " variables eliminated");
                                lineVarsCount[j] = numVarsPerLine(j, matrice);
                            }
                        }
                        break;
                    }
                }
            }

            //check if there's a solution, unqie or at all
            if (!solvable(matrice, lineVarsCount))
            {
                return null;
            }

            //back-substitue and show answers
            for (sbyte row = (sbyte)(eqNum - 1); row > -1; row--)
            {
                for (byte col = 0; col < varNum; col++)
                {
                    if (matrice[col, row] != 0)
                    {
                        scaleRow(matrice, (byte)(row), 1 / matrice[col, row]);

                        solutionMatrice[col] = matrice[varNum, row];
                        for (byte i = 0; i < row; i++)
                        {
                            matrice[varNum, i] -= solutionMatrice[col] * matrice[col, i];
                            matrice[col, i] = 0;
                        }

                        showMatrice(matrice, (eqNum - row).ToString() + " variables solved");

                        break;
                    }
                }
            }

            return solutionMatrice;
        }
    }
}
