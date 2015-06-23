namespace sweet.framework.Infrastructure.Model
{
    public class ResultHandler
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public static ResultHandler Success()
        {
            var result = new ResultHandler { IsSuccess = true, Message = "操作成功" };

            return result;
        }

        public static ResultHandler<T> Success<T>(T result)
        {
            var handler = new ResultHandler<T> { IsSuccess = true, Message = "操作成功", Result = result };

            return handler;
        }

        public static ResultHandler Fail(string message)
        {
            var result = new ResultHandler { IsSuccess = false, Message = message };

            return result;
        }

        public static ResultHandler<T> Fail<T>(string message, T data = null)
            where T : class
        {
            var result = new ResultHandler<T> { IsSuccess = false, Message = message, Result = data };

            return result;
        }
    }

    public class ResultHandler<T> : ResultHandler
    {
        public T Result { get; set; }
    }
}