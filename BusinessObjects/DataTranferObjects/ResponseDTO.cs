using System.Net;

namespace BusinessObjects.DataTranferObjects
{
    public class ResponseDTO<T>
    {
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public T? data { get; set; }

        public ResponseDTO(string message, HttpStatusCode status, T? data)
        {
            this.message = message;
            this.statusCode = status;
            this.data = data;
        }
    }
}
