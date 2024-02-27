namespace Domain
{
    public class DemoItem(string name, decimal price)
    {
        public int Id { get; set; }
        public string Name { get; set; } = name;
        public decimal Price { get; set; } = price;
    }
}
