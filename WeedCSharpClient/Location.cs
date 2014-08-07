namespace WeedCSharpClient
{
    public class Location
    {
        public string publicUrl { get; set; }
        public string url { get; set; }

        public override string ToString()
        {
            return string.Format("Location [publicUrl={0}, url={1}]", publicUrl, url);
        }
    }
}
