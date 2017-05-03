using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grapher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static string printTree(ParseTree<string> node)
        {
            if (node.left != null && node.right != null)
            {
                return node.operation + Environment.NewLine + printTree(node.left) + " " + printTree(node.right);
            }
            else
            {
                if (node.left != null)
                {
                    return node.operation + Environment.NewLine + printTree(node.left);
                }

                if (node.right != null)
                {
                    return node.operation + Environment.NewLine + printTree(node.right);
                }

                return node.operation;
            }
        }

        static float calcTree(ParseTree<string> node)
        {
            if (node == null)
            {
                return 0;
            }
            if (node.operation.Length == 1 && baseOperand(node.operation))
            {
                if (node.operation == "+")
                {
                    return calcTree(node.left) + calcTree(node.right);
                }
                else if (node.operation == "-")
                {
                    return calcTree(node.left) - calcTree(node.right);
                }
                else if (node.operation == "*")
                {
                    return calcTree(node.left) * calcTree(node.right);
                }
                else if (node.operation == "/")
                {
                    return calcTree(node.left) / calcTree(node.right);
                }
            }
            else if (node.operation == "x")
            {
                return 10;
            }
            else if (node.operation.ToCharArray()[0] == 'x' && node.operation.ToCharArray()[1] == '^')
            {
                return (float)(Math.Pow(10, getPow(node.operation, 2)));
            }
            return float.Parse(node.operation);
        }

        static bool baseOperand(char charCheck)
        {
            if (charCheck == '+' || charCheck == '-' || charCheck == '*' || charCheck == '/')
            {
                return true;
            }
            return false;
        }

        static bool baseOperand(string stringCheck)
        {
            if (stringCheck == "+" || stringCheck == "-" || stringCheck == "*" || stringCheck == "/")
            {
                return true;
            }
            return false;
        }

        static float createFloat(long whole, long deci)
        {
            byte deciLength = (byte)(deci.ToString().Length);
            float retFloat = whole;
            retFloat += (float)(deci * Math.Pow(10, -deciLength));
            return retFloat;
        }

        static void insertXBranch(ParseTree<string> insBranch, string insX)
        {
            if (insBranch.operation == null || insBranch.operation == "")
            {
                insBranch.operation = insX;
            }
            else if (insBranch.left == null)
            {
                insBranch.left = new ParseTree<string>(insX);
            }
            else
            {
                insBranch.right = new ParseTree<string>(insX);
            }
        }

        static float getPow(string num, short startPow)
        {
            char[] numArr = num.Substring(startPow).ToCharArray();
            byte rubbish;

            byte i;
            if (numArr[0] == '-')
            {
                i = 1;
            }
            else
            {
                i = 0;
            }
            for (; i < numArr.Length; i++)
            {
                if (byte.TryParse(numArr[i].ToString(), out rubbish) || numArr[i] == '.')
                {
                    continue;
                }
                else
                {
                    return float.Parse((new string(numArr).Substring(0,i)));
                }
            }
            return float.Parse(new string(numArr));
        }

        static void insertFloatBranch(ParseTree<string> insBranch, float insFloat)
        {
            if (insBranch.operation == null || insBranch.operation == "")
            {
                insBranch.operation = insFloat.ToString();
            }
            else if (insBranch.left == null)
            {
                insBranch.left = new ParseTree<string>(insFloat.ToString());
            }
            else
            {
                insBranch.right = new ParseTree<string>(insFloat.ToString());
            }
        }

        private void ParseButton_Click(object sender, EventArgs e)
        {
            char[] function = FunctionText.Text.ToCharArray();

            ParseTree<string> root = new ParseTree<string>("");
            ParseTree<string> branch = root;

            byte numIndex;
            long intNum = 0;
            long deciNum = 0;
            bool isNum = false;
            bool isFloat = false;

            for (short i = 0; i < function.Length; i++)
            {
                if (byte.TryParse(function[i].ToString(), out numIndex))
                {
                    isNum = true;

                    while (isNum && i < function.Length)
                    {
                        if (byte.TryParse(function[i].ToString(), out numIndex))
                        {
                            if (!isFloat)
                            {
                                intNum = intNum * 10 + numIndex;
                            }
                            else
                            {
                                deciNum = deciNum * 10 + numIndex;
                            }
                            i++;
                        }
                        else if (function[i] == '.' && !isFloat)
                        {
                            isFloat = true;
                            i++;
                        }
                        else
                        {
                            if (i < function.Length && function[i] == 'x')
                            {
                                if (i < function.Length - 2 && function[i + 1] == '^')
                                {
                                    insertFloatBranch(branch, createFloat(intNum, deciNum));
                                    branch = new ParseTree<string>(branch,"*",(new ParseTree<string>("x^" + getPow(new string(function), (short)(i + 2)).ToString())));                          
                                    i += (short)(getPow(new string(function), (short)(i + 2)).ToString().Length + 2);
                                    isNum = false;
                                    isFloat = false;
                                    intNum = 0;
                                    deciNum = 0;
                                }
                                else
                                {
                                    insertFloatBranch(branch, createFloat(intNum, deciNum));
                                    branch = new ParseTree<string>(new ParseTree<string>("x"), "*", branch);
                                    i++;
                                    isNum = false;
                                    isFloat = false;
                                    intNum = 0;
                                    deciNum = 0;
                                }
                            }
                            else
                            {
                                if (isNum)
                                {
                                    insertFloatBranch(branch, createFloat(intNum, deciNum));
                                    isNum = false;
                                    isFloat = false;
                                    intNum = 0;
                                    deciNum = 0;
                                }
                            }
                        }
                    }
                    i--;

                    if (isNum)
                    {
                        insertFloatBranch(branch, createFloat(intNum, deciNum));
                        isNum = false;
                        isFloat = false;
                        intNum = 0;
                        deciNum = 0;
                    }
                }
                else if (baseOperand(function[i]))
                {
                    if (branch.operation == null || branch.operation == "")
                    {
                        branch.operation = function[i].ToString();
                        if (function[i] == '-' && branch.left == null)
                        {
                            branch.left = new ParseTree<string>("0");
                        }
                    }
                    else
                    {
                        if (function[i] == '-' && branch.left == null)
                        {
                            branch.left = new ParseTree<string>("0");
                        }
                        branch = new ParseTree<string>(branch, function[i].ToString(), null);
                    }
                }
                else if (function[i] == 'x')
                {
                    if (i < function.Length - 2 && function[i + 1] == '^')
                    {
                        insertXBranch(branch,"x^" + getPow(new string(function), (short)(i+2)).ToString());
                        i += (short)(getPow(new string(function), (short)(i + 2)).ToString().Length + 1);
                    }
                    else
                    {
                        insertXBranch(branch, "x");
                    }
                }
            }
            MessageBox.Show(calcTree(branch).ToString());
        }
    }
}
