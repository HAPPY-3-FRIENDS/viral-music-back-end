using BusinessObjects.DataTranferObjects;
using System.Net;

namespace ViralMusicAPI.Handler
{
    public class ResponseBuilderHandler
    {
        public static ResponseDTO<T> generateResponse<T>(string message, HttpStatusCode httpStatusCode, T? objectResponse)
        {
            return new ResponseDTO<T>(message, httpStatusCode, objectResponse);
        }
    }
}
