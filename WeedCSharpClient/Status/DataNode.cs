namespace WeedCSharpClient.Status
{
    public class DataNode : AbstractNode
    {
        public string PublicUrl;
        public string Url;
        public int Volumes;

        public Location AsLocation()
        {
            var location = new Location
                        {
                            publicUrl = PublicUrl,
                            url = Url
                        };
            return location;
        }
    }
}
