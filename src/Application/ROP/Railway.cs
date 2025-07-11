namespace Application.ROP
{
    public static class Railway
    {
        public static async Task<Result<TOut>> Bind<TIn, TOut>(
            this Task<Result<TIn>> currentTask,
            Func<TIn, Task<Result<TOut>>> nextTask)
        {
            Result<TIn> currentResult = await currentTask;

            if (!currentResult.Succeeded)
                return Result<TOut>.Failure(currentResult.StatusCode, currentResult.Errors);

            if (currentResult.Value is null)
            {
                throw new Exception("El valor de un resultado exitoso no puede se nulo");
            }

            return await nextTask(currentResult.Value);
        }

        public static async Task<Result<TOut>> Map<TIn, TOut>(
            this Task<Result<TIn>> currentTask,
            Func<TIn, Task<TOut>> nextTask)
        {
            return await currentTask.Bind(async x => Result<TOut>.Success(await nextTask(x)));
        }

        public static async Task<Result<TIn>> Then<TIn>(
            this Task<Result<TIn>> currentTask,
            Func<TIn, Task> nextTask)
        {
            return await currentTask.Bind(async x =>
            {
                await nextTask(x);
                return Result<TIn>.Success(x);
            });
        }
    }
}
