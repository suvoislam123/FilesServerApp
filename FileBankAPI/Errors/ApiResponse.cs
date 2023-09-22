namespace FileBankAPI.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "You are a bad people. So you have made a bad request",
                401 => "You are not Authorized",
                404 => "Resource Not Found.",
                500 => "Server Errors",
                _ => null
            };
        }

    }
}
