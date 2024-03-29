﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	public class AuthorizationService
	{
		public const string headerKey = "x-api-key";

		private string secret;

		public AuthorizationService(IConfiguration config, ILogger<AuthorizationService> logger) {
			secret = config["jwt_secret"];

			if (string.IsNullOrEmpty(secret)) {
				logger.LogWarning("No configuration found for 'jwt_secret'. Generating a random one for this instance.");
				HMACSHA256 hmac = new HMACSHA256();
				secret = Convert.ToBase64String(hmac.Key);
			}
		}

		/**
		 * <summary>
		 * Generates a valid token that includes the user's username and expiration time.
		 * </summary>
		 */
		public string GenerateToken (string username, UserRole role) {
			byte[] key = Convert.FromBase64String(secret);
			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
			SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor {
				Subject = new ClaimsIdentity(new[] {
					new Claim(ClaimTypes.Name, username.ToString()),
					new Claim(ClaimTypes.Role, role.ToString())
				}),
				Expires = DateTime.UtcNow.AddHours(24),
				SigningCredentials = new SigningCredentials(securityKey,
					SecurityAlgorithms.HmacSha256Signature)
			};

			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
			return handler.WriteToken(token);
		}

		/**
		 * <summary>
		 * Gets a <see cref="ClaimsPrincipal"/> from a token. If the token is not valid, returns null;
		 * </summary>
		 */
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
	}
}
