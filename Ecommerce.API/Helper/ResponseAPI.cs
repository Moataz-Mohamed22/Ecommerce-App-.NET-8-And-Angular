namespace Ecommerce.API.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statusCode, string message=null) {
            StatusCode = statusCode;
            Message = message??GetMessageFromStatusCode(statusCode);
        }
        private string? GetMessageFromStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                201 => "Created",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                409 => "Conflict",
                500 => "Internal Server Error",
                _ => null
            };
        }
        public int StatusCode { get;  }
        public string? Message { get; }
    }
}
