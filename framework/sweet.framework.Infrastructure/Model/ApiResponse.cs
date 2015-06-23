namespace sweet.framework.Infrastructure.Model
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        protected ApiResponse()
        {
        }

        public static ApiResponse Success()
        {
            var result = new ApiResponse { IsSuccess = true, Message = "操作成功" };

            return result;
        }

        public static ApiResponse<T> Success<T>(T data)
        {
            var result = new ApiResponse<T> { IsSuccess = true, Message = "操作成功", Data = data };

            return result;
        }

        public static ApiResponse Fail(string message)
        {
            var result = new ApiResponse { IsSuccess = false, Message = message };

            return result;
        }

        public static ApiResponse<T> Fail<T>(string message, T data = null)
            where T : class
        {
            var result = new ApiResponse<T> { IsSuccess = false, Message = message, Data = data };

            return result;
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }

        internal ApiResponse()
        {
        }
    }
}