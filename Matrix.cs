using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearAlgebra
{
    class Matrix
    {
    /* Author:  Scott Wolfskill  
     * Created       04/24/2016
     * Last edited   02/05/2017
     * 
     * Represents a matrix as a 2D array with entries represented by the Fraction class.
     * (References: Fraction.cs, which refers to ModArithmetic.cs) */

        protected int r, c; //r = rows, c = columns
        protected Fraction[][] entries; //r x c  2D array that holds all entries

        public Matrix() //the empty matrix
        {
            r = 0;
            c = 0;
            entries = null;
        }

        public Matrix(int rows, int columns) //Generates a r x c matrix with default entries of 0.
        {
            createMatrix(rows, columns, 0);
        }

        /// <summary>
        /// Creates a (rows) x (columns) matrix where every entry is set to (value).
        /// </summary>
        public Matrix(int rows, int columns, Fraction value)
        {
            createMatrix(rows, columns, value);
        }

        public Matrix(Matrix source) //copy ctor
        {
            copy(source);
        }

        protected void copy(Matrix source)
        {
            r = source.r;
            c = source.c;
            entries = new Fraction[r][];
            for (int i = 0; i < r; i++)
            {
                entries[i] = new Fraction[c];
                for (int j = 0; j < c; j++)
                {
                    entries[i][j] = source.entries[i][j];
                }
            }
        }

        /// <summary>
        /// Returns the number of rows; read-only.
        /// </summary>
        public int rows()
        {
            return r;
        }

        /// <summary>
        /// Returns the number of columns; read-only.
        /// </summary>
        public int columns()
        {
            return c;
        }

        protected void createMatrix(int rows, int columns, Fraction value) //create a (rows) x (columns) matrix where every entry is set to (value).
        {
            r = rows;
            c = columns;
            entries = new Fraction[rows][];
            for (int i = 0; i < rows; i++)
            {
                entries[i] = new Fraction[columns];
                for (int j = 0; j < columns; j++)
                {
                    entries[i][j] = value;
                }
            }
        }

        #region Logic
        public bool Equals(Matrix other)
        {
            return this == other;
        }

        public override bool Equals(object o)
        {
            try
            {
                return (bool)(this == (Matrix)o);
            }
            catch
            {
                return false;
            }
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            if (a.r != b.r || a.c != b.c) return false; //unequal dimensions
            for (int rr = 0; rr < a.r; rr++)
            {
                for (int cc = 0; cc < a.c; cc++)
                {
                    if (a.entries[rr][cc] != b.entries[rr][cc]) return false;
                }
            }
            return true;
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            return !(a == b);
        }
#endregion

#region Operator Overloading: +, -, *, ^ ; ++, --
        public static Matrix operator +(Matrix a, Matrix b) //simple: component-wise addition
        {
            if (a.r != b.r || a.c != b.c) throw new InvalidOperationException("For addition, the dimensions of both matrices must be the same.");
            Matrix sum = new Matrix(a.r, a.c);
            
            return sum;
        }

        public static Matrix operator -(Matrix a, Matrix b) //simple: component-wise subtraction
        {
            if (a.r != b.r || a.c != b.c) throw new InvalidOperationException("For subtraction, the dimensions of both matrices must be the same.");

            return new Matrix();
        }

        public static Matrix operator *(Matrix a, Matrix b) //matrix multiplication. TODO
        {
            if (a.c != b.r) throw new InvalidOperationException("For multiplication, the number of columns of A must match the number of rows of B.");
            Matrix product = new Matrix(a.r, b.c); //ex: 1x3 X 3x4 = 1x4 matrix
            //TODO
            return product;
        }

        public static Matrix operator ^(Matrix a, int pow) //component-wise exponentiation
        {
            //Allowed values for pow: pow >= -1
            if (pow < -1) throw new ArgumentOutOfRangeException("Expected pow >= -1");
            if (pow == -1) return a.inverse();
            else if (pow == 0) return new Matrix(a.r, a.c, 1);
            else
            {
                Matrix toReturn = new Matrix(a);
                if (pow != 1) //no further operations needed if pow == 1
                {
                    if (a.diagonal()) //raise every entry on the main diagonal to the power
                    {
                        int smallerDim = a.r < a.c ? a.r : a.c; //the number of entries on the main diagonal is determined by the smallest dimension
                        for (int i = 0; i < smallerDim; i++)
                        {
                            toReturn.entries[i][i] = toReturn.entries[i][i] ^ pow;
                        }
                    }
                    else //do it the slow way
                    {
                        for (int i = 1; i < pow; i++)
                        {
                            toReturn = toReturn * a;
                        }
                    }
                }
                return toReturn;
            }
        }

        public static Matrix operator ++(Matrix a) //increment each element by 1
        {
            return a + new Matrix(a.r, a.c, 1);
        }

        public static Matrix operator --(Matrix a) //decrement each element by 1
        {
            return a - new Matrix(a.r, a.c, 1);
        }
#endregion

#region Matrix Operations and Properties: Transpose, Determinant, Inverse, Echelon, Reduced Echelon
        public Matrix transpose() //TODO
        {
            /* 1st col becomes 1st row, 2nd col becomes 2nd row, etc. Ex:
             * [ 1 2 ] T   =  [ 1 3 ]
             * [ 3 4 ]        [ 2 4 ]        Important to note that (A^T)^T = A  */
            Matrix trans = new Matrix(c, r);
            //TODO
            return trans;
        }

        public Fraction determinant() //partially done
        {
            /* For 1x1: det = entry. For 2x2: | a b | = ad - bc. For 3x3:
             *                                | c d |
             */
            if (r != c) throw new InvalidOperationException("Determinants are only defined for square matrices.");
            if (r == 1) return entries[0][0];
            if(r == 2) return (entries[0][0] * entries[1][1]) - (entries[0][1] * entries[1][0]);
            //Now complicated: need to make smaller unless it's an upper triangular already
            Fraction det = new Fraction(1, 1);
            if (upperTriangular())
            {
                for (int i = 0; i < r; i++) //for upper triangulars, det is just the product of the diagonal.
                    det *= entries[i][i];
            }else { //TODO

            }
            return det;
        }

        public Matrix inverse()
        {
            return new Matrix(); //TODO
        }

        public bool invertible() //TODO
        {
            return false; //CHANGE
        }

        public bool upperTriangular() //TODO
        {
            /* Returns true if it is an upper triangular matrix, i.e of the form [ * * * *]
             * where all entries BELOW the main diagonal are 0.                  [ 0 * * *]
             *                                                                   [ 0 0 * *] */
            return false; //CHANGE
        }

        public bool lowerTriangular() //TODO
        {
            /* Returns true if it is a lower triangular matrix, i.e of the form [ * 0 0 0 ]
             * where all entries ABOVE the main diagonal are 0.                 [ * * 0 0 ]
             *                                                                  [ * * * 0 ] */
            return false; //CHANGE
        }

        public bool diagonal()
        {
            return (upperTriangular() && lowerTriangular());
        }

        public Matrix echelon() //return the matrix in row-echelon form without altering the original.
        {
            if (upperTriangular()) return this; //no work to be done
            //else, TODO
            return new Matrix();
        }

        public Matrix rref() //return the matrix in reduced row-echelon form without altering the original.
        {
            //TODO
            return new Matrix();
        }
#endregion

    }
}
