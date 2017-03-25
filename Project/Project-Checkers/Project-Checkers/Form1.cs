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

        public Form1()
        {
            InitializeComponent();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            BrdSize = (short)(BrdSizeBox.Value);
            Board = new Button[BrdSize,BrdSize];
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
                    intBrd[i, k] = 0;
                    Board[i, k].Tag = new short[4] { i, k, 0, 0};

                    Board[i, k].Click += Form1_Click;

                    if ((i + k) % 2 == 0)//sets the buttons color and type
                    {
                        if (k < 3)
                        {
                            Board[i, k].BackColor = Color.DarkOrchid;
                            BlackCheckers++;
                        }
                        else if (k > BrdSize - 4)
                        {
                            Board[i, k].BackColor = Color.SandyBrown;
                            WhiteCheckers++;
                        }
                        else
                        {
                            Board[i, k].BackColor = Color.Black;
                        }
                    }
                    else
                    {
                        Board[i, k].BackColor = Color.White;
                    }

                    panel1.Controls.Add(Board[i,k]);
                }
            }
        }

        void Form1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
