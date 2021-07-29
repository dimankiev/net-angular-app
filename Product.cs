using MongoDB.Bson;

namespace OrdersPortal
{
    public class Product
    {
        public ObjectId? _id { get; set; }
        public int? ProductId { get; set; }
        
        public string Name { get; set; }
        
        public string Category { get; set; }
        
        public string Size { get; set; }
        
        public int Price { get; set; }
    }

    public class OrderedProduct : Product
    {
        private int quantity { get; set; }
        public int Quantity
        {
            get => quantity;
            set
            {
                if (value < 0) quantity = 0;
                else quantity = value;
            }
        }
    }

    public class StockProduct : Product
    {
        private int stock { get; set; }
        public int Stock
        {
            get => stock;
            set
            {
                if (value < 0) stock = 0;
                else stock = value;
            }
        }
    }
}