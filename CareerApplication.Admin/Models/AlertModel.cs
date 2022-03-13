using MudBlazor;

namespace CareerApplication.Admin.Models
{
    public class AlertModel
    {
        public bool IsVisible { get; set; }
        public string Message { get; set; }
        public Severity AlertType { get; set; }
    }
}
