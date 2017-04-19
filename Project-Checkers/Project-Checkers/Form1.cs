using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Checkers
{
    public partial class Form1 : Form
    {
        short[,] intBrd;
        Button[,] Board;
        short BlackCheckers;
        short WhiteCheckers;
        short BrdSize;
        short[] PressedButton = new short[2];
        short[][] DyingButtons = new short[4][];
        short turnVal = 2;
        bool compActive = false;

        public Form1()
        {
            InitializeComponent();
        }

        static short BrdScore(short[,] OriginBrd)
        {
            short Score = 0;
            short whiteCheckers = 0;
            short blackCheckers = 0;
            short BrdSize = (short)(OriginBrd.GetLength(0));

            for (short i = 0; i < BrdSize; i++)
            {
                for (short k = 0; k < BrdSize; k++)
                {
                    if (OriginBrd[i, k] == 0)
                    {
                        continue;
                    }
                    else if (OriginBrd[i, k] == 2)
                    {
                        whiteCheckers++;
                        Score += (short)(k + 1 - BrdSize);
                    }
                    else if (OriginBrd[i, k] == 1)
                    {
                        blackCheckers++;
                        Score += k;
                    }
                    else if (OriginBrd[i, k] == -2)
                    {
                        whiteCheckers++;
                        Score -= (short)(BrdSize * 4);
                    }
                    else if (OriginBrd[i, k] == -1)
                    {
                        blackCheckers++;
                        Score += (short)(BrdSize * 4);
                    }
                }
            }

            if (whiteCheckers <= 1)//check if loss condition
            {
                Score -= 9999;
            }
            else if (blackCheckers <= 1)
            {
                Score += 9999;
            }

            Score += (short)((whiteCheckers - blackCheckers) * 4);
            return Score;
        }

        public void CleanSelected()
        {
            short BrdSize = (short)(intBrd.GetLength(0));
            for (short i = 0; i < BrdSize; i++)
            {
                for (short k = 0; k < BrdSize; k++)
                {
                    if (intBrd[i, k] != 3)
                    {
                        continue;
                    }
                    else
                    {
                        Board[i, k].BackgroundImage = null;
                        intBrd[i, k] = 0;
                    }
                }
            }
        }

        public void winCheck()
        {
            if (BlackCheckers == 1)
            {
                MessageBox.Show("White Wins!");
                this.Close();
            }
            else if (WhiteCheckers == 1)
            {
                MessageBox.Show("Black Wins!");
                this.Close();
            }
        }

        public short[] CompMove(short[,] originBrd)
        {
            short[][,] tempBrd = new short[2][,];
            tempBrd[0] = (short[,])originBrd.Clone();
            short[] MoveScore = new short[3] { -1000, 1000, -1000 };
            short[] ButtonLoc = null;
            short[] col = new short[2];
            short[] row = new short[2];

            for (row[0] = 0; row[0] < BrdSize; row[0]++)
            {
                for (col[0] = 0; col[0] < BrdSize; col[0]++)
                {
                    if (tempBrd[0][col[0], row[0]] == 1)//simulate normal checker move
                    {
                        if (col[0] != 0 && row[0] != BrdSize - 1)
                        {
                            if (tempBrd[0][col[0] - 1, row[0] + 1] == 2 || tempBrd[0][col[0] - 1, row[0] + 1] == -2)
                            {
                                if (col[0] != 1 && row[0] != BrdSize - 2)
                                {
                                    if (tempBrd[0][col[0] - 2, row[0] + 2] == 0)
                                    {
                                        if (row[0] + 2 == BrdSize - 1)
                                        {
                                            tempBrd[0][col[0] - 2, row[0] + 2] = -1;
                                        }
                                        else
                                        {
                                            tempBrd[0][col[0] - 2, row[0] + 2] = 1;
                                        }
                                        tempBrd[0][col[0] - 1, row[0] + 1] = 0;
                                        tempBrd[0][col[0], row[0]] = 0;

                                        #region 2-Move-AI
                                        MoveScore[1] = 1000;
                                        tempBrd[1] = (short[,])tempBrd[0].Clone();
                                        for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                        {
                                            for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                            {
                                                if (tempBrd[1][col[1], row[1]] == 2)
                                                {
                                                    if (col[1] != 0 && row[1] != 0)
                                                    {
                                                        if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                        {
                                                            if (col[1] != 1 && row[1] != 1)
                                                            {
                                                                if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                                {
                                                                    if (row[1] - 2 == 0)
                                                                    {
                                                                        tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                    }
                                                                    else
                                                                    {
                                                                        tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                    }
                                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                    tempBrd[1][col[1], row[1]] = 0;

                                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                    if (MoveScore[2] < MoveScore[1])
                                                                    {
                                                                        MoveScore[1] = MoveScore[2];
                                                                    }

                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                    tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                    tempBrd[1][col[1], row[1]] = 2;
                                                                }
                                                            }
                                                        }
                                                        else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                        {
                                                            if (row[1] - 1 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                            }
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                    if (col[1] != BrdSize - 1 && row[1] != 0)
                                                    {
                                                        if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                        {
                                                            if (col[1] != BrdSize - 2 && row[1] != 1)
                                                            {
                                                                if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                                {
                                                                    if (row[1] - 2 == 0)
                                                                    {
                                                                        tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                    }
                                                                    else
                                                                    {
                                                                        tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                    }

                                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                    tempBrd[1][col[1], row[1]] = 0;

                                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                    if (MoveScore[2] < MoveScore[1])
                                                                    {
                                                                        MoveScore[1] = MoveScore[2];
                                                                    }

                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                    tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                    tempBrd[1][col[1], row[1]] = 2;
                                                                }
                                                            }
                                                        }
                                                        else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                        {
                                                            if (row[1] - 1 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                            }
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1], row[1]] == -2)
                                                {
                                                    for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                    {
                                                        if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                        {
                                                            if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                                tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                                tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                    {
                                                        if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                        {
                                                            if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                                tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                                tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                    {
                                                        if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                        {
                                                            if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                                tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                                tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                    {
                                                        if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                        {
                                                            if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                                tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[2] = MoveScore[1];
                                                                }

                                                                tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                                tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        if (MoveScore[1] > MoveScore[0])
                                        {
                                            MoveScore[0] = MoveScore[1];
                                            ButtonLoc = new short[6] { col[0], row[0], (short)(col[0] - 2), (short)(row[0] + 2), (short)(col[0] - 1), (short)(row[0] + 1) };
                                        }

                                        tempBrd[0][col[0] - 2, row[0] + 2] = 0;
                                        tempBrd[0][col[0] - 1, row[0] + 1] = originBrd[col[0] - 1, row[0] + 1];
                                        tempBrd[0][col[0], row[0]] = 1;
                                    }
                                }
                            }
                            else if (tempBrd[0][col[0] - 1, row[0] + 1] == 0)
                            {
                                if (row[0] + 1 == BrdSize - 1)
                                {
                                    tempBrd[0][col[0] - 1, row[0] + 1] = -1;
                                }
                                else
                                {
                                    tempBrd[0][col[0] - 1, row[0] + 1] = 1;
                                }
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 2-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                    ButtonLoc = new short[4] { col[0], row[0], (short)(col[0] - 1), (short)(row[0] + 1) };
                                }

                                tempBrd[0][col[0] - 1, row[0] + 1] = 0;
                                tempBrd[0][col[0], row[0]] = 1;
                            }
                        }
                        if (col[0] != BrdSize - 1 && row[0] != BrdSize - 1)
                        {
                            if (tempBrd[0][col[0] + 1, row[0] + 1] == 2 || tempBrd[0][col[0] + 1, row[0] + 1] == -2)
                            {
                                if (col[0] != BrdSize - 2 && row[0] != BrdSize - 2)
                                {
                                    if (tempBrd[0][col[0] + 2, row[0] + 2] == 0)
                                    {
                                        tempBrd[0][col[0] + 2, row[0] + 2] = 1;
                                        tempBrd[0][col[0] + 1, row[0] + 1] = 0;
                                        tempBrd[0][col[0], row[0]] = 0;

                                        #region 2-Move-AI
                                        MoveScore[1] = 1000;
                                        tempBrd[1] = (short[,])tempBrd[0].Clone();
                                        for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                        {
                                            for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                            {
                                                if (tempBrd[1][col[1], row[1]] == 2)
                                                {
                                                    if (col[1] != 0 && row[1] != 0)
                                                    {
                                                        if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                        {
                                                            if (col[1] != 1 && row[1] != 1)
                                                            {
                                                                if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                                {
                                                                    if (row[1] - 2 == 0)
                                                                    {
                                                                        tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                    }
                                                                    else
                                                                    {
                                                                        tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                    }
                                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                    tempBrd[1][col[1], row[1]] = 0;

                                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                    if (MoveScore[2] < MoveScore[1])
                                                                    {
                                                                        MoveScore[1] = MoveScore[2];
                                                                    }

                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                    tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                    tempBrd[1][col[1], row[1]] = 2;
                                                                }
                                                            }
                                                        }
                                                        else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                        {
                                                            if (row[1] - 1 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                            }
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                    if (col[1] != BrdSize - 1 && row[1] != 0)
                                                    {
                                                        if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                        {
                                                            if (col[1] != BrdSize - 2 && row[1] != 1)
                                                            {
                                                                if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                                {
                                                                    if (row[1] - 2 == 0)
                                                                    {
                                                                        tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                    }
                                                                    else
                                                                    {
                                                                        tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                    }

                                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                    tempBrd[1][col[1], row[1]] = 0;

                                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                    if (MoveScore[2] < MoveScore[1])
                                                                    {
                                                                        MoveScore[1] = MoveScore[2];
                                                                    }

                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                    tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                    tempBrd[1][col[1], row[1]] = 2;
                                                                }
                                                            }
                                                        }
                                                        else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                        {
                                                            if (row[1] - 1 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                            }
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1], row[1]] == -2)
                                                {
                                                    for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                    {
                                                        if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                        {
                                                            if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                                tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                                tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                    {
                                                        if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                        {
                                                            if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                                tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                                tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                    {
                                                        if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                        {
                                                            if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                                tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                                tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                    {
                                                        if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                        {
                                                            if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                                tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[2] = MoveScore[1];
                                                                }

                                                                tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                                tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        if (MoveScore[1] > MoveScore[0])
                                        {
                                            MoveScore[0] = MoveScore[1];
                                            ButtonLoc = new short[6] { col[0], row[0], (short)(col[0] + 2), (short)(row[0] + 2), (short)(col[0] + 1), (short)(row[0] + 1) };
                                        }

                                        tempBrd[0][col[0] + 2, row[0] + 2] = 0;
                                        tempBrd[0][col[0] + 1, row[0] + 1] = originBrd[col[0] + 1, row[0] + 1];
                                        tempBrd[0][col[0], row[0]] = 1;
                                    }
                                }
                            }
                            else if (tempBrd[0][col[0] + 1, row[0] + 1] != 1 && tempBrd[0][col[0] + 1, row[0] + 1] != -1)
                            {
                                tempBrd[0][col[0] + 1, row[0] + 1] = originBrd[col[0], row[0]];
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 2-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                    ButtonLoc = new short[4] { col[0], row[0], (short)(col[0] + 1), (short)(row[0] + 1) };
                                }

                                tempBrd[0][col[0] + 1, row[0] + 1] = 0;
                                tempBrd[0][col[0], row[0]] = originBrd[col[0], row[0]];
                            }
                        }
                    }
                    else if (tempBrd[0][col[0], row[0]] == -1)//simulate king checker move
                    {
                        for (short i = 1; i < row[0] + 1 && i < col[0] + 1; i++)
                        {
                            if (tempBrd[0][col[0] - i, row[0] - i] == 0)
                            {
                                tempBrd[0][col[0] - i, row[0] - i] = -1;
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 2-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                    ButtonLoc = new short[4] { col[0], row[0], (short)(col[0] - i), (short)(row[0] - i) };
                                }

                                tempBrd[0][col[0] - i, row[0] - i] = 0;
                                tempBrd[0][col[0], row[0]] = -1;
                            }
                            else if (tempBrd[0][col[0] - i, row[0] - i] == 1 || tempBrd[0][col[0] - i, row[0] - i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col[0] - i, row[0] - i] == 2 || tempBrd[0][col[0] - i, row[0] - i] == -2)
                            {
                                if (row[0] - i > 0 && col[0] - i > 0 && tempBrd[0][col[0] - i - 1, row[0] - i - 1] == 0)
                                {
                                    tempBrd[0][col[0] - i - 1, row[0] - i - 1] = -1;
                                    tempBrd[0][col[0] - i, row[0] - i] = 0;
                                    tempBrd[0][col[0], row[0]] = 0;

                                    #region 2-Move-AI
                                    MoveScore[1] = 1000;
                                    tempBrd[1] = (short[,])tempBrd[0].Clone();
                                    for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                    {
                                        for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                        {
                                            if (tempBrd[1][col[1], row[1]] == 2)
                                            {
                                                if (col[1] != 0 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != 1 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                }
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                                if (col[1] != BrdSize - 1 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != BrdSize - 2 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                }

                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                            }
                                            else if (tempBrd[1][col[1], row[1]] == -2)
                                            {
                                                for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (MoveScore[1] > MoveScore[0])
                                    {
                                        MoveScore[0] = MoveScore[1];
                                        ButtonLoc = new short[6] { col[0], row[0], (short)(col[0] - i - 1), (short)(row[0] - i - 1), (short)(col[0] - i), (short)(row[0] - i) };
                                    }

                                    tempBrd[0][col[0] - i - 1, row[0] - i - 1] = 0;
                                    tempBrd[0][col[0] - i, row[0] - i] = originBrd[col[0] - i, row[0] - i];
                                    tempBrd[0][col[0], row[0]] = -1;
                                }
                                break;
                            }
                        }

                        for (short i = 1; i < row[0] + 1 && i < BrdSize - col[0]; i++)
                        {
                            if (tempBrd[0][col[0] + i, row[0] - i] == 0)
                            {
                                tempBrd[0][col[0] + i, row[0] - i] = -1;
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 2-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                    ButtonLoc = new short[4] { col[0], row[0], (short)(col[0] + i), (short)(row[0] - i) };
                                }

                                tempBrd[0][col[0] + i, row[0] - i] = 0;
                                tempBrd[0][col[0], row[0]] = -1;
                            }
                            else if (tempBrd[0][col[0] + i, row[0] - i] == 1 || tempBrd[0][col[0] + i, row[0] - i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col[0] + i, row[0] - i] == 2 || tempBrd[0][col[0] + i, row[0] - i] == -2)
                            {
                                if (row[0] - i > 0 && col[0] + i < BrdSize - 1 && tempBrd[0][col[0] + i + 1, row[0] - i - 1] == 0)
                                {
                                    tempBrd[0][col[0] + i + 1, row[0] - i - 1] = -1;
                                    tempBrd[0][col[0] + i, row[0] - i] = 0;
                                    tempBrd[0][col[0], row[0]] = 0;

                                    #region 2-Move-AI
                                    MoveScore[1] = 1000;
                                    tempBrd[1] = (short[,])tempBrd[0].Clone();
                                    for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                    {
                                        for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                        {
                                            if (tempBrd[1][col[1], row[1]] == 2)
                                            {
                                                if (col[1] != 0 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != 1 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                }
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                                if (col[1] != BrdSize - 1 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != BrdSize - 2 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                }

                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                            }
                                            else if (tempBrd[1][col[1], row[1]] == -2)
                                            {
                                                for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (MoveScore[1] > MoveScore[0])
                                    {
                                        MoveScore[0] = MoveScore[1];
                                        ButtonLoc = new short[6] { col[0], row[0], (short)(col[0] + i + 1), (short)(row[0] - i - 1), (short)(col[0] + i), (short)(row[0] - i) };
                                    }

                                    tempBrd[0][col[0] + i + 1, row[0] - i - 1] = 0;
                                    tempBrd[0][col[0] + i, row[0] - i] = originBrd[col[0] + i, row[0] - i];
                                    tempBrd[0][col[0], row[0]] = -1;
                                }
                                break;
                            }
                        }

                        for (short i = 1; i < BrdSize - row[0] && i < col[0] + 1; i++)
                        {
                            if (tempBrd[0][col[0] - i, row[0] + i] == 0)
                            {
                                tempBrd[0][col[0] - i, row[0] + i] = -1;
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 2-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                    ButtonLoc = new short[4] { col[0], row[0], (short)(col[0] - i), (short)(row[0] + i) };
                                }

                                tempBrd[0][col[0] - i, row[0] + i] = 0;
                                tempBrd[0][col[0], row[0]] = -1;
                            }
                            else if (tempBrd[0][col[0] - i, row[0] + i] == 1 || tempBrd[0][col[0] - i, row[0] + i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col[0] - i, row[0] + i] == 2 || tempBrd[0][col[0] - i, row[0] + i] == -2)
                            {
                                if (row[0] + i < BrdSize - 1 && col[0] - i > 0 && tempBrd[0][col[0] - i - 1, row[0] + i + 1] == 0)
                                {
                                    tempBrd[0][col[0] - i - 1, row[0] + i + 1] = -1;
                                    tempBrd[0][col[0] - i, row[0] + i] = 0;
                                    tempBrd[0][col[0], row[0]] = 0;

                                    #region 2-Move-AI
                                    MoveScore[1] = 1000;
                                    tempBrd[1] = (short[,])tempBrd[0].Clone();
                                    for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                    {
                                        for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                        {
                                            if (tempBrd[1][col[1], row[1]] == 2)
                                            {
                                                if (col[1] != 0 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != 1 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                }
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                                if (col[1] != BrdSize - 1 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != BrdSize - 2 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                }

                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                            }
                                            else if (tempBrd[1][col[1], row[1]] == -2)
                                            {
                                                for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (MoveScore[1] > MoveScore[0])
                                    {
                                        MoveScore[0] = MoveScore[1];
                                        ButtonLoc = new short[6] { col[0], row[0], (short)(col[0] - i - 1), (short)(row[0] + i + 1), (short)(col[0] - i), (short)(row[0] + i) };
                                    }

                                    tempBrd[0][col[0] - i - 1, row[0] + i + 1] = 0;
                                    tempBrd[0][col[0] - i, row[0] + i] = originBrd[col[0] - i, row[0] + i];
                                    tempBrd[0][col[0], row[0]] = -1;
                                }
                                break;
                            }
                        }

                        for (short i = 1; i < BrdSize - row[0] && i < BrdSize - col[0]; i++)
                        {
                            if (tempBrd[0][col[0] + i, row[0] + i] == 0)
                            {
                                tempBrd[0][col[0] + i, row[0] + i] = -1;
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 2-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                    ButtonLoc = new short[4] { col[0], row[0], (short)(col[0] + i), (short)(row[0] + i) };
                                }

                                tempBrd[0][col[0] + i, row[0] + i] = 0;
                                tempBrd[0][col[0], row[0]] = -1;
                            }
                            else if (tempBrd[0][col[0] + i, row[0] + i] == 1 || tempBrd[0][col[0] + i, row[0] + i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col[0] + i, row[0] + i] == 2 || tempBrd[0][col[0] + i, row[0] + i] == -2)
                            {
                                if (row[0] + i < BrdSize - 1 && col[0] + i < BrdSize - 1 && tempBrd[0][col[0] + i + 1, row[0] + i + 1] == 0)
                                {
                                    tempBrd[0][col[0] + i + 1, row[0] + i + 1] = -1;
                                    tempBrd[0][col[0] + i, row[0] + i] = 0;
                                    tempBrd[0][col[0], row[0]] = 0;

                                    #region 2-Move-AI
                                    MoveScore[1] = 1000;
                                    tempBrd[1] = (short[,])tempBrd[0].Clone();
                                    for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                    {
                                        for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                        {
                                            if (tempBrd[1][col[1], row[1]] == 2)
                                            {
                                                if (col[1] != 0 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != 1 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                }
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                                if (col[1] != BrdSize - 1 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != BrdSize - 2 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                }

                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                            }
                                            else if (tempBrd[1][col[1], row[1]] == -2)
                                            {
                                                for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = CompMoveCont(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (MoveScore[1] > MoveScore[0])
                                    {
                                        MoveScore[0] = MoveScore[1];
                                        ButtonLoc = new short[6] { col[0], row[0], (short)(col[0] + i + 1), (short)(row[0] + i + 1), (short)(col[0] + i), (short)(row[0] + i) };
                                    }

                                    tempBrd[0][col[0] + i + 1, row[0] + i + 1] = 0;
                                    tempBrd[0][col[0] + i, row[0] + i] = originBrd[col[0] + i, row[0] + i];
                                    tempBrd[0][col[0], row[0]] = -1;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return ButtonLoc;
        }

        public short CompMoveCont(short[,] originBrd)
        {
            short[] MoveScore = new short[3] { -1000, 1000, -1000 };
            short[][,] tempBrd = new short[2][,];
            tempBrd[0] = (short[,])originBrd.Clone();
            short[] col = new short[2];
            short[] row = new short[2];

            for (row[0] = 0; row[0] < BrdSize; row[0]++)
            {
                for (col[0] = 0; col[0] < BrdSize; col[0]++)
                {
                    if (tempBrd[0][col[0], row[0]] == 1)
                    {
                        if (col[0] != 0 && row[0] != BrdSize - 1)
                        {
                            if (tempBrd[0][col[0] - 1, row[0] + 1] == 2 || tempBrd[0][col[0] - 1, row[0] + 1] == -2)
                            {
                                if (col[0] != 1 && row[0] != BrdSize - 2)
                                {
                                    if (tempBrd[0][col[0] - 2, row[0] + 2] == 0)
                                    {
                                        if (row[0] + 2 == BrdSize - 1)
                                        {
                                            tempBrd[0][col[0] - 2, row[0] + 2] = -1;
                                        }
                                        else
                                        {
                                            tempBrd[0][col[0] - 2, row[0] + 2] = 1;
                                        }
                                        tempBrd[0][col[0] - 1, row[0] + 1] = 0;
                                        tempBrd[0][col[0], row[0]] = 0;

                                        #region 4-Move-AI
                                        MoveScore[1] = 1000;
                                        tempBrd[1] = (short[,])tempBrd[0].Clone();
                                        for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                        {
                                            for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                            {
                                                if (tempBrd[1][col[1], row[1]] == 2)
                                                {
                                                    if (col[1] != 0 && row[1] != 0)
                                                    {
                                                        if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                        {
                                                            if (col[1] != 1 && row[1] != 1)
                                                            {
                                                                if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                                {
                                                                    if (row[1] - 2 == 0)
                                                                    {
                                                                        tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                    }
                                                                    else
                                                                    {
                                                                        tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                    }
                                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                    tempBrd[1][col[1], row[1]] = 0;

                                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                                    if (MoveScore[2] < MoveScore[1])
                                                                    {
                                                                        MoveScore[1] = MoveScore[2];
                                                                    }

                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                    tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                    tempBrd[1][col[1], row[1]] = 2;
                                                                }
                                                            }
                                                        }
                                                        else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                        {
                                                            if (row[1] - 1 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                            }
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                    if (col[1] != BrdSize - 1 && row[1] != 0)
                                                    {
                                                        if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                        {
                                                            if (col[1] != BrdSize - 2 && row[1] != 1)
                                                            {
                                                                if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                                {
                                                                    if (row[1] - 2 == 0)
                                                                    {
                                                                        tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                    }
                                                                    else
                                                                    {
                                                                        tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                    }

                                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                    tempBrd[1][col[1], row[1]] = 0;

                                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                                    if (MoveScore[2] < MoveScore[1])
                                                                    {
                                                                        MoveScore[1] = MoveScore[2];
                                                                    }

                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                    tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                    tempBrd[1][col[1], row[1]] = 2;
                                                                }
                                                            }
                                                        }
                                                        else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                        {
                                                            if (row[1] - 1 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                            }
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1], row[1]] == -2)
                                                {
                                                    for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                    {
                                                        if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                        {
                                                            if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                                tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                                tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                    {
                                                        if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                        {
                                                            if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                                tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                                tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                    {
                                                        if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                        {
                                                            if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                                tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                                tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                    {
                                                        if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                        {
                                                            if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                                tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[2] = MoveScore[1];
                                                                }

                                                                tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                                tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        if (MoveScore[1] > MoveScore[0])
                                        {
                                            MoveScore[0] = MoveScore[1];
                                        }

                                        tempBrd[0][col[0] - 2, row[0] + 2] = 0;
                                        tempBrd[0][col[0] - 1, row[0] + 1] = originBrd[col[0] - 1, row[0] + 1];
                                        tempBrd[0][col[0], row[0]] = 1;
                                    }
                                }
                            }
                            else if (tempBrd[0][col[0] - 1, row[0] + 1] == 0)
                            {
                                if (row[0] + 1 == BrdSize - 1)
                                {
                                    tempBrd[0][col[0] - 1, row[0] + 1] = -1;
                                }
                                else
                                {
                                    tempBrd[0][col[0] - 1, row[0] + 1] = 1;
                                }
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 4-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                }

                                tempBrd[0][col[0] - 1, row[0] + 1] = 0;
                                tempBrd[0][col[0], row[0]] = 1;
                            }
                        }
                        if (col[0] != BrdSize - 1 && row[0] != BrdSize - 1)
                        {
                            if (tempBrd[0][col[0] + 1, row[0] + 1] == 2 || tempBrd[0][col[0] + 1, row[0] + 1] == -2)
                            {
                                if (col[0] != BrdSize - 2 && row[0] != BrdSize - 2)
                                {
                                    if (tempBrd[0][col[0] + 2, row[0] + 2] == 0)
                                    {
                                        tempBrd[0][col[0] + 2, row[0] + 2] = originBrd[col[0], row[0]];
                                        tempBrd[0][col[0] + 1, row[0] + 1] = 0;
                                        tempBrd[0][col[0], row[0]] = 0;

                                        #region 4-Move-AI
                                        MoveScore[1] = 1000;
                                        tempBrd[1] = (short[,])tempBrd[0].Clone();
                                        for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                        {
                                            for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                            {
                                                if (tempBrd[1][col[1], row[1]] == 2)
                                                {
                                                    if (col[1] != 0 && row[1] != 0)
                                                    {
                                                        if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                        {
                                                            if (col[1] != 1 && row[1] != 1)
                                                            {
                                                                if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                                {
                                                                    if (row[1] - 2 == 0)
                                                                    {
                                                                        tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                    }
                                                                    else
                                                                    {
                                                                        tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                    }
                                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                    tempBrd[1][col[1], row[1]] = 0;

                                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                                    if (MoveScore[2] < MoveScore[1])
                                                                    {
                                                                        MoveScore[1] = MoveScore[2];
                                                                    }

                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                    tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                    tempBrd[1][col[1], row[1]] = 2;
                                                                }
                                                            }
                                                        }
                                                        else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                        {
                                                            if (row[1] - 1 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                            }
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                    if (col[1] != BrdSize - 1 && row[1] != 0)
                                                    {
                                                        if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                        {
                                                            if (col[1] != BrdSize - 2 && row[1] != 1)
                                                            {
                                                                if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                                {
                                                                    if (row[1] - 2 == 0)
                                                                    {
                                                                        tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                    }
                                                                    else
                                                                    {
                                                                        tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                    }

                                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                    tempBrd[1][col[1], row[1]] = 0;

                                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                                    if (MoveScore[2] < MoveScore[1])
                                                                    {
                                                                        MoveScore[1] = MoveScore[2];
                                                                    }

                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                    tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                    tempBrd[1][col[1], row[1]] = 2;
                                                                }
                                                            }
                                                        }
                                                        else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                        {
                                                            if (row[1] - 1 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                            }
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1], row[1]] == -2)
                                                {
                                                    for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                    {
                                                        if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                        {
                                                            if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                                tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                                tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                    {
                                                        if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                        {
                                                            if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                                tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                                tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                    {
                                                        if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                        {
                                                            if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                                tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                                tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }

                                                    for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                    {
                                                        if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                        {
                                                            break;
                                                        }
                                                        else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                        {
                                                            if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                            {
                                                                tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                                tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[2] = MoveScore[1];
                                                                }

                                                                tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                                tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                                tempBrd[1][col[1], row[1]] = -2;
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        if (MoveScore[1] > MoveScore[0])
                                        {
                                            MoveScore[0] = MoveScore[1];
                                        }

                                        tempBrd[0][col[0] + 2, row[0] + 2] = 0;
                                        tempBrd[0][col[0] + 1, row[0] + 1] = originBrd[col[0] + 1, row[0] + 1];
                                        tempBrd[0][col[0], row[0]] = originBrd[col[0], row[0]];
                                    }
                                }
                            }
                            else if (tempBrd[0][col[0] + 1, row[0] + 1] != 1 && tempBrd[0][col[0] + 1, row[0] + 1] != -1)
                            {
                                tempBrd[0][col[0] + 1, row[0] + 1] = originBrd[col[0], row[0]];
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 4-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                }

                                tempBrd[0][col[0] + 1, row[0] + 1] = 0;
                                tempBrd[0][col[0], row[0]] = originBrd[col[0], row[0]];
                            }
                        }
                    }
                    else if (tempBrd[0][col[0], row[0]] == -1)
                    {
                        for (short n = 1; n < row[0] + 1 && n < col[0] + 1; n++)
                        {
                            if (tempBrd[0][col[0] - n, row[0] - n] == 0)
                            {
                                tempBrd[0][col[0] - n, row[0] - n] = -1;
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 4-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                }

                                tempBrd[0][col[0] - n, row[0] - n] = 0;
                                tempBrd[0][col[0], row[0]] = -1;
                            }
                            else if (tempBrd[0][col[0] - n, row[0] - n] == 1 || tempBrd[0][col[0] - n, row[0] - n] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col[0] - n, row[0] - n] == 2 || tempBrd[0][col[0] - n, row[0] - n] == -2)
                            {
                                if (row[0] - n > 0 && col[0] - n > 0 && tempBrd[0][col[0] - n - 1, row[0] - n - 1] == 0)
                                {
                                    tempBrd[0][col[0] - n - 1, row[0] - n - 1] = -1;
                                    tempBrd[0][col[0] - n, row[0] - n] = 0;
                                    tempBrd[0][col[0], row[0]] = 0;

                                    #region 4-Move-AI
                                    MoveScore[1] = 1000;
                                    tempBrd[1] = (short[,])tempBrd[0].Clone();
                                    for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                    {
                                        for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                        {
                                            if (tempBrd[1][col[1], row[1]] == 2)
                                            {
                                                if (col[1] != 0 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != 1 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                }
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                                if (col[1] != BrdSize - 1 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != BrdSize - 2 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                }

                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                            }
                                            else if (tempBrd[1][col[1], row[1]] == -2)
                                            {
                                                for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (MoveScore[1] > MoveScore[0])
                                    {
                                        MoveScore[0] = MoveScore[1];
                                    }

                                    tempBrd[0][col[0] - n - 1, row[0] - n - 1] = 0;
                                    tempBrd[0][col[0] - n, row[0] - n] = originBrd[col[0] - n, row[0] - n];
                                    tempBrd[0][col[0], row[0]] = -1;
                                }
                                break;
                            }
                        }

                        for (short n = 1; n < row[0] + 1 && n < BrdSize - col[0]; n++)
                        {
                            if (tempBrd[0][col[0] + n, row[0] - n] == 0)
                            {
                                tempBrd[0][col[0] + n, row[0] - n] = -1;
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 4-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                }

                                tempBrd[0][col[0] + n, row[0] - n] = 0;
                                tempBrd[0][col[0], row[0]] = -1;
                            }
                            else if (tempBrd[0][col[0] + n, row[0] - n] == 1 || tempBrd[0][col[0] + n, row[0] - n] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col[0] + n, row[0] - n] == 2 || tempBrd[0][col[0] + n, row[0] - n] == -2)
                            {
                                if (row[0] - n > 0 && col[0] + n < BrdSize - 1 && tempBrd[0][col[0] + n + 1, row[0] - n - 1] == 0)
                                {
                                    tempBrd[0][col[0] + n + 1, row[0] - n - 1] = -1;
                                    tempBrd[0][col[0] + n, row[0] - n] = 0;
                                    tempBrd[0][col[0], row[0]] = 0;

                                    #region 4-Move-AI
                                    MoveScore[1] = 1000;
                                    tempBrd[1] = (short[,])tempBrd[0].Clone();
                                    for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                    {
                                        for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                        {
                                            if (tempBrd[1][col[1], row[1]] == 2)
                                            {
                                                if (col[1] != 0 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != 1 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                }
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                                if (col[1] != BrdSize - 1 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != BrdSize - 2 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                }

                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                            }
                                            else if (tempBrd[1][col[1], row[1]] == -2)
                                            {
                                                for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (MoveScore[1] > MoveScore[0])
                                    {
                                        MoveScore[0] = MoveScore[1];
                                    }

                                    tempBrd[0][col[0] + n + 1, row[0] - n - 1] = 0;
                                    tempBrd[0][col[0] + n, row[0] - n] = originBrd[col[0] + n, row[0] - n];
                                    tempBrd[0][col[0], row[0]] = -1;
                                }
                                break;
                            }
                        }

                        for (short n = 1; n < BrdSize - row[0] && n < col[0] + 1; n++)
                        {
                            if (tempBrd[0][col[0] - n, row[0] + n] == 0)
                            {
                                tempBrd[0][col[0] - n, row[0] + n] = -1;
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 4-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                }

                                tempBrd[0][col[0] - n, row[0] + n] = 0;
                                tempBrd[0][col[0], row[0]] = -1;
                            }
                            else if (tempBrd[0][col[0] - n, row[0] + n] == 1 || tempBrd[0][col[0] - n, row[0] + n] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col[0] - n, row[0] + n] == 2 || tempBrd[0][col[0] - n, row[0] + n] == -2)
                            {
                                if (row[0] + n < BrdSize - 1 && col[0] - n > 0 && tempBrd[0][col[0] - n - 1, row[0] + n + 1] == 0)
                                {
                                    tempBrd[0][col[0] - n - 1, row[0] + n + 1] = -1;
                                    tempBrd[0][col[0] - n, row[0] + n] = 0;
                                    tempBrd[0][col[0], row[0]] = 0;

                                    #region 4-Move-AI
                                    MoveScore[1] = 1000;
                                    tempBrd[1] = (short[,])tempBrd[0].Clone();
                                    for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                    {
                                        for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                        {
                                            if (tempBrd[1][col[1], row[1]] == 2)
                                            {
                                                if (col[1] != 0 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != 1 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                }
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                                if (col[1] != BrdSize - 1 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != BrdSize - 2 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                }

                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                            }
                                            else if (tempBrd[1][col[1], row[1]] == -2)
                                            {
                                                for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (MoveScore[1] > MoveScore[0])
                                    {
                                        MoveScore[0] = MoveScore[1];
                                    }

                                    tempBrd[0][col[0] - n - 1, row[0] + n + 1] = 0;
                                    tempBrd[0][col[0] - n, row[0] + n] = originBrd[col[0] - n, row[0] + n];
                                    tempBrd[0][col[0], row[0]] = -1;
                                }
                                break;
                            }
                        }

                        for (short n = 1; n < BrdSize - row[0] && n < BrdSize - col[0]; n++)
                        {
                            if (tempBrd[0][col[0] + n, row[0] + n] == 0)
                            {
                                tempBrd[0][col[0] + n, row[0] + n] = -1;
                                tempBrd[0][col[0], row[0]] = 0;

                                #region 4-Move-AI
                                MoveScore[1] = 1000;
                                tempBrd[1] = (short[,])tempBrd[0].Clone();
                                for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                {
                                    for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                    {
                                        if (tempBrd[1][col[1], row[1]] == 2)
                                        {
                                            if (col[1] != 0 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != 1 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                            }
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                            if (col[1] != BrdSize - 1 && row[1] != 0)
                                            {
                                                if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                {
                                                    if (col[1] != BrdSize - 2 && row[1] != 1)
                                                    {
                                                        if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                        {
                                                            if (row[1] - 2 == 0)
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                            }
                                                            else
                                                            {
                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                            }

                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                            tempBrd[1][col[1], row[1]] = 2;
                                                        }
                                                    }
                                                }
                                                else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                {
                                                    if (row[1] - 1 == 0)
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                    }
                                                    else
                                                    {
                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                    }
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[2] = MoveScore[1];
                                                    }

                                                    tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                    tempBrd[1][col[1], row[1]] = 2;
                                                }
                                            }
                                        }
                                        else if (tempBrd[1][col[1], row[1]] == -2)
                                        {
                                            for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                {
                                                    if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                            {
                                                if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }

                                            for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                            {
                                                if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                {
                                                    tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                    tempBrd[1][col[1], row[1]] = 0;

                                                    MoveScore[2] = BrdScore(tempBrd[1]);
                                                    if (MoveScore[2] < MoveScore[1])
                                                    {
                                                        MoveScore[1] = MoveScore[2];
                                                    }

                                                    tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                    tempBrd[1][col[1], row[1]] = -2;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    break;
                                                }
                                                else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                {
                                                    if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                        tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (MoveScore[1] > MoveScore[0])
                                {
                                    MoveScore[0] = MoveScore[1];
                                }

                                tempBrd[0][col[0] + n, row[0] + n] = 0;
                                tempBrd[0][col[0], row[0]] = -1;
                            }
                            else if (tempBrd[0][col[0] + n, row[0] + n] == 1 || tempBrd[0][col[0] + n, row[0] + n] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col[0] + n, row[0] + n] == 2 || tempBrd[0][col[0] + n, row[0] + n] == -2)
                            {
                                if (row[0] + n < BrdSize - 1 && col[0] + n < BrdSize - 1 && tempBrd[0][col[0] + n + 1, row[0] + n + 1] == 0)
                                {
                                    tempBrd[0][col[0] + n + 1, row[0] + n + 1] = -1;
                                    tempBrd[0][col[0] + n, row[0] + n] = 0;
                                    tempBrd[0][col[0], row[0]] = 0;

                                    #region 4-Move-AI
                                    MoveScore[1] = 1000;
                                    tempBrd[1] = (short[,])tempBrd[0].Clone();
                                    for (row[1] = 0; row[1] < BrdSize; row[1]++)
                                    {
                                        for (col[1] = 0; col[1] < BrdSize; col[1]++)
                                        {
                                            if (tempBrd[1][col[1], row[1]] == 2)
                                            {
                                                if (col[1] != 0 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] - 1, row[1] - 1] == 1 || tempBrd[1][col[1] - 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != 1 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] - 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] - 2, row[1] - 2] = 2;
                                                                }
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] - 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] - 1, row[1] - 1] = tempBrd[0][col[1] - 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] - 1, row[1] - 1] == 0)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] - 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                                if (col[1] != BrdSize - 1 && row[1] != 0)
                                                {
                                                    if (tempBrd[1][col[1] + 1, row[1] - 1] == 1 || tempBrd[1][col[1] + 1, row[1] - 1] == -1)
                                                    {
                                                        if (col[1] != BrdSize - 2 && row[1] != 1)
                                                        {
                                                            if (tempBrd[1][col[1] + 2, row[1] - 2] == 0)
                                                            {
                                                                if (row[1] - 2 == 0)
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = -2;
                                                                }
                                                                else
                                                                {
                                                                    tempBrd[1][col[1] + 2, row[1] - 2] = 2;
                                                                }

                                                                tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                                tempBrd[1][col[1], row[1]] = 0;

                                                                MoveScore[2] = BrdScore(tempBrd[1]);
                                                                if (MoveScore[2] < MoveScore[1])
                                                                {
                                                                    MoveScore[1] = MoveScore[2];
                                                                }

                                                                tempBrd[1][col[1] + 2, row[1] - 2] = 0;
                                                                tempBrd[1][col[1] + 1, row[1] - 1] = tempBrd[0][col[1] + 1, row[1] - 1];
                                                                tempBrd[1][col[1], row[1]] = 2;
                                                            }
                                                        }
                                                    }
                                                    else if (tempBrd[1][col[1] + 1, row[1] - 1] != 1 && tempBrd[1][col[1] + 1, row[1] - 1] != -1)
                                                    {
                                                        if (row[1] - 1 == 0)
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = -2;
                                                        }
                                                        else
                                                        {
                                                            tempBrd[1][col[1] + 1, row[1] - 1] = 2;
                                                        }
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[2] = MoveScore[1];
                                                        }

                                                        tempBrd[1][col[1] + 1, row[1] - 1] = 0;
                                                        tempBrd[1][col[1], row[1]] = 2;
                                                    }
                                                }
                                            }
                                            else if (tempBrd[1][col[1], row[1]] == -2)
                                            {
                                                for (short m = 1; m < row[1] + 1 && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 2 || tempBrd[1][col[1] - m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] - m] == 1 || tempBrd[1][col[1] - m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] - m] = tempBrd[0][col[1] - m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < row[1] + 1 && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] - m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] - m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 2 || tempBrd[1][col[1] + m, row[1] - m] == -2)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] - m] == 1 || tempBrd[1][col[1] + m, row[1] - m] == -1)
                                                    {
                                                        if (row[1] - m > 0 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] - m - 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] - m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] - m - 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] - m] = tempBrd[0][col[1] + m, row[1] - m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < col[1] + 1; m++)
                                                {
                                                    if (tempBrd[1][col[1] - m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] - m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 1 || tempBrd[1][col[1] - m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] - m, row[1] + m] == 2 || tempBrd[1][col[1] - m, row[1] + m] == -2)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] - m > 0 && tempBrd[1][col[1] - m - 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] - m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[1] = MoveScore[2];
                                                            }

                                                            tempBrd[1][col[1] - m - 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] - m, row[1] + m] = tempBrd[0][col[1] - m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }

                                                for (short m = 1; m < BrdSize - row[1] && m < BrdSize - col[1]; m++)
                                                {
                                                    if (tempBrd[1][col[1] + m, row[1] + m] == 0)
                                                    {
                                                        tempBrd[1][col[1] + m, row[1] + m] = -2;
                                                        tempBrd[1][col[1], row[1]] = 0;

                                                        MoveScore[2] = BrdScore(tempBrd[1]);
                                                        if (MoveScore[2] < MoveScore[1])
                                                        {
                                                            MoveScore[1] = MoveScore[2];
                                                        }

                                                        tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                        tempBrd[1][col[1], row[1]] = -2;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        break;
                                                    }
                                                    else if (tempBrd[1][col[1] + m, row[1] + m] == 1 || tempBrd[1][col[1] + m, row[1] + m] == -1)
                                                    {
                                                        if (row[1] + m < BrdSize - 1 && col[1] + m < BrdSize - 1 && tempBrd[1][col[1] + m + 1, row[1] + m + 1] == 0)
                                                        {
                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = -2;
                                                            tempBrd[1][col[1] + m, row[1] + m] = 0;
                                                            tempBrd[1][col[1], row[1]] = 0;

                                                            MoveScore[2] = BrdScore(tempBrd[1]);
                                                            if (MoveScore[2] < MoveScore[1])
                                                            {
                                                                MoveScore[2] = MoveScore[1];
                                                            }

                                                            tempBrd[1][col[1] + m + 1, row[1] + m + 1] = 0;
                                                            tempBrd[1][col[1] + m, row[1] + m] = tempBrd[0][col[1] + m, row[1] + m];
                                                            tempBrd[1][col[1], row[1]] = -2;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (MoveScore[1] > MoveScore[0])
                                    {
                                        MoveScore[0] = MoveScore[1];
                                    }

                                    tempBrd[0][col[0] + n + 1, row[0] + n + 1] = 0;
                                    tempBrd[0][col[0] + n, row[0] + n] = originBrd[col[0] + n, row[0] + n];
                                    tempBrd[0][col[0], row[0]] = -1;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return MoveScore[0];
        }

        void Form1_Click(object sender, EventArgs e)
        {
            short[] ButtonLoc = (short[])(((Button)(sender)).Tag);
            short col = ButtonLoc[0];
            short row = ButtonLoc[1];
            short rivalTurnVal = (short)(turnVal % 2 + 1);
            short moveDir;            

            if (intBrd[col, row] == turnVal)
            {
                if (turnVal == 2)//black checker case
                {
                    moveDir = 1;
                }
                else//white checker case
                {
                    moveDir = -1;
                }

                DyingButtons[0] = null;
                DyingButtons[1] = null;
                DyingButtons[2] = null;
                DyingButtons[3] = null;

                CleanSelected();

                if (col != 0)//checks that the button isn't on an edge
                {
                    if (intBrd[col - 1, row - moveDir] == rivalTurnVal || intBrd[col - 1, row - moveDir] == -rivalTurnVal)
                    {                                                    //checks if there is a rival in this direction
                        if (col != 1 && row != rivalTurnVal / 2 * (BrdSize - 3) + 1)
                        {
                            if (intBrd[col - 2, row - 2 * moveDir] == 0)
                            {                     //checks to see if the enemy is in a position where he can be eaten
                                Board[col - 2, row - 2 * moveDir].BackgroundImage = Properties.Resources.selectMark;
                                intBrd[col - 2, row - 2 * moveDir] = 3;
                                DyingButtons[0] = new short[2] { (short)(col - 1), (short)(row - moveDir) };
                                PressedButton = new short[2] { col, row };
                            }
                        }
                    }
                    else if (intBrd[col - 1, row - moveDir] != turnVal && intBrd[col - 1, row - moveDir] != -turnVal)
                    {
                        Board[col - 1, row - moveDir].BackgroundImage = Properties.Resources.selectMark;
                        intBrd[col - 1, row - moveDir] = 3;
                        PressedButton = new short[2] { col, row };
                    }
                }
                if (col != BrdSize - 1)
                {
                    if (intBrd[col + 1, row - moveDir] == rivalTurnVal || intBrd[col + 1, row - moveDir] == -rivalTurnVal)
                    {
                        if (col != BrdSize - 2 && row != rivalTurnVal / 2 * (BrdSize - 3) + 1)
                        {
                            if (intBrd[col + 2, row - 2 * moveDir] == 0)
                            {
                                Board[col + 2, row - 2 * moveDir].BackgroundImage = Properties.Resources.selectMark;
                                intBrd[col + 2, row - 2 * moveDir] = 3;
                                DyingButtons[1] = new short[2] { (short)(col + 1), (short)(row - moveDir) };
                                PressedButton = new short[2] { col, row };
                            }
                        }
                    }
                    else if (intBrd[col + 1, row - moveDir] != turnVal && intBrd[col + 1, row - moveDir] != -turnVal)
                    {
                        Board[col + 1, row - moveDir].BackgroundImage = Properties.Resources.selectMark;
                        intBrd[col + 1, row - moveDir] = 3;
                        PressedButton = new short[2] { col, row };
                    }
                }
            }
            else if (intBrd[col, row] == -turnVal)
            {
                DyingButtons[0] = null;
                DyingButtons[1] = null;
                DyingButtons[2] = null;
                DyingButtons[3] = null;

                CleanSelected();

                bool CanPlace = false;

                for (short i = 1; i < row + 1 && i < col + 1; i++)
                {   //select tiles in all directions
                    if (intBrd[col - i, row - i] == 0)
                    {
                        Board[col - i, row - i].BackgroundImage = Properties.Resources.selectMark;
                        intBrd[col - i, row - i] = 3;
                        CanPlace = true;
                    }
                    else if (intBrd[col - i, row - i] == turnVal || intBrd[col - i, row - i] == -turnVal)
                    {
                        break;
                    }
                    else if (intBrd[col - i, row - i] == rivalTurnVal || intBrd[col - i, row - i] == -rivalTurnVal)
                    {
                        if (row - i > 0 && col - i > 0 && intBrd[col - i - 1, row - i - 1] == 0)
                        {
                            Board[col - i - 1, row - i - 1].BackgroundImage = Properties.Resources.selectMark;
                            intBrd[col - i - 1, row - i - 1] = 3;
                            DyingButtons[0] = new short[2] { (short)(col - i), (short)(row - i) };
                            CanPlace = true;
                        }
                        break;
                    }
                }

                for (short i = 1; i < row + 1 && i < BrdSize - col; i++)
                {
                    if (intBrd[col + i, row - i] == 0)
                    {
                        Board[col + i, row - i].BackgroundImage = Properties.Resources.selectMark;
                        intBrd[col + i, row - i] = 3;
                        CanPlace = true;
                    }
                    else if (intBrd[col + i, row - i] == turnVal || intBrd[col + i, row - i] == -turnVal)
                    {
                        break;
                    }
                    else if (intBrd[col + i, row - i] == rivalTurnVal || intBrd[col + i, row - i] == -rivalTurnVal)
                    {
                        if (row - i > 0 && col + i < BrdSize - 1 && intBrd[col + i + 1, row - i - 1] == 0)
                        {
                            Board[col + i + 1, row - i - 1].BackgroundImage = Properties.Resources.selectMark;
                            intBrd[col + i + 1, row - i - 1] = 3;
                            DyingButtons[1] = new short[2] { (short)(col + i), (short)(row - i) };
                            CanPlace = true;
                        }
                        break;
                    }
                }

                for (short i = 1; i < BrdSize - row && i < col + 1; i++)
                {
                    if (intBrd[col - i, row + i] == 0)
                    {
                        Board[col - i, row + i].BackgroundImage = Properties.Resources.selectMark;
                        intBrd[col - i, row + i] = 3;
                        CanPlace = true;
                    }
                    else if (intBrd[col - i, row + i] == turnVal || intBrd[col - i, row + i] == -turnVal)
                    {
                        break;
                    }
                    else if (intBrd[col - i, row + i] == rivalTurnVal || intBrd[col - i, row + i] == -rivalTurnVal)
                    {
                        if (row + i < BrdSize - 1 && col - i > 0 && intBrd[col - i - 1, row + i + 1] == 0)
                        {
                            Board[col - i - 1, row + i + 1].BackgroundImage = Properties.Resources.selectMark;
                            intBrd[col - i - 1, row + i + 1] = 3;
                            DyingButtons[2] = new short[2] { (short)(col - i), (short)(row + i) };
                            CanPlace = true;
                        }
                        break;
                    }
                }

                for (short i = 1; i < BrdSize - row && i < BrdSize - col; i++)
                {
                    if (intBrd[col + i, row + i] == 0)
                    {
                        Board[col + i, row + i].BackgroundImage = Properties.Resources.selectMark;
                        intBrd[col + i, row + i] = 3;
                        CanPlace = true;
                    }
                    else if (intBrd[col + i, row + i] == turnVal || intBrd[col + i, row + i] == -turnVal)
                    {
                        break;
                    }
                    else if (intBrd[col + i, row + i] == rivalTurnVal || intBrd[col + i, row + i] == -rivalTurnVal)
                    {
                        if (row + i < BrdSize - 1 && col + i < BrdSize - 1 && intBrd[col + i + 1, row + i + 1] == 0)
                        {
                            Board[col + i + 1, row + i + 1].BackgroundImage = Properties.Resources.selectMark;
                            intBrd[col + i + 1, row + i + 1] = 3;
                            DyingButtons[3] = new short[2] { (short)(col + i), (short)(row + i) };
                            CanPlace = true;
                        }
                        break;
                    }
                }

                if (CanPlace == true)
                {
                    PressedButton = new short[2] { col, row };
                }
            }
            else if (intBrd[col, row] == 3)
            {
                CleanSelected();

                if (turnVal == 2)//Move Black Checkers
                {
                    if (DyingButtons[0] != null && DyingButtons[0][0] == col + 1 && DyingButtons[0][1] == row + 1)
                    {//check if a checker is eaten via this move
                        intBrd[DyingButtons[0][0], DyingButtons[0][1]] = 0;
                        Board[DyingButtons[0][0], DyingButtons[0][1]].BackgroundImage = null;
                        WhiteCheckers--;
                    }
                    else if (DyingButtons[1] != null && DyingButtons[1][0] == col - 1 && DyingButtons[1][1] == row + 1)
                    {
                        intBrd[DyingButtons[1][0], DyingButtons[1][1]] = 0;
                        Board[DyingButtons[1][0], DyingButtons[1][1]].BackgroundImage = null;
                        WhiteCheckers--;
                    }
                    else if (DyingButtons[2] != null && DyingButtons[2][0] == col + 1 && DyingButtons[2][1] == row - 1)
                    {
                        intBrd[DyingButtons[2][0], DyingButtons[2][1]] = 0;
                        Board[DyingButtons[2][0], DyingButtons[2][1]].BackgroundImage = null;
                        WhiteCheckers--;
                    }
                    else if (DyingButtons[3] != null && DyingButtons[3][0] == col - 1 && DyingButtons[3][1] == row - 1)
                    {
                        intBrd[DyingButtons[3][0], DyingButtons[3][1]] = 0;
                        Board[DyingButtons[3][0], DyingButtons[3][1]].BackgroundImage = null;
                        WhiteCheckers--;
                    }

                    if (intBrd[PressedButton[0], PressedButton[1]] == turnVal && row > 0)
                    {       //check if it's a king or becomes one
                        Board[col, row].BackgroundImage = Properties.Resources.blackChecker;
                        intBrd[col, row] = 2;
                    }
                    else
                    {
                        Board[col, row].BackgroundImage = Properties.Resources.blackKing;
                        intBrd[col, row] = -2;
                    }

                    Board[PressedButton[0], PressedButton[1]].BackgroundImage = null;
                    intBrd[PressedButton[0], PressedButton[1]] = 0;

                    PressedButton = null;

                    WhiteCheckerBox.Text = "White Checkers: " + WhiteCheckers.ToString();//updates textbox

                    winCheck();

                    turnVal = rivalTurnVal;

                    turnBoxTextUpdate();

                    if (compActive)
                    {
                        compTurn();
                    }
                }
                else
                {
                    if (intBrd[PressedButton[0], PressedButton[1]] == turnVal)//same for the most part as the black move function
                    {
                        if (DyingButtons[0] != null && DyingButtons[0][0] == col + 1 && DyingButtons[0][1] == row - 1)
                        {
                            intBrd[DyingButtons[0][0], DyingButtons[0][1]] = 0;
                            Board[DyingButtons[0][0], DyingButtons[0][1]].BackgroundImage = null;
                            BlackCheckers--;
                        }
                        else if (DyingButtons[1] != null && DyingButtons[1][0] == col - 1 && DyingButtons[1][1] == row - 1)
                        {
                            intBrd[DyingButtons[1][0], DyingButtons[1][1]] = 0;
                            Board[DyingButtons[1][0], DyingButtons[1][1]].BackgroundImage = null;
                            BlackCheckers--;
                        }
                    }//here because the DyingButtons[] array doesn't perfectly line up there is a difference between kings and non-kings
                    else
                    {
                        if (DyingButtons[0] != null && DyingButtons[0][0] == col + 1 && DyingButtons[0][1] == row + 1)
                        {
                            intBrd[DyingButtons[0][0], DyingButtons[0][1]] = 0;
                            Board[DyingButtons[0][0], DyingButtons[0][1]].BackgroundImage = null;
                            BlackCheckers--;
                        }
                        else if (DyingButtons[1] != null && DyingButtons[1][0] == col - 1 && DyingButtons[1][1] == row + 1)
                        {
                            intBrd[DyingButtons[1][0], DyingButtons[1][1]] = 0;
                            Board[DyingButtons[1][0], DyingButtons[1][1]].BackgroundImage = null;
                            BlackCheckers--;
                        }
                        else if (DyingButtons[0] != null && DyingButtons[2][0] == col + 1 && DyingButtons[2][1] == row - 1)
                        {
                            intBrd[DyingButtons[2][0], DyingButtons[2][1]] = 0;
                            Board[DyingButtons[2][0], DyingButtons[2][1]].BackgroundImage = null;
                            BlackCheckers--;
                        }
                        else if (DyingButtons[3] != null && DyingButtons[3][0] == col - 1 && DyingButtons[3][1] == row - 1)
                        {
                            intBrd[DyingButtons[3][0], DyingButtons[3][1]] = 0;
                            Board[DyingButtons[3][0], DyingButtons[3][1]].BackgroundImage = null;
                            BlackCheckers--;
                        }
                    }

                    if (intBrd[PressedButton[0], PressedButton[1]] == turnVal && row < BrdSize - 1)
                    {
                        Board[col, row].BackgroundImage = Properties.Resources.whiteChecker;
                        intBrd[col, row] = 1;
                    }
                    else
                    {
                        Board[col, row].BackgroundImage = Properties.Resources.whiteKing;
                        intBrd[col, row] = -1;
                    }

                    Board[PressedButton[0], PressedButton[1]].BackgroundImage = null;
                    intBrd[PressedButton[0], PressedButton[1]] = 0;


                    PressedButton = null;

                    BlackCheckerBox.Text = "Black Checkers: " + BlackCheckers.ToString();

                    winCheck();

                    turnVal = rivalTurnVal;

                    turnBoxTextUpdate();
                }
            }
        }

        static short[] forceCompMove(short[,] originBrd)
        {
            short[,] tempBrd;
            tempBrd = (short[,])originBrd.Clone();
            short BrdSize = (short)(originBrd.GetLength(0));
            short Score = -1000;
            short MoveScore;
            short[] ButtonLoc = null;

            for (short row = 0; row < BrdSize; row++)
            {
                for (short col = 0; col < BrdSize; col++)
                {
                    if (tempBrd[col, row] == 1)//simulate normal checker move
                    {
                        if (col != 0 && row != BrdSize - 1)
                        {
                            if (tempBrd[col - 1, row + 1] == 2 || tempBrd[col - 1, row + 1] == -2)
                            {
                                if (col != 1 && row != BrdSize - 2)
                                {
                                    if (tempBrd[col - 2, row + 2] == 0)
                                    {
                                        if (row + 2 == BrdSize - 1)
                                        {
                                            tempBrd[col - 2, row + 2] = -1;
                                        }
                                        else
                                        {
                                            tempBrd[col - 2, row + 2] = 1;
                                        }
                                        tempBrd[col - 1, row + 1] = 0;
                                        tempBrd[col, row] = 0;

                                        MoveScore = BrdScore(tempBrd);
                                        if (MoveScore > Score)
                                        {
                                            Score = MoveScore;
                                            ButtonLoc = new short[6] { col, row, (short)(col - 2), (short)(row + 2), (short)(col - 1), (short)(row + 1) };
                                        }

                                        tempBrd[col - 2, row + 2] = 0;
                                        tempBrd[col - 1, row + 1] = originBrd[col - 1, row + 1];
                                        tempBrd[col, row] = 1;
                                    }
                                }
                            }
                            else if (tempBrd[col - 1, row + 1] == 0)
                            {
                                if (row + 1 == BrdSize - 1)
                                {
                                    tempBrd[col - 1, row + 1] = -1;
                                }
                                else
                                {
                                    tempBrd[col - 1, row + 1] = 1;
                                }
                                tempBrd[col, row] = 0;

                                MoveScore = BrdScore(tempBrd);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] { col, row, (short)(col - 1), (short)(row + 1) };
                                }

                                tempBrd[col - 1, row + 1] = 0;
                                tempBrd[col, row] = 1;
                            }
                        }
                        if (col != BrdSize - 1 && row != BrdSize - 1)
                        {
                            if (tempBrd[col + 1, row + 1] == 2 || tempBrd[col + 1, row + 1] == -2)
                            {
                                if (col != BrdSize - 2 && row != BrdSize - 2)
                                {
                                    if (tempBrd[col + 2, row + 2] == 0)
                                    {
                                        tempBrd[col + 2, row + 2] = originBrd[col, row];
                                        tempBrd[col + 1, row + 1] = 0;
                                        tempBrd[col, row] = 0;

                                        MoveScore = BrdScore(tempBrd);
                                        if (MoveScore > Score)
                                        {
                                            Score = MoveScore;
                                            ButtonLoc = new short[6] { col, row, (short)(col + 2), (short)(row + 2), (short)(col + 1), (short)(row + 1) };
                                        }

                                        tempBrd[col + 2, row + 2] = 0;
                                        tempBrd[col + 1, row + 1] = originBrd[col + 1, row + 1];
                                        tempBrd[col, row] = originBrd[col, row];
                                    }
                                }
                            }
                            else if (tempBrd[col + 1, row + 1] != 1 && tempBrd[col + 1, row + 1] != -1)
                            {
                                tempBrd[col + 1, row + 1] = originBrd[col, row];
                                tempBrd[col, row] = 0;

                                MoveScore = BrdScore(tempBrd);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] { col, row, (short)(col + 1), (short)(row + 1) };
                                }

                                tempBrd[col + 1, row + 1] = 0;
                                tempBrd[col, row] = originBrd[col, row];
                            }
                        }
                    }
                    else if (tempBrd[col, row] == -1)//simulate king checker move
                    {
                        for (short i = 1; i < row + 1 && i < col + 1; i++)
                        {
                            if (tempBrd[col - i, row - i] == 0)
                            {
                                tempBrd[col - i, row - i] = -1;
                                tempBrd[col, row] = 0;

                                MoveScore = BrdScore(tempBrd);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] { col, row, (short)(col - i), (short)(row - i) };
                                }

                                tempBrd[col - i, row - i] = 0;
                                tempBrd[col, row] = -1;
                            }
                            else if (tempBrd[col - i, row - i] == 1 || tempBrd[col - i, row - i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[col - i, row - i] == 2 || tempBrd[col - i, row - i] == -2)
                            {
                                if (row - i > 0 && col - i > 0 && tempBrd[col - i - 1, row - i - 1] == 0)
                                {
                                    tempBrd[col - i - 1, row - i - 1] = -1;
                                    tempBrd[col - i, row - i] = 0;
                                    tempBrd[col, row] = 0;

                                    MoveScore = BrdScore(tempBrd);
                                    if (MoveScore > Score)
                                    {
                                        Score = MoveScore;
                                        ButtonLoc = new short[6] { col, row, (short)(col - i - 1), (short)(row - i - 1), (short)(col - i), (short)(row - i) };
                                    }

                                    tempBrd[col - i - 1, row - i - 1] = 0;
                                    tempBrd[col - i, row - i] = originBrd[col - i, row - i];
                                    tempBrd[col, row] = -1;
                                }
                                break;
                            }
                        }

                        for (short i = 1; i < row + 1 && i < BrdSize - col; i++)
                        {
                            if (tempBrd[col + i, row - i] == 0)
                            {
                                tempBrd[col + i, row - i] = -1;
                                tempBrd[col, row] = 0;

                                MoveScore = BrdScore(tempBrd);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] { col, row, (short)(col + i), (short)(row - i) };
                                }

                                tempBrd[col + i, row - i] = 0;
                                tempBrd[col, row] = -1;
                            }
                            else if (tempBrd[col + i, row - i] == 1 || tempBrd[col + i, row - i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[col + i, row - i] == 2 || tempBrd[col + i, row - i] == -2)
                            {
                                if (row - i > 0 && col + i < BrdSize - 1 && tempBrd[col + i + 1, row - i - 1] == 0)
                                {
                                    tempBrd[col + i + 1, row - i - 1] = -1;
                                    tempBrd[col + i, row - i] = 0;
                                    tempBrd[col, row] = 0;

                                    MoveScore = BrdScore(tempBrd);
                                    if (MoveScore > Score)
                                    {
                                        Score = MoveScore;
                                        ButtonLoc = new short[6] { col, row, (short)(col + i + 1), (short)(row - i - 1), (short)(col + i), (short)(row - i) };
                                    }

                                    tempBrd[col + i + 1, row - i - 1] = 0;
                                    tempBrd[col + i, row - i] = originBrd[col + i, row - i];
                                    tempBrd[col, row] = -1;
                                }
                                break;
                            }
                        }

                        for (short i = 1; i < BrdSize - row && i < col + 1; i++)
                        {
                            if (tempBrd[col - i, row + i] == 0)
                            {
                                tempBrd[col - i, row + i] = -1;
                                tempBrd[col, row] = 0;

                                MoveScore = BrdScore(tempBrd);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] { col, row, (short)(col - i), (short)(row + i) };
                                }

                                tempBrd[col - i, row + i] = 0;
                                tempBrd[col, row] = -1;
                            }
                            else if (tempBrd[col - i, row + i] == 1 || tempBrd[col - i, row + i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[col - i, row + i] == 2 || tempBrd[col - i, row + i] == -2)
                            {
                                if (row + i < BrdSize - 1 && col - i > 0 && tempBrd[col - i - 1, row + i + 1] == 0)
                                {
                                    tempBrd[col - i - 1, row + i + 1] = -1;
                                    tempBrd[col - i, row + i] = 0;
                                    tempBrd[col, row] = 0;

                                    MoveScore = BrdScore(tempBrd);
                                    if (MoveScore > Score)
                                    {
                                        Score = MoveScore;
                                        ButtonLoc = new short[6] { col, row, (short)(col - i - 1), (short)(row + i + 1), (short)(col - i), (short)(row + i) };
                                    }

                                    tempBrd[col - i - 1, row + i + 1] = 0;
                                    tempBrd[col - i, row + i] = originBrd[col - i, row + i];
                                    tempBrd[col, row] = -1;
                                }
                                break;
                            }
                        }

                        for (short i = 1; i < BrdSize - row && i < BrdSize - col; i++)
                        {
                            if (tempBrd[col + i, row + i] == 0)
                            {
                                tempBrd[col + i, row + i] = -1;
                                tempBrd[col, row] = 0;

                                MoveScore = BrdScore(tempBrd);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] { col, row, (short)(col + i), (short)(row + i) };
                                }

                                tempBrd[col + i, row + i] = 0;
                                tempBrd[col, row] = -1;
                            }
                            else if (tempBrd[col + i, row + i] == 1 || tempBrd[col + i, row + i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[col + i, row + i] == 2 || tempBrd[col + i, row + i] == -2)
                            {
                                if (row + i < BrdSize - 1 && col + i < BrdSize - 1 && tempBrd[col + i + 1, row + i + 1] == 0)
                                {
                                    tempBrd[col + i + 1, row + i + 1] = -1;
                                    tempBrd[col + i, row + i] = 0;
                                    tempBrd[col, row] = 0;

                                    MoveScore = BrdScore(tempBrd);
                                    if (MoveScore > Score)
                                    {
                                        Score = MoveScore;
                                        ButtonLoc = new short[6] { col, row, (short)(col + i + 1), (short)(row + i + 1), (short)(col + i), (short)(row + i) };
                                    }

                                    tempBrd[col + i + 1, row + i + 1] = 0;
                                    tempBrd[col + i, row + i] = originBrd[col + i, row + i];
                                    tempBrd[col, row] = -1;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return ButtonLoc;
        }

        public void compTurn()
        {
            this.Update();

            CleanSelected();

            if (turnVal == 1)
            {
                System.Threading.Thread.Sleep(1000);

                short[] ButtonLoc = CompMove(intBrd);

                if (ButtonLoc == null)
                {
                    ButtonLoc = forceCompMove(intBrd);

                    if (ButtonLoc == null)
                    {
                        MessageBox.Show("White has no possible moves, White loses");
                    }

                }
                else
                {
                    if (ButtonLoc[3] < BrdSize - 1 && intBrd[ButtonLoc[0], ButtonLoc[1]] != -1)
                    {
                        intBrd[ButtonLoc[2], ButtonLoc[3]] = 1;
                        Board[ButtonLoc[2], ButtonLoc[3]].BackgroundImage = Properties.Resources.whiteChecker;
                    }
                    else
                    {
                        intBrd[ButtonLoc[2], ButtonLoc[3]] = -1;
                        Board[ButtonLoc[2], ButtonLoc[3]].BackgroundImage = Properties.Resources.whiteKing;
                    }

                    if (ButtonLoc.Length == 6)
                    {
                        intBrd[ButtonLoc[4], ButtonLoc[5]] = 0;
                        Board[ButtonLoc[4], ButtonLoc[5]].BackgroundImage = null;
                        BlackCheckers--;

                        BlackCheckerBox.Text = "Black Checkers: " + BlackCheckers.ToString();
                    }

                    intBrd[ButtonLoc[0], ButtonLoc[1]] = 0;
                    Board[ButtonLoc[0], ButtonLoc[1]].BackgroundImage = null;

                    turnVal = 2;

                    turnBoxTextUpdate();

                    winCheck();
                }
            }
        }

        public void turnBoxTextUpdate()
        {
            if (turnVal == 1)
            {
                TurnBox.BackColor = Color.White;
                TurnBox.ForeColor = Color.Black;
                TurnBox.Text = "White Turn";
            }
            else
            {
                TurnBox.ForeColor = Color.White;
                TurnBox.BackColor = Color.Black;
                TurnBox.Text = "Black Turn";
            }
        }

        private void CompButton_Click(object sender, EventArgs e)
        {
            if (compActive)
            {
                compActive = false;
                CompButton.Text = "Turn Computer ON?";
                CompButton.BackColor = Color.GreenYellow;
            }
            else
            {
                compActive = true;
                CompButton.Text = "Turn Computer OFF?";
                CompButton.BackColor = Color.Red;

                compTurn();
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            BrdSize = (short)(BrdSizeBox.Value);
            Board = new Button[BrdSize, BrdSize];
            intBrd = new short[BrdSize, BrdSize];
            short ButtonSize = Math.Max((short)(80 - Math.Pow(BrdSize / 2, 2)), (short)(30));

            panel1.Size = new Size(BrdSize * ButtonSize, BrdSize * ButtonSize);//sets panel and window sizes
            this.Size = new Size(BrdSize * ButtonSize + 100, BrdSize * ButtonSize + 150);

            for (short i = 0; i < BrdSize; i++)//creates board and inputs buttons
            {
                for (short k = 0; k < BrdSize; k++)//sets button properties
                {
                    Board[i, k] = new Button();

                    Board[i, k].Location = new Point(ButtonSize * i, ButtonSize * k);
                    Board[i, k].Size = new Size(ButtonSize, ButtonSize);
                    Board[i, k].Tag = new short[2] { i, k };
                    Board[i, k].BackgroundImageLayout = ImageLayout.Stretch;
                    Board[i, k].BackColor = Color.Black;

                    Board[i, k].Click += Form1_Click;

                    if ((i + k) % 2 == 1)//sets the buttons color and type
                    {
                        if (k < 3)
                        {
                            Board[i, k].BackgroundImage = Properties.Resources.whiteChecker;
                            intBrd[i, k] = 1;
                            BlackCheckers++;
                        }
                        else if (k > BrdSize - 4)
                        {
                            Board[i, k].BackgroundImage = Properties.Resources.blackChecker;
                            intBrd[i, k] = 2;
                            WhiteCheckers++;
                        }
                        else
                        {
                            intBrd[i, k] = 0;
                        }
                    }
                    else
                    {
                        Board[i, k].BackColor = Color.White;
                        Board[i, k].Enabled = false;
                        intBrd[i, k] = 0;
                    }

                    panel1.Controls.Add(Board[i, k]);
                }
            }

            WhiteCheckerBox.Text = "White Checkers: " + WhiteCheckers.ToString();
            BlackCheckerBox.Text = "Black Checkers: " + BlackCheckers.ToString();

            turnBoxTextUpdate();

            StartButton.Enabled = false;
            BrdSizeBox.Enabled = false;
            CompButton.Enabled = true;
            CompButton.Visible = true;
            BlackCheckerBox.Visible = true;
            WhiteCheckerBox.Visible = true;
            NoMoveButton.Enabled = true;
            NoMoveButton.Visible = true;
            CompButton.BackColor = Color.GreenYellow;
        }

        private void NoMoveButton_Click(object sender, EventArgs e)
        {
            bool CanMove = false;
            short rivalTurnVal = (short)(turnVal % 2 + 1);
            short moveDir;

            if (turnVal == 2)
            {
                moveDir = 1;
            }
            else
            {
                moveDir = -1;
            }

            for (short row = 0; row < BrdSize; row++)
            {
                for (short col = (short)(row % 2 + 1); col < BrdSize; col += 2)
                {
                    if (intBrd[col, row] == turnVal)
                    {
                        if (col != 0)
                        {
                            if (intBrd[col - 1, row - moveDir] == rivalTurnVal || intBrd[col - 1, row - moveDir] == -rivalTurnVal)
                            {
                                if (col != 1 && row != rivalTurnVal / 2 * (BrdSize - 3) + 1)
                                {
                                    if (intBrd[col - 2, row - 2 * moveDir] == 0)
                                    {
                                        CanMove = true;
                                        break;
                                    }
                                }
                            }
                            else if (intBrd[col - 1, row - moveDir] != turnVal && intBrd[col - 1, row - moveDir] != -turnVal)
                            {
                                CanMove = true;
                                break;
                            }
                        }
                        if (col != BrdSize - 1)
                        {
                            if (intBrd[col + 1, row - moveDir] == rivalTurnVal || intBrd[col + 1, row - moveDir] == -rivalTurnVal)
                            {
                                if (col != BrdSize - 2 && row != rivalTurnVal / 2 * (BrdSize - 3) + 1)
                                {
                                    if (intBrd[col + 2, row - 2 * moveDir] == 0)
                                    {
                                        CanMove = true;
                                        break;
                                    }
                                }
                            }
                            else if (intBrd[col + 1, row - moveDir] != turnVal && intBrd[col + 1, row - moveDir] != -turnVal)
                            {
                                CanMove = true;
                                break;
                            }
                        }
                    }
                    else if (intBrd[col, row] == -turnVal)
                    {
                        for (short i = 1; i < row + 1 && i < col + 1; i++)
                        {
                            if (intBrd[col - i, row - i] == 0)
                            {
                                CanMove = true;
                                break;
                            }
                            else if (intBrd[col - i, row - i] == turnVal || intBrd[col - i, row - i] == -turnVal)
                            {
                                break;
                            }
                            else if (intBrd[col - i, row - i] == rivalTurnVal || intBrd[col - i, row - i] == -rivalTurnVal)
                            {
                                if (row - i > 0 && col - i > 0 && intBrd[col - i - 1, row - i - 1] == 0)
                                {
                                    CanMove = true;
                                    break;
                                }
                            }
                        }

                        for (short i = 1; i < row + 1 && i < BrdSize - col; i++)
                        {
                            if (intBrd[col + i, row - i] == 0)
                            {
                                CanMove = true;
                                break;
                            }
                            else if (intBrd[col + i, row - i] == turnVal || intBrd[col + i, row - i] == -turnVal)
                            {
                                break;
                            }
                            else if (intBrd[col + i, row - i] == rivalTurnVal || intBrd[col + i, row - i] == -rivalTurnVal)
                            {
                                if (row - i > 0 && col + i < BrdSize - 1 && intBrd[col + i + 1, row - i - 1] == 0)
                                {
                                    CanMove = true;
                                    break;
                                }
                            }
                        }

                        for (short i = 1; i < BrdSize - row && i < col + 1; i++)
                        {
                            if (intBrd[col - i, row + i] == 0)
                            {
                                CanMove = true;
                                break;
                            }
                            else if (intBrd[col - i, row + i] == turnVal || intBrd[col - i, row + i] == -turnVal)
                            {
                                break;
                            }
                            else if (intBrd[col - i, row + i] == rivalTurnVal || intBrd[col - i, row + i] == -rivalTurnVal)
                            {
                                if (row + i < BrdSize - 1 && col - i > 0 && intBrd[col - i - 1, row + i + 1] == 0)
                                {
                                    CanMove = true;
                                    break;
                                }
                            }
                        }

                        for (short i = 1; i < BrdSize - row && i < BrdSize - col; i++)
                        {
                            if (intBrd[col + i, row + i] == 0)
                            {
                                CanMove = true;
                                break;
                            }
                            else if (intBrd[col + i, row + i] == turnVal || intBrd[col + i, row + i] == -turnVal)
                            {
                                break;
                            }
                            else if (intBrd[col + i, row + i] == rivalTurnVal || intBrd[col + i, row + i] == -rivalTurnVal)
                            {
                                if (row + i < BrdSize - 1 && col + i < BrdSize - 1 && intBrd[col + i + 1, row + i + 1] == 0)
                                {
                                    CanMove = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (CanMove)
                {
                    break;
                }
            }
            if (CanMove)
            {
                MessageBox.Show("There are still possible moves.");
            }
            else
            {
                if (turnVal == 1)
                {
                    MessageBox.Show("White has no more possible moves, White loses.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Black has no more possible moves, Black loses.");
                    this.Close();
                }
            }
        }
    }
}