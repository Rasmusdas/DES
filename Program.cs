namespace DataEncryptionStandard
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BitArray key = DES.FromValue(0x0123456789abcdef, 64);
            BitArray message = DES.FromValue(0x4e6f772069732074, 64);

            key = key.Reverse();
            message = message.Reverse();

            DESKeyScheduler scheduler = new DESKeyScheduler(key, 16);

            scheduler.GenerateKeys();
        
            DES des = new(scheduler);

            Console.WriteLine(des.Crypt(key, message, 16).Reverse().AsLong().ToString("x"));

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
    }
}
