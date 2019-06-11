using System;

namespace NNSharp.lib
{
    public class Matrix
    {
        public double[,] Array { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }
        public Matrix(int rows, int cols)
        {
            this.Rows = rows;
            this.Columns = cols;
            this.Array = new double[rows, cols];
        }

        public Matrix(Matrix matrix)
        {
            this.Rows = matrix.Rows;
            this.Columns = matrix.Columns;
            this.Array = new double[this.Rows, this.Columns];
            
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    this.Array[i, j] = matrix.Array[i, j];
                }
            }

        }

        public Matrix(double[,] matrix)
        {
            this.Array = matrix;
            this.Rows = matrix.GetLength(0);
            this.Columns = matrix.GetLength(1);
        }
        public Matrix(int rows, int cols, params double[] items)
        {
            int k = 0;
            if (items.Length != rows * cols)
                throw new Exception("Invalid params");
            this.Rows = rows;
            this.Columns = cols;
            this.Array = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    this.Array[i, j] = items[k++];
                }
            }
        }

        public static void Print(Matrix matrix)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    Console.Write(matrix.Array[i, j] + " ");
                }
                Console.WriteLine();
            }

        }

        public void Randomize()
        {
            Random rnd = new Random();

            for (int i = 0; i < this.Rows; i++)
                for (int j = 0; j < this.Columns; j++)
                    this.Array[i, j] = 2 * rnd.NextDouble() - 1;
        }

        public static Matrix Add(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Rows == matrix2.Rows && matrix1.Columns == matrix2.Columns)
            {
                Matrix m = new Matrix(matrix1.Rows, matrix1.Columns);
                for (int i = 0; i < matrix1.Rows; i++)
                {
                    for (int j = 0; j < matrix1.Columns; j++)
                    {
                        m.Array[i, j] = matrix1.Array[i, j] + matrix2.Array[i, j];
                    }
                }
                return m;
            }
            else
                throw new Exception("Invalid params");
        }

        public static Matrix Subtract(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Rows == matrix2.Rows && matrix1.Columns == matrix2.Columns)
            {
                Matrix m = new Matrix(matrix1.Rows, matrix1.Columns);
                for (int i = 0; i < matrix1.Rows; i++)
                {
                    for (int j = 0; j < matrix1.Columns; j++)
                    {
                        m.Array[i, j] = matrix1.Array[i, j] - matrix2.Array[i, j];
                    }
                }
                return m;
            }
            else
                throw new Exception("Invalid params");
        }

        public static Matrix Multiply(Matrix matrix, double value)
        {
            Matrix m = new Matrix(matrix.Rows, matrix.Columns);
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    m.Array[i, j] = matrix.Array[i, j] * value;
                }
            }
            return m;
        }

        public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Columns == matrix2.Rows)
            {
                double temp = 0;
                Matrix m = new Matrix(matrix1.Rows, matrix2.Columns);
                for (int i = 0; i < matrix1.Rows; i++)
                {
                    for (int j = 0; j < matrix2.Columns; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < matrix1.Columns; k++)
                        {
                            temp += matrix1.Array[i, k] * matrix2.Array[k, j];
                        }
                        m.Array[i, j] = temp;
                    }
                }
                return m;
            }
            else
                throw new Exception("Invalid params");
        }

        public static Matrix HadamardProduct(Matrix matrix1, Matrix matrix2)
        {
            Matrix m = new Matrix(matrix1.Rows, matrix1.Columns);
            for (int i = 0; i < matrix1.Rows; i++)
            {
                for (int j = 0; j < matrix1.Columns; j++)
                {
                    m.Array[i, j] = matrix1.Array[i, j] * matrix2.Array[i, j];
                }
            }
            return m;
        }

        public static Matrix Transpose(Matrix matrix)
        {
            Matrix m = new Matrix(matrix.Columns, matrix.Rows);

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    m.Array[j, i] = matrix.Array[i, j];
                }
            }

            return m;
        }

    }
}