using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms; //For MessageBox only

namespace ModularArithmeticCalc
{
    //Author: Scott Wolfskill
    //Created      09/28/15
    //Last edited  04/24/16
    public static class ModArithmetic
    {
        /* FUNCITONALITY: 
         * Can compute remainders, 
         * Determine congruence mod k, 
         * Simplify a number to its simplest congruence mod k  (0...k-1)
         * Simplify a number to a power to its simplest congruence mod k  (0...k-1)
         */
            
            /* TODO: Implement Fermat's Little Theorem if p in (mod p) is prime:
             * FLT: If p is prime and n is not a multiple of p, then n^(p-1) is congruent to 1 (mod p).
             *  (so n raised to 1 less than a prime p) */
        public static int remainder(int modBase, int num) //modBase = num to divide by, num = number to be divided
        {
            //Potential problem: doing double-type division. Is this very slow? Would it be faster to check if factor*modBase > num (while factor++)?
            if (modBase == 0) return -1; //cannot divide by 0! -1 will denote error
            if (num == 0) return 0; //0 / n = 0 where n != 0; remainder = 0
            int factor = (int) Math.Floor((double) num / modBase);
            return num - (modBase * factor);

        }

        public static int gcd(int a, int b) //compute the Greatest Common Divisor using the Euclidean Algorithm
        {
            return gcd(a, b, true);
        }

        private static int gcd(int a, int b, bool start)
        {
            if (b > a && start) //always want a >= b for the initial call. After that, no, because things will get screwed up.
            {
                int swap = a;
                a = b;
                b = swap;
            }
            int r = remainder(b, a);
            if (r == 0) return b;
            else return gcd(b, r, false);
        }

        public static int lcm(int a, int b) //finds the least common multiple of two numbers
        {
            if (a == 0 && b == 0) 
                throw new ArgumentOutOfRangeException("a or b" + Environment.NewLine + "Reason: both a and b cannot be 0 as that would involve dividing by 0.");
            return Math.Abs(a * b) / gcd(a, b);
        }

        public static int findLow(int modBase, int num) //Find the equivalent of num in modular arithmetic from 0...modBase
        {
            if (num < modBase && num >= 0) //Nothing necessary
            {
                return num;
            }
            else //Expected case; work to be done
            {
                return remainder(modBase, num);
            }
        }

        public static int findLowPow(int modBase, int num, int pow) //Find the equivalent of num^pow in modular arithmetic from 0...modBase
        {
            //MessageBox.Show("findLowPow: modBase = " + modBase + "; num = " + num + "; pow = " + pow);
            //Strategy: if even pow, break up n^p into (n^2)^(p/2). If odd pow, break up into n(n^2)^((p-1)/2)
                // Could have overflow if num is very large
            //Base cases:
            if (num == 1) return 1; //1^n = 1
            if (num == 0)
            {
                if (pow == 0)
                {
                    MessageBox.Show("ERROR: Invalid input: 0 ^ 0 is undefined!");
                    return -1; //denotes error
                }
                return 0;
            }
            if (pow == 0) return 1; //n^0 = 1 except 0^0 is undef.
            if (pow == 1) return findLow(modBase, num);
            if (num >= modBase) return findLowPow(modBase, findLow(modBase, num), pow); //In mod 5, [7]^n = [2]^n
            //Recursive cases:
            if (remainder(2, pow) == 0) //even power
            {
                return findLowPow(modBase, findLow(modBase, num * num), pow / 2);
            }
            else //odd power
            {
                return findLow(modBase, num * findLowPow(modBase, findLow(modBase, num * num), (pow - 1) / 2));
            }
        }

        public static bool equals(int modBase, int num1, int num2) //Check for congruence mod k between two ints.
        {
            return (findLow(modBase, num1) == findLow(modBase, num2));
        }
    }
}
