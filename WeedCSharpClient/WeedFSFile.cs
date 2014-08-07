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
                throw new ArgumentException("Fid cannot be empty", "Fid");
            }

            int pos = Fid.IndexOf(',');
            if (pos == -1)
            {
                throw new ArgumentException("Cannot parse fid: " + Fid, "Fid");
            }

            var fidStr = Fid.Substring(0, pos);
            long fid;
            var parseResult = long.TryParse(fidStr, out fid);
            if (parseResult)
            {
                return fid;
            }
            else
            {
                throw new FormatException(string.Format("Cannot parse {0} to long", fidStr));
            }
        }

        public override string ToString()
        {
            return string.Format("WeedFSFile [fid={0}, version={1}]", Fid, Version);
        }
    }
}
