using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHomework
{
    class Program
    {
        static void UpDownX(int num)
        {
            for (int i = 1; i < num + 1; i++) 
            {
                for (int k = 0; k < num - i; k++)
                {
                    Console.Write(" ");
                }
                for (int j = 0; j < i*2-1; j++)
                {
                    Console.Write("X");
                }
                Console.WriteLine();
            }
        }

        static void DownUpX(int num)
        {
            for (int i = 0; i < num; i++)
            {
                for (int k = 1; k < i+1; k++)
                {
                    Console.Write(" ");
                }
                for (int j = 0; j < 2*num - 2*i-1; j++) 
                {
                    Console.Write("X");
                }
                Console.WriteLine();
            }
        }

        static void RhombusX(int num)//num is the number of lines until the center of the Rhombus
        {
            UpDownX(num);
            for (int i = 0; i < num - 1; i++) 
            {
                for (int k = 0; k < i + 1; k++)
                {
                    Console.Write(" ");
                }
                for (int j = 0; j < 2 * num - 2 * i - 3; j++)
                {
                    Console.Write("X");
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            //UpDownX(10);
            //DownUpX(10);
            RhombusX(10);
        }
    }
}
