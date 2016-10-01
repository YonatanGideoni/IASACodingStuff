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

        public Form1()
        {
            InitializeComponent();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            short GridLength = Convert.ToInt16(GridSizeBox.Value + 2);
            short ButtonSize = 20;

            Board = new Button[GridLength, GridLength];
            intBrd = new short[GridLength, GridLength];

            BoardPanel.Size = new Size(GridLength * ButtonSize, GridLength * ButtonSize);
            this.Size = new Size(400, BoardPanel.Height + BoardPanel.Location.Y + 50);

            for (int i = 0; i < GridLength; i++)
            {
                for (int k = 0; k < GridLength; k++)
                {
                    Board[i, k] = new Button();
                    Board[i, k].Size = new Size(ButtonSize, ButtonSize);
                    Board[i, k].Location = new Point(ButtonSize * i, ButtonSize * k);
                    intBrd[i, k] = 0;
                    Board[i, k].BackColor = default(Color);
                    Board[i, k].Tag = i.ToString() + k.ToString();

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
            Board[GridLength / 2 - 1, GridLength / 2].BackColor = Color.White;
            Board[GridLength / 2, GridLength / 2 - 1].BackColor = Color.White;
            Board[GridLength / 2 - 1, GridLength / 2 - 1].BackColor = Color.Black;

            intBrd[GridLength / 2, GridLength / 2] = 1;
            intBrd[GridLength / 2 - 1, GridLength / 2] = 2;
            intBrd[GridLength / 2, GridLength / 2 - 1] = 2;
            intBrd[GridLength / 2 - 1, GridLength / 2 - 1] = 1;
        }

        public bool CanPlace(short type, short row, short col)
        {

            for (short i = 1; i < Board.GetLength(0) - row - 2; i++)
            {
                if (type != intBrd[row + i, col] && intBrd[row + i, col] != 0)//check if a touching tile is an enemy tile
                {
                    continue;
                }
                else if (intBrd[row + i, col] == 0) //if it is an uninhabited tile, stop checking
                {
                    break;
                }
                else //if the next tile over is a friendly tile, a friendly tile may be placed  
                {
                    return true;
                }
            }
            // Same deal for all the other loops. Each checks a different direction
            for (short i = 0; i < Board.GetLength(0) - col - 2; i++)
            {
                if (type != intBrd[row, col + i] && intBrd[row, col + i] != 0)
                {
                    continue;
                }
                else if (intBrd[row, col + i] == 0)
                {
                    break;
                }
                else
                {
                    return true;
                }
            }
            for (short i = 0; i < col; i++)
            {
                if (type != intBrd[row, col - i] && intBrd[row, col - i] != 0)
                {
                    continue;
                }
                else if (intBrd[row, col - i] == 0)
                {
                    break;
                }
                else
                {
                    return true;
                }
            }
            for (short i = 0; i < row; i++)
            {
                if (type != intBrd[row - i, col] && intBrd[row - i, col] != 0)
                {
                    continue;
                }
                else if (intBrd[row - i, col] == 0)
                {
                    break;
                }
                else
                {
                    return true;
                }
            }
            for (short i = 0; i < row && i < col; i++)
            {
                if (type != intBrd[row - i, col - i] && intBrd[row - i, col - i] != 0)
                {
                    continue;
                }
                else if (intBrd[row - i, col - i] == 0)
                {
                    break;
                }
                else
                {
                    return true;
                }
            }
            for (short i = 0; i < Board.GetLength(0) - 2 - row && i < col; i++)
            {
                if (type != intBrd[row + i, col - i] && intBrd[row + i, col - i] != 0)
                {
                    continue;
                }
                else if (intBrd[row + i, col - i] == 0)
                {
                    break;
                }
                else
                {
                    return true;
                }
            }
            for (short i = 0; i < Board.GetLength(0) - 2 - row && i < Board.GetLength(0) - 2 - col; i++)
            {
                if (type != intBrd[row + i, col + i] && intBrd[row + i, col + i] != 0)
                {
                    continue;
                }
                else if (intBrd[row + i, col + i] == 0)
                {
                    break;
                }
                else
                {
                    return true;
                }
            }
            for (short i = 0; i < row && i < Board.GetLength(0) - 2 - col; i++)
            {
                if (type != intBrd[row - i, col + i] && intBrd[row - i, col + i] != 0)
                {
                    continue;
                }
                else if (intBrd[row - i, col + i] == 0)
                {
                    break;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        void Form1_Click(object sender, EventArgs e)
        {
            short ButtonLoc = short.Parse(((Button)(sender)).Tag.ToString());
            short GridLength = Convert.ToInt16(GridSizeBox.Value);
            short row = (short)(ButtonLoc / GridLength);
            short col = (short)(ButtonLoc % GridLength);

            if (CanPlace(1, row, col) == true)
            {
                MessageBox.Show("Can Place!");
            }
            else
            {
                MessageBox.Show("Can't Place Here :(");
            }
        }
    }
}
