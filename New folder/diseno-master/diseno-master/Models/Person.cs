using System.Collections.Generic;

namespace NuCloudWeb.Models {
    public class Person {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<int> PhoneNumbers { get; set; }
    }
}