using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MatrixSwitcher.ViewModel.ValueConverter {
    [ValueConversion(typeof(List<string>), typeof(string))]
    public class TransformTipToString : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            List<string> tips = value as List<string>;
            try {
                return string.Join('\n', tips);
            }
            catch (Exception) {

            }
            return tips;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
