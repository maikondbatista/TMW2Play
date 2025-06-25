using Microsoft.AspNetCore.Mvc;
using System.Net;
using TMW2Play.Service.Domain.Services;

namespace TMW2Play.Api.Controllers.Base
{
    public class ApiController(INotificationService notificationService) : ControllerBase
    {

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ResponseAsync(object result)
        {
            if (Valid())
            {
                return await ReturnResult(HttpStatusCode.OK, result);
            }
            else
            {
                return await ReturnResult(HttpStatusCode.BadRequest, notificationService.Notifications());
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ResponseAsync()
        {
            if (Valid())
            {
                return await ReturnResult(HttpStatusCode.NoContent);
            }
            else
            {
                return await ReturnResult(HttpStatusCode.BadRequest, notificationService.Notifications());
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<IActionResult> ReturnResult(HttpStatusCode status, object result)
        {
            return (status) switch
            {
                HttpStatusCode.NotFound => NotFound(result),

                HttpStatusCode.Conflict => Conflict(result),

                HttpStatusCode.NoContent => NoContent(),

                HttpStatusCode.OK => Ok(result),

                HttpStatusCode.Unauthorized => Unauthorized(result),

                _ => BadRequest(result),
            };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<IActionResult> ReturnResult(HttpStatusCode status)
        {
            return NoContent();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public bool Valid()
        {
            if (notificationService.Notifications().Count > 0)
                return false;

            return true;
        }
    }
}
