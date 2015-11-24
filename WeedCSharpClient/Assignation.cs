using WeedCSharpClient.Net;

namespace WeedCSharpClient
{
    public class Assignation
    {
        public WeedFSFile WeedFSFile { get; set; }
        public Location Location { get; set; }
        private int _versionCount;

        public Assignation(AssignResult result)
        {
            WeedFSFile = result.GetWeedFSFile();
            Location = result.GetLocation();
            _versionCount = result.count;
        }

        public Assignation() { }

        public override string ToString()
        {
            return $"AssignedWeedFSFile [weedFSFile={WeedFSFile}, location={Location}, versionCount={_versionCount}]";
        }
    }
}
