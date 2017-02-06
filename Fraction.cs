using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms; //for MessageBox
using ModularArithmeticCalc; //for ModArithmetic.cs

namespace LinearAlgebra
{
    class Fraction
    {
    /* Author:  Scott Wolfskill  
     * Created       04/24/2016
     * Last edited   04/24/2016
     * 
     * References: ModArithmetic.cs */

        protected int a, b; //a is numerator, b is denominator

        public Fraction() //no-arg ctor. Just makes a 0/1 fraction
        {
            a = 0;
            b = 1;
        }
        /// <summary>
        /// Constructs a rational number where a is the numerator and b is the denominator.
        /// </summary>
        public Fraction(int a, int b)
        {
            if (b == 0) //undefined
                throw new ArgumentOutOfRangeException("b" + Environment.NewLine +  "Reason: cannot divide by 0.");
            else
            {
                this.a = a;
                this.b = b;
                simplify();
            }
        }

        public Fraction(Fraction source) //copy CTOR
        {
            if (source == null) throw new ArgumentNullException("source");
            this.a = source.a;
            this.b = source.b;
        }

        /// <summary>
        /// Returns the numerator; read-only.
        /// </summary>
        public int numerator()
        {
            return a;
        }

        /// <summary>
        /// Returns the denominator; read-only.
        /// </summary>
        public int denominator()
        {
            return b;
        }

        protected void simplify() //simplifies fractions: i.e -3/-9 becomes 1/3
        {
            //1. Simplify signs
            simplifySigns();
            //2. Fully reduce
            if (a == 0)
                b = 1;
            else if(Math.Abs(a) != 1) //if a is 1 or -1, already fully reduced 
            {
                int gcd = ModArithmetic.gcd(a, b);
                //MessageBox.Show(gcd.ToString());
                gcd = Math.Abs(gcd);
                a = a / gcd;
                b = b / gcd;
            }
        }

        protected void simplifySigns()
        {
            if ((a < 0 && b < 0) || (a > 0 && b < 0)) //1st: two negatives make two positives. 2nd: for notation, want negative on top.
            {
                a = -1 * a;
                b = -1 * b;
            }
        }

#region Logic
        public bool Equals(Fraction other)
        {
            return this == other;
        }

        public override bool Equals(object o)
        {
            try
            {
                return (bool)(this == (Fraction)o);
            }
            catch
            {
                return false;
            }
        }

        public static bool operator ==(Fraction x, Fraction y)
        {
            /* Warning: this will only function correctly if both fractions are simplified 
            (which they should be unless one fraction is in the middle of some operation) */
            return (x.a == y.a && x.b == y.b);
        }

        public static bool operator !=(Fraction x, Fraction y)
        {
            return !(x == y);
        }

        public static bool operator >(Fraction x, Fraction y)
        {
            //First, do easy check of sign
            if (x.a > 0 && y.a <= 0) return true;
            if (x.a < 0 && y.b >= 0) return false;
            //Need both fractions to have a common denominator in order to compare them. So find lcm:
            int lcm = ModArithmetic.lcm(x.b, y.b);
            return (x.a * (lcm / x.b) > y.a * (lcm / y.b));
        }

        public static bool operator <(Fraction x, Fraction y)
        {
            return y > x; //only need to overload one of <, >
        }

        public static bool operator >=(Fraction x, Fraction y)
        {
            if (x == y) return true; //check == first because it's faster
            else return x > y;
        }

        public static bool operator <=(Fraction x, Fraction y)
        {
            return y >= x; //only need to overload one of <=, >=
        }
#endregion

#region Operator Overloading: +, -, *, /, ^ ; ++, --
        public static Fraction operator +(Fraction x, Fraction y) 
        {
            Fraction sum = new Fraction();
            sum.b = ModArithmetic.lcm(x.b, y.b);
            //now multiply each fraction by the each fraction's denominator / lcm  (multiply by whatever factor away they are from the lcm)
            sum.a = x.a * (sum.b / x.b) + y.a * (sum.b / y.b); //ex: given 3/4 and 1/6, find lcm = 12. so sum = (3*(12/4) + 1*(12/6))/12 = (3*3 + 1*2)/12 = 11/12
            sum.simplify();
            return sum;
        }

        public static Fraction operator -(Fraction x, Fraction y)
        {
            return x + new Fraction(-1 * y.a, y.b); //x + y = x + (-y)
        }

        public static Fraction operator *(Fraction x, Fraction y)
        {
            return new Fraction(x.a * y.a, x.b * y.b);
        }

        public static Fraction operator /(Fraction x, Fraction y)
        {
            if (y.a == 0) throw new ArgumentException("y cannot be 0; division by 0 is undefined.");
            return x * y.reciprocal(); //(a/b) / (c/d) = (a/b) * (d/c)
        }

        public static Fraction operator ^(Fraction x, int pow) 
        {
            //simple: component-wise exponentiation
            if (pow == -1) return x.reciprocal();
            return new Fraction((int) Math.Pow(x.a, pow), (int) Math.Pow(x.b, pow));
        }

        public static Fraction operator ++(Fraction x)
        {
            return x + 1;
        }

        public static Fraction operator --(Fraction x)
        {
            return x - 1;
        }
#endregion

#region Type conversion
        public static implicit operator Fraction(int x) //explicitly convert an int to a fraction
        {
            return new Fraction(x, 1);
        }

        public static implicit operator Fraction(double x) //explicitly convert double into a fraction
        {
            string temp = x.ToString();
            if (temp.Contains('.'))
            {
                int digits = temp.Substring(temp.IndexOf('.')).Length - 1;
                return new Fraction((int) (x * Math.Pow(10, digits)), (int) Math.Pow(10, digits));
            }
            else //really an int
                return (int) x;
        }
#endregion

        public Fraction reciprocal() //returns the reciprocal. Doesn't change the original.
        {
            if (a == 0) throw new InvalidOperationException("0 has no reciprocal; division by 0 is undefined.");
            return new Fraction(b, a);
        }

        public Double ToDouble()
        {
            return a / b;
        }

        public override string ToString()
        {
            if (b == 1) return a.ToString();
            else return a.ToString() + "/" + b.ToString();
        }
    }
}
