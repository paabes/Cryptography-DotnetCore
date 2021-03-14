using System;
using System.Text;
using static Crypto.Diffe;

namespace Crypto
{
    public static class RsaLib
    {
        public static ulong PrimeGen(ulong down, ulong up)
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
        public static bool TestPrime(ulong number)
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
        public static string RsaEncText(ulong p, ulong q, string plain)
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

        public static ulong Get_e(ulong m)
        {
            ulong e;
            for (e = 2; e < ulong.MaxValue; e++)
            {
                if (GCD(m, e) == 1) break;
            }
            return e;
        }

        public static ulong Get_d(ulong m, ulong e)
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
        public static byte[] Encode(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        public static string Decode(byte[] bytestream, Encoding encoding)
        {
            var decoded = encoding.GetString(bytestream);
            return decoded;
        }
        public static ulong GCD(ulong a, ulong b)
        {
            if (a == 0) return b;
            return GCD(b % a, a);
        }
        public static Tuple<ulong, ulong, string> RsaBrute(ulong N, ulong e, string cip)
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
        public static bool IsBase64Encoded(String str)
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
        public static string RsaDecText(ulong n, ulong d, string base64)
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
    }
}