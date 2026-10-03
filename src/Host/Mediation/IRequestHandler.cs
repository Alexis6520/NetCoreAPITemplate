namespace Host.Mediation;

public interface IBaseRequestHandler;

public interface IRequestHandler<TRequest> : IBaseRequestHandler where TRequest : IRequest
{
    Task Execute(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRequestHandler<TRequest, TResponse> : IBaseRequestHandler where TRequest : IRequest<TResponse>
{
    Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default);
}
