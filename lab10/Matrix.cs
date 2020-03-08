﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace lab10
{
    public class Matrix : ICloneable, IEnumerable, IEnumerator
    {
        private List<List<double>> _matrix;
        private int enumeratorIndex = 0;

        public Matrix()
        {
            _matrix = new List<List<double>>(1) {[0] = new List<double> {default}};
        }

        public Matrix(int size)
        {
            if (size < 1) throw new SizeException();
            _matrix = new List<List<double>>(size);
            for (int i = 0; i < size; i++)
            {
                _matrix.Add(new List<double>(size));
                for (int j = 0; j < size; j++)
                {
                    _matrix[i].Add(default);
                }
            }
        }

        public Matrix(params double[] m)
        {
            int size = (int) Math.Sqrt(m.Length);
            
            _matrix = new List<List<double>>(size);
            for (int i = 0; i < size; i++)
            {
                _matrix.Add(new List<double>(size));
                for (int j = 0; j < size; j++)
                {
                    _matrix[i].Add(default);
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _matrix[i][j] = m[i * size + j];
                }
            }
        }

        public Matrix(Matrix matrix)
        {
            var size = matrix.Size;
            _matrix = new List<List<double>>(size);
            for (int i = 0; i < size; i++)
            {
                _matrix.Add(new List<double>(size));
                for (int j = 0; j < size; j++)
                {
                    _matrix[i].Add(matrix._matrix[i][j]);
                }
            }
        }

        public int Size => _matrix.Count;
        
        public object Clone()
        {
            return new Matrix(this);
        }

        private void Add(Matrix matrix)
        {
            if (Size != matrix.Size) throw new DifferentSizeException();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _matrix[i][j] += matrix._matrix[i][j];
                }
            }
        }

        private void Sub(Matrix matrix)
        {
            if (Size != matrix.Size) throw new DifferentSizeException();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _matrix[i][j] -= matrix._matrix[i][j];
                }
            }
        }

        public void Mul(int k)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _matrix[i][j] *= k;
                }
            }
        }

        private void Mul(double d)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _matrix[i][j] *= d;
                }
            }
        }

        private void Mul(Matrix m)
        {
            if (Size != m.Size) throw new DifferentSizeException();
            Matrix t = new Matrix(this);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < Size; k++)
                    {
                        sum += t._matrix[i][k] * m._matrix[k][j];
                    }
                    _matrix[i][j] = sum;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            foreach (var list in _matrix)
            {
                foreach (var num in list)
                {
                    s.Append($"{num} ");
                }

                s.Append('\n');
            }

            return s.Remove(s.Length - 1, 1).ToString();
        }

        static public Matrix Sum(Matrix a, Matrix b)
        {
            Matrix t = new Matrix(a);
            t.Add(b);
            return t;
        }
        
        static public Matrix Subtract(Matrix a, Matrix b)
        {
            Matrix t = new Matrix(a);
            t.Sub(b);
            return t;
        }
        
        static public Matrix Multiply(Matrix a, Matrix b)
        {
            Matrix t = new Matrix(a);
            t.Mul(b);
            return t;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            return Sum(a, b);
        }
        
        public static Matrix operator -(Matrix a, Matrix b)
        {
            return Subtract(a, b);
        }
        
        public static Matrix operator *(Matrix a, Matrix b)
        {
            return Multiply(a, b);
        }

        public static Matrix operator *(Matrix a, double n)
        {
            return Multiply(a, n);
        }

        public static Matrix operator /(Matrix a, Matrix b)
        {
            return a * Inverse(b);
        }

        private static Matrix Multiply(Matrix matrix, double d)
        {
            var res = new Matrix(matrix);
            res.Mul(d);
            return res;
        }

        public Matrix Transpose()
        {
            var m = new Matrix(this);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    m._matrix[i][j] = _matrix[j][i];
                }
            }
            return m;
        }

        private void Swap(int first, int second)
        {
            List<double> t = new List<double>(_matrix[first]);
            _matrix[first] = _matrix[second];
            _matrix[second] = t;
        }

        public double Determinant()
        {
            // bool isZero = false;
            ToUpperTriangleForm();
            double det = 1;
            for (int i = 0; i < Size; i++)
            {
                det *= _matrix[i][i];
            }

            return det;
        }

        private int FindNotZero(int index)
        {
            for (int i = index + 1; i < Size; i++)
            {
                if (Math.Abs(_matrix[index][i]) > double.Epsilon) return i;
            }

            return index;
        }

        private bool IsZeroColumn(int column)
        {
            var res = true;
            for (int j = 0; j < Size; j++)
            {
                res &= Math.Abs(_matrix[j][column]) < double.Epsilon;
            }

            return res;
        }

        private bool IsZeroRow(int row)
        {
            var res = true;
            for (int i = 0; i < Size; i++)
            {
                res &= Math.Abs(_matrix[row][i]) < double.Epsilon;
            }

            return res;
        }

        public void ToUpperTriangleForm()
        {
            for (int i = 0; i < Size; i++)
            {
                // if (IsZeroColumn(i)) return 0;
                if (Math.Abs(_matrix[i][i]) < double.Epsilon)
                {
                    var index = FindNotZero(i);
                    if (i != index) Swap(i, FindNotZero(i));
                }
                
                for (int j = i; j < Size - 1; j++)
                {
                    double coefficient = _matrix[i + 1][j] / _matrix[i][j];
                    for (int k = j; k < Size; k++)
                    {
                        _matrix[j + 1][k] -= coefficient * _matrix[i][k];
                    }
                }
            }
        }

        public static Matrix Inverse(Matrix m)
        {
            if (Math.Abs(m.Determinant()) < double.Epsilon) throw new InverseMatrixDoesntExistException(); 
            var inverse = new Matrix(m.Size);

            for (int i = 0; i < inverse.Size; i++)
            {
                for (int j = 0; j < m.Size; j++)
                {
                    inverse._matrix[i][j] = m.Pop(i, j).Determinant();
                }
            }

            inverse *= 1 / m.Determinant();
            
            return inverse.Transpose();
        }

        private Matrix Pop(int row, int column)
        {
            Matrix less = new Matrix(this);
            for (int i = 0; i < column; i++)
            {
                _matrix[i].RemoveAt(column);
            }
            less._matrix.RemoveAt(row);
            return less;
        }

        public IEnumerator GetEnumerator()
        {
            enumeratorIndex = -1;
            return this;
        }

        public bool MoveNext()
        {
            return ++enumeratorIndex < Size * Size;
        }

        public void Reset()
        {
            enumeratorIndex = 0;
        }

        object IEnumerator.Current => _matrix[enumeratorIndex / Size][enumeratorIndex % Size];
        
        //                                EXCEPTIONS
        
        [Serializable]
        public class SizeException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public SizeException()
            {
            }

            public SizeException(string message) : base(message)
            {
            }

            public SizeException(string message, Exception inner) : base(message, inner)
            {
            }

            protected SizeException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }
        
        [Serializable]
        public class InverseMatrixDoesntExistException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public InverseMatrixDoesntExistException()
            {
            }

            public InverseMatrixDoesntExistException(string message) : base(message)
            {
            }

            public InverseMatrixDoesntExistException(string message, Exception inner) : base(message, inner)
            {
            }

            protected InverseMatrixDoesntExistException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        public class DifferentSizeException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public DifferentSizeException()
            {
            }

            public DifferentSizeException(string message) : base(message)
            {
            }

            public DifferentSizeException(string message, Exception inner) : base(message, inner)
            {
            }

            protected DifferentSizeException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }

    }
}