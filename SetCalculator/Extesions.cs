using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetCalculator
{
    public static class Extesions
    {
        public static bool HasDuplicates<T>(this T item, IEnumerable<T> collection)
        {
            if (collection == null || item == null) return false;

            foreach (var collectionItem in collection)
                if (collectionItem.Equals(item))
                    return true;

            return false;
        }

        public static bool IsOperationSymbol(this char c) =>
            c.ToString() == LotOperation.Union ||
            c.ToString() == LotOperation.Intersection ||
            c.ToString() == LotOperation.Difference ||
            c.ToString() == LotOperation.SymDifference;
        
        private static int OperationPriority(char c)
        {
            switch (c.ToString())
            {
                case LotOperation.Difference:
                case LotOperation.SymDifference:
                    return 1;

                case LotOperation.Union:
                    return 2;

                case LotOperation.Intersection:
                    return 3;

                default:
                    return 0;
            }
        }

        public static string ToPrefix(this string expression)
        {
            var operators = new Stack<char>();
            var operands = new Stack<string>();

            for (int i = 0; i < expression.Length; i++)
            {     
                if (expression[i] == '(')
                    operators.Push(expression[i]);
                
                else if (expression[i] == ')')
                {
                    while (
                        operators.Count != 0 &&
                        operators.Peek() != '(')
                    {
                        var leftOperand = operands.Peek();
                        operands.Pop();

                        var rightOperand = operands.Peek();
                        operands.Pop();

                        char operation = operators.Peek();
                        operators.Pop();

                        var tmp = operation + rightOperand + leftOperand;
                        operands.Push(tmp);
                    }

                    operators.Pop();
                }

                else if (!IsOperationSymbol(expression[i]))
                {
                    var varName = "";
                    while (!IsOperationSymbol(expression[i]))
                    {
                        varName += expression[i].ToString();
                        i++;

                        if (i == expression.Length)
                            break;
                    }

                    operands.Push(varName + " ");
                    i--;
                }

                else
                {
                    while (
                        operators.Count != 0 &&
                        OperationPriority(expression[i]) <=
                        OperationPriority(operators.Peek()))

                    {
                        var leftOperand = operands.Peek();
                        operands.Pop();

                        var rightOperand = operands.Peek();
                        operands.Pop();

                        char operation = operators.Peek();
                        operators.Pop();

                        var tmp = operation + rightOperand + leftOperand;
                        operands.Push(tmp);
                    }

                    operators.Push(expression[i]);
                }
            }

            while (operators.Count != 0)
            {
                var leftOperand = operands.Peek();
                operands.Pop();

                var rightOperand = operands.Peek();
                operands.Pop();

                char operation = operators.Peek();
                operators.Pop();

                var tmp = operation + rightOperand + leftOperand;
                operands.Push(tmp);
            }

            return operands.Peek();
        }
    }
}
