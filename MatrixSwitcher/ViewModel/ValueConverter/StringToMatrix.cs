using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace MatrixSwitcher.ViewModel.ValueConverter {
    [ValueConversion(typeof(Mathematics.Matrix), typeof(string))]
    public class StringToMatrix : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                Mathematics.Matrix matrix = (Mathematics.Matrix)value;
                return matrix.ToString();
            }
            catch {
                return "Unknow";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                string data = (string)value;
                return MatrixPresenter.StringToMatrix(data);
            }
            catch {
                return null;
            }
        }
    }
}
