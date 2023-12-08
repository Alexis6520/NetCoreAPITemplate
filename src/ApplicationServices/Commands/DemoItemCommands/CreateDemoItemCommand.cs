using MediatR;

namespace ApplicationServices.Commands.DemoItemCommands
{
    /// <summary>
    /// Comando para crear un artículo demo y obtener el Id generado
    /// </summary>
    public class CreateDemoItemCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
