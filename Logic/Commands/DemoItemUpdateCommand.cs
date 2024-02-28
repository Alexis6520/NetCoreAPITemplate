using MediatR;
using System.Text.Json.Serialization;

namespace Logic.Commands
{
    public class DemoItemUpdateCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
