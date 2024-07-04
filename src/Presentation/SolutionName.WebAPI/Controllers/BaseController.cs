using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SolutionName.Application.Attributes;
using SolutionName.Application.Constants;
using SolutionName.Application.DTOs.Account.Authentication.User;
using SolutionName.Application.Enums;
using SolutionName.Application.Models.Shared;
using SolutionName.Application.Utilities.Utility;
using System.Reflection;

namespace SolutionName.WebAPI.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]

    public class BaseController : ControllerBase
    {
        [HttpGet("Test")]
        public ActionResult Test()
        {
            return Ok("Worked !");
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, RoleName = "Admin")]
        public IActionResult GetAuthorizeDefinitionEndpoints()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            List<AuthDefinitionModel> enpoints = new();
            if (controllers != null)
                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));
                    if (actions != null)
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);
                            if (attributes != null)
                            {
                                AuthDefinitionModel point = null;

                                var authorizeDefinitionAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                                //Your logic here...
                            }
                        }
                }


            return Ok(enpoints);
        }

        #region helper Methods
        // Get source IP address for the current request
        protected string IpAddress()
        {
            if (Request.Headers.ContainsKey(ApiHeaderKeysConst.XForwardedFor))
                return Request.Headers[ApiHeaderKeysConst.XForwardedFor];
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private string GetHeaderValue(IHttpContextAccessor httpContextAccessor, string headerKey)
        {
            if (httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.Request.Headers.TryGetValue(headerKey, out StringValues headerValues))
            {
                return headerValues.First();
            }

            return string.Empty;
        }

        #region Cookie operations
        // Set cookie with a given name, value, and expiration date
        protected void SetCookie(string? cookieName, string? cookieValue)
        {
            var cookieOptions = CookieOptions();
            if (!string.IsNullOrEmpty(cookieName) && !string.IsNullOrEmpty(cookieValue))
                Response.Cookies.Append(cookieName, cookieValue, cookieOptions);
        }

        // Set refresh token cookie
        protected void SetRefreshTokenCookie(UserDto dto, long? expiration)
        {
            var cookieOptions = CookieOptions();
            Response.Cookies.Append(ApiHeaderKeysConst.RefreshToken, dto.RefreshToken, cookieOptions);
        }

        // Returns the cookie options with HttpOnly set to true, expiration set to one day from now, and Secure set based on the environment
        protected CookieOptions CookieOptions()
        {
            var env = Environment.GetEnvironmentVariable(ApiHeaderKeysConst.ASPNETCORE_ENVIRONMENT);
            var isProd = env == ApiHeaderKeysConst.Production;
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(1),
                Secure = isProd
            };
            return cookieOptions;
        }

        // Delete all cookies
        protected void DeleteCookies()
        {
            var env = Environment.GetEnvironmentVariable(ApiHeaderKeysConst.ASPNETCORE_ENVIRONMENT);
            var isProd = env == ApiHeaderKeysConst.Production;
            foreach (var cookie in HttpContext.Request.Cookies)
                Response.Cookies.Delete(cookie.Key, new CookieOptions
                {
                    Secure = isProd
                });
        }
        #endregion

        #region Cryption operations
        protected string Encrypt(int? id)
        {
            if (id == null)
                id = 0;

            return TextEncryption.Encrypt(id.ToString());
        }

        protected int Decrypt(string id)
        {
            if (id is null)

                return 0;

            return Convert.ToInt32(TextEncryption.Decrypt(id.ToString()));
        }
        #endregion
        #endregion

    }

}
