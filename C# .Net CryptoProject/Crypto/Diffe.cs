using System;
using Domain;


namespace Crypto
{
    public class Diffe
    {
        // prime ulong g
        // prime ulong q
        // ulong a
        // ulong b
       
        public static ulong InputPrime()
        {
            var passed = false;
            var input = "";
            ulong longinput = 0;
            
            
            while (passed != true)
            {
                input = Console.ReadLine();
                
                if (ulong.TryParse(input, out longinput))
                {
                    if (longinput != 0)
                    {
                        if (TestPrime(longinput))
                        {
                            passed = true;
                        }
                        else
                        {
                            Console.WriteLine("input should be a PRIME");
                        }
                    }
                    else
                    {
                        Console.WriteLine("0 is NOT a prime!");
                    }
                }

                else
                {
                    Console.WriteLine("Prime number should be an integer!");
                }
            } return longinput;
            
        }
        public static ulong InputInt()
        {
            var passed = false;
            var input = "";
            ulong longinput = 0;
            
            while (passed != true)
            {
                input = Console.ReadLine();
                
                if ((ulong.TryParse(input, out longinput)) & input!="0")
                {
                    passed = true;
                }

                else
                {
                    Console.WriteLine("The number should be an integer!");
                }
            } return longinput;
        }
        public static bool TestPrime(ulong number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (ulong)Math.Floor(Math.Sqrt(number));

            for (ulong i = 3; i <= boundary; i+=2)
                if (number % i == 0)
                    return false;

            return true;        
        }
        public static ulong ExpMod(ulong x, ulong y, ulong p) 
        {
            ulong res = 1;
            x = x % p;  
     
            if (x == 0) 
                return 0; 
  
            while (y > 0) 
            {
                if ((y & 1) == 1) 
                    res = (res * x) % p; 
                
                y = y >> 1;  
                x = (x * x) % p;  
            } 
            return res; 
        }

       
    }
}