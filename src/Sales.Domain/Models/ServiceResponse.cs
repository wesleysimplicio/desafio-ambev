namespace Sales.Domain.Models
{
    public class ServicesResponse<T> where T : class
    {
        public ServicesResponse(bool success, T objecto, string message = null)
        {
            Success = success;
            Object = objecto;
            Message = message;
        }
        public bool Success { get; set; }
        public T Object { get; set; }
        public string Message { get; set; }
    }
}
