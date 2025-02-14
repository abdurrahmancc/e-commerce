namespace e_commerce.Utilities
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string   Message { get; set; } = string.Empty;
        public T Data { get; set; }
        public List<string> Errors  { get; set; }
        public int StatusCode { get; set; }
        public DateTime TimeStamp { get; set; }



        //constructor response
        private ApiResponse(bool success, string message, T data, List<string> errors, int statusCode ) { 
            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
            StatusCode = statusCode;
            TimeStamp = DateTime.Now;
        }

        // static method for creating a successful response
        public static ApiResponse<T> SuccessResponse(T data, int statusCode, string message = "")
        {
            return new ApiResponse<T>(true, message, data, null, statusCode);
        }

        // static method for creating a Error response
        public static ApiResponse<T> ErrorResponse(List<string> error, int statusCode, string message = "")
        {
            return new ApiResponse<T>(false, message, default, error, statusCode);
        }

    }
}
