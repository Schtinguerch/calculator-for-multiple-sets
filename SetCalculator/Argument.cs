using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetCalculator
{
    public class Argument
    {
        private string _declaration;
        private string _argumentName;
        private Lot<object> _outLot;

        private Lot<object> ConvertToLot(string lotString)
        {
            if (string.IsNullOrWhiteSpace(lotString)) return new Lot<object>();
            if (lotString.First() != '{' || lotString.Last() != '}') return new Lot<object>();

            var lotItemsDeclaration = lotString.Substring(0, lotString.Length - 1);
            lotItemsDeclaration = lotItemsDeclaration.Substring(1, lotString.Length - 2);

            var outLot = new Lot<object>();
            var items = lotItemsDeclaration.Split(',');

            foreach (var item in items)
            {
                if (item.Contains("{"))
                    outLot.Add(ConvertToLot(item));
                else
                    outLot.Add(item);
            }

            return outLot;
        }

        public string Declaration
        {
            get => _declaration;
            set
            {
                _declaration = value;
                var briefDeclaration = _declaration.Replace(" ", "");
                var tokens = briefDeclaration.Split('=');

                if (tokens.Length != 2)
                {
                    _outLot = new Lot<object>();
                    return;
                }

                _argumentName = tokens[0];
                _outLot = ConvertToLot(tokens[1]);
            }
        }

        public string ArgumentName => _argumentName;

        public Lot<object> Lot => _outLot;

        public Argument(string declaration) =>
            Declaration = declaration;
    }
}
