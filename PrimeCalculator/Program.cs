using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Primes
{
    class Program
    {
        static int[] primes;
        static Dictionary<int, Dictionary<int, int>> phiTable =
            new Dictionary<int, Dictionary<int, int>>();

        // Sieve of Erathosthenes. Used for getting first N primes
        static int[] Erathosthenes(bool[] isPrimeArr)
        {
            int n = isPrimeArr.Length;
            isPrimeArr[0] = isPrimeArr[1] = true;
            for (int i = 2; i < Math.Sqrt(n); i++)
            {
                if (!isPrimeArr[i])
                {
                    for (int j = i * i; j < n; j += i)
                    {
                        isPrimeArr[j] = true;
                    }
                }
            }
            var res = new List<int>();

            for (int i = 0; i < n; i++)
            {
                if (!isPrimeArr[i])
                {
                    res.Add(i);
                }
            }
            return res.ToArray();
        }

        // Phi function used for calculations
        static int Phi(int x, int a)
        {
            // If value's already been calculated and inserted into the table,
            // just use it
            if (phiTable.ContainsKey(x) && phiTable[x].ContainsKey(a))
            {
                return phiTable[x][a];
            }
            else if (a == 1)
                return (x + 1) / 2;
            else
            {
                // Recursively calling Phi
                int res = Phi(x, a - 1) - Phi(x / primes[a - 1], a - 1);
                // If value hasn't been calculated before ,
                // we shall add it to the table
                if (!phiTable.ContainsKey(x))
                {

                    phiTable[x] = new Dictionary<int, int>();
                }
                phiTable[x][a] = res;
                return res;
            }
        }

        // Phi function, which's used for calculating number of primes up to N
        static int Pi(int n)
        {
            // In case if value's less than the number of primes,
            // got by sieving
            if (n < primes.Length)
            {
                int res = Array.BinarySearch(primes, n);
                // Returning the position of insertion
                return res < 0 ? -res - 1 : res;
            }
            // Otherwise Pi will be called recursively and
            // Phi function will be used
            else
            {
                int a = Pi((int)Math.Sqrt(n));
                return Phi(n, a) + a - 1;
            }
        }

        static void Main(string[] args)
        {
            /* Testing values */

            // Calculating the number of primes up to N
            int N = 1_000_000_000;

            // Size of array of prime numbers, which are gonna be
            // calculated by sieving
            int primeArrSize = 100000;
            var arr = new bool[primeArrSize];

            Stopwatch t = new Stopwatch();

            t.Start();
            primes = Erathosthenes(arr);
            int res = Pi(N);
            t.Stop();

            Console.WriteLine(res);
            Console.WriteLine(t.ElapsedMilliseconds + "ms");
        }
    }
}