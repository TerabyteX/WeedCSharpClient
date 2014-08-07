using System.Collections.Generic;

namespace WeedCSharpClient.Status
{
    public class Topology : AbstractNode
    {
        public List<DataCenter> DataCenters;
        public List<Layout> layouts;
        private Stats _stats;

        public int GetDataCenterCount()
        {
            if (_stats == null)
            {
                ComputeStats();
                return 0;
            }

            return _stats.DcCount;
        }

        public int GetRackCount()
        {
            if (_stats == null)
            {
                ComputeStats();
                return 0;
            }

            return _stats.RackCount;
        }

        public int GetDataNodeCount()
        {
            if (_stats == null)
            {
                ComputeStats();
                return 0;
            }

            return _stats.NodeCount;
        }

        public List<DataNode> GetDataNodes()
        {
            if (_stats == null)
            {
                ComputeStats();
                return new List<DataNode>();
            }

            return _stats.NodeList;
        }

        private void ComputeStats()
        {
            if (DataCenters == null)
            {
                return;
            }

            _stats = new Stats();
            foreach (var dc in DataCenters)
            {
                _stats.DcCount += 1;
                if (dc.Racks == null)
                {
                    continue;
                }
                foreach (var rack in dc.Racks)
                {
                    _stats.RackCount += 1;
                    if (rack.DataNodes == null)
                    {
                        continue;
                    }
                    _stats.NodeCount += rack.DataNodes.Count;
                    _stats.NodeList.AddRange(rack.DataNodes);
                }
            }
        }

        private class Stats
        {
            public int DcCount;
            public int RackCount;
            public int NodeCount;
            public List<DataNode> NodeList = new List<DataNode>();
        }
    }
}
