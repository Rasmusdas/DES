using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEncryptionStandard
{
    public static class Utils
    {

        public static bool verbose = true;
        public static string AsString(this BitArray arr)
        {
            string res = "";

            for (int i = 0; i < arr.Length; i++)
            {
                res = (arr[i] ? 1 : 0) + res;
            }

            return res;
        }

        public static void Log(object obj)
        {
            if(verbose)
            {
                Console.WriteLine(obj.ToString());
            }
        }

        public static BitArray Reverse(this BitArray arr)
        {
            BitArray res = new BitArray(arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                res[i] = arr[arr.Length - 1 - i];
            }

            return res;
        }

        public static int AsInt(this BitArray arr)
        {
            int res = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i])
                {
                    res += (int)Math.Pow(2, i);
                }
            }

            return res;
        }

        public static long AsLong(this BitArray arr)
        {
            long res = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i])
                {
                    res += (long)Math.Pow(2, i);
                }
            }

            return res;
        }

        public static BitArray[] Split(this BitArray arr, int n)
        {
            if (arr.Length % n != 0)
            {
                throw new ArgumentException("Arr must be divisible into n equal parts");
            }

            BitArray[] res = new BitArray[n];

            for (int i = 0; i < n; i++)
            {
                BitArray newRes = new BitArray(arr.Length / n);
                for (int j = 0; j < arr.Length / n; j++)
                {
                    newRes[j] = arr[i * arr.Length / n + j];
                }
                res[i] = newRes;
            }

            return res;
        }

        public static BitArray Append(this BitArray arr, BitArray arr2)
        {
            
            BitArray res = new BitArray(arr.Length+arr2.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                res[i] = arr[i];
            }

            for (int i = 0; i < arr2.Length; i++)
            {
                res[arr.Length + i] = arr2[i];
            }

            return res;
        }
    }
}
