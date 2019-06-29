using System;
using System.Net;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ConsoleApp1
{
    class Program
    {
        private static string HttpRequest(string Url)
        {
            Console.WriteLine(Url);
            string html = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            Console.WriteLine(html);
            return html;
        }
        static void ShowMongoInfo()
        {
            MongoClient dbClient = new MongoClient("mongodb://127.0.0.1:27017");

            //Database List  
            var dbList = dbClient.ListDatabases().ToList();

            Console.WriteLine("The list of databases are :");
            foreach (var item in dbList)
            {
                Console.WriteLine(item);
            }

            //Get Database and Collection  
            IMongoDatabase db = dbClient.GetDatabase("local");
            var collList = db.ListCollections().ToList();
            Console.WriteLine("The list of collections are :");
            foreach (var item in collList)
            {
                Console.WriteLine(item);
            }
        }
        static void CreateMongoItem(string html)
        {
            MongoClient dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = dbClient.GetDatabase("local");

            var httprequests = db.GetCollection<BsonDocument>("http_request_challenge");

            //CREATE  
            BsonDocument requestDoc = new BsonDocument();

            //Method 1  
            Random generator = new Random();
            String r = generator.Next(0, 999999).ToString("D6");
            BsonElement requestNumberElement = new BsonElement("RequestUniqueID", r);
            requestDoc.Add(requestNumberElement);

            //Method 2  
            requestDoc.Add(new BsonElement("RequestNumber", html));

            httprequests.InsertOne(requestDoc);

        }
        static void Main(string[] args)
        {
            Console.WriteLine("Sending Http request for google");
            string requestURL = "http://127.0.0.1:5000/";
            string html = HttpRequest(requestURL);
            //ShowMongoInfo();
            CreateMongoItem(html);
            System.Threading.Thread.Sleep(2000);
        }
    }
}
