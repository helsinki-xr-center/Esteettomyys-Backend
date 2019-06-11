using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	public class AuthorizationService
	{
		public const string apikey = "x-api-key";
		public const string claimName = ClaimTypes.Name;

		private string secret;

		public AuthorizationService(IConfiguration config) {
			secret = config["jwt_secret"];
		}

		public string GenerateToken (string username) {
			byte[] key = Convert.FromBase64String(secret);
			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
			SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor {
				Subject = new ClaimsIdentity(new[] {
					new Claim(claimName, username.ToString())}),
				Expires = DateTime.UtcNow.AddHours(24),
				SigningCredentials = new SigningCredentials(securityKey,
					SecurityAlgorithms.HmacSha256Signature)
			};

			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
			return handler.WriteToken(token);
		}

		public ClaimsPrincipal GetPrincipal (string token) {
			try {
				JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
				if (jwtToken == null)
					return null;
				byte[] key = Convert.FromBase64String(secret);
				TokenValidationParameters parameters = new TokenValidationParameters() {
					RequireExpirationTime = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = new SymmetricSecurityKey(key)
				};
				SecurityToken securityToken;
				ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
					parameters, out securityToken);
				return principal;
			} catch (Exception e) {
				return null;
			}
		}

		public string GetUsernameFromTokenIfValid (string token) {
			string username = null;
			ClaimsPrincipal principal = GetPrincipal(token);
			if (principal == null)
				return null;
			ClaimsIdentity identity = null;
			try {
				identity = (ClaimsIdentity)principal.Identity;
			} catch (NullReferenceException) {
				return null;
			}
			Claim idClaim = identity.FindFirst(claimName);
			username = idClaim.Value;
			return username;
		}

	}
}
