using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SpiralNum
{
    class Program
    {
        static void Main(string[] args)
        {   
            int totalWidth = 7;
            int totalHeight = 10;
            int[,] a = new int[totalWidth, totalHeight];
            int[] dir = new int[4] {1, 0, -1, 0 };
            int x = 0;
            int y = 0;
            int XCorner = 0;
            int YCorner = 0;
            int CornerSum = 0;

            for (int i = 0; i < totalWidth * totalHeight; i++ )
            {
                XCorner = (i + totalHeight - XCorner) / (totalWidth + totalHeight - 1 - YCorner);
                YCorner = (i - YCorner) / (totalWidth + totalHeight - 1 - XCorner);
                CornerSum = XCorner + YCorner;
                int XdirNum = (CornerSum) % dir.Length;
                int YdirNum = (dir.Length - 1 + CornerSum) % dir.Length;
                a[x, y] = i;
                Console.SetCursorPosition(3 * x, 3 * y);
                Console.Write("{0}", a[x, y]);
                x += dir[XdirNum];
                y += dir[YdirNum];
            }
            Console.SetCursorPosition(3 * totalWidth, 3 * totalHeight);
            Thread.Sleep(99999999);
        }
    }
}
