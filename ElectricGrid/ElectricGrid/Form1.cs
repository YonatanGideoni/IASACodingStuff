using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectricGrid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Button[,] gridArr;
        byte[,] gridByte;
        byte[] prevButton;

        private void InitGridButton_Click(object sender, EventArgs e)
        {
            byte gridSize = (byte)GridSizeNum.Value;
            byte buttonSize = 20;
            gridArr = new Button[gridSize, gridSize];
            gridByte = new byte[gridSize, gridSize];

            Panel grid = new Panel();
            grid.Size = new Size(gridSize * (buttonSize + 1), gridSize * (buttonSize + 1));
            this.Size = grid.Size;
            grid.Location = new Point(0, 0);
            grid.BackColor = Color.Yellow;

            for (byte i = 0; i < gridSize - 1; i++)
            {
                for (byte j = 0; j < gridSize - 1; j++)
                {
                    gridArr[i, j] = new Button();
                    gridByte[i, j] = 0;
                    gridArr[i, j].Size = new Size(buttonSize, buttonSize);
                    gridArr[i, j].Location = new Point(buttonSize * i, buttonSize * j);
                    gridArr[i, j].BackColor = Color.Black;
                    gridArr[i, j].Click += Form1_Click;
                    gridArr[i, j].Tag = new byte[2] { i, j };
                    grid.Controls.Add(gridArr[i, j]);
                }
            }

            this.Controls.Add(grid);

            gridByte[0, gridSize / 2 - 1] = 1;
            gridArr[0, gridSize / 2 - 1].BackColor = Color.Green;
            prevButton = new byte[2] { 0, (byte)(gridSize / 2 - 1) };

            GridSizeNum.Enabled = false;
            InitGridButton.Enabled = false;
            GridSizeText.Visible = false;
            GridSizeNum.Visible = false;
            InitGridButton.Visible = false;
        }

        void Form1_Click(object sender, EventArgs e)
        {
            byte gridSize = (byte)gridArr.GetLength(0);
            byte[] pressedButton = (byte[])(((Button)(sender)).Tag);
            if (prevButton == null)
            {
                prevButton = new byte[2] { pressedButton[0], pressedButton[1] };
            }
            else
            {
                if (pressedButton[0] - prevButton[0] == 0 || pressedButton[1] - prevButton[1] == 0)
                {
                    if (pressedButton[0] - prevButton[0] == 0)
                    {
                        for (byte i = Math.Min(pressedButton[1], prevButton[1]); i < 1 + Math.Max(pressedButton[1], prevButton[1]); i++)
                        {
                            gridArr[prevButton[0], i].BackColor = Color.Red;
                            gridByte[prevButton[0], i] = 2;
                        }
                    }
                    else
                    {
                        for (byte i = Math.Min(pressedButton[0], prevButton[0]); i < 1 + Math.Max(pressedButton[0], prevButton[0]); i++)
                        {
                            gridArr[i, prevButton[1]].BackColor = Color.Red;
                            gridByte[i, prevButton[1]] = 2;
                        }
                    }
                    prevButton = null;
                }
                else
                {
                    if (prevButton != new byte[2] { (byte)(gridSize / 2 - 1), 0 })
                    {
                        prevButton = null;
                    }
                }
            }
        }
    }
}