using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace OrdersPortal
{
    public class Database
    {
        private static string connectionString = "mongodb://mongo-0-a:27017/crm?replicaSet=rs0";

        public async Task<List<Order>> GetAllOrders(Order filterData)
        {
            MongoClient client = new(connectionString);
            IMongoDatabase db = client.GetDatabase("crm");
            IMongoCollection<Order> col = db.GetCollection<Order>("orders");
            List<Order> dbOrders = new();
            ConventionRegistry.Register("IgnoreIfDefault", 
                new ConventionPack { new IgnoreIfDefaultConvention(true) }, 
                t => true);
            BsonDocument filter = filterData.ToBsonDocument();
            using (var cursor = await col.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var orders = cursor.Current;
                    foreach (Order doc in orders)
                    {
                        dbOrders.Add(doc);
                    }
                }
            }

            return dbOrders;
        }
        
        public async Task<List<Customer>> GetAllCustomers(Customer filterData)
        {
            MongoClient client = new(connectionString);
            IMongoDatabase db = client.GetDatabase("crm");
            IMongoCollection<Customer> col = db.GetCollection<Customer>("customers");
            ConventionRegistry.Register("IgnoreIfDefault", 
                new ConventionPack { new IgnoreIfDefaultConvention(true) }, 
                t => true);
            BsonDocument filter = filterData.ToBsonDocument();
            List<Customer> dbCustomers = new();
            using (var cursor = await col.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var orders = cursor.Current;
                    foreach (Customer doc in orders)
                    {
                        dbCustomers.Add(doc);
                    }
                }
            }

            return dbCustomers;
        }
        
        public async Task<List<StockProduct>> GetAllProducts(StockProduct filterData)
        {
            MongoClient client = new(connectionString);
            IMongoDatabase db = client.GetDatabase("crm");
            IMongoCollection<StockProduct> col = db.GetCollection<StockProduct>("products");
            ConventionRegistry.Register("IgnoreIfDefault", 
                new ConventionPack { new IgnoreIfDefaultConvention(true) }, 
                t => true);
            BsonDocument filter = filterData.ToBsonDocument();
            List<StockProduct> dbProducts = new();
            using (var cursor = await col.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var orders = cursor.Current;
                    foreach (StockProduct doc in orders)
                    {
                        dbProducts.Add(doc);
                    }
                }
            }

            return dbProducts;
        }
        
        public async void InsertCustomer(Customer newCustomer)
        {
            MongoClient client = new(connectionString);
            IMongoDatabase db = client.GetDatabase("crm");
            IMongoCollection<Customer> col = db.GetCollection<Customer>("customers");
            await col.InsertOneAsync(newCustomer);
        }
        
        public async void InsertOrder(Order newOrder)
        {
            MongoClient client = new(connectionString);
            IMongoDatabase db = client.GetDatabase("crm");
            IMongoCollection<Order> col = db.GetCollection<Order>("orders");
            await col.InsertOneAsync(newOrder);
        }
        
        public async void InsertProduct(StockProduct newProduct)
        {
            MongoClient client = new(connectionString);
            IMongoDatabase db = client.GetDatabase("crm");
            IMongoCollection<StockProduct> col = db.GetCollection<StockProduct>("products");
            await col.InsertOneAsync(newProduct);
        }
    }
}