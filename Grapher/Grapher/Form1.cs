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
            else if (node.operation == "x" || node.operation == "y")
            {
                return 10;
            }
            else if (node.operation == "^")
            {
                return (float)(Math.Pow(calcTree(node.left), calcTree(node.right)));
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
            ParseTree<string> root = new ParseTree<string>("");
            ParseTree<string> branch = root;

            object[] retBranch;
            byte numIndex;
            long intNum = 0;
            long deciNum = 0;
            bool isNum = false;
            bool isFloat = false;
            float powFloat;

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
                                    retBranch=parseBranch((short)(i+2),function);

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
                    deciNum = 0;
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
                            if (branch.left == null)
                            {
                                branch = new ParseTree<string>(branch, function[i].ToString(), null);
                            }
                            else
                            {
                                branch = new ParseTree<string>(branch.left, branch.operation,
                                        new ParseTree<string>(branch.right, function[i].ToString(), new ParseTree<string>(null)));
                            }
                        }
                        else
                        {
                            branch = new ParseTree<string>(branch, function[i].ToString(), null);
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
                            else
                            {

                            }
                            i += (short)(retBranch[0]);
                        }
                        else
                        {
                            insertVarBranch(branch, function[i].ToString() + "^" + getPow(new string(function), (short)(i + 2)).ToString());
                            i += (short)(getPow(new string(function), (short)(i + 2)).ToString().Length + 1);
                        }                        
                    }
                    else
                    {
                        insertVarBranch(branch, function[i].ToString());
                    }
                }
                else if (function[i] == '(')
                {
                    retBranch = parseBranch((short)(i + 1), function);
                    i += (short)(retBranch[0]);
                    if (branch.left == null)
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

        static ParseTree<string> emptyNode()
        {
            return new ParseTree<string>("0");
        }

        private void ParseButton_Click(object sender, EventArgs e)
        {
            char[] function = FunctionText.Text.ToCharArray();

            object[] branch = parseBranch(0, function);
            
            MessageBox.Show(calcTree((ParseTree<string>)(branch[1])).ToString());
        }
    }
}