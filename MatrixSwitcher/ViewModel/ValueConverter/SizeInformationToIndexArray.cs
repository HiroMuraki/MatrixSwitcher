using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MatrixSwitcher.ViewModel.ValueConverter {
    [ValueConversion(typeof(int), typeof(List<string>))]
    public class SizeInformationToIndexArray : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            List<string> indexs = new List<string>();
            try {
                int sizeValue = (int)value;
                for (int i = 0; i < sizeValue; i++) {
                    indexs.Add($"{i+1}");
                }
            }
            catch (Exception) {

            }
            return indexs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
