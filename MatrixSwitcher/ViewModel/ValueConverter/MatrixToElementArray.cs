using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MatrixSwitcher.ViewModel.ValueConverter {
    [ValueConversion(typeof(Mathematics.Matrix), typeof(List<Mathematics.RationalNumber>))]
    public class MatrixToElementArray : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            Mathematics.Matrix matrix = (Mathematics.Matrix)value;
            if (matrix is null) {
                return null;
            }
            List<Mathematics.RationalNumber> elementArray = new List<Mathematics.RationalNumber>();
            for (int row = 0; row < matrix.RowSize; row++) {
                for (int col = 0; col < matrix.ColumnSize; col++) {
                    elementArray.Add(matrix[row, col]);
                }
            }
            return elementArray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
