using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEncryptionStandard;
using DES;

namespace DES
{
    public class DESKeyScheduler
    {
        int[] pc1 = { 56, 48, 40, 32, 24, 16, 8, 0, 57, 49, 41, 33, 25, 17,9,1,58,50,42,34,26,18,10,2,59,51,43,35, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 60, 52, 44, 36, 28, 20, 12, 4, 27, 19, 11, 3 };
        int[] pc1R = { 62,54,46,38,30,22,14,6,61,53,45,37,29,21,13,5,60,52,44,36,28,20,12,4,27,19,11,3 };
        int[] pc2 = { 13,16,10,23,0,4,2,27,14,5,20,9,22,18,11,3,25,7,15,6,26,19,12,1,40,51,30,36,46,54,29,39,50,44,32,47,43,48,38,55,33,52,45,41,49,35,28,31 };
        int[] shiftAmount = { 1,1,2,2,2,2,2,2,1,2,2,2,2,2,2,1};

        private BitArray _key;

        private BitArray[] _keys;

        private int _rounds;
        
        public DESKeyScheduler(BitArray key, int rounds)
        {
            _key = key;
            _keys = new BitArray[rounds];
            _rounds = rounds;
        }

        private BitArray[] _GenerateKeys()
        {
            BitArray[] res = new BitArray[_rounds];

            BitArray c = new BitArray(28);
            BitArray d = new BitArray(28);

            BitArray intermediate = new BitArray(56);
            

            for (int i = 0; i < pc1.Length; i++)
            {
                intermediate[i] = _key[pc1[i]];
            }
            var splits = intermediate.Split(2);

            c = splits[0];
            d = splits[1];



            for (int round = 0; round < _rounds; round++)
            {
                BitArray roundKey = new BitArray(48);
                bool c1 = c[0];
                bool c2 = c[1];
                bool d1 = d[0];
                bool d2 = d[1];

                c.RightShift(shiftAmount[round]);
                d.RightShift(shiftAmount[round]);

                c[27] = c1;
                d[27] = d1;

                if (shiftAmount[round] > 1)
                {
                    c[27] = c2;
                    d[27] = d2;

                    c[26] = c1;
                    d[26] = d1;
                }

                for (int i = 0; i < 24; i++)
                {
                    roundKey[i] = c[pc2[i]];
                    roundKey[i + 24] = d[pc2[i + 24] - 28];
                }

                res[round] = roundKey;

                //Console.WriteLine("Key " + round + ": " +ToPrint(roundKey.AsLong(),6,48));
            }

            return res;
        }

        public BitArray GetKey(int round, bool decrypt = false)
        {
            if(_keys == null)
            {
                _keys = _GenerateKeys();
            }

            return _keys[decrypt ? _rounds-1-round : round];
        }

        public static string ToPrint(long v1, int n = 4, int len = 64)
        {
            string s = Convert.ToString(v1, 2);

            string res = "";

            while(s.Length < len)
            {
                s = "0" + s;
            }

            for (int i = 0; i < len; i++)
            {
                res += s[i];
                if((i+1)%n == 0 && i > 0)
                {
                    res += " ";
                }
            }

            return res;

        }

        public void GenerateKeys()
        {
            _keys = _GenerateKeys();
        }
    }
}
