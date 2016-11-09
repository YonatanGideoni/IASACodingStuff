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

        static short BrdCheck(short[,] tempBrd)
        {
            short BrdPoints = 0;
            short BrdLength = (short)(tempBrd.GetLength(0) - 1);
            short BrdHeight = (short)(tempBrd.GetLength(1) - 1);

            for (short i = 1; i < BrdLength; i++)
            {
                for (short k = 1; k < BrdHeight; k++)
                {
                    if (i > 1 && i < BrdLength - 1 && k > 1 && k < BrdHeight - 1)
                    {
                        if (tempBrd[i, k] == 0)//skips uninhabited tiles. Same for all other checks
                        {
                            continue;
                        }
                        else if (tempBrd[i, k] == 2)//awards a point for a friendly non-frame tile
                        {
                            BrdPoints++;
                        }
                        else//takes a point for an enemy non-frame tile
                        {
                            BrdPoints--;
                        }
                    }
                    else if (i == 1 && k == 1)
                    {
                        if (tempBrd[i, k] == 0)//this check and the next three are the same, just for corners. Awards more points
                        {
                            continue;
                        }
                        else if (tempBrd[i, k] == 2)
                        {
                            BrdPoints += 25;
                        }
                        else
                        {
                            BrdPoints -= 25;
                        }
                    }
                    else if (i == BrdLength - 1 && k == BrdHeight - 1)
                    {
                        if (tempBrd[i, k] == 0)
                        {
                            continue;
                        }
                        else if (tempBrd[i, k] == 2)
                        {
                            BrdPoints += 25;
                        }
                        else
                        {
                            BrdPoints -= 25;
                        }
                    }
                    else if (i == 1 && k == BrdHeight - 1)
                    {
                        {
                            if (tempBrd[i, k] == 0)
                            {
                                continue;
                            }
                            else if (tempBrd[i, k] == 2)
                            {
                                BrdPoints += 25;
                            }
                            else
                            {
                                BrdPoints -= 25;
                            }
                        }
                    }
                    else if (i == BrdLength - 1 && k == 1)
                    {
                        {
                            if (tempBrd[i, k] == 0)
                            {
                                continue;
                            }
                            else if (tempBrd[i, k] == 2)
                            {
                                BrdPoints += 25;
                            }
                            else
                            {
                                BrdPoints -= 25;
                            }
                        }
                    }
                    else
                    {
                        if (tempBrd[i, k] == 0)//check for non corner non frame tiles
                        {
                            continue;
                        }
                        else if (tempBrd[i, k] == 2)
                        {
                            BrdPoints += 15;
                        }
                        else
                        {
                            BrdPoints -= 15;
                        }
                    }
                }
            }
            return BrdPoints;
        }

        public short[] MoveCheck(short[,] CurrentBrd)
        {
            short BrdLength = (short)(CurrentBrd.GetLength(0));
            short BrdHeight = (short)(CurrentBrd.GetLength(1));
            short[] BranchScore = new short[3] { -1000, 1000, -1000 };
            short[][,] tempBrd = new short[3][,];
            short[] ButtonLoc = new short[3];
            bool[] CanPlace = new bool[3];
            short[] row = new short[3];
            short[] col = new short[3];

            for (row[0] = 1; row[0] < BrdLength - 1; row[0]++)
            {
                for (col[0] = 1; col[0] < BrdLength - 1; col[0]++)
                {
                    tempBrd[0] = (short[,])CurrentBrd.Clone();//shallow clone of the array
                    CanPlace[0] = false;

                    if (tempBrd[0][row[0], col[0]] != 0)//Don't check buttons that have already been pressed
                    {
                        continue;
                    }

                    if (2 != tempBrd[0][row[0] + 1, col[0]])//checks if the adjacent tile in this direction is the same color
                    {
                        for (short j = 1; j < BrdLength - row[0] - 2; j++)
                        {
                            if (2 != tempBrd[0][row[0] + j, col[0]] && tempBrd[0][row[0] + j, col[0]] != 0)//checks that the next tile is an enemy tile
                            {
                                continue;
                            }
                            else if (tempBrd[0][row[0] + j, col[0]] == 0)//if it reaches an uninhabited tile, stop checking
                            {
                                break;
                            }
                            else//if after some enemy tiles it reaches a friendly tile, it can be placed.
                            {
                                CanPlace[0] = true;
                                break;
                            }
                        }
                    }//same logic for all the other checks, each is in a different direction

                    if (2 != tempBrd[0][row[0], col[0] + 1])
                    {
                        for (short j = 1; j < BrdHeight - col[0] - 2; j++)
                        {
                            if (2 != tempBrd[0][row[0], col[0] + j] && tempBrd[0][row[0], col[0] + j] != 0)
                            {
                                continue;
                            }
                            else if (tempBrd[0][row[0], col[0] + j] == 0)
                            {
                                break;
                            }
                            else
                            {
                                CanPlace[0] = true;
                                break;
                            }
                        }
                    }

                    if (2 != tempBrd[0][row[0], col[0] - 1])
                    {
                        for (short j = 1; j < col[0]; j++)
                        {
                            if (2 != tempBrd[0][row[0], col[0] - j] && tempBrd[0][row[0], col[0] - j] != 0)
                            {
                                continue;
                            }
                            else if (tempBrd[0][row[0], col[0] - j] == 0)
                            {
                                break;
                            }
                            else
                            {
                                CanPlace[0] = true;
                                break;
                            }
                        }
                    }

                    if (2 != tempBrd[0][row[0] - 1, col[0]])
                    {
                        for (short j = 1; j < row[0]; j++)
                        {
                            if (2 != tempBrd[0][row[0] - j, col[0]] && tempBrd[0][row[0] - j, col[0]] != 0)
                            {
                                continue;
                            }
                            else if (tempBrd[0][row[0] - j, col[0]] == 0)
                            {
                                break;
                            }
                            else
                            {
                                CanPlace[0] = true;
                                break;
                            }
                        }
                    }

                    if (2 != tempBrd[0][row[0] - 1, col[0] - 1])
                    {
                        for (short j = 1; j < row[0] && j < col[0]; j++)
                        {
                            if (2 != tempBrd[0][row[0] - j, col[0] - j] && tempBrd[0][row[0] - j, col[0] - j] != 0)
                            {
                                continue;
                            }
                            else if (tempBrd[0][row[0] - j, col[0] - j] == 0)
                            {
                                break;
                            }
                            else
                            {
                                CanPlace[0] = true;
                                break;
                            }
                        }
                    }

                    if (2 != tempBrd[0][row[0] + 1, col[0] - 1])
                    {
                        for (short j = 1; j < BrdLength - 1 - row[0] && j < col[0]; j++)
                        {
                            if (2 != tempBrd[0][row[0] + j, col[0] - j] && tempBrd[0][row[0] + j, col[0] - j] != 0)
                            {
                                continue;
                            }
                            else if (tempBrd[0][row[0] + j, col[0] - j] == 0)
                            {
                                break;
                            }
                            else
                            {
                                CanPlace[0] = true;
                                break;
                            }
                        }
                    }

                    if (2 != tempBrd[0][row[0] + 1, col[0] + 1])
                    {
                        for (short j = 1; j < BrdLength - 1 - row[0] && j < BrdHeight - 1 - col[0]; j++)
                        {
                            if (2 != tempBrd[0][row[0] + j, col[0] + j] && tempBrd[0][row[0] + j, col[0] + j] != 0)
                            {
                                continue;
                            }
                            else if (tempBrd[0][row[0] + j, col[0] + j] == 0)
                            {
                                break;
                            }
                            else
                            {
                                CanPlace[0] = true;
                                break;
                            }
                        }
                    }

                    if (2 != tempBrd[0][row[0] - 1, col[0] + 1])
                    {
                        for (short j = 1; j < row[0] && j < BrdHeight - 1 - col[0]; j++)
                        {
                            if (2 != tempBrd[0][row[0] - j, col[0] + j] && tempBrd[0][row[0] - j, col[0] + j] != 0)
                            {
                                continue;
                            }
                            else if (tempBrd[0][row[0] - j, col[0] + j] == 0)
                            {
                                break;
                            }
                            else
                            {
                                CanPlace[0] = true;
                                break;
                            }
                        }
                    }

                    if (CanPlace[0] == true)//simulates a move to the board
                    {
                        tempBrd[0][row[0], col[0]] = 2;

                        if (2 != tempBrd[0][row[0] + 1, col[0]])
                        {
                            for (short j = 1; j < BrdLength - row[0] - 2; j++)
                            {
                                if (2 != tempBrd[0][row[0] + j, col[0]] && tempBrd[0][row[0] + j, col[0]] != 0)
                                {
                                    continue;
                                }
                                else if (tempBrd[0][row[0] + j, col[0]] == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    for (short m = 1; m < j; m++)
                                    {
                                        tempBrd[0][row[0] + m, col[0]] = 2;
                                    }
                                    break;
                                }
                            }
                        }

                        if (2 != tempBrd[0][row[0], col[0] + 1])
                        {
                            for (short j = 1; j < BrdLength - col[0] - 2; j++)
                            {
                                if (2 != tempBrd[0][row[0], col[0] + j] && tempBrd[0][row[0], col[0] + j] != 0)
                                {
                                    continue;
                                }
                                else if (tempBrd[0][row[0], col[0] + j] == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    for (short m = 1; m < j; m++)
                                    {
                                        tempBrd[0][row[0], col[0] + m] = 2;
                                    }
                                    break;
                                }
                            }
                        }

                        if (2 != tempBrd[0][row[0], col[0] - 1])
                        {
                            for (short j = 1; j < col[0]; j++)
                            {
                                if (2 != tempBrd[0][row[0], col[0] - j] && tempBrd[0][row[0], col[0] - j] != 0)
                                {
                                    continue;
                                }
                                else if (tempBrd[0][row[0], col[0] - j] == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    for (short m = 1; m < j; m++)
                                    {
                                        tempBrd[0][row[0], col[0] - m] = 2;
                                    }
                                    break;
                                }
                            }
                        }

                        if (2 != tempBrd[0][row[0] - 1, col[0]])
                        {
                            for (short j = 1; j < row[0]; j++)
                            {
                                if (2 != tempBrd[0][row[0] - j, col[0]] && tempBrd[0][row[0] - j, col[0]] != 0)
                                {
                                    continue;
                                }
                                else if (tempBrd[0][row[0] - j, col[0]] == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    for (short m = 1; m < j; m++)
                                    {
                                        tempBrd[0][row[0] - m, col[0]] = 2;
                                    }
                                    break;
                                }
                            }
                        }

                        if (2 != tempBrd[0][row[0] - 1, col[0] - 1])
                        {
                            for (short j = 1; j < row[0] && j < col[0]; j++)
                            {
                                if (2 != tempBrd[0][row[0] - j, col[0] - j] && tempBrd[0][row[0] - j, col[0] - j] != 0)
                                {
                                    continue;
                                }
                                else if (tempBrd[0][row[0] - j, col[0] - j] == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    for (short m = 1; m < j; m++)
                                    {
                                        tempBrd[0][row[0] - m, col[0] - m] = 2;
                                    }
                                    break;
                                }
                            }
                        }

                        if (2 != tempBrd[0][row[0] + 1, col[0] - 1])
                        {
                            for (short j = 1; j < BrdLength - 1 - row[0] && j < col[0]; j++)
                            {
                                if (2 != tempBrd[0][row[0] + j, col[0] - j] && tempBrd[0][row[0] + j, col[0] - j] != 0)
                                {
                                    continue;
                                }
                                else if (tempBrd[0][row[0] + j, col[0] - j] == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    for (short m = 1; m < j; m++)
                                    {
                                        tempBrd[0][row[0] + m, col[0] - m] = 2;
                                    }
                                    break;
                                }
                            }
                        }

                        if (2 != tempBrd[0][row[0] + 1, col[0] + 1])
                        {
                            for (short j = 1; j < BrdLength - 1 - row[0] && j < BrdLength - 1 - col[0]; j++)
                            {
                                if (2 != tempBrd[0][row[0] + j, col[0] + j] && tempBrd[0][row[0] + j, col[0] + j] != 0)
                                {
                                    continue;
                                }
                                else if (tempBrd[0][row[0] + j, col[0] + j] == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    for (short m = 1; m < j; m++)
                                    {
                                        tempBrd[0][row[0] + m, col[0] + m] = 2;
                                    }
                                    break;
                                }
                            }
                        }

                        if (2 != tempBrd[0][row[0] - 1, col[0] + 1])
                        {
                            for (short j = 1; j < row[0] && j < BrdLength - 1 - col[0]; j++)
                            {
                                if (2 != tempBrd[0][row[0] - j, col[0] + j] && tempBrd[0][row[0] - j, col[0] + j] != 0)
                                {
                                    continue;
                                }
                                else if (tempBrd[0][row[0] - j, col[0] + j] == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    for (short m = 1; m < j; m++)
                                    {
                                        tempBrd[0][row[0] - m, col[0] + m] = 2;
                                    }
                                    break;
                                }
                            }
                        }

                        if (BlackTile + BlueTile > (BrdLength - 2) * (BrdHeight - 2) - 31)//if there is only one or two moves left, place the piece there if you can
                        {
                            ButtonLoc[0] = row[0];
                            ButtonLoc[1] = col[0];
                            return ButtonLoc;
                        }
                        else
                        {
                            BranchScore[1] = 1000;

                            for (row[1] = 1; row[1] < BrdLength - 1; row[1]++)
                            {
                                for (col[1] = 1; col[1] < BrdLength - 1; col[1]++)
                                {
                                    tempBrd[1] = (short[,])tempBrd[0].Clone();//shallow clone of the array which already had a move simulated
                                    CanPlace[1] = false;

                                    if (tempBrd[1][row[1], col[1]] != 0)//Same check and move function but for enemy tile
                                    {
                                        continue;
                                    }

                                    if (1 != tempBrd[1][row[1] + 1, col[1]])
                                    {
                                        for (short j = 1; j < BrdLength - row[1] - 2; j++)
                                        {
                                            if (1 != tempBrd[1][row[1] + j, col[1]] && tempBrd[1][row[1] + j, col[1]] != 0)
                                            {
                                                continue;
                                            }
                                            else if (tempBrd[1][row[1] + j, col[1]] == 0)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                CanPlace[1] = true;
                                                break;
                                            }
                                        }
                                    }//same logic for all the other checks, each is in a different direction

                                    if (1 != tempBrd[1][row[1], col[1] + 1])
                                    {
                                        for (short j = 1; j < BrdHeight - col[1] - 2; j++)
                                        {
                                            if (1 != tempBrd[1][row[1], col[1] + j] && tempBrd[1][row[1], col[1] + j] != 0)
                                            {
                                                continue;
                                            }
                                            else if (tempBrd[1][row[1], col[1] + j] == 0)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                CanPlace[1] = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (1 != tempBrd[1][row[1], col[1] - 1])
                                    {
                                        for (short j = 1; j < col[1]; j++)
                                        {
                                            if (1 != tempBrd[1][row[1], col[1] - j] && tempBrd[1][row[1], col[1] - j] != 0)
                                            {
                                                continue;
                                            }
                                            else if (tempBrd[1][row[1], col[1] - j] == 0)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                CanPlace[1] = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (1 != tempBrd[1][row[1] - 1, col[1]])
                                    {
                                        for (short j = 1; j < row[1]; j++)
                                        {
                                            if (1 != tempBrd[1][row[1] - j, col[1]] && tempBrd[1][row[1] - j, col[1]] != 0)
                                            {
                                                continue;
                                            }
                                            else if (tempBrd[1][row[1] - j, col[1]] == 0)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                CanPlace[1] = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (1 != tempBrd[1][row[1] - 1, col[1] - 1])
                                    {
                                        for (short j = 1; j < row[1] && j < col[1]; j++)
                                        {
                                            if (1 != tempBrd[1][row[1] - j, col[1] - j] && tempBrd[1][row[1] - j, col[1] - j] != 0)
                                            {
                                                continue;
                                            }
                                            else if (tempBrd[1][row[1] - j, col[1] - j] == 0)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                CanPlace[1] = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (1 != tempBrd[1][row[1] + 1, col[1] - 1])
                                    {
                                        for (short j = 1; j < BrdLength - 1 - row[1] && j < col[1]; j++)
                                        {
                                            if (1 != tempBrd[1][row[1] + j, col[1] - j] && tempBrd[1][row[1] + j, col[1] - j] != 0)
                                            {
                                                continue;
                                            }
                                            else if (tempBrd[1][row[1] + j, col[1] - j] == 0)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                CanPlace[1] = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (1 != tempBrd[1][row[1] + 1, col[1] + 1])
                                    {
                                        for (short j = 1; j < BrdLength - 1 - row[1] && j < BrdHeight - 1 - col[1]; j++)
                                        {
                                            if (1 != tempBrd[1][row[1] + j, col[1] + j] && tempBrd[1][row[1] + j, col[1] + j] != 0)
                                            {
                                                continue;
                                            }
                                            else if (tempBrd[1][row[1] + j, col[1] + j] == 0)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                CanPlace[1] = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (1 != tempBrd[1][row[1] - 1, col[1] + 1])
                                    {
                                        for (short j = 1; j < row[1] && j < BrdHeight - 1 - col[1]; j++)
                                        {
                                            if (1 != tempBrd[1][row[1] - j, col[1] + j] && tempBrd[1][row[1] - j, col[1] + j] != 0)
                                            {
                                                continue;
                                            }
                                            else if (tempBrd[1][row[1] - j, col[1] + j] == 0)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                CanPlace[1] = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (CanPlace[1] == true)//simulates a move to the board
                                    {
                                        tempBrd[1][row[1], col[1]] = 1;

                                        if (1 != tempBrd[1][row[1] + 1, col[1]])
                                        {
                                            for (short j = 1; j < BrdLength - row[1] - 2; j++)
                                            {
                                                if (1 != tempBrd[1][row[1] + j, col[1]] && tempBrd[1][row[1] + j, col[1]] != 0)
                                                {
                                                    continue;
                                                }
                                                else if (tempBrd[1][row[1] + j, col[1]] == 0)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    for (short m = 1; m < j; m++)
                                                    {
                                                        tempBrd[1][row[1] + m, col[1]] = 1;
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        if (1 != tempBrd[1][row[1], col[1] + 1])
                                        {
                                            for (short j = 1; j < BrdLength - col[1] - 2; j++)
                                            {
                                                if (1 != tempBrd[1][row[1], col[1] + j] && tempBrd[1][row[1], col[1] + j] != 0)
                                                {
                                                    continue;
                                                }
                                                else if (tempBrd[1][row[1], col[1] + j] == 0)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    for (short m = 1; m < j; m++)
                                                    {
                                                        tempBrd[1][row[1], col[1] + m] = 1;
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        if (1 != tempBrd[1][row[1], col[1] - 1])
                                        {
                                            for (short j = 1; j < col[1]; j++)
                                            {
                                                if (1 != tempBrd[1][row[1], col[1] - j] && tempBrd[1][row[1], col[1] - j] != 0)
                                                {
                                                    continue;
                                                }
                                                else if (tempBrd[1][row[1], col[1] - j] == 0)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    for (short m = 1; m < j; m++)
                                                    {
                                                        tempBrd[1][row[1], col[1] - m] = 1;
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        if (1 != tempBrd[1][row[1] - 1, col[1]])
                                        {
                                            for (short j = 1; j < row[1]; j++)
                                            {
                                                if (1 != tempBrd[1][row[1] - j, col[1]] && tempBrd[1][row[1] - j, col[1]] != 0)
                                                {
                                                    continue;
                                                }
                                                else if (tempBrd[1][row[1] - j, col[1]] == 0)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    for (short m = 1; m < j; m++)
                                                    {
                                                        tempBrd[1][row[1] - m, col[1]] = 1;
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        if (1 != tempBrd[1][row[1] - 1, col[1] - 1])
                                        {
                                            for (short j = 1; j < row[1] && j < col[1]; j++)
                                            {
                                                if (1 != tempBrd[1][row[1] - j, col[1] - j] && tempBrd[1][row[1] - j, col[1] - j] != 0)
                                                {
                                                    continue;
                                                }
                                                else if (tempBrd[1][row[1] - j, col[1] - j] == 0)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    for (short m = 1; m < j; m++)
                                                    {
                                                        tempBrd[1][row[1] - m, col[1] - m] = 1;
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        if (1 != tempBrd[1][row[1] + 1, col[1] - 1])
                                        {
                                            for (short j = 1; j < BrdLength - 1 - row[1] && j < col[1]; j++)
                                            {
                                                if (1 != tempBrd[1][row[1] + j, col[1] - j] && tempBrd[1][row[1] + j, col[1] - j] != 0)
                                                {
                                                    continue;
                                                }
                                                else if (tempBrd[1][row[1] + j, col[1] - j] == 0)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    for (short m = 1; m < j; m++)
                                                    {
                                                        tempBrd[1][row[1] + m, col[1] - m] = 1;
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        if (1 != tempBrd[1][row[1] + 1, col[1] + 1])
                                        {
                                            for (short j = 1; j < BrdLength - 1 - row[1] && j < BrdLength - 1 - col[1]; j++)
                                            {
                                                if (1 != tempBrd[1][row[1] + j, col[1] + j] && tempBrd[1][row[1] + j, col[1] + j] != 0)
                                                {
                                                    continue;
                                                }
                                                else if (tempBrd[1][row[1] + j, col[1] + j] == 0)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    for (short m = 1; m < j; m++)
                                                    {
                                                        tempBrd[1][row[1] + m, col[1] + m] = 1;
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        if (1 != tempBrd[1][row[1] - 1, col[1] + 1])
                                        {
                                            for (short j = 1; j < row[1] && j < BrdLength - 1 - col[1]; j++)
                                            {
                                                if (1 != tempBrd[1][row[1] - j, col[1] + j] && tempBrd[1][row[1] - j, col[1] + j] != 0)
                                                {
                                                    continue;
                                                }
                                                else if (tempBrd[1][row[1] - j, col[1] + j] == 0)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    for (short m = 1; m < j; m++)
                                                    {
                                                        tempBrd[1][row[1] - m, col[1] + m] = 1;
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        BranchScore[2] = -1000;
                                        for (row[2] = 1; row[2] < BrdLength - 1; row[2]++)
                                        {
                                            for (col[2] = 1; col[2] < BrdLength - 1; col[2]++)
                                            {
                                                tempBrd[2] = (short[,])tempBrd[1].Clone();//shallow clone of the array
                                                CanPlace[2] = false;

                                                if (tempBrd[2][row[2], col[2]] != 0)//Don't check buttons that have already been pressed
                                                {
                                                    continue;
                                                }

                                                if (2 != tempBrd[2][row[2] + 1, col[2]])//same kind of move check
                                                {
                                                    for (short j = 1; j < BrdLength - row[2] - 2; j++)
                                                    {
                                                        if (2 != tempBrd[2][row[2] + j, col[2]] && tempBrd[2][row[2] + j, col[2]] != 0)
                                                        {
                                                            continue;
                                                        }
                                                        else if (tempBrd[2][row[2] + j, col[2]] == 0)
                                                        {
                                                            break;
                                                        }
                                                        else//if after some enemy tiles it reaches a friendly tile, it can be placed.
                                                        {
                                                            CanPlace[2] = true;
                                                            break;
                                                        }
                                                    }
                                                }//same logic for all the other checks, each is in a different direction

                                                if (2 != tempBrd[2][row[2], col[2] + 1])
                                                {
                                                    for (short j = 1; j < BrdHeight - col[2] - 2; j++)
                                                    {
                                                        if (2 != tempBrd[2][row[2], col[2] + j] && tempBrd[2][row[2], col[2] + j] != 0)
                                                        {
                                                            continue;
                                                        }
                                                        else if (tempBrd[2][row[2], col[2] + j] == 0)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            CanPlace[2] = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (2 != tempBrd[2][row[2], col[2] - 1])
                                                {
                                                    for (short j = 1; j < col[2]; j++)
                                                    {
                                                        if (2 != tempBrd[2][row[2], col[2] - j] && tempBrd[2][row[2], col[2] - j] != 0)
                                                        {
                                                            continue;
                                                        }
                                                        else if (tempBrd[2][row[2], col[2] - j] == 0)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            CanPlace[2] = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (2 != tempBrd[2][row[2] - 1, col[2]])
                                                {
                                                    for (short j = 1; j < row[2]; j++)
                                                    {
                                                        if (2 != tempBrd[2][row[2] - j, col[2]] && tempBrd[2][row[2] - j, col[2]] != 0)
                                                        {
                                                            continue;
                                                        }
                                                        else if (tempBrd[2][row[2] - j, col[2]] == 0)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            CanPlace[2] = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (2 != tempBrd[2][row[2] - 1, col[2] - 1])
                                                {
                                                    for (short j = 1; j < row[2] && j < col[2]; j++)
                                                    {
                                                        if (2 != tempBrd[2][row[2] - j, col[2] - j] && tempBrd[2][row[2] - j, col[2] - j] != 0)
                                                        {
                                                            continue;
                                                        }
                                                        else if (tempBrd[2][row[2] - j, col[2] - j] == 0)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            CanPlace[2] = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (2 != tempBrd[2][row[2] + 1, col[2] - 1])
                                                {
                                                    for (short j = 1; j < BrdLength - 1 - row[2] && j < col[2]; j++)
                                                    {
                                                        if (2 != tempBrd[2][row[2] + j, col[2] - j] && tempBrd[2][row[2] + j, col[2] - j] != 0)
                                                        {
                                                            continue;
                                                        }
                                                        else if (tempBrd[2][row[2] + j, col[2] - j] == 0)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            CanPlace[2] = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (2 != tempBrd[2][row[2] + 1, col[2] + 1])
                                                {
                                                    for (short j = 1; j < BrdLength - 1 - row[2] && j < BrdHeight - 1 - col[2]; j++)
                                                    {
                                                        if (2 != tempBrd[2][row[2] + j, col[2] + j] && tempBrd[2][row[2] + j, col[2] + j] != 0)
                                                        {
                                                            continue;
                                                        }
                                                        else if (tempBrd[2][row[2] + j, col[2] + j] == 0)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            CanPlace[2] = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (2 != tempBrd[2][row[2] - 1, col[2] + 1])
                                                {
                                                    for (short j = 1; j < row[2] && j < BrdHeight - 1 - col[2]; j++)
                                                    {
                                                        if (2 != tempBrd[2][row[2] - j, col[2] + j] && tempBrd[2][row[2] - j, col[2] + j] != 0)
                                                        {
                                                            continue;
                                                        }
                                                        else if (tempBrd[2][row[2] - j, col[2] + j] == 0)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            CanPlace[2] = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (CanPlace[2] == true)//simulates a move to the board
                                                {
                                                    tempBrd[2][row[2], col[2]] = 2;

                                                    if (2 != tempBrd[2][row[2] + 1, col[2]])
                                                    {
                                                        for (short j = 1; j < BrdLength - row[2] - 2; j++)
                                                        {
                                                            if (2 != tempBrd[2][row[2] + j, col[2]] && tempBrd[2][row[2] + j, col[2]] != 0)
                                                            {
                                                                continue;
                                                            }
                                                            else if (tempBrd[2][row[2] + j, col[2]] == 0)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                for (short m = 1; m < j; m++)
                                                                {
                                                                    tempBrd[2][row[2] + m, col[2]] = 2;
                                                                }
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (2 != tempBrd[2][row[2], col[2] + 1])
                                                    {
                                                        for (short j = 1; j < BrdLength - col[2] - 2; j++)
                                                        {
                                                            if (2 != tempBrd[2][row[2], col[2] + j] && tempBrd[2][row[2], col[2] + j] != 0)
                                                            {
                                                                continue;
                                                            }
                                                            else if (tempBrd[2][row[2], col[2] + j] == 0)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                for (short m = 1; m < j; m++)
                                                                {
                                                                    tempBrd[2][row[2], col[2] + m] = 2;
                                                                }
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (2 != tempBrd[2][row[2], col[2] - 1])
                                                    {
                                                        for (short j = 1; j < col[2]; j++)
                                                        {
                                                            if (2 != tempBrd[2][row[2], col[2] - j] && tempBrd[2][row[2], col[2] - j] != 0)
                                                            {
                                                                continue;
                                                            }
                                                            else if (tempBrd[2][row[2], col[2] - j] == 0)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                for (short m = 1; m < j; m++)
                                                                {
                                                                    tempBrd[2][row[2], col[2] - m] = 2;
                                                                }
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (2 != tempBrd[2][row[2] - 1, col[2]])
                                                    {
                                                        for (short j = 1; j < row[2]; j++)
                                                        {
                                                            if (2 != tempBrd[2][row[2] - j, col[2]] && tempBrd[2][row[2] - j, col[2]] != 0)
                                                            {
                                                                continue;
                                                            }
                                                            else if (tempBrd[2][row[2] - j, col[2]] == 0)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                for (short m = 1; m < j; m++)
                                                                {
                                                                    tempBrd[2][row[2] - m, col[2]] = 2;
                                                                }
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (2 != tempBrd[2][row[2] - 1, col[2] - 1])
                                                    {
                                                        for (short j = 1; j < row[2] && j < col[2]; j++)
                                                        {
                                                            if (2 != tempBrd[2][row[2] - j, col[2] - j] && tempBrd[2][row[2] - j, col[2] - j] != 0)
                                                            {
                                                                continue;
                                                            }
                                                            else if (tempBrd[2][row[2] - j, col[2] - j] == 0)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                for (short m = 1; m < j; m++)
                                                                {
                                                                    tempBrd[2][row[2] - m, col[2] - m] = 2;
                                                                }
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (2 != tempBrd[2][row[2] + 1, col[2] - 1])
                                                    {
                                                        for (short j = 1; j < BrdLength - 1 - row[2] && j < col[2]; j++)
                                                        {
                                                            if (2 != tempBrd[2][row[2] + j, col[2] - j] && tempBrd[2][row[2] + j, col[2] - j] != 0)
                                                            {
                                                                continue;
                                                            }
                                                            else if (tempBrd[2][row[2] + j, col[2] - j] == 0)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                for (short m = 1; m < j; m++)
                                                                {
                                                                    tempBrd[2][row[2] + m, col[2] - m] = 2;
                                                                }
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (2 != tempBrd[2][row[2] + 1, col[2] + 1])
                                                    {
                                                        for (short j = 1; j < BrdLength - 1 - row[2] && j < BrdLength - 1 - col[2]; j++)
                                                        {
                                                            if (2 != tempBrd[2][row[2] + j, col[2] + j] && tempBrd[2][row[2] + j, col[2] + j] != 0)
                                                            {
                                                                continue;
                                                            }
                                                            else if (tempBrd[2][row[2] + j, col[2] + j] == 0)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                for (short m = 1; m < j; m++)
                                                                {
                                                                    tempBrd[2][row[2] + m, col[2] + m] = 2;
                                                                }
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (2 != tempBrd[2][row[2] - 1, col[2] + 1])
                                                    {
                                                        for (short j = 1; j < row[2] && j < BrdLength - 1 - col[2]; j++)
                                                        {
                                                            if (2 != tempBrd[2][row[2] - j, col[2] + j] && tempBrd[2][row[2] - j, col[2] + j] != 0)
                                                            {
                                                                continue;
                                                            }
                                                            else if (tempBrd[2][row[2] - j, col[2] + j] == 0)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                for (short m = 1; m < j; m++)
                                                                {
                                                                    tempBrd[2][row[2] - m, col[2] + m] = 2;
                                                                }
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (BranchScore[2] < BrdCheck(tempBrd[2]))//remember if this is the best possible move for this branch
                                                    {
                                                        BranchScore[2] = BrdCheck(tempBrd[2]);
                                                    }
                                                }
                                            }
                                        }

                                        if (BranchScore[1] > BranchScore[2])//remember if this is the worst possible player move for this branch
                                        {
                                            BranchScore[1] = BranchScore[2];
                                        }
                                    }
                                }
                            }
                            if (BranchScore[0] < BranchScore[1])//if this is the best possible branch, use it
                            {
                                BranchScore[0] = BranchScore[1];
                                ButtonLoc[0] = row[0];
                                ButtonLoc[1] = col[0];
                            }
                        }
                    }
                }
            }
            return ButtonLoc;
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            short GridLength = Convert.ToInt16(GridSizeBox.Value + 2);
            short ButtonSize = 35;

            Board = new Button[GridLength, GridLength];//sets the int and Button array sizes
            intBrd = new short[GridLength, GridLength];

            BoardPanel.Size = new Size(GridLength * ButtonSize, GridLength * ButtonSize);//sets the panel and window sizes
            this.Size = new Size(Math.Max(BoardPanel.Width + BoardPanel.Location.X + 50, 400), BoardPanel.Height + BoardPanel.Location.Y + 100);

            for (short i = 0; i < GridLength; i++)
            {
                for (short k = 0; k < GridLength; k++)//assigns the Buttons values and adds them to the panel
                {
                    Board[i, k] = new Button();
                    Board[i, k].Size = new Size(ButtonSize, ButtonSize);
                    Board[i, k].Location = new Point(ButtonSize * i, ButtonSize * k);
                    intBrd[i, k] = 0;
                    Board[i, k].BackColor = default(Color);
                    Board[i, k].Tag = new short[2] { i, k };

                    BoardPanel.Controls.Add(Board[i, k]);
                    Board[i, k].Click += Form1_Click;

                    if (i == 0 || i == GridLength - 1 || k == 0 || k == GridLength - 1)//if it is a frame button, make it invisible and inactive
                    {
                        Board[i, k].Enabled = false;
                        Board[i, k].Visible = false;
                    }
                }
            }

            Board[GridLength / 2, GridLength / 2].BackColor = Color.Black;//create the center starting buttons
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

            turnType = 1;//black starts
            TurnTextBox.Text = "Black's Turn";//inputs the necessary text in the textboxes
            TileCountBox.Text = "Black Tiles:" + BlackTile.ToString() + " Blue Tiles:" + BlueTile.ToString();
        }

        void Form1_Click(object sender, EventArgs e)
        {
            short[] ButtonLoc = (short[])((Array)(((Button)(sender)).Tag));//retrieves the pressed button's location
            short row = ButtonLoc[0];
            short col = ButtonLoc[1];
            bool CanPlace = false;
            short BrdLength = (short)Board.GetLength(0);
            short BrdHeight = (short)Board.GetLength(1);

            if (turnType != intBrd[row + 1, col])
            {
                for (short i = 1; i < BrdLength - row - 2; i++)
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
                for (short i = 1; i < BrdHeight - col - 2; i++)
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
                for (short i = 1; i < BrdLength - 1 - row && i < col; i++)
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
                for (short i = 1; i < BrdLength - 1 - row && i < BrdHeight - 1 - col; i++)
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
                for (short i = 1; i < row && i < BrdHeight - 1 - col; i++)
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

            if (turnType == 1 && CanPlace == true)//changes the button you pressed if it is a legitimate tile
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

            if (BlackTile + BlueTile == (BrdLength - 2) * (BrdHeight - 2))//Win condition
            {
                if (BlackTile > BlueTile)
                {
                    MessageBox.Show("Black Wins!");
                }
                else if (BlackTile == BlueTile)
                {
                    MessageBox.Show("It's a Tie!");
                }
                else
                {
                    MessageBox.Show("Blue Wins!");
                }
            }
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            if (turnType == 1)//skip turn function
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

        private void CompTurn_Click(object sender, EventArgs e)
        {
            if (turnType == 2)//Computer turn function. The computer is always blue
            {
                short[] ButtonLoc = MoveCheck(intBrd);//finds the optimal button to press

                short BrdLength = (short)Board.GetLength(0);
                short BrdHeight = (short)Board.GetLength(1);

                short row = ButtonLoc[0];//retrieves button location
                short col = ButtonLoc[1];

                if (row == 0 || col == 0)
                {
                    TurnTextBox.Text = "Black's Turn";
                    turnType = 1;
                    MessageBox.Show("The turn has been skipped, as there are no more possible moves");
                }
                else
                {

                    if (2 != intBrd[row + 1, col])//same as the player move function, adjusted for only blue (computer) tiles
                    {
                        for (short i = 1; i < BrdLength - row - 2; i++)
                        {
                            if (2 != intBrd[row + i, col] && intBrd[row + i, col] != 0)
                            {
                                continue;
                            }
                            else if (intBrd[row + i, col] == 0)
                            {
                                break;
                            }
                            else
                            {
                                for (short k = 1; k < i; k++)
                                {
                                    intBrd[row + k, col] = 2;
                                    Board[row + k, col].BackColor = Color.Blue;
                                    Board[row + k, col].Enabled = false;
                                    BlueTile++;
                                    BlackTile--;
                                }

                                break;
                            }
                        }
                    }

                    if (2 != intBrd[row, col + 1])
                    {
                        for (short i = 1; i < BrdHeight - col - 2; i++)
                        {
                            if (2 != intBrd[row, col + i] && intBrd[row, col + i] != 0)
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
                                    intBrd[row, col + k] = 2;
                                    Board[row, col + k].BackColor = Color.Blue;
                                    Board[row, col + k].Enabled = false;
                                    BlueTile++;
                                    BlackTile--;
                                }

                                break;
                            }
                        }
                    }

                    if (2 != intBrd[row, col - 1])
                    {
                        for (short i = 1; i < col; i++)
                        {
                            if (2 != intBrd[row, col - i] && intBrd[row, col - i] != 0)
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
                                    intBrd[row, col - k] = 2;
                                    Board[row, col - k].BackColor = Color.Blue;
                                    Board[row, col - k].Enabled = false;
                                    BlueTile++;
                                    BlackTile--;
                                }

                                break;
                            }
                        }
                    }

                    if (2 != intBrd[row - 1, col])
                    {
                        for (short i = 1; i < row; i++)
                        {
                            if (2 != intBrd[row - i, col] && intBrd[row - i, col] != 0)
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
                                    intBrd[row - k, col] = 2;
                                    Board[row - k, col].BackColor = Color.Blue;
                                    Board[row - k, col].Enabled = false;
                                    BlueTile++;
                                    BlackTile--;
                                }
                                break;
                            }
                        }
                    }

                    if (2 != intBrd[row - 1, col - 1])
                    {
                        for (short i = 1; i < row && i < col; i++)
                        {
                            if (2 != intBrd[row - i, col - i] && intBrd[row - i, col - i] != 0)
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
                                    intBrd[row - k, col - k] = 2;
                                    Board[row - k, col - k].BackColor = Color.Blue;
                                    Board[row - k, col - k].Enabled = false;
                                    BlueTile++;
                                    BlackTile--;
                                }
                                break;
                            }
                        }
                    }

                    if (2 != intBrd[row + 1, col - 1])
                    {
                        for (short i = 1; i < BrdLength - 1 - row && i < col; i++)
                        {
                            if (2 != intBrd[row + i, col - i] && intBrd[row + i, col - i] != 0)
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
                                    intBrd[row + k, col - k] = 2;
                                    Board[row + k, col - k].BackColor = Color.Blue;
                                    Board[row + k, col - k].Enabled = false;
                                    BlueTile++;
                                    BlackTile--;
                                }
                                break;
                            }
                        }
                    }

                    if (2 != intBrd[row + 1, col + 1])
                    {
                        for (short i = 1; i < BrdLength - 1 - row && i < BrdHeight - 1 - col; i++)
                        {
                            if (2 != intBrd[row + i, col + i] && intBrd[row + i, col + i] != 0)
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
                                    intBrd[row + k, col + k] = 2;
                                    Board[row + k, col + k].BackColor = Color.Blue;
                                    Board[row + k, col + k].Enabled = false;
                                    BlueTile++;
                                    BlackTile--;
                                }
                                break;
                            }
                        }
                    }

                    if (2 != intBrd[row - 1, col + 1])
                    {
                        for (short i = 1; i < row && i < BrdHeight - 1 - col; i++)
                        {
                            if (2 != intBrd[row - i, col + i] && intBrd[row - i, col + i] != 0)
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
                                    intBrd[row - k, col + k] = 2;
                                    Board[row - k, col + k].BackColor = Color.Blue;
                                    Board[row - k, col + k].Enabled = false;
                                    BlueTile++;
                                    BlackTile--;
                                }
                                break;
                            }
                        }
                    }

                    BlueTile++;//gives origin button correct values and changes turn
                    TurnTextBox.Text = "Black's Turn";
                    TileCountBox.Text = "Black Tiles:" + BlackTile.ToString() + " Blue Tiles:" + BlueTile.ToString();
                    Board[row, col].BackColor = Color.Blue;
                    Board[row, col].Enabled = false;
                    intBrd[row, col] = 2;

                    if (BlackTile + BlueTile == (BrdLength - 2) * (BrdHeight - 2))//Win condition
                    {
                        if (BlackTile > BlueTile)
                        {
                            MessageBox.Show("Black Wins!");
                        }
                        else if (BlackTile == BlueTile)
                        {
                            MessageBox.Show("It's a Tie!");
                        }
                        else
                        {
                            MessageBox.Show("Blue Wins!");
                        }
                    }
                    turnType = 1;
                }
            }
            else
            {
                MessageBox.Show("This is currently your turn");
            }
        }
    }
}