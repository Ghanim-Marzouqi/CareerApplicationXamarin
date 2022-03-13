using System;
namespace CareerApplication.Admin.Models
{
    public class Error
    {
        public string Message { get; set; } = string.Empty;
    }

    public class ErrorModel
    {
        public Error Error { get; set; }
    }
}
