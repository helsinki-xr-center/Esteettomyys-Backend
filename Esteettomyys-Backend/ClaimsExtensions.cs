using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	public static class ClaimsExtensions
	{
		public static string GetAuthenticatedUsername(this ClaimsPrincipal principal) {
			if(principal.HasClaim(x => x.Type == ClaimTypes.Name)) {
				return principal.Identity.Name;
			}
			return null;
		}
	}
}
