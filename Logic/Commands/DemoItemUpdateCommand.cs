using MediatR;
using System.Text.Json.Serialization;

namespace Logic.Commands
{
    public class DemoItemUpdateCommand(string name,decimal price) : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; } = name;
        public decimal Price { get; set; } = price;
    }
}
