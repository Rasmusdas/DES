using DES;

namespace DataEncryptionStandard
{
    /*
     * Implementation of DES primarily using BitArrays
     * 
     * Was not worth it, should probably have used ints and just bit shifted :/
     */

    public class DES
    {
        int[] iP = { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };
        int[] e = { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };
        int[] p = { 16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25 };
        int[] ipRev = { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };

        public int[,,] sBox =  

                        {{{14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,7},
                        {0,15,7,4,14,2,13,1,10,6,12,11,9,5,3,8},
                        {4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0},
                        {15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13}},

                        {{15,1,8,14,6,11,3,4,9,7,2,13,12,0,5,10},
                        {3,13,4,7,15,2,8,14,12,0,1,10,6,9,11,5},
                        {0,14,7,11,10,4,13,1,5,8,12,6,9,3,2,15},
                        {13,8,10,1,3,15,4,2,11,6,7,12,0,5,14,9}},

                        {{10,0,9,14,6,3,15,5,1,13,12,7,11,4,2,8},
                        {13,7,0,9,3,4,6,10,2,8,5,14,12,11,15,1},
                        {13,6,4,9,8,15,3,0,11,1,2,12,5,10,14,7},
                        {1,10,13,0,6,9,8,7,4,15,14,3,11,5,2,12}},

                        {{7,13,14,3,0,6,9,10,1,2,8,5,11,12,4,15},
                        {13,8,11,5,6,15,0,3,4,7,2,12,1,10,14,9},
                        {10,6,9,0,12,11,7,13,15,1,3,14,5,2,8,4},
                        {3,15,0,6,10,1,13,8,9,4,5,11,12,7,2,14}},

                        {{2,12,4,1,7,10,11,6,8,5,3,15,13,0,14,9},
                        {14,11,2,12,4,7,13,1,5,0,15,10,3,9,8,6},
                        {4,2,1,11,10,13,7,8,15,9,12,5,6,3,0,14},
                        {11,8,12,7,1,14,2,13,6,15,0,9,10,4,5,3}},

                        {{12,1,10,15,9,2,6,8,0,13,3,4,14,7,5,11},
                        {10,15,4,2,7,12,9,5,6,1,13,14,0,11,3,8},
                        {9,14,15,5,2,8,12,3,7,0,4,10,1,13,11,6},
                        {4,3,2,12,9,5,15,10,11,14,1,7,6,0,8,13}},

                        {{4,11,2,14,15,0,8,13,3,12,9,7,5,10,6,1},
                        {13,0,11,7,4,9,1,10,14,3,5,12,2,15,8,6},
                        {1,4,11,13,12,3,7,14,10,15,6,8,0,5,9,2},
                        {6,11,13,8,1,4,10,7,9,5,0,15,14,2,3,12}},

                        {{13,2,8,4,6,15,11,1,10,9,3,14,5,0,12,7},
                        {1,15,13,8,10,3,7,4,12,5,6,11,0,14,9,2},
                        {7,11,4,1,9,12,14,2,0,6,10,13,15,3,5,8},
                        {2,1,14,7,4,10,8,13,15,12,9,0,3,5,6,11}}};

        public DESKeyScheduler _scheduler;

        public DES(DESKeyScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public BitArray InitialPermutation(BitArray arr)
        {
            if (arr.Length != 64)
            {
                throw new ArgumentException("Arr size must be 64");
            }

            BitArray res = new BitArray(64);

            for (int i = 0; i < 64; i++)
            {
                res[i] = arr[iP[i] - 1];
            }

            return res;
        }

        public BitArray FinalPermutation(BitArray arr)
        {
            if (arr.Length != 64)
            {
                throw new ArgumentException("Arr size must be 64");
            }

            BitArray res = new BitArray(64);

            for (int i = 0; i < 64; i++)
            {
                res[i] = arr[ipRev[i] - 1];
            }

            return res;
        }

        public BitArray Expand(BitArray arr)
        {
            if (arr.Length != 32)
            {
                throw new ArgumentException("Arr size must be 32");
            }

            BitArray res = new BitArray(48);

            for (int i = 0; i < 48; i++)
            {
                res[i] = arr[e[i] - 1];
            }

            return res;
        }

        public BitArray Permute(BitArray arr)
        {
            if (arr.Length != 32)
            {
                throw new ArgumentException("Arr size must be 32");
            }

            BitArray res = new BitArray(32);

            for (int i = 0; i < 32; i++)
            {
                res[i] = arr[p[i] - 1];
            }

            return res;
        }

        public BitArray SBox(BitArray arr, int sBoxNum, int row)
        {
            if (arr.Length != 4)
            {
                throw new ArgumentException("Arr size must be 4");
            }

            return FromValue(sBox[sBoxNum, row, arr.AsInt()],4);
        }

        public BitArray SBox(BitArray arr, int sBoxNum)
        {
            if (arr.Length != 6)
            {
                throw new ArgumentException("Arr size must be 6");
            }

            int row = (arr[0] ? 1 : 0) + (arr[5] ? 2 : 0);

            BitArray resArr = new BitArray(4);

            for (int i = 0; i < resArr.Length; i++)
            {
                resArr[i] = arr[i + 1];
            }

            return FromValue(sBox[sBoxNum, row, resArr.AsInt()],4);
        }

        public BitArray[] Split(BitArray arr, int n)
        {
            if(arr.Length % n != 0)
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

        public BitArray Crypt(BitArray key, BitArray message, int round)
        {
            Console.WriteLine(message.AsLong().ToString("x"));
            message = InitialPermutation(message);

            for (int r = 0; r < round; r++)
            {
                var splitMess = Split(message, 2);

                var expanded = Expand(splitMess[1]);

                var expandedXOR = expanded.Xor(_scheduler.GetKey(r));

                expandedXOR = expandedXOR.Reverse();

                var expandSplit = Split(expandedXOR, 8);

                for (int i = 0; i < expandSplit.Length; i++)
                {
                    expandSplit[i] = SBox(expandSplit[i], 7 - i);
                }

                BitArray expandedCombined = new BitArray(32);

                for (int i = 0; i < expandSplit.Length; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        expandedCombined[i * 4 + j] = expandSplit[i][j];
                    }
                }

                expandedCombined = expandedCombined.Reverse();
                var permuted = Permute(expandedCombined);
                var final = splitMess[0].Xor(permuted);

                if (r == round - 1)
                {
                    message = final.Append(splitMess[1]);
                }
                else
                {
                    message = splitMess[1].Append(final);
                }

                Console.WriteLine("Round " + r + ": " + message.Reverse().AsLong().ToString("x"));

            }
            return FinalPermutation(message);
        }

        public BitArray SBox(BitArray[] arr)
        {
            if (arr.Length != 8)
            {
                throw new ArgumentException("There must be 8 sub messages");
            }

            BitArray res = new BitArray(32);

            for (int i = 0; i < 8; i++)
            {
                BitArray partArr = SBox(arr[i], i);

                for (int j = 0; j < 4; j++)
                {
                    res[i * 4 + j] = partArr[j];
                }
            }

            return res;
        }


        public static BitArray FromValue(int val, int n)
        {
            BitArray res = new BitArray(n);

            for (int i = 0; i < res.Length; i++)
            {
                res[i] = (val & 1) == 1;
                val = val >> 1;
            }

            return res;
        }

        public static BitArray FromValue(long val, int n)
        {
            BitArray res = new BitArray(n);

            for (int i = 0; i < res.Length; i++)
            {
                res[i] = (val & 1) == 1;
                val = val >> 1;
            }

            return res;
        }
    }
}
