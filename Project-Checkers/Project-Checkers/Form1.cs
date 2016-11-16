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
        short[] PressedButton=new short[2]{5,5};
        short turnVal = 2;
        bool CleanYellow = false;

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
                    Board[i, k].Tag = new short[4] { i, k, 0, 0 };

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
        }

        void Form1_Click(object sender, EventArgs e)
        {
            short[] ButtonTags = (short[])(((Button)(sender)).Tag);
            short col = ButtonTags[0];
            short row = ButtonTags[1];
            short BrdLength = (short)(intBrd.GetLength(0));
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

            if (CleanYellow == true) {
                if (intBrd[col, row] == turnVal)
                {
                    if (PressedButton[0] != 0 && PressedButton[1] != 0)
                    {
                        if (intBrd[PressedButton[0] - 1, PressedButton[1] - moveDir] == rivalTurnVal)
                        {
                            if (PressedButton[0] != 1 && PressedButton[1] != 1)
                            {
                                if (intBrd[PressedButton[0] - 2, PressedButton[1] - 2 * moveDir] == 0)
                                {
                                    Board[PressedButton[0] - 2, PressedButton[1] - 2 * moveDir].BackColor = Color.Black;
                                    intBrd[PressedButton[0] - 2, PressedButton[1] - 2 * moveDir] = 0;
                                }
                            }
                        }
                        else
                        {
                            Board[PressedButton[0] - 1, PressedButton[1] - moveDir].BackColor = Color.Black;
                            intBrd[PressedButton[0] - 1, PressedButton[1] - moveDir] = 0;
                        }
                    }
                    if (PressedButton[0] != BrdLength - 1 && PressedButton[1] != BrdLength - 1)
                    {
                        if (intBrd[PressedButton[0] + 1, PressedButton[1] - moveDir] == rivalTurnVal)
                        {
                            if (PressedButton[0] != BrdLength - 2 && PressedButton[1] != BrdLength - 2)
                            {
                                if (intBrd[PressedButton[0] + 2, PressedButton[1] - 2 * moveDir] == 0)
                                {
                                    Board[PressedButton[0] + 2, PressedButton[1] - 2 * moveDir].BackColor = Color.Black;
                                    intBrd[PressedButton[0] + 2, PressedButton[1] - 2 * moveDir] = 0;
                                }
                            }
                        }
                        else
                        {
                            Board[PressedButton[0] + 1, PressedButton[1] - moveDir].BackColor = Color.Black;
                            intBrd[PressedButton[0] + 1, PressedButton[1] - moveDir] = 0;
                        }
                    }
                    CleanYellow = false;
                }

                if (col != 0 && row != 0)//checks that the button isn't on an edge
                {
                    if (intBrd[col - 1, row - moveDir] == rivalTurnVal)//checks if there is a rival in this direction
                    {
                        if (col != 1 && row != 1)
                        {
                            if (intBrd[col - 2, row - 2 * moveDir] == 0)//checks to see if the enemy is in a position where he can be eaten
                            {
                                Board[col - 2, row - 2 * moveDir].BackColor = Color.Yellow;
                                intBrd[col - 2, row - 2 * moveDir] = 3;
                                CleanYellow = true;
                                PressedButton = new short[2] { col, row };
                            }
                        }
                    }
                    else if (intBrd[col - 1, row - moveDir] != turnVal)
                    {
                        Board[col - 1, row - moveDir].BackColor = Color.Yellow;
                        intBrd[col - 1, row - moveDir] = 3;
                        CleanYellow = true;
                        PressedButton = new short[2] { col, row };
                    }
                }
                if (col != BrdLength - 1 && row != BrdLength - 1)
                {
                    if (intBrd[col + 1, row - moveDir] == rivalTurnVal)
                    {
                        if (col != BrdLength - 2 && row != BrdLength - 2)
                        {
                            if (intBrd[col + 2, row - 2 * moveDir] == 0)
                            {
                                Board[col + 2, row - 2 * moveDir].BackColor = Color.Yellow;
                                intBrd[col + 2, row - 2 * moveDir] = 3;
                                CleanYellow = true;
                                PressedButton = new short[2] { col, row };
                            }
                        }
                    }
                    else if (intBrd[col + 1, row - moveDir] != turnVal)
                    {
                        Board[col + 1, row - moveDir].BackColor = Color.Yellow;
                        intBrd[col + 1, row - moveDir] = 3;
                        CleanYellow = true;
                        PressedButton = new short[2] { col, row };
                    }
                }
            }
            else if (intBrd[col, row] == 3)
            {
                if(turnVal==2)
                {                    
                    if (PressedButton[0] != 0 && PressedButton[1] != 0)
                    {
                        if (intBrd[PressedButton[0] - 1, PressedButton[1] - moveDir] == rivalTurnVal)
                        {
                            if (PressedButton[0] != 1 && PressedButton[1] != 1)
                            {
                                if (intBrd[PressedButton[0] - 2, PressedButton[1] - 2 * moveDir] == 0)
                                {
                                    Board[PressedButton[0] - 2, PressedButton[1] - 2 * moveDir].BackColor = Color.Black;
                                    intBrd[PressedButton[0] - 2, PressedButton[1] - 2 * moveDir] = 0;
                                }
                            }
                        }
                        else if (intBrd[PressedButton[0] - 1, PressedButton[1] - moveDir] != turnVal)
                        {
                            Board[PressedButton[0] - 1, PressedButton[1] - moveDir].BackColor = Color.Black;
                            intBrd[PressedButton[0] - 1, PressedButton[1] - moveDir] = 0;
                        }
                    }
                    if (PressedButton[0] != BrdLength - 1 && PressedButton[1] != BrdLength - 1)
                    {
                        if (intBrd[PressedButton[0] + 1, PressedButton[1] - moveDir] == rivalTurnVal)
                        {
                            if (PressedButton[0] != BrdLength - 2 && PressedButton[1] != BrdLength - 2)
                            {
                                if (intBrd[PressedButton[0] + 2, PressedButton[1] - 2 * moveDir] == 0)
                                {
                                    Board[PressedButton[0] + 2, PressedButton[1] - 2 * moveDir].BackColor = Color.Black;
                                    intBrd[PressedButton[0] + 2, PressedButton[1] - 2 * moveDir] = 0;
                                }
                            }
                        }
                        else if (intBrd[PressedButton[0] + 1, PressedButton[1] - moveDir] != turnVal)
                        {
                            Board[PressedButton[0] + 1, PressedButton[1] - moveDir].BackColor = Color.Black;
                            intBrd[PressedButton[0] + 1, PressedButton[1] - moveDir] = 0;
                        }
                    }
                    CleanYellow = false;

                    Board[col, row].BackColor = Color.SandyBrown;
                    intBrd[col, row] = 2;
                    Board[PressedButton[0], PressedButton[1]].BackColor = Color.Black;
                    intBrd[PressedButton[0], PressedButton[1]] = 0;
                    turnVal = rivalTurnVal;
                }
                else
                {
                    if (PressedButton[0] != 0 && PressedButton[1] != 0)//makes all the possible-place buttons return to default
                    {
                        if (intBrd[PressedButton[0] - 1, PressedButton[1] - moveDir] == rivalTurnVal)
                        {
                            if (PressedButton[0] != 1 && PressedButton[1] != 1)
                            {
                                if (intBrd[PressedButton[0] - 2, PressedButton[1] - 2 * moveDir] == 0)
                                {
                                    Board[PressedButton[0] - 2, PressedButton[1] - 2 * moveDir].BackColor = Color.Black;
                                    intBrd[PressedButton[0] - 2, PressedButton[1] - 2 * moveDir] = 0;
                                }
                            }
                        }
                        else if (intBrd[PressedButton[0] - 1, PressedButton[1] - moveDir] != turnVal)
                        {
                            Board[PressedButton[0] - 1, PressedButton[1] - moveDir].BackColor = Color.Black;
                            intBrd[PressedButton[0] - 1, PressedButton[1] - moveDir] = 0;
                        }
                    }
                    if (PressedButton[0] != BrdLength - 1 && PressedButton[1] != BrdLength - 1)
                    {
                        if (intBrd[PressedButton[0] + 1, PressedButton[1] - moveDir] == rivalTurnVal)
                        {
                            if (PressedButton[0] != BrdLength - 2 && PressedButton[1] != BrdLength - 2)
                            {
                                if (intBrd[PressedButton[0] + 2, PressedButton[1] - 2 * moveDir] == 0)
                                {
                                    Board[PressedButton[0] + 2, PressedButton[1] - 2 * moveDir].BackColor = Color.Black;
                                    intBrd[PressedButton[0] + 2, PressedButton[1] - 2 * moveDir] = 0;
                                }
                            }
                        }
                        else if (intBrd[PressedButton[0] + 1, PressedButton[1] - moveDir] != turnVal)
                        {
                            Board[PressedButton[0] + 1, PressedButton[1] - moveDir].BackColor = Color.Black;
                            intBrd[PressedButton[0] + 1, PressedButton[1] - moveDir] = 0;
                        }
                    }
                    CleanYellow = false;

                    Board[col, row].BackColor = Color.DarkOrchid;
                    intBrd[col, row] = 1;
                    Board[PressedButton[0], PressedButton[1]].BackColor = Color.Black;
                    intBrd[PressedButton[0], PressedButton[1]] = 0;
                    turnVal = rivalTurnVal;
                }
            }
        }
    }
}