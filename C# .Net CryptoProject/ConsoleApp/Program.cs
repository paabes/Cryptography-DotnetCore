using System;
using static Crypto.Diffe;
using static Crypto.RsaLib;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Boolean chosen = false;
            var input = String.Empty;

            while (chosen != true)
            {
                Console.WriteLine("*************************");
                Console.WriteLine("Choose a desirable cipher");
                Console.WriteLine("1 - Diffe-Hellman");
                Console.WriteLine("2 - RSA");
                Console.WriteLine("x - Quit");
                Console.WriteLine("*************************");
                input = Console.ReadLine()?.ToLower();

                if (input == "1")
                {
                    chosen = true;
                    DiffeHell();
                    chosen = false;

                }
                else if (input == "2")
                {
                    chosen = true;
                    RSA();
                    chosen = false;
                }

                static void DiffeHell()
                {
                    Console.WriteLine("______________________________________");
                    Console.WriteLine("So Diffe-Hellman it is!");
                    Console.WriteLine("Input some PRIME base number g");

                    var baseG = InputPrime();
                    Console.WriteLine("Input some PRIME number p");
                    var publicP = InputPrime();

                    Console.WriteLine("Input some random integer a");
                    var randomA = InputInt();
                    Console.WriteLine("Input some random integer b");
                    var randomB = InputInt();

                    Console.WriteLine("Your chosen base g: " + baseG);
                    Console.WriteLine("Your chosen p: " + publicP);
                    Console.WriteLine("Your chosen a: " + randomA);
                    Console.WriteLine("Your chosen b: " + randomB);


                    var X_secret = ExpMod(baseG, randomA, publicP);
                    var Y_secret = ExpMod(baseG, randomB, publicP);

                    var x_key = ExpMod(Y_secret, randomA, publicP);
                    var y_key = ExpMod(X_secret, randomB, publicP);

                    Console.WriteLine("key of person X:" + x_key);
                    Console.WriteLine("key of person Y:" + y_key);

                    if (x_key == y_key)
                    {
                        Console.WriteLine("Keys Match! Get this man a pass :)");
                    }
                }

                static void RSA()
                {
                    Console.WriteLine("______________________________________");
                    Console.WriteLine("So RSA it is!");
                    Console.WriteLine("Ecript or Bruteforce Decript?");
                    Console.WriteLine("1- Ecript");
                    Console.WriteLine("2- Bruteforce Decript");
                    Console.WriteLine("3- Standard Decript");
                    var gvn = false;
                    var nsgvn = false;
                    var opt2 = "";
                    ulong p = 5;
                    ulong q = 5;



                    while (gvn == false)
                    {
                        var opt = Console.ReadLine();

                        if (opt == "1")
                        {
                            Console.WriteLine("Enter primes or Generate Primes?");
                            Console.WriteLine("1- Enter primes");
                            Console.WriteLine("2- Generate Primes");

                            while (nsgvn == false)
                            {
                                opt2 = Console.ReadLine();
                                if (opt2 == "1")
                                {
                                    nsgvn = true;
                                    Console.WriteLine("Input some 1st PRIME p");
                                    var less = false;
                                    p = InputPrime();

                                    while (less == false)
                                    {
                                        Console.WriteLine("Input some 2nd PRIME number q");
                                        q = InputPrime();

                                        if ((ulong.MaxValue / p) > q)
                                        {
                                            less = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("q*p is too large, choose smaller q: ");
                                        }
                                    }
                                }
                                else if (opt2 == "2")
                                {
                                    nsgvn = true;

                                    var provided = false;
                                    while (provided == false)
                                    {
                                        Console.WriteLine("Enter a lower boundary of prime number");
                                        var low = InputInt();
                                        Console.WriteLine("Enter an upper boundary of prime number");
                                        var upper = InputInt();
                                        provided = true;

                                        p = PrimeGen(low, upper);

                                        var less = false;
                                        while (less == false)
                                        {
                                            q = PrimeGen(low, upper);

                                            if ((ulong.MaxValue / p) > q)
                                            {
                                                less = true;
                                            }
                                        }
                                    }


                                }
                                else
                                {
                                    Console.WriteLine("Not a valid Option!");
                                }
                            }

                            gvn = true;
                            Console.WriteLine("Enter plaintext you wish to encrypt");
                            var messagepln = Console.ReadLine();
                            if (!string.IsNullOrEmpty(messagepln))
                            {
                                var n = p * q;
                                var m = (p - 1) * (q - 1);
                                var e = Get_e(m);
                                var d = Get_d(m, e);

                                Console.WriteLine("Message: " + messagepln);
                                Console.WriteLine("Your Public Key n: " + n + " e: " + e);
                                Console.WriteLine("Your Private Key n: " + n + " d: " + d);
                                var cipher64 = RsaEncText(p, q, messagepln);
                                Console.WriteLine("ciphertext in Base64: " + cipher64);
                            }
                            else
                            {
                                Console.WriteLine("Plaintext cannot be null!");
                            }

                        }
                        else if (opt == "2")
                        {
                            gvn = true;
                            Console.WriteLine("for reduced computational time test with primes such that p*q<10 000");
                            Console.WriteLine("enter product of primes n: ");
                            var n = InputInt();
                            Console.WriteLine("enter e : ");
                            var e = InputInt();
                            Console.WriteLine("enter ciphertext in Base64: ");
                            var ciphertext = Console.ReadLine();

                            if (!string.IsNullOrEmpty(ciphertext))
                            {
                                var itis = IsBase64Encoded(ciphertext);
                                if (itis)
                                {
                                    var recovered = RsaBrute(n, e, ciphertext);
                                }
                                else
                                {
                                    Console.WriteLine("ciphertext does not seem to be in Base64, try again!");
                                }
                            }

                            else
                            {
                                Console.WriteLine("ciphertext cannot be null, try again!");
                            }


                        }
                        else if (opt == "3")
                        {
                            gvn = true;
                            Console.WriteLine("Enter value of 'n': ");
                            var n = InputInt();
                            Console.WriteLine("Enter value of 'd': ");
                            var d = InputInt();
                            Console.WriteLine("Enter enter ciphertext in Base64: ");
                            var ciphertext = Console.ReadLine();

                            if (!string.IsNullOrEmpty(ciphertext))
                            {
                                var itis = IsBase64Encoded(ciphertext);
                                if (itis)
                                {
                                    // Console.WriteLine("it is!");
                                    var plain = RsaDecText(n, d, ciphertext);
                                    Console.WriteLine("retrieved plaintext is: " + plain);

                                }
                                else
                                {
                                    Console.WriteLine("ciphertext does not seem to be in Base64, try again!");
                                }
                            }

                            else
                            {
                                Console.WriteLine("ciphertext cannot be null, try again!");
                            }

                        }
                        else
                        {
                            Console.WriteLine("This is NOT a valid option!");
                        }
                    }


                }
            }
        }
    }
}