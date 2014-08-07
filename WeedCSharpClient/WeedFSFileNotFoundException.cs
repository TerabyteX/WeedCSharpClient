namespace WeedCSharpClient
{
    public class WeedFSFileNotFoundException : WeedFSException
    {
        private static readonly long _serialVersionUid = 1L;

        public WeedFSFileNotFoundException(WeedFSFile file, Location location)
            : base(file.Fid + " not found on " + location.publicUrl) { }
    }
}
