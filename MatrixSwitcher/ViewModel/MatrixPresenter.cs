using Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace MatrixSwitcher.ViewModel {
    public enum MatrixOperator {
        Add,
        Sub,
        Multiply,
        First,
        Second
    }
    public enum MatrixTransformStatus {
        Ok,
        Error
    }
    public class MatrixPresenter : ViewModelBase {
        #region 正则匹配
        private static readonly Regex CR = new Regex(@"[Cc][Rr][ ]+[0-9]+[ ]+[0-9]+");
        private static readonly Regex MR = new Regex(@"[Mm][Rr][ ]+[0-9]+[ ]+[-]?[0-9/]+");
        private static readonly Regex DR = new Regex(@"[Dd][Rr][ ]+[0-9]+[ ]+[0-9]+[ ]+[-]?[0-9/]+");
        private static readonly Regex Trans = new Regex(@"[Tt][Ss]");
        #endregion

        #region 属性
        #region 后备字段
        private Mathematics.Matrix _matrix1;
        private Mathematics.Matrix _matrix2;
        private Mathematics.Matrix _mainMatrix;
        private MatrixOperator _operator;
        private MatrixTransformStatus _transfromStatus;
        private string _matrixTransformErrorMessage;
        private List<string> _transforms;
        private List<string> _transformTips;
        #endregion
        public Mathematics.Matrix Matrix1 {
            get { return _matrix1; }
            set {
                _matrix1 = value;
                this.OnPropertyChanged(nameof(Matrix1));
            }
        }
        public MatrixOperator Operator {
            get {
                return _operator;
            }
            set {
                _operator = value;
                this.OnPropertyChanged(nameof(Operator));
                GetMainMatrix();
            }
        }
        public Mathematics.Matrix Matrix2 {
            get { return _matrix2; }
            set {
                _matrix2 = value;
                this.OnPropertyChanged(nameof(Matrix2));
            }
        }
        public Mathematics.Matrix MainMatrix {
            get {
                return _mainMatrix;
            }
            private set {
                _mainMatrix = value;
                this.OnMainMatrixChanged();
            }
        }
        public int RowSize {
            get {
                return this.MainMatrix.RowSize;
            }
        }
        public int ColumnSize {
            get {
                return this.MainMatrix.ColumnSize;
            }
        }
        public string Determinate {
            get {
                string value = "???";
                try {
                    value = this.MainMatrix.GetDeterminate().ToString();
                }
                catch {
                    value = "null";
                }
                return value;
            }
        }
        public List<string> Transforms {
            get {
                return this._transforms;
            }
            set {
                this._transforms = value;
            }
        }
        public List<string> TransfromTips {
            get {
                return _transformTips;
            }
            set {
                _transformTips = value;
            }
        }
        public MatrixTransformStatus TransformStatus {
            get {
                return this._transfromStatus;
            }
            set {
                this._transfromStatus = value;
                this.OnPropertyChanged(nameof(TransformStatus));
            }
        }
        public string MatrixTransformErrorMessage {
            get {
                return this._matrixTransformErrorMessage;
            }
            set {
                this._matrixTransformErrorMessage = value;
                this.OnPropertyChanged(nameof(MatrixTransformErrorMessage));
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MatrixPresenter() {
            this.Matrix1 = Matrix.GetIdentityMatrix(3, 3);
            this.Matrix2 = Matrix.GetIdentityMatrix(3, 3);
            this.Operator = MatrixOperator.First;
            this.Transforms = new List<string>();
            this.TransfromTips = new List<string>();
            this.TransformStatus = MatrixTransformStatus.Ok;
            GetMainMatrix();
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 异步执行矩阵变换操作
        /// </summary>
        public async void StartTransformsAsync() {
            await Task.Run(this.StartransformsCore);
        }
        /// <summary>
        ///  交换1号和2号矩阵
        /// </summary>
        public void SwapMatrix() {
            Mathematics.Matrix temp = Matrix1;
            Matrix1 = Matrix2;
            Matrix2 = temp;
            GetMainMatrix();
        }
        #endregion

        #region 公共静态方法
        /// <summary>
        /// 将字符串输入转化为矩阵
        /// </summary>
        /// <param name="data">一个以空格分割列元素，换行符分割行元素的字符串</param>
        /// <returns></returns>
        public static Mathematics.Matrix StringToMatrix(string data) {
            data = Regex.Replace(data, @"[ ]+", " ");

            string[] rowsData = data.Split('\n');
            List<string> rows = new List<string>();
            foreach (string row in rowsData) {
                if (string.IsNullOrEmpty(row)) {
                    continue;
                }
                rows.Add(row.Trim());
            }

            int rowSize = rows.Count;
            int columnSize = rows[0].Split(' ').Length;

            Mathematics.Matrix matrix = new Matrix(rowSize, columnSize);
            for (int row = 0; row < rowSize; row++) {
                List<string> elements = new List<string>();
                elements.AddRange(rows[row].Split(' '));
                while (elements.Count < columnSize) {
                    elements.Add("0/1");
                }

                for (int col = 0; col < columnSize; col++) {
                    matrix[row, col] = new RationalNumber(elements[col]);
                }
            }

            return matrix;
        }
        #endregion

        #region 私有辅助方法
        /// <summary>
        /// 获取主矩阵
        /// </summary>
        private void GetMainMatrix() {
            try {
                this.MainMatrix = Operator switch {
                    MatrixOperator.Add => this.Matrix1 + this.Matrix2,
                    MatrixOperator.Sub => this.Matrix1 - this.Matrix2,
                    MatrixOperator.Multiply => this.Matrix1 * this.Matrix2,
                    MatrixOperator.First => this.Matrix1.GetDeepCopy(),
                    MatrixOperator.Second => this.Matrix2.GetDeepCopy(),
                    _ => Mathematics.Matrix.GetIdentityMatrix(3, 3),
                };
            }
            catch {
                this.MainMatrix = Matrix.GetIdentityMatrix(3, 3);
            }
        }
        /// <summary>
        /// 用于主矩阵变化时引发
        /// </summary>
        private void OnMainMatrixChanged() {
            this.OnPropertyChanged(nameof(MainMatrix));
            this.OnPropertyChanged(nameof(RowSize));
            this.OnPropertyChanged(nameof(ColumnSize));
            this.OnPropertyChanged(nameof(Determinate));
        }
        /// <summary>
        /// 化简矩阵
        /// </summary>
        private void SimpifyMatrix() {
            for (int row = 0; row < this.RowSize; row++) {
                for (int col = 0; col < this.ColumnSize; col++) {
                    this.MainMatrix[row, col] = this.MainMatrix[row, col].GetSimpified();
                }
            }
        }
        /// <summary>
        /// 执行矩阵变换操作
        /// </summary>
        private void StartransformsCore() {
            GetMainMatrix();
            this.TransfromTips?.Clear();
            this.MatrixTransformErrorMessage = "";
            try {
                foreach (var item in Transforms) {
                    string transform = Regex.Replace(item, "[ ]+", " ");
                    string[] args = transform.Split(' ');
                    if (CR.IsMatch(transform)) {
                        int xRow = Convert.ToInt32(args[1]);
                        int yRow = Convert.ToInt32(args[2]);
                        this.MainMatrix.RowSwitch(xRow - 1, yRow - 1);
                        this.TransfromTips.Add($"r{xRow } \u2b82 r{yRow}");
                    } else if (MR.IsMatch(transform)) {
                        int row = Convert.ToInt32(args[1]);
                        RationalNumber n = new RationalNumber(args[2]);
                        this.MainMatrix.MRSwitch(n, row - 1);
                        this.TransfromTips.Add($"r{row} x {n}");
                    } else if (DR.IsMatch(transform)) {
                        int targetRow = Convert.ToInt32(args[1]);
                        int sourceRow = Convert.ToInt32(args[2]);
                        RationalNumber n = new RationalNumber(args[3]);
                        this.MainMatrix.DRSwitch(n, sourceRow - 1, targetRow - 1);
                        this.TransfromTips.Add($"r{targetRow} + r{sourceRow} * {n}");
                    } else if (Trans.IsMatch(transform)) {
                        this.MainMatrix = this.MainMatrix.Trans();
                        this.TransfromTips.Add($"转置");
                    } else {
                        throw new InvalidOperationException($"无效的操作: {transform}");
                    }
                }
                this.TransformStatus = MatrixTransformStatus.Ok;
            }
            catch (InvalidOperationException e) {
                this.TransformStatus = MatrixTransformStatus.Error;
                this.MatrixTransformErrorMessage = e.Message;
            }
            catch (ArgumentOutOfRangeException e) {
                this.TransformStatus = MatrixTransformStatus.Error;
                this.MatrixTransformErrorMessage = e.Message;
            }
            catch {
                this.TransformStatus = MatrixTransformStatus.Error;
                this.MatrixTransformErrorMessage = "未知错误";
            }
            finally {
                this.OnPropertyChanged(nameof(TransfromTips));
                this.SimpifyMatrix();
                this.OnMainMatrixChanged();
            }
        }
        #endregion
    }
}
