using System.Net;

namespace SDVDaily.Models
{
    public class ResponseViewModel<T>
    {
        public string? message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public T? data { get; set; }

        public ResponseViewModel()
        {
            statusCode = HttpStatusCode.InternalServerError;
            message = string.Empty;
            data = default(T);
        }
    }
}
