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

        public Form1()
        {
            InitializeComponent();
        }

        static short BrdScore(short[,] OriginBrd)
        {
            short BrdSize=(short)(OriginBrd.GetLength(0));
            short Score = 0;

            for (short i = 0; i < BrdSize; i++)
            {
                for (short k = (short)(i % 2); k < BrdSize; k+=2)
                {
                    if (OriginBrd[i, k] == 0)
                    {
                        continue;
                    }
                    else if (OriginBrd[i, k] == 2)
                    {
                        Score += (short)(k+1-BrdSize);
                    }
                    else if (OriginBrd[i, k] == 1)
                    {
                        Score += k;
                    }
                    else if (OriginBrd[i, k] == -2)
                    {
                        Score -= (short)(BrdSize * 2);
                    }
                    else if (OriginBrd[i, k] == -1)
                    {
                        Score += (short)(BrdSize * 2);
                    }
                }
            }
            return Score;
        }

        static short[] CompMove(short[,] originBrd)
        {
            short[][,] tempBrd = new short[2][,];
            tempBrd[0]= (short[,])originBrd.Clone();
            short BrdSize=(short)(originBrd.GetLength(0));
            short Score = -100;
            short MoveScore;
            short[] ButtonLoc=null;


            for (short row = 0; row < BrdSize; row++)
            {
                for (short col = (short)(row % 2); col < BrdSize; col+=2)
                {
                    if (tempBrd[0][col, row] == 1)//simulate normal checker move
                    {
                        if (col != 0 && row!=BrdSize-1)
                        {
                            if (tempBrd[0][col - 1, row + 1] == 2 || tempBrd[0][col - 1, row + 1] == -2)
                            {
                                if (col != 1 && row != BrdSize-2)
                                {
                                    if (tempBrd[0][col - 2, row + 2] == 0)
                                    {
                                        if (row + 2 == BrdSize - 1)
                                        {
                                            tempBrd[0][col - 2, row + 2] = -1;
                                        }
                                        else
                                        {
                                            tempBrd[0][col - 2, row + 2] = 1;
                                        }
                                        tempBrd[0][col - 1, row + 1] = 0;
                                        tempBrd[0][col, row] = 0;

                                        MoveScore = BrdScore(tempBrd[0]);
                                        if (MoveScore > Score)
                                        {
                                            Score = MoveScore;
                                            ButtonLoc = new short[6] {col,row, (short)(col - 2), (short)(row + 2), (short)(col - 1), (short)(row + 1) };
                                        }

                                        tempBrd[0][col - 2, row + 2] = 0;
                                        tempBrd[0][col - 1, row + 1] = originBrd[col - 1, row + 1];
                                        tempBrd[0][col, row] = 1;
                                    }
                                }
                            }
                            else if (tempBrd[0][col - 1, row + 1] ==0)
                            {
                                if (row + 1 == BrdSize - 1)
                                {
                                    tempBrd[0][col - 1, row + 1] = -1;
                                }
                                else
                                {
                                    tempBrd[0][col - 1, row + 1] = 1;
                                }
                                tempBrd[0][col, row] = 0;

                                MoveScore = BrdScore(tempBrd[0]);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] {col,row, (short)(col - 1), (short)(row + 1) };
                                }

                                tempBrd[0][col - 1, row + 1] = 0;
                                tempBrd[0][col, row] = 1;
                            }
                        }
                        if (col != BrdSize - 1 && row!=BrdSize-1)
                        {
                            if (tempBrd[0][col + 1, row + 1] == 2 || tempBrd[0][col + 1, row + 1] == -2)
                            {
                                if (col != BrdSize - 2 && row != BrdSize-2)
                                {
                                    if (tempBrd[0][col + 2, row + 2] == 0)
                                    {
                                        tempBrd[0][col + 2, row + 2] = originBrd[col,row];
                                        tempBrd[0][col + 1, row + 1] = 0;
                                        tempBrd[0][col, row] = 0;

                                        MoveScore = BrdScore(tempBrd[0]);
                                        if (MoveScore > Score)
                                        {
                                            Score = MoveScore;
                                            ButtonLoc = new short[6] { col, row, (short)(col + 2), (short)(row + 2), (short)(col + 1), (short)(row + 1) };
                                        }

                                        tempBrd[0][col + 2, row + 2] = 0;
                                        tempBrd[0][col + 1, row + 1] = originBrd[col + 1, row + 1];
                                        tempBrd[0][col, row] = originBrd[col, row];
                                    }
                                }
                            }
                            else if (tempBrd[0][col + 1, row + 1] != 1 && tempBrd[0][col + 1, row + 1] != -1)
                            {
                                tempBrd[0][col + 1, row + 1] = originBrd[col,row];
                                tempBrd[0][col, row] = 0;

                                MoveScore = BrdScore(tempBrd[0]);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] { col, row, (short)(col + 1), (short)(row + 1) };
                                }

                                tempBrd[0][col + 1, row + 1] = 0;
                                tempBrd[0][col, row] = originBrd[col, row];
                            }
                        }
                    }
                    else if (tempBrd[0][col, row] == -1)//simulate king checker move
                    {
                        for (short i = 1; i < row + 1 && i < col + 1; i++)
                        {
                            if (tempBrd[0][col - i, row - i] == 0)
                            {
                                tempBrd[0][col - i, row - i] = -1;
                                tempBrd[0][col, row] = 0;

                                MoveScore = BrdScore(tempBrd[0]);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] { col, row, (short)(col  - i), (short)(row  - i) };
                                }

                                tempBrd[0][col - i, row - i] = 0;
                                tempBrd[0][col, row] = -1;
                            }
                            else if (tempBrd[0][col - i, row - i] == 1 || tempBrd[0][col - i, row - i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col - i, row - i] == 2 || tempBrd[0][col - i, row - i] == -2)
                            {
                                if (row - i > 0 && col - i > 0 && tempBrd[0][col - i - 1, row - i - 1] == 0)
                                {
                                    tempBrd[0][col - i - 1, row - i - 1] = -1;
                                    tempBrd[0][col - i, row - i] = 0;
                                    tempBrd[0][col, row] = 0;

                                    MoveScore = BrdScore(tempBrd[0]);
                                    if (MoveScore > Score)
                                    {
                                        Score = MoveScore;
                                        ButtonLoc = new short[6] { col, row, (short)(col - i-1), (short)(row - i-1),(short)(col-i),(short)(row-i) };
                                    }

                                    tempBrd[0][col - i - 1, row - i - 1] = 0;
                                    tempBrd[0][col - i, row - i] = originBrd[col-i,row-i];
                                    tempBrd[0][col, row] = -1;
                                }
                                break;
                            }
                        }

                        for (short i = 1; i < row + 1 && i < BrdSize - col; i++)
                        {
                            if (tempBrd[0][col + i, row - i] == 0)
                            {
                                tempBrd[0][col + i, row - i] = -1;
                                tempBrd[0][col, row] = 0;

                                MoveScore = BrdScore(tempBrd[0]);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] { col, row, (short)(col + i), (short)(row - i) };
                                }

                                tempBrd[0][col + i, row - i] = 0;
                                tempBrd[0][col, row] = -1;
                            }
                            else if (tempBrd[0][col + i, row - i] == 1 || tempBrd[0][col + i, row - i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col + i, row - i] == 2 || tempBrd[0][col + i, row - i] == -2)
                            {
                                if (row - i > 0 && col + i < BrdSize - 1 && tempBrd[0][col + i + 1, row - i - 1] == 0)
                                {
                                    tempBrd[0][col + i + 1, row - i - 1] = -1;
                                    tempBrd[0][col + i, row - i] = 0;
                                    tempBrd[0][col, row] = 0;

                                    MoveScore = BrdScore(tempBrd[0]);
                                    if (MoveScore > Score)
                                    {
                                        Score = MoveScore;
                                        ButtonLoc = new short[6] { col, row, (short)(col + i + 1), (short)(row - i - 1), (short)(col + i), (short)(row - i) };
                                    }

                                    tempBrd[0][col + i + 1, row - i - 1] = 0;
                                    tempBrd[0][col + i, row - i] = originBrd[col + i, row - i];
                                    tempBrd[0][col, row] = -1;
                                }
                                break;
                            }
                        }

                        for (short i = 1; i < BrdSize - row && i < col + 1; i++)
                        {
                            if (tempBrd[0][col - i, row + i] == 0)
                            {
                                tempBrd[0][col - i, row + i] = -1;
                                tempBrd[0][col, row] = 0;

                                MoveScore = BrdScore(tempBrd[0]);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] { col, row, (short)(col - i), (short)(row + i) };
                                }

                                tempBrd[0][col - i, row + i] = 0;
                                tempBrd[0][col, row] = -1;
                            }
                            else if (tempBrd[0][col - i, row + i] == 1 || tempBrd[0][col - i, row + i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col - i, row + i] == 2 || tempBrd[0][col - i, row + i] == -2)
                            {
                                if (row + i < BrdSize - 1 && col - i > 0 && tempBrd[0][col - i - 1, row + i + 1] == 0)
                                {
                                    tempBrd[0][col - i - 1, row + i + 1] = -1;
                                    tempBrd[0][col - i, row + i] = 0;
                                    tempBrd[0][col, row] = 0;

                                    MoveScore = BrdScore(tempBrd[0]);
                                    if (MoveScore > Score)
                                    {
                                        Score = MoveScore;
                                        ButtonLoc = new short[6] { col, row, (short)(col - i - 1), (short)(row + i + 1), (short)(col - i), (short)(row + i) };
                                    }

                                    tempBrd[0][col - i - 1, row + i + 1] = 0;
                                    tempBrd[0][col - i, row + i] = originBrd[col - i, row + i];
                                    tempBrd[0][col, row] = -1;
                                }
                                break;
                            }
                        }

                        for (short i = 1; i < BrdSize - row && i < BrdSize - col; i++)
                        {
                            if (tempBrd[0][col + i, row + i] == 0)
                            {
                                tempBrd[0][col + i, row + i] = -1;
                                tempBrd[0][col, row] = 0;

                                MoveScore = BrdScore(tempBrd[0]);
                                if (MoveScore > Score)
                                {
                                    Score = MoveScore;
                                    ButtonLoc = new short[4] { col, row, (short)(col + i), (short)(row + i) };
                                }

                                tempBrd[0][col + i, row + i] = 0;
                                tempBrd[0][col, row] = -1;
                            }
                            else if (tempBrd[0][col + i, row + i] == 1 || tempBrd[0][col + i, row + i] == -1)
                            {
                                break;
                            }
                            else if (tempBrd[0][col + i, row + i] == 2 || tempBrd[0][col + i, row + i] == -2)
                            {
                                if (row + i < BrdSize - 1 && col + i < BrdSize - 1 && tempBrd[0][col + i + 1, row + i + 1] == 0)
                                {
                                    tempBrd[0][col + i + 1, row + i + 1] = -1;
                                    tempBrd[0][col + i, row + i] = 0;
                                    tempBrd[0][col, row] = 0;

                                    MoveScore = BrdScore(tempBrd[0]);
                                    if (MoveScore > Score)
                                    {
                                        Score = MoveScore;
                                        ButtonLoc = new short[6] { col, row, (short)(col + i + 1), (short)(row + i + 1), (short)(col + i), (short)(row + i) };
                                    }

                                    tempBrd[0][col + i + 1, row + i + 1] = 0;
                                    tempBrd[0][col + i, row + i] = originBrd[col + i, row + i];
                                    tempBrd[0][col, row] = -1;
                                }
                                break;
                            }
                        }
                    }
                }
            }
                        
            return ButtonLoc;
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            BrdSize = (short)(BrdSizeBox.Value);
            Board = new Button[BrdSize, BrdSize];
            intBrd = new short[BrdSize, BrdSize];
            short ButtonSize = 40;

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

                    Board[i, k].Click += Form1_Click;

                    if ((i + k) % 2 == 0)//sets the buttons color and type
                    {
                        if (k < 3)
                        {
                            Board[i, k].BackColor = Color.DarkOrchid;
                            intBrd[i, k] = 1;
                            BlackCheckers++;
                        }
                        else if (k > BrdSize - 4)
                        {
                            Board[i, k].BackColor = Color.SandyBrown;
                            intBrd[i, k] = 2;
                            WhiteCheckers++;
                        }
                        else
                        {
                            Board[i, k].BackColor = Color.Black;
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
        }

        void Form1_Click(object sender, EventArgs e)
        {
            short[] ButtonLoc = (short[])(((Button)(sender)).Tag);
            short col = ButtonLoc[0];
            short row = ButtonLoc[1];
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

            if (intBrd[col, row] == turnVal)
            {
                DyingButtons[0] = null;
                DyingButtons[1] = null;
                DyingButtons[2] = null;
                DyingButtons[3] = null;

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
                            Board[i, k].BackColor = Color.Black;
                            intBrd[i, k] = 0;
                        }
                    }
                }

                if (col != 0)//checks that the button isn't on an edge
                {
                    if (intBrd[col - 1, row - moveDir] == rivalTurnVal || intBrd[col - 1, row - moveDir] == -rivalTurnVal)//checks if there is a rival in this direction
                    {
                        if (col != 1 && row != rivalTurnVal / 2 * (BrdSize - 3) + 1)
                        {
                            if (intBrd[col - 2, row - 2 * moveDir] == 0)//checks to see if the enemy is in a position where he can be eaten
                            {
                                Board[col - 2, row - 2 * moveDir].BackColor = Color.Yellow;
                                intBrd[col - 2, row - 2 * moveDir] = 3;
                                DyingButtons[0] = new short[2] { (short)(col - 1), (short)(row - moveDir) };
                                PressedButton = new short[2] { col, row };
                            }
                        }
                    }
                    else if (intBrd[col - 1, row - moveDir] != turnVal && intBrd[col - 1, row - moveDir] != -turnVal)
                    {
                        Board[col - 1, row - moveDir].BackColor = Color.Yellow;
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
                                Board[col + 2, row - 2 * moveDir].BackColor = Color.Yellow;
                                intBrd[col + 2, row - 2 * moveDir] = 3;
                                DyingButtons[1] = new short[2] { (short)(col + 1), (short)(row - moveDir) };
                                PressedButton = new short[2] { col, row };
                            }
                        }
                    }
                    else if (intBrd[col + 1, row - moveDir] != turnVal && intBrd[col + 1, row - moveDir] != -turnVal)
                    {
                        Board[col + 1, row - moveDir].BackColor = Color.Yellow;
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

                for (short i = 0; i < BrdSize; i++)//clean yellow
                {
                    for (short k = 0; k < BrdSize; k++)
                    {
                        if (intBrd[i, k] != 3)
                        {
                            continue;
                        }
                        else
                        {
                            Board[i, k].BackColor = Color.Black;
                            intBrd[i, k] = 0;
                        }
                    }
                }

                bool CanPlace = false;

                for (short i = 1; i < row + 1 && i < col + 1; i++)//create yellow tiles in all directions
                {
                    if (intBrd[col - i, row - i] == 0)
                    {
                        Board[col - i, row - i].BackColor = Color.Yellow;
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
                            Board[col - i - 1, row - i - 1].BackColor = Color.Yellow;
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
                        Board[col + i, row - i].BackColor = Color.Yellow;
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
                            Board[col + i + 1, row - i - 1].BackColor = Color.Yellow;
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
                        Board[col - i, row + i].BackColor = Color.Yellow;
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
                            Board[col - i - 1, row + i + 1].BackColor = Color.Yellow;
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
                        Board[col + i, row + i].BackColor = Color.Yellow;
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
                            Board[col + i + 1, row + i + 1].BackColor = Color.Yellow;
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
                if (turnVal == 2)//Move Black Checkers
                {
                    if (DyingButtons[0] != null && DyingButtons[0][0] == col + 1 && DyingButtons[0][1] == row + 1)
                    {//check if a checker is eaten via this move
                        intBrd[DyingButtons[0][0], DyingButtons[0][1]] = 0;
                        Board[DyingButtons[0][0], DyingButtons[0][1]].BackColor = Color.Black;
                        WhiteCheckers--;
                    }
                    else if (DyingButtons[1] != null && DyingButtons[1][0] == col - 1 && DyingButtons[1][1] == row + 1)
                    {
                        intBrd[DyingButtons[1][0], DyingButtons[1][1]] = 0;
                        Board[DyingButtons[1][0], DyingButtons[1][1]].BackColor = Color.Black;
                        WhiteCheckers--;
                    }
                    else if (DyingButtons[2] != null && DyingButtons[2][0] == col + 1 && DyingButtons[2][1] == row - 1)
                    {
                        intBrd[DyingButtons[2][0], DyingButtons[2][1]] = 0;
                        Board[DyingButtons[2][0], DyingButtons[2][1]].BackColor = Color.Black;
                        WhiteCheckers--;
                    }
                    else if (DyingButtons[3] != null && DyingButtons[3][0] == col - 1 && DyingButtons[3][1] == row - 1)
                    {
                        intBrd[DyingButtons[3][0], DyingButtons[3][1]] = 0;
                        Board[DyingButtons[3][0], DyingButtons[3][1]].BackColor = Color.Black;
                        WhiteCheckers--;
                    }

                    if (intBrd[PressedButton[0], PressedButton[1]] == turnVal && row > 0)//check if it's a king or becomes one
                    {
                        Board[col, row].BackColor = Color.SandyBrown;
                        intBrd[col, row] = 2;
                    }
                    else
                    {
                        Board[col, row].BackColor = Color.Brown;
                        intBrd[col, row] = -2;
                    }

                    Board[PressedButton[0], PressedButton[1]].BackColor = Color.Black;
                    intBrd[PressedButton[0], PressedButton[1]] = 0;

                    for (short i = 0; i < BrdSize; i++)//cleans yellow tiles
                    {
                        for (short k = 0; k < BrdSize; k++)
                        {
                            if (intBrd[i, k] != 3)
                            {
                                continue;
                            }
                            else
                            {
                                Board[i, k].BackColor = Color.Black;
                                intBrd[i, k] = 0;
                            }
                        }
                    }

                    PressedButton = null;

                    WhiteCheckerBox.Text = "White Checkers: " + WhiteCheckers.ToString();//updates textboxes
                    BlackCheckerBox.Text = "Black Checkers: " + BlackCheckers.ToString();
                    TurnBox.Text = "White Turn";

                    if (WhiteCheckers == 1)//win condition
                    {
                        MessageBox.Show("Black Wins!");
                    }

                    turnVal = rivalTurnVal;
                }
                else
                {
                    if (intBrd[PressedButton[0], PressedButton[1]] == turnVal)//same for the most part as the black move function
                    {
                        if (DyingButtons[0] != null && DyingButtons[0][0] == col + 1 && DyingButtons[0][1] == row - 1)
                        {
                            intBrd[DyingButtons[0][0], DyingButtons[0][1]] = 0;
                            Board[DyingButtons[0][0], DyingButtons[0][1]].BackColor = Color.Black;
                            BlackCheckers--;
                        }
                        else if (DyingButtons[1] != null && DyingButtons[1][0] == col - 1 && DyingButtons[1][1] == row - 1)
                        {
                            intBrd[DyingButtons[1][0], DyingButtons[1][1]] = 0;
                            Board[DyingButtons[1][0], DyingButtons[1][1]].BackColor = Color.Black;
                            BlackCheckers--;
                        }
                    }//here because the DyingButtons[] array doesn't perfectly line up there is a difference between kings and non-kings
                    else
                    {
                        if (DyingButtons[0] != null && DyingButtons[0][0] == col + 1 && DyingButtons[0][1] == row + 1)
                        {
                            intBrd[DyingButtons[0][0], DyingButtons[0][1]] = 0;
                            Board[DyingButtons[0][0], DyingButtons[0][1]].BackColor = Color.Black;
                            BlackCheckers--;
                        }
                        else if (DyingButtons[1] != null && DyingButtons[1][0] == col - 1 && DyingButtons[1][1] == row + 1)
                        {
                            intBrd[DyingButtons[1][0], DyingButtons[1][1]] = 0;
                            Board[DyingButtons[1][0], DyingButtons[1][1]].BackColor = Color.Black;
                            BlackCheckers--;
                        }
                        else if (DyingButtons[0] != null && DyingButtons[2][0] == col + 1 && DyingButtons[2][1] == row - 1)
                        {
                            intBrd[DyingButtons[2][0], DyingButtons[2][1]] = 0;
                            Board[DyingButtons[2][0], DyingButtons[2][1]].BackColor = Color.Black;
                            BlackCheckers--;
                        }
                        else if (DyingButtons[3] != null && DyingButtons[3][0] == col - 1 && DyingButtons[3][1] == row - 1)
                        {
                            intBrd[DyingButtons[3][0], DyingButtons[3][1]] = 0;
                            Board[DyingButtons[3][0], DyingButtons[3][1]].BackColor = Color.Black;
                            BlackCheckers--;
                        }
                    }

                    if (intBrd[PressedButton[0], PressedButton[1]] == turnVal && row < BrdSize - 1)
                    {
                        Board[col, row].BackColor = Color.DarkOrchid;
                        intBrd[col, row] = 1;
                    }
                    else
                    {
                        Board[col, row].BackColor = Color.Purple;
                        intBrd[col, row] = -1;
                    }

                    Board[PressedButton[0], PressedButton[1]].BackColor = Color.Black;
                    intBrd[PressedButton[0], PressedButton[1]] = 0;

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
                                Board[i, k].BackColor = Color.Black;
                                intBrd[i, k] = 0;
                            }
                        }
                    }

                    PressedButton = null;

                    WhiteCheckerBox.Text = "White Checkers: " + WhiteCheckers.ToString();
                    BlackCheckerBox.Text = "Black Checkers: " + BlackCheckers.ToString();
                    TurnBox.Text = "Black Turn";

                    if (BlackCheckers == 1)
                    {
                        MessageBox.Show("White Wins!");
                    }

                    turnVal = rivalTurnVal;
                }
            }
        }

        private void CompButton_Click(object sender, EventArgs e)
        {
            if (turnVal == 1)
            {
                short[] ButtonLoc = CompMove(intBrd);

                if (ButtonLoc[3] < BrdSize-1 && intBrd[ButtonLoc[0], ButtonLoc[1]] != -2)
                {
                    intBrd[ButtonLoc[2], ButtonLoc[3]] = 1;
                    Board[ButtonLoc[2], ButtonLoc[3]].BackColor = Color.DarkOrchid;
                }
                else
                {
                    intBrd[ButtonLoc[2], ButtonLoc[3]] = -1;
                    Board[ButtonLoc[2], ButtonLoc[3]].BackColor = Color.Purple;
                }

                if (ButtonLoc.Length == 6)
                {
                    intBrd[ButtonLoc[4], ButtonLoc[5]] = 0;
                    Board[ButtonLoc[4], ButtonLoc[5]].BackColor = Color.Black;
                    BlackCheckers--;

                    BlackCheckerBox.Text = "Black Checkers: " + BlackCheckers.ToString();
                }

                intBrd[ButtonLoc[0], ButtonLoc[1]] = 0;
                Board[ButtonLoc[0], ButtonLoc[1]].BackColor = Color.Black;

                turnVal = 2;
            }
            else
            {
                MessageBox.Show("It's currently your turn");
            }
        }
    }
}