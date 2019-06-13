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

	/**
	 * <summary>
	 * Marks a Method as being only accessible with a valid api key. The user needs to send their api key in the header 'x-api-key' value.
	 * </summary>
	 */
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class JwtAuthroizeAttribute : AuthorizeAttribute, IAuthorizationFilter
	{
		
		public void OnAuthorization (AuthorizationFilterContext context) {
			AuthorizationService authorization = context.HttpContext.RequestServices.GetService(typeof(AuthorizationService)) as AuthorizationService;

			if (!context.HttpContext.Request.Headers.TryGetValue(AuthorizationService.headerKey, out StringValues values)) {
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