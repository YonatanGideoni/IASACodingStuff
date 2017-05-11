﻿using System;
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

        static float calcTree(ParseTree<string> node, float xVal)
        {
            if (node == null)
            {
                return 0;
            }
            if (baseOperand(node.operation))
            {
                if (node.operation == "+")
                {
                    return calcTree(node.left, xVal) + calcTree(node.right, xVal);
                }
                else if (node.operation == "-")
                {
                    return calcTree(node.left, xVal) - calcTree(node.right, xVal);
                }
                else if (node.operation == "*")
                {
                    return calcTree(node.left, xVal) * calcTree(node.right, xVal);
                }
                else if (node.operation == "/")
                {
                    return calcTree(node.left, xVal) / calcTree(node.right, xVal);
                }
            }
            else if (node.operation == "x" || node.operation == "y")
            {
                return xVal;
            }
            else if (node.operation == "^")
            {
                return (float)(Math.Pow(calcTree(node.left, xVal), calcTree(node.right, xVal)));
            }
            else if ((bool)(trigOperand(node.operation)[0]))
            {
                if ((string)(trigOperand(node.operation)[1]) == "cos")
                {
                    return (float)(Math.Cos(calcTree(node.left, xVal)));
                }
                else if ((string)(trigOperand(node.operation)[1]) == "sin")
                {
                    return (float)(Math.Sin(calcTree(node.left, xVal)));
                }
                else
                {
                    return (float)(Math.Tan(calcTree(node.left, xVal)));
                }                
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

        static object[] trigOperand(char[] stringCheck, short startChar)
        {
            if (stringCheck.Length - startChar < 3)
            {
                return new object[1] { false };
            }
            string funcString = new string(stringCheck).Substring(startChar, 3);

            if (funcString == "cos" || funcString == "sin" || funcString == "tan")
            {
                return new object[2] { true, funcString };
            }

            return new object[1] { false };
        }

        static object[] trigOperand(string stringCheck)
        {
            if (stringCheck == "cos" || stringCheck == "sin" || stringCheck == "tan")
            {
                return new object[2] { true, stringCheck };
            }

            return new object[1] { false };
        }

        static float createFloat(long whole, long deci)
        {
            byte deciLength = (byte)(deci.ToString().Length);
            float retFloat = whole;
            retFloat += (float)((deci-Math.Pow(10,deciLength-1)) * Math.Pow(10, -deciLength+1));
            return retFloat;
        }

        static void insertVarBranch(ParseTree<string> insBranch, string insVar)
        {
            if (insBranch.operation == null || insBranch.operation == "")
            {
                insBranch.operation = insVar;
            }
            else if (insBranch.left == null)
            {
                insBranch.left = new ParseTree<string>(insVar);
            }
            else
            {
                insBranch.right = new ParseTree<string>(insVar);
            }
        }

        static void insertVarBranch(ParseTree<string> insBranch, string insVar, ParseTree<string> powBranch)
        {
            insBranch = new ParseTree<string>(insBranch, insVar, powBranch);
        }

        static ParseTree<string> getPow(string num, short startPow)
        {
            char[] numArr = num.Substring(startPow).ToCharArray();
            byte rubbish;

            byte i;
            if (numArr[0] == '-')
            {
                i = 1;
            }
            else if (numArr[0] == '(')
            {
                return (ParseTree<string>)(parseBranch(1, numArr)[1]);
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
                    return new ParseTree<string>(float.Parse((new string(numArr).Substring(0, i))).ToString());
                }
            }
            return new ParseTree<string>(float.Parse((new string(numArr).Substring(0, i))).ToString());
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
            else if (insBranch.right == null)
            {
                insBranch.right = new ParseTree<string>(insFloat.ToString());
            }
            else
            {
                insertFloatBranch(insBranch.right, insFloat);
            }
        }

        static object[] parseBranch(short startChar, char[] function)
        {
            try
            {
                ParseTree<string> root = new ParseTree<string>("");
                ParseTree<string> branch = root;

                object[] retBranch;
                byte numIndex;
                long intNum = 0;
                long deciNum = 1;
                bool isNum = false;
                bool isFloat = false;
                float powFloat;
                string retOperand;

                for (short i = startChar; i < function.Length; i++)
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
                                if (i < function.Length && (function[i] == 'x' || function[i] == 'y'))
                                {
                                    if (i < function.Length - 2 && function[i + 1] == '^')
                                    {
                                        insertFloatBranch(branch, createFloat(intNum, deciNum));
                                        retBranch = parseBranch((short)(i + 2), function);

                                        if (float.TryParse((string)(((ParseTree<string>)(retBranch[1])).operation), out powFloat))
                                        {
                                            branch = new ParseTree<string>(branch.left, branch.operation,
                                                 (new ParseTree<string>(branch.right, "*", new ParseTree<string>(function[i].ToString() + "^" + powFloat.ToString()))));
                                            i += (short)(powFloat.ToString().Length + 2);
                                            isNum = false;
                                        }
                                    }
                                    else
                                    {
                                        insertFloatBranch(branch, createFloat(intNum, deciNum));
                                        branch = new ParseTree<string>(branch.left, branch.operation,
                                                    new ParseTree<string>(branch.right, "*", new ParseTree<string>(function[i].ToString())));
                                        i++;
                                        isNum = false;
                                    }
                                }
                                else
                                {
                                    if (isNum)
                                    {
                                        insertFloatBranch(branch, createFloat(intNum, deciNum));
                                        isNum = false;
                                    }
                                }
                            }
                        }
                        i--;

                        if (isNum)
                        {
                            insertFloatBranch(branch, createFloat(intNum, deciNum));
                            isNum = false;
                        }
                        isFloat = false;
                        intNum = 0;
                        deciNum = 1;
                    }
                    else if (baseOperand(function[i]))
                    {
                        if (branch.operation == null || branch.operation == "")
                        {
                            branch.operation = function[i].ToString();
                            if ((function[i] == '-' || function[i] == '+') && branch.left == null)
                            {
                                branch.left = emptyNode();
                            }
                        }
                        else
                        {
                            if (function[i] == '-' && branch.left == null)
                            {
                                branch = new ParseTree<string>(branch, "-", null);
                            }
                            else if (function[i] == '/' || function[i] == '*')
                            {
                                if (i < function.Length - 2 && function[i + 1] == '(')
                                {
                                    retBranch = parseBranch((short)(i + 2), function);
                                    branch = new ParseTree<string>(branch, "*", (ParseTree<string>)(retBranch[1]));
                                    i = (short)(retBranch[0]);
                                }
                                else
                                {
                                    if (branch.left == null || branch.right == null)
                                    {
                                        branch = new ParseTree<string>(branch, function[i].ToString(), null);
                                    }
                                    else
                                    {
                                        if (baseOperand(branch.operation))
                                        {
                                            branch = new ParseTree<string>(branch.left, branch.operation,
                                                new ParseTree<string>(branch.right, function[i].ToString(), new ParseTree<string>(null)));
                                        }
                                        else if (branch.operation == "^")
                                        {
                                            branch = new ParseTree<string>(branch, function[i].ToString(), null);
                                        }

                                    }
                                }
                            }
                            else
                            {
                                branch = new ParseTree<string>(branch, function[i].ToString(), null);
                            }
                        }
                    }
                    else if (function[i] == '^')
                    {
                        if (i < function.Length - 2 && function[i + 1] == '(')
                        {
                            retBranch = parseBranch((short)(i + 2), function);
                            if (branch.right == null)
                            {
                                branch = new ParseTree<string>(branch, "^", (ParseTree<string>)(retBranch[1]));
                            }
                            else
                            {
                                branch = new ParseTree<string>(branch.left, branch.operation, new ParseTree<string>(branch.right, "^", (ParseTree<string>)(retBranch[1])));
                            }
                            i = (short)(retBranch[0]);
                        }
                        else
                        {
                            if (branch.right == null)
                            {
                                branch = new ParseTree<string>(branch, "^", null);
                            }
                            else
                            {
                                branch = new ParseTree<string>(branch.left, branch.operation, new ParseTree<string>(branch.right, "^", null));
                            }
                        }
                    }
                    else if (function[i] == 'x' || function[i] == 'y')
                    {
                        if (i < function.Length - 2 && function[i + 1] == '^')
                        {
                            if (function[i + 2] == '(')
                            {
                                retBranch = parseBranch((short)(i + 3), function);

                                if (branch.operation == null || branch.operation == "")
                                {
                                    branch = new ParseTree<string>(new ParseTree<string>(function[i].ToString()), "^", (ParseTree<string>)(retBranch[1]));
                                }
                                else if (branch.left == null)
                                {
                                    insertVarBranch(branch.right, branch.operation,
                                                new ParseTree<string>(new ParseTree<string>(function[i].ToString()), "^", (ParseTree<string>)(retBranch[1])));
                                }
                                else if (branch.right == null)
                                {
                                    insertVarBranch(new ParseTree<string>(new ParseTree<string>(function[i].ToString()), "^", (ParseTree<string>)(retBranch[1])),
                                               branch.operation, branch.left);
                                }
                                i = (short)(retBranch[0]);
                            }
                            else
                            {
                                if (branch.operation == null || branch.operation == "")
                                {
                                    branch = new ParseTree<string>(new ParseTree<string>(function[i].ToString()), "^", (ParseTree<string>)((getPow(new string(function), (short)(i + 2)))));
                                }
                                else
                                {
                                    branch = new ParseTree<string>(branch.left, branch.operation, new ParseTree<string>
                                    (new ParseTree<string>(function[i].ToString()), "^", (ParseTree<string>)(getPow(new string(function), (short)(i + 2)))));
                                }

                                i += (short)(getPow(new string(function), (short)(i + 2)).ToString().Length + 1);
                            }
                        }
                        else
                        {
                            insertVarBranch(branch, function[i].ToString());
                        }
                    }
                    else if ((bool)(trigOperand(function, i)[0]))
                    {
                        if (function[i + 3] == '(')
                        {
                            retBranch=parseBranch((short)(i+4),function);
                            retOperand = (string)(trigOperand(function, i)[1]);
                            if (branch.operation == null || branch.operation == "")
                            {
                                branch = new ParseTree<string>((ParseTree<string>)(retBranch[1]), retOperand, null);
                            }
                            else
                            {
                                if (branch.right == null)
                                {
                                    branch = new ParseTree<string>(new ParseTree<string>((ParseTree<string>)(retBranch[1]), retOperand, null), 
                                                                                    branch.operation, branch.left);
                                }
                                else
                                {
                                    branch = new ParseTree<string>(new ParseTree<string>((ParseTree<string>)(retBranch[1]), retOperand, null),
                                                                                    branch.operation, branch.right);
                                }
                            }
                            i = (short)(retBranch[0]);
                        }
                        else
                        {
                            return new object[2] { "", 2 };
                        }                        
                    }
                    else if (function[i] == '(')
                    {
                        retBranch = parseBranch((short)(i + 1), function);
                        i = (short)(retBranch[0]);

                        if (branch.operation == null || branch.operation == "")
                        {
                            branch = (ParseTree<string>)(retBranch[1]);
                        }
                        else if (branch.left == null)
                        {
                            branch = new ParseTree<string>((ParseTree<string>)(retBranch[1]), branch.operation, branch.right);
                        }
                        else if (branch.right == null)
                        {
                            branch = new ParseTree<string>(branch.left, branch.operation, (ParseTree<string>)(retBranch[1]));
                        }
                    }
                    else if (function[i] == ')')
                    {
                        return new object[2] { i, branch };
                    }
                }

                return new object[2] { (short)(function.Length), branch };
            }
            catch
            {
                return new object[2] { "", 2 };
            }
        }

        static ParseTree<string> emptyNode()
        {
            return new ParseTree<string>("0");
        }

        private void ParseButton_Click(object sender, EventArgs e)
        {
            char[] function = FunctionText.Text.ToCharArray();

            object[] branch = parseBranch(0, function);
            float minX = Math.Max((float)(MinXVal.Value),-graphPanel.Width/2);
            float maxX = Math.Min((float)(MaxXVal.Value),graphPanel.Width/2);
            if (maxX <= minX)
            {
                MessageBox.Show("Your graph needs to start before it ends!");
            }
            else
            {
                float y = 0;
                float prevX = minX;
                float prevY = calcTree((ParseTree<string>)(branch[1]), minX);
                Graphics graph = graphPanel.CreateGraphics();
                graph.Clear(Color.White);

                try
                {
                    for (float x = minX; x < maxX; x += (float)(0.001))
                    {
                        y = calcTree((ParseTree<string>)(branch[1]), x);
                        if (!float.IsNaN(y) && !float.IsInfinity(y))
                        {
                            if (Math.Abs(y - prevY) < 10)//don't connect asymptotes to other parts of graph
                            {
                                graph.DrawLine(Pens.Blue, graphPanel.Width / 2 + x, graphPanel.Height / 2 - y, graphPanel.Width / 2 + prevX, graphPanel.Height / 2 - prevY);
                                prevX = x;
                                prevY = y;
                            }
                            else
                            {
                                prevY = y;
                                prevX = x;
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("There is a syntax problem, please write your function differently.");
                }
            }
        }
    }
}