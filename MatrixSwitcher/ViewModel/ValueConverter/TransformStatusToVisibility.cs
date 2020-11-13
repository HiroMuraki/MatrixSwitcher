using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MatrixSwitcher.ViewModel.ValueConverter {
    [ValueConversion(typeof(MatrixTransformStatus), typeof(Visibility))]
    public class TransformStatusToVisibility : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                MatrixTransformStatus status = (MatrixTransformStatus)value;
                switch (status) {
                    case MatrixTransformStatus.Ok:
                        return Visibility.Collapsed;
                    case MatrixTransformStatus.Error:
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
            catch (Exception) {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
