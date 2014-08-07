using System;
using System.IO;

namespace WeedCSharpClient
{
    public class WeedFSException : IOException
    {
        private static readonly long _serialVersionUid = 1L;

        public WeedFSException(string reason) : base(reason) { }

        public WeedFSException(Exception cause) : base(string.Empty, cause) { }

        public WeedFSException(string reason, Exception cause) : base(reason, cause) { }
    }
}
