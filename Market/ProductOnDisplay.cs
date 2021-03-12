namespace Market
{
    public class ProductOnDisplay : Product
    {
        public int Price { get; set; }
        public int Quantity { get; set; }

        public ProductOnDisplay(string name, int volume, int id, int price, int quantity) : base(name, volume, id)
        {
            Price = price;
            Quantity = quantity;
        }
    }
}