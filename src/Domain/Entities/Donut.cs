namespace Domain.Entities
{
    /// <summary>
    /// Representa una donita del Kripy Kreme :)
    /// </summary>
    public class Donut
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
