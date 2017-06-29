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
        bool powerActive = false;

        private void InitGridButton_Click(object sender, EventArgs e)
        {
            byte gridSize = (byte)GridSizeNum.Value;
            byte buttonSize = 30;
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
                    gridArr[i, j].MouseDown += Form1_MouseDown;
                    gridArr[i, j].Tag = new byte[2] { i, j };
                    gridArr[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                    gridArr[i, j].FlatAppearance.BorderSize = 0;
                    grid.Controls.Add(gridArr[i, j]);
                }
            }

            this.Controls.Add(grid);

            gridByte[0, gridSize / 2 - 1] = 0;
            gridArr[0, gridSize / 2 - 1].BackColor = Color.Green;
            prevButton = new byte[2] { 0, (byte)(gridSize / 2 - 1) };

            GridSizeNum.Enabled = false;
            InitGridButton.Enabled = false;
            GridSizeText.Visible = false;
            GridSizeNum.Visible = false;
            InitGridButton.Visible = false;
        }

        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            byte gridSize = (byte)gridArr.GetLength(0);
            byte[] pressedButton = (byte[])(((Button)(sender)).Tag);
            if (!powerActive)
            {
                if (e.Button == MouseButtons.Left)
                {
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
                                    gridArr[prevButton[0], i].BackgroundImage = Properties.Resources.VerticalWire;
                                    gridByte[prevButton[0], i] = 2;
                                }
                            }
                            else
                            {
                                for (byte i = Math.Min(pressedButton[0], prevButton[0]); i < 1 + Math.Max(pressedButton[0], prevButton[0]); i++)
                                {
                                    gridArr[i, prevButton[1]].BackgroundImage = Properties.Resources.HorizontalWire;
                                    gridByte[i, prevButton[1]] = 3;
                                }
                            }

                            if (isIntersection(prevButton))
                            {
                                gridArr[prevButton[0], prevButton[1]].BackgroundImage = Properties.Resources._Intersection;
                            }
                            if (isIntersection(pressedButton))
                            {
                                gridArr[pressedButton[0], pressedButton[1]].BackgroundImage = Properties.Resources._Intersection;
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
                else if (e.Button == MouseButtons.Right)
                {
                    switch (gridByte[pressedButton[0], pressedButton[1]])
                    {
                        case 3:
                            gridArr[pressedButton[0], pressedButton[1]].BackgroundImage = Properties.Resources.HorizontalResistor1;
                            gridByte[pressedButton[0], pressedButton[1]] = 1;
                            break;
                        case 1:
                            gridArr[pressedButton[0], pressedButton[1]].BackgroundImage = Properties.Resources.HorizontalResistor5;
                            gridByte[pressedButton[0], pressedButton[1]] = 5;
                            break;
                        case 5:
                            gridArr[pressedButton[0], pressedButton[1]].BackgroundImage = Properties.Resources.HorizontalResistor10;
                            gridByte[pressedButton[0], pressedButton[1]] = 10;
                            break;
                        case 10:
                            gridArr[pressedButton[0], pressedButton[1]].BackgroundImage = Properties.Resources.HorizontalWire;
                            gridByte[pressedButton[0], pressedButton[1]] = 3;
                            break;
                    }

                }
            }
        }

        public bool isIntersection(byte[] buttonLoc)
        {
            byte gridSize = (byte)gridArr.GetLength(0);
            bool verticalConnect = false;
            bool horizontalConnect = false;
            if (buttonLoc[0] != 0)
            {
                if (gridByte[buttonLoc[0] - 1, buttonLoc[1]] == 3)
                {
                    verticalConnect = true;
                }
            }
            if (buttonLoc[0] != gridSize - 1)
            {
                if (gridByte[buttonLoc[0] + 1, buttonLoc[1]] == 3)
                {
                    verticalConnect = true;
                }
            }
            if (buttonLoc[1] != 0)
            {
                if (gridByte[buttonLoc[0], buttonLoc[1] - 1] == 2)
                {
                    horizontalConnect = true;
                }
            }
            if (buttonLoc[1] != gridSize - 1)
            {
                if (gridByte[buttonLoc[0], buttonLoc[1] + 1] == 2)
                {
                    horizontalConnect = true;
                }
            }

            if (horizontalConnect && verticalConnect)
            {
                gridByte[buttonLoc[0], buttonLoc[1]] = 4;
                return true;
            }
            return false;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == Keys.P.ToString())
            {
                powerActive = true;
            }
        }
    }
}