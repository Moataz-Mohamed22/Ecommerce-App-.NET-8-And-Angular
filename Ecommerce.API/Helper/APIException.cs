namespace Ecommerce.API.Helper
{
    public class APIException : ResponseAPI
    {
        public APIException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }
        public string Details { get; set; }
    }
}
