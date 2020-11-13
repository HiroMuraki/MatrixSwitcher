using Mathematics;
using MatrixSwitcher.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatrixSwitcher {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MatrixPresenter Matrix { get; set; }

        public MainWindow() {
            this.Matrix = new MatrixPresenter();
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            string[] operations = (sender as TextBox).Text.Split('\n');
            this.Matrix.Transforms.Clear();
            foreach (var operation in operations) {
                if (string.IsNullOrEmpty(operation)) {
                    continue;
                }
                this.Matrix.Transforms.Add(operation.Trim());
            }
            this.Matrix.StartTransformsAsync();
        }

        private void CopyMatrix_Click(object sender, RoutedEventArgs e) {
            Clipboard.SetText(this.Matrix.MainMatrix?.ToString());
        }

        private void CopyElement_Click(object sender, RoutedEventArgs e) {
            string information = "";
            try {
                RationalNumber value = (RationalNumber)((sender as FrameworkElement).Tag);
                information = $"{value} : {value.Value}";
            }
            catch {
                information = "未知错误";
            }
            MessageBox.Show(information);
        }

        private void SwitchMatrix_Click(object sender, RoutedEventArgs e) {
            this.Matrix.SwapMatrix();
        }

        private void Window_Move(object sender, MouseButtonEventArgs e) {
            this.DragMove();
        }

        private void Window_Close(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
    }
}
