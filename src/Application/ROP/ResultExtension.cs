namespace Application.ROP
{
    public static class ResultExtension
    {
        /// <summary>
        /// Encadena una función que devuelve un resultado de 2 vías
        /// </summary>
        /// <typeparam name="TIn">Tipo de entrada</typeparam>
        /// <typeparam name="TOut">Tipo de salida</typeparam>
        /// <param name="result">Resultado anterior</param>
        /// <param name="func">Función a encadenar</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static Result<TOut> Bind<TIn,TOut>(
            this Result<TIn> result,
            Func<TIn, Result<TOut>> func)
        {
            if (!result.Succeeded) return Result<TOut>.Failure(result.Errors);

            if (result.Value == null)
            {
                throw new NullReferenceException("El valor no puede ser nulo");
            }

            return func(result.Value);
        }

        /// <summary>
        /// Mapea una función que no devuelve errores de dominio
        /// </summary>
        /// <typeparam name="TIn">Tipo de entrada</typeparam>
        /// <typeparam name="TOut">Tipo de salida</typeparam>
        /// <param name="result">Resultado anterior</param>
        /// <param name="func">Función a encadenar</param>
        /// <returns></returns>
        public static Result<TOut> Map<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, TOut> func)
        {
            return result.Bind(value => Result<TOut>.Success(func(value))); 
        }

        /// <summary>
        /// Encadena un método que no devuelve nada
        /// </summary>
        /// <typeparam name="TIn">Tipo de entrada</typeparam>
        /// <param name="result">Resultado anterior</param>
        /// <param name="action">Accíon a ejecutar</param>
        /// <returns></returns>
        public static Result<TIn> Then<TIn>(
            this Result<TIn> result,
            Action<TIn> action)
        {
            return result.Bind(value =>
            {
                action(value);
                return result;
            });
        }

        /// <summary>
        /// Encadena una función que devuelve un resultado de 2 vías de forma asíncrona
        /// </summary>
        /// <typeparam name="TIn">Tipo de entrada</typeparam>
        /// <typeparam name="TOut">Tipo de salida</typeparam>
        /// <param name="resultTask">Resultado anterior</param>
        /// <param name="func">Función a encadenar</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task<Result<TOut>> BindAsync<TIn, TOut>(
            this Task<Result<TIn>> resultTask,
            Func<TIn, Task<Result<TOut>>> func)
        {
            Result<TIn> result = await resultTask;

            if (!result.Succeeded) return Result<TOut>.Failure(result.Errors);

            if (result.Value == null)
            {
                throw new NullReferenceException("El valor no puede ser nulo");
            }

            return await func(result.Value);
        }

        /// <summary>
        /// Mapea una función que no devuelve errores de dominio de forma asíncrona
        /// </summary>
        /// <typeparam name="TIn">Tipo de entrada</typeparam>
        /// <typeparam name="TOut">Tipo de salida</typeparam>
        /// <param name="resultTask">Resultado anterior</param>
        /// <param name="func">Función a encadenar</param>
        /// <returns></returns>
        public static async Task<Result<TOut>> MapAsync<TIn, TOut>(
            this Task<Result<TIn>> resultTask,
            Func<TIn, Task<TOut>> func)
        {
            return await resultTask.BindAsync(async value => Result<TOut>.Success(await func(value)));
        }

        /// <summary>
        /// Encadena un método que no devuelve nada de forma asíncrona
        /// </summary>
        /// <typeparam name="TIn">Tipo de entrada</typeparam>
        /// <param name="resultTask">Resultado anterior</param>
        /// <param name="action">Accíon a ejecutar</param>
        /// <returns></returns>
        public static async Task<Result<TIn>> ThenAsync<TIn>(
            this Task<Result<TIn>> resultTask,
            Func<TIn, Task> action)
        {
            return await resultTask.BindAsync(async value =>
            {
                await action(value);
                return Result<TIn>.Success(value);
            });
        }
    }
}
