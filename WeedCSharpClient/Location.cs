namespace WeedCSharpClient
{
    public class Location
    {
        public string publicUrl { get; set; }
        public string url { get; set; }

        public override string ToString()
        {
            return $"Location [publicUrl={publicUrl}, url={url}]";
        }
    }
}
