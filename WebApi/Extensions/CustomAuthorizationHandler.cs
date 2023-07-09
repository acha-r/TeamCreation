using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Extensions
{

    public class BearerRequirementFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;
        public BearerRequirementFilter(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headers = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"];
            if (!headers.Any() || headers.FirstOrDefault() != "Bearer SkFabTZibXE1aE14ckpQUUxHc2dnQ2RzdlFRTTM2NFE2cGI4d3RQNjZmdEFITmdBQkE=")
            {
                context.Result = new UnauthorizedObjectResult(string.Empty);
                return;
            }
        }
    }
    public class TokenRequirementAttribute : TypeFilterAttribute
    {
        public TokenRequirementAttribute() : base(typeof(BearerRequirementFilter))
        {
        }
    }
}
