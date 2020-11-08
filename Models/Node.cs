using System.Collections.Generic;

namespace NuCloudWeb.Models {
    public class Node {
        public int Cod { get; set; }
        public string Name { get; set; }
        public List<Member> Leaders { get; set; }
        public List<Member> Members { get; set; }
    }

    public class Zone : Node {
        public List<Branch> Children { get; set; }
    }

    public class Branch : Node {
        public List<Group> Children { get; set; }
    }
}
