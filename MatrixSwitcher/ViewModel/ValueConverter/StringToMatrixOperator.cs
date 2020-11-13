using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MatrixSwitcher.ViewModel.ValueConverter {
    [ValueConversion(typeof(string), typeof(MatrixOperator))]
    public class StringToMatrixOperator : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                MatrixOperator op = (MatrixOperator)value;
                switch (op) {
                    case MatrixOperator.Add:
                        return "+";
                    case MatrixOperator.Sub:
                        return "-";
                    case MatrixOperator.Multiply:
                        return "x";
                    case MatrixOperator.First:
                        return "<";
                    case MatrixOperator.Second:
                        return ">";
                    default:
                        return "<";
                }
            }
            catch {
                return "<";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                string op = (value as ListBoxItem).Content as string;
                switch (op) {
                    case "<":
                        return MatrixOperator.First;
                    case ">":
                        return MatrixOperator.Second;
                    case "+":
                        return MatrixOperator.Add;
                    case "-":
                        return MatrixOperator.Sub;
                    case "x":
                        return MatrixOperator.Multiply;
                    default:
                        return MatrixOperator.First;
                }
            }
            catch {
                return MatrixOperator.First;
            }
        }
    }
}
