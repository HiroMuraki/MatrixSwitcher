using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MatrixSwitcher.ViewModel.ValueConverter {
    [ValueConversion(typeof(MatrixTransformStatus), typeof(Brush))]
    public class TransformStatusToColor : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            Color color;
            try {
                MatrixTransformStatus status = (MatrixTransformStatus)value;
                switch (status) {
                    case MatrixTransformStatus.Ok:
                        color = Color.FromRgb(0x31, 0xd4, 0x85);
                        break;
                    case MatrixTransformStatus.Error:
                        color = Color.FromRgb(0xd4, 0x31, 0x31);
                        break;
                    default:
                        color = Colors.White;
                        break;
                }

            }
            catch {
                color = Colors.White;
            }
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
