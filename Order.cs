using System;
using MongoDB.Bson;

namespace OrdersPortal
{
    public class Order
    {
        public ObjectId? _id { get; set; }
        public int? Number { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }
        
        public string Comment { get; set; }
        
        public OrderedProduct[] Products { get; set; }

        public int? Total { get; set; }
        
        public string Status { get; set; }
    }
}