using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgByta
{
    public class BoothAlgorithm

    {
        public int boothMultiply(int multiplicand, int multiplier)
        {
            int[] m = toBinary(multiplicand); // Multiplicand to binary
            int[] m2 = toBinary(-multiplicand); // 2's Complement of multiplicand
            int[] q = toBinary(multiplier); // Multiplier to binary
            int[] Addition = new int[17]; //Binary Array for positive Multiplicand used in addition
            int[] Subtraction = new int[17]; //Binary Array for negative Multiplicand used in addition
            int[] ProductReg = new int[17]; // Binary Array for product register with extra bit for initializing (17)

            // Fill Addition and Subtraction Arrays from the Left
            for (int i = 0; i < 16; i++)
            {
                Addition[i] = m[i];
                Subtraction[i] = m2[i];
            }

            // Fill the Product Register from the right
            for (int i = 8; i <= 16; i++)
            {
                ProductReg[i] = q[i - 8];
            }

            // Make sure to Initilize
            ProductReg[16] = 0;

            //Display the results

            steps(Addition, "A");
            steps(Subtraction, "S");
            steps(ProductReg, "P");
            Console.WriteLine(" ");

            // Checks the product register last two bits to choose which action for current step
            for (int i = 0; i < 8; i++)
            {
                if (ProductReg[15] == 0 && ProductReg[16] == 0)
                {
                    // 00 means do nothing
                }
                if (ProductReg[15] == 1 && ProductReg[16] == 1)
                {
                    // 11 means do nothing
                }
                if (ProductReg[15] == 1 && ProductReg[16] == 0)
                {
                    // 10 means to subtract (add negative multiplicand)
                    steps(Subtraction, "S");
                    add(ProductReg, Subtraction);

                }
                if (ProductReg[15] == 0 && ProductReg[16] == 1)
                {
                    // 01 means to add
                    steps(Addition, "A");
                    add(ProductReg, Addition);
                }
                // Shiftin over in each s
                shiftRight(ProductReg);
                steps(ProductReg, "P");
                Console.WriteLine();
            }

            // Converts the Product register to a string
            string original = string.Join("", ProductReg);

            // Checks the length to remove the initializer index
            if (original.Length >= 17)
            {
                string binaryResult = original.Remove(original.Length - 1);
                Console.WriteLine("Binary Result: " + binaryResult);
                return Convert.ToInt32(binaryResult, 2);
            }
            else
            {
                Console.WriteLine("Binary Result: " + original);
                return Convert.ToInt32(original, 2);
            }

        }

        // Shifts the Product register to the right
        public void shiftRight(int[] X)
        {
            // Shifts the product register to the right
            for (int i = 16; i >= 1; i--)
            {
                X[i] = X[i - 1];
            }
        }

        // Adds the Subtraction or Addition Register
        public void add(int[] X, int[] Y)
        {
            int carryBit = 0;
            for (int i = 8; i >= 0; i--)
            {
                int temporary = X[i] + Y[i] + carryBit;
                X[i] = temporary % 2;
                carryBit = temporary / 2;
            }

        }

        // Converts to binary
        public int[] toBinary(int number)
        {
            int n = number;

            // In C#, to String converts to base 2 and utilizes 2s complement
            // Comes in as 32 bits
            var bin = Convert.ToString(n, 2);

            // For negative numbers
            if (n < 0)
            {
                // Trims the extra 1's and adds 0 to index 16
                if (bin.Length == 32)
                {
                    bin = bin.Remove(1, 24);
                    bin = bin.PadRight(16, '0');
                }
                // converts to an array
                return bin.Select(c => int.Parse(c.ToString())).ToArray();
            }

            if (bin.Length == 32)
            {
                bin = bin.Remove(0, 8);
                // Shifts over to accomadate for it being on the right
                bin = bin.PadRight(16, '0');
                // converts to an array
                return bin.Select(c => int.Parse(c.ToString())).ToArray();
            }

            if (bin.Length < 8)
            {
                // If its lesss make it 8 bits long
                bin = bin.PadLeft(8, '0');
                // Shifts over to accomadate for it being on the right
                bin = bin.PadRight(16, '0');
                //// Converts string into an int array
                return bin.Select(c => int.Parse(c.ToString())).ToArray();
            }
            return bin.Select(c => int.Parse(c.ToString())).ToArray();
        }

        // Displays the steps
        public void steps(int[] X, string name)
        {
            Console.Write(name + ": ");

            for (int i = 0; i < X.Length; i++)
            {

                if (i == 8)
                {
                    Console.Write(" ");
                }

                if (i == 16)
                {
                    Console.Write(" ");
                }

                Console.Write(X[i]);
            }
            Console.WriteLine();

        }

        // MAIN 
        public static void Main(string[] args)
        {
            // Initialize variables to pass in
            int M = 0; // Multiplicand
            int Q = 0; // Multiplier
            int R = 0; // Result

            Console.WriteLine("Booth Algorithm");
            BoothAlgorithm ba = new BoothAlgorithm();
            Console.WriteLine("If input bin pres \"b\" if dex press any one other button ");
            if (Console.ReadLine() != "b")
            {
                // Have user input two integers for Multiplicand & Multiplier
                Console.WriteLine("\nPlease enter an Multiplier: ");
                Q = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(Convert.ToString(Q, 2));           
                Console.WriteLine("Please enter an Multiplicand: ");
                M = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(Convert.ToString(M, 2));
            }
            else
            {
                Console.WriteLine("Please enter an Multiplier: ");
                Q = Convert.ToInt32(Console.ReadLine(), 2);
                Console.WriteLine("Dec " + Q);
                Console.WriteLine("Please enter an Multiplicand: ");
                M = Convert.ToInt32(Console.ReadLine(), 2);
                Console.WriteLine("Dec " + M);
            }
            Console.WriteLine("Answer: " + M + "*" + Q);
            // Perform Booths
            R = ba.boothMultiply(M, Q);
            int decResult = M * Q;
            Console.WriteLine("Decimal Result: " + decResult + " Booth Algorithm Result " + R);
            Console.ReadLine();
        }
    }
}

