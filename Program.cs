using System;
using System.Numerics;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TI3
{
    public static class Program
    {
        private static (BigInteger, BigInteger, BigInteger) ExtGcd(BigInteger a, BigInteger b)
        {
            if (a < b)
            {
                BigInteger tmp = b;
                b = a;
                a = tmp;
            }

            if (b.IsZero)
                return (a, 1, 0);
            
            BigInteger x2 = 1,
                       x1 = 0,
                       y2 = 0,
                       y1 = 1,
                       x, y, q, r;

            while (b.Sign > 0)
            {
                q = a / b;
                r = a - q * b;
                x = x2 - q * x1;
                y = y2 - q * y1;

                a = b;
                b = r;
                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 = y;
            }

            return (a, x2, y2);
        }

        private static BigInteger fastexp(BigInteger a, BigInteger z, BigInteger m)
        {
            BigInteger x = 1;

            for (; !z.IsZero; --z)
            {
                while (z.IsEven)
                {
                    z /= 2;
                    
                    a = (a * a) % m;
                }

                x = (x * a) % m;
            }

            return x;
        }

        private static void GenKey()
        {
            Console.WriteLine("Enter p, q and e");

            BigInteger p, q, e;
            p = BigInteger.Parse(Console.ReadLine());
            q = BigInteger.Parse(Console.ReadLine());
            e = BigInteger.Parse(Console.ReadLine());

            BigInteger n = p * q;
            BigInteger phi = (p - 1) * (q - 1);

            var res = ExtGcd(e, phi);

            BigInteger d = res.Item3;
            if (d.Sign < 0)
                d = res.Item2;

            Console.WriteLine($"The public key is  ({d}, {n})");
            Console.WriteLine($"The private key is ({e}, {n})");
        }

        private static BigInteger LetterAsNum(char c)
        {
            return c - 'а' + 1;
        }

        private static BigInteger Hash(string s, BigInteger n)
        {
            BigInteger hash = 100;

            s = s.ToLower();

            for (int i = 0; i < s.Length; ++i)
            {
                hash = fastexp(hash + LetterAsNum(s[i]), 2, n);
            }

            return hash;
        }

        private static void SignMessage()
        {
            Console.WriteLine("Enter e:");
            var e = BigInteger.Parse(Console.ReadLine());

            Console.WriteLine("Enter n:");
            var n = BigInteger.Parse(Console.ReadLine());

            Console.WriteLine("Enter the message:");
            string message = Console.ReadLine();

            BigInteger hash = Hash(message, n);

            Console.WriteLine("The hash is " + hash);

            BigInteger signature = fastexp(hash, e, n);

            Console.WriteLine("The signature is " + signature);
        }

        private static void CheckMessage()
        {
            Console.WriteLine("Enter d:");
            var d = BigInteger.Parse(Console.ReadLine());

            Console.WriteLine("Enter n:");
            var n = BigInteger.Parse(Console.ReadLine());

            Console.WriteLine("Enter the message:");
            string message = Console.ReadLine();

            Console.WriteLine("Enter the signature:");
            var signature = BigInteger.Parse(Console.ReadLine());

            BigInteger computedHash = Hash(message, n);

            Console.WriteLine("The computed hash is " + computedHash);

            BigInteger hash = fastexp(signature, d, n);

            Console.WriteLine("The correct hash is " + hash);

            if (hash.Equals(computedHash))
                Console.WriteLine("The message is valid");
            else
                Console.WriteLine("The message is invalid");
        }

        public static void Main()
        {
            while(true)
            {
                Console.WriteLine("1) Generate key");
                Console.WriteLine("2) Sign message");
                Console.WriteLine("3) Check message");

                string s = Console.ReadLine();

                if (s.Contains('1'))
                    GenKey();
                else if (s.Contains('2'))
                    SignMessage();
                else if (s.Contains('3'))
                    CheckMessage();
                else
                {
                    Console.WriteLine("Invalid command");
                    Console.WriteLine("Please try again");
                }
            }
        }
    }
}
