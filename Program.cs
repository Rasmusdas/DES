namespace DataEncryptionStandard
{
    internal class Program
    {

        static ulong diff = 0x405c000004000000;
        static ulong diffOut = 0x6000000000000000;

        static ulong alpha = 0x2104008000008000;
        static ulong delta = 0x0104008000011000;

        static int n = 10000;
        static float runs = 10;

        public static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        public static async Task MainAsync(string[] args)
        {
            int totalProb = 0;
            BitArray key = GenerateRandom(64);
            List<Task<int>> tasks = new List<Task<int>>();
            for (int i = 0; i < runs; i++)
            {
                Task<int> run = Task.Run(() => { return DoRunLin(n,key); });
                tasks.Add(run);
            }

            foreach(var v in tasks)
            {
                int val = await v;
                if(val < n/2f)
                {
                    totalProb += val;
                }
                else
                {
                    totalProb += n-val;
                }
                Console.WriteLine(val);
            }

            Console.WriteLine(Math.Abs((totalProb / (n * runs))));
            Console.WriteLine(Math.Abs((totalProb / (n * runs)) - 0.5f));



            //Console.WriteLine("Message:");
            //Console.WriteLine(DESKeyScheduler.ToPrint(message.AsLong()) + "\n");


            //message = des.InitialPermutation(message);

            //Console.WriteLine("IP:");
            //Console.WriteLine(DESKeyScheduler.ToPrint(message.AsLong())+"\n");

            //var splitMess = des.Split(message,2);

            //Console.WriteLine("Expanded:");
            //Console.WriteLine(DESKeyScheduler.ToPrint(des.Expand(splitMess[1]).AsLong(), 6, 48) + "\n");


            //Console.WriteLine("Key:");
            //Console.WriteLine(DESKeyScheduler.ToPrint(scheduler.GetKey(0).AsLong(), 6, 48)+ "\n");


            //var expanded = des.Expand(splitMess[1]);
            //var expandedXOR = expanded.Xor(scheduler.GetKey(0));

            //Console.WriteLine("Key + XOR:");
            //Console.WriteLine(DESKeyScheduler.ToPrint(expandedXOR.AsLong(), 6, 48)+"\n");

            //expandedXOR = expandedXOR.Reverse();

            //var expandSplit = des.Split(expandedXOR, 8);

            //for (int i = 0; i < expandSplit.Length; i++)
            //{
            //    expandSplit[i] = des.SBox(expandSplit[i], 7 - i);
            //}

            //BitArray expandedCombined = new BitArray(32);

            //for (int i = 0; i < expandSplit.Length; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        expandedCombined[i * 4 + j] = expandSplit[i][j];
            //    }
            //}

            //expandedCombined = expandedCombined.Reverse();

            //Console.WriteLine("Sboxes");

            //Console.WriteLine(DESKeyScheduler.ToPrint(expandedCombined.AsLong(), 4, 32)+ "\n");

            //var permuted = des.Permute(expandedCombined);

            //Console.WriteLine("Permuted");

            //Console.WriteLine(DESKeyScheduler.ToPrint(permuted.AsLong(), 4, 32) + "\n");


            //Console.WriteLine("Final");
            //var final = splitMess[0].Xor(permuted);

            //Console.WriteLine(DESKeyScheduler.ToPrint(final.AsLong(), 4, 32) + "\n");

            //Console.WriteLine(final.Reverse().AsLong().ToString("x"));

            //Console.WriteLine(final.Reverse().Append(splitMess[1].Reverse()).AsLong().ToString("x"));








            //BitArray arr = new BitArray(32);

            //arr.SetAll(true);

            //arr.LeftShift(2);

            //Console.Write(Convert.ToString(arr.AsLong(),2));

            //BitArray arr = new BitArray(32);
            //BitArray newArr = new BitArray(6);

            //for (int i = 0; i < 8; i++)
            //{
            //    arr[i * 4 + 3] = true;
            //}

            //foreach(var v in des.Split(des.Expand(arr),8))
            //{
            //    Console.WriteLine(v.AsString());
            //}
        }

        public static BitArray GenerateRandom(int n)
        {
            BitArray arr = new BitArray(n);

            for (int i = 0; i < n; i++)
            {
                arr[i] = Random.Shared.Next(0, 2) == 1 ? true : false;
            }

            return arr;
        }

        public static bool Same(BitArray arr1, BitArray arr2)
        {
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static int DoRun(int n)
        {
            int prob = 0;

            for (int i = 0; i < n; i++)
            {
                BitArray key = GenerateRandom(64);
                BitArray message = GenerateRandom(64);
                BitArray messageCopy = message.DeepCopy();
                messageCopy = messageCopy.Xor(DES.FromValue(diff, 64));

                key = key.Reverse();
                message = message.Reverse();
                messageCopy = messageCopy.Reverse();

                DESKeyScheduler scheduler = new DESKeyScheduler(key, 6);

                scheduler.GenerateKeys();

                DES des = new(scheduler);

                BitArray ciph1 = des.Crypt(key, message, 5, false);
                BitArray ciph2 = des.Crypt(key, messageCopy, 5, false);

                BitArray res = ciph1.Xor(ciph2);

                if (Same(DES.FromValue(diff, 64).Reverse(), res))
                {
                    prob++;
                }
            }

            return prob;
        }

        public static int DoRunLin(int n,BitArray key )
        {
            int count = 0;
            BitArray alphaArr = DES.FromValue(alpha, 64);
            alphaArr = alphaArr.Reverse();

            BitArray deltaArr = DES.FromValue(delta, 64);
            deltaArr = deltaArr.Reverse();

            
            DESKeyScheduler scheduler = new DESKeyScheduler(key, 7);

            scheduler.GenerateKeys();
            for (int i = 0; i < n; i++)
            {
                BitArray message = GenerateRandom(64);

                int scalarMA = Scalar(message, alphaArr);

                DES des = new(scheduler);

                BitArray ciph1 = des.Crypt(key, message, 3, false);

                int scalarMB = Scalar(ciph1, alphaArr);

                if((scalarMA ^ scalarMB) == 0)
                {
                    count++;
                }
            }

            return count;
        }

        public static int Scalar(BitArray arr1, BitArray arr2)
        {
            int res = 0;

            for (int i = 0; i < arr1.Length; i++)
            {
                if(arr1[i] && arr2[i])
                {
                    res = (res + 1) % 2;
                }
            }

            return res;
        }
    }
}
