using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grapher
{
    public class ParseTree<T>
    {
        /// <summary>
        /// Creates a new tree branch with an operand and null left and right nodes.
        /// </summary>
        public ParseTree(T operand)
        {
            this.left = null;
            this.right = null;
            this.operation = operand;
        }

        /// <summary>
        /// Creates a new tree branch with an operand, left, and right nodes.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operand"></param>
        /// <param name="right"></param>
        public ParseTree(ParseTree<T> left, T operand, ParseTree<T> right)
        {
            this.left = left;
            this.right = right;
            this.operation = operand;
        }

        public T operation { get; set; }

        public ParseTree<T> right { get; set; }

        public ParseTree<T> left{ get; set; }        
    }
}
