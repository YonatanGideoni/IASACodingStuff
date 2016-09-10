using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadSpiralNum
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] a = new int[5, 8];
            int up = 0, down = a.GetLength(0) - 1, left = 0, right = a.GetLength(1) - 1;

            for (int i = 0; i < a.GetLength(0) * a.GetLength(1); ) { 
                for (int k = left; k < right + 1 && i < a.GetLength(0) * a.GetLength(1); k++)
                {
                    a[up, k] = i;
                    i++;
                    Console.SetCursorPosition(3 * k, 3 * up);
                    Console.Write("{0}", a[up, k]);
                }
                up++;

                for (int k = up; k < down + 1 && i < a.GetLength(0) * a.GetLength(1); k++)
                {
                    a[k, right] = i;
                    i++;
                    Console.SetCursorPosition(3 * right, 3 * k);
                    Console.Write("{0}", a[k, right]);
                }
                right--;

                for (int k = right; k > left - 1 && i < a.GetLength(0) * a.GetLength(1); k--)
                {
                    a[down,k] = i;
                    i++;
                    Console.SetCursorPosition(3 * k, 3 * down);
                    Console.Write("{0}", a[down,k]);
                }
                down--;

                for (int k = down; k > up - 1 && i < a.GetLength(0) * a.GetLength(1); k--)
                {
                    a[k, left] = i;
                    i++;
                    Console.SetCursorPosition(3 * left, 3 * k);
                    Console.Write("{0}", a[k, left]);
                }
                left++;
            }
        }
    }
}
