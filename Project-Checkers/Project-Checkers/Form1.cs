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
                        if (col != 1 && row != (-moveDir*(BrdSize - 2)+BrdSize-2)/2)
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
                        if (col != BrdSize - 2 && row != (-moveDir * (BrdSize - 2) + BrdSize - 2) / 2)
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

                bool CanPlace = false;

                for (short i = 1; i < row && i < col; i++)
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
                        }
                        break;
                    }
                }

                for (short i = 1; i < row && i < BrdSize - col; i++)
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
                        }
                        break;
                    }
                }

                for (short i = 1; i < BrdSize - row && i < col; i++)
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
                if (turnVal == 2)
                {
                    if (DyingButtons[0] != null && DyingButtons[0][0] == col + 1 && DyingButtons[0][1] == row + 1)
                    {
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

                    if (intBrd[PressedButton[0], PressedButton[1]] == turnVal && row > 0)
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
                    TurnBox.Text = "White Turn";

                    if (WhiteCheckers == 1)
                    {
                        MessageBox.Show("White Wins!");
                    }

                    turnVal = rivalTurnVal;
                }
                else
                {
                    if (intBrd[PressedButton[0], PressedButton[1]] == turnVal)
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
                    }
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
                        MessageBox.Show("Is king");
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
                        MessageBox.Show("Black Wins!");
                    }

                    turnVal = rivalTurnVal;
                }
            }
        }
    }
}