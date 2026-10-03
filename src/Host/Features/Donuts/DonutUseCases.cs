using Host.Features.Donuts.Commands;
using Host.Features.Donuts.Queries;
using Host.Mediation;

namespace Host.Features.Donuts;

public record DonutUseCases(
    CreateDonutHandler Create,
    DonutsListHandler GetList
) : IUseCaseCollection;
