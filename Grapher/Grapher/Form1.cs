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

        static double calcTree(ParseTree<string> node, double xVal, double yVal)
        {
            if (node == null)
            {
                return 0;
            }
            if (baseOperand(node.operation))
            {
                if (node.operation == "+")
                {
                    return calcTree(node.left, xVal, yVal) + calcTree(node.right, xVal, yVal);
                }
                else if (node.operation == "-")
                {
                    return calcTree(node.left, xVal, yVal) - calcTree(node.right, xVal, yVal);
                }
                else if (node.operation == "*")
                {
                    return calcTree(node.left, xVal, yVal) * calcTree(node.right, xVal, yVal);
                }
                else if (node.operation == "/")
                {
                    return calcTree(node.left, xVal, yVal) / calcTree(node.right, xVal, yVal);
                }
            }
            else if (node.operation == "x")
            {
                return xVal;
            }
            else if (node.operation == "y")
            {
                return yVal;
            }
            else if (node.operation == "^")
            {
                return (float)(Math.Pow(calcTree(node.left, xVal, yVal), calcTree(node.right, xVal, yVal)));
            }
            else if ((bool)(trigHOperand(node.operation)[0]))
            {
                if ((string)(trigHOperand(node.operation)[1]) == "cosh")
                {
                    return (double)(Math.Cosh(calcTree(node.left, xVal, yVal)));
                }
                else if ((string)(trigHOperand(node.operation)[1]) == "sinh")
                {
                    return (double)(Math.Sinh(calcTree(node.left, xVal, yVal)));
                }
                else
                {
                    return (double)(Math.Tanh(calcTree(node.left, xVal, yVal)));
                }
            }
            else if ((bool)(trigOperand(node.operation)[0]))
            {
                if ((string)(trigOperand(node.operation)[1]) == "cos")
                {
                    return (double)(Math.Cos(calcTree(node.left, xVal, yVal)));
                }
                else if ((string)(trigOperand(node.operation)[1]) == "sin")
                {
                    return (double)(Math.Sin(calcTree(node.left, xVal, yVal)));
                }
                else
                {
                    return (double)(Math.Tan(calcTree(node.left, xVal, yVal)));
                }
            }
            else if ((bool)(logOperand(node.operation)[0]))
            {
                if ((string)(logOperand(node.operation)[1]) == "log")
                {
                    return (float)(Math.Log10(calcTree(node.left, xVal, yVal)));
                }
                else
                {
                    return (float)(Math.Log(calcTree(node.left, xVal, yVal)));
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

        static object[] trigHOperand(char[] stringCheck, short startChar)
        {
            if (stringCheck.Length - startChar < 4)
            {
                return new object[1] { false };
            }
            string funcString = new string(stringCheck).Substring(startChar, 4);

            if (funcString == "cosh" || funcString == "sinh" || funcString == "tanh")
            {
                return new object[2] { true, funcString };
            }

            return new object[1] { false };
        }

        static object[] trigHOperand(string stringCheck)
        {
            if (stringCheck == "cosh" || stringCheck == "sinh" || stringCheck == "tanh")
            {
                return new object[2] { true, stringCheck };
            }

            return new object[1] { false };
        }

        static object[] logOperand(char[] stringCheck, short startChar)
        {
            if (stringCheck.Length - startChar < 3)
            {
                return new object[1] { false };
            }

            string funcString = new string(stringCheck).Substring(startChar, 3);

            if (funcString == "log" || funcString == "lan")
            {
                return new object[2] { true, funcString };
            }

            return new object[1] { false };
        }

        static object[] logOperand(string stringCheck)
        {
            if (stringCheck == "log" || stringCheck == "lan")
            {
                return new object[2] { true, stringCheck };
            }

            return new object[1] { false };
        }

        static float createFloat(long whole, long deci)
        {
            byte deciLength = (byte)(deci.ToString().Length);
            float retFloat = whole;
            retFloat += (float)((deci - Math.Pow(10, deciLength - 1)) * Math.Pow(10, -deciLength + 1));
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

        static bool forceInsertBranch(ParseTree<string> insBranch, ParseTree<string> branch)
        {
            if (branch.operation == null)
            {
                branch = insBranch;
                return true;
            }
            else if (branch.right.operation == null)
            {
                branch.right = insBranch;
                return true;
            }
            else if (baseOperand(branch.right.operation))
            {
                if (!forceInsertBranch(insBranch, branch.right))
                {
                    return forceInsertBranch(insBranch, branch.left);
                }
            }
            return false;
        }

        static object[] parseBranch(short startChar, char[] function)
        {
            object[] errorObj = new object[2] { "", 2 };

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
                                if (isNum)
                                {
                                    insertFloatBranch(branch, createFloat(intNum, deciNum));
                                    isNum = false;
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
                                if((bool)(trigHOperand(branch.operation)[0]) || (bool)(trigOperand(branch.operation)[0]) || (bool)(logOperand(branch.operation)[0]))
                                {
                                    branch = new ParseTree<string>(branch, function[i].ToString(), null);
                                }
                                else if (i < function.Length - 2 && function[i + 1] == '(')
                                {
                                    retBranch = parseBranch((short)(i + 2), function);
                                    branch = new ParseTree<string>(branch, function[i].ToString(), (ParseTree<string>)(retBranch[1]));
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
                    else if ((bool)(trigHOperand(function, i)[0]))//check for hyperbolic functions
                    {
                        if (function[i + 4] == '(')
                        {
                            retBranch = parseBranch((short)(i + 5), function);
                            retOperand = (string)(trigHOperand(function, i)[1]);
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
                                else if (branch.left == null)
                                {
                                    branch = new ParseTree<string>(new ParseTree<string>((ParseTree<string>)(retBranch[1]), retOperand, null),
                                                                                    branch.operation, branch.right);
                                }
                                else
                                {
                                    forceInsertBranch(new ParseTree<string>((ParseTree<string>)(retBranch[1]), retOperand, null), branch);
                                }
                            }
                            i = (short)(retBranch[0]);
                        }
                        else
                        {
                            return errorObj;
                        }
                    }
                    else if ((bool)(trigOperand(function, i)[0]))//check for trig functions
                    {
                        if (function[i + 3] == '(')
                        {
                            retBranch = parseBranch((short)(i + 4), function);
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
                                else if (branch.left == null)
                                {
                                    branch = new ParseTree<string>(new ParseTree<string>((ParseTree<string>)(retBranch[1]), retOperand, null),
                                                                                    branch.operation, branch.right);
                                }
                                else
                                {
                                    forceInsertBranch(new ParseTree<string>((ParseTree<string>)(retBranch[1]), retOperand, null), branch);
                                }
                            }
                            i = (short)(retBranch[0]);
                        }
                        else
                        {
                            return errorObj;
                        }
                    }
                    else if ((bool)(logOperand(function, i)[0]))
                    {
                        if (function[i + 3] == '(')
                        {
                            retBranch = parseBranch((short)(i + 4), function);
                            retOperand = (string)(logOperand(function, i)[1]);
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
                                else if(branch.left==null)
                                {
                                    branch = new ParseTree<string>(new ParseTree<string>((ParseTree<string>)(retBranch[1]), retOperand, null),
                                                                                    branch.operation, branch.right);
                                }
                                else
                                {
                                    forceInsertBranch(new ParseTree<string>((ParseTree<string>)(retBranch[1]), retOperand, null),branch);
                                }
                            }
                            i = (short)(retBranch[0]);
                        }
                        else
                        {
                            return errorObj;
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
                return errorObj;
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
            float minX = (float)(MinXVal.Value);
            float maxX = (float)(MaxXVal.Value);
            float minY = (float)(MinYVal.Value);
            float maxY = (float)(MaxYVal.Value);

            float incY = 100 / (Math.Abs(maxY) + Math.Abs(minY));
            float incX = 100 / (Math.Abs(maxX) + Math.Abs(minX));

            double[,] zCoord = new double[100, 100];
            double[,] xCoord = new double[100, 100];
            double[,] yCoord = new double[100, 100];

            Graphics graph = graphPanel.CreateGraphics();
            graph.Clear(Color.White);

            if (maxX <= minX || maxY<=minY)
            {
                MessageBox.Show("Your graph needs to start before it ends!");
            }
            else
            {
                try
                {
                    for (double y = minY; y < maxY; y += incY)
                    {
                        for (double x = minX; x < maxX; x += incX)
                        {
                            zCoord[(int)((x - minX) / incX), (int)((y - minY) / incY)] = calcTree((ParseTree<string>)(branch[1]), x, y);
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