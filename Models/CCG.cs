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
        public string Date;
        [BsonElement("sender")]
        public string Sender;
        [BsonElement("body")]
        public string Body;
    }

    [BsonIgnoreExtraElements]
    public class Message : CCG {
        [BsonElement("type")]
        public string Type;
        [BsonElement("subject")]
        public string Subject;
    }

    [BsonIgnoreExtraElements]
    public class News : CCG {
        [BsonElement("title")]
        public string Title;
    }
}
