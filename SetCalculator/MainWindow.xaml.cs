using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SetCalculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void VariableAskButton_Click(object sender, RoutedEventArgs e)
        {
            var helpWindow = new VariableHelpWindow();
            helpWindow.Show();
        }

        private void EquationAskButton_Click(object sender, RoutedEventArgs e)
        {
            var helpWindow = new ExpressionHelpWindow();
            helpWindow.Show();
        }

        private void StartEquationButton_Click(object sender, RoutedEventArgs e)
        {
            var declarations = LotsTextBox.Text.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);
            var arguments = new List<Argument>();

            foreach (var declaration in declarations)
                if (declaration.Length > 3)
                    arguments.Add(new Argument(declaration.Replace("\n", "").Replace(";", "")));

            var expression = ExpressionTextBox.Text;
            var evaluator = new LotExpressionEvaluator()
            {
                Arguments = arguments
            };

            var resultLot = evaluator.ResultLot(expression);
            ResultTextBox.Text = resultLot.ToString();
        }
    }
}
