using System;

namespace WeedCSharpClient
{
    public class WeedFSFile
    {
        public readonly string Fid;
        public int Version = 0;

        public WeedFSFile(string fid)
        {
            Fid = fid;
        }

        public long GetVolumeId()
        {
            if (string.IsNullOrEmpty(Fid))
            {
                throw new ArgumentException("Fid cannot be empty", nameof(Fid));
            }

            var pos = Fid.IndexOf(',');
            if (pos == -1)
            {  
                throw new ArgumentException("Cannot parse fid: " + Fid, nameof(Fid));
            }

            var fidStr = Fid.Substring(0, pos);
            long fid;
            var parseResult = long.TryParse(fidStr, out fid);
            if (parseResult)
            {
                return fid;
            }

            throw new FormatException($"Cannot parse {fidStr} to long");
        }

        public override string ToString()
        {
            return $"WeedFSFile [fid={Fid}, version={Version}]";
        }
    }
}
