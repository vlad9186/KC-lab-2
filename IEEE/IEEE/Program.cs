using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEEE
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("input first number");
            string num1 = Console.ReadLine();
            Console.WriteLine("input second number");
            string num2 = Console.ReadLine();
            floatingPoint(num1, num2);
            Console.ReadKey();
        }
        public static void floatingPoint(string num1, string num2)
        {
            float first, second;
            first = float.Parse(num1);
            second = float.Parse(num2);

            long firstBits = GetFloatBits(first),
            secondBits = GetFloatBits(second);

            int sign1 = (int)((firstBits >> 31) & 1),
            sign2 = (int)((secondBits >> 31) & 1);

            int expo1 = (int)((firstBits >> 23) & 255),
            expo2 = (int)((secondBits >> 23) & 255);

            long mantissa1 = firstBits & ((int)Math.Pow(2, 23) - 1),
            mantissa2 = secondBits & ((int)Math.Pow(2, 23) - 1);
            Console.WriteLine("\nencoding the specified values in ieee 754:");
            Console.WriteLine("X: " + Convert.ToString(Convert.ToInt32(sign1), 2) + " " + Convert.ToString(Convert.ToInt32(expo1), 2) + " " + Convert.ToString(Convert.ToInt32(mantissa1), 2));
            Console.WriteLine("Y: " + Convert.ToString(Convert.ToInt32(sign2), 2) + " " + Convert.ToString(Convert.ToInt32(expo2), 2) + " " + Convert.ToString(Convert.ToInt32(mantissa2), 2) + " \n\n Align binary points: ");
            Console.WriteLine("is will need aditional bit: " + (mantissa1 + mantissa2 > Math.Pow(2, 23) && (first - Math.Abs(first) == 0) && (second - Math.Abs(second) == 0)) + "(pred Normalize result)");
            int new_expo;
            string new_mantisa1, new_mantisa2;
            if (mantissa1 + mantissa2 > Math.Pow(2, 23) && (first - Math.Abs(first) == 0) && (second - Math.Abs(second) == 0))
            {
                new_expo = Math.Max(expo1, expo2) + 1;
            }
            else
            {
                new_expo = Math.Max(expo1, expo2);
            }

            new_mantisa1 = Convert.ToString(Convert.ToInt32(mantissa1 >> new_expo - expo1), 2);
            new_mantisa2 = Convert.ToString(Convert.ToInt32(mantissa2 >> new_expo - expo2), 2);
            Console.WriteLine("max expo from x and y: " + Math.Max(expo1, expo2) + " \nnew expo: " + new_expo + " bin: " + Convert.ToString(Convert.ToInt32(new_expo), 2));
            new_mantisa1 = add_mant_to_23(new_mantisa1, new_expo - expo1 - 1);
            new_mantisa2 = add_mant_to_23(new_mantisa2, new_expo - expo2 - 1);

            Console.WriteLine("X: " + Convert.ToString(Convert.ToInt32(sign1), 2) + " " + Convert.ToString(Convert.ToInt32(new_expo), 2) + " " + new_mantisa1);
            Console.WriteLine("Y: " + Convert.ToString(Convert.ToInt32(sign2), 2) + " " + Convert.ToString(Convert.ToInt32(new_expo), 2) + " " + new_mantisa2);
            string sum_m1_m2 = add(new_mantisa1, new_mantisa2);
            int sum_man;
            sum_man = Convert.ToInt32(sum_m1_m2, 2);
            Console.WriteLine("\nAdd significands:\nnew mantisa: " + Convert.ToString(sum_man, 2));

            int resultMask = sum_man;
            resultMask |= new_expo << 23;
            resultMask |= sign1 << 31;

            byte[] bytes = new byte[4];
            bytes[0] = (byte)(resultMask & 255);
            bytes[1] = (byte)((resultMask >> 8) & 255);
            bytes[2] = (byte)((resultMask >> 16) & 255);
            bytes[3] = (byte)((resultMask >> 24) & 255);

            float result = BitConverter.ToSingle(bytes, 0);
            Console.WriteLine("Result: " + FinishStringWithZeros(Convert.ToString(resultMask, 2), 32) + " ( " + result + " ) ");


        }

        static string add_mant_to_23(string man, int p)
        {
            while (man.Length < 23)
            {

                man = '0' + man;
            }
            string res = string.Empty;
            for (int i = 0; i < 23; i++)
            {
                if (i == p)
                {
                    res += '1';
                }
                else
                {
                    res += man[i];
                }
            }

            return res;
        }

        static int GetFloatBits(float num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            int res = 0;
            res |= bytes[0];
            res |= bytes[1] << 8;
            res |= bytes[2] << 16;
            res |= bytes[3] << 24;
            return res;
        }

        static int[] toBinary(int number)
        {
            int n = number;
            var bin = Convert.ToString(n, 2);
            return bin.Select(c => int.Parse(c.ToString())).ToArray();
        }

        static string FinishStringWithZeros(string val, int bitcount)
        {
            int count = bitcount - val.Length;
            string head = "";
            for (int i = 0; i < count; ++i)
                head += "0";
            return head + val;
        }
        static string add(string X, string Y)
        {
            string res = string.Empty;
            int carryBit = 0;
            for (int i = 22; i > -1; i--)
            {
                int temporary = str_to_int(X[i]) + str_to_int(Y[i]) + carryBit;
                res = Convert.ToString(temporary % 2) + res;
                carryBit = temporary / 2;
            }
            return res;
        }
        static int str_to_int(char s)
        {
            if (s == '0')
                return 0;
            else
                return 1;
        }
    }
}
