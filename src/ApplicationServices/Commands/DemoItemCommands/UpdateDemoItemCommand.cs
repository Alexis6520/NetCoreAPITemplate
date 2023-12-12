using MediatR;
using System.Text.Json.Serialization;

namespace ApplicationServices.Commands.DemoItemCommands
{
    public class UpdateDemoItemCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
