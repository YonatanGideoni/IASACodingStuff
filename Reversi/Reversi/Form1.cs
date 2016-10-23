using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi
{
    public partial class Form1 : Form
    {
        Button[,] Board;
        short[,] intBrd;
        short BlackTile = 2;
        short BlueTile = 2;
        short turnType = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            short GridLength = Convert.ToInt16(GridSizeBox.Value + 2);
            short ButtonSize = 30;

            Board = new Button[GridLength, GridLength];
            intBrd = new short[GridLength, GridLength];

            BoardPanel.Size = new Size(GridLength * ButtonSize, GridLength * ButtonSize);
            this.Size = new Size(Math.Max(BoardPanel.Width + BoardPanel.Location.X + 50, 400), BoardPanel.Height + BoardPanel.Location.Y + 50);

            for (short i = 0; i < GridLength; i++)
            {
                for (short k = 0; k < GridLength; k++)
                {
                    Board[i, k] = new Button();
                    Board[i, k].Size = new Size(ButtonSize, ButtonSize);
                    Board[i, k].Location = new Point(ButtonSize * i, ButtonSize * k);
                    intBrd[i, k] = 0;
                    Board[i, k].BackColor = default(Color);
                    Board[i, k].Tag = new short[2] { i, k };

                    BoardPanel.Controls.Add(Board[i, k]);
                    Board[i, k].Click += Form1_Click;
                    
                    if (i == 0 || i == GridLength - 1 || k == 0 || k == GridLength - 1)
                    {
                        Board[i, k].Enabled = false;
                        Board[i, k].Visible = false;
                    }
                }
            }

            Board[GridLength / 2, GridLength / 2].BackColor = Color.Black;
            Board[GridLength / 2 - 1, GridLength / 2].BackColor = Color.Blue;
            Board[GridLength / 2, GridLength / 2 - 1].BackColor = Color.Blue;
            Board[GridLength / 2 - 1, GridLength / 2 - 1].BackColor = Color.Black;
            Board[GridLength / 2, GridLength / 2].Enabled = false;
            Board[GridLength / 2 - 1, GridLength / 2].Enabled = false;
            Board[GridLength / 2, GridLength / 2 - 1].Enabled = false;
            Board[GridLength / 2 - 1, GridLength / 2 - 1].Enabled = false;

            intBrd[GridLength / 2, GridLength / 2] = 1;
            intBrd[GridLength / 2 - 1, GridLength / 2] = 2;
            intBrd[GridLength / 2, GridLength / 2 - 1] = 2;
            intBrd[GridLength / 2 - 1, GridLength / 2 - 1] = 1;

            turnType = 1;
            TurnTextBox.Text = "Black's Turn";
            TileCountBox.Text = "Black Tiles:" + BlackTile.ToString() + " Blue Tiles:" + BlueTile.ToString();
        }

        void Form1_Click(object sender, EventArgs e)
        {
            short[] ButtonLoc = (short[])((Array)(((Button)(sender)).Tag));
            short row = ButtonLoc[0];
            short col = ButtonLoc[1];
            bool CanPlace = false;

            if (turnType != intBrd[row + 1, col])
            {
                for (short i = 1; i < Board.GetLength(0) - row - 2; i++)
                {
                    if (turnType != intBrd[row + i, col] && intBrd[row + i, col] != 0)//check if a touching tile is an enemy tile
                    {
                        continue;
                    }
                    else if (intBrd[row + i, col] == 0) //if it is an uninhabited tile, stop checking
                    {
                        break;
                    }
                    else //if the next tile over is a friendly tile, start placing friendly tiles  
                    {
                        for (short k = 1; k < i; k++)
                        {
                            intBrd[row + k, col] = turnType;
                            if (turnType == 1)
                            {
                                Board[row + k, col].BackColor = Color.Black;
                                Board[row + k, col].Enabled = false;
                                BlackTile++;
                                BlueTile--;
                            }
                            else
                            {
                                Board[row + k, col].BackColor = Color.Blue;
                                Board[row + k, col].Enabled = false;
                                BlueTile++;
                                BlackTile--;
                            }
                        }
                        CanPlace = true;
                        break;
                    }
                }
            }
            // Same deal for all the other loops. Each checks a different direction

            if (turnType != intBrd[row, col + 1])
            {
                for (short i = 1; i < Board.GetLength(0) - col - 2; i++)
                {
                    if (turnType != intBrd[row, col + i] && intBrd[row, col + i] != 0)
                    {
                        continue;
                    }
                    else if (intBrd[row, col + i] == 0)
                    {
                        break;
                    }
                    else
                    {
                        for (short k = 1; k < i; k++)
                        {
                            intBrd[row, col + k] = turnType;
                            if (turnType == 1)
                            {
                                Board[row, col + k].BackColor = Color.Black;
                                Board[row, col + k].Enabled = false;
                                BlackTile++;
                                BlueTile--;
                            }
                            else
                            {
                                Board[row, col + k].BackColor = Color.Blue;
                                Board[row, col + k].Enabled = false;
                                BlueTile++;
                                BlackTile--;
                            }
                        }
                        CanPlace = true;
                        break;
                    }
                }
            }

            if (turnType != intBrd[row, col - 1])
            {
                for (short i = 1; i < col; i++)
                {
                    if (turnType != intBrd[row, col - i] && intBrd[row, col - i] != 0)
                    {
                        continue;
                    }
                    else if (intBrd[row, col - i] == 0)
                    {
                        break;
                    }
                    else
                    {
                        for (short k = 1; k < i; k++)
                        {
                            intBrd[row, col - k] = turnType;
                            if (turnType == 1)
                            {
                                Board[row, col - k].BackColor = Color.Black;
                                Board[row, col - k].Enabled = false;
                                BlackTile++;
                                BlueTile--;
                            }
                            else
                            {
                                Board[row, col - k].BackColor = Color.Blue;
                                Board[row, col - k].Enabled = false;
                                BlueTile++;
                                BlackTile--;
                            }
                        }
                        CanPlace = true;
                        break;
                    }
                }
            }

            if (turnType != intBrd[row - 1, col])
            {
                for (short i = 1; i < row; i++)
                {
                    if (turnType != intBrd[row - i, col] && intBrd[row - i, col] != 0)
                    {
                        continue;
                    }
                    else if (intBrd[row - i, col] == 0)
                    {
                        break;
                    }
                    else
                    {
                        for (short k = 1; k < i; k++)
                        {
                            intBrd[row - k, col] = turnType;
                            if (turnType == 1)
                            {
                                Board[row - k, col].BackColor = Color.Black;
                                Board[row - k, col].Enabled = false;
                                BlackTile++;
                                BlueTile--;
                            }
                            else
                            {
                                Board[row - k, col].BackColor = Color.Blue;
                                Board[row - k, col].Enabled = false;
                                BlueTile++;
                                BlackTile--;
                            }
                        }
                        CanPlace = true;
                        break;
                    }
                }
            }

            if (turnType != intBrd[row - 1, col - 1])
            {
                for (short i = 1; i < row && i < col; i++)
                {
                    if (turnType != intBrd[row - i, col - i] && intBrd[row - i, col - i] != 0)
                    {
                        continue;
                    }
                    else if (intBrd[row - i, col - i] == 0)
                    {
                        break;
                    }
                    else
                    {
                        for (short k = 1; k < i; k++)
                        {
                            intBrd[row - k, col - k] = turnType;
                            if (turnType == 1)
                            {
                                Board[row - k, col - k].BackColor = Color.Black;
                                Board[row - k, col - k].Enabled = false;
                                BlackTile++;
                                BlueTile--;
                            }
                            else
                            {
                                Board[row - k, col - k].BackColor = Color.Blue;
                                Board[row - k, col - k].Enabled = false;
                                BlueTile++;
                                BlackTile--;
                            }
                        }
                        CanPlace = true;
                        break;
                    }
                }
            }

            if (turnType != intBrd[row + 1, col - 1])
            {
                for (short i = 1; i < Board.GetLength(0) - 1 - row && i < col; i++)
                {
                    if (turnType != intBrd[row + i, col - i] && intBrd[row + i, col - i] != 0)
                    {
                        continue;
                    }
                    else if (intBrd[row + i, col - i] == 0)
                    {
                        break;
                    }
                    else
                    {
                        for (short k = 1; k < i; k++)
                        {
                            intBrd[row + k, col - k] = turnType;
                            if (turnType == 1)
                            {
                                Board[row + k, col - k].BackColor = Color.Black;
                                Board[row + k, col - k].Enabled = false;
                                BlackTile++;
                                BlueTile--;
                            }
                            else
                            {
                                Board[row + k, col - k].BackColor = Color.Blue;
                                Board[row + k, col - k].Enabled = false;
                                BlueTile++;
                                BlackTile--;
                            }
                        }
                        CanPlace = true;
                        break;
                    }
                }
            }

            if (turnType != intBrd[row + 1, col + 1])
            {
                for (short i = 1; i < Board.GetLength(0) - 1 - row && i < Board.GetLength(0) - 1 - col; i++)
                {
                    if (turnType != intBrd[row + i, col + i] && intBrd[row + i, col + i] != 0)
                    {
                        continue;
                    }
                    else if (intBrd[row + i, col + i] == 0)
                    {
                        break;
                    }
                    else
                    {
                        for (short k = 1; k < i; k++)
                        {
                            intBrd[row + k, col + k] = turnType;
                            if (turnType == 1)
                            {
                                Board[row + k, col + k].BackColor = Color.Black;
                                Board[row + k, col + k].Enabled = false;
                                BlackTile++;
                                BlueTile--;
                            }
                            else
                            {
                                Board[row + k, col + k].BackColor = Color.Blue;
                                Board[row + k, col + k].Enabled = false;
                                BlueTile++;
                                BlackTile--;
                            }
                        }
                        CanPlace = true;
                        break;
                    }
                }
            }

            if (turnType != intBrd[row - 1, col + 1])
            {
                for (short i = 1; i < row && i < Board.GetLength(0) - 1 - col; i++)
                {
                    if (turnType != intBrd[row - i, col + i] && intBrd[row - i, col + i] != 0)
                    {
                        continue;
                    }
                    else if (intBrd[row - i, col + i] == 0)
                    {
                        break;
                    }
                    else
                    {
                        for (short k = 1; k < i; k++)
                        {
                            intBrd[row - k, col + k] = turnType;
                            if (turnType == 1)
                            {
                                Board[row - k, col + k].BackColor = Color.Black;
                                Board[row - k, col + k].Enabled = false;
                                BlackTile++;
                                BlueTile--;
                            }
                            else
                            {
                                Board[row - k, col + k].BackColor = Color.Blue;
                                Board[row - k, col + k].Enabled = false;
                                BlueTile++;
                                BlackTile--;
                            }
                        }
                        CanPlace = true;
                        break;
                    }
                }
            }

            if (turnType == 1 && CanPlace == true)
            {
                intBrd[row, col] = 1;
                Board[row, col].BackColor = Color.Black;
                Board[row, col].Enabled = false;
                BlackTile++;
                turnType = 2;
                TurnTextBox.Text = "Blue's Turn";
            }
            else if (CanPlace == true)
            {
                intBrd[row, col] = 2;
                Board[row, col].BackColor = Color.Blue;
                Board[row, col].Enabled = false;
                BlueTile++;
                turnType = 1;
                TurnTextBox.Text = "Black's Turn";
            }

            TileCountBox.Text = "Black Tiles:" + BlackTile.ToString() + " Blue Tiles:" + BlueTile.ToString();


            if (BlackTile + BlueTile == (Board.GetLength(0) - 2) * (Board.GetLength(0) - 2))//Win condition
            {
                if (BlackTile > BlueTile)
                {
                    MessageBox.Show("Black Wins!");
                }
                else
                {
                    MessageBox.Show("Blue Wins!");
                }
            }
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            if (turnType == 1)
            {
                TurnTextBox.Text = "Blue's Turn";
                turnType = 2;
            }
            else
            {
                TurnTextBox.Text = "Black's Turn";
                turnType = 1;
            }
        }
    }
}