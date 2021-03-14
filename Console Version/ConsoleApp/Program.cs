using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ConsoleApp
{

    class Program

    {
        static void Main(string[] args)
        {

            Boolean chosen = false;
            var input = "";



            while (chosen != true)
            {
                Console.WriteLine("*************************");
                Console.WriteLine("Choose a desirable cipher");
                Console.WriteLine("1 - Caesar cipher");
                Console.WriteLine("2 - Vigenere cipher");
                Console.WriteLine("3 - Diffe-Hellman");
                Console.WriteLine("4 - RSA");
                Console.WriteLine("x - Quit");
                Console.WriteLine("*************************");
                input = Console.ReadLine()?.ToLower(); /*  if isn't null, execute */

                if (input == "1")
                {
                    chosen = true;
                    caesar();
                    chosen = false;

                }
                else if (input == "2")
                {
                    chosen = true;
                    vigenere();
                    chosen = false;
                }
                else if (input == "3")
                {
                    chosen = true;
                    Diffe();
                    chosen = false;
                }
                
                else if (input == "4")
                {
                    chosen = true;
                    RSA();
                    chosen = false;
                }
                else if (input == "x")
                {
                    Console.WriteLine("Quitting :)");
                    chosen = true;
                    return;
                }
                else
                {
                    Console.WriteLine("This option is not allowed!");
                    Console.WriteLine("___________________________");
                }
            }




        }

        static void caesar()
        {
            Console.WriteLine("______________________________________");
            Console.WriteLine("So Caesar it is!");
            var option = "";
            Boolean quitt = false;
            Boolean provided = false;
            Boolean passed = false;
            var key = 0;

            while (passed != true)

            {


                Console.WriteLine("Encipt or Decript?");
                Console.WriteLine("e - Encript");
                Console.WriteLine("d - Decript");
                Console.WriteLine("x - quit");

                option = Console.ReadLine()?.ToLower().Trim();

                if (option == "e")
                {
                    passed = true;

                    // input, validate and save shift amount

                    var data = CaesKeyValidate();

                    key = data.Item1;
                    quitt = data.Item2;

                    // input and valid plaintext

                    if (quitt == false)
                    {

                        do
                        {
                            Console.Write("Enter your plaintext:  ");
                            var plaintext = Console.ReadLine();

                            if (plaintext != null && plaintext.Length != 0)
                            {
                                Console.WriteLine("the length of your plaintext is: " + plaintext.Length);
                                var bytestream = Encode(plaintext, Encoding.UTF8);
                                var ciphertext = caesarEnc(bytestream, (byte) key);
                                var base64 = System.Convert.ToBase64String(ciphertext);
                                Console.Write("your ciphertext is: ");

                                foreach (var cip in ciphertext)
                                {
                                    Console.Write(cip + " ");
                                }

                                Console.Write(" (spaces added for viusal aid)");
                                Console.WriteLine();
                                Console.WriteLine("Base 64: " + base64);

                                provided = true;
                            }
                            else
                            {
                                Console.WriteLine("plaintext cannot be null, try again!");
                            }
                        } while (provided != true);


                    }
                }

                else if (option == "d")
                {
                    passed = true;
                    var data = CaesKeyValidate();

                    key = data.Item1;
                    quitt = data.Item2;

                    if (quitt == false)
                    {

                        do
                        {

                            Console.WriteLine("Enter your ciphertext in Base64: ");
                            var ciphertext = Console.ReadLine();

                            if (ciphertext != null && ciphertext.Length != 0)
                            {
                                var itis = IsBase64Encoded(ciphertext);
                                if (itis)
                                {
                                    // Console.WriteLine("it is!");
                                    var cipher8 = Convert.FromBase64String(ciphertext);
                                    provided = true;
                                    var deciphered = caesarDec(cipher8, (byte) key);
                                    var plain = Decode(deciphered, Encoding.UTF8);

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
                        } while (provided != true);
                    }



                }

                else if (option == "x")
                {
                    passed = true;
                    return;
                }

                else
                {
                    passed = false;
                }
            }

        }


        static void vigenere()
        {
            Console.WriteLine("______________________________________");
            Console.WriteLine("So Vigenere it is!");
            var option = "";
            Boolean quitt = false;
            Boolean provided = false;
            Boolean passed = false;
            var keyword = "";

            while (passed != true)

            {


                Console.WriteLine("Encipt or Decript?");
                Console.WriteLine("e - Encript");
                Console.WriteLine("d - Decript");
                Console.WriteLine("x - quit");

                option = Console.ReadLine()?.ToLower().Trim();

                if (option == "e")
                {
                    passed = true;
                    var data = VigKeyValidate();

                    keyword = data.Item1;
                    quitt = data.Item2;

                    // input and valid plaintext

                    if (quitt == false)
                    {

                        do
                        {
                            Console.Write("Enter your plaintext:  ");
                            var plaintext = Console.ReadLine();

                            if (plaintext != null && plaintext.Length != 0)
                            {
                                Console.WriteLine("the length of your plaintext is: " + plaintext.Length);
                                var bytestream = Encode(plaintext, Encoding.UTF8);
                                var keystream = Encode(keyword, Encoding.UTF8);
                                var ciphertext = VigenereEnc(bytestream, keystream);
                                var base64 = System.Convert.ToBase64String(ciphertext);
                                Console.Write("your ciphertext is: ");

                                foreach (var cip in ciphertext)
                                {
                                    Console.Write(cip + " ");
                                }

                                Console.Write(" (spaces added for viusal aid)");
                                Console.WriteLine();
                                Console.WriteLine("Base 64: " + base64);

                                provided = true;
                            }
                            else
                            {
                                Console.WriteLine("plaintext cannot be null, try again!");
                            }
                        } while (provided != true);


                    }
                }

                else if (option == "d")
                {
                    passed = true;
                    var data = VigKeyValidate();

                    var key = data.Item1;
                    quitt = data.Item2;

                    if (quitt == false)
                    {

                        do
                        {

                            Console.WriteLine("Enter your ciphertext in Base64: ");
                            var ciphertext = Console.ReadLine();

                            if (ciphertext != null && ciphertext.Length != 0)
                            {
                                var itis = IsBase64Encoded(ciphertext);
                                if (itis)
                                {
                                    var cipher8 = Convert.FromBase64String(ciphertext);
                                    var key8 = Encode(key, Encoding.UTF8);
                                    var deciphered = VigenereDec(cipher8, key8);
                                    var plain = Decode(deciphered, Encoding.UTF8);
                                    Console.WriteLine("retrieved plaintext is: " + plain);
                                    provided = true;
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
                        } while (provided != true);
                    }



                }

                else if (option == "x")
                {
                    passed = true;
                    return;
                }

                else
                {
                    passed = false;
                }
            }
        }

        static byte[] VigenereEnc(byte[] inputbts, byte[] keybts)
        {
            var shifted = new byte[inputbts.Length];
            var scaled = new byte[inputbts.Length];
            var k = 0;
            var j = 0;

            if (keybts.Length < inputbts.Length)
            {
                while (scaled.Length < inputbts.Length)
                {
                    if (k <= keybts.Length & j < inputbts.Length)
                    {
                        scaled[j] = keybts[k];
                        k++;
                        j++;
                    }
                    else
                    {
                        k = 0;
                    }
                }
            }

            else
            {
                for (int i = 0; i < inputbts.Length; i++)
                {
                    scaled[i] = keybts[i];
                }
            }

            for (int i = 0; i < inputbts.Length; i++)
            {
                if (scaled[i] == 0)
                {
                    shifted[i] = inputbts[i];
                }
                else
                {
                    shifted[i] = (byte) ((inputbts[i] + scaled[i]) % 255);
                }
            }

            return shifted;
        }

        static byte[] VigenereDec(byte[] inputbts, byte[] keybts)
        {
            var shifted = new byte[inputbts.Length];
            var scaled = new byte[inputbts.Length];
            var k = 0;
            var j = 0;

            if (keybts.Length < inputbts.Length)
            {
                while (scaled.Length < inputbts.Length)
                {
                    if (k <= keybts.Length & j < inputbts.Length)
                    {
                        scaled[j] = keybts[k];
                        k++;
                        j++;
                    }
                    else
                    {
                        k = 0;
                    }
                }
            }

            else
            {
                for (int i = 0; i < inputbts.Length; i++)
                {
                    scaled[i] = keybts[i];
                }
            }

            for (int i = 0; i < inputbts.Length; i++)
            {
                if (scaled[i] == 0)
                {
                    shifted[i] = inputbts[i];
                }
                else
                {
                    shifted[i] = (byte) ((inputbts[i] - scaled[i]) % 255);
                }
            }

            return shifted;
        }

        static byte[] Encode(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        static string Decode(byte[] bytestream, Encoding encoding)
        {
            var decoded = encoding.GetString(bytestream);
            return decoded;
        }


        static byte[] caesarEnc(byte[] inputbts, byte shiftnum)
        {
            var shifted = new byte[inputbts.Length];

            if (shiftnum == 0)
            {
                for (int i = 0; i < inputbts.Length; i++)
                {
                    shifted[i] = inputbts[i];
                }
            }

            else
            {
                for (int i = 0; i < inputbts.Length; i++)
                {
                    shifted[i] = (byte) ((inputbts[i] + shiftnum) % 255); // doublecheck all cases jic
                }
            }



            return shifted;
        }

        static byte[] caesarDec(byte[] inputbts, byte shiftnum)
        {
            var decoded = new byte[inputbts.Length];

            if (shiftnum == 0)
            {
                for (int i = 0; i < inputbts.Length; i++)
                {
                    decoded[i] = inputbts[i];
                }
            }
            else
            {
                for (int i = 0; i < inputbts.Length; i++)
                {
                    decoded[i] = (byte) ((inputbts[i] - shiftnum) % 255);
                }
            }

            return decoded;
        }

        static Tuple<int, bool> CaesKeyValidate()
        {
            var given = false;
            var quitt = false;
            var key = 0;
            var amount = "";
            do
            {
                Console.WriteLine("______________________________________");
                Console.Write("Enter the shift amount, or X to quit:  ");
                amount = Console.ReadLine()?.ToLower().Trim(); /* removes spaces from front and back */

                if (amount != "x" && amount != null)
                {
                    if (int.TryParse(amount, out var shiftamount)
                    ) /* amount-string, shiftamount-int. tryparse statement is either true or false, if true returns converted int as shiftamount */
                    {
                        key = shiftamount % 255;

                        if (key == 0)
                        {
                            Console.WriteLine(
                                "shifting by 0 or multiples of +255/-255 is like trying to cover your face with your selife when robbing the bank");
                        }

                        else if (key == -1)
                        {
                            Console.WriteLine(
                                "shifting by -1, or, in general, by [-1 + n*(-255)] would do nothing, come on! it’s basic math!");
                        }

                        else
                        {
                            Console.WriteLine("Your Caesar key is: " + key);
                            given = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("shift amount must be an integer! try again");
                    }
                }
                else if (amount == "x")
                {
                    Console.WriteLine("Quitting..:)");
                    given = true;
                    quitt = true;

                }
                else
                {
                    given = false;
                }

            } while (given != true);


            var returns = Tuple.Create(key, quitt);
            return returns;
        }

        static Tuple<string, bool> VigKeyValidate()
        {
            var given = false;
            var quitt = false;
            var amount = "";
            do
            {
                Console.WriteLine("______________________________________");
                Console.Write("Enter the keyWORD, or X to quit:  ");
                amount = Console.ReadLine()?.ToLower().Trim(); /* removes spaces from front and back */

                if (amount != "x" && amount != null)
                {
                    given = true;
                }
                else if (amount == "x")
                {
                    Console.WriteLine("Quitting..:)");
                    given = true;
                    quitt = true;

                }
                else
                {
                    given = false;
                }

            } while (given != true);


            var returns = Tuple.Create(amount, quitt);
            return returns;
        }

        static bool IsBase64Encoded(String str)
        {
            try
            {
                // If no exception is caught, then it is possibly a base64 encoded string
                byte[] data = Convert.FromBase64String(str);
                // The part that checks if the string was properly padded to the
                return (str.Replace(" ", "").Length % 4 == 0);
            }
            catch
            {
                // If exception is caught, then it is not a base64 encoded string
                return false;
            }
        }

        static void Diffe()
        {
            Console.WriteLine("______________________________________");
            Console.WriteLine("So Diffe-Hellman it is!");
            Console.WriteLine("Input some PRIME base number g");

            var baseG = InputPrime();
            Console.WriteLine("Input some PRIME number p");
            var publicP = InputPrime();

            Console.WriteLine("Your base g: " + baseG);
            Console.WriteLine("Your prime p: " + publicP);

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

        static ulong InputPrime()
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
            }

            return longinput;

        }

        static bool IsPrime(ulong number) //takes forever with large numbers
        {
            bool IsPrime = true;
            ulong kappa = 2;

            while (kappa < number)
            {
                if (number % kappa == 0)
                {
                    IsPrime = false;
                    break;
                }
                else
                {
                    kappa++;
                }
            }

            return IsPrime;
        }

        static ulong InputInt()
        {
            var passed = false;
            var input = "";
            ulong longinput = 0;

            while (passed != true)
            {
                input = Console.ReadLine();

                if ((ulong.TryParse(input, out longinput)) & input != "0")
                {
                    passed = true;
                }

                else
                {
                    Console.WriteLine("The number should be an integer!");
                }
            }

            return longinput;
        }

        /*  static ulong ExpModd(ulong bas, ulong mod, ulong power)
          {
              ulong k = power/2;
              string str = "";
              string bin = "";
  
              while (power>=1)
              {
                  str = str + (power % 2).ToString();
                  power = k;
                  k = power / 2;
              }
  
              for (var i = str.Length - 1; i >= 0; i--)
              {
                  bin = bin + str[i];
              }
  
              char[] cArray = bin.ToCharArray();
              string reverse = String.Empty;
              ulong powerM = 1;
              
              for (int i = cArray.Length - 1; i > -1; i--)
              {
                  reverse += cArray[i];
              }
  
              foreach (var kar in reverse)
              {
                  
                  if (kar == '1')
                  {
                      powerM = (powerM * bas)%mod;
                      bas = (bas * bas)%mod;
                  }
                  
              }
  
              return powerM;
  
          } */

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

        static ulong GCD(ulong a, ulong b)
        {
            if (a == 0) return b;
            return GCD(b % a, a);
        }

        static ulong ExpMod(ulong x, ulong y, ulong p)
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

        static bool TestPrime(ulong number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (ulong) Math.Floor(Math.Sqrt(number));

            for (ulong i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

        static ulong PrimeGen(ulong down, ulong up)
        {
            var given = false;
            ulong prime = 1;

            while (given == false)
            {
                Random rd = new Random();
                int rand_num = rd.Next(2, 100);

                if (up - (ulong) rand_num > down)
                {
                    ulong num = down + (ulong) rand_num;
                    if (TestPrime(num))
                    {
                        prime = num;
                        given = true;
                    }
                }
            }

            return prime;
        }

        static Tuple<ulong, ulong, ulong, ulong, ulong, ulong> RsaEnc(ulong p, ulong q)
        {
            var n = p * q;
            var m = (p - 1) * (q - 1);
            ulong cipherr = 1;
            ulong plainMessagee = 1;

            Console.WriteLine("Your chosen p: " + p);
            Console.WriteLine("Your chosen q: " + q);
            Console.WriteLine("Your n: " + n);
            Console.WriteLine("Your m: " + m);

            ulong e;
            for (e = 2; e < ulong.MaxValue; e++)
            {
                if (GCD(m, e) == 1) break;
            }

            ulong d = 0;
            for (ulong k = 2; k < ulong.MaxValue; k++)
            {
                if ((1 + k * m) % e == 0)
                {
                    d = (1 + k * m) / e;
                    break;
                }
            }

            Console.WriteLine($"Public key ({n},{e})");
            Console.WriteLine($"Private key ({n},{d})");


            var provided = false;


            while (provided == false)
            {
                Console.WriteLine("enter message: ");
                Console.Write("Message:");
                var messageStr = Console.ReadLine();

                if ((ulong.TryParse(messageStr, out var message)) & message != 0)
                {
                    var cipher = ExpMod(message, e, n);
                    cipherr = cipher;
                    Console.WriteLine($"Cipher: {cipher}");


                    var plainMessage = ExpMod(cipher, d, n);
                    plainMessagee = plainMessage;
                    Console.WriteLine($"plainMessage: {plainMessage}");

                    Console.WriteLine("*******************");


                    // Bigint version for doublechecking

                    var cipher2 = BigInteger.ModPow(message, e, n);
                    Console.WriteLine($"Cipher Bigint: {cipher2}");

                    var plainMessage2 = BigInteger.ModPow(cipher2, d, n);
                    Console.WriteLine($"plain Bigint: {plainMessage2}");
                    provided = true;


                }
                else
                {
                    Console.WriteLine("message should be an integer!");
                }
            }


            var data = Tuple.Create(m, n, e, d, cipherr, plainMessagee);
            return data;

        }

        static Tuple<ulong, ulong, string> RsaBrute(ulong N, ulong e, string cip)
        {
            ulong p = 1;
            ulong q = 1;
            ulong n2 = N;
            ulong m = 1;
            if (N % 2 != 0)
            {
                n2 = N + 1;
            }

            n2 = n2 / 2;

            for (ulong x = 2; x <= n2; x++)
            {
                if (TestPrime(x))
                {
                    for (ulong z = 2; z <= n2; z++)
                    {
                        if (TestPrime(z))
                        {
                            if (x < ulong.MaxValue / z)
                            {
                                ulong n = x * z;

                                if (n == N)
                                {
                                    m = (x - 1) * (z - 1);
                                    if (GCD(m, e) == 1)
                                    {
                                        p = x;
                                        q = z;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("prime1 " + p);
            Console.WriteLine("prime2 " + q);

            ulong d = 0;
            for (ulong k = 2; k < ulong.MaxValue; k++)
            {
                if ((1 + k * m) % e == 0)
                {
                    d = (1 + k * m) / e;
                    break;
                }
            }

            var pln = RsaDecText(N, d, cip);
            Console.WriteLine("Recovered Message: " + pln);


            var data = Tuple.Create(p, q, pln);
            return data;

        }

        static ulong exponentMod(ulong A, ulong B, ulong C)
        {

            // Base cases  
            if (A == 0)
                return 0;
            if (B == 0)
                return 1;

            // If B is even  
            ulong y;
            if (B % 2 == 0)
            {
                y = exponentMod(A, B / 2, C);
                y = (y * y) % C;
            }

            // If B is odd  
            else
            {
                y = A % C;
                y = (y * exponentMod(A, B - 1,
                    C) % C) % C;
            }

            return ((y + C) % C);
        }

        static ulong power(ulong x, ulong y, ulong p)
        {
            // Initialize result 
            ulong res = 1;

            // Update x if it is more  
            // than or equal to p 
            x = x % p;

            if (x == 0)
                return 0;

            while (y > 0)
            {
                // If y is odd, multiply  
                // x with result 
                if ((y & 1) == 1)
                    res = (res * x) % p;

                // y must be even now 
                // y = y / 2 
                y = y >> 1;
                x = (x * x) % p;
            }

            return res;
        }

        static ulong ipow(ulong basse, ulong exp, ulong cc)
        {
            ulong result = 1;
            while (exp != 0)
            {
                if ((exp & 1) == 1)
                    result *= basse;
                exp >>= 1;
                basse *= basse;
            }

            return result % cc;
        }

        static string RsaEncText(ulong p, ulong q, string plain)
        {
            var bytestream = Encode(plain, Encoding.UTF8);
            var size = bytestream.Length;

            ulong[] array = new ulong[size];
            ulong[] arrayC = new ulong [size];

            // copy byte array to ulong array for encription

            for (int i = 0; i < size; i++)
            {
                array[i] = bytestream[i];
            }
            // generate keys

            var n = p * q;
            var m = (p - 1) * (q - 1);
            ulong e = Get_e(m);

            // encrypt each byte in ulong array

            for (ulong i = 0; i < (ulong) size; i++)
            {
                arrayC[i] = ExpMod(array[i], e, n);
            }

            // save encrypted ulong bytes as comma-separated values in string

            string gagu2 = String.Empty;

            for (int i = 0; i < size; i++)
            {
                gagu2 = gagu2 + arrayC[i] + ',';
            }

            // UTF-8 encode this string again to obtain byte array for Base64 conversion

            var bytestream2 = Encode(gagu2, Encoding.UTF8);
            var base64 = System.Convert.ToBase64String(bytestream2);

            return base64;
        }

        static string RsaDecText(ulong n, ulong d, string base64)
        {
            // decode from B64 to UTF-8 -> Byte[]

            var cipher8 = Convert.FromBase64String(base64);
            var dec_bytestream2 = Decode(cipher8, Encoding.UTF8);

            ulong[] cip_ulong = new ulong[dec_bytestream2.Length];
            string stringbuilder = String.Empty;
            var j = 0;

            // restore ulong array from comma separated string values

            for (int i = 0; i < dec_bytestream2.Length; i++)
            {
                if (dec_bytestream2[i] != ',')
                {
                    stringbuilder += dec_bytestream2[i];
                }
                else
                {
                    stringbuilder = stringbuilder.Trim();
                    cip_ulong[j] = ulong.Parse(stringbuilder);
                    j++;
                    stringbuilder = String.Empty;

                }
            }

            // decript every byte in ulong array

            ulong[] decripted = new ulong[cip_ulong.Length];

            for (int i = 0; i < cip_ulong.Length; i++)
            {
                decripted[i] = ExpMod(cip_ulong[i], d, n);

            }

            // restore initial encoded byte values and decode to get original plaintext
            byte[] decUTF8 = new byte[decripted.Length];
            for (int i = 0; i < decripted.Length; i++)
            {
                decUTF8[i] = (byte) decripted[i];
            }

            var decplain = Decode(decUTF8, Encoding.UTF8);
            return decplain;

        }

        static ulong Get_e(ulong m)
        {
            ulong e;
            for (e = 2; e < ulong.MaxValue; e++)
            {
                if (GCD(m, e) == 1) break;
            }
            return e;
        }

        static ulong Get_d(ulong m, ulong e)
        {
            ulong d = 0;
            for (ulong k = 2; k < ulong.MaxValue; k++)
            {
                if ((1 + k * m) % e == 0)
                {
                    d = (1 + k * m) / e;
                    break;
                }
            }
            return d;
        }




    }
}