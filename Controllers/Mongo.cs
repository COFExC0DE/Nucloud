using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace NuCloudWeb.Controllers {
    public class Mongo {
        private static Mongo instance = null;
        public MongoClient Client;
        public MongoDatabaseBase DB;
        public MongoCollectionBase<BsonDocument> Collection;

        public static Mongo Instance {
            get {
                if (instance == null)
                    instance = new Mongo();
                return instance;
            }
        }

        public Mongo() {
            Client = new MongoClient("mongodb+srv://nucloudtec:tec-nucloud@cluster0.vh5yq.mongodb.net/NuCloud?retryWrites=true&w=majority");
            DB = Client.GetDatabase("NuCloud") as MongoDatabaseBase;
            Collection = DB.GetCollection<BsonDocument>("Messages") as MongoCollectionBase<BsonDocument>;
        }

        public void InsertTest() {
            var data = new BsonDocument { { "type", "Petitoria" }, { "sender", "yo" }, { "date", "1/1/2021" }, { "text", "Esta es una petitoria" } };
            Collection.InsertOne(data);
        }
    }
}
