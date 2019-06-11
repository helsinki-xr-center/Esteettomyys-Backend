using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class JwtAuthroizeAttribute : AuthorizeAttribute, IAuthorizationFilter
	{
		
		public void OnAuthorization (AuthorizationFilterContext context) {
			AuthorizationService authorization = context.HttpContext.RequestServices.GetService(typeof(AuthorizationService)) as AuthorizationService;

			if (!context.HttpContext.Request.Headers.TryGetValue(AuthorizationService.apikey, out StringValues values)) {
				context.Result = new UnauthorizedResult();
				return;
			}

			var key = values.First(x => !string.IsNullOrEmpty(x));

			var principal = authorization.GetPrincipal(key);

			if(principal == null) {
				context.Result = new UnauthorizedResult();
				return;
			}

			context.HttpContext.User = principal;
		}
	}
}