using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser_Grapher
{
    public class ParseTree<T>
    {
        private ParseTree<T> left;
        private T operation;
        private ParseTree<T> right;

        public ParseTree(T operand)
        {
            this.left = null;
            this.right = null;
            this.operation = operand;
        }

        public T getOperand()
        {
            return this.operation;
        }

        public void setOperand(T operand)
        {
            this.operation = operand;
        }

        public void setRight(ParseTree<T> newRight)
        {
            this.right = newRight;
        }

        public void setLeft(ParseTree<T> newLeft)
        {
            this.left = newLeft;
        }
    }
    }
}
