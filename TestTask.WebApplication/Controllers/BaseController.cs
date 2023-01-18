using Microsoft.AspNetCore.Mvc;
using static TestTask.WebApplication.Controllers.Enum;

namespace TestTask.WebApplication.Controllers
{
    public class Enum
    {
        public enum NotificationType
        {
            error,
            success,
            warning,
            info
        }

    }
    public abstract class BaseController : Controller
    {
        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "swal('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "";
            TempData["notification"] = msg;
        }
    }
}
