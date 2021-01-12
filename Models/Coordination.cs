using System.Collections.Generic;

namespace NuCloudWeb.Models {
    public class Coordination : Node {
        public Chief Chief { get; set; }
        public List<Zone> Children { get; set; }
    }
}