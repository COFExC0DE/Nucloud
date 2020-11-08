using System.Collections.Generic;

namespace NuCloudWeb.Models {
    public class Cloud {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public Address Address { get; set; }
        public int PhoneNumber { get; set; }
        public string Image { get; set; }
        public Coordination Coordination { get; set; }
    }
}
