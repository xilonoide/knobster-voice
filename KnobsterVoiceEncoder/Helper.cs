//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace KnobsterVoiceEncoder
//{
//    public static class Base32
//    {
//        private static readonly char[] _digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();
//        private const int _mask = 31;
//        private const int _shift = 5;

//        private static int CharToInt(char c)
//        {
//            switch (c)
//            {
//                case 'A': return 0;
//                case 'B': return 1;
//                case 'C': return 2;
//                case 'D': return 3;
//                case 'E': return 4;
//                case 'F': return 5;
//                case 'G': return 6;
//                case 'H': return 7;
//                case 'I': return 8;
//                case 'J': return 9;
//                case 'K': return 10;
//                case 'L': return 11;
//                case 'M': return 12;
//                case 'N': return 13;
//                case 'O': return 14;
//                case 'P': return 15;
//                case 'Q': return 16;
//                case 'R': return 17;
//                case 'S': return 18;
//                case 'T': return 19;
//                case 'U': return 20;
//                case 'V': return 21;
//                case 'W': return 22;
//                case 'X': return 23;
//                case 'Y': return 24;
//                case 'Z': return 25;
//                case '2': return 26;
//                case '3': return 27;
//                case '4': return 28;
//                case '5': return 29;
//                case '6': return 30;
//                case '7': return 31;
//            }
//            return -1;
//        }

//        public static byte[] FromBase32String(string encoded)
//        {
//            if (encoded == null)
//                throw new ArgumentNullException(nameof(encoded));

//            // Remove whitespace and padding. Note: the padding is used as hint 
//            // to determine how many bits to decode from the last incomplete chunk
//            // Also, canonicalize to all upper case
//            encoded = encoded.Trim().TrimEnd('=').ToUpper();
//            if (encoded.Length == 0)
//                return new byte[0];

//            var outLength = encoded.Length * _shift / 8;
//            var result = new byte[outLength];
//            var buffer = 0;
//            var next = 0;
//            var bitsLeft = 0;
//            var charValue = 0;
//            foreach (var c in encoded)
//            {
//                charValue = CharToInt(c);
//                if (charValue < 0)
//                    throw new FormatException("Illegal character: `" + c + "`");

//                buffer <<= _shift;
//                buffer |= charValue & _mask;
//                bitsLeft += _shift;
//                if (bitsLeft >= 8)
//                {
//                    result[next++] = (byte)(buffer >> (bitsLeft - 8));
//                    bitsLeft -= 8;
//                }
//            }

//            return result;
//        }

//        public static string ToBase32String(byte[] data, bool padOutput = false)
//        {
//            return ToBase32String(data, 0, data.Length, padOutput);
//        }

//        public static string ToBase32String(byte[] data, int offset, int length, bool padOutput = false)
//        {
//            if (data == null)
//                throw new ArgumentNullException(nameof(data));

//            if (offset < 0)
//                throw new ArgumentOutOfRangeException(nameof(offset));

//            if (length < 0)
//                throw new ArgumentOutOfRangeException(nameof(length));

//            if ((offset + length) > data.Length)
//                throw new ArgumentOutOfRangeException();

//            if (length == 0)
//                return "";

//            // SHIFT is the number of bits per output character, so the length of the
//            // output is the length of the input multiplied by 8/SHIFT, rounded up.
//            // The computation below will fail, so don't do it.
//            if (length >= (1 << 28))
//                throw new ArgumentOutOfRangeException(nameof(data));

//            var outputLength = (length * 8 + _shift - 1) / _shift;
//            var result = new StringBuilder(outputLength);

//            var last = offset + length;
//            int buffer = data[offset++];
//            var bitsLeft = 8;
//            while (bitsLeft > 0 || offset < last)
//            {
//                if (bitsLeft < _shift)
//                {
//                    if (offset < last)
//                    {
//                        buffer <<= 8;
//                        buffer |= (data[offset++] & 0xff);
//                        bitsLeft += 8;
//                    }
//                    else
//                    {
//                        int pad = _shift - bitsLeft;
//                        buffer <<= pad;
//                        bitsLeft += pad;
//                    }
//                }
//                int index = _mask & (buffer >> (bitsLeft - _shift));
//                bitsLeft -= _shift;
//                result.Append(_digits[index]);
//            }
//            if (padOutput)
//            {
//                int padding = 8 - (result.Length % 8);
//                if (padding > 0) result.Append('=', padding == 8 ? 0 : padding);
//            }
//            return result.ToString();
//        }
//    }
//}

using System;
using System.Text;

static partial class Crockbase32
{
    const string Symbols = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

    public static string EncodeByteString(string input)
    {
        var sb = new StringBuilder();
        EncodeByteString(input, sb);
        return sb.ToString();
    }

    public static void EncodeByteString(string input, StringBuilder output)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        if (output == null) throw new ArgumentNullException(nameof(output));

        ushort bb = 0;
        var bits = 0;
        foreach (var ch in input)
            Encode(checked((byte)ch), ref bb, ref bits, output);
        Flush(bb, bits, output);
    }

    public static string Encode(byte[] buffer)
    {
        if (buffer == null) throw new ArgumentNullException(nameof(buffer));

        return Encode(buffer, 0, buffer.Length);
    }

    public static string Encode(byte[] buffer, int offset, int length)
    {
        var sb = new StringBuilder();
        Encode(buffer, offset, length, sb);
        return sb.ToString();
    }

    public static void Encode(byte[] buffer, StringBuilder output)
    {
        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (output == null) throw new ArgumentNullException(nameof(output));

        Encode(buffer, 0, buffer.Length, output);
    }

    public static void Encode(byte[] buffer, int offset, int length, StringBuilder output)
    {
        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
        if (offset + length > buffer.Length) throw new ArgumentOutOfRangeException();
        if (output == null) throw new ArgumentNullException(nameof(output));

        ushort bb = 0;
        var bits = 0;
        for (; length > 0; offset++, length--)
            Encode(buffer[offset], ref bb, ref bits, output);
        Flush(bb, bits, output);
    }

    static void Encode(byte b, ref ushort bb, ref int bits, StringBuilder output)
    {
        bb |= (ushort)(b << (8 - bits));
        bits += 8;
        for (; bits >= 5; bb <<= 5, bits -= 5)
            output.Append(Symbols[bb >> 11]);
    }

    static void Flush(ushort bb, int bits, StringBuilder output)
    {
        if (bits > 0)
            output.Append(Symbols[bb >> 11]);
    }

    static readonly sbyte[] ValueBySymbol =
    {
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
         0,  1,  2,  3,  4,  5,  6,  7,  8,  9, -1, -1, -1, -1, -1, -1, /*
             A   B   C   D   E   F   G   H   I   J   K   L   M   N   O   */
        -1, 10, 11, 12, 13, 14, 15, 16, 17,  1, 18, 19,  1, 20, 21,  0, /*
         P   Q   R   S   T   U   V   W   X   Y   Z                       */
        22, 23, 24, 25, 26, -1, 27, 28, 29, 30, 31, -1, -1, -1, -1, -1, /*
             a   b   c   d   e   f   g   h   i   j   k   l   m   n   o   */
        -1, 10, 11, 12, 13, 14, 15, 16, 17,  1, 18, 19,  1, 20, 21,  0, /*
         p   q   r   s   t   u   v   w   x   y   z                       */
        22, 23, 24, 25, 26, -1, 27, 28, 29, 30, 31, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
    };

    static readonly byte[] ZeroBytes = { };

    public static byte[] Decode(string input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        if (input.Length == 0)
            return ZeroBytes;

        var si = 0;
        var hyphens = 0;
        int i;
        while ((i = input.IndexOf('-', si)) >= 0)
        {
            hyphens++;
            si = i + 1;
        }
        var buffer = new byte[(input.Length - hyphens) * 5 / 8];
        Decode(input, buffer);
        return buffer;
    }

    static void Decode(string input, byte[] buffer, int offset = 0)
    {
        ushort bb = 0;
        var bits = 0;
        foreach (var ch in input)
        {
            if (ch == '-')
                continue;
            var b = ValueBySymbol[ch];
            if (b < 0)
                throw new FormatException($"'{ch}' is an invalid symbol.");
            bb |= (ushort)((ushort)b << (11 - bits));
            bits += 5;
            for (; bits >= 8; bits -= 8, bb <<= 8)
                buffer[offset++] = (byte)(bb >> 8);
        }
    }
}