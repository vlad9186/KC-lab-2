using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DividionRightShift
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("input first number");
            string num1 = Console.ReadLine();
            Console.WriteLine("input second number");
            string num2 = Console.ReadLine();
            division(num1, num2);
            Console.ReadKey();
        }
        public static void division(string num1, string num2)
        {
            Int64 divisor, remainderAndQuotient;
            try
            {
                divisor = Int32.Parse(num1);
                remainderAndQuotient = Int32.Parse(num2);
            }
            catch
            {
                Console.WriteLine("Passed arguments are not int32 numbers.");
                return;
            }
            divisor <<= 32;

            // --- // --- // --- // --- // --- // --- // --- // --- // --- // --- // --- // --- //

            bool setRemLSBToOne = false;
            for (int i = 0; i <= 32; ++i)
            {
                Console.WriteLine("Step #" + (i + 1) + ":\n");

                Console.Write("Divisor is ");
                if (divisor <= remainderAndQuotient)
                {
                    remainderAndQuotient -= divisor;
                    setRemLSBToOne = true;
                    Console.Write("less");
                }
                else
                    Console.Write("more");

                Console.WriteLine(" than remainder.");
                Console.WriteLine("Shift remainder left one bit.");

                remainderAndQuotient <<= 1;

                if (setRemLSBToOne)
                {
                    setRemLSBToOne = false;
                    remainderAndQuotient |= 1; //lsb - 1
                    Console.WriteLine("Set remainder lsb to 1");
                }
                Console.WriteLine();

                Console.WriteLine("Divisor:\n" + FinishStringWithZeros(Convert.ToString(divisor, 2)) +
                "\nRemainder and quotient:\n" + FinishStringWithZeros(Convert.ToString(remainderAndQuotient, 2)) + "\n");
            }
            long quotient = remainderAndQuotient & ((long)Math.Pow(2, 33) - 1);
            long remainder = remainderAndQuotient >> 33;
            Console.WriteLine("Quotient:\n" + FinishStringWithZeros(Convert.ToString(quotient, 2)) +
            " ( " + quotient + " )\n");

            Console.WriteLine("Remainder:\n" + FinishStringWithZeros(Convert.ToString(remainder, 2)) +
            " ( " + remainder + " )");
        }
        static string FinishStringWithZeros(string val)
        {
            int count = 64 - val.Length;
            string head = "";
            for (int i = 0; i < count; ++i)
                head += "0";
            return head + val;
        }
    }

}
