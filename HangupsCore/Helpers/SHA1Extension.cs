using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace HangupsCore.Helpers
{
    public static class SHA1Extension
    {
        public static string ComputeSHA1Hash(this string input)
        {
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider provider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);

            //hash it
            IBuffer hash = provider.HashData(buffer);
            return CryptographicBuffer.EncodeToHexString(hash).ToUpperInvariant();
        }
    }
}
