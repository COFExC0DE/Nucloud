using System.Collections.Generic;

namespace NuCloudWeb.Models {
    public class Member : Person {
        public List<Group> Coaches { get; set; }
        public List<Node> Leads { get; set; }
        public Address Address { get; set; }
        public bool Coach { get; set; }
        public bool Active { get; set; }
    }
}