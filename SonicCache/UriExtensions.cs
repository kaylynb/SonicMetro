using System;
using SonicUtil.Utility;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace SonicCache
{
    public static class UriExtensions
    {
        private static readonly HashAlgorithmProvider MD5Provider = HashAlgorithmProvider.OpenAlgorithm("MD5");
        public static string GetFileHash(this Uri uri)
        {
            ThrowIf.Null(uri, "uri");

            var buffer = CryptographicBuffer.ConvertStringToBinary(uri.AbsoluteUri, BinaryStringEncoding.Utf8);
            var hashed = MD5Provider.HashData(buffer);
            return CryptographicBuffer.EncodeToHexString(hashed);
        }
    }
}