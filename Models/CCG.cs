using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NuCloudWeb.Models {
    [BsonIgnoreExtraElements]
    public abstract class CCG {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("date")]
        public string Date { get; set; }
        [BsonElement("sender")]
        public string Sender { get; set; }
        [BsonElement("body")]
        public string Body { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Message : CCG {
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("subject")]
        public string Subject { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class News : CCG {
        [BsonElement("title")]
        public string Title { get; set; }
    }
}
