using SecurityLogin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.Idempotent
{
    public interface IIdempotentKeyGenerator
    {
        string GetKey(string header, object[] args);
    }
    
    public class DefaultIdempotentKeyGenerator : IIdempotentKeyGenerator
    {
        public static readonly DefaultIdempotentKeyGenerator Instance = new DefaultIdempotentKeyGenerator();

        private DefaultIdempotentKeyGenerator() { }

        public string GetKey(string header, object[] args)
        {
            return KeyGenerator.Concat(header, args);
        }
    }
}
