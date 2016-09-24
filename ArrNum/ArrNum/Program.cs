using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrNum
{
    class Program
    {
        static int ReverNum(int num)
        {
            int tempNum = num;
            int returnNum = 0;
            while (tempNum > 0)
            {
                returnNum = returnNum * 10 + tempNum % 10;
                tempNum = tempNum / 10;
            }
            return returnNum;
        }
        static void NumShift(int[] arr)
        {
            int firstDigit = arr[0];
            while (firstDigit > 9)
            {
                firstDigit /= 10;
            }
            arr[arr.Length - 1] = ReverNum(ReverNum(arr[arr.Length - 1]) / 10) * 10 + firstDigit;
            for(int i=0;i<arr.Length-1;i++)
            {
                int newNum = arr[i + 1];
                while (newNum > 9)
                {
                    newNum /= 10;
                }
                arr[i] = ReverNum(ReverNum(arr[i]) / 10)*10 + newNum;
            }
        }

        static void Show(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(" " + arr[i]);
            }
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            int[] testArr = new int[4] { 923, 456, 789, 101 };
            Show(testArr);
            NumShift(testArr);
            Show(testArr);
        }
    }
}
