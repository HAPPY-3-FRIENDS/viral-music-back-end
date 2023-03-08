namespace BusinessObjects.DataTranferObjects
{
    public class ResponseDTO<T> where T : class
    {
        public string message { get; set; }
        public int status { get; set; }
        public T data { get; set; }

        public ResponseDTO(string message, int status, T data)
        {
            this.message = message;
            this.status = status;
            this.data = data;
        }
    }
}
