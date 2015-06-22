namespace WeedCSharpClient.Net
{
    public class AssignResult : Result
    {
        public int count { get; set; }
        public string fid { get; set; }
        public string publicUrl { get; set; }
        public string url { get; set; }

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
