using BusinessObjects.Utils;

namespace BusinessObjects.DataTranferObjects
{
    public class ErrorResponseDTO
    {
        public string timestamp { get; set; }
        public string message { get; set; }
        public int errorCode { get; set; }
        public string error { get; set; }

        public ErrorResponseDTO()
        {
        }

        public ErrorResponseDTO(string message, int errorCode, string error)
        {
            this.timestamp = DateTimeUtil.getTimestampNow();
            this.message = message;
            this.errorCode = errorCode;
            this.error = error;
        }


    }
}
