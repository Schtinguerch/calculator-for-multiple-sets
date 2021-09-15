using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetCalculator
{
    public class LotExpressionEvaluator
    {
        private readonly Dictionary<string, string> Replacements = new Dictionary<string, string>()
        {
            { "unite", LotOperation.Union },
            { "unt", LotOperation.Union },
            { "+", LotOperation.Union },

            { "intersect", LotOperation.Intersection },
            { "itr", LotOperation.Intersection },
            { "*", LotOperation.Intersection },

            { "difference", LotOperation.Difference },
            { "dif", LotOperation.Difference },
            { "\\", LotOperation.Difference },

            { "symmetric", LotOperation.SymDifference },
            { "sym", LotOperation.SymDifference },
            { "#", LotOperation.SymDifference },
        };

        private LotCalculator<object> _calculator;
        private Dictionary<string, Lot<object>> _lots;
        private string _expression;

        public IEnumerable<Argument> Arguments { get; set; }

        public string Expression
        {
            get => _expression;
            set
            {
                var correctedExpression = value;
                foreach (var replacement in Replacements)
                    correctedExpression = correctedExpression.Replace(replacement.Key, replacement.Value);

                _expression = correctedExpression;
            }
        }

        public Lot<object> ResultLot(string expression)
        {
            Expression = expression;
            return ResultLot();
        }

        public Lot<object> ResultLot()
        {
            var prefixExpression = PrepareDictionaryAndConvertPrefix().Replace("(", "").Replace(")", "");
            return EvaluatePrefix(prefixExpression);
        }

        private string PrepareDictionaryAndConvertPrefix()
        {
            var hasUniversum = false;
            var universum = new Lot<object>(Arguments.First().Lot);

            _lots = new Dictionary<string, Lot<object>>();
            _calculator = new LotCalculator<object>();

            foreach (var argument in Arguments)
            {
                _lots.Add(argument.ArgumentName, argument.Lot);
                universum = _calculator.Union(universum, argument.Lot);

                if (argument.ArgumentName == "U")
                    hasUniversum = true;
            }

            if (!hasUniversum) _lots.Add("U", universum);
            _calculator.Universum = _lots["U"];

            return Expression.ToPrefix();
        }

        private Lot<object> GetLotViaName(string lotName)
        {
            if (lotName[0].ToString() == LotOperation.Complement)
            {
                var actualLotName = lotName.Substring(1);
                return _calculator.Complement(_lots[actualLotName]);
            }

            return _lots[lotName];
        }

        private Lot<object> EvaluatePrefix(string expression)
        {
            var operands = new Stack<Lot<object>>();

            for (int i = expression.Length - 1; i >= 0; i--)
            {
                if (expression[i] == ' ') continue;

                if (!expression[i].IsOperationSymbol())
                {
                    var varName = "";
                    while (!expression[i].IsOperationSymbol() && expression[i] != ' ')
                    {
                        varName = expression[i].ToString() + varName;
                        i--;

                        if (i >= 0)
                        if (expression[i].ToString() == LotOperation.Complement)
                        {
                            varName = LotOperation.Complement + varName;
                            break;
                        }

                        if (i == 0)
                            break;
                    }

                    operands.Push(GetLotViaName(varName));
                    i++;
                }

                else
                {
                    var leftOperand = operands.Peek();
                    operands.Pop();

                    var rightOperand = operands.Peek();
                    operands.Pop();

                    switch (expression[i].ToString())
                    {
                        case LotOperation.Union:
                            operands.Push(_calculator.Union(leftOperand, rightOperand));
                            break;

                        case LotOperation.Intersection:
                            operands.Push(_calculator.Intersect(leftOperand, rightOperand));
                            break;

                        case LotOperation.Difference:
                            operands.Push(_calculator.Difference(leftOperand, rightOperand));
                            break;

                        case LotOperation.SymDifference:
                            operands.Push(_calculator.SymDifference(leftOperand, rightOperand));
                            break;
                    }
                }
            }

            return operands.Peek();
        }
    }
}
