namespace Market
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }

        public Product (string name, int volume, int id)
        {
            Id = id;
            Name = name;
            Volume = volume;
        }
    }
}