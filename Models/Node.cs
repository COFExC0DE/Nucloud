using System.Collections.Generic;

namespace NuCloudWeb.Models {
    public class Node {
        public int Cod { get; set; }
        public string Name { get; set; }
        public List<Node> Children { get; set; }
        public List<Member> Leaders { get; set; }
        public List<Member> Members { get; set; }
    }
}
