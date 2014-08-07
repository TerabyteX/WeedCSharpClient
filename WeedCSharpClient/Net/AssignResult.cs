namespace WeedCSharpClient.Net
{
    public class AssignResult : Result
    {
        public int count;
        public string fid;
        public string publicUrl;
        public string url;

        public Location GetLocation()
        {
            return new Location
                    {
                        publicUrl = publicUrl,
                        url = url
                    };
        }

        public WeedFSFile GetWeedFSFile()
        {
            return new WeedFSFile(fid);
        }
    }
}
