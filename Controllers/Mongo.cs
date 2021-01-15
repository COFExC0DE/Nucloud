using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NuCloudWeb.Models;
using MongoDB.Driver.Linq;

namespace NuCloudWeb.Controllers {
    public class Mongo {
        private static Mongo instance = null;
        public MongoClient Client;
        public MongoDatabaseBase DB;
        public MongoCollectionBase<Message> Messages;
        public MongoCollectionBase<News> News;

        public static Mongo Instance {
            get {
                if (instance == null)
                    instance = new Mongo();
                return instance;
            }
        }

        public void Set() {
            Client = new MongoClient("mongodb+srv://nucloudtec:tec-nucloud@cluster0.vh5yq.mongodb.net/NuCloud?retryWrites=true&w=majority");
            DB = Client.GetDatabase("NuCloud") as MongoDatabaseBase;
            News = DB.GetCollection<News>("News") as MongoCollectionBase<News>;
            Messages = DB.GetCollection<Message>("Messages") as MongoCollectionBase<Message>;
        }

        public void InsertMessage(Message m) {
            Messages.InsertOneAsync(m);
        }

        public void InsertNews(string title, string date, string body, string sender) {
            News.InsertOne(new News() {
                Title = title,
                Body = body,
                Sender = sender,
                Date = date
            });
        }

        public List<Message> GetMessages() {
            var messages = Messages.AsQueryable<Message>().ToList();
            return messages;
        }

        public List<Message> GetMessagesType(string type) {
            var messages = Messages.Find(x => x.Type == type).ToList();
            return messages;
        }

        public void ClearInbox() {
            Messages.DeleteMany(x => x.Id != null);
        }

        public int GetMessageCount(string type) {
            var messages = Messages.Find(x => x.Type == type).ToList();
            return messages.Count;
        }

        public Message GetMessage(string id) {
            var msg = Messages.Find(x => x.Id == id).ToList().First();
            return msg;
        }

        public List<News> GetNews() {
            var news = News.AsQueryable<News>().ToList();
            return news;
        }

    }
}
