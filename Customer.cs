using MongoDB.Bson;

namespace OrdersPortal
{
    public class Customer
    {
        public ObjectId? _id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int? TotalCost { get; set; }
        public int? TotalOrders { get; set; }
    }
}